//#define ELEMENTKNOWSVARIABLES
#define MONO_MULTIDIMARRAY_WORKAROUND       // not using multidimensional arrays is about 2% faster on .NET because of fewer bound checks
//#define OPCOST_WITH_GEO_MEAN

using System;
using System.Collections.Generic;
using de.unika.ipd.grGen.libGr;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace de.unika.ipd.grGen.lgsp
{
    public interface IUndoItem
    {
        void DoUndo(IGraph graph);

        IUndoItem Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap);
    }
    
    public class LGSPUndoElemAdded : IUndoItem
    {
        private IGraphElement _elem;

        public LGSPUndoElemAdded(IGraphElement elem) { _elem = elem; }

        public void DoUndo(IGraph graph)
        {
            if(_elem is INode) graph.Remove((INode) _elem);
            else graph.Remove((IEdge) _elem);
        }

        public IUndoItem Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap)
        {
            return new LGSPUndoElemAdded(oldToNewMap[_elem]);
        }
    }

    public class LGSPUndoElemRemoved : IUndoItem
    {
        private IGraphElement _elem;
        private String _name;
        private LinkedList<Variable> _vars;

        public LGSPUndoElemRemoved(IGraphElement elem, IGraph graph)
        {
            _elem = elem;
            _vars = graph.GetElementVariables(_elem);
            if(graph is NamedGraph) _name = ((NamedGraph) graph).GetElementName(_elem);
            else _name = null;
        }

        private LGSPUndoElemRemoved(IGraphElement elem, String name, LinkedList<Variable> vars)
        {
            _elem = elem;
            _name = name;
            _vars = vars;
        }

        public void DoUndo(IGraph graph)
        {
            if(_elem is LGSPNode) ((LGSPGraph) graph).AddNodeWithEvent((LGSPNode) _elem);
            else ((LGSPGraph) graph).AddEdgeWithEvent((LGSPEdge) _elem);

            if(graph is NamedGraph)
                ((NamedGraph) graph).SetElementName(_elem, _name);

            if(_vars != null)
            {
                foreach(Variable var in _vars)
                    graph.SetVariableValue(var.Name, _elem);
            }
        }

        public IUndoItem Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap)
        {
            LinkedList<Variable> newVars = new LinkedList<Variable>();
            foreach(Variable var in _vars)
            {
                newVars.AddLast(new Variable(var.Name, oldToNewMap[var.Element]));
            }
            return new LGSPUndoElemRemoved(oldToNewMap[_elem], _name, newVars);
        }
    }

    public class LGSPUndoAttributeChanged : IUndoItem
    {
        private IGraphElement _elem;
        private String _attrName;
        private Object _oldValue;

        public LGSPUndoAttributeChanged(IGraphElement elem, String attrName, Object oldValue)
        {
            _elem = elem; _attrName = attrName; _oldValue = oldValue;
        }

        // TODO: Perhaps replace reflection stuff by custom emitted accessor functions stored in a Dictionary in the type objects
        public void DoUndo(IGraph graph)
        {
            IAttributes attributes;
            PropertyInfo prop;
            LGSPNode node = _elem as LGSPNode;
            if(node != null)
            {
                attributes = node.attributes;
                prop = attributes.GetType().GetProperty(_attrName);
                graph.ChangingNodeAttribute(node, node.type.GetAttributeType(_attrName), prop.GetValue(attributes, null), _oldValue);
            }
            else
            {
                LGSPEdge edge = (LGSPEdge) _elem;
                attributes = edge.attributes;
                prop = attributes.GetType().GetProperty(_attrName);
                graph.ChangingEdgeAttribute(edge, edge.type.GetAttributeType(_attrName), prop.GetValue(attributes, null), _oldValue);
            }
            prop.SetValue(attributes, _oldValue, null);
        }

        public IUndoItem Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap)
        {
            return new LGSPUndoAttributeChanged(oldToNewMap[_elem], _attrName, _oldValue);
        }
    }

    public class LGSPUndoElemTypeChanged : IUndoItem
    {
        private IGraphElement _elem;
        private IType _oldType;
        private IAttributes _oldAttrs;

        public LGSPUndoElemTypeChanged(IGraphElement elem, IType oldType, IAttributes oldAttrs)
        {
            _elem = elem;
            _oldType = oldType;
            _oldAttrs = oldAttrs;
        }

        public void DoUndo(IGraph graph)
        {
            LGSPGraph lgraph = (LGSPGraph) graph;
            if(_elem is LGSPNode)
            {
                LGSPNode node = (LGSPNode) _elem;
                lgraph.RemoveNode(node, node.type.typeID);
                lgraph.AddNode(node, ((ITypeFramework) _oldType).typeID);
                node.type = (ITypeFramework) _oldType;
                node.attributes = _oldAttrs;
            }
            else
            {
                LGSPEdge edge = (LGSPEdge) _elem;
                lgraph.RemoveEdge(edge, edge.type.typeID);
                lgraph.AddEdge(edge, ((ITypeFramework) _oldType).typeID);
                edge.type = (ITypeFramework) _oldType;
                edge.attributes = _oldAttrs;
            }
        }

        public IUndoItem Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap)
        {
            return new LGSPUndoElemTypeChanged(oldToNewMap[_elem], _oldType, _oldAttrs == null ? null : (IAttributes) _oldAttrs.Clone());
        }
    }

    public class LGSPTransactionManager : ITransactionManager
    {
        private LinkedList<IUndoItem> undoItems = new LinkedList<IUndoItem>();
        private bool recording = false;
        private bool undoing = false;
        private LGSPGraph graph;

        public LGSPTransactionManager(LGSPGraph lgspgraph)
        {
            graph = lgspgraph;
        }

        private void SubscribeEvents()
        {
            graph.OnNodeAdded += new NodeAddedHandler(ElementAdded);
            graph.OnEdgeAdded += new EdgeAddedHandler(ElementAdded);
            graph.OnRemovingNode += new RemovingNodeHandler(RemovingElement);
            graph.OnRemovingEdge += new RemovingEdgeHandler(RemovingElement);
            graph.OnChangingNodeAttribute += new ChangingNodeAttributeHandler(ChangingElementAttribute);
            graph.OnChangingEdgeAttribute += new ChangingEdgeAttributeHandler(ChangingElementAttribute);
            graph.OnSettingNodeType += new SettingNodeTypeHandler(SettingElementType);
            graph.OnSettingEdgeType += new SettingEdgeTypeHandler(SettingElementType);
        }

        private void UnsubscribeEvents()
        {
            graph.OnNodeAdded -= new NodeAddedHandler(ElementAdded);
            graph.OnEdgeAdded -= new EdgeAddedHandler(ElementAdded);
            graph.OnRemovingNode -= new RemovingNodeHandler(RemovingElement);
            graph.OnRemovingEdge -= new RemovingEdgeHandler(RemovingElement);
            graph.OnChangingNodeAttribute -= new ChangingNodeAttributeHandler(ChangingElementAttribute);
            graph.OnChangingEdgeAttribute -= new ChangingEdgeAttributeHandler(ChangingElementAttribute);
            graph.OnSettingNodeType -= new SettingNodeTypeHandler(SettingElementType);
            graph.OnSettingEdgeType -= new SettingEdgeTypeHandler(SettingElementType);
        }

        /// <summary>
        /// Starts a transaction
        /// </summary>
        /// <returns>A transaction ID to be used with Commit or Rollback</returns>
        public int StartTransaction()
        {
            if(!recording)
            {
                recording = true;
                SubscribeEvents();
            }
            return undoItems.Count;
        }

        /// <summary>
        /// Removes the rollback data and stops this transaction
        /// </summary>
        /// <param name="transactionID">Transaction ID returned by a StartTransaction call</param>
        public void Commit(int transactionID)
        {
            if(transactionID == 0)
            {
                undoItems.Clear();
                recording = false;
                UnsubscribeEvents();
            }
            else
            {
                while(undoItems.Count > transactionID) undoItems.RemoveLast();
            }
        }

        /// <summary>
        /// Undoes all changes during a transaction
        /// </summary>
        /// <param name="transactionID">The ID of the transaction to be rollbacked</param>
        public void Rollback(int transactionID)
        {
            undoing = true;
            while(undoItems.Count > transactionID)
            {
                undoItems.Last.Value.DoUndo(graph);
                undoItems.RemoveLast();
            }
            undoing = false;
            if(transactionID == 0)
            {
                recording = false;
                UnsubscribeEvents();
            }
        }

        public void ElementAdded(IGraphElement elem)
        {
            if(recording && !undoing) undoItems.AddLast(new LGSPUndoElemAdded(elem));
        }

        public void RemovingElement(IGraphElement elem)
        {
            if(recording && !undoing) undoItems.AddLast(new LGSPUndoElemRemoved(elem, graph));
        }

        public void ChangingElementAttribute(IGraphElement elem, AttributeType attrType, Object oldValue, Object newValue)
        {
            if(recording && !undoing) undoItems.AddLast(new LGSPUndoAttributeChanged(elem, attrType.Name, oldValue));
        }

        public void SettingElementType(IGraphElement elem, IType oldType, IAttributes oldAttrs, IType newType, IAttributes newAttrs)
        {
            if(recording && !undoing) undoItems.AddLast(new LGSPUndoElemTypeChanged(elem, oldType, oldAttrs));
        }

        public bool TransactionActive { get { return recording; } }

/*        public ITransactionManager Clone(Dictionary<IGraphElement, IGraphElement> oldToNewMap)
        {
            LGSPTransactionManager newTM = new LGSPTransactionManager();
            newTM.recording = recording;
            newTM.undoing = undoing;                              
            foreach(IUndoItem undoItem in undoItems)
            {
                IUndoItem newUndoItem = undoItem.Clone(oldToNewMap);
                newTM.undoItems.AddLast(newUndoItem);
            }
        }*/
    }
    
    public enum LGSPDir { In, Out };

    public class LGSPGraph : BaseGraph
    {
        private static int actionID = 0;
        private static int graphID = 0;

        private String name;
        private LGSPBackend backend = null;
        private IGraphModel model;
        private String modelAssemblyName;

        private LGSPTransactionManager transactionManager;

        private bool reuseOptimization = true;

        /// <summary>
        /// If true (the default case), elements deleted during a rewrite
        /// may be reused in the same rewrite.
        /// As a result new elements may not be discriminable anymore from
        /// already deleted elements using object equality, hash maps, etc.
        /// In cases where this is needed this optimization should be disabled.
        /// </summary>
        public override bool ReuseOptimization
        {
            get { return reuseOptimization; }
            set { reuseOptimization = value; }
        }

#if MONO_MULTIDIMARRAY_WORKAROUND
        public int dim0size, dim1size, dim2size;  // dim3size is always 2
        public float[] vstructs;
#else
        public float[, , ,] vstructs;
#endif

        /// <summary>
        /// The number of compatible nodes in the graph for each type at the time of the last analysis.
        /// It is null, if no analysis has been executed, yet.
        /// </summary>
        public int[] nodeCounts;

        /// <summary>
        /// The number of compatible edges in the graph for each type at the time of the last analysis.
        /// It is null, if no analysis has been executed, yet.
        /// </summary>
        public int[] edgeCounts;

#if OPCOST_WITH_GEO_MEAN
        public float[] nodeLookupCosts, edgeLookupCosts;
#endif

        /// <summary>
        /// The mean out degree (independent of edge types) of the nodes of a graph for each node type
        /// at the time of the last analysis.
        /// It is null, if no analysis has been executed, yet.
        /// </summary>
        public float[] meanOutDegree;

        /// <summary>
        /// The mean in degree (independent of edge types) of the nodes of a graph for each node type
        /// at the time of the last analysis.
        /// It is null, if no analysis has been executed, yet.
        /// </summary>
        public float[] meanInDegree;
       
        public LGSPNode[] nodesByTypeHeads;
        public int[] nodesByTypeCounts;
        public LGSPEdge[] edgesByTypeHeads;
        public int[] edgesByTypeCounts;

        public LGSPGraph(IGraphModel grmodel) : this(grmodel, "lgspGraph_" + graphID++) { }

        public LGSPGraph(IGraphModel grmodel, String grname)
        {
            model = grmodel;
            name = grname;

            modelAssemblyName = Assembly.GetAssembly(grmodel.GetType()).Location;

            InitializeGraph();
        }

        public LGSPGraph(LGSPBackend lgspBackend, IGraphModel grmodel, String grname, String modelassemblyname)
            : this(grmodel, grname)
        {
            backend = lgspBackend;
            modelAssemblyName = modelassemblyname;
        }

        /// <summary>
        /// Copy constructor.
        /// Open transaction data are lost.
        /// </summary>
        /// <param name="dataSource">The LGSPGraph object to get the data from</param>
        /// <param name="newName">Name of the copied graph.</param>
        protected LGSPGraph(LGSPGraph dataSource, String newName)
        {
            model = dataSource.model;
            name = newName;

            InitializeGraph();

            if(dataSource.backend != null)
            {
                backend = dataSource.backend;
                modelAssemblyName = dataSource.modelAssemblyName;
            }

            Dictionary<IGraphElement, IGraphElement> oldToNewMap = new Dictionary<IGraphElement, IGraphElement>();

            for(int i = 0; i < dataSource.nodesByTypeHeads.Length; i++)
            {
                for(LGSPNode head = dataSource.nodesByTypeHeads[i], node = head.typePrev; node != head; node = node.typePrev)
                {
                    LGSPNode newNode = new LGSPNode(node.type);
                    newNode.attributes = node.attributes == null ? null : (IAttributes) node.attributes.Clone();
                    AddNode(newNode, node.type.typeID);
                    oldToNewMap[node] = newNode;
                }
            }

            for(int i = 0; i < dataSource.edgesByTypeHeads.Length; i++)
            {
                for(LGSPEdge head = dataSource.edgesByTypeHeads[i], edge = head.typePrev; edge != head; edge = edge.typePrev)
                {
                    LGSPEdge newEdge = (LGSPEdge) AddEdge(edge.type, (INode) oldToNewMap[edge.source], (INode) oldToNewMap[edge.target]);
                    newEdge.attributes = edge.attributes == null ? null : (IAttributes) edge.attributes.Clone();
                    oldToNewMap[edge] = newEdge;
                }
            }

            foreach(KeyValuePair<IGraphElement, LinkedList<Variable>> kvp in dataSource.ElementMap)
            {
                IGraphElement newElem = oldToNewMap[kvp.Key];
                foreach(Variable var in kvp.Value)
                    SetVariableValue(var.Name, newElem);
            }

#if MONO_MULTIDIMARRAY_WORKAROUND
            dim0size = dataSource.dim0size;
            dim1size = dataSource.dim1size;
            dim2size = dataSource.dim2size;
            if(dataSource.vstructs != null)
                vstructs = (float[]) dataSource.vstructs.Clone();
#else
            if(dataSource.vstructs != null)
                vstructs = (float[ , , , ]) dataSource.vstructs.Clone();
#endif
            if(dataSource.nodeCounts != null)
                nodeCounts = (int[]) dataSource.nodeCounts.Clone();
            if(dataSource.edgeCounts != null)
                edgeCounts = (int[]) dataSource.edgeCounts.Clone();
#if OPCOST_WITH_GEO_MEAN
            if(dataSource.nodeLookupCosts != null)
                nodeLookupCosts = (float[]) dataSource.nodeLookupCosts.Clone();
            if(dataSource.edgeLookupCosts != null)
                edgeLookupCosts = (float[]) dataSource.edgeLookupCosts.Clone();
#endif
            if(dataSource.meanInDegree != null)
                meanInDegree = (float[]) dataSource.meanInDegree.Clone();
            if(dataSource.meanOutDegree != null)
                meanOutDegree = (float[]) dataSource.meanOutDegree.Clone();
        }

        private void InitializeGraph()
        {
            transactionManager = new LGSPTransactionManager(this);

            nodesByTypeHeads = new LGSPNode[model.NodeModel.Types.Length];
            for(int i = 0; i < model.NodeModel.Types.Length; i++)
            {
                LGSPNode head = new LGSPNode();
                head.typeNext = head;
                head.typePrev = head;
                nodesByTypeHeads[i] = head;
            }
            nodesByTypeCounts = new int[model.NodeModel.Types.Length];
            edgesByTypeHeads = new LGSPEdge[model.EdgeModel.Types.Length];
            for(int i = 0; i < model.EdgeModel.Types.Length; i++)
            {
                LGSPEdge head = new LGSPEdge();
                head.typeNext = head;
                head.typePrev = head;
                edgesByTypeHeads[i] = head;
            }
            edgesByTypeCounts = new int[model.EdgeModel.Types.Length];

            // Reset variables
            ElementMap.Clear();
            VariableMap.Clear();

            // Reset statistical data
#if MONO_MULTIDIMARRAY_WORKAROUND
            dim0size = dim1size = dim2size = 0;
#endif
            vstructs = null;
            nodeCounts = null;
            edgeCounts = null;
#if OPCOST_WITH_GEO_MEAN
            nodeLookupCosts = null;
            edgeLookupCosts = null;
#endif
            meanInDegree = null;
            meanOutDegree = null;
        }

        public override String Name { get { return name; } }
        public override IGraphModel Model { get { return model; } }

        public override ITransactionManager TransactionManager { get { return transactionManager; } }

        /// <summary>
        /// For persistent backends permanently destroys the graph
        /// </summary>
        public override void DestroyGraph()
        {
        }

        /// <summary>
        /// Loads a LGSPActions implementation
        /// </summary>
        /// <param name="actionFilename">Filename of a action file. This can be either a library (.dll) or source code (.cs)</param>
        /// <returns>A LGSPActions object as BaseActions</returns>
        public override BaseActions LoadActions(String actionFilename, DumpInfo dumpInfo)
        {
            Assembly assembly;
            String assemblyName;

            String extension = Path.GetExtension(actionFilename);
            if(extension.Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                CSharpCodeProvider compiler = new CSharpCodeProvider();
                CompilerParameters compParams = new CompilerParameters();
                compParams.ReferencedAssemblies.Add("System.dll");
                compParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(IBackend)).Location);
                compParams.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(LGSPActions)).Location);
                compParams.ReferencedAssemblies.Add(modelAssemblyName);

