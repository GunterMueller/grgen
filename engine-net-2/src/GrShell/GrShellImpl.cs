/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.5
 * Copyright (C) 2009 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 */

//#define MATCHREWRITEDETAIL

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using ASTdapter;
using grIO;
using de.unika.ipd.grGen.libGr;
using de.unika.ipd.grGen.lgsp;

namespace de.unika.ipd.grGen.grShell
{
    struct Param        // KeyValuePair<String, String> waere natuerlich schoener... CSharpCC kann aber kein .NET 2.0 ...
    {
        public String Key;
        public String Value;

        public Param(String key, String value)
        {
            Key = key;
            Value = value;
        }
    }

    class ElementDef
    {
        public String ElemName;
        public String VarName;
        public String TypeName;
        public ArrayList Attributes;

        public ElementDef(String elemName, String varName, String typeName, ArrayList attributes)
        {
            ElemName = elemName;
            VarName = varName;
            TypeName = typeName;
            Attributes = attributes;
        }
    }

    class ShellGraph
    {
        public NamedGraph Graph;
        public BaseActions Actions = null;
        public DumpInfo DumpInfo;
        public VCGFlags VcgFlags = VCGFlags.OrientTopToBottom | VCGFlags.EdgeLabels;
        public ParserPackage Parser = null;

        public String BackendFilename;
        public String[] BackendParameters;
        public String ModelFilename;
        public String ActionsFilename = null;

        public ShellGraph(IGraph graph, String backendFilename, String[] backendParameters, String modelFilename)
        {
            Graph = new NamedGraph(graph);
            DumpInfo = new DumpInfo(Graph.GetElementName);
            BackendFilename = backendFilename;
            BackendParameters = backendParameters;
            ModelFilename = modelFilename;
        }
    }

    class GrShellImpl
    {
        public static readonly String VersionString = "GrShell v2.5";

        IBackend curGraphBackend = new LGSPBackend();
        String backendFilename = null;
        String[] backendParameters = null;

        List<ShellGraph> graphs = new List<ShellGraph>();
        ShellGraph curShellGraph = null;

        bool silence = false; // node/edge created successfully messages
        bool cancelSequence = false;
        Debugger debugger = null;
        bool pendingDebugEnable = false;
        String debugLayout = "Orthogonal";

        /// <summary>
        /// Maps layouts to layout option names to their values.
        /// This only reflects the settings made by the user and may even contain illegal entries,
        /// if the options were set before yComp was attached.
        /// </summary>
        Dictionary<String, Dictionary<String, String>> debugLayoutOptions = new Dictionary<String, Dictionary<String, String>>();

        IWorkaround workaround = WorkaroundManager.Workaround;
        public LinkedList<GrShellTokenManager> TokenSourceStack = new LinkedList<GrShellTokenManager>();

        public GrShellImpl()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
        }

        public bool OperationCancelled { get { return cancelSequence; } }
        public IWorkaround Workaround { get { return workaround; } }
        public bool InDebugMode { get { return debugger != null && !debugger.ConnectionLost; } }

        public static void PrintVersion()
        {
            Console.WriteLine(VersionString + " ($Revision$)");
        }

        private bool BackendExists()
        {                                       
            if(curGraphBackend == null)
            {
                Console.WriteLine("No backend. Select a backend, first.");
                return false;
            }
            return true;
        }

        private bool GraphExists()
        {
            if(curShellGraph == null || curShellGraph.Graph == null)
            {
                Console.WriteLine("No graph. Make a new graph, first.");
                return false;
            }
            return true;
        }

        private bool ActionsExists()
        {
            if(curShellGraph == null || curShellGraph.Actions == null)
            {
                Console.WriteLine("No actions. Select an actions object, first.");
                return false;
            }
            return true;
        }

        public ShellGraph CurrentShellGraph { get { return curShellGraph; } }

        public NamedGraph CurrentGraph
        {
            get
            {
                if(!GraphExists()) return null;
                return curShellGraph.Graph;
            }
        }

        public BaseActions CurrentActions
        {
            get
            {
                if (!ActionsExists()) return null;
                return curShellGraph.Actions;
            }
        }

        public bool Silence
        {
            get
            {
                return silence;
            }
            set
            {
                silence = value;
                if (silence) Console.WriteLine("Disabled \"new node/edge created successfully\"-messages");
                else Console.WriteLine("Enabled \"new node/edge created successfully\"-messages");
            }
        }

        public IGraphElement GetElemByVar(String varName)
        {
            if(!GraphExists()) return null;
            object elem = curShellGraph.Graph.GetVariableValue(varName);
            if(elem == null)
            {
                Console.WriteLine("Unknown variable: \"{0}\"", varName);
                return null;
            }
            if(!(elem is IGraphElement))
            {
                Console.WriteLine("\"{0}\" is not a graph element!", varName);
                return null;
            }
            return (IGraphElement) elem;
        }

        public IGraphElement GetElemByVarOrNull(String varName)
        {
            if(!GraphExists()) return null;
            return curShellGraph.Graph.GetVariableValue(varName) as IGraphElement;
        }

        public IGraphElement GetElemByName(String elemName)
        {
            if(!GraphExists()) return null;
            IGraphElement elem = curShellGraph.Graph.GetGraphElement(elemName);
            if(elem == null)
            {
                Console.WriteLine("Unknown graph element: \"{0}\"", elemName);
                return null;
            }
            return elem;
        }

        public INode GetNodeByVar(String varName)
        {
            IGraphElement elem = GetElemByVar(varName);
            if(elem == null) return null;
            if(!(elem is INode))
            {
                Console.WriteLine("\"{0}\" is not a node!", varName);
                return null;
            }
            return (INode) elem;
        }

        public INode GetNodeByName(String elemName)
        {
            IGraphElement elem = GetElemByName(elemName);
            if(elem == null) return null;
            if(!(elem is INode))
            {
                Console.WriteLine("\"{0}\" is not a node!", elemName);
                return null;
            }
            return (INode) elem;
        }

        public IEdge GetEdgeByVar(String varName)
        {
            IGraphElement elem = GetElemByVar(varName);
            if(elem == null) return null;
            if(!(elem is IEdge))
            {
                Console.WriteLine("\"{0}\" is not an edge!", varName);
                return null;
            }
            return (IEdge) elem;
        }

        public IEdge GetEdgeByName(String elemName)
        {
            IGraphElement elem = GetElemByName(elemName);
            if(elem == null) return null;
            if(!(elem is IEdge))
            {
                Console.WriteLine("\"{0}\" is not an edge!", elemName);
                return null;
            }
            return (IEdge) elem;
        }

        public NodeType GetNodeType(String typeName)
        {
            if(!GraphExists()) return null;
            NodeType type = curShellGraph.Graph.Model.NodeModel.GetType(typeName);
            if(type == null)
            {
                Console.WriteLine("Unknown node type: \"{0}\"", typeName);
                return null;
            }
            return type;
        }

        public EdgeType GetEdgeType(String typeName)
        {
            if(!GraphExists()) return null;
            EdgeType type = curShellGraph.Graph.Model.EdgeModel.GetType(typeName);
            if(type == null)
            {
                Console.WriteLine("Unknown edge type: \"{0}\"", typeName);
                return null;
            }
            return type;
        }

        public ShellGraph GetShellGraph(String graphName)
        {
            foreach(ShellGraph shellGraph in graphs)
                if(shellGraph.Graph.Name == graphName)
                    return shellGraph;

            Console.WriteLine("Unknown graph: \"{0}\"", graphName);
            return null;
        }

        public ShellGraph GetShellGraph(int index)
        {
            if((uint) index >= (uint) graphs.Count)
                Console.WriteLine("Graph index out of bounds!");

            return graphs[index];
        }

        public void HandleSequenceParserRuleException(SequenceParserRuleException ex)
        {
            IAction action = ex.Action;
            if(action == null)
            {
                Console.WriteLine("Unknown rule: \"{0}\"", ex.RuleName);
                return;
            }
            switch(ex.Kind)
            {
                case SequenceParserError.BadNumberOfParametersOrReturnParameters:
                    if(action.RulePattern.Inputs.Length != ex.NumGivenInputs && action.RulePattern.Outputs.Length != ex.NumGivenOutputs)
                        Console.WriteLine("Wrong number of parameters and return values for action \"" + ex.RuleName + "\"!");
                    else if(action.RulePattern.Inputs.Length != ex.NumGivenInputs)
                        Console.WriteLine("Wrong number of parameters for action \"" + ex.RuleName + "\"!");
                    else if(action.RulePattern.Outputs.Length != ex.NumGivenOutputs)
                        Console.WriteLine("Wrong number of return values for action \"" + ex.RuleName + "\"!");
                    else
                        goto default;
                    break;

                case SequenceParserError.BadParameter:
                    Console.WriteLine("The " + (ex.BadParamIndex + 1) + ". parameter is not valid for action \"" + ex.RuleName + "\"!");
                    break;

                case SequenceParserError.RuleNameUsedByVariable:
                    Console.WriteLine("The name of the variable conflicts with the name of action \"" + ex.RuleName + "\"!");
                    return;

                case SequenceParserError.VariableUsedWithParametersOrReturnParameters:
                    Console.WriteLine("The variable \"" + ex.RuleName + "\" may neither receive parameters nor return values!");
                    return;

                default:
                    throw new ArgumentException("Invalid error kind: " + ex.Kind);
            }

            Console.Write("Prototype: {0}", ex.RuleName);
            if(action.RulePattern.Inputs.Length != 0)
            {
                Console.Write("(");
                bool first = true;
                foreach(GrGenType type in action.RulePattern.Inputs)
                {
                    Console.Write("{0}{1}", first ? "" : ", ", type.Name);
                    first = false;
                }
                Console.Write(")");
            }
            if(action.RulePattern.Outputs.Length != 0)
            {
                Console.Write(" : (");
                bool first = true;
                foreach(GrGenType type in action.RulePattern.Outputs)
                {
                    Console.Write("{0}{1}", first ? "" : ", ", type.Name);
                    first = false;
                }
                Console.Write(")");
            }
            Console.WriteLine();
        }

        public IAction GetAction(String actionName, int numParams, int numReturns, bool retSpecified)
        {
            if(!ActionsExists()) return null;
            IAction action = curShellGraph.Actions.GetAction(actionName);
            if(action == null)
            {
                Console.WriteLine("Unknown action: \"{0}\"", actionName);
                return null;
            }
            if(action.RulePattern.Inputs.Length != numParams || action.RulePattern.Outputs.Length != numReturns && retSpecified)
            {
                if(action.RulePattern.Inputs.Length != numParams && action.RulePattern.Outputs.Length != numReturns)
                    Console.WriteLine("Wrong number of parameters and return values for action \"{0}\"!", actionName);
                else if(action.RulePattern.Inputs.Length != numParams)
                    Console.WriteLine("Wrong number of parameters for action \"{0}\"!", actionName);
                else
                    Console.WriteLine("Wrong number of return values for action \"{0}\"!", actionName);
                Console.Write("Prototype: {0}", actionName);
                if(action.RulePattern.Inputs.Length != 0)
                {
                    Console.Write("(");
                    bool first = true;
                    foreach(GrGenType type in action.RulePattern.Inputs)
                    {
                        Console.Write("{0}{1}", first ? "" : ",", type.Name);
                    }
                    Console.Write(")");
                }
                if(action.RulePattern.Outputs.Length != 0)
                {
                    Console.Write(" : (");
                    bool first = true;
                    foreach(GrGenType type in action.RulePattern.Outputs)
                    {
                        Console.Write("{0}{1}", first ? "" : ",", type.Name);
                    }
                    Console.Write(")");
                }
                Console.WriteLine();
                return null;
            }
            return action;
        }

        #region Help text methods

