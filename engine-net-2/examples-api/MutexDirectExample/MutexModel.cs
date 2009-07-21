// This file has been generated automatically by GrGen.
// Do not modify this file! Any changes will be lost!
// Generated from "..\..\examples\Mutex\MutexPimped.grg" on Tue Jul 21 00:01:28 GMT+01:00 2009

using System;
using System.Collections.Generic;
using GRGEN_LIBGR = de.unika.ipd.grGen.libGr;
using GRGEN_LGSP = de.unika.ipd.grGen.lgsp;

namespace de.unika.ipd.grGen.Model_Mutex
{
	using GRGEN_MODEL = de.unika.ipd.grGen.Model_Mutex;
	//
	// Enums
	//

	public class Enums
	{
	}

	//
	// Node types
	//

	public enum NodeTypes { @Node, @Process, @Resource };

	// *** Node Node ***


	public sealed class @Node : GRGEN_LGSP.LGSPNode, GRGEN_LIBGR.INode
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@Node[] pool = new GRGEN_MODEL.@Node[10];
		
		static @Node() {
		}
		
		public @Node() : base(GRGEN_MODEL.NodeType_Node.typeVar)
		{
			// implicit initialization, map/set creation of Node
		}

		public static GRGEN_MODEL.NodeType_Node TypeInstance { get { return GRGEN_MODEL.NodeType_Node.typeVar; } }

		public override GRGEN_LIBGR.INode Clone() { return new GRGEN_MODEL.@Node(this); }