//                compParams.GenerateInMemory = true;
                compParams.CompilerOptions = "/optimize";
                compParams.OutputAssembly = String.Format("lgsp-action-assembly-{0:00000}.dll", actionID++);

                CompilerResults compResults = compiler.CompileAssemblyFromFile(compParams, actionFilename);
                if(compResults.Errors.HasErrors)
                {
                    String errorMsg = compResults.Errors.Count + " Errors:";
                    foreach(CompilerError error in compResults.Errors)
                        errorMsg += String.Format("\r\nLine: {0} - {1}", error.Line, error.ErrorText);
                    throw new ArgumentException("Illegal actions C# source code: " + errorMsg);
                }
                
                assembly = compResults.CompiledAssembly;
                assemblyName = compParams.OutputAssembly;
            }
            else if(extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
            {
                assembly = Assembly.LoadFrom(actionFilename);
                assemblyName = actionFilename;
                if(backend != null) backend.AddAssembly(assembly);          // TODO: still needed??
            }
            else
            {
                throw new ArgumentException("The action filename must be either a .cs or a .dll filename!");
            }

            Type actionsType = null;
            try
            {
                foreach(Type type in assembly.GetTypes())
                {
                    if(!type.IsClass || type.IsNotPublic) continue;
                    if(type.BaseType == typeof(LGSPActions))
                    {
                        if(actionsType != null)
                        {
                            throw new ArgumentException(
                                "The given action file contains more than one LGSPActions implementation!");
                        }
                        actionsType = type;
                    }
                }
            }
            catch(ReflectionTypeLoadException e)
            {
                String errorMsg = "";
                foreach(Exception ex in e.LoaderExceptions)
                    errorMsg += "- " + ex.Message + Environment.NewLine;
                if(errorMsg.Length == 0) errorMsg = e.Message;
                throw new ArgumentException(errorMsg);
            }
            if(actionsType == null)
            {
                throw new ArgumentException("The given action file doesn't contain an LGSPActions implementation!");
            }

            if(dumpInfo == null) dumpInfo = new DumpInfo(GetElementName);

            LGSPActions actions = (LGSPActions) assembly.CreateInstance(
                actionsType.FullName, false, BindingFlags.CreateInstance, null,
                new Object[] { this, new VCGDumperFactory(VCGFlags.OrientTopToBottom, dumpInfo),
                    modelAssemblyName, assemblyName },
                null, null);

            if(Model.MD5Hash != actions.ModelMD5Hash)
                throw new ArgumentException("The given action file has been compiled with another model assembly!");

            return actions;
        }

        public override int GetNumExactNodes(IType nodeType)
        {
            return nodesByTypeCounts[((ITypeFramework) nodeType).TypeID];
        }

        public override int GetNumExactEdges(IType edgeType)
        {
            return edgesByTypeCounts[((ITypeFramework) edgeType).TypeID];
        }

        public override IEnumerable<INode> GetExactNodes(IType nodeType)
        {
            LGSPNode head = nodesByTypeHeads[((ITypeFramework) nodeType).TypeID];
            LGSPNode cur = head.typeNext;
            LGSPNode next;
            while(cur != head)
            {
                next = cur.typeNext;
                yield return cur;
                cur = next;
            }
        }

        public override IEnumerable<IEdge> GetExactEdges(IType edgeType)
        {
            LGSPEdge head = edgesByTypeHeads[((ITypeFramework) edgeType).TypeID];
            LGSPEdge cur = head.typeNext;
            LGSPEdge next;
            while(cur != head)
            {
                next = cur.typeNext;
                yield return cur;
                cur = next;
            }
        }

        public override int GetNumCompatibleNodes(IType nodeType)
        {
            int num = 0;
            foreach(IType type in nodeType.SubOrSameTypes)
                num += nodesByTypeCounts[((ITypeFramework) type).TypeID];

            return num;
        }

        public override int GetNumCompatibleEdges(IType edgeType)
        {
            int num = 0;
            foreach(IType type in edgeType.SubOrSameTypes)
                num += edgesByTypeCounts[((ITypeFramework) type).TypeID];

            return num;
        }

        public override IEnumerable<INode> GetCompatibleNodes(IType nodeType)
        {
            foreach(IType type in nodeType.SubOrSameTypes)
            {
                LGSPNode head = nodesByTypeHeads[((ITypeFramework) type).TypeID];
                LGSPNode cur = head.typeNext;
                LGSPNode next;
                while(cur != head)
                {
                    next = cur.typeNext;
                    yield return cur;
                    cur = next;
                }
            }
        }

        public override IEnumerable<IEdge> GetCompatibleEdges(IType edgeType)
        {
            foreach(IType type in edgeType.SubOrSameTypes)
            {
                LGSPEdge head = edgesByTypeHeads[((ITypeFramework) type).TypeID];
                LGSPEdge cur = head.typeNext;
                LGSPEdge next;
                while(cur != head)
                {
                    next = cur.typeNext;
                    yield return cur;
                    cur = next;
                }
            }
        }

        public void MoveHeadAfter(LGSPNode elem)
        {
            if(elem.type == null) return;       // elem is head
            LGSPNode head = nodesByTypeHeads[elem.type.typeID];

            head.typePrev.typeNext = head.typeNext;
            head.typeNext.typePrev = head.typePrev;

            elem.typeNext.typePrev = head;
            head.typeNext = elem.typeNext;
            head.typePrev = elem;
            elem.typeNext = head;
        }

        public void MoveHeadAfter(LGSPEdge elem)
        {
            if(elem.type == null) return;       // elem is head
            LGSPEdge head = edgesByTypeHeads[elem.type.typeID];

            head.typePrev.typeNext = head.typeNext;
            head.typeNext.typePrev = head.typePrev;

            elem.typeNext.typePrev = head;
            head.typeNext = elem.typeNext;
            head.typePrev = elem;
            elem.typeNext = head;
        }

        internal void AddNodeWithEvent(LGSPNode node)
        {
            AddNode(node, node.type.typeID);
            NodeAdded(node);
        }

        internal void AddEdgeWithEvent(LGSPEdge edge)
        {
            AddEdge(edge, edge.type.typeID);
            EdgeAdded(edge);
        }

        /// <summary>
        /// Adds an existing node to this graph.
        /// The graph may not already contain the node!
        /// Intended only for undo, clone and internal use!
        /// </summary>
        internal void AddNode(LGSPNode node, int typeid)
        {
            LGSPNode head = nodesByTypeHeads[typeid];
            head.typeNext.typePrev = node;
            node.typeNext = head.typeNext;
            node.typePrev = head;
            head.typeNext = node;

            nodesByTypeCounts[typeid]++;
        }

        /// <summary>
        /// Adds an existing edge to this graph.
        /// The graph may not already contain the edge!
        /// Intended only for undo and internal use!
        /// </summary>
        internal void AddEdge(LGSPEdge edge, int typeid)
        {
            LGSPEdge head = edgesByTypeHeads[typeid];
            head.typeNext.typePrev = edge;
            edge.typeNext = head.typeNext;
            edge.typePrev = head;
            head.typeNext = edge;

            edge.source.AddOutgoing(edge);
            edge.target.AddIncoming(edge);

            edgesByTypeCounts[typeid]++;
        }

        public LGSPNode AddNode(ITypeFramework nodeType)
        {
            LGSPNode node = new LGSPNode(nodeType);
            AddNode(node, nodeType.typeID);
            NodeAdded(node);
            return node;
        }

        public override INode AddNode(IType nodeType)
        {
            return AddNode((ITypeFramework)nodeType);
        }

        public override INode AddNode(IType nodeType, String varName)
        {
            LGSPNode node = new LGSPNode((ITypeFramework) nodeType);
            AddNode(node, ((ITypeFramework) nodeType).typeID);
            SetVariableValue(varName, node);
            NodeAdded(node);
            return node;
        }

        public LGSPEdge AddEdge(ITypeFramework edgeType, LGSPNode source, LGSPNode target)
        {
            LGSPEdge edge = new LGSPEdge(edgeType, source, target);
            AddEdge(edge, edgeType.typeID);
            EdgeAdded(edge);
            return edge;
        }

        public override IEdge AddEdge(IType edgeType, INode source, INode target)
        {
            return AddEdge((ITypeFramework)edgeType, (LGSPNode)source, (LGSPNode)target);
        }

        public override IEdge AddEdge(IType edgeType, INode source, INode target, String varName)
        {
            LGSPNode lsource = (LGSPNode) source;
            LGSPNode ltarget = (LGSPNode) target;
            LGSPEdge edge = new LGSPEdge((ITypeFramework) edgeType, lsource, ltarget);
            AddEdge(edge, ((ITypeFramework) edgeType).typeID);
            transactionManager.ElementAdded(edge);
            SetVariableValue(varName, edge);
//            VariableAdded(edge, varName);
            EdgeAdded(edge);
            return edge;
        }

        internal void RemoveNode(LGSPNode node, int typeid)
        {
            node.typePrev.typeNext = node.typeNext;
            node.typeNext.typePrev = node.typePrev;
            node.typeNext = null;
            node.typePrev = null;

            nodesByTypeCounts[typeid]--;
        }

        public override void Remove(INode node)
        {
            LGSPNode lnode = (LGSPNode) node;
            if(lnode.HasOutgoing || lnode.HasIncoming)
                throw new ArgumentException("The given node still has edges left!");
            if(lnode.typeNext == null) return;          // node not in graph (anymore)

            RemovingNode(node);
#if ELEMENTKNOWSVARIABLES
            if(lnode.variableList != null)
            {
                foreach(Variable var in lnode.variableList)
                {
                    VariableMap.Remove(var.Name);
                }
                lnode.variableList = null;
                // TODO: What about ElementMap??
            }
#else
            if(lnode.hasVariables)
            {
                foreach(Variable var in ElementMap[lnode])
                    VariableMap.Remove(var.Name);
                ElementMap.Remove(lnode);
                lnode.hasVariables = false;
            }
#endif
            RemoveNode(lnode, lnode.type.typeID);
        }

        internal void RemoveEdge(LGSPEdge edge, int typeid)
        {
            edge.typePrev.typeNext = edge.typeNext;
            edge.typeNext.typePrev = edge.typePrev;
            edge.typeNext = null;
            edge.typePrev = null;

            edgesByTypeCounts[typeid]--;
        }

        public override void Remove(IEdge edge)
        {
            LGSPEdge ledge = (LGSPEdge) edge;
            if(ledge.typeNext == null) return;          // edge not in graph (anymore)

            RemovingEdge(edge);
#if ELEMENTKNOWSVARIABLES
            if(ledge.variableList != null)
            {
                foreach(Variable var in ledge.variableList)
                {
                    VariableMap.Remove(var.Name);
                }
                ledge.variableList = null;
                // TODO: What about ElementMap??
            }
#else
            if(ledge.hasVariables)
            {
                foreach(Variable var in ElementMap[ledge])
                    VariableMap.Remove(var.Name);
                ElementMap.Remove(ledge);
                ledge.hasVariables = false;
            }
#endif
            ledge.source.RemoveOutgoing(ledge);
            ledge.target.RemoveIncoming(ledge);
            RemoveEdge(ledge, ledge.type.typeID);
        }

        public override void RemoveEdges(INode node)
        {
            LGSPNode lnode = (LGSPNode) node;

            RemovingEdges(node);
            foreach(LGSPEdge edge in lnode.Outgoing)
                Remove(edge);
            foreach(LGSPEdge edge in lnode.Incoming)
                Remove(edge);
        }

        /// <summary>
        /// Reuses an LGSPNode object for a new node and optionally changes the type.
        /// This causes a RemovingEdges, a RemovingNode and a NodeAdded event and removes all edges
        /// and variables pointing to the old element.
        /// </summary>
        /// <param name="node">The LGSPNode object to be reused.</param>
        /// <param name="newType">The new type to be used for the node or null, if the type is not to be changed</param>
        public void ReuseNode(LGSPNode node, ITypeFramework newType)
        {
            RemoveEdges(node);
            RemovingNode(node);
#if ELEMENTKNOWSVARIABLES
            if(node.variableList != null)
            {
                foreach(Variable var in node.variableList)
                    VariableMap.Remove(var.Name);
                node.variableList = null;
                // TODO: What about ElementMap??
            }
#else
            if(node.hasVariables)
            {
                foreach(Variable var in ElementMap[node])
                    VariableMap.Remove(var.Name);
                ElementMap.Remove(node);
                node.hasVariables = false;
            }
#endif

            if(newType != null)
            {
                node.typePrev.typeNext = node.typeNext;
                node.typeNext.typePrev = node.typePrev;
                nodesByTypeCounts[node.type.typeID]--;
                node.type = newType;

                LGSPNode head = nodesByTypeHeads[newType.typeID];
                head.typeNext.typePrev = node;
                node.typeNext = head.typeNext;
                node.typePrev = head;
                head.typeNext = node;
                nodesByTypeCounts[newType.typeID]++;
            }

            // Reset attributes
            node.attributes = node.type.CreateAttributes();

            NodeAdded(node);
        }

        /// <summary>
        /// Reuses an LGSPEdge object for a new edge and optionally changes the source, target and/or type.
        /// This causes a RemovingEdge and a EdgeAdded event and removes all variables pointing to the old element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="newSource"></param>
        /// <param name="newTarget"></param>
        /// <param name="newType"></param>
        public void ReuseEdge(LGSPEdge edge, LGSPNode newSource, LGSPNode newTarget, ITypeFramework newType)
        {
            RemovingEdge(edge);
#if ELEMENTKNOWSVARIABLES
            if(edge.variableList != null)
            {
                foreach(Variable var in edge.variableList)
                    VariableMap.Remove(var.Name);
                edge.variableList = null;
                // TODO: What about ElementMap??
            }
#else
            if(edge.hasVariables)
            {
                foreach(Variable var in ElementMap[edge])
                    VariableMap.Remove(var.Name);
                ElementMap.Remove(edge);
                edge.hasVariables = false;
            }
#endif

            if(newSource != null)
            {
                // removeOutgoing
                if(edge == edge.source.outhead)
                {
                    edge.source.outhead = edge.outNext;
                    if(edge.source.outhead == edge)
                        edge.source.outhead = null;
                }
                edge.outPrev.outNext = edge.outNext;
                edge.outNext.outPrev = edge.outPrev;
                edge.source = newSource;

                // addOutgoing
                LGSPEdge outhead = newSource.outhead;
                if(outhead == null)
                {
                    newSource.outhead = edge;
                    edge.outNext = edge;
                    edge.outPrev = edge;
                }
                else
                {
                    outhead.outPrev.outNext = edge;
                    edge.outPrev = outhead.outPrev;
                    edge.outNext = outhead;
                    outhead.outPrev = edge;
                }
            }

            if(newTarget != null)
            {
                // removeIncoming
                if(edge == edge.target.inhead)
                {
                    edge.target.inhead = edge.inNext;
                    if(edge.target.inhead == edge)
                        edge.target.inhead = null;
                }
                edge.inPrev.inNext = edge.inNext;
                edge.inNext.inPrev = edge.inPrev;
                edge.target = newTarget;

                // addIncoming
                LGSPEdge inhead = newTarget.inhead;
                if(inhead == null)
                {
                    newTarget.inhead = edge;
                    edge.inNext = edge;
                    edge.inPrev = edge;
                }
                else
                {
                    inhead.inPrev.inNext = edge;
                    edge.inPrev = inhead.inPrev;
                    edge.inNext = inhead;
                    inhead.inPrev = edge;
                }
            }

            if(newType != null)
            {
                edge.typePrev.typeNext = edge.typeNext;
                edge.typeNext.typePrev = edge.typePrev;
                edgesByTypeCounts[edge.type.typeID]--;
                edge.type = newType;

                LGSPEdge head = edgesByTypeHeads[newType.typeID];
                head.typeNext.typePrev = edge;
                edge.typeNext = head.typeNext;
                edge.typePrev = head;
                head.typeNext = edge;
                edgesByTypeCounts[newType.typeID]++;
            }

            // Reset attributes
            edge.attributes = edge.type.CreateAttributes();

            EdgeAdded(edge);
        }

        public override void Clear()
        {
            ClearingGraph();
            InitializeGraph();
        }

        private void RetypeWithCopyCommons<T>(LGSPGraphElement<T> elem, ITypeFramework newType)
        {
//            IAttributes newAttrs = (IAttributes) Activator.CreateInstance(newType.AttributesType);
            IAttributes newAttrs = newType.CreateAttributes();

            IEnumerable<AttributeType> minAttrTypes;
            IType otherType;

            // Use the type with the smaller amount of attributes to reduce number of loop iterations
            if(elem.Type.NumAttributes < newType.NumAttributes)
            {
                minAttrTypes = elem.Type.AttributeTypes;
                otherType = newType;
            }
            else
            {
                minAttrTypes = newType.AttributeTypes;
                otherType = elem.Type;
            }

            // Copy all attributes from common super types into newAttrs    
            foreach(AttributeType attrType in minAttrTypes)
            {
                AttributeType otherAttrType = otherType.GetAttributeType(attrType.Name);
                if(otherAttrType == null || attrType.OwnerType != otherAttrType.OwnerType) continue;

                object value = elem.GetAttribute(attrType.Name);
                newAttrs.GetType().GetProperty(attrType.Name).SetValue(newAttrs, value, null);
            }
            if(elem is INode)
                SettingNodeType((INode) elem, elem.type, elem.attributes, newType, newAttrs);
            else
                SettingEdgeType((IEdge) elem, elem.type, elem.attributes, newType, newAttrs);
            elem.type = newType;
            elem.attributes = newAttrs;
        }

        public override IAttributes SetNodeType(INode node, IType newNodeType)
        {
            LGSPNode lnode = (LGSPNode) node;
            IAttributes oldAttrs = lnode.attributes;
            int oldtypeid = lnode.type.typeID;
            ITypeFramework newType = (ITypeFramework) newNodeType;

            RetypeWithCopyCommons(lnode, newType);

            // Update node type array
            RemoveNode(lnode, oldtypeid);
            AddNode(lnode, newType.typeID);

            return oldAttrs;
        }

        public override IAttributes SetEdgeType(IEdge edge, IType newEdgeType)
        {
            LGSPEdge ledge = (LGSPEdge) edge;
            IAttributes oldAttrs = ledge.attributes;
            int oldtypeid = ledge.type.typeID;
            ITypeFramework newType = (ITypeFramework) newEdgeType;

            RetypeWithCopyCommons(ledge, newType);

            // Update edge type array
            RemoveEdge(ledge, oldtypeid);
            AddEdge(ledge, newType.typeID);

            return oldAttrs;
        }

        #region Variables management

        protected Dictionary<IGraphElement, LinkedList<Variable>> ElementMap = new Dictionary<IGraphElement, LinkedList<Variable>>();
        protected Dictionary<String, Variable> VariableMap = new Dictionary<String, Variable>();

        /// <summary>
        /// Returns the first variable name for the given element it finds (if any).
        /// </summary>
        /// <param name="elem">Element which name is to be found</param>
        /// <returns>A name which can be used in GetVariableValue to get this element</returns>
        public override String GetElementName(IGraphElement elem)
        {
            LinkedList<Variable> variableList;
            if(ElementMap.TryGetValue(elem, out variableList))
                return variableList.First.Value.Name;
            return "$" + elem.GetHashCode();
        }

        /// <summary>
        /// Returns a linked list of variables mapped to the given graph element
        /// or null, if no variable points to this element
        /// </summary>
        public override LinkedList<Variable> GetElementVariables(IGraphElement elem)
        {
#if ELEMENTKNOWSVARIABLES
            LGSPNode node = elem as LGSPNode;
            if(node != null) return node.variableList;
            else return ((LGSPEdge) elem).variableList;
#else
            LinkedList<Variable> variableList;
            ElementMap.TryGetValue(elem, out variableList);
            return variableList;
#endif
        }

        /// <summary>
        /// Retrieves the IGraphElement for a variable name or null, if the variable isn't set yet or anymore
        /// </summary>
        /// <param name="varName">The variable name to lookup</param>
        /// <returns>The according IGraphElement or null</returns>
        public override IGraphElement GetVariableValue(String varName)
        {
            Variable var;
            VariableMap.TryGetValue(varName, out var);
            if(var == null) return null;
            return var.Element;
        }

        /// <summary>
        /// Detaches the specified variable from the according graph element.
        /// If it was the last variable pointing to the element, the variable list for the element is removed.
        /// </summary>
        /// <param name="var">Variable to detach.</param>
        private void DetachVariableFromElement(Variable var)
        {
            LinkedList<Variable> oldVarList = ElementMap[var.Element];
            oldVarList.Remove(var);
            if(oldVarList.Count == 0)
            {
                ElementMap.Remove(var.Element);

                LGSPNode oldNode = var.Element as LGSPNode;
                if(oldNode != null) oldNode.hasVariables = false;
                else
                {
                    LGSPEdge oldEdge = (LGSPEdge) var.Element;
                    oldEdge.hasVariables = false;
                }
            }
        }

        /// <summary>
        /// Sets the value of the given variable to the given IGraphElement.
        /// If the variable name is null, this function does nothing.
        /// If elem is null, the variable is unset.
        /// </summary>
        /// <param name="varName">The name of the variable.</param>
        /// <param name="elem">The new value of the variable or null to unset the variable.</param>
        public override void SetVariableValue(String varName, IGraphElement elem)
        {
#if ELEMENTKNOWSVARIABLES            
            Variable var;

            if (varName == null) return;
            if (elem == null)
            {
                if (VariableMap.TryGetValue(varName, out var))
                {
                    LinkedList<Variable> oldVarList;
                    LGSPNode oldNode = var.Element as LGSPNode;
                    if (oldNode != null)
                    {
                        oldVarList = oldNode.variableList;
                        if (oldVarList != null)
                            oldVarList.Remove(var);
                    }
                    else
                    {
                        LGSPEdge oldEdge = (LGSPEdge)var.Element;
                        oldVarList = oldEdge.variableList;
                        if (oldVarList != null)
                            oldVarList.Remove(var);
                    }
                    VariableMap.Remove(varName);
                }
                return;
            }

            if(!VariableMap.TryGetValue(varName, out var))
            {
                var = new Variable(varName, elem);
                VariableMap[varName] = var;
            }
            else
            {
                LinkedList<Variable> oldVarList;
                LGSPNode oldNode = var.Element as LGSPNode;
                if(oldNode != null)
                {
                    oldVarList = oldNode.variableList;
                    if(oldVarList != null)
                        oldVarList.Remove(var);
                }
                else
                {
                    LGSPEdge oldEdge = (LGSPEdge) var.Element;
                    oldVarList = oldEdge.variableList;
                    if(oldVarList != null)
                        oldVarList.Remove(var);
                }
                var.Element = elem;
            }

            LinkedList<Variable> newVarList;
            LGSPNode node = elem as LGSPNode;
            if(node != null)
            {
                newVarList = node.variableList;
                if(newVarList == null)
                {
                    newVarList = new LinkedList<Variable>();
                    newVarList.AddFirst(var);
                    node.variableList = newVarList;
                }
                else if(!newVarList.Contains(var))
                    newVarList.AddFirst(var);
            }
            else
            {
                LGSPEdge edge = (LGSPEdge) elem;
                newVarList = edge.variableList;
                if(newVarList == null)
                {
                    newVarList = new LinkedList<Variable>();
                    newVarList.AddFirst(var);
                    edge.variableList = newVarList;
                }
                else if(!newVarList.Contains(var))
                    newVarList.AddFirst(var);
            }
#else
            if(varName == null) return;

            Variable var;
            VariableMap.TryGetValue(varName, out var);

            if(var != null)
            {
                if(var.Element == elem) return;     // Variable already set to this element?
                DetachVariableFromElement(var);

                if(elem == null)
                {
                    VariableMap.Remove(varName);
                    return;
                }
                var.Element = elem;
            }
            else
            {
                if(elem == null) return;

                var = new Variable(varName, elem);
                VariableMap[varName] = var;
            }

            LinkedList<Variable> newVarList;
            if(!ElementMap.TryGetValue(elem, out newVarList))
            {
                newVarList = new LinkedList<Variable>();
                ElementMap[elem] = newVarList;
            }
            newVarList.AddFirst(var);

            LGSPNode node = elem as LGSPNode;
            if(node != null)
                node.hasVariables = true;
            else
            {
                LGSPEdge edge = (LGSPEdge) elem;
                edge.hasVariables = true;
            }
#endif
        }

        #endregion Variables management