        public void Help(List<String> commands)
        {
            if(commands.Count != 0)
            {
                switch(commands[0])
                {
                    case "new":
                        HelpNew(commands);
                        return;

                    case "select":
                        HelpSelect(commands);
                        return;

                    case "delete":
                        HelpDelete(commands);
                        return;

                    case "show":
                        HelpShow(commands);
                        return;

                    case "debug":
                        HelpDebug(commands);
                        return;

                    case "dump":
                        HelpDump(commands);
                        return;

                    case "map":
                        HelpMap(commands);
                        return;

                    case "set":
                        HelpSet(commands);
                        return;

                    case "custom":
                        HelpCustom(commands);
                        return;

                    default:
                        Console.WriteLine("No further help available.\n");
                        break;
                }
            }

            // Not mentioned: open graph, grs

            Console.WriteLine("\nList of available commands:\n"
                + " - new ...                   Creation commands\n"
                + " - select ...                Selection commands\n"
                + " - include <filename>        Includes and executes the given .grs-script\n"
                + " - silence (on | off)        Switches \"new ... created\" messages on/off\n"
                + " - delete ...                Deletes something\n"
                + " - clear graph [<graph>]     Clears the current or the given graph\n"
                + " - show ...                  Shows something\n"
                + " - node type <n1> is <n2>    Tells whether the node n1 has a type compatible\n"
                + "                             to the type of n2\n"
                + " - edge type <e1> is <e2>    Tells whether the edge e1 has a type compatible\n"
                + "                             to the type of e2\n"
                + " - debug ...                 Debugging related commands\n"
                + " - xgrs <xgrs>               Executes the given extended graph rewrite sequence\n"
                + " - validate xgrs <xgrs>      Validates the current graph according to the\n"
                + "                             given XGRS (true = valid)\n"
                + " - validate [strict]         Validates the current graph according to the\n"
                + "                             connection assertions given by the graph model.\n"
                + "                             In strict mode, all graph elements have to be\n"
                + "                             mentioned as part of a connection assertion\n"
                + " - dump ...                  Dump related commands\n"
                + " - save graph <filename>     Saves the current graph as a GrShell script\n"
                + " - export <filename>         Exports the current graph. The extension states\n"
                + "                             which format shall be used.\n"
                + " - import <filename>+        Imports a graph model and/or a graph instance\n"
                + " - echo <text>               Writes the given text to the console\n"
                + " - custom graph ...          Graph backend specific commands\n"
                + " - custom actions ...        Action backend specific commands\n"
                + " - redirect emit <filename>  Redirects the GrGen emit instructions to a file\n"
                + " - sync io                   Writes out all files (grIO framework)\n"
                + " - parse file <filename>     Parses the given file (ASTdapter framework)\n"
                + " - parse <text>              Parses the given string (ASTdapter framework)\n"
                + " - randomseed (time|<seed>)  Sets the seed of the random number generator\n"
                + "                             to the current time or the given integer\n"
                + " - <var> = allocvisitflag    Allocates a visitor flag and assigns it to var\n"
                + " - [<var>=]isvisited <elem> <id>  Tells whether the given element is marked as\n"
                + "                                  visited for the given visitor and optionally\n"
                + "                                  assigns the result to var\n"
                + " - setvisited <elem> <id> <vis>   Marks the given element according to vis\n"
                + "                                  for the given visitor id. vis may be true,\n"
                + "                                  false, or a boolean variable\n"
                + " - freevisitflag <id>        Frees the given visitor flag\n"
                + " - resetvisitflag <id>       Unmarks all graph elements for the given visitor\n"
                + " - map ...                   Map related commands\n"
                + " - set ...                   Set related commands\n"
                + " - <elem>.<member>           Shows the given graph element member\n"
                + " - <elem>.<member> = <val>   Sets the value of the given element member\n"
                + " - <var> = new map <t1> <t2> Creates a new map<t1, t2> variable\n"
                + " - <var> = new set <t1>      Creates a new set<t1> variable\n"
                + " - <var> = <expr>            Assigns var the given expression.\n"
                + "                             Currently the expression may be:\n"
                + "                               - null\n"
                + "                               - <elem>\n"
                + "                               - <elem>.<member>\n"
                + "                               - <text>\n"
                + "                               - <number> (integer or floating point)\n"
                + "                               - true or false\n"
                + " - ! <command>               Executes the given system command\n"
                + " - help <command>*           Displays this help or help about a command\n"
                + " - exit | quit               Exits the GrShell\n");
        }