		private @Node(GRGEN_MODEL.@Node oldElem) : base(GRGEN_MODEL.NodeType_Node.typeVar)
		{
		}
		public static GRGEN_MODEL.@Node CreateNode(GRGEN_LGSP.LGSPGraph graph)
		{
			GRGEN_MODEL.@Node node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Node();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Node
			}
			graph.AddNode(node);
			return node;
		}

		public static GRGEN_MODEL.@Node CreateNode(GRGEN_LGSP.LGSPGraph graph, string varName)
		{
			GRGEN_MODEL.@Node node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Node();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Node
			}
			graph.AddNode(node, varName);
			return node;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The node type \"Node\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The node type \"Node\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of Node
		}
	}

	public sealed class NodeType_Node : GRGEN_LIBGR.NodeType
	{
		public static GRGEN_MODEL.NodeType_Node typeVar = new GRGEN_MODEL.NodeType_Node();
		public static bool[] isA = new bool[] { true, false, false, };
		public static bool[] isMyType = new bool[] { true, true, true, };
		public NodeType_Node() : base((int) NodeTypes.@Node)
		{
		}
		public override string Name { get { return "Node"; } }
		public override GRGEN_LIBGR.INode CreateNode()
		{
			return new GRGEN_MODEL.@Node();
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.INode CreateNodeWithCopyCommons(GRGEN_LIBGR.INode oldINode)
		{
			return new GRGEN_MODEL.@Node();
		}

	}

	// *** Node Process ***

	public interface IProcess : GRGEN_LIBGR.INode
	{
	}

	public sealed class @Process : GRGEN_LGSP.LGSPNode, GRGEN_MODEL.IProcess
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@Process[] pool = new GRGEN_MODEL.@Process[10];
		
		// explicit initializations of Process for target Process
		// implicit initializations of Process for target Process
		static @Process() {
		}
		
		public @Process() : base(GRGEN_MODEL.NodeType_Process.typeVar)
		{
			// implicit initialization, map/set creation of Process
			// explicit initializations of Process for target Process
		}

		public static GRGEN_MODEL.NodeType_Process TypeInstance { get { return GRGEN_MODEL.NodeType_Process.typeVar; } }

		public override GRGEN_LIBGR.INode Clone() { return new GRGEN_MODEL.@Process(this); }

		private @Process(GRGEN_MODEL.@Process oldElem) : base(GRGEN_MODEL.NodeType_Process.typeVar)
		{
		}
		public static GRGEN_MODEL.@Process CreateNode(GRGEN_LGSP.LGSPGraph graph)
		{
			GRGEN_MODEL.@Process node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Process();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Process
				// explicit initializations of Process for target Process
			}
			graph.AddNode(node);
			return node;
		}

		public static GRGEN_MODEL.@Process CreateNode(GRGEN_LGSP.LGSPGraph graph, string varName)
		{
			GRGEN_MODEL.@Process node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Process();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Process
				// explicit initializations of Process for target Process
			}
			graph.AddNode(node, varName);
			return node;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The node type \"Process\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The node type \"Process\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of Process
			// explicit initializations of Process for target Process
		}
	}

	public sealed class NodeType_Process : GRGEN_LIBGR.NodeType
	{
		public static GRGEN_MODEL.NodeType_Process typeVar = new GRGEN_MODEL.NodeType_Process();
		public static bool[] isA = new bool[] { true, true, false, };
		public static bool[] isMyType = new bool[] { false, true, false, };
		public NodeType_Process() : base((int) NodeTypes.@Process)
		{
		}
		public override string Name { get { return "Process"; } }
		public override GRGEN_LIBGR.INode CreateNode()
		{
			return new GRGEN_MODEL.@Process();
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.INode CreateNodeWithCopyCommons(GRGEN_LIBGR.INode oldINode)
		{
			return new GRGEN_MODEL.@Process();
		}

	}

	// *** Node Resource ***

	public interface IResource : GRGEN_LIBGR.INode
	{
	}

	public sealed class @Resource : GRGEN_LGSP.LGSPNode, GRGEN_MODEL.IResource
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@Resource[] pool = new GRGEN_MODEL.@Resource[10];
		
		// explicit initializations of Resource for target Resource
		// implicit initializations of Resource for target Resource
		static @Resource() {
		}
		
		public @Resource() : base(GRGEN_MODEL.NodeType_Resource.typeVar)
		{
			// implicit initialization, map/set creation of Resource
			// explicit initializations of Resource for target Resource
		}

		public static GRGEN_MODEL.NodeType_Resource TypeInstance { get { return GRGEN_MODEL.NodeType_Resource.typeVar; } }

		public override GRGEN_LIBGR.INode Clone() { return new GRGEN_MODEL.@Resource(this); }

		private @Resource(GRGEN_MODEL.@Resource oldElem) : base(GRGEN_MODEL.NodeType_Resource.typeVar)
		{
		}
		public static GRGEN_MODEL.@Resource CreateNode(GRGEN_LGSP.LGSPGraph graph)
		{
			GRGEN_MODEL.@Resource node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Resource();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Resource
				// explicit initializations of Resource for target Resource
			}
			graph.AddNode(node);
			return node;
		}

		public static GRGEN_MODEL.@Resource CreateNode(GRGEN_LGSP.LGSPGraph graph, string varName)
		{
			GRGEN_MODEL.@Resource node;
			if(poolLevel == 0)
				node = new GRGEN_MODEL.@Resource();
			else
			{
				node = pool[--poolLevel];
				node.inhead = null;
				node.outhead = null;
				node.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				// implicit initialization, map/set creation of Resource
				// explicit initializations of Resource for target Resource
			}
			graph.AddNode(node, varName);
			return node;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The node type \"Resource\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The node type \"Resource\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of Resource
			// explicit initializations of Resource for target Resource
		}
	}

	public sealed class NodeType_Resource : GRGEN_LIBGR.NodeType
	{
		public static GRGEN_MODEL.NodeType_Resource typeVar = new GRGEN_MODEL.NodeType_Resource();
		public static bool[] isA = new bool[] { true, false, true, };
		public static bool[] isMyType = new bool[] { false, false, true, };
		public NodeType_Resource() : base((int) NodeTypes.@Resource)
		{
		}
		public override string Name { get { return "Resource"; } }
		public override GRGEN_LIBGR.INode CreateNode()
		{
			return new GRGEN_MODEL.@Resource();
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.INode CreateNodeWithCopyCommons(GRGEN_LIBGR.INode oldINode)
		{
			return new GRGEN_MODEL.@Resource();
		}

	}

	//
	// Node model
	//

	public sealed class MutexNodeModel : GRGEN_LIBGR.INodeModel
	{
		public MutexNodeModel()
		{
			GRGEN_MODEL.NodeType_Node.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.NodeType_Node.typeVar.subOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Node.typeVar,
				GRGEN_MODEL.NodeType_Process.typeVar,
				GRGEN_MODEL.NodeType_Resource.typeVar,
			};
			GRGEN_MODEL.NodeType_Node.typeVar.directSubGrGenTypes = GRGEN_MODEL.NodeType_Node.typeVar.directSubTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Process.typeVar,
				GRGEN_MODEL.NodeType_Resource.typeVar,
			};
			GRGEN_MODEL.NodeType_Node.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.NodeType_Node.typeVar.superOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Node.typeVar,
			};
			GRGEN_MODEL.NodeType_Node.typeVar.directSuperGrGenTypes = GRGEN_MODEL.NodeType_Node.typeVar.directSuperTypes = new GRGEN_LIBGR.NodeType[] {
			};
			GRGEN_MODEL.NodeType_Process.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.NodeType_Process.typeVar.subOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Process.typeVar,
			};
			GRGEN_MODEL.NodeType_Process.typeVar.directSubGrGenTypes = GRGEN_MODEL.NodeType_Process.typeVar.directSubTypes = new GRGEN_LIBGR.NodeType[] {
			};
			GRGEN_MODEL.NodeType_Process.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.NodeType_Process.typeVar.superOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Process.typeVar,
				GRGEN_MODEL.NodeType_Node.typeVar,
			};
			GRGEN_MODEL.NodeType_Process.typeVar.directSuperGrGenTypes = GRGEN_MODEL.NodeType_Process.typeVar.directSuperTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Node.typeVar,
			};
			GRGEN_MODEL.NodeType_Resource.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.NodeType_Resource.typeVar.subOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Resource.typeVar,
			};
			GRGEN_MODEL.NodeType_Resource.typeVar.directSubGrGenTypes = GRGEN_MODEL.NodeType_Resource.typeVar.directSubTypes = new GRGEN_LIBGR.NodeType[] {
			};
			GRGEN_MODEL.NodeType_Resource.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.NodeType_Resource.typeVar.superOrSameTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Resource.typeVar,
				GRGEN_MODEL.NodeType_Node.typeVar,
			};
			GRGEN_MODEL.NodeType_Resource.typeVar.directSuperGrGenTypes = GRGEN_MODEL.NodeType_Resource.typeVar.directSuperTypes = new GRGEN_LIBGR.NodeType[] {
				GRGEN_MODEL.NodeType_Node.typeVar,
			};
		}
		public bool IsNodeModel { get { return true; } }
		public GRGEN_LIBGR.NodeType RootType { get { return GRGEN_MODEL.NodeType_Node.typeVar; } }
		GRGEN_LIBGR.GrGenType GRGEN_LIBGR.ITypeModel.RootType { get { return GRGEN_MODEL.NodeType_Node.typeVar; } }
		public GRGEN_LIBGR.NodeType GetType(string name)
		{
			switch(name)
			{
				case "Node" : return GRGEN_MODEL.NodeType_Node.typeVar;
				case "Process" : return GRGEN_MODEL.NodeType_Process.typeVar;
				case "Resource" : return GRGEN_MODEL.NodeType_Resource.typeVar;
			}
			return null;
		}
		GRGEN_LIBGR.GrGenType GRGEN_LIBGR.ITypeModel.GetType(string name)
		{
			return GetType(name);
		}
		private GRGEN_LIBGR.NodeType[] types = {
			GRGEN_MODEL.NodeType_Node.typeVar,
			GRGEN_MODEL.NodeType_Process.typeVar,
			GRGEN_MODEL.NodeType_Resource.typeVar,
		};
		public GRGEN_LIBGR.NodeType[] Types { get { return types; } }
		GRGEN_LIBGR.GrGenType[] GRGEN_LIBGR.ITypeModel.Types { get { return types; } }
		private Type[] typeTypes = {
			typeof(GRGEN_MODEL.NodeType_Node),
			typeof(GRGEN_MODEL.NodeType_Process),
			typeof(GRGEN_MODEL.NodeType_Resource),
		};
		public Type[] TypeTypes { get { return typeTypes; } }
		private GRGEN_LIBGR.AttributeType[] attributeTypes = {
		};
		public IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { return attributeTypes; } }
	}

	//
	// Edge types
	//

	public enum EdgeTypes { @AEdge, @Edge, @UEdge, @next, @blocked, @held_by, @token, @release, @request };

	// *** Edge AEdge ***


	public sealed class EdgeType_AEdge : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_AEdge typeVar = new GRGEN_MODEL.EdgeType_AEdge();
		public static bool[] isA = new bool[] { true, false, false, false, false, false, false, false, false, };
		public static bool[] isMyType = new bool[] { true, true, true, true, true, true, true, true, true, };
		public EdgeType_AEdge() : base((int) EdgeTypes.@AEdge)
		{
		}
		public override string Name { get { return "AEdge"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Arbitrary; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			throw new Exception("The abstract edge type AEdge cannot be instantiated!");
		}
		public override bool IsAbstract { get { return true; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			throw new Exception("Cannot retype to the abstract type AEdge!");
		}
	}

	// *** Edge Edge ***


	public sealed class @Edge : GRGEN_LGSP.LGSPEdge, GRGEN_LIBGR.IEdge
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@Edge[] pool = new GRGEN_MODEL.@Edge[10];
		
		static @Edge() {
		}
		
		public @Edge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_Edge.typeVar, source, target)
		{
			// implicit initialization, map/set creation of Edge
		}

		public static GRGEN_MODEL.EdgeType_Edge TypeInstance { get { return GRGEN_MODEL.EdgeType_Edge.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@Edge(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @Edge(GRGEN_MODEL.@Edge oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_Edge.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@Edge CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@Edge edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@Edge(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of Edge
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@Edge CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@Edge edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@Edge(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of Edge
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"Edge\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"Edge\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of Edge
		}
	}

	public sealed class EdgeType_Edge : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_Edge typeVar = new GRGEN_MODEL.EdgeType_Edge();
		public static bool[] isA = new bool[] { true, true, false, false, false, false, false, false, false, };
		public static bool[] isMyType = new bool[] { false, true, false, true, true, true, true, true, true, };
		public EdgeType_Edge() : base((int) EdgeTypes.@Edge)
		{
		}
		public override string Name { get { return "Edge"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@Edge((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@Edge((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge UEdge ***


	public sealed class @UEdge : GRGEN_LGSP.LGSPEdge, GRGEN_LIBGR.IEdge
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@UEdge[] pool = new GRGEN_MODEL.@UEdge[10];
		
		static @UEdge() {
		}
		
		public @UEdge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_UEdge.typeVar, source, target)
		{
			// implicit initialization, map/set creation of UEdge
		}

		public static GRGEN_MODEL.EdgeType_UEdge TypeInstance { get { return GRGEN_MODEL.EdgeType_UEdge.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@UEdge(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @UEdge(GRGEN_MODEL.@UEdge oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_UEdge.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@UEdge CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@UEdge edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@UEdge(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of UEdge
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@UEdge CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@UEdge edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@UEdge(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of UEdge
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"UEdge\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"UEdge\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of UEdge
		}
	}

	public sealed class EdgeType_UEdge : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_UEdge typeVar = new GRGEN_MODEL.EdgeType_UEdge();
		public static bool[] isA = new bool[] { true, false, true, false, false, false, false, false, false, };
		public static bool[] isMyType = new bool[] { false, false, true, false, false, false, false, false, false, };
		public EdgeType_UEdge() : base((int) EdgeTypes.@UEdge)
		{
		}
		public override string Name { get { return "UEdge"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Undirected; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@UEdge((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@UEdge((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge next ***

	public interface Inext : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @next : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Inext
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@next[] pool = new GRGEN_MODEL.@next[10];
		
		// explicit initializations of next for target next
		// implicit initializations of next for target next
		static @next() {
		}
		
		public @next(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_next.typeVar, source, target)
		{
			// implicit initialization, map/set creation of next
			// explicit initializations of next for target next
		}

		public static GRGEN_MODEL.EdgeType_next TypeInstance { get { return GRGEN_MODEL.EdgeType_next.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@next(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @next(GRGEN_MODEL.@next oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_next.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@next CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@next edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@next(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of next
				// explicit initializations of next for target next
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@next CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@next edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@next(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of next
				// explicit initializations of next for target next
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"next\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"next\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of next
			// explicit initializations of next for target next
		}
	}

	public sealed class EdgeType_next : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_next typeVar = new GRGEN_MODEL.EdgeType_next();
		public static bool[] isA = new bool[] { true, true, false, true, false, false, false, false, false, };
		public static bool[] isMyType = new bool[] { false, false, false, true, false, false, false, false, false, };
		public EdgeType_next() : base((int) EdgeTypes.@next)
		{
		}
		public override string Name { get { return "next"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@next((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@next((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge blocked ***

	public interface Iblocked : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @blocked : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Iblocked
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@blocked[] pool = new GRGEN_MODEL.@blocked[10];
		
		// explicit initializations of blocked for target blocked
		// implicit initializations of blocked for target blocked
		static @blocked() {
		}
		
		public @blocked(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_blocked.typeVar, source, target)
		{
			// implicit initialization, map/set creation of blocked
			// explicit initializations of blocked for target blocked
		}

		public static GRGEN_MODEL.EdgeType_blocked TypeInstance { get { return GRGEN_MODEL.EdgeType_blocked.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@blocked(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @blocked(GRGEN_MODEL.@blocked oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_blocked.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@blocked CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@blocked edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@blocked(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of blocked
				// explicit initializations of blocked for target blocked
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@blocked CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@blocked edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@blocked(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of blocked
				// explicit initializations of blocked for target blocked
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"blocked\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"blocked\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of blocked
			// explicit initializations of blocked for target blocked
		}
	}

	public sealed class EdgeType_blocked : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_blocked typeVar = new GRGEN_MODEL.EdgeType_blocked();
		public static bool[] isA = new bool[] { true, true, false, false, true, false, false, false, false, };
		public static bool[] isMyType = new bool[] { false, false, false, false, true, false, false, false, false, };
		public EdgeType_blocked() : base((int) EdgeTypes.@blocked)
		{
		}
		public override string Name { get { return "blocked"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@blocked((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@blocked((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge held_by ***

	public interface Iheld_by : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @held_by : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Iheld_by
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@held_by[] pool = new GRGEN_MODEL.@held_by[10];
		
		// explicit initializations of held_by for target held_by
		// implicit initializations of held_by for target held_by
		static @held_by() {
		}
		
		public @held_by(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_held_by.typeVar, source, target)
		{
			// implicit initialization, map/set creation of held_by
			// explicit initializations of held_by for target held_by
		}

		public static GRGEN_MODEL.EdgeType_held_by TypeInstance { get { return GRGEN_MODEL.EdgeType_held_by.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@held_by(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @held_by(GRGEN_MODEL.@held_by oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_held_by.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@held_by CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@held_by edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@held_by(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of held_by
				// explicit initializations of held_by for target held_by
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@held_by CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@held_by edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@held_by(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of held_by
				// explicit initializations of held_by for target held_by
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"held_by\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"held_by\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of held_by
			// explicit initializations of held_by for target held_by
		}
	}

	public sealed class EdgeType_held_by : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_held_by typeVar = new GRGEN_MODEL.EdgeType_held_by();
		public static bool[] isA = new bool[] { true, true, false, false, false, true, false, false, false, };
		public static bool[] isMyType = new bool[] { false, false, false, false, false, true, false, false, false, };
		public EdgeType_held_by() : base((int) EdgeTypes.@held_by)
		{
		}
		public override string Name { get { return "held_by"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@held_by((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@held_by((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge token ***

	public interface Itoken : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @token : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Itoken
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@token[] pool = new GRGEN_MODEL.@token[10];
		
		// explicit initializations of token for target token
		// implicit initializations of token for target token
		static @token() {
		}
		
		public @token(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_token.typeVar, source, target)
		{
			// implicit initialization, map/set creation of token
			// explicit initializations of token for target token
		}

		public static GRGEN_MODEL.EdgeType_token TypeInstance { get { return GRGEN_MODEL.EdgeType_token.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@token(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @token(GRGEN_MODEL.@token oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_token.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@token CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@token edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@token(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of token
				// explicit initializations of token for target token
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@token CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@token edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@token(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of token
				// explicit initializations of token for target token
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"token\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"token\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of token
			// explicit initializations of token for target token
		}
	}

	public sealed class EdgeType_token : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_token typeVar = new GRGEN_MODEL.EdgeType_token();
		public static bool[] isA = new bool[] { true, true, false, false, false, false, true, false, false, };
		public static bool[] isMyType = new bool[] { false, false, false, false, false, false, true, false, false, };
		public EdgeType_token() : base((int) EdgeTypes.@token)
		{
		}
		public override string Name { get { return "token"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@token((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@token((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge release ***

	public interface Irelease : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @release : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Irelease
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@release[] pool = new GRGEN_MODEL.@release[10];
		
		// explicit initializations of release for target release
		// implicit initializations of release for target release
		static @release() {
		}
		
		public @release(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_release.typeVar, source, target)
		{
			// implicit initialization, map/set creation of release
			// explicit initializations of release for target release
		}

		public static GRGEN_MODEL.EdgeType_release TypeInstance { get { return GRGEN_MODEL.EdgeType_release.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@release(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @release(GRGEN_MODEL.@release oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_release.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@release CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@release edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@release(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of release
				// explicit initializations of release for target release
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@release CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@release edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@release(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of release
				// explicit initializations of release for target release
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"release\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"release\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of release
			// explicit initializations of release for target release
		}
	}

	public sealed class EdgeType_release : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_release typeVar = new GRGEN_MODEL.EdgeType_release();
		public static bool[] isA = new bool[] { true, true, false, false, false, false, false, true, false, };
		public static bool[] isMyType = new bool[] { false, false, false, false, false, false, false, true, false, };
		public EdgeType_release() : base((int) EdgeTypes.@release)
		{
		}
		public override string Name { get { return "release"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@release((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@release((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	// *** Edge request ***

	public interface Irequest : GRGEN_LIBGR.IEdge
	{
	}

	public sealed class @request : GRGEN_LGSP.LGSPEdge, GRGEN_MODEL.Irequest
	{
		private static int poolLevel = 0;
		private static GRGEN_MODEL.@request[] pool = new GRGEN_MODEL.@request[10];
		
		// explicit initializations of request for target request
		// implicit initializations of request for target request
		static @request() {
		}
		
		public @request(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
			: base(GRGEN_MODEL.EdgeType_request.typeVar, source, target)
		{
			// implicit initialization, map/set creation of request
			// explicit initializations of request for target request
		}

		public static GRGEN_MODEL.EdgeType_request TypeInstance { get { return GRGEN_MODEL.EdgeType_request.typeVar; } }

		public override GRGEN_LIBGR.IEdge Clone(GRGEN_LIBGR.INode newSource, GRGEN_LIBGR.INode newTarget)
		{ return new GRGEN_MODEL.@request(this, (GRGEN_LGSP.LGSPNode) newSource, (GRGEN_LGSP.LGSPNode) newTarget); }

		private @request(GRGEN_MODEL.@request oldElem, GRGEN_LGSP.LGSPNode newSource, GRGEN_LGSP.LGSPNode newTarget)
			: base(GRGEN_MODEL.EdgeType_request.typeVar, newSource, newTarget)
		{
		}
		public static GRGEN_MODEL.@request CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			GRGEN_MODEL.@request edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@request(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of request
				// explicit initializations of request for target request
			}
			graph.AddEdge(edge);
			return edge;
		}

		public static GRGEN_MODEL.@request CreateEdge(GRGEN_LGSP.LGSPGraph graph, GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			GRGEN_MODEL.@request edge;
			if(poolLevel == 0)
				edge = new GRGEN_MODEL.@request(source, target);
			else
			{
				edge = pool[--poolLevel];
				edge.flags &= ~(uint) GRGEN_LGSP.LGSPElemFlags.HAS_VARIABLES;
				edge.source = source;
				edge.target = target;
				// implicit initialization, map/set creation of request
				// explicit initializations of request for target request
			}
			graph.AddEdge(edge, varName);
			return edge;
		}

		public override void Recycle()
		{
			if(poolLevel < 10)
				pool[poolLevel++] = this;
		}

		public override object GetAttribute(string attrName)
		{
			throw new NullReferenceException(
				"The edge type \"request\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void SetAttribute(string attrName, object value)
		{
			throw new NullReferenceException(
				"The edge type \"request\" does not have the attribute \" + attrName + \"\"!");
		}
		public override void ResetAllAttributes()
		{
			// implicit initialization, map/set creation of request
			// explicit initializations of request for target request
		}
	}

	public sealed class EdgeType_request : GRGEN_LIBGR.EdgeType
	{
		public static GRGEN_MODEL.EdgeType_request typeVar = new GRGEN_MODEL.EdgeType_request();
		public static bool[] isA = new bool[] { true, true, false, false, false, false, false, false, true, };
		public static bool[] isMyType = new bool[] { false, false, false, false, false, false, false, false, true, };
		public EdgeType_request() : base((int) EdgeTypes.@request)
		{
		}
		public override string Name { get { return "request"; } }
		public override GRGEN_LIBGR.Directedness Directedness { get { return GRGEN_LIBGR.Directedness.Directed; } }
		public override GRGEN_LIBGR.IEdge CreateEdge(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target)
		{
			return new GRGEN_MODEL.@request((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}
		public override bool IsAbstract { get { return false; } }
		public override bool IsConst { get { return false; } }
		public override int NumAttributes { get { return 0; } }
		public override IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { yield break; } }
		public override GRGEN_LIBGR.AttributeType GetAttributeType(string name) { return null; }
		public override bool IsA(GRGEN_LIBGR.GrGenType other)
		{
			return (this == other) || isA[other.TypeID];
		}
		public override GRGEN_LIBGR.IEdge CreateEdgeWithCopyCommons(GRGEN_LIBGR.INode source, GRGEN_LIBGR.INode target, GRGEN_LIBGR.IEdge oldIEdge)
		{
			return new GRGEN_MODEL.@request((GRGEN_LGSP.LGSPNode) source, (GRGEN_LGSP.LGSPNode) target);
		}

	}

	//
	// Edge model
	//

	public sealed class MutexEdgeModel : GRGEN_LIBGR.IEdgeModel
	{
		public MutexEdgeModel()
		{
			GRGEN_MODEL.EdgeType_AEdge.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_AEdge.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
				GRGEN_MODEL.EdgeType_UEdge.typeVar,
				GRGEN_MODEL.EdgeType_next.typeVar,
				GRGEN_MODEL.EdgeType_blocked.typeVar,
				GRGEN_MODEL.EdgeType_held_by.typeVar,
				GRGEN_MODEL.EdgeType_token.typeVar,
				GRGEN_MODEL.EdgeType_release.typeVar,
				GRGEN_MODEL.EdgeType_request.typeVar,
			};
			GRGEN_MODEL.EdgeType_AEdge.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_AEdge.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
				GRGEN_MODEL.EdgeType_UEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_AEdge.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_AEdge.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_AEdge.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_AEdge.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_Edge.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_Edge.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
				GRGEN_MODEL.EdgeType_next.typeVar,
				GRGEN_MODEL.EdgeType_blocked.typeVar,
				GRGEN_MODEL.EdgeType_held_by.typeVar,
				GRGEN_MODEL.EdgeType_token.typeVar,
				GRGEN_MODEL.EdgeType_release.typeVar,
				GRGEN_MODEL.EdgeType_request.typeVar,
			};
			GRGEN_MODEL.EdgeType_Edge.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_Edge.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_next.typeVar,
				GRGEN_MODEL.EdgeType_blocked.typeVar,
				GRGEN_MODEL.EdgeType_held_by.typeVar,
				GRGEN_MODEL.EdgeType_token.typeVar,
				GRGEN_MODEL.EdgeType_release.typeVar,
				GRGEN_MODEL.EdgeType_request.typeVar,
			};
			GRGEN_MODEL.EdgeType_Edge.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_Edge.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_Edge.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_Edge.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_UEdge.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_UEdge.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_UEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_UEdge.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_UEdge.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_UEdge.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_UEdge.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_UEdge.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_UEdge.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_UEdge.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
			};
			GRGEN_MODEL.EdgeType_next.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_next.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_next.typeVar,
			};
			GRGEN_MODEL.EdgeType_next.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_next.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_next.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_next.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_next.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_next.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_next.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_blocked.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_blocked.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_blocked.typeVar,
			};
			GRGEN_MODEL.EdgeType_blocked.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_blocked.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_blocked.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_blocked.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_blocked.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_blocked.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_blocked.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_held_by.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_held_by.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_held_by.typeVar,
			};
			GRGEN_MODEL.EdgeType_held_by.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_held_by.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_held_by.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_held_by.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_held_by.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_held_by.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_held_by.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_token.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_token.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_token.typeVar,
			};
			GRGEN_MODEL.EdgeType_token.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_token.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_token.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_token.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_token.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_token.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_token.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_release.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_release.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_release.typeVar,
			};
			GRGEN_MODEL.EdgeType_release.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_release.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_release.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_release.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_release.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_release.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_release.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_request.typeVar.subOrSameGrGenTypes = GRGEN_MODEL.EdgeType_request.typeVar.subOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_request.typeVar,
			};
			GRGEN_MODEL.EdgeType_request.typeVar.directSubGrGenTypes = GRGEN_MODEL.EdgeType_request.typeVar.directSubTypes = new GRGEN_LIBGR.EdgeType[] {
			};
			GRGEN_MODEL.EdgeType_request.typeVar.superOrSameGrGenTypes = GRGEN_MODEL.EdgeType_request.typeVar.superOrSameTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_request.typeVar,
				GRGEN_MODEL.EdgeType_AEdge.typeVar,
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
			GRGEN_MODEL.EdgeType_request.typeVar.directSuperGrGenTypes = GRGEN_MODEL.EdgeType_request.typeVar.directSuperTypes = new GRGEN_LIBGR.EdgeType[] {
				GRGEN_MODEL.EdgeType_Edge.typeVar,
			};
		}
		public bool IsNodeModel { get { return false; } }
		public GRGEN_LIBGR.EdgeType RootType { get { return GRGEN_MODEL.EdgeType_AEdge.typeVar; } }
		GRGEN_LIBGR.GrGenType GRGEN_LIBGR.ITypeModel.RootType { get { return GRGEN_MODEL.EdgeType_AEdge.typeVar; } }
		public GRGEN_LIBGR.EdgeType GetType(string name)
		{
			switch(name)
			{
				case "AEdge" : return GRGEN_MODEL.EdgeType_AEdge.typeVar;
				case "Edge" : return GRGEN_MODEL.EdgeType_Edge.typeVar;
				case "UEdge" : return GRGEN_MODEL.EdgeType_UEdge.typeVar;
				case "next" : return GRGEN_MODEL.EdgeType_next.typeVar;
				case "blocked" : return GRGEN_MODEL.EdgeType_blocked.typeVar;
				case "held_by" : return GRGEN_MODEL.EdgeType_held_by.typeVar;
				case "token" : return GRGEN_MODEL.EdgeType_token.typeVar;
				case "release" : return GRGEN_MODEL.EdgeType_release.typeVar;
				case "request" : return GRGEN_MODEL.EdgeType_request.typeVar;
			}
			return null;
		}
		GRGEN_LIBGR.GrGenType GRGEN_LIBGR.ITypeModel.GetType(string name)
		{
			return GetType(name);
		}
		private GRGEN_LIBGR.EdgeType[] types = {
			GRGEN_MODEL.EdgeType_AEdge.typeVar,
			GRGEN_MODEL.EdgeType_Edge.typeVar,
			GRGEN_MODEL.EdgeType_UEdge.typeVar,
			GRGEN_MODEL.EdgeType_next.typeVar,
			GRGEN_MODEL.EdgeType_blocked.typeVar,
			GRGEN_MODEL.EdgeType_held_by.typeVar,
			GRGEN_MODEL.EdgeType_token.typeVar,
			GRGEN_MODEL.EdgeType_release.typeVar,
			GRGEN_MODEL.EdgeType_request.typeVar,
		};
		public GRGEN_LIBGR.EdgeType[] Types { get { return types; } }
		GRGEN_LIBGR.GrGenType[] GRGEN_LIBGR.ITypeModel.Types { get { return types; } }
		private Type[] typeTypes = {
			typeof(GRGEN_MODEL.EdgeType_AEdge),
			typeof(GRGEN_MODEL.EdgeType_Edge),
			typeof(GRGEN_MODEL.EdgeType_UEdge),
			typeof(GRGEN_MODEL.EdgeType_next),
			typeof(GRGEN_MODEL.EdgeType_blocked),
			typeof(GRGEN_MODEL.EdgeType_held_by),
			typeof(GRGEN_MODEL.EdgeType_token),
			typeof(GRGEN_MODEL.EdgeType_release),
			typeof(GRGEN_MODEL.EdgeType_request),
		};
		public Type[] TypeTypes { get { return typeTypes; } }
		private GRGEN_LIBGR.AttributeType[] attributeTypes = {
		};
		public IEnumerable<GRGEN_LIBGR.AttributeType> AttributeTypes { get { return attributeTypes; } }
	}

	//
	// IGraphModel implementation
	//

	public sealed class MutexGraphModel : GRGEN_LIBGR.IGraphModel
	{
		private MutexNodeModel nodeModel = new MutexNodeModel();
		private MutexEdgeModel edgeModel = new MutexEdgeModel();
		private GRGEN_LIBGR.ValidateInfo[] validateInfos = {
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_next.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 0, 1, 0, 1),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_blocked.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 0, long.MaxValue, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_held_by.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_token.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_release.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_request.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, 0, long.MaxValue, 0, long.MaxValue),
		};
		private GRGEN_LIBGR.EnumAttributeType[] enumAttributeTypes = {
		};

		public string ModelName { get { return "Mutex"; } }
		public GRGEN_LIBGR.INodeModel NodeModel { get { return nodeModel; } }
		public GRGEN_LIBGR.IEdgeModel EdgeModel { get { return edgeModel; } }
		public IEnumerable<GRGEN_LIBGR.ValidateInfo> ValidateInfo { get { return validateInfos; } }
		public IEnumerable<GRGEN_LIBGR.EnumAttributeType> EnumAttributeTypes { get { return enumAttributeTypes; } }
		public string MD5Hash { get { return "b2c79abf46750619401de30166fff963"; } }
	}
	//
	// IGraph/IGraphModel implementation
	//

	public class Mutex : GRGEN_LGSP.LGSPGraph, GRGEN_LIBGR.IGraphModel
	{
		public Mutex() : base(GetNextGraphName())
		{
			InitializeGraph(this);
		}

		public GRGEN_MODEL.@Node CreateNodeNode()
		{
			return GRGEN_MODEL.@Node.CreateNode(this);
		}

		public GRGEN_MODEL.@Node CreateNodeNode(string varName)
		{
			return GRGEN_MODEL.@Node.CreateNode(this, varName);
		}

		public GRGEN_MODEL.@Process CreateNodeProcess()
		{
			return GRGEN_MODEL.@Process.CreateNode(this);
		}

		public GRGEN_MODEL.@Process CreateNodeProcess(string varName)
		{
			return GRGEN_MODEL.@Process.CreateNode(this, varName);
		}

		public GRGEN_MODEL.@Resource CreateNodeResource()
		{
			return GRGEN_MODEL.@Resource.CreateNode(this);
		}

		public GRGEN_MODEL.@Resource CreateNodeResource(string varName)
		{
			return GRGEN_MODEL.@Resource.CreateNode(this, varName);
		}

		public @GRGEN_MODEL.@Edge CreateEdgeEdge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@Edge.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@Edge CreateEdgeEdge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@Edge.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@UEdge CreateEdgeUEdge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@UEdge.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@UEdge CreateEdgeUEdge(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@UEdge.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@next CreateEdgenext(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@next.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@next CreateEdgenext(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@next.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@blocked CreateEdgeblocked(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@blocked.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@blocked CreateEdgeblocked(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@blocked.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@held_by CreateEdgeheld_by(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@held_by.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@held_by CreateEdgeheld_by(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@held_by.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@token CreateEdgetoken(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@token.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@token CreateEdgetoken(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@token.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@release CreateEdgerelease(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@release.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@release CreateEdgerelease(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@release.CreateEdge(this, source, target, varName);
		}

		public @GRGEN_MODEL.@request CreateEdgerequest(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target)
		{
			return @GRGEN_MODEL.@request.CreateEdge(this, source, target);
		}

		public @GRGEN_MODEL.@request CreateEdgerequest(GRGEN_LGSP.LGSPNode source, GRGEN_LGSP.LGSPNode target, string varName)
		{
			return @GRGEN_MODEL.@request.CreateEdge(this, source, target, varName);
		}

		private MutexNodeModel nodeModel = new MutexNodeModel();
		private MutexEdgeModel edgeModel = new MutexEdgeModel();
		private GRGEN_LIBGR.ValidateInfo[] validateInfos = {
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_next.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 0, 1, 0, 1),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_blocked.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 0, long.MaxValue, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_held_by.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_token.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_release.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, 1, 1, 0, long.MaxValue),
			new GRGEN_LIBGR.ValidateInfo(GRGEN_MODEL.EdgeType_request.typeVar, GRGEN_MODEL.NodeType_Process.typeVar, GRGEN_MODEL.NodeType_Resource.typeVar, 0, long.MaxValue, 0, long.MaxValue),
		};
		private GRGEN_LIBGR.EnumAttributeType[] enumAttributeTypes = {
		};

		public string ModelName { get { return "Mutex"; } }
		public GRGEN_LIBGR.INodeModel NodeModel { get { return nodeModel; } }
		public GRGEN_LIBGR.IEdgeModel EdgeModel { get { return edgeModel; } }
		public IEnumerable<GRGEN_LIBGR.ValidateInfo> ValidateInfo { get { return validateInfos; } }
		public IEnumerable<GRGEN_LIBGR.EnumAttributeType> EnumAttributeTypes { get { return enumAttributeTypes; } }
		public string MD5Hash { get { return "b2c79abf46750619401de30166fff963"; } }
	}
}