#if USE_SUB_SUPER_ENUMERATORS
        public void AnalyseGraph()
        {
            int numNodeTypes = Model.NodeModel.Types.Length;
            int numEdgeTypes = Model.EdgeModel.Types.Length;

            int[,] outgoingVCount = new int[numEdgeTypes, numNodeTypes];
            int[,] incomingVCount = new int[numEdgeTypes, numNodeTypes];

#if MONO_MULTIDIMARRAY_WORKAROUND
            dim0size = numNodeTypes;
            dim1size = numEdgeTypes;
            dim2size = numNodeTypes;
            vstructs = new float[numNodeTypes*numEdgeTypes*numNodeTypes*2];
#else
            vstructs = new float[numNodeTypes, numEdgeTypes, numNodeTypes, 2];
#endif
            nodeCounts = new int[numNodeTypes];
            edgeCounts = new int[numEdgeTypes];

            foreach(ITypeFramework nodeType in Model.NodeModel.Types)
            {
                foreach(ITypeFramework superType in nodeType.SuperOrSameTypes)
                    nodeCounts[superType.typeID] += nodes[superType.typeID].Count;

                for(LGSPNode nodeHead = nodes[nodeType.typeID].Head, node = nodeHead.next; node != nodeHead; node = node.next)
                {
                    // count outgoing v structures
                    outgoingVCount.Initialize();
                    for(LGSPEdge edgeHead = node.outgoing.Head, edge = edgeHead.outNode.next; edge != edgeHead; edge = edge.outNode.next)
                    {
                        ITypeFramework targetType = edge.target.type;
//                        outgoingVCount[edge.type.TypeID, nodeType.TypeID]++;

                        foreach(ITypeFramework edgeSuperType in edge.Type.SuperOrSameTypes)
                        {
                            int superTypeID = edgeSuperType.typeID;
                            foreach(ITypeFramework targetSuperType in targetType.SuperOrSameTypes)
                            {
                                outgoingVCount[superTypeID, targetSuperType.typeID]++;
                            }
                        }
                    }

                    // count incoming v structures
                    incomingVCount.Initialize();
                    for(LGSPEdge edgeHead = node.incoming.Head, edge = edgeHead.inNode.next; edge != edgeHead; edge = edge.inNode.next)
                    {
                        ITypeFramework sourceType = edge.source.type;
//                        incomingVCount[edge.type.TypeID, nodeType.TypeID]++;

                        foreach(ITypeFramework edgeSuperType in edge.Type.SuperOrSameTypes)
                        {
                            int superTypeID = edgeSuperType.typeID;
                            foreach(ITypeFramework sourceSuperType in sourceType.SuperOrSameTypes)
                            {
                                incomingVCount[superTypeID, sourceSuperType.typeID]++;
                            }
                        }
                    }

                    // finalize the counting and collect resulting local v-struct info

                    for(LGSPEdge edgeHead = node.outgoing.Head, edge = edgeHead.outNode.next; edge != edgeHead; edge = edge.outNode.next)
                    {
                        ITypeFramework targetType = edge.target.type;
                        int targetTypeID = targetType.typeID;

                        foreach(ITypeFramework edgeSuperType in edge.Type.SuperOrSameTypes)
                        {
                            int superTypeID = edgeSuperType.typeID;

                            foreach(ITypeFramework targetSuperType in targetType.SuperOrSameTypes)
                            {
                                if(outgoingVCount[superTypeID, targetSuperType.typeID] > 1)
                                {
                                    float val = (float) Math.Log(outgoingVCount[superTypeID, targetSuperType.typeID]);
                                    foreach(ITypeFramework nodeSuperType in nodeType.SuperOrSameTypes)
#if MONO_MULTIDIMARRAY_WORKAROUND
//                                        vstructs[((superTypeID * dim1size + nodeSuperType.TypeID) * dim2size + targetTypeID) * 2
//                                            + (int) LGSPDir.Out] += val;
                                        vstructs[((nodeSuperType.typeID * dim1size + superTypeID) * dim2size + targetTypeID) * 2
                                            + (int) LGSPDir.Out] += val;
#else
//                                        vstructs[superTypeID, nodeSuperType.TypeID, targetTypeID, (int) LGSPDir.Out] += val;
                                        vstructs[nodeSuperType.TypeID, superTypeID, targetTypeID, (int) LGSPDir.Out] += val;
#endif
                                    outgoingVCount[superTypeID, targetSuperType.typeID] = 0;
                                }
                            }
                        }
                    }

                    for(LGSPEdge edgeHead = node.incoming.Head, edge = edgeHead.inNode.next; edge != edgeHead; edge = edge.inNode.next)
                    {
                        ITypeFramework sourceType = edge.source.type;
                        int sourceTypeID = sourceType.typeID;

                        foreach(ITypeFramework edgeSuperType in edge.Type.SuperOrSameTypes)
                        {
                            int superTypeID = edgeSuperType.typeID;
                            foreach(ITypeFramework sourceSuperType in sourceType.SuperOrSameTypes)
                            {
                                if(incomingVCount[superTypeID, sourceSuperType.typeID] > 1)
                                {
                                    float val = (float) Math.Log(incomingVCount[superTypeID, sourceSuperType.typeID]);

                                    foreach(ITypeFramework nodeSuperType in nodeType.SuperOrSameTypes)
#if MONO_MULTIDIMARRAY_WORKAROUND
//                                        vstructs[((superTypeID * dim1size + nodeSuperType.TypeID) * dim2size + sourceTypeID) * 2
//                                            + (int) LGSPDir.In] += val;
                                        vstructs[((nodeSuperType.typeID * dim1size + superTypeID) * dim2size + sourceTypeID) * 2
                                            + (int) LGSPDir.In] += val;
#else
//                                        vstructs[superTypeID, nodeSuperType.TypeID, sourceTypeID, (int) LGSPDir.In] += val;
                                        vstructs[nodeSuperType.TypeID, superTypeID, sourceTypeID, (int) LGSPDir.In] += val;
#endif
                                    incomingVCount[superTypeID, sourceSuperType.typeID] = 0;
                                }
                            }
                        }
                    }
                }
            }

            nodeLookupCosts = new float[numNodeTypes];
            for(int i = 0; i < numNodeTypes; i++)
            {
                if(nodeCounts[i] <= 1)
                    nodeLookupCosts[i] = 0.00001F;                              // TODO: check this value (>0 because of preset elements)
                else
                    nodeLookupCosts[i] = (float) Math.Log(nodeCounts[i]);
            }

            // Calculate edgeCounts
            foreach(ITypeFramework edgeType in Model.EdgeModel.Types)
            {
                edgeCounts[edgeType.typeID] += edges[edgeType.typeID].Count;
                foreach(ITypeFramework superType in edgeType.SuperTypes)
                    edgeCounts[superType.typeID] += edges[superType.typeID].Count;
            }

            edgeLookupCosts = new float[numEdgeTypes];
            for(int i = 0; i < numEdgeTypes; i++)
            {
                if(edgeCounts[i] <= 1)
                    edgeLookupCosts[i] = 0.00001F;                              // TODO: check this value (>0 because of preset elements)
                else
                    edgeLookupCosts[i] = (float) Math.Log(edgeCounts[i]);
            }
        }
#elif OPCOST_WITH_GEO_MEAN
        public void AnalyzeGraph()
        {
            int numNodeTypes = Model.NodeModel.Types.Length;
            int numEdgeTypes = Model.EdgeModel.Types.Length;

            int[,] outgoingVCount = new int[numEdgeTypes, numNodeTypes];
            int[,] incomingVCount = new int[numEdgeTypes, numNodeTypes];

#if MONO_MULTIDIMARRAY_WORKAROUND
            dim0size = numNodeTypes;
            dim1size = numEdgeTypes;
            dim2size = numNodeTypes;
            vstructs = new float[numNodeTypes*numEdgeTypes*numNodeTypes*2];
#else
            vstructs = new float[numNodeTypes, numEdgeTypes, numNodeTypes, 2];
#endif
            nodeCounts = new int[numNodeTypes];
            edgeCounts = new int[numEdgeTypes];
            nodeIncomingCount = new float[numNodeTypes];
            nodeOutgoingCount = new float[numNodeTypes];

            foreach(ITypeFramework nodeType in Model.NodeModel.Types)
            {
                foreach(ITypeFramework superType in nodeType.superOrSameTypes)
                    nodeCounts[superType.typeID] += nodesByTypeCounts[nodeType.typeID];

                for(LGSPNode nodeHead = nodesByTypeHeads[nodeType.typeID], node = nodeHead.typeNext; node != nodeHead; node = node.typeNext)
                {
                    //
                    // count outgoing v structures
                    //

                    for(int i = 0; i < numEdgeTypes; i++)
                        for(int j = 0; j < numNodeTypes; j++)
                            outgoingVCount[i, j] = 0;

                    LGSPEdge outhead = node.outhead;
                    if(outhead != null)
                    {
                        LGSPEdge edge = outhead;
                        do
                        {
                            ITypeFramework targetType = edge.target.type;
                            nodeOutgoingCount[nodeType.typeID]++;
                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int superTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework targetSuperType in targetType.superOrSameTypes)
                                {
                                    outgoingVCount[superTypeID, targetSuperType.typeID]++;
                                }
                            }
                            edge = edge.outNext;
                        }
                        while(edge != outhead);
                    }

                    //
                    // count incoming v structures
                    //

                    for(int i = 0; i < numEdgeTypes; i++)
                        for(int j = 0; j < numNodeTypes; j++)
                            incomingVCount[i, j] = 0;

                    LGSPEdge inhead = node.inhead;
                    if(inhead != null)
                    {
                        LGSPEdge edge = inhead;
                        do
                        {
                            ITypeFramework sourceType = edge.source.type;
                            nodeIncomingCount[nodeType.typeID]++;
                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int superTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework sourceSuperType in sourceType.superOrSameTypes)
                                {
                                    incomingVCount[superTypeID, sourceSuperType.typeID]++;
                                }
                            }
                            edge = edge.inNext;
                        }
                        while(edge != inhead);
                    }

                    //
                    // finalize the counting and collect resulting local v-struct info
                    //

                    if(outhead != null)
                    {
                        LGSPEdge edge = outhead;
                        do
                        {
                            ITypeFramework targetType = edge.target.type;
                            int targetTypeID = targetType.typeID;

                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int edgeSuperTypeID = edgeSuperType.typeID;

                                foreach(ITypeFramework targetSuperType in targetType.superOrSameTypes)
                                {
                                    int targetSuperTypeID = targetSuperType.typeID;
                                    if(outgoingVCount[edgeSuperTypeID, targetSuperTypeID] > 1)
                                    {
                                        float val = (float) Math.Log(outgoingVCount[edgeSuperTypeID, targetSuperTypeID]);
                                        foreach(ITypeFramework nodeSuperType in nodeType.superOrSameTypes)
                                        {
#if MONO_MULTIDIMARRAY_WORKAROUND
                                            vstructs[((nodeSuperType.typeID * dim1size + edgeSuperTypeID) * dim2size + targetSuperTypeID) * 2
                                                + (int) LGSPDir.Out] += val;
#else
                                            vstructs[nodeSuperType.TypeID, edgeSuperTypeID, targetSuperTypeID, (int) LGSPDir.Out] += val;
#endif
                                        }
                                        outgoingVCount[edgeSuperTypeID, targetSuperTypeID] = 0;
                                    }
                                }
                            }
                            edge = edge.outNext;
                        }
                        while(edge != outhead);
                    }

                    if(inhead != null)
                    {
                        LGSPEdge edge = inhead;
                        do
                        {
                            ITypeFramework sourceType = edge.source.type;
                            int sourceTypeID = sourceType.typeID;

                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int edgeSuperTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework sourceSuperType in sourceType.superOrSameTypes)
                                {
                                    int sourceSuperTypeID = sourceSuperType.typeID;
                                    if(incomingVCount[edgeSuperTypeID, sourceSuperTypeID] > 1)
                                    {
                                        float val = (float) Math.Log(incomingVCount[edgeSuperTypeID, sourceSuperTypeID]);

                                        foreach(ITypeFramework nodeSuperType in nodeType.superOrSameTypes)
#if MONO_MULTIDIMARRAY_WORKAROUND
                                            vstructs[((nodeSuperType.typeID * dim1size + edgeSuperTypeID) * dim2size + sourceSuperTypeID) * 2
                                                + (int) LGSPDir.In] += val;
#else
                                            vstructs[nodeSuperType.TypeID, edgeSuperTypeID, sourceSuperTypeID, (int) LGSPDir.In] += val;
#endif
                                        incomingVCount[edgeSuperTypeID, sourceSuperTypeID] = 0;
                                    }
                                }
                            }
                            edge = edge.inNext;
                        }
                        while(edge != inhead);
                    }
                }
            }

            nodeLookupCosts = new float[numNodeTypes];
            for(int i = 0; i < numNodeTypes; i++)
            {
                if(nodeCounts[i] <= 1)
                    nodeLookupCosts[i] = 0;
                else
                    nodeLookupCosts[i] = (float) Math.Log(nodeCounts[i]);
            }

            // Calculate edgeCounts
            foreach(ITypeFramework edgeType in Model.EdgeModel.Types)
            {
                foreach(ITypeFramework superType in edgeType.superOrSameTypes)
                    edgeCounts[superType.typeID] += edgesByTypeCounts[edgeType.typeID];
            }

            edgeLookupCosts = new float[numEdgeTypes];
            for(int i = 0; i < numEdgeTypes; i++)
            {
                if(edgeCounts[i] <= 1)
                    edgeLookupCosts[i] = 0;
                else
                    edgeLookupCosts[i] = (float) Math.Log(edgeCounts[i]);
            }
        }
#else
        public void AnalyzeGraph()
        {
            int numNodeTypes = Model.NodeModel.Types.Length;
            int numEdgeTypes = Model.EdgeModel.Types.Length;

            int[,] outgoingVCount = new int[numEdgeTypes, numNodeTypes];
            int[,] incomingVCount = new int[numEdgeTypes, numNodeTypes];

#if MONO_MULTIDIMARRAY_WORKAROUND
            dim0size = numNodeTypes;
            dim1size = numEdgeTypes;
            dim2size = numNodeTypes;
            vstructs = new float[numNodeTypes * numEdgeTypes * numNodeTypes * 2];
#else
            vstructs = new float[numNodeTypes, numEdgeTypes, numNodeTypes, 2];
#endif
            nodeCounts = new int[numNodeTypes];
            edgeCounts = new int[numEdgeTypes];
            meanInDegree = new float[numNodeTypes];
            meanOutDegree = new float[numNodeTypes];

#if SCHNELLERER_ANSATZ_NUR_ANGEFANGEN
            foreach(ITypeFramework edgeType in Model.EdgeModel.Types)
            {
                /*                foreach(ITypeFramework superType in nodeType.superOrSameTypes)
                                    nodeCounts[superType.typeID] += nodesByTypeCounts[nodeType.typeID];*/

                for(LGSPEdge edgeHead = edgesByTypeHeads[edgeType.typeID], edge = edgeHead.typeNext; edge != edgeHead; edge = edge.typeNext)
                {
                    ITypeFramework sourceType = edge.source.type;
                    ITypeFramework targetType = edge.target.type;

#if MONO_MULTIDIMARRAY_WORKAROUND
                    vstructs[((sourceType.typeID * dim1size + edgeType.typeID) * dim2size + targetType.typeID) * 2 + (int) LGSPDir.Out] += val;
#else
                    vstructs[nodeSuperType.TypeID, edgeSuperTypeID, targetSuperTypeID, (int) LGSPDir.Out] += val;
#endif
                }
            }
#endif

            foreach(ITypeFramework nodeType in Model.NodeModel.Types)
            {
                foreach(ITypeFramework superType in nodeType.superOrSameTypes)
                    nodeCounts[superType.typeID] += nodesByTypeCounts[nodeType.typeID];

                for(LGSPNode nodeHead = nodesByTypeHeads[nodeType.typeID], node = nodeHead.typeNext; node != nodeHead; node = node.typeNext)
                {
                    //
                    // count outgoing v structures
                    //

                    for(int i = 0; i < numEdgeTypes; i++)
                        for(int j = 0; j < numNodeTypes; j++)
                            outgoingVCount[i, j] = 0;

                    LGSPEdge outhead = node.outhead;
                    if(outhead != null)
                    {
                        LGSPEdge edge = outhead;
                        do
                        {
                            ITypeFramework targetType = edge.target.type;
                            meanOutDegree[nodeType.typeID]++;
                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int superTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework targetSuperType in targetType.superOrSameTypes)
                                {
                                    outgoingVCount[superTypeID, targetSuperType.typeID]++;
                                }
                            }
                            edge = edge.outNext;
                        }
                        while(edge != outhead);
                    }

                    //
                    // count incoming v structures
                    //

                    for(int i = 0; i < numEdgeTypes; i++)
                        for(int j = 0; j < numNodeTypes; j++)
                            incomingVCount[i, j] = 0;

                    LGSPEdge inhead = node.inhead;
                    if(inhead != null)
                    {
                        LGSPEdge edge = inhead;
                        do
                        {
                            ITypeFramework sourceType = edge.source.type;
                            meanInDegree[nodeType.typeID]++;
                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int superTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework sourceSuperType in sourceType.superOrSameTypes)
                                {
                                    incomingVCount[superTypeID, sourceSuperType.typeID]++;
                                }
                            }
                            edge = edge.inNext;
                        }
                        while(edge != inhead);
                    }

                    //
                    // finalize the counting and collect resulting local v-struct info
                    //

                    if(outhead != null)
                    {
                        LGSPEdge edge = outhead;
                        do
                        {
                            ITypeFramework targetType = edge.target.type;
                            int targetTypeID = targetType.typeID;

                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int edgeSuperTypeID = edgeSuperType.typeID;

                                foreach(ITypeFramework targetSuperType in targetType.superOrSameTypes)
                                {
                                    int targetSuperTypeID = targetSuperType.typeID;
                                    if(outgoingVCount[edgeSuperTypeID, targetSuperTypeID] > 0)
                                    {
//                                        float val = (float) Math.Log(outgoingVCount[edgeSuperTypeID, targetSuperTypeID]);     // > 1 im if
                                        float val = outgoingVCount[edgeSuperTypeID, targetSuperTypeID];
                                        foreach(ITypeFramework nodeSuperType in nodeType.superOrSameTypes)
                                        {
#if MONO_MULTIDIMARRAY_WORKAROUND
                                            vstructs[((nodeSuperType.typeID * dim1size + edgeSuperTypeID) * dim2size + targetSuperTypeID) * 2
                                                + (int) LGSPDir.Out] += val;
#else
                                            vstructs[nodeSuperType.TypeID, edgeSuperTypeID, targetSuperTypeID, (int) LGSPDir.Out] += val;
#endif
                                        }
                                        outgoingVCount[edgeSuperTypeID, targetSuperTypeID] = 0;
                                    }
                                }
                            }
                            edge = edge.outNext;
                        }
                        while(edge != outhead);
                    }

                    if(inhead != null)
                    {
                        LGSPEdge edge = inhead;
                        do
                        {
                            ITypeFramework sourceType = edge.source.type;
                            int sourceTypeID = sourceType.typeID;

                            foreach(ITypeFramework edgeSuperType in edge.type.superOrSameTypes)
                            {
                                int edgeSuperTypeID = edgeSuperType.typeID;
                                foreach(ITypeFramework sourceSuperType in sourceType.superOrSameTypes)
                                {
                                    int sourceSuperTypeID = sourceSuperType.typeID;
                                    if(incomingVCount[edgeSuperTypeID, sourceSuperTypeID] > 0)
                                    {
//                                        float val = (float) Math.Log(incomingVCount[edgeSuperTypeID, sourceSuperTypeID]);     // > 1 im if
                                        float val = incomingVCount[edgeSuperTypeID, sourceSuperTypeID];
                                        foreach(ITypeFramework nodeSuperType in nodeType.superOrSameTypes)
#if MONO_MULTIDIMARRAY_WORKAROUND
                                            vstructs[((nodeSuperType.typeID * dim1size + edgeSuperTypeID) * dim2size + sourceSuperTypeID) * 2
                                                + (int) LGSPDir.In] += val;
#else
                                            vstructs[nodeSuperType.TypeID, edgeSuperTypeID, sourceSuperTypeID, (int) LGSPDir.In] += val;
#endif
                                        incomingVCount[edgeSuperTypeID, sourceSuperTypeID] = 0;
                                    }
                                }
                            }
                            edge = edge.inNext;
                        }
                        while(edge != inhead);
                    }
                }

                int numCompatibleNodes = nodeCounts[nodeType.typeID];
                if(numCompatibleNodes != 0)
                {
                    meanOutDegree[nodeType.typeID] /= numCompatibleNodes;
                    meanInDegree[nodeType.typeID] /= numCompatibleNodes;
                }
            }


/*            // Calculate nodeCounts
            foreach(ITypeFramework nodeType in Model.NodeModel.Types)
            {
                foreach(ITypeFramework superType in edgeType.superOrSameTypes)
                    nodeCounts[superType.typeID] += nodesByTypeCounts[nodeType.typeID];
            }*/

            // Calculate edgeCounts
            foreach(ITypeFramework edgeType in Model.EdgeModel.Types)
            {
                foreach(ITypeFramework superType in edgeType.superOrSameTypes)
                    edgeCounts[superType.typeID] += edgesByTypeCounts[edgeType.typeID];
            }
        }
#endif

        /// <summary>
        /// Mature a graph.
        /// This method should be invoked after adding all nodes and edges to the graph.
        /// The backend may implement analyses on the graph to speed up matching etc.
        /// The graph may not be modified by this function.
        /// </summary>
        public override void Mature()
        {
            // Nothing to be done here, yet
        }

        /// <summary>
        /// Does graph-backend dependent stuff.
        /// </summary>
        /// <param name="args">Any kind of paramteres for the stuff to do</param>
        public override void Custom(params object[] args)
        {
            if(args.Length == 0) goto invalidCommand;

            switch((String) args[0])
            {
                case "analyze":
                case "analyze_graph":
                {
                    int startticks = Environment.TickCount;
                    AnalyzeGraph();
                    Console.WriteLine("Graph '{0}' analyzed in {1} ms.", name, Environment.TickCount - startticks);
                    return;
                }

                case "optimizereuse":
                    if(args.Length != 2)
                        throw new ArgumentException("Usage: optimizereuse <bool>\n"
                                + "If <bool> == true, deleted elements may be reused in a rewrite.\n"
                                + "As a result new elements may not be discriminable anymore from\n"
                                + "already deleted elements using object equality, hash maps, etc.");

                    if(!bool.TryParse((String) args[1], out reuseOptimization))
                        throw new ArgumentException("Illegal bool value specified: \"" + (String) args[1] + "\"");
                    return;
            }

invalidCommand:
            throw new ArgumentException("Possible commands:\n"
                + "- analyze: Analyzes the graph. The generated information can then be\n"
                + "     used by Actions implementations to optimize the pattern matching\n"
                + "- optimizereuse: Sets whether deleted elements may be reused in a rewrite\n"
                + "     (default: true)");
        }

        /// <summary>
        /// Duplicates a graph.
        /// The new graph will use the same model and backend as the other graph.
        /// Open transaction data will not be cloned.
        /// </summary>
        /// <param name="newName">Name of the new graph.</param>
        /// <returns>A new graph with the same structure as this graph.</returns>
        public override IGraph Clone(String newName)
        {
            return new LGSPGraph(this, newName);
        }
    }
}