        public void HelpNew(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"new\":\n"
                + " - new graph <filename> [<graphname>]\n"
                + "   Creates a graph from the given .gm or .grg file and optionally\n"
                + "   assigns it the given name.\n\n"
                + " - new [<var>][:<type>['('[$=<name>,][<attributes>]')']]\n"
                + "   Creates a node of the given type, name, and attributes and\n"
                + "   assigns it to the given variable.\n"
                + "   Examples:\n"
                + "     - new :Process\n"
                + "     - new proc1:Process($=\"first process\", speed=4.6, id=300)\n\n"
                + " - new <srcNode> -[<var>][:<type>['('[$=<name>,][<attributes>]')']]-> <tgtNode>\n"
                + "   Creates an edge of the given type, name, and attributes from srcNode\n"
                + "   to tgtNode and assigns it to the given variable.\n"
                + "   Examples:\n"
                + "     - new n1 --> n2\n"
                + "     - new proc1 -:next-> proc2\n"
                + "     - new proc1 -req:request(amount=5)-> res1\n");
        }

        public void HelpSelect(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"select\":\n"
                + " - select backend <dllname> [<paramlist>]\n"
                + "   Selects the backend to be used to create graphs.\n"
                + "   Defaults to the lgspBackend.\n\n"
                + " - select graph <name>\n"
                + "   Selects the given already loaded graph.\n\n"
                + " - select actions <dllname>\n"
                + "   Selects the actions assembly for the current graph.\n\n"
                + " - select parser <dllname> <startmethod>\n"
                + "   Selects the ANTLR parser assembly and the name of the start symbol method\n"
                + "   (ASTdapter framework)\n");
        }

        public void HelpDelete(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"delete\":\n"
                + " - delete node <node>\n"
                + "   Deletes the given node from the current graph.\n\n"
                + " - delete edge <edge>\n"
                + "   Deletes the given edge from the current graph.\n\n"
                + " - delete graph [<graph>]\n"
                + "   Deletes the current or the given graph.\n");
        }

        public void HelpShow(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"show\":\n"
                + " - show (nodes|edges) [[only] <type>]\n"
                + "   Shows all nodes/edges, the nodes/edges compatible to the given type, or\n"
                + "   only nodes/edges of the exact type.\n\n"
                + " - show num (nodes|edges) [[only] <type>]\n"
                + "   Shows the number of elements as above.\n\n"
                + " - show (node|edge) types\n"
                + "   Shows the node/edge types of the current graph model.\n\n"
                + " - show (node|edge) (sub|super) types <type>\n"
                + "   Shows the sub/super types of the given type.\n\n"
                + " - show (node|edge) attributes [[only] <type>]\n"
                + "   Shows all attributes of all types, of all types compatible to the given\n"
                + "   type, or only of the given type.\n\n"
                + " - show (node|edge) <elem>\n"
                + "   Shows the attributes of the given node/edge.\n\n"
                + " - show var <var>\n"
                + "   Shows the value of the given variable.\n\n"
                + " - show graph <program> [<arguments>]\n"
                + "   Shows the current graph in VCG format with the given program.\n"
                + "   The name of the temporary VCG file will always be the first parameter.\n"
                + "   Example: show graph ycomp\n\n"
                + " - show graphs\n"
                + "   Lists the names of the currently loaded graphs.\n\n"
                + " - show actions\n"
                + "   Lists the available actions associated with the current graph.\n\n"
                + " - show backend\n"
                + "   Shows the name of the current backend and its parameters.\n");
        }

        public void HelpDebug(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            // Not mentioned: debug grs

            Console.WriteLine("\nList of available commands for \"debug\":\n"
                + " - debug apply ['(' <retvars> ')' = ] <rule> ['(' <params> ')']\n"
                + "   Applies the given rule using the parameters and assigning results.\n"
                + "   For all '?'s used as parameters you will be asked to select the\n"
                + "   according element in yComp.\n\n"
                + " - debug xgrs <xgrs>\n"
                + "   Debugs the given XGRS.\n\n"
                + " - debug (enable | disable)\n"
                + "   Enables/disables debug mode.\n\n"
                + " - debug layout\n"
                + "   Forces yComp to relayout the graph.\n\n"
                + " - debug set layout [<algo>]\n"
                + "   Selects the layout algorithm for yComp. If algorithm is not given,\n"
                + "   all available layout algorithms are listed.\n\n"
                + " - debug get layout options\n"
                + "   Lists all available layout options for the current layout algorithm.\n\n"
                + " - debug set layout option <name> <value>\n"
                + "   Sets the value of the given layout option.\n\n");
        }

        public void HelpDump(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"dump\":\n"
                + " - dump graph <filename>\n"
                + "   Dumps the current graph to a file in VCG format.\n\n"
                + " - dump set node [only] <type> <property> [<value>]\n"
                + "   Sets dump properties for the given or all compatible node types.\n"
                + "   If no value is given, a list of possible values is shown.\n"
                + "   Supported properties:\n"
                + "    - color\n"
                + "    - bordercolor\n"
                + "    - shape\n"
                + "    - textcolor\n"
                + "    - labels (on | off | <constanttext>)\n\n"
                + " - dump set edge [only] <type> <property> [<value>]\n"
                + "   Sets dump properties for the given or all compatible edge types.\n"
                + "   If no value is given, a list of possible values is shown.\n"
                + "   Supported properties:\n"
                + "    - color\n"
                + "    - textcolor\n"
                + "    - labels (on | off | <constanttext>)\n\n"
                + " - dump add (node | edge) [only] <type> exclude\n"
                + "   Excludes the given or compatible types from dumping.\n\n"
                + " - dump add node [only] <nodetype> group [by [hidden] <mode>\n"
                + "                [[only] <edgetype> [with [only] adjnodetype]]]\n"
                + "   Declares the given nodetype as a group node type.\n"
                + "   The mode determines by which edges the adjacent nodes are grouped\n"
                + "   into the group node. The available modes are:\n"
                + "    - no\n"
                + "    - incoming\n"
                + "    - outgoing\n"
                + "    - any\n"
                + "   Furthermore, the edge types and the adjacent node types can be restricted.\n\n"
                + " - dump add (node | edge) [only] <type> infotag <member>\n"
                + "   Adds an info tag to the given or compatible node types, which is displayed\n"
                + "   as \"<member> = <value>\" under the label of the graph element.\n\n"
                + " - dump add (node | edge) [only] <type> shortinfotag <member>\n"
                + "   Adds an info tag to the given or compatible node types, which is displayed\n"
                + "   as \"<value>\" under the label of the graph element.\n");
        }

        public void HelpMap(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"map\":\n"
                + " - map (<elem>.<member> | <var>) add <keyExpr> <valExpr>\n"
                + "   Sets the value of the map for the given key.\n\n"
                + " - map (<elem>.<member> | <var>) remove <keyExpr>\n"
                + "   Removes the given key from the map.\n\n"
                + " - map (<elem>.<member> | <var>) size\n"
                + "   Prints the size of the map.\n");
        }

        public void HelpSet(List<String> commands)
        {
            if(commands.Count > 1)
            {
                Console.WriteLine("\nNo further help available.");
            }

            Console.WriteLine("\nList of available commands for \"set\":\n"
                + " - set (<elem>.<member> | <var>) add <keyExpr>\n"
                + "   Marks the given key as part of the set.\n\n"
                + " - set (<elem>.<member> | <var>) remove <keyExpr>\n"
                + "   Removes the given key from the set.\n\n"
                + " - set (<elem>.<member> | <var>) size\n"
                + "   Prints the size of the set.\n");
        }

        public void HelpCustom(List<String> commands)
        {
            if(commands.Count > 1)
            {
                switch(commands[1])
                {
                    case "graph":
                        CustomGraph(new List<String>());
                        return;

                    case "actions":
                        CustomActions(new List<String>());
                        return;

                    default:
                        Console.WriteLine("\nNo further help available.");
                        break;
                }
            }

            Console.WriteLine("\nList of available commands for \"custom\":\n"
                + " - custom graph:\n\n");
            CustomGraph(new List<String>());
            Console.WriteLine("\n - custom actions:\n\n");
            CustomActions(new List<String>());
            Console.WriteLine();
        }

        #endregion Help text methods

        public void SyncIO()
        {
            Infrastructure.Flush(curShellGraph.Graph);
        }

        public void ExecuteCommandLine(String cmdLine)
        {
            // treat first word as the filename and the rest as arguments

            cmdLine = cmdLine.Trim();
            int firstSpace = cmdLine.IndexOf(' ');
            String filename;
            String args;
            if(firstSpace == -1)
            {
                filename = cmdLine;
                args = "";
            }
            else
            {
                filename = cmdLine.Substring(0, firstSpace);
                args = cmdLine.Substring(firstSpace + 1, cmdLine.Length - firstSpace - 1);
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, args);
                Process cmdProcess = Process.Start(startInfo);
                cmdProcess.WaitForExit();
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to execute file \"" + filename + "\": " + e.Message);
            }
        }

        public bool Include(GrShell grShell, String filename)
        {
            try
            {
                using(TextReader reader = new StreamReader(filename))
                {
                    SimpleCharStream charStream = new SimpleCharStream(reader);
                    GrShellTokenManager tokenSource = new GrShellTokenManager(charStream);
                    TokenSourceStack.AddFirst(tokenSource);
                    bool oldShowPrompt = grShell.ShowPrompt;
                    grShell.ShowPrompt = false;
                    try
                    {
                        grShell.ReInit(tokenSource);
                        while(!grShell.Quit && !grShell.Eof)
                        {
                            if(!grShell.ParseShellCommand())
                                return false;
                        }
                        grShell.Eof = false;
                    }
                    finally
                    {
                        TokenSourceStack.RemoveFirst();
                        grShell.ReInit(TokenSourceStack.First.Value);
                        grShell.ShowPrompt = oldShowPrompt;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error during include of \"" + filename + "\": " + e.Message);
                return false;
            }
            return true;
        }

        public void Quit()
        {
            if(InDebugMode)
                SetDebugMode(false);

            Console.WriteLine("Bye!\n");
        }

        public void Cleanup()
        {
            foreach(ShellGraph shellGraph in graphs)
            {
                if(shellGraph.Graph.EmitWriter != Console.Out)
                {
                    shellGraph.Graph.EmitWriter.Close();
                    shellGraph.Graph.EmitWriter = Console.Out;
                }
            }
        }

        #region Model operations
        public bool SelectBackend(String assemblyName, ArrayList parameters)
        {
            // replace wrong directory separator chars by the right ones
            if(Path.DirectorySeparatorChar != '\\')
                assemblyName = assemblyName.Replace('\\', Path.DirectorySeparatorChar);

            try
            {
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type backendType = null;
                foreach(Type type in assembly.GetTypes())
                {
                    if(!type.IsClass || type.IsNotPublic) continue;
                    if(type.GetInterface("IBackend") != null)
                    {
                        if(backendType != null)
                        {
                            Console.WriteLine("The given backend contains more than one IBackend implementation!");
                            return false;
                        }
                        backendType = type;
                    }
                }
                if(backendType == null)
                {
                    Console.WriteLine("The given backend doesn't contain an IBackend implementation!");
                    return false;
                }
                curGraphBackend = (IBackend) Activator.CreateInstance(backendType);
                backendFilename = assemblyName;
                backendParameters = (String[]) parameters.ToArray(typeof(String));
                Console.WriteLine("Backend selected successfully.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to load backend: {0}", ex.Message);
                return false;
            }
            return true;
        }

        public bool NewGraph(String specFilename, String graphName)
        {
            if(!BackendExists()) return false;

            // replace wrong directory separator chars by the right ones
            if(Path.DirectorySeparatorChar != '\\')
                specFilename = specFilename.Replace('\\', Path.DirectorySeparatorChar);

            if(specFilename.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || specFilename.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                IGraph graph;
                try
                {
                    graph = curGraphBackend.CreateGraph(specFilename, graphName, backendParameters);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unable to create graph with model \"{0}\":\n{1}", specFilename, e.Message);
                    return false;
                }
                try
                {
                    curShellGraph = new ShellGraph(graph, backendFilename, backendParameters, specFilename);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Unable to create new graph: {0}", ex.Message);
                    return false;
                }
                graphs.Add(curShellGraph);
                Console.WriteLine("New graph \"{0}\" of model \"{1}\" created.", graphName, graph.Model.ModelName);
            }
            else
            {
                if(!File.Exists(specFilename))
                {
                    String ruleFilename = specFilename + ".grg";
                    if(!File.Exists(ruleFilename))
                    {
                        String gmFilename = specFilename + ".gm";
                        if(!File.Exists(gmFilename))
                        {
                            Console.WriteLine("The specification file \"" + specFilename + "\" or \"" + ruleFilename + "\" or \"" + gmFilename + "\" does not exist!");
                            return false;
                        }
                        else specFilename = gmFilename;
                    }
                    else specFilename = ruleFilename;
                }

                if(specFilename.EndsWith(".gm", StringComparison.OrdinalIgnoreCase))
                {
                    IGraph graph;
                    try
                    {
                        graph = curGraphBackend.CreateFromSpec(specFilename, graphName);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Unable to create graph from specification file \"{0}\":\n{1}", specFilename, e.Message);
                        return false;
                    }
                    try
                    {
                        curShellGraph = new ShellGraph(graph, backendFilename, backendParameters, specFilename);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine("Unable to create new graph: {0}", ex.Message);
                        return false;
                    }
                    graphs.Add(curShellGraph);
                    Console.WriteLine("New graph \"{0}\" created from spec file \"{1}\".", graphName, specFilename);
                }
                else if(specFilename.EndsWith(".grg", StringComparison.OrdinalIgnoreCase))
                {
                    IGraph graph;
                    BaseActions actions;
                    try
                    {
                        curGraphBackend.CreateFromSpec(specFilename, graphName, out graph, out actions);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Unable to create graph from specification file \"{0}\":\n{1}", specFilename, e.Message);
                        return false;
                    }
                    try
                    {
                        curShellGraph = new ShellGraph(graph, backendFilename, backendParameters, specFilename);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine("Unable to create new graph: {0}", ex.Message);
                        return false;
                    }
                    curShellGraph.Actions = actions;
                    graphs.Add(curShellGraph);
                    Console.WriteLine("New graph \"{0}\" and actions created from spec file \"{1}\".", graphName, specFilename);
                }
                else
                {
                    Console.WriteLine("Unknown specification file format of file: \"" + specFilename + "\"");
                    return false;
                }
            }

            if(pendingDebugEnable)
                SetDebugMode(true);
            return true;
        }

        public bool OpenGraph(String modelFilename, String graphName)
        {
            if(!BackendExists()) return false;

            // replace wrong directory separator chars by the right ones
            if(Path.DirectorySeparatorChar != '\\')
                modelFilename = modelFilename.Replace('\\', Path.DirectorySeparatorChar);
            
            IGraph graph;
            try
            {
                graph = curGraphBackend.OpenGraph(modelFilename, graphName, backendParameters);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to open graph with model \"{0}\":\n{1}", modelFilename, e.Message);
                return false;
            }
            try
            {
                curShellGraph = new ShellGraph(graph, backendFilename, backendParameters, modelFilename);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to open graph: {0}", ex.Message);
                return false;
            }
            graphs.Add(curShellGraph);

            if(pendingDebugEnable)
                SetDebugMode(true);
            return true;
        }

        public void SelectGraph(ShellGraph shellGraph)
        {
            curShellGraph = shellGraph ?? curShellGraph;
        }

        public bool SelectActions(String actionFilename)
        {
            if(!GraphExists()) return false;

            // replace wrong directory separator chars by the right ones
            if(Path.DirectorySeparatorChar != '\\')
                actionFilename = actionFilename.Replace('\\', Path.DirectorySeparatorChar);

            try
            {
                curShellGraph.Actions = curShellGraph.Graph.LoadActions(actionFilename);
                curShellGraph.ActionsFilename = actionFilename;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to load the actions from \"{0}\":\n{1}", actionFilename, ex.Message);
                return false;
            }
            return true;
        }
        
        public bool SelectParser(String parserAssemblyName, String mainMethodName)
        {
            if(!GraphExists()) return false;

            // replace wrong directory separator chars by the right ones
            if(Path.DirectorySeparatorChar != '\\')
                parserAssemblyName = parserAssemblyName.Replace('\\', Path.DirectorySeparatorChar);

            try
            {
                curShellGraph.Parser = new ParserPackage(parserAssemblyName, mainMethodName);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to load parser from \"" + parserAssemblyName + "\":\n" + ex.Message);
                return false;
            }
            return true;
        }
        #endregion Model operations

        #region "new" graph element commands
        public INode NewNode(ElementDef elemDef)
        {
            if(!GraphExists()) return null;

            NodeType nodeType;
            if(elemDef.TypeName != null)
            {
                nodeType = curShellGraph.Graph.Model.NodeModel.GetType(elemDef.TypeName);
                if(nodeType == null)
                {
                    Console.WriteLine("Unknown node type: \"" + elemDef.TypeName + "\"");
                    return null;
                }
                if(nodeType.IsAbstract)
                {
                    Console.WriteLine("Abstract node type \"" + elemDef.TypeName + "\" may not be instantiated!");
                    return null;
                }
            }
            else nodeType = curShellGraph.Graph.Model.NodeModel.RootType;

            if(elemDef.Attributes != null && !CheckAttributes(nodeType, elemDef.Attributes)) return null;

            INode node;
            try
            {
                node = curShellGraph.Graph.AddNode(nodeType, elemDef.VarName, elemDef.ElemName);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Unable to create new node: {0}", e.Message);
                return null;
            }
            if(node == null)
            {
                Console.WriteLine("Creation of new node failed.");
                return null;
            }


            if(elemDef.Attributes != null)
            {
                if(!SetAttributes(node, elemDef.Attributes))
                {
                    Console.WriteLine("Unexpected failure: Unable to set node attributes inspite of check?!");
                    curShellGraph.Graph.Remove(node);
                    return null;
                }
            }

            if (!silence)
            {
                if (elemDef.ElemName == null)
                    Console.WriteLine("New node \"{0}\" of type \"{1}\" has been created.", curShellGraph.Graph.GetElementName(node), node.Type.Name);
                else
                    Console.WriteLine("New node \"{0}\" of type \"{1}\" has been created.", elemDef.ElemName, node.Type.Name);
            }

            return node;
        }

        public IEdge NewEdge(ElementDef elemDef, INode node1, INode node2)
        {
            if(node1 == null || node2 == null) return null;

            EdgeType edgeType;
            if(elemDef.TypeName != null)
            {
                edgeType = curShellGraph.Graph.Model.EdgeModel.GetType(elemDef.TypeName);
                if(edgeType == null)
                {
                    Console.WriteLine("Unknown edge type: \"" + elemDef.TypeName + "\"");
                    return null;
                }
                if(edgeType.IsAbstract)
                {
                    Console.WriteLine("Abstract edge type \"" + elemDef.TypeName + "\" may not be instantiated!");
                    return null;
                }
            }
            else edgeType = curShellGraph.Graph.Model.EdgeModel.RootType;

            if(elemDef.Attributes != null && !CheckAttributes(edgeType, elemDef.Attributes)) return null;

            IEdge edge;
            try
            {
                edge = curShellGraph.Graph.AddEdge(edgeType, node1, node2, elemDef.VarName, elemDef.ElemName);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Unable to create new edge: {0}", e.Message);
                return null;
            }
            if(edge == null)
            {
                Console.WriteLine("Creation of new edge failed.");
                return null;
            }

            if(elemDef.Attributes != null)
            {
                if(!SetAttributes(edge, elemDef.Attributes))
                {
                    Console.WriteLine("Unexpected failure: Unable to set edge attributes inspite of check?!");
                    curShellGraph.Graph.Remove(edge);
                    return null;
                }
            }

            if (!silence)
            {
                if (elemDef.ElemName == null)
                    Console.WriteLine("New edge \"{0}\" of type \"{1}\" has been created.", curShellGraph.Graph.GetElementName(edge), edge.Type.Name);
                else
                    Console.WriteLine("New edge \"{0}\" of type \"{1}\" has been created.", elemDef.ElemName, edge.Type.Name);
            }
            
            return edge;
        }

        private bool CheckAttributes(GrGenType type, ArrayList attributes)
        {
            foreach(Param par in attributes)
            {
                AttributeType attrType = type.GetAttributeType(par.Key);
                if(attrType == null)
                {
                    Console.WriteLine("Type \"{0}\" does not have an attribute \"{1}\"!", type.Name, par.Key);
                    return false;
                }
                switch(attrType.Kind)
                {
                    case AttributeKind.BooleanAttr:
                        if(par.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                            break;
                        else if(par.Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                            break;
                        Console.WriteLine("Attribute \"{0}\" must be either \"true\" or \"false\"!", par.Key);
                        return false;
                    case AttributeKind.EnumAttr:
                    {
                        int val;
                        if(Int32.TryParse(par.Value, out val)) break;
                        bool ok = false;
                        foreach(EnumMember member in attrType.EnumType.Members)
                        {
                            if(par.Value == member.Name)
                            {
                                ok = true;
                                break;
                            }
                        }
                        if(ok) break;

                        Console.WriteLine("Attribute \"{0}\" must be one of the following values:", par.Key);
                        foreach(EnumMember member in attrType.EnumType.Members)
                            Console.WriteLine(" - {0} = {1}", member.Name, member.Value);

                        return false;
                    }
                    case AttributeKind.IntegerAttr:
                    {
                        int val;
                        if(Int32.TryParse(par.Value, out val)) break;

                        Console.WriteLine("Attribute \"{0}\" must be an integer!", par.Key);
                        return false;
                    }
                    case AttributeKind.StringAttr:
                        break;
                    case AttributeKind.FloatAttr:
                    {
                        float val;
                        if(Single.TryParse(par.Value, out val)) break;

                        Console.WriteLine("Attribute \"{0}\" must be a floating point number!", par.Key);
                        return false;
                    }
                    case AttributeKind.DoubleAttr:
                    {
                        double val;
                        if(Double.TryParse(par.Value, out val)) break;

                        Console.WriteLine("Attribute \"{0}\" must be a floating point number!", par.Key);
                        return false;
                    }
                    case AttributeKind.ObjectAttr:
                    {
                        Console.WriteLine("Attribute \"" + par.Key + "\" is an object type attribute!\n"
                            + "It is not possible to assign a value to an object type attribute!");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool SetAttributes(IGraphElement elem, ArrayList attributes)
        {
            foreach(Param par in attributes)
            {
                AttributeType attrType = elem.Type.GetAttributeType(par.Key);
                if(attrType == null)
                {
                    Console.WriteLine("Type \"{0}\" does not have an attribute \"{1}\"!", elem.Type.Name, par.Key);
                    return false;
                }
                object value = null;
                switch(attrType.Kind)
                {
                    case AttributeKind.BooleanAttr:
                        if(par.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                            value = true;
                        else if(par.Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                            value = false;
                        else
                        {
                            Console.WriteLine("Attribute {0} must be either \"true\" or \"false\"!", par.Key);
                            return false;
                        }
                        break;
                    case AttributeKind.EnumAttr:
                    {
                        int val;
                        if(Int32.TryParse(par.Value, out val))
                        {
                            value = val;
                        }
                        else
                        {
                            foreach(EnumMember member in attrType.EnumType.Members)
                            {
                                if(par.Value == member.Name)
                                {
                                    value = member.Value;
                                    break;
                                }
                            }
                            if(value == null)
                            {
                                Console.WriteLine("Attribute {0} must be one of the following values:", par.Key);
                                foreach(EnumMember member in attrType.EnumType.Members)
                                    Console.WriteLine(" - {0} = {1}", member.Name, member.Value);

                                return false;
                            }
                        }
                        break;
                    }
                    case AttributeKind.IntegerAttr:
                    {
                        int val;
                        if(!Int32.TryParse(par.Value, out val))
                        {
                            Console.WriteLine("Attribute {0} must be an integer!", par.Key);
                            return false;
                        }
                        value = val;
                        break;
                    }
                    case AttributeKind.StringAttr:
                        value = par.Value;
                        break;
                    case AttributeKind.FloatAttr:
                    {
                        float val;
                        if(!Single.TryParse(par.Value, out val))
                        {
                            Console.WriteLine("Attribute \"{0}\" must be a floating point number!", par.Key);
                            return false;
                        }
                        value = val;
                        break;
                    }
                    case AttributeKind.DoubleAttr:
                    {
                        double val;
                        if(!Double.TryParse(par.Value, out val))
                        {
                            Console.WriteLine("Attribute \"{0}\" must be a floating point number!", par.Key);
                            return false;
                        }
                        value = val;
                        break;
                    }
                    case AttributeKind.ObjectAttr:
                    {
                        Console.WriteLine("Attribute \"" + par.Key + "\" is an object type attribute!\n"
                            + "It is not possible to assign a value to an object type attribute!");
                        return false;
                    }
                    case AttributeKind.SetAttr:
                    {
                        // MAP TODO
                        return false;
                    }
                    case AttributeKind.MapAttr:
                    {
                        // MAP TODO
                        return false;
                    }
                      
                }

                AttributeChangeType changeType = AttributeChangeType.Assign;
                if (elem is INode)
                    curShellGraph.Graph.ChangingNodeAttribute((INode)elem, attrType, changeType, value, null);
                else
                    curShellGraph.Graph.ChangingEdgeAttribute((IEdge)elem, attrType, changeType, value, null);
                elem.SetAttribute(par.Key, value);
            }
            return true;
        }
        #endregion "new" graph element commands

        #region "remove" / "destroy" commands
        public bool Remove(INode node)
        {
            if(node == null) return false;
            try
            {
                curShellGraph.Graph.RemoveEdges(node);
                curShellGraph.Graph.Remove(node);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Unable to remove node: " + e.Message);
                return false;
            }
            return false;
        }

        public bool Remove(IEdge edge)
        {
            if(edge == null) return false;
            curShellGraph.Graph.Remove(edge);
            return true;
        }

        public void ClearGraph(ShellGraph shellGraph, bool shellGraphSpecified)
        {
            if(!shellGraphSpecified)
            {
                if(!GraphExists()) return;
                shellGraph = curShellGraph;
            }
            else if(shellGraph == null) return;

            shellGraph.Graph.Clear();
        }

        public bool DestroyGraph(ShellGraph shellGraph, bool shellGraphSpecified)
        {
            if(!shellGraphSpecified)
            {
                if(!GraphExists()) return false;
                shellGraph = curShellGraph;
            }
            else if(shellGraph == null) return false;

            if(InDebugMode && debugger.CurrentShellGraph == shellGraph) SetDebugMode(false);

            if(shellGraph == curShellGraph)
                curShellGraph = null;
            shellGraph.Graph.DestroyGraph();
            graphs.Remove(shellGraph);

            return true;
        }
        #endregion "remove" commands

        #region "show" commands
        private bool ShowElements<T>(IEnumerable<T> elements) where T : IGraphElement
        {
            if(!elements.GetEnumerator().MoveNext()) return false;

            Console.WriteLine("{0,-20} {1}", "name", "type");
            foreach(IGraphElement elem in elements)
            {
                Console.WriteLine("{0,-20} {1}", curShellGraph.Graph.GetElementName(elem), elem.Type.Name);
            }
            return true;
        }

        public void ShowNodes(NodeType nodeType, bool only)
        {
            if(nodeType == null)
            {
                if(!GraphExists()) return;
                nodeType = curShellGraph.Graph.Model.NodeModel.RootType;
            }

            IEnumerable<INode> nodes = only ? curShellGraph.Graph.GetExactNodes(nodeType)
                : curShellGraph.Graph.GetCompatibleNodes(nodeType);
            if(!ShowElements(nodes))
                Console.WriteLine("There are no nodes " + (only ? "compatible to" : "of") + " type \"" + nodeType.Name + "\"!");
        }

        public void ShowEdges(EdgeType edgeType, bool only)
        {
            if(edgeType == null)
            {
                if(!GraphExists()) return;
                edgeType = curShellGraph.Graph.Model.EdgeModel.RootType;
            }

            IEnumerable<IEdge> edges = only ? curShellGraph.Graph.GetExactEdges(edgeType)
                : curShellGraph.Graph.GetCompatibleEdges(edgeType);
            if(!ShowElements(edges))
                Console.WriteLine("There are no edges of " + (only ? "compatible to" : "of") + " type \"" + edgeType.Name + "\"!");
        }

        public void ShowNumNodes(NodeType nodeType, bool only)
        {
            if(nodeType == null)
            {
                if(!GraphExists()) return;
                nodeType = curShellGraph.Graph.Model.NodeModel.RootType;
            }
            if(only)
                Console.WriteLine("Number of nodes of type \"" + nodeType.Name + "\": "
                    + curShellGraph.Graph.GetNumExactNodes(nodeType));
            else
                Console.WriteLine("Number of nodes compatible to type \"" + nodeType.Name + "\": "
                    + curShellGraph.Graph.GetNumCompatibleNodes(nodeType));
        }

        public void ShowNumEdges(EdgeType edgeType, bool only)
        {
            if(edgeType == null)
            {
                if(!GraphExists()) return;
                edgeType = curShellGraph.Graph.Model.EdgeModel.RootType;
            }
            if(only)
                Console.WriteLine("Number of edges of type \"" + edgeType.Name + "\": "
                    + curShellGraph.Graph.GetNumExactEdges(edgeType));
            else
                Console.WriteLine("Number of edges compatible to type \"" + edgeType.Name + "\": "
                    + curShellGraph.Graph.GetNumCompatibleEdges(edgeType));
        }

        public void ShowNodeTypes()
        {
            if(!GraphExists()) return;

            if(curShellGraph.Graph.Model.NodeModel.Types.Length == 0)
            {
                Console.WriteLine("This model has no node types!");
            }
            else
            {
                Console.WriteLine("Node types:");
                foreach(NodeType type in curShellGraph.Graph.Model.NodeModel.Types)
                    Console.WriteLine(" - \"{0}\"", type.Name);
            }
        }

        public void ShowEdgeTypes()
        {
            if(!GraphExists()) return;

            if(curShellGraph.Graph.Model.EdgeModel.Types.Length == 0)
            {
                Console.WriteLine("This model has no edge types!");
            }
            else
            {
                Console.WriteLine("Edge types:");
                foreach(EdgeType type in curShellGraph.Graph.Model.EdgeModel.Types)
                    Console.WriteLine(" - \"{0}\"", type.Name);
            }
        }

        public void ShowSuperTypes(GrGenType elemType, bool isNode)
        {
            if(elemType == null) return;

            if(!elemType.SuperTypes.GetEnumerator().MoveNext())
            {
                Console.WriteLine((isNode ? "Node" : "Edge") + " type \"" + elemType.Name + "\" has no super types!");
            }
            else
            {
                Console.WriteLine("Super types of " + (isNode ? "node" : "edge") + " type \"" + elemType.Name + "\":");
                foreach(GrGenType type in elemType.SuperTypes)
                    Console.WriteLine(" - \"" + type.Name + "\"");
            }
        }

        public void ShowSubTypes(GrGenType elemType, bool isNode)
        {
            if(elemType == null) return;

            if(!elemType.SuperTypes.GetEnumerator().MoveNext())
            {
                Console.WriteLine((isNode ? "Node" : "Edge") + " type \"" + elemType.Name + "\" has no super types!");
            }
            else
            {
                Console.WriteLine("Sub types of " + (isNode ? "node" : "edge") + " type \"{0}\":", elemType.Name);
                foreach(GrGenType type in elemType.SubTypes)
                    Console.WriteLine(" - \"{0}\"", type.Name);
            }
        }

        internal class ShowGraphParam
        {
            public readonly String ProgramName;
            public readonly String Arguments;
            public readonly String GraphFilename;

            public ShowGraphParam(String programName, String arguments, String graphFilename)
            {
                ProgramName = programName;
                Arguments = arguments;
                GraphFilename = graphFilename;
            }
        }

        /// <summary>
        /// Executes the specified viewer and deletes the dump file after the viewer has exited
        /// </summary>
        /// <param name="obj">A ShowGraphParam object</param>
        private void ShowGraphThread(object obj)
        {
            ShowGraphParam param = (ShowGraphParam) obj;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(param.ProgramName,
                    (param.Arguments == null) ? param.GraphFilename : (param.Arguments + " " + param.GraphFilename));
                Process viewer = Process.Start(startInfo);
                viewer.WaitForExit();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                File.Delete(param.GraphFilename);
            }
        }

        public void ShowGraphWith(String programName, String arguments)
        {
            if(!GraphExists()) return;

            String filename;
            int id = 0;
            do
            {
                filename = "tmpgraph" + id + ".vcg";
                id++;
            }
            while(File.Exists(filename));

            VCGDumper dump = new VCGDumper(filename, curShellGraph.VcgFlags, debugLayout);
            curShellGraph.Graph.Dump(dump, curShellGraph.DumpInfo);
            dump.FinishDump();

            Thread t = new Thread(new ParameterizedThreadStart(ShowGraphThread));
            t.Start(new ShowGraphParam(programName, arguments, filename));
        }

        public void ShowGraphs()
        {
            if(graphs.Count == 0)
            {
                Console.WriteLine("No graphs available.");
                return;
            }
            Console.WriteLine("Graphs:");
            for(int i = 0; i < graphs.Count; i++)
                Console.WriteLine(" - \"" + graphs[i].Graph.Name + "\" (" + i + ")");
        }

        public void ShowActions()
        {
            if(!ActionsExists()) return;

            if(!curShellGraph.Actions.Actions.GetEnumerator().MoveNext())
            {
                Console.WriteLine("No actions available.");
                return;
            }

            Console.WriteLine("Actions:");
            foreach(IAction action in curShellGraph.Actions.Actions)
            {
                Console.Write(" - " + action.Name);
                if(action.RulePattern.Inputs.Length != 0)
                {
                    Console.Write("(");
                    bool isFirst = true;
                    foreach(GrGenType inType in action.RulePattern.Inputs)
                    {
                        if(!isFirst) Console.Write(", ");
                        else isFirst = false;
                        Console.Write(inType.Name);
                    }
                    Console.Write(")");
                }

                if(action.RulePattern.Outputs.Length != 0)
                {
                    Console.Write(" : (");
                    bool isFirst = true;
                    foreach(GrGenType outType in action.RulePattern.Outputs)
                    {
                        if(!isFirst) Console.Write(", ");
                        else isFirst = false;
                        Console.Write(outType.Name);
                    }
                    Console.Write(")");
                }
                Console.WriteLine();
            }
        }

        public void ShowBackend()
        {
            if(!BackendExists()) return;

            Console.WriteLine("Backend name: {0}", curGraphBackend.Name);
            if(!curGraphBackend.ArgumentNames.GetEnumerator().MoveNext())
            {
                Console.WriteLine("This backend has no arguments.");
            }

            Console.WriteLine("Arguments:");
            foreach(String str in curGraphBackend.ArgumentNames)
                Console.WriteLine(" - {0}", str);
        }

        /// <summary>
        /// Displays the attributes from the given type or all types, if typeName is null.
        /// If showAll is false, inherited attributes are not shown (only applies to a given type)
        /// </summary>
        /// <typeparam name="T">An IType interface</typeparam>
        /// <param name="showOnly">If true, only non inherited attributes are shown</param>
        /// <param name="typeName">Type which attributes are to be shown or null to show all attributes of all types</param>
        /// <param name="model">The model to take the attributes from</param>
        private void ShowAvailableAttributes(IEnumerable<AttributeType> attrTypes, GrGenType onlyType)
        {
            bool first = true;
            foreach(AttributeType attrType in attrTypes)
            {
                if(onlyType != null && attrType.OwnerType != onlyType) continue;

                if(first)
                {
                    Console.WriteLine("{0,-24} type::attribute\n", "kind");
                    first = false;
                }

                String kind;
                switch(attrType.Kind)
                {
                    case AttributeKind.IntegerAttr: kind = "int"; break;
                    case AttributeKind.BooleanAttr: kind = "boolean"; break;
                    case AttributeKind.StringAttr: kind = "string"; break;
                    case AttributeKind.EnumAttr: kind = attrType.EnumType.Name; break;
                    case AttributeKind.FloatAttr: kind = "float"; break;
                    case AttributeKind.DoubleAttr: kind = "double"; break;
                    case AttributeKind.ObjectAttr: kind = "object"; break;
                    default: kind = "<INVALID>"; break;
                }
                Console.WriteLine("{0,-24} {1}::{2}", kind, attrType.OwnerType.Name, attrType.Name);
            }
            if(first)
                Console.WriteLine("No attribute types found.");
        }

        public void ShowAvailableNodeAttributes(bool showOnly, NodeType nodeType)
        {
            if(nodeType == null)
                ShowAvailableAttributes(curShellGraph.Graph.Model.NodeModel.AttributeTypes, null);
            else
                ShowAvailableAttributes(nodeType.AttributeTypes, showOnly ? nodeType : null);
        }

        public void ShowAvailableEdgeAttributes(bool showOnly, EdgeType edgeType)
        {
            if(edgeType == null)
                ShowAvailableAttributes(curShellGraph.Graph.Model.EdgeModel.AttributeTypes, null);
            else
                ShowAvailableAttributes(edgeType.AttributeTypes, showOnly ? edgeType : null);
        }

        public AttributeType GetElementAttributeType(IGraphElement elem, String attributeName)
        {
            AttributeType attrType = elem.Type.GetAttributeType(attributeName);
            if(attrType == null)
            {
                Console.WriteLine(((elem is INode) ? "Node" : "Edge") + " \"" + curShellGraph.Graph.GetElementName(elem)
                    + "\" does not have an attribute \"" + attributeName + "\"!");
                return attrType;
            }
            return attrType;
        }

        public void ShowElementAttributes(IGraphElement elem)
        {
            if(elem == null) return;
            if(elem.Type.NumAttributes == 0)
            {
                Console.WriteLine("{0} \"{1}\" of type \"{2}\" does not have any attributes!", (elem is INode) ? "Node" : "Edge",
                    curShellGraph.Graph.GetElementName(elem), elem.Type.Name);
                return;
            }
            Console.WriteLine("All attributes for {0} \"{1}\" of type \"{2}\":", (elem is INode) ? "node" : "edge",
                curShellGraph.Graph.GetElementName(elem), elem.Type.Name);
            foreach(AttributeType attrType in elem.Type.AttributeTypes)
                Console.WriteLine(" - {0}::{1} = {2}", attrType.OwnerType.Name,
                    attrType.Name, elem.GetAttribute(attrType.Name));
        }

        public void ShowElementAttribute(IGraphElement elem, String attributeName)
        {
            if(elem == null) return;

            AttributeType attrType = GetElementAttributeType(elem, attributeName);
            if(attrType == null) return;

            if (attrType.Kind == AttributeKind.MapAttr)
            {
                Type keyType, valueType;
                IDictionary dict = DictionaryHelper.GetDictionaryTypes(
                    elem.GetAttribute(attributeName), out keyType, out valueType);
                Console.Write("The value of attribute \"" + attributeName + "\" is: \"{");
                bool first = true;
                foreach(DictionaryEntry entry in dict)
                {
                    if (first)
                        first = false;
                    else
                        Console.Write(", ");
                    Console.Write(entry.Key + "->" + entry.Value);
                }
                Console.WriteLine("}\".");
            }
            else if (attrType.Kind == AttributeKind.SetAttr)
            {
                Type keyType, valueType;
                IDictionary dict = DictionaryHelper.GetDictionaryTypes(
                    elem.GetAttribute(attributeName), out keyType, out valueType);
                Console.Write("The value of attribute \"" + attributeName + "\" is: \"{");
                bool first = true;
                foreach (DictionaryEntry entry in dict)
                {
                    if (first)
                        first = false;
                    else
                        Console.Write(", ");
                    Console.Write(entry.Key);
                }
                Console.WriteLine("}\".");
            }
            else
            {
                Console.WriteLine("The value of attribute \"" + attributeName + "\" is: \"" + elem.GetAttribute(attributeName) + "\".");
            }
        }

        public void ShowVar(String name)
        {
            object val = GetVarValue(name);
            if (val != null)
            {
                if (val.GetType() == typeof(int)) Console.WriteLine("The value of variable \"" + name + "\" of type int is: \"" + (int)val + "\"");
                else if (val.GetType() == typeof(float)) Console.WriteLine("The value of variable \"" + name + "\" of type float is: \"" + (float)val + "\"");
                else if (val.GetType() == typeof(double)) Console.WriteLine("The value of variable \"" + name + "\" of type double is: \"" + (double)val + "\"");
                else if (val.GetType() == typeof(bool)) Console.WriteLine("The value of variable \"" + name + "\" of type bool is: \"" + (bool)val + "\"");
                else if (val.GetType() == typeof(string)) Console.WriteLine("The value of variable \"" + name + "\" of type string is: \"" + (string)val + "\"");
                else Console.WriteLine("Type of variable \"" + name + "\" is not known");
                // todo: map / set ; including key/value of type enum
            }
        }

        #endregion "show" commands

        public object GetElementAttribute(IGraphElement elem, String attributeName)
        {
            if(elem == null) return null;

            AttributeType attrType = GetElementAttributeType(elem, attributeName);
            if(attrType == null) return null;

            return elem.GetAttribute(attributeName);
        }

        public void SetElementAttribute(IGraphElement elem, String attributeName, String attributeValue)
        {
            if(elem == null) return;
            ArrayList attributes = new ArrayList();
            attributes.Add(new Param(attributeName, attributeValue));
            if(!CheckAttributes(elem.Type, attributes)) return;
            SetAttributes(elem, attributes);
        }

        public object GetVarValue(String varName)
        {
            if(!GraphExists()) return null;
            object val = curShellGraph.Graph.GetVariableValue(varName);
            if(val == null)
            {
                Console.WriteLine("Unknown variable: \"{0}\"", varName);
                return null;
            }
            return val;
        }

        public void SetVariable(String varName, object elem)
        {
            if(!GraphExists()) return;
            curShellGraph.Graph.SetVariableValue(varName, elem);
        }

        public int AllocVisitFlag()
        {
            if(!GraphExists()) return -1;
            return curShellGraph.Graph.AllocateVisitedFlag();
        }

        public bool IsVisited(IGraphElement elem, object idObj, bool printResult, out bool val)
        {
            val = false;    // make compiler happy

            if(!GraphExists()) return false;
            if(!(idObj is int))
            {
                Console.WriteLine("Value of variable must be integer!");
                return false;
            }
            val = curShellGraph.Graph.IsVisited(elem, (int) idObj);
            if(printResult)
                Console.WriteLine("\"" + curShellGraph.Graph.GetElementName(elem)
                    + (val ? "\" has already been visited." : "\" has not been visited yet."));
            return true;
        }

        public bool SetVisited(IGraphElement elem, object idObj, object val)
        {
            if(!GraphExists()) return false;
            if(!(idObj is int))
            {
                Console.WriteLine("Value of the visitor ID variable must be integer!");
                return false;
            }
            if(!(val is bool))
            {
                Console.WriteLine("Value of variable for new flag value must be boolean!");
                return false;
            }
            curShellGraph.Graph.SetVisited(elem, (int) idObj, (bool) val);
            return true;
        }

        public bool FreeVisitFlag(object idObj)
        {
            if(!GraphExists()) return false;
            if(!(idObj is int))
            {
                Console.WriteLine("Value of variable must be integer!");
                return false;
            }
            curShellGraph.Graph.FreeVisitedFlag((int) idObj);
            return true;
        }

        public bool ResetVisitFlag(object idObj)
        {
            if(!GraphExists()) return false;
            if(!(idObj is int))
            {
                Console.WriteLine("Value of variable must be integer!");
                return false;
            }
            curShellGraph.Graph.ResetVisitedFlag((int) idObj);
            return true;
        }

        public void SetRandomSeed(int seed)
        {
            Sequence.randomGenerator = new Random(seed);
        }

        public bool RedirectEmit(String filename)
        {
            if(!GraphExists()) return false;

            if(curShellGraph.Graph.EmitWriter != Console.Out)
                curShellGraph.Graph.EmitWriter.Close();
            if(filename == "-")
                curShellGraph.Graph.EmitWriter = Console.Out;
            else
            {
                try
                {
                    curShellGraph.Graph.EmitWriter = new StreamWriter(filename);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Unable to redirect emit to file \"" + filename + "\":\n" + ex.Message);
                    curShellGraph.Graph.EmitWriter = Console.Out;
                    return false;
                }
            }
            return true;
        }

        public bool ParseFile(String filename)
        {
            if(!GraphExists()) return false;
            if(curShellGraph.Parser == null)
            {
                Console.WriteLine("Please use \"select parser <parserAssembly> <mainMethod>\" first!");
                return false;
            }
            try
            {
                using(FileStream file = new FileStream(filename, FileMode.Open))
                {
                    ASTdapter.ASTdapter astDapter = new ASTdapter.ASTdapter(curShellGraph.Parser);
                    astDapter.Load(file, curShellGraph.Graph);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to parse file \"" + filename + "\":\n" + ex.Message);
                return false;
            }
            return true;
        }

        public bool ParseString(String str)
        {
            if(!GraphExists()) return false;
            if(curShellGraph.Parser == null)
            {
                Console.WriteLine("Please use \"select parser <parserAssembly> <mainMethod>\" first!");
                return false;
            }
            try
            {
                ASTdapter.ASTdapter astDapter = new ASTdapter.ASTdapter(curShellGraph.Parser);
                astDapter.Load(str, curShellGraph.Graph);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to parse string \"" + str + "\":\n" + ex.Message);
                return false;
            }
            return true;
        }

#if DEBUGACTIONS
        private void ShowSequenceDetails(Sequence seq, PerformanceInfo perfInfo)
        {
            switch(seq.OperandClass)
            {
                case Sequence.OperandType.Concat:
                    ShowSequenceDetails(seq.LeftOperand, perfInfo);
                    ShowSequenceDetails(seq.RightOperand, perfInfo);
                    break;
                case Sequence.OperandType.Star:
                case Sequence.OperandType.Max:
                    ShowSequenceDetails(seq.LeftOperand, perfInfo);
                    break;
                case Sequence.OperandType.Rule:
                case Sequence.OperandType.RuleAll:
                    Console.WriteLine(" - {0,-18}: Matches = {1,6}  Match = {2,6} ms  Rewrite = {3,6} ms",
                        ((IAction) seq.Value).Name, seq.GetTotalApplied(),
                        perfInfo.TimeDiffToMS(seq.GetTotalMatchTime()), perfInfo.TimeDiffToMS(seq.GetTotalRewriteTime()));
                    break;
            }
        }
#endif

        bool ContainsSpecial(Sequence seq)
        {
            if((seq.SequenceType == SequenceType.Rule || seq.SequenceType == SequenceType.RuleAll) && ((SequenceRule)seq).Special)
                return true;

            foreach(Sequence child in seq.Children)
                if(ContainsSpecial(child)) return true;

            return false;
        }

        Sequence curGRS;
        SequenceRule curRule;

        public void ApplyRewriteSequence(Sequence seq, bool debug)
        {
            bool installedDumpHandlers = false;

            if(!ActionsExists()) return;

            if(debug || CheckDebuggerAlive())
            {
                debugger.NotifyOnConnectionLost = true;
                debugger.InitNewRewriteSequence(seq, debug);
            }

            if(!InDebugMode && ContainsSpecial(seq))
            {
                curShellGraph.Graph.OnEntereringSequence += new EnterSequenceHandler(DumpOnEntereringSequence);
                curShellGraph.Graph.OnExitingSequence += new ExitSequenceHandler(DumpOnExitingSequence);
                installedDumpHandlers = true;
            }
            else curShellGraph.Graph.OnEntereringSequence += new EnterSequenceHandler(NormalEnteringSequenceHandler);

            curGRS = seq;
            curRule = null;

            Console.WriteLine("Executing Graph Rewrite Sequence... (CTRL+C for abort)");
            cancelSequence = false;
            PerformanceInfo perfInfo = new PerformanceInfo();
            curShellGraph.Graph.PerformanceInfo = perfInfo;
            try
            {
                curShellGraph.Graph.ApplyGraphRewriteSequence(seq);
                Console.WriteLine("Executing Graph Rewrite Sequence done after {0} ms:", perfInfo.TotalTimeMS);
#if DEBUGACTIONS || MATCHREWRITEDETAIL
                Console.WriteLine(" - {0} matches found in {1} ms", perfInfo.MatchesFound, perfInfo.TotalMatchTimeMS);
                Console.WriteLine(" - {0} rewrites performed in {1} ms", perfInfo.RewritesPerformed, perfInfo.TotalRewriteTimeMS);
#if DEBUGACTIONS
                Console.WriteLine("\nDetails:");
                ShowSequenceDetails(seq, perfInfo);
#endif
#else
                Console.WriteLine(" - {0} matches found", perfInfo.MatchesFound);
                Console.WriteLine(" - {0} rewrites performed", perfInfo.RewritesPerformed);
#endif
            }
            catch(OperationCanceledException)
            {
                cancelSequence = true;      // make sure cancelSequence is set to true
                if(curRule == null)
                    Console.WriteLine("Rewrite sequence aborted!");
                else
                {
                    Console.WriteLine("Rewrite sequence aborted after:");
                    Debugger.PrintSequence(curGRS, curRule, Workaround);
                    Console.WriteLine();
                }
            }
            curShellGraph.Graph.PerformanceInfo = null;
            curRule = null;
            curGRS = null;

            if(InDebugMode)
            {
                debugger.NotifyOnConnectionLost = false;
                debugger.FinishRewriteSequence();
            }

            StreamWriter emitWriter = curShellGraph.Graph.EmitWriter as StreamWriter;
            if(emitWriter != null)
                emitWriter.Flush();

            if(installedDumpHandlers)
            {
                curShellGraph.Graph.OnEntereringSequence -= new EnterSequenceHandler(DumpOnEntereringSequence);
                curShellGraph.Graph.OnExitingSequence -= new ExitSequenceHandler(DumpOnExitingSequence);
            }
            else curShellGraph.Graph.OnEntereringSequence -= new EnterSequenceHandler(NormalEnteringSequenceHandler);
        }

        public void WarnDeprecatedGrs(Sequence seq)
        {
            Console.Write(
                "-------------------------------------------------------------------------------\n"
                + "The \"grs\"-command is deprecated and may not be supported in later versions!\n"
                + "An equivalent \"xgrs\"-command is:\n  xgrs ");
            Debugger.PrintSequence(seq, null, Workaround);
            Console.WriteLine("\n-------------------------------------------------------------------------------");
        }

        public void Cancel()
        {
            if(InDebugMode)
                debugger.AbortRewriteSequence();
            throw new OperationCanceledException();                 // abort rewrite sequence
        }

        void NormalEnteringSequenceHandler(Sequence seq)
        {
            if(cancelSequence)
                Cancel();

            if(seq.SequenceType == SequenceType.Rule || seq.SequenceType == SequenceType.RuleAll)
                curRule = (SequenceRule) seq;
        }

        void DumpOnEntereringSequence(Sequence seq)
        {
            if(seq.SequenceType == SequenceType.Rule || seq.SequenceType == SequenceType.RuleAll)
            {
                curRule = (SequenceRule) seq;
                if(curRule.Special)
                    curShellGraph.Graph.OnFinishing += new BeforeFinishHandler(DumpOnFinishing);
            }
        }

        void DumpOnExitingSequence(Sequence seq)
        {
            if(seq.SequenceType == SequenceType.Rule || seq.SequenceType == SequenceType.RuleAll)
            {
                SequenceRule ruleSeq = (SequenceRule) seq;
                if(ruleSeq != null && ruleSeq.Special)
                    curShellGraph.Graph.OnFinishing -= new BeforeFinishHandler(DumpOnFinishing);
            }

            if(cancelSequence)
                Cancel();
        }

        void DumpOnFinishing(IMatches matches, bool special)
        {
            int i = 1;
            Console.WriteLine("Matched " + matches.Producer.Name + " rule:");
            foreach(IMatch match in matches)
            {
                Console.WriteLine(" - " + i + ". match:");
                DumpMatch(match, "   ");
                ++i;
            }
        }

        void DumpMatch(IMatch match, String indentation)
        {
            int i = 0;
            foreach (INode node in match.Nodes)
                Console.WriteLine(indentation + match.Pattern.Nodes[i++].UnprefixedName + ": " + curShellGraph.Graph.GetElementName(node));
            int j = 0;
            foreach (IEdge edge in match.Edges)
                Console.WriteLine(indentation + match.Pattern.Edges[j++].UnprefixedName + ": " + curShellGraph.Graph.GetElementName(edge));

            foreach(IMatch nestedMatch in match.EmbeddedGraphs)
            {
                Console.WriteLine(indentation + nestedMatch.Pattern.Name + ":");
                DumpMatch(nestedMatch, indentation + "  ");
            }
            foreach (IMatch nestedMatch in match.Alternatives)
            {
                Console.WriteLine(indentation + nestedMatch.Pattern.Name + ":");
                DumpMatch(nestedMatch, indentation + "  ");
            }
            foreach (IMatches nestedMatches in match.Iterateds)
            {
                foreach (IMatch nestedMatch in nestedMatches)
                {
                    Console.WriteLine(indentation + nestedMatch.Pattern.Name + ":");
                    DumpMatch(nestedMatch, indentation + "  ");
                }
            }
            foreach (IMatch nestedMatch in match.Independents)
            {
                Console.WriteLine(indentation + nestedMatch.Pattern.Name + ":");
                DumpMatch(nestedMatch, indentation + "  ");
            }
        }

        void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if(curGRS == null || cancelSequence) return;
            if(curRule == null) Console.WriteLine("Cancelling...");
            else Console.WriteLine("Cancelling: Waiting for \"" + curRule.RuleObj.Action.Name + "\" to finish...");
            e.Cancel = true;        // we handled the cancel event
            cancelSequence = true;
        }

        /// <summary>
        /// Enables or disables debug mode.
        /// </summary>
        /// <param name="enable">Whether to enable or not.</param>
        /// <returns>True, if the mode has the desired value at the end of the function.</returns>
        public bool SetDebugMode(bool enable)
        {
            if(enable)
            {
                if(CurrentShellGraph == null)
                {
                    Console.WriteLine("Debug mode will be enabled as soon as a graph has been created!");
                    pendingDebugEnable = true;
                    return false;
                }
                if(InDebugMode && CheckDebuggerAlive())
                {
                    Console.WriteLine("You are already in debug mode!");
                    return true;
                }

                Dictionary<String, String> optMap;
                debugLayoutOptions.TryGetValue(debugLayout, out optMap);
                try
                {
                    debugger = new Debugger(this, debugLayout, optMap);
                }
                catch(Exception ex)
                {
                    if(ex.Message != "Connection to yComp lost")
                        Console.WriteLine(ex.Message);
                    return false;
                }
                pendingDebugEnable = false;
            }
            else
            {
                if(CurrentShellGraph == null && pendingDebugEnable)
                {
                    Console.WriteLine("Debug mode will not be enabled anymore when a graph has been created.");
                    pendingDebugEnable = false;
                    return true;
                }

                if(!InDebugMode)
                {
                    Console.WriteLine("You are not in debug mode!");
                    return true;
                }

                debugger.Close();
                debugger = null;
            }
            return true;
        }

        public bool CheckDebuggerAlive()
        {
            if(!InDebugMode) return false;
            if(!debugger.YCompClient.Sync())
            {
                debugger = null;
                return false;
            }
            return true;
        }

        bool ApplyRule(RuleObject ruleObject)
        {
            IRulePattern pattern = ruleObject.Action.RulePattern;

            bool askForElems = false;
            for(int i = 0; i < ruleObject.ParamVars.Length; i++)
            {
                String param = ruleObject.ParamVars[i];
                if(param == "?")
                {
                    askForElems = true;
                    break;
                }
            }

            debugger.NotifyOnConnectionLost = true;
            try
            {
                if(askForElems)
                {
                    Console.WriteLine("Select the wildcarded elements via double clicking in yComp (ESC for abort):");
                    for(int i = 0; i < ruleObject.ParamVars.Length; i++)
                    {
                        String param = ruleObject.ParamVars[i];
                        if(param == "?")
                        {
                            GrGenType paramType = pattern.Inputs[i];
                            if(paramType is VarType)
                            {
                                Console.WriteLine("Values not supported as placeholder inputs.");
                                return false;
                            }

                            // skip prefixes ("<rulename>_<kind>_" with <kind> in {"node", "edge"}
                            String name = pattern.InputNames[i].Substring(ruleObject.RuleName.Length + 6);
                            while(true)
                            {
                                Console.Write("  " + (paramType.IsNodeType ? "Node" : "Edge")
                                    + " " + name + ":" + paramType.Name + ": ");

                                debugger.YCompClient.WaitForElement(true);

                                // Allow to aborting with ESC
                                while(true)
                                {
                                    if(Console.KeyAvailable && workaround.ReadKey(true).Key == ConsoleKey.Escape)
                                    {
                                        Console.WriteLine("... aborted");
                                        debugger.YCompClient.WaitForElement(false);
                                        return false;
                                    }
                                    if(debugger.YCompClient.CommandAvailable)
                                        break;
                                    Thread.Sleep(100);
                                }

                                String cmd = debugger.YCompClient.ReadCommand();
                                if(cmd.Length < 7 || !cmd.StartsWith("send "))
                                {
                                    Console.WriteLine("Unexpected yComp command: \"" + cmd + "\"");
                                    continue;
                                }

                                // Skip 'n' or 'e'
                                String id = cmd.Substring(6);
                                Console.WriteLine("@(\"" + id + "\")");

                                IGraphElement elem = curShellGraph.Graph.GetGraphElement(id);
                                if(elem == null)
                                {
                                    Console.WriteLine("Graph element does not exist (anymore?).");
                                    continue;
                                }
                                if(elem.Type.IsNodeType != paramType.IsNodeType)
                                {
                                    Console.WriteLine("The graph element is a" + (elem.Type.IsNodeType ? " node" : "n edge")
                                        + " but a" + (paramType.IsNodeType ? " node" : "n edge") + " is expected.");
                                    continue;
                                }
                                if(!elem.Type.IsA(paramType))
                                {
                                    Console.WriteLine(elem.Type.Name + " is not compatible with " + paramType.Name + ".");
                                    continue;
                                }
                                ruleObject.Parameters[i] = elem;
                                ruleObject.ParamVars[i] = null;
                                break;
                            }
                        }
                    }
                }

                Console.WriteLine("\nExecuting graph rewrite action...");
                int numFound = curShellGraph.Graph.ApplyRewrite(ruleObject, 0, 1, false, false);
                if(numFound == 0)
                    Console.WriteLine("  No matches found");
                else
                    Console.WriteLine("  One match found and rewritten");
            }
            catch(OperationCanceledException)
            {
                return false;
            }
            debugger.NotifyOnConnectionLost = false;

            return true;
        }

        public bool DebugApply(RuleObject ruleObject)
        {
            bool debugModeActivated;

            if(!ActionsExists()) return false;

            if(!CheckDebuggerAlive())
            {
                if(!SetDebugMode(true)) return false;
                debugModeActivated = true;
            }
            else
            {
                debugModeActivated = false;
            }

            bool noerror = ApplyRule(ruleObject);

            if(debugModeActivated && CheckDebuggerAlive())   // enabled debug mode here and didn't loose connection?
            {
                Console.Write("Do you want to leave debug mode (y/n)? ");

                char key;
                while((key = workaround.ReadKey(true).KeyChar) != 'y' && key != 'n') ;
                Console.WriteLine(key);

                if(key == 'y')
                    SetDebugMode(false);
            }

            return noerror;
        }

        public void DebugRewriteSequence(Sequence seq)
        {
            bool debugModeActivated;

            if(!CheckDebuggerAlive())
            {
                if(!SetDebugMode(true)) return;
                debugModeActivated = true;
            }
            else debugModeActivated = false;

            ApplyRewriteSequence(seq, true);

            if(debugModeActivated && CheckDebuggerAlive())   // enabled debug mode here and didn't loose connection?
            {
                Console.Write("Do you want to leave debug mode (y/n)? ");

                char key;
                while((key = workaround.ReadKey(true).KeyChar) != 'y' && key != 'n') ;
                Console.WriteLine(key);

                if(key == 'y')
                    SetDebugMode(false);
            }
        }

        public void DebugLayout()
        {
            if(!CheckDebuggerAlive())
            {
                Console.WriteLine("YComp is not active, yet!");
                return;
            }
            debugger.ForceLayout();
        }

        public void SetDebugLayout(String layout)
        {
            if(layout == null || !YCompClient.IsValidLayout(layout))
            {
                if(layout != null)
                    Console.WriteLine("\"" + layout + "\" is not a valid layout name!");
                Console.WriteLine("Available layouts:");
                foreach(String layoutName in YCompClient.AvailableLayouts)
                    Console.WriteLine(" - " + layoutName);
                Console.WriteLine("Current layout: " + debugLayout);
                return;
            }

            if(InDebugMode)
                debugger.SetLayout(layout);

            debugLayout = layout;
        }

        public void GetDebugLayoutOptions()
        {
            if(!CheckDebuggerAlive())
            {
                Console.WriteLine("Layout options can only be read, when YComp is active!");
                return;
            }

            debugger.GetLayoutOptions();
        }

        public void SetDebugLayoutOption(String optionName, String optionValue)
        {
            Dictionary<String, String> optMap;
            if(!debugLayoutOptions.TryGetValue(debugLayout, out optMap))
            {
                optMap = new Dictionary<String, String>();
                debugLayoutOptions[debugLayout] = optMap;
            }

            if(!CheckDebuggerAlive())
            {
                optMap[optionName] = optionValue;       // remember option for debugger startup
                return;
            }

            if(debugger.SetLayoutOption(optionName, optionValue))
                optMap[optionName] = optionValue;       // only remember option if no error was reported
        }

        #region "dump" commands
        public void DumpGraph(String filename)
        {
            if(!GraphExists()) return;

            try
            {
                using(VCGDumper dump = new VCGDumper(filename, curShellGraph.VcgFlags, debugLayout))
                    curShellGraph.Graph.Dump(dump, curShellGraph.DumpInfo);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to dump graph: " + ex.Message);
            }
        }

        private GrColor? ParseGrColor(String colorName)
        {
            GrColor color;

            if(colorName == null)
                goto showavail;

            try
            {
                color = (GrColor) Enum.Parse(typeof(GrColor), colorName, true);
            }
            catch(ArgumentException)
            {
                Console.Write("Unknown color: " + colorName);
                goto showavail;
            }
            return color;

showavail:
            Console.WriteLine("\nAvailable colors are:");
            foreach(String name in Enum.GetNames(typeof(GrColor)))
                Console.WriteLine(" - {0}", name);
            Console.WriteLine();
            return null;
        }

        private GrNodeShape? ParseGrNodeShape(String shapeName)
        {
            GrNodeShape shape;

            if(shapeName == null)
                goto showavail;

            try
            {
                shape = (GrNodeShape) Enum.Parse(typeof(GrNodeShape), shapeName, true);
            }
            catch(ArgumentException)
            {
                Console.Write("Unknown node shape: " + shapeName);
                goto showavail;
            }
            return shape;

showavail:
            Console.WriteLine("\nAvailable node shapes are:", shapeName);
            foreach(String name in Enum.GetNames(typeof(GrNodeShape)))
                Console.WriteLine(" - {0}", name);
            Console.WriteLine();
            return null;
        }

        public bool SetDumpLabel(GrGenType type, String label, bool only)
        {
            if(type == null) return false;

            if(only)
                curShellGraph.DumpInfo.SetElemTypeLabel(type, label);
            else
            {
                foreach(GrGenType subType in type.SubOrSameTypes)
                    curShellGraph.DumpInfo.SetElemTypeLabel(subType, label);
            }

            if(InDebugMode)
                debugger.UpdateYCompDisplay();

            return true;
        }

        delegate void SetNodeDumpColorProc(NodeType type, GrColor color);

        private bool SetDumpColor(NodeType type, String colorName, bool only, SetNodeDumpColorProc setDumpColorProc)
        {
            GrColor? color = ParseGrColor(colorName);
            if(color == null) return false;

            if(only)
                setDumpColorProc(type, (GrColor) color);
            else
            {
                foreach(NodeType subType in type.SubOrSameTypes)
                    setDumpColorProc(subType, (GrColor) color);
            }

            if(InDebugMode)
                debugger.UpdateYCompDisplay();

            return true;
        }

        delegate void SetEdgeDumpColorProc(EdgeType type, GrColor color);

        private bool SetDumpColor(EdgeType type, String colorName, bool only, SetEdgeDumpColorProc setDumpColorProc)
        {
            GrColor? color = ParseGrColor(colorName);
            if(color == null) return false;

            if(only)
                setDumpColorProc(type, (GrColor) color);
            else
            {
                foreach(EdgeType subType in type.SubOrSameTypes)
                    setDumpColorProc(subType, (GrColor) color);
            }

            if(InDebugMode)
                debugger.UpdateYCompDisplay();

            return true;
        }

        public bool SetDumpNodeTypeColor(NodeType type, String colorName, bool only)
        {
            if(type == null) return false;
            return SetDumpColor(type, colorName, only, curShellGraph.DumpInfo.SetNodeTypeColor);
        }

        public bool SetDumpNodeTypeBorderColor(NodeType type, String colorName, bool only)
        {
            if(type == null) return false;
            return SetDumpColor(type, colorName, only, curShellGraph.DumpInfo.SetNodeTypeBorderColor);
        }

        public bool SetDumpNodeTypeTextColor(NodeType type, String colorName, bool only)
        {
            if(type == null) return false;
            return SetDumpColor(type, colorName, only, curShellGraph.DumpInfo.SetNodeTypeTextColor);
        }

        public bool SetDumpNodeTypeShape(NodeType type, String shapeName, bool only)
        {
            if(type == null) return false;

            GrNodeShape? shape = ParseGrNodeShape(shapeName);
            if(shape == null) return false;

            if(only)
                curShellGraph.DumpInfo.SetNodeTypeShape(type, (GrNodeShape) shape);
            else
            {
                foreach(NodeType subType in type.SubOrSameTypes)
                    curShellGraph.DumpInfo.SetNodeTypeShape(subType, (GrNodeShape) shape);
            }
            if(InDebugMode)
                debugger.UpdateYCompDisplay();

            return true;
        }

        public bool SetDumpEdgeTypeColor(EdgeType type, String colorName, bool only)
        {
            if(type == null) return false;
            return SetDumpColor(type, colorName, only, curShellGraph.DumpInfo.SetEdgeTypeColor);
        }

        public bool SetDumpEdgeTypeTextColor(EdgeType type, String colorName, bool only)
        {
            if(type == null) return false;
            return SetDumpColor(type, colorName, only, curShellGraph.DumpInfo.SetEdgeTypeTextColor);
        }

        public bool AddDumpExcludeNodeType(NodeType nodeType, bool only)
        {
            if(nodeType == null) return false;

            if(only)
                curShellGraph.DumpInfo.ExcludeNodeType(nodeType);
            else
                foreach(NodeType subType in nodeType.SubOrSameTypes)
                    curShellGraph.DumpInfo.ExcludeNodeType(subType);

            return true;
        }

        public bool AddDumpExcludeEdgeType(EdgeType edgeType, bool only)
        {
            if(edgeType == null) return false;

            if(only)
                curShellGraph.DumpInfo.ExcludeEdgeType(edgeType);
            else
                foreach(EdgeType subType in edgeType.SubOrSameTypes)
                    curShellGraph.DumpInfo.ExcludeEdgeType(subType);

            return true;
        }

        public bool AddDumpGroupNodesBy(NodeType nodeType, bool exactNodeType, EdgeType edgeType, bool exactEdgeType,
                NodeType adjNodeType, bool exactAdjNodeType, GroupMode groupMode)
        {
            if(nodeType == null || edgeType == null || adjNodeType == null) return false;

            curShellGraph.DumpInfo.AddOrExtendGroupNodeType(nodeType, exactNodeType, edgeType, exactEdgeType,
                adjNodeType, exactAdjNodeType, groupMode);

            return true;
        }

        public bool AddDumpInfoTag(GrGenType type, String attrName, bool only, bool isshort)
        {
            if(type == null) return false;

            AttributeType attrType = type.GetAttributeType(attrName);
            if(attrType == null)
            {
                Console.WriteLine("Type \"" + type.Name + "\" has no attribute \"" + attrName + "\"");
                return false;
            }

            InfoTag infoTag = new InfoTag(attrType, isshort);
            if(only)
                curShellGraph.DumpInfo.AddTypeInfoTag(type, infoTag);
            else
                foreach(GrGenType subtype in type.SubOrSameTypes)
                    curShellGraph.DumpInfo.AddTypeInfoTag(subtype, infoTag);

            if(InDebugMode)
                debugger.UpdateYCompDisplay();

            return true;
        }

        public void DumpReset()
        {
            if(!GraphExists()) return;

            curShellGraph.DumpInfo.Reset();

            if(InDebugMode)
                debugger.UpdateYCompDisplay();
        }
        #endregion "dump" commands

        #region "map" commands

        public object MapNew(String keyTypeName, String valueTypeName)
        {
            Type keyType = DictionaryHelper.GetTypeFromNameForDictionary(keyTypeName, curShellGraph.Graph);
            if (keyType == null) Console.WriteLine("\"" + keyTypeName + "\" is not a valid type for a map.");
            Type valueType = DictionaryHelper.GetTypeFromNameForDictionary(valueTypeName, curShellGraph.Graph);
            if (valueType == null) Console.WriteLine("\"" + valueTypeName + "\" is not a valid type for a map.");
            return DictionaryHelper.NewDictionary(keyType, valueType);
        }

        public String GetMapIdentifier(bool usedGraphElement, IGraphElement elem, String attrOrVarName)
        {
            if(usedGraphElement) return curShellGraph.Graph.GetElementName(elem) + "." + attrOrVarName;
            else return attrOrVarName;
        }

        public void MapAdd(bool usedGraphElement, IGraphElement elem, String attrOrVarName, object keyExpr, object valueExpr)
        {
            object map;

            if(usedGraphElement)
            {
                if(elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if(attrType == null) return;
                map = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                map = GetVarValue(attrOrVarName);
                if(map == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(map, out keyType, out valueType);
            if(dict == null)
            {
                Console.WriteLine(GetMapIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a map.");
                return;
            }
            if(keyType != keyExpr.GetType())
            {
                Console.WriteLine("Key type must be " + keyType + ", but is " + keyExpr.GetType() + ".");
                return;
            }
            if(valueType != valueExpr.GetType())
            {
                Console.WriteLine("Value type must be " + valueType + ", but is " + valueExpr.GetType() + ".");
                return;
            }
            dict[keyExpr] = valueExpr;
        }

        public void MapRemove(bool usedGraphElement, IGraphElement elem, String attrOrVarName, object keyExpr)
        {
            object map;

            if(usedGraphElement)
            {
                if(elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if(attrType == null) return;
                map = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                map = GetVarValue(attrOrVarName);
                if(map == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(map, out keyType, out valueType);
            if (dict == null)
            {
                Console.WriteLine(GetMapIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a map.");
                return;
            }
            if(keyType != keyExpr.GetType())
            {
                Console.WriteLine("Key type must be " + keyType + ", but is " + keyExpr.GetType() + ".");
                return;
            }
            dict.Remove(keyExpr);
        }

        public void MapSize(bool usedGraphElement, IGraphElement elem, String attrOrVarName)
        {
            object map;

            if(usedGraphElement)
            {
                if(elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if(attrType == null) return;
                map = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                map = GetVarValue(attrOrVarName);
                if(map == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(map, out keyType, out valueType);
            if(dict == null)
            {
                Console.WriteLine(GetMapIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a map.");
                return;
            }
            Console.WriteLine(GetMapIdentifier(usedGraphElement, elem, attrOrVarName) + " contains " + dict.Count + " entries.");
        }

        #endregion "map" commands


        #region "set" commands

        public object SetNew(String keyTypeName)
        {
            Type keyType = DictionaryHelper.GetTypeFromNameForDictionary(keyTypeName, curShellGraph.Graph);
            if (keyType == null) Console.WriteLine("\"" + keyTypeName + "\" is not a valid type for a set.");
            return DictionaryHelper.NewDictionary(keyType, typeof(SetValueType));
        }

        public String GetSetIdentifier(bool usedGraphElement, IGraphElement elem, String attrOrVarName)
        {
            if (usedGraphElement) return curShellGraph.Graph.GetElementName(elem) + "." + attrOrVarName;
            else return attrOrVarName;
        }

        public void SetAdd(bool usedGraphElement, IGraphElement elem, String attrOrVarName, object keyExpr)
        {
            object set;

            if (usedGraphElement)
            {
                if (elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if (attrType == null) return;
                set = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                set = GetVarValue(attrOrVarName);
                if (set == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(set, out keyType, out valueType);
            if (dict == null)
            {
                Console.WriteLine(GetSetIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a set.");
                return;
            }
            if (keyType != keyExpr.GetType())
            {
                Console.WriteLine("Set type must be " + keyType + ", but is " + keyExpr.GetType() + ".");
                return;
            }
            if (valueType != typeof(SetValueType))
            {
                Console.WriteLine("Not a set.");
                return;
            }
            dict[keyExpr] = null;
        }

        public void SetRemove(bool usedGraphElement, IGraphElement elem, String attrOrVarName, object keyExpr)
        {
            object set;

            if (usedGraphElement)
            {
                if (elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if (attrType == null) return;
                set = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                set = GetVarValue(attrOrVarName);
                if (set == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(set, out keyType, out valueType);
            if (dict == null)
            {
                Console.WriteLine(GetSetIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a set.");
                return;
            }
            if (keyType != keyExpr.GetType())
            {
                Console.WriteLine("Set type must be " + keyType + ", but is " + keyExpr.GetType() + ".");
                return;
            }
            if (valueType != typeof(SetValueType))
            {
                Console.WriteLine("Not a set.");
                return;
            }
            dict.Remove(keyExpr);
        }

        public void SetSize(bool usedGraphElement, IGraphElement elem, String attrOrVarName)
        {
            object set;

            if (usedGraphElement)
            {
                if (elem == null) return;
                AttributeType attrType = GetElementAttributeType(elem, attrOrVarName);
                if (attrType == null) return;
                set = elem.GetAttribute(attrOrVarName);
            }
            else
            {
                set = GetVarValue(attrOrVarName);
                if (set == null) return;
            }

            Type keyType, valueType;
            IDictionary dict = DictionaryHelper.GetDictionaryTypes(set, out keyType, out valueType);
            if (dict == null)
            {
                Console.WriteLine(GetSetIdentifier(usedGraphElement, elem, attrOrVarName) + " is not a set.");
                return;
            }
            Console.WriteLine(GetSetIdentifier(usedGraphElement, elem, attrOrVarName) + " contains " + dict.Count + " entries.");
        }

        #endregion "set" commands

        private String StringToTextToken(String str)
        {
            if(str.IndexOf('\"') != -1) return "\'" + str + "\'";
            else return "\"" + str + "\"";
        }
                                         
        public void SaveGraph(String filename)
        {
            if(!GraphExists()) return;

            FileStream file = null;
            StreamWriter sw;
            try
            {
                file = new FileStream(filename, FileMode.Create);
                sw = new StreamWriter(file);
            }                           
            catch(IOException e)
            {
                Console.WriteLine("Unable to create file \"{0}\": ", e.Message);
                if(file != null) file.Close();
                return;
            }
            try
            {
                NamedGraph graph = curShellGraph.Graph;
                sw.WriteLine("# Graph \"{0}\" saved by grShell", graph.Name);
                sw.WriteLine();
				if(curShellGraph.BackendFilename != null)
				{
					sw.Write("select backend " + StringToTextToken(curShellGraph.BackendFilename));
					foreach(String param in curShellGraph.BackendParameters)
					{
						sw.Write(" " + StringToTextToken(param));
					}
					sw.WriteLine();
				}

                GRSExport.Export(graph, sw);

                foreach(KeyValuePair<NodeType, GrColor> nodeTypeColor in curShellGraph.DumpInfo.NodeTypeColors)
                    sw.WriteLine("dump set node only {0} color {1}", nodeTypeColor.Key.Name, nodeTypeColor.Value);

                foreach(KeyValuePair<NodeType, GrColor> nodeTypeBorderColor in curShellGraph.DumpInfo.NodeTypeBorderColors)
                    sw.WriteLine("dump set node only {0} bordercolor {1}", nodeTypeBorderColor.Key.Name, nodeTypeBorderColor.Value);

                foreach(KeyValuePair<NodeType, GrColor> nodeTypeTextColor in curShellGraph.DumpInfo.NodeTypeTextColors)
                    sw.WriteLine("dump set node only {0} textcolor {1}", nodeTypeTextColor.Key.Name, nodeTypeTextColor.Value);

                foreach(KeyValuePair<NodeType, GrNodeShape> nodeTypeShape in curShellGraph.DumpInfo.NodeTypeShapes)
                    sw.WriteLine("dump set node only {0} shape {1}", nodeTypeShape.Key.Name, nodeTypeShape.Value);

                foreach(KeyValuePair<EdgeType, GrColor> edgeTypeColor in curShellGraph.DumpInfo.EdgeTypeColors)
                    sw.WriteLine("dump set edge only {0} color {1}", edgeTypeColor.Key.Name, edgeTypeColor.Value);

                foreach(KeyValuePair<EdgeType, GrColor> edgeTypeTextColor in curShellGraph.DumpInfo.EdgeTypeTextColors)
                    sw.WriteLine("dump set edge only {0} textcolor {1}", edgeTypeTextColor.Key.Name, edgeTypeTextColor.Value);

                if((curShellGraph.VcgFlags & VCGFlags.EdgeLabels) == 0)
                    sw.WriteLine("dump set edge labels off");

                foreach(NodeType excludedNodeType in curShellGraph.DumpInfo.ExcludedNodeTypes)
                    sw.WriteLine("dump add node only " + excludedNodeType.Name + " exclude");

                foreach(EdgeType excludedEdgeType in curShellGraph.DumpInfo.ExcludedEdgeTypes)
                    sw.WriteLine("dump add edge only " + excludedEdgeType.Name + " exclude");

                foreach(GroupNodeType groupNodeType in curShellGraph.DumpInfo.GroupNodeTypes)
                {
                    foreach(KeyValuePair<EdgeType, Dictionary<NodeType, GroupMode>> ekvp in groupNodeType.GroupEdges)
                    {
                        foreach(KeyValuePair<NodeType, GroupMode> nkvp in ekvp.Value)
                        {
                            String groupModeStr;
                            switch(nkvp.Value & GroupMode.GroupAllNodes)
                            {
                                case GroupMode.None:               groupModeStr = "no";       break;
                                case GroupMode.GroupIncomingNodes: groupModeStr = "incoming"; break;
                                case GroupMode.GroupOutgoingNodes: groupModeStr = "outgoing"; break;
                                case GroupMode.GroupAllNodes:      groupModeStr = "any";      break;
                                default: groupModeStr = "This case does not exist by definition..."; break;
                            }
                            sw.WriteLine("dump add node only " + groupNodeType.NodeType.Name
                                + " group by " + ((nkvp.Value & GroupMode.Hidden) != 0 ? "hidden " : "") + groupModeStr
                                + " only " + ekvp.Key.Name + " with only " + nkvp.Key.Name);
                        }
                    }
                }

                foreach(KeyValuePair<GrGenType, List<InfoTag>> infoTagPair in curShellGraph.DumpInfo.InfoTags)
                {
                    String kind;
                    if(infoTagPair.Key.IsNodeType) kind = "node";
                    else kind = "edge";

                    foreach(InfoTag infoTag in infoTagPair.Value)
                    {
                        sw.WriteLine("dump add " + kind + " only " + infoTagPair.Key.Name
                            + (infoTag.ShortInfoTag ? " shortinfotag " : " infotag ") + infoTag.AttributeType.Name);
                    }
                }

                if(debugLayoutOptions.Count != 0)
                {
                    foreach(KeyValuePair<String, Dictionary<String, String>> layoutOptions in debugLayoutOptions)
                    {
                        sw.WriteLine("debug set layout " + layoutOptions.Key);
                        foreach(KeyValuePair<String, String> option in layoutOptions.Value)
                            sw.WriteLine("debug set layout option " + option.Key + " " + option.Value);
                    }
                    sw.WriteLine("debug set layout " + debugLayout);
                }

                sw.WriteLine("# end of graph \"{0}\" saved by grShell", graph.Name);
            }
            catch(IOException e)
            {
                Console.WriteLine("Write error: Unable to export file: {0}", e.Message);
            }
            finally
            {
                sw.Close();
            }
        }

        public bool Export(String filename)
        {
            if(!GraphExists()) return false;

            try
            {
                Porter.Export(curShellGraph.Graph, filename);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to export graph: " + e.Message);
                return false;
            }
            Console.WriteLine("Graph \"" + curShellGraph.Graph.Name + "\" exported.");
            return true;
        }

        public bool Import(List<String> filenames)
        {
            if(!BackendExists()) return false;

            IGraph graph;
            try
            {
                int startTime = Environment.TickCount;
                graph = Porter.Import(curGraphBackend, filenames);
                System.Console.Out.WriteLine("import done after: " + (Environment.TickCount - startTime) + " ms");
                System.Console.Out.WriteLine("graph size after import: " + System.GC.GetTotalMemory(true) + " bytes");
                startTime = Environment.TickCount;
                curShellGraph = new ShellGraph(graph, backendFilename, backendParameters, graph.Model.ModelName + ".gm");
                System.Console.Out.WriteLine("shell import done after: " + (Environment.TickCount - startTime) + " ms");
                System.Console.Out.WriteLine("shell graph size after import: " + System.GC.GetTotalMemory(true) + " bytes");
                curShellGraph.Actions = graph.Actions;
                graphs.Add(curShellGraph);
                ShowNumNodes(null, false);
                ShowNumEdges(null, false);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to import graph: " + e.Message);
                return false;
            }
            Console.WriteLine("Graph \"" + graph.Name + "\" imported.");
            return true;
        }

/*        private String FormatName(IGraphElement elem)
        {
            String name;
            if(!curShellGraph.IDToName.TryGetValue(elem.ID, out name))
                return elem.ID.ToString();
            else
                return String.Format("\"{0}\" ({1})", name, elem.ID);
        }*/

        private void DumpElems<T>(IEnumerable<T> elems) where T : IGraphElement
        {
            bool first = true;
            foreach(IGraphElement elem in elems)
            {
                if(!first)
                    Console.Write(',');
                else
                    first = false;
                Console.Write("\"{0}\"", curShellGraph.Graph.GetElementName(elem));
            }
        }

        public void Validate(bool strict)
        {
            List<ConnectionAssertionError> errors;
            if(!GraphExists()) return;
            if(curShellGraph.Graph.Validate(strict, out errors))
                Console.WriteLine("The graph is valid.");
            else
            {
                Console.WriteLine("The graph is NOT valid:");
                foreach(ConnectionAssertionError error in errors)
                {
                    ValidateInfo valInfo = error.ValidateInfo;
                    switch(error.CAEType)
                    {
                        case CAEType.EdgeNotSpecified:
                        {
                            IEdge edge = (IEdge) error.Elem;
                            Console.WriteLine("  CAE: {0} \"{1}\" -- {2} \"{3}\" --> {4} \"{5}\" not specified",
                                edge.Source.Type.Name, curShellGraph.Graph.GetElementName(edge.Source), 
                                edge.Type.Name, curShellGraph.Graph.GetElementName(edge), 
                                edge.Target.Type.Name, curShellGraph.Graph.GetElementName(edge.Target));
                            break;
                        }
                        case CAEType.NodeTooFewSources:
                        {
                            INode node = (INode) error.Elem;
                            Console.Write("  CAE: {0} \"{1}\" [{2}<{3}] -- {4} ", valInfo.SourceType.Name,
                                curShellGraph.Graph.GetElementName(node), error.FoundEdges,
                                valInfo.SourceLower, valInfo.EdgeType.Name);
                            DumpElems(node.GetCompatibleOutgoing(valInfo.EdgeType));
                            Console.WriteLine(" --> {0}", valInfo.TargetType.Name);
                            break;
                        }
                        case CAEType.NodeTooManySources:
                        {
                            INode node = (INode) error.Elem;
                            Console.Write("  CAE: {0} \"{1}\" [{2}>{3}] -- {4} ", valInfo.SourceType.Name,
                                curShellGraph.Graph.GetElementName(node), error.FoundEdges,
                                valInfo.SourceUpper, valInfo.EdgeType.Name);
                            DumpElems(node.GetCompatibleOutgoing(valInfo.EdgeType));
                            Console.WriteLine(" --> {0}", valInfo.TargetType.Name);
                            break;
                        }
                        case CAEType.NodeTooFewTargets:
                        {
                            INode node = (INode) error.Elem;
                            Console.Write("  CAE: {0} -- {1} ", valInfo.SourceType.Name,
                                valInfo.EdgeType.Name);
                            DumpElems(node.GetCompatibleIncoming(valInfo.EdgeType));
                            Console.WriteLine(" --> {0} \"{1}\" [{2}<{3}]", valInfo.TargetType.Name,
                                curShellGraph.Graph.GetElementName(node), error.FoundEdges, valInfo.TargetLower);
                            break;
                        }
                        case CAEType.NodeTooManyTargets:
                        {
                            INode node = (INode) error.Elem;
                            Console.Write("  CAE: {0} -- {1} ", valInfo.SourceType.Name,
                                valInfo.EdgeType.Name);
                            DumpElems(node.GetCompatibleIncoming(valInfo.EdgeType));
                            Console.WriteLine(" --> {0} \"{1}\" [{2}>{3}]", valInfo.TargetType.Name,
                                curShellGraph.Graph.GetElementName(node), error.FoundEdges, valInfo.TargetUpper);
                            break;
                        }
                    }
                }
            }
        }

        public void ValidateWithSequence(Sequence seq)
        {
            if(!ActionsExists()) return;

            if(!curShellGraph.Graph.ValidateWithSequence(seq))
                Console.WriteLine("The graph is NOT valid with respect to the given sequence!");
            else
                Console.WriteLine("The graph is valid with respect to the given sequence.");
        }

        public void NodeTypeIsA(INode node1, INode node2)
        {
            if(node1 == null || node2 == null) return;

            NodeType type1 = node1.Type;
            NodeType type2 = node2.Type;

            Console.WriteLine("{0} type {1} is a node: {2}", type1.Name, type2.Name,
                type1.IsA(type2) ? "yes" : "no");
        }

        public void EdgeTypeIsA(IEdge edge1, IEdge edge2)
        {
            if(edge1 == null || edge2 == null) return;

            EdgeType type1 = edge1.Type;
            EdgeType type2 = edge2.Type;

            Console.WriteLine("{0} type {1} is an edge: {2}", type1.Name, type2.Name,
                type1.IsA(type2) ? "yes" : "no");
        }

        public void CustomGraph(List<String> parameterList)
        {
            if(!GraphExists()) return;

            String[] parameters = parameterList.ToArray();
            try
            {
                curShellGraph.Graph.Custom(parameters);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CustomActions(List<String> parameterList)
        {
            if(!ActionsExists()) return;

            String[] parameters = parameterList.ToArray();
            try
            {
                curShellGraph.Actions.Custom(parameters);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}