﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// Imports a graph model from the ECore format.
    /// </summary>
    class ECoreImport
    {
        class RefType
        {
            public bool Ordered;
            public String TypeName;

            public RefType(bool ordered, String typeName)
            {
                Ordered = ordered;
                TypeName = typeName;
            }
        }

        class NodeType
        {
            public List<String> SuperTypes = new List<String>();
            public Dictionary<String, RefType> RefAttrToRefType = new Dictionary<String, RefType>();
        }

        IGraph graph;
        Dictionary<String, NodeType> typeMap = new Dictionary<String, NodeType>();
        Dictionary<String, INode> nodeMap = new Dictionary<String, INode>();
        Dictionary<String, Dictionary<String, int>> enumToLiteralToValue = new Dictionary<String, Dictionary<String, int>>();

        /// <summary>
        /// Creates a new graph from the given ECore metamodels.
        /// If a grg file is given, the graph will use the graph model declared in it and the according
        /// actions object will be associated to the graph.
        /// If a xmi file is given, the model instance will be imported into the graph.
        /// Any errors will be reported by exception.
        /// </summary>
        /// <param name="backend">The backend to use to create the graph.</param>
        /// <param name="ecoreFilenames">A list of ECore model specification files. It must at least contain one element.</param>
        /// <param name="grgFilename">A grg file to be used to create the graph, or null.</param>
        /// <param name="xmiFilename">The filename of the model instance to be imported, or null.</param>
        public static IGraph Import(IBackend backend, List<String> ecoreFilenames, String grgFilename, String xmiFilename)
        {
            ECoreImport imported = new ECoreImport();
            imported.graph = imported.ImportModels(ecoreFilenames, grgFilename, backend);
            if(xmiFilename != null)
            {
                Console.WriteLine("Importing graph...");
                imported.ImportGraph(xmiFilename);
            }
            return imported.graph;
        }

        private IGraph ImportModels(List<String> ecoreFilenames, String grgFilename, IBackend backend)
        {
            foreach(String ecoreFilename in ecoreFilenames)
            {
                String modelText = ParseModel(ecoreFilename);

                String modelfilename = ecoreFilename.Substring(0, ecoreFilename.LastIndexOf('.')) + "__ecore.gm";

                // Do we have to update the model file (.gm)?
                if(!File.Exists(modelfilename) || File.GetLastWriteTime(ecoreFilename) > File.GetLastWriteTime(modelfilename))
                {
                    Console.WriteLine("Writing model file \"" + modelfilename + "\"...");
                    using(StreamWriter writer = new StreamWriter(modelfilename))
                        writer.Write(modelText);
                }
            }

            if(grgFilename == null)
            {
                grgFilename = "";
                foreach(String ecoreFilename in ecoreFilenames)
                    grgFilename += ecoreFilename.Substring(0, ecoreFilename.LastIndexOf('.')) + "_";
                grgFilename += "_ecore.grg";

                StringBuilder sb = new StringBuilder();
                sb.Append("// Automatically generated\n// Do not change, changes will be lost!\n\nusing ");

                DateTime grgTime;
                if(!File.Exists(grgFilename)) grgTime = DateTime.MinValue;
                else grgTime = File.GetLastWriteTime(grgFilename);

                bool mustWriteGrg = false;

                bool first = true;
                foreach(String ecore in ecoreFilenames)
                {
                    if(first) first = false;
                    else sb.Append(", ");
                    sb.Append(ecore.Substring(0, ecore.LastIndexOf('.')) + "__ecore");

                    if(File.GetLastWriteTime(ecore) > grgTime)
                        mustWriteGrg = true;
                }

                if(mustWriteGrg)
                {
                    sb.Append(";\n");
                    using(StreamWriter writer = new StreamWriter(grgFilename))
                        writer.Write(sb.ToString());
                }
            }

            IGraph graph;
            BaseActions actions;
            backend.CreateFromSpec(grgFilename, "defaultname", out graph, out actions);
            graph.Actions = actions;
            return graph;
        }

        private String ParseModel(String ecoreFilename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ecoreFilename);

            XmlElement xmielem = doc["xmi:XMI"];
            if(xmielem == null)
                throw new Exception("The document has no xmi:XMI element.");

            StringBuilder sb = new StringBuilder();
            sb.Append("// Automatically generated from \"" + ecoreFilename + "\"\n// Do not change, changes will be lost!\n\n");

            // Parse the ECore file, create the model file content, and collect important infos used to import a model instance
            foreach(XmlElement package in xmielem.GetElementsByTagName("ecore:EPackage"))
            {
                String packageName = package.GetAttribute("nsPrefix");
                foreach(XmlElement classifier in package.GetElementsByTagName("eClassifiers"))
                {
                    String classifierType = classifier.GetAttribute("xsi:type");
                    String classifierName = classifier.GetAttribute("name");

                    switch(classifierType)
                    {
                        case "ecore:EClass":
                            ParseEClass(sb, xmielem, packageName, classifier, classifierName);
                            break;
                        case "ecore:EEnum":
                            ParseEEnum(sb, xmielem, packageName, classifier, classifierName);
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        private String GetGrGenTypeName(String xmitypename, XmlElement xmielem, String packageDelim)
        {
            // xmitypename has the syntax "#/<number>/<typename>"
            int lastSlashPos = xmitypename.LastIndexOf('/');

            int rootIndex = int.Parse(xmitypename.Substring(2, lastSlashPos - 2));

            // Remove "#/<number>/" from begining of xmitypename
            xmitypename = xmitypename.Substring(lastSlashPos + 1);

            // Handle some GrGen primitive types
            // All others are considered as normal types
            switch(xmitypename)
            {
                case "String": xmitypename = "string"; break;
                case "Boolean": xmitypename = "boolean"; break;
                case "Integer": xmitypename = "int"; break;
                default:
                {
                    XmlElement packageNode = (XmlElement) xmielem.ChildNodes[rootIndex];
                    String packageName = packageNode.GetAttribute("nsPrefix");
                    xmitypename = packageName + packageDelim + xmitypename;
                    break;
                }
            }
            return xmitypename;
        }

        private String GetGrGenTypeName(String xmitypename, XmlElement xmielem)
        {
            return GetGrGenTypeName(xmitypename, xmielem, "_");
        }

        private void ParseEClass(StringBuilder sb, XmlElement xmielem, String packageName, XmlElement classifier, String classifierName)
        {
            bool first;
            NodeType nodeType = new NodeType();

            String abstractStr = classifier.GetAttribute("abstract");
            if(abstractStr == "true")
                sb.Append("abstract ");
            sb.Append("node class " + packageName.Replace('.', '_') + "_" + classifierName);

            typeMap[packageName + ":" + classifierName] = nodeType;

            String superTypePathsStr = classifier.GetAttribute("eSuperTypes");
            if(superTypePathsStr != "")
            {
                sb.Append(" extends ");

                String[] superTypePaths = superTypePathsStr.Split(' ');
                first = true;
                foreach(String superType in superTypePaths)
                {
                    if(first) first = false;
                    else sb.Append(", ");

                    String name = GetGrGenTypeName(superType, xmielem, ":");
                    nodeType.SuperTypes.Add(name);
                    sb.Append(name.Replace(':', '_').Replace('.', '_'));
                }
            }

            // First iterate over all ecore:EAttribute structural features
            first = true;
            foreach(XmlElement item in classifier.GetElementsByTagName("eStructuralFeatures"))
            {
                String itemType = item.GetAttribute("xsi:type");
                if(itemType == "ecore:EAttribute")
                {
                    if(first)
                    {
                        sb.Append(" {\n");
                        first = false;
                    }

                    String attrName = item.GetAttribute("name");
                    String attrType = item.GetAttribute("eType");
                    attrType = GetGrGenTypeName(attrType, xmielem);

                    sb.Append("\t_" + attrName + ":" + attrType + ";\n");
                }
            }
            if(first) sb.Append(";\n\n");
            else sb.Append("}\n\n");

            // Then iterate over all ecore:EReference structural features modelled as edge types
            foreach(XmlElement item in classifier.GetElementsByTagName("eStructuralFeatures"))
            {
                String itemType = item.GetAttribute("xsi:type");
                if(itemType == "ecore:EReference")
                {
                    String refName = item.GetAttribute("name");
                    String refType = item.GetAttribute("eType");

                    String edgeTypeName = packageName + "_" + classifierName + "_" + refName;
                    String typeName = GetGrGenTypeName(refType, xmielem, ":");
                    bool ordered = item.GetAttribute("ordered") != "false";          // Default is ordered

                    sb.Append("edge class " + edgeTypeName.Replace('.', '_'));
                    if(ordered)
                        sb.Append(" {\n\tindex:int;\n}\n\n");
                    else
                        sb.Append(";\n\n");

                    nodeType.RefAttrToRefType[refName] = new RefType(ordered, typeName);
                }
            }
        }

        private void ParseEEnum(StringBuilder sb, XmlElement xmielem, String packageName, XmlElement classifier, String classifierName)
        {
            Dictionary<String, int> literalToValue = new Dictionary<String, int>();

            String enumTypeName = packageName + "_" + classifierName;

            sb.Append("enum " + enumTypeName.Replace('.', '_') + " {\n");
            bool first = true;
            foreach(XmlElement item in classifier.GetElementsByTagName("eLiterals"))
            {
                if(first) first = false;
                else sb.Append(",\n");

                String name = item.GetAttribute("name");
                String value = item.GetAttribute("value");

                sb.Append("\t_" + name + " = " + value);

                int val = int.Parse(value);
                literalToValue[name] = val;

                String literal = item.GetAttribute("literal");
                if(literal != "") literalToValue[literal] = val;
            }
            sb.Append("\n}\n\n");

            enumToLiteralToValue[enumTypeName] = literalToValue;
        }

        private void ImportGraph(String importFilename)
        {
            // First pass: build the nodes and the parent edges given by nesting
            using(XmlTextReader reader = new XmlTextReader(importFilename))
            {
                while(reader.Read())
                {
                    if(reader.NodeType != XmlNodeType.Element) continue;

                    String tagName = reader.Name;
                    String grgenTypeName = tagName.Replace(':', '_').Replace(':', '_');
                    INode gnode = graph.AddNode(graph.Model.NodeModel.GetType(grgenTypeName));

                    String id = null;
                    if(reader.MoveToAttribute("xmi:id"))
                        id = reader.Value;
                    nodeMap[id] = gnode;

                    ParseNodeFirstPass(reader, gnode, tagName);
                }
            }

            // Second pass: assign the attributes and edges given by reference attributes
            using(XmlTextReader reader = new XmlTextReader(importFilename))
            {
                while(reader.Read())
                {
                    if(reader.NodeType != XmlNodeType.Element) continue;

                    ParseNodeSecondPass(reader, reader.Name, reader.IsEmptyElement);
                }
            }
        }

        private RefType FindRefType(String parentTypeName, String tagName)
        {
            NodeType nodeType = typeMap[parentTypeName];

            RefType refType;
            if(nodeType.RefAttrToRefType.TryGetValue(tagName, out refType)) return refType;

            foreach(String superType in nodeType.SuperTypes)
            {
                refType = FindRefType(superType, tagName);
                if(refType != null) return refType;
            }
            return null;
        }

        private String FindRefTypeName(String parentTypeName, String tagName)
        {
            RefType refType = FindRefType(parentTypeName, tagName);
            if(refType == null) return null;
            else return refType.TypeName;
        }

        private bool IsRefOrdered(String parentTypeName, String tagName)
        {
            return FindRefType(parentTypeName, tagName).Ordered;
        }

        private String FindContainingTypeName(String parentTypeName, String tagName)
        {
            NodeType nodeType = typeMap[parentTypeName];

            if(nodeType.RefAttrToRefType.ContainsKey(tagName)) return parentTypeName;

            foreach(String superType in nodeType.SuperTypes)
            {
                String containingType = FindContainingTypeName(superType, tagName);
                if(containingType != null) return containingType;
            }
            return null;
        }

        private void ParseNodeFirstPass(XmlTextReader reader, INode parentNode, String parentTypeName)
        {
            INodeModel nodeModel = graph.Model.NodeModel;
            IEdgeModel edgeModel = graph.Model.EdgeModel;

            Dictionary<String, int> tagNameToNextIndex = new Dictionary<String, int>();

            while(reader.Read())
            {
                if(reader.NodeType == XmlNodeType.EndElement) break;
                if(reader.NodeType != XmlNodeType.Element) continue;

                bool emptyElem = reader.IsEmptyElement;
                String tagName = reader.Name;
                String id = null;
                if(reader.MoveToAttribute("xmi:id"))
                    id = reader.Value;

                String typeName = null;
                if(reader.MoveToAttribute("xsi:type"))
                    typeName = reader.Value;
                else
                {
                    String qualAttr = parentTypeName + "." + tagName;
                    typeName = FindRefTypeName(parentTypeName, tagName);

                    if(typeName == null)
                    {
                        // Treat it as an attribute
                        AssignAttribute(parentNode, tagName, reader.ReadInnerXml());
                        continue;
                    }
                }
                String grgenTypeName = typeName.Replace(':', '_').Replace('.', '_');
                INode gnode = graph.AddNode(nodeModel.GetType(grgenTypeName));
                nodeMap[id] = gnode;

                String edgeTypeName = FindContainingTypeName(parentTypeName, tagName);
                String grgenEdgeTypeName = edgeTypeName.Replace(':', '_').Replace('.', '_') + "_" + tagName;
                IEdge parentEdge = graph.AddEdge(edgeModel.GetType(grgenEdgeTypeName), parentNode, gnode);
                if(IsRefOrdered(parentTypeName, tagName))
                {
                    int nextIndex;
                    tagNameToNextIndex.TryGetValue(tagName, out nextIndex);
                    parentEdge.SetAttribute("index", nextIndex);
                    tagNameToNextIndex[tagName] = nextIndex + 1;
                }

                if(!emptyElem)
                    ParseNodeFirstPass(reader, gnode, typeName);
            }
        }

        private void ParseNodeSecondPass(XmlTextReader reader, String curTypeName, bool isEmpty)
        {
            if(!reader.MoveToAttribute("xmi:id"))
            {
                // probably an attribute in element form (already handled), so ignore
                reader.Skip();
                return;
            }

            String id = reader.Value;
            INode curNode = nodeMap[id];

            for(int attrIndex = 0; attrIndex < reader.AttributeCount; attrIndex++)
            {
                reader.MoveToAttribute(attrIndex);

                String name = reader.Name;
                if(name.StartsWith("xmi:") || name == "xsi:type" || name.StartsWith("xmlns:")) continue;

                String attrRefType = FindRefTypeName(curTypeName, name);
                if(attrRefType != null)
                {
                    // List of references as in attribute separated by spaces

                    String refEdgeTypeName = FindContainingTypeName(curTypeName, name);
                    String grgenRefEdgeTypeName = refEdgeTypeName.Replace(':', '_').Replace('.', '_') + "_" + name;

                    IEdgeModel edgeModel = graph.Model.EdgeModel;
                    String[] destNodeNames = reader.Value.Split(' ');
                    if(IsRefOrdered(curTypeName, name))
                    {
                        int i = 0;
                        foreach(String destNodeName in destNodeNames)
                        {
                            IEdge parentEdge = graph.AddEdge(edgeModel.GetType(grgenRefEdgeTypeName), curNode, nodeMap[destNodeName]);
                            parentEdge.SetAttribute("index", i);
                            i++;
                        }
                    }
                    else
                    {
                        foreach(String destNodeName in destNodeNames)
                            graph.AddEdge(edgeModel.GetType(grgenRefEdgeTypeName), curNode, nodeMap[destNodeName]);
                    }
                }
                else
                {
                    AssignAttribute(curNode, name, reader.Value);
                }
            }

            if(isEmpty) return;

            while(reader.Read())
            {
                if(reader.NodeType == XmlNodeType.EndElement) break;
                if(reader.NodeType != XmlNodeType.Element) continue;

                bool emptyElem = reader.IsEmptyElement;
                String tagName = reader.Name;
                String typeName;
                if(reader.MoveToAttribute("xsi:type"))
                    typeName = reader.Value;
                else
                {
                    String qualAttr = curTypeName + "." + tagName;
                    typeName = FindRefTypeName(curTypeName, tagName);
                }

                ParseNodeSecondPass(reader, typeName, emptyElem);
            }
        }

        private void AssignAttribute(INode node, String attrname, String attrval)
        {
            AttributeType attrType = node.Type.GetAttributeType("_" + attrname);

            object value = null;
            switch(attrType.Kind)
            {
                case AttributeKind.BooleanAttr:
                    if(attrval.Equals("true", StringComparison.OrdinalIgnoreCase))
                        value = true;
                    else if(attrval.Equals("false", StringComparison.OrdinalIgnoreCase))
                        value = false;
                    else
                        throw new Exception("Attribute \"" + attrname + "\" must be either \"true\" or \"false\"!");
                    break;

                case AttributeKind.EnumAttr:
                {
                    int val;
                    if(Int32.TryParse(attrval, out val)) value = val;
                    else value = enumToLiteralToValue[attrType.EnumType.Name][attrval];
                    break;
                }

                case AttributeKind.IntegerAttr:
                {
                    int val;
                    if(!Int32.TryParse(attrval, out val))
                        throw new Exception("Attribute \"" + attrname + "\" must be an integer!");
                    value = val;
                    break;
                }

                case AttributeKind.StringAttr:
                    value = attrval;
                    break;

                case AttributeKind.FloatAttr:
                {
                    float val;
                    if(!Single.TryParse(attrval, out val))
                        throw new Exception("Attribute \"" + attrname + "\" must be a floating point number!");
                    value = val;
                    break;
                }

                case AttributeKind.DoubleAttr:
                {
                    double val;
                    if(!Double.TryParse(attrval, out val))
                        throw new Exception("Attribute \"" + attrname + "\" must be a floating point number!");
                    value = val;
                    break;
                }

                case AttributeKind.ObjectAttr:
                    throw new Exception("Attribute \"" + attrname + "\" is an object type attribute!\n"
                        + "It is not possible to assign a value to an object type attribute!");

                case AttributeKind.SetAttr:
                case AttributeKind.MapAttr:
                default:
                    throw new Exception("Unsupported attribute value type: \"" + attrType.Kind + "\"");
            }

            node.SetAttribute("_" + attrname, value);
        }
    }
}