using System;
using System.Collections.Generic;
using System.Text;
using de.unika.ipd.grGen.libGr;
using de.unika.ipd.grGen.lgsp;
using de.unika.ipd.grGen.Model_MutexPimped;

namespace de.unika.ipd.grGen.Action_MutexPimped
{
	public class Rule_aux_attachResource : LGSPRulePattern
	{
		private static Rule_aux_attachResource instance = null;
		public static Rule_aux_attachResource Instance { get { if (instance==null) { instance = new Rule_aux_attachResource(); instance.initialize(); } return instance; } }

		public static NodeType[] aux_attachResource_node_p_AllowedTypes = null;
		public static bool[] aux_attachResource_node_p_IsAllowedType = null;
		public enum aux_attachResource_NodeNums { @p, };
		public enum aux_attachResource_EdgeNums { };
		public enum aux_attachResource_SubNums { };
		public enum aux_attachResource_AltNums { };
		public static NodeType[] aux_attachResource_neg_0_node_r_AllowedTypes = null;
		public static bool[] aux_attachResource_neg_0_node_r_IsAllowedType = null;
		public static EdgeType[] aux_attachResource_neg_0_edge__edge0_AllowedTypes = null;
		public static bool[] aux_attachResource_neg_0_edge__edge0_IsAllowedType = null;
		public enum aux_attachResource_neg_0_NodeNums { @r, @p, };
		public enum aux_attachResource_neg_0_EdgeNums { @_edge0, };
		public enum aux_attachResource_neg_0_SubNums { };
		public enum aux_attachResource_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_aux_attachResource()
#else
		private Rule_aux_attachResource()
#endif
		{
			name = "aux_attachResource";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_aux_attachResource;
			PatternNode aux_attachResource_node_p = new PatternNode((int) NodeTypes.@Process, "aux_attachResource_node_p", "p", aux_attachResource_node_p_AllowedTypes, aux_attachResource_node_p_IsAllowedType, 5.5F, -1);
			PatternGraph aux_attachResource_neg_0;
			PatternNode aux_attachResource_neg_0_node_r = new PatternNode((int) NodeTypes.@Resource, "aux_attachResource_neg_0_node_r", "r", aux_attachResource_neg_0_node_r_AllowedTypes, aux_attachResource_neg_0_node_r_IsAllowedType, 5.5F, -1);
			PatternEdge aux_attachResource_neg_0_edge__edge0 = new PatternEdge(aux_attachResource_neg_0_node_r, aux_attachResource_node_p, true, (int) EdgeTypes.@held_by, "aux_attachResource_neg_0_edge__edge0", "_edge0", aux_attachResource_neg_0_edge__edge0_AllowedTypes, aux_attachResource_neg_0_edge__edge0_IsAllowedType, 5.5F, -1);
			aux_attachResource_neg_0 = new PatternGraph(
				"neg_0",
				"aux_attachResource_",
				false,
				new PatternNode[] { aux_attachResource_neg_0_node_r, aux_attachResource_node_p }, 
				new PatternEdge[] { aux_attachResource_neg_0_edge__edge0 }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_aux_attachResource = new PatternGraph(
				"aux_attachResource",
				"",
				false,
				new PatternNode[] { aux_attachResource_node_p }, 
				new PatternEdge[] {  }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { aux_attachResource_neg_0,  }, 
				new Condition[] {  }, 
				new bool[1, 1] {
					{ true, },
				},
				new bool[0, 0] 			);
			aux_attachResource_node_p.PointOfDefinition = pat_aux_attachResource;
			aux_attachResource_neg_0_node_r.PointOfDefinition = aux_attachResource_neg_0;
			aux_attachResource_neg_0_edge__edge0.PointOfDefinition = aux_attachResource_neg_0;

			patternGraph = pat_aux_attachResource;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) aux_attachResource_NodeNums.@p];
			Node_Resource node_r = Node_Resource.CreateNode(graph);
			Edge_held_by edge__edge0 = Edge_held_by.CreateEdge(graph, node_r, node_p);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) aux_attachResource_NodeNums.@p];
			Node_Resource node_r = Node_Resource.CreateNode(graph);
			Edge_held_by edge__edge0 = Edge_held_by.CreateEdge(graph, node_r, node_p);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] { "r" };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "_edge0" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_blockedRule : LGSPRulePattern
	{
		private static Rule_blockedRule instance = null;
		public static Rule_blockedRule Instance { get { if (instance==null) { instance = new Rule_blockedRule(); instance.initialize(); } return instance; } }

		public static NodeType[] blockedRule_node_p1_AllowedTypes = null;
		public static NodeType[] blockedRule_node_r_AllowedTypes = null;
		public static NodeType[] blockedRule_node_p2_AllowedTypes = null;
		public static bool[] blockedRule_node_p1_IsAllowedType = null;
		public static bool[] blockedRule_node_r_IsAllowedType = null;
		public static bool[] blockedRule_node_p2_IsAllowedType = null;
		public static EdgeType[] blockedRule_edge_req_AllowedTypes = null;
		public static EdgeType[] blockedRule_edge_hb_AllowedTypes = null;
		public static bool[] blockedRule_edge_req_IsAllowedType = null;
		public static bool[] blockedRule_edge_hb_IsAllowedType = null;
		public enum blockedRule_NodeNums { @p1, @r, @p2, };
		public enum blockedRule_EdgeNums { @req, @hb, };
		public enum blockedRule_SubNums { };
		public enum blockedRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_blockedRule()
#else
		private Rule_blockedRule()
#endif
		{
			name = "blockedRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_blockedRule;
			PatternNode blockedRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "blockedRule_node_p1", "p1", blockedRule_node_p1_AllowedTypes, blockedRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode blockedRule_node_r = new PatternNode((int) NodeTypes.@Resource, "blockedRule_node_r", "r", blockedRule_node_r_AllowedTypes, blockedRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode blockedRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "blockedRule_node_p2", "p2", blockedRule_node_p2_AllowedTypes, blockedRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternEdge blockedRule_edge_req = new PatternEdge(blockedRule_node_p1, blockedRule_node_r, true, (int) EdgeTypes.@request, "blockedRule_edge_req", "req", blockedRule_edge_req_AllowedTypes, blockedRule_edge_req_IsAllowedType, 5.5F, -1);
			PatternEdge blockedRule_edge_hb = new PatternEdge(blockedRule_node_r, blockedRule_node_p2, true, (int) EdgeTypes.@held_by, "blockedRule_edge_hb", "hb", blockedRule_edge_hb_AllowedTypes, blockedRule_edge_hb_IsAllowedType, 5.5F, -1);
			pat_blockedRule = new PatternGraph(
				"blockedRule",
				"",
				false,
				new PatternNode[] { blockedRule_node_p1, blockedRule_node_r, blockedRule_node_p2 }, 
				new PatternEdge[] { blockedRule_edge_req, blockedRule_edge_hb }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			blockedRule_node_p1.PointOfDefinition = pat_blockedRule;
			blockedRule_node_r.PointOfDefinition = pat_blockedRule;
			blockedRule_node_p2.PointOfDefinition = pat_blockedRule;
			blockedRule_edge_req.PointOfDefinition = pat_blockedRule;
			blockedRule_edge_hb.PointOfDefinition = pat_blockedRule;

			patternGraph = pat_blockedRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) blockedRule_NodeNums.@r];
			LGSPNode node_p1 = match.Nodes[(int) blockedRule_NodeNums.@p1];
			Edge_blocked edge_b = Edge_blocked.CreateEdge(graph, node_r, node_p1);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) blockedRule_NodeNums.@r];
			LGSPNode node_p1 = match.Nodes[(int) blockedRule_NodeNums.@p1];
			Edge_blocked edge_b = Edge_blocked.CreateEdge(graph, node_r, node_p1);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "b" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_giveRule : LGSPRulePattern
	{
		private static Rule_giveRule instance = null;
		public static Rule_giveRule Instance { get { if (instance==null) { instance = new Rule_giveRule(); instance.initialize(); } return instance; } }

		public static NodeType[] giveRule_node_r_AllowedTypes = null;
		public static NodeType[] giveRule_node_p1_AllowedTypes = null;
		public static NodeType[] giveRule_node_p2_AllowedTypes = null;
		public static bool[] giveRule_node_r_IsAllowedType = null;
		public static bool[] giveRule_node_p1_IsAllowedType = null;
		public static bool[] giveRule_node_p2_IsAllowedType = null;
		public static EdgeType[] giveRule_edge_rel_AllowedTypes = null;
		public static EdgeType[] giveRule_edge_n_AllowedTypes = null;
		public static bool[] giveRule_edge_rel_IsAllowedType = null;
		public static bool[] giveRule_edge_n_IsAllowedType = null;
		public enum giveRule_NodeNums { @r, @p1, @p2, };
		public enum giveRule_EdgeNums { @rel, @n, };
		public enum giveRule_SubNums { };
		public enum giveRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_giveRule()
#else
		private Rule_giveRule()
#endif
		{
			name = "giveRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_giveRule;
			PatternNode giveRule_node_r = new PatternNode((int) NodeTypes.@Resource, "giveRule_node_r", "r", giveRule_node_r_AllowedTypes, giveRule_node_r_IsAllowedType, 5.5F, -1);
			PatternNode giveRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "giveRule_node_p1", "p1", giveRule_node_p1_AllowedTypes, giveRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode giveRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "giveRule_node_p2", "p2", giveRule_node_p2_AllowedTypes, giveRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternEdge giveRule_edge_rel = new PatternEdge(giveRule_node_r, giveRule_node_p1, true, (int) EdgeTypes.@release, "giveRule_edge_rel", "rel", giveRule_edge_rel_AllowedTypes, giveRule_edge_rel_IsAllowedType, 1.0F, -1);
			PatternEdge giveRule_edge_n = new PatternEdge(giveRule_node_p1, giveRule_node_p2, true, (int) EdgeTypes.@next, "giveRule_edge_n", "n", giveRule_edge_n_AllowedTypes, giveRule_edge_n_IsAllowedType, 5.5F, -1);
			pat_giveRule = new PatternGraph(
				"giveRule",
				"",
				false,
				new PatternNode[] { giveRule_node_r, giveRule_node_p1, giveRule_node_p2 }, 
				new PatternEdge[] { giveRule_edge_rel, giveRule_edge_n }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			giveRule_node_r.PointOfDefinition = pat_giveRule;
			giveRule_node_p1.PointOfDefinition = pat_giveRule;
			giveRule_node_p2.PointOfDefinition = pat_giveRule;
			giveRule_edge_rel.PointOfDefinition = pat_giveRule;
			giveRule_edge_n.PointOfDefinition = pat_giveRule;

			patternGraph = pat_giveRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) giveRule_NodeNums.@r];
			LGSPNode node_p2 = match.Nodes[(int) giveRule_NodeNums.@p2];
			LGSPEdge edge_rel = match.Edges[(int) giveRule_EdgeNums.@rel];
			Edge_token edge_t = Edge_token.CreateEdge(graph, node_r, node_p2);
			graph.Remove(edge_rel);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) giveRule_NodeNums.@r];
			LGSPNode node_p2 = match.Nodes[(int) giveRule_NodeNums.@p2];
			LGSPEdge edge_rel = match.Edges[(int) giveRule_EdgeNums.@rel];
			Edge_token edge_t = Edge_token.CreateEdge(graph, node_r, node_p2);
			graph.Remove(edge_rel);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "t" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_ignoreRule : LGSPRulePattern
	{
		private static Rule_ignoreRule instance = null;
		public static Rule_ignoreRule Instance { get { if (instance==null) { instance = new Rule_ignoreRule(); instance.initialize(); } return instance; } }

		public static NodeType[] ignoreRule_node_r_AllowedTypes = null;
		public static NodeType[] ignoreRule_node_p_AllowedTypes = null;
		public static bool[] ignoreRule_node_r_IsAllowedType = null;
		public static bool[] ignoreRule_node_p_IsAllowedType = null;
		public static EdgeType[] ignoreRule_edge_b_AllowedTypes = null;
		public static bool[] ignoreRule_edge_b_IsAllowedType = null;
		public enum ignoreRule_NodeNums { @r, @p, };
		public enum ignoreRule_EdgeNums { @b, };
		public enum ignoreRule_SubNums { };
		public enum ignoreRule_AltNums { };
		public static NodeType[] ignoreRule_neg_0_node_m_AllowedTypes = null;
		public static bool[] ignoreRule_neg_0_node_m_IsAllowedType = null;
		public static EdgeType[] ignoreRule_neg_0_edge_hb_AllowedTypes = null;
		public static bool[] ignoreRule_neg_0_edge_hb_IsAllowedType = null;
		public enum ignoreRule_neg_0_NodeNums { @m, @p, };
		public enum ignoreRule_neg_0_EdgeNums { @hb, };
		public enum ignoreRule_neg_0_SubNums { };
		public enum ignoreRule_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_ignoreRule()
#else
		private Rule_ignoreRule()
#endif
		{
			name = "ignoreRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_ignoreRule;
			PatternNode ignoreRule_node_r = new PatternNode((int) NodeTypes.@Resource, "ignoreRule_node_r", "r", ignoreRule_node_r_AllowedTypes, ignoreRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode ignoreRule_node_p = new PatternNode((int) NodeTypes.@Process, "ignoreRule_node_p", "p", ignoreRule_node_p_AllowedTypes, ignoreRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge ignoreRule_edge_b = new PatternEdge(ignoreRule_node_r, ignoreRule_node_p, true, (int) EdgeTypes.@blocked, "ignoreRule_edge_b", "b", ignoreRule_edge_b_AllowedTypes, ignoreRule_edge_b_IsAllowedType, 5.5F, -1);
			PatternGraph ignoreRule_neg_0;
			PatternNode ignoreRule_neg_0_node_m = new PatternNode((int) NodeTypes.@Resource, "ignoreRule_neg_0_node_m", "m", ignoreRule_neg_0_node_m_AllowedTypes, ignoreRule_neg_0_node_m_IsAllowedType, 5.5F, -1);
			PatternEdge ignoreRule_neg_0_edge_hb = new PatternEdge(ignoreRule_neg_0_node_m, ignoreRule_node_p, true, (int) EdgeTypes.@held_by, "ignoreRule_neg_0_edge_hb", "hb", ignoreRule_neg_0_edge_hb_AllowedTypes, ignoreRule_neg_0_edge_hb_IsAllowedType, 5.5F, -1);
			ignoreRule_neg_0 = new PatternGraph(
				"neg_0",
				"ignoreRule_",
				false,
				new PatternNode[] { ignoreRule_neg_0_node_m, ignoreRule_node_p }, 
				new PatternEdge[] { ignoreRule_neg_0_edge_hb }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_ignoreRule = new PatternGraph(
				"ignoreRule",
				"",
				false,
				new PatternNode[] { ignoreRule_node_r, ignoreRule_node_p }, 
				new PatternEdge[] { ignoreRule_edge_b }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { ignoreRule_neg_0,  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			ignoreRule_node_r.PointOfDefinition = pat_ignoreRule;
			ignoreRule_node_p.PointOfDefinition = pat_ignoreRule;
			ignoreRule_edge_b.PointOfDefinition = pat_ignoreRule;
			ignoreRule_neg_0_node_m.PointOfDefinition = ignoreRule_neg_0;
			ignoreRule_neg_0_edge_hb.PointOfDefinition = ignoreRule_neg_0;

			patternGraph = pat_ignoreRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPEdge edge_b = match.Edges[(int) ignoreRule_EdgeNums.@b];
			graph.Remove(edge_b);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPEdge edge_b = match.Edges[(int) ignoreRule_EdgeNums.@b];
			graph.Remove(edge_b);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] {  };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_killRule : LGSPRulePattern
	{
		private static Rule_killRule instance = null;
		public static Rule_killRule Instance { get { if (instance==null) { instance = new Rule_killRule(); instance.initialize(); } return instance; } }

		public static NodeType[] killRule_node_p1_AllowedTypes = null;
		public static NodeType[] killRule_node_p_AllowedTypes = null;
		public static NodeType[] killRule_node_p2_AllowedTypes = null;
		public static bool[] killRule_node_p1_IsAllowedType = null;
		public static bool[] killRule_node_p_IsAllowedType = null;
		public static bool[] killRule_node_p2_IsAllowedType = null;
		public static EdgeType[] killRule_edge_n1_AllowedTypes = null;
		public static EdgeType[] killRule_edge_n2_AllowedTypes = null;
		public static bool[] killRule_edge_n1_IsAllowedType = null;
		public static bool[] killRule_edge_n2_IsAllowedType = null;
		public enum killRule_NodeNums { @p1, @p, @p2, };
		public enum killRule_EdgeNums { @n1, @n2, };
		public enum killRule_SubNums { };
		public enum killRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_killRule()
#else
		private Rule_killRule()
#endif
		{
			name = "killRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_killRule;
			PatternNode killRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "killRule_node_p1", "p1", killRule_node_p1_AllowedTypes, killRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode killRule_node_p = new PatternNode((int) NodeTypes.@Process, "killRule_node_p", "p", killRule_node_p_AllowedTypes, killRule_node_p_IsAllowedType, 5.5F, -1);
			PatternNode killRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "killRule_node_p2", "p2", killRule_node_p2_AllowedTypes, killRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternEdge killRule_edge_n1 = new PatternEdge(killRule_node_p1, killRule_node_p, true, (int) EdgeTypes.@next, "killRule_edge_n1", "n1", killRule_edge_n1_AllowedTypes, killRule_edge_n1_IsAllowedType, 5.5F, -1);
			PatternEdge killRule_edge_n2 = new PatternEdge(killRule_node_p, killRule_node_p2, true, (int) EdgeTypes.@next, "killRule_edge_n2", "n2", killRule_edge_n2_AllowedTypes, killRule_edge_n2_IsAllowedType, 5.5F, -1);
			pat_killRule = new PatternGraph(
				"killRule",
				"",
				false,
				new PatternNode[] { killRule_node_p1, killRule_node_p, killRule_node_p2 }, 
				new PatternEdge[] { killRule_edge_n1, killRule_edge_n2 }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			killRule_node_p1.PointOfDefinition = pat_killRule;
			killRule_node_p.PointOfDefinition = pat_killRule;
			killRule_node_p2.PointOfDefinition = pat_killRule;
			killRule_edge_n1.PointOfDefinition = pat_killRule;
			killRule_edge_n2.PointOfDefinition = pat_killRule;

			patternGraph = pat_killRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p1 = match.Nodes[(int) killRule_NodeNums.@p1];
			LGSPNode node_p2 = match.Nodes[(int) killRule_NodeNums.@p2];
			LGSPNode node_p = match.Nodes[(int) killRule_NodeNums.@p];
			LGSPEdge edge_n2 = match.Edges[(int) killRule_EdgeNums.@n2];
			LGSPEdge edge_n1 = match.Edges[(int) killRule_EdgeNums.@n1];
			Edge_next edge_n;
			if(edge_n2.type == EdgeType_next.typeVar)
			{
				// re-using edge_n2 as edge_n
				edge_n = (Edge_next) edge_n2;
				graph.ReuseEdge(edge_n2, node_p1, null);
			}
			else
			{
				graph.Remove(edge_n2);
				edge_n = Edge_next.CreateEdge(graph, node_p1, node_p2);
			}
			graph.Remove(edge_n1);
			graph.RemoveEdges(node_p);
			graph.Remove(node_p);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p1 = match.Nodes[(int) killRule_NodeNums.@p1];
			LGSPNode node_p2 = match.Nodes[(int) killRule_NodeNums.@p2];
			LGSPNode node_p = match.Nodes[(int) killRule_NodeNums.@p];
			LGSPEdge edge_n2 = match.Edges[(int) killRule_EdgeNums.@n2];
			LGSPEdge edge_n1 = match.Edges[(int) killRule_EdgeNums.@n1];
			Edge_next edge_n = Edge_next.CreateEdge(graph, node_p1, node_p2);
			graph.Remove(edge_n2);
			graph.Remove(edge_n1);
			graph.RemoveEdges(node_p);
			graph.Remove(node_p);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "n" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_mountRule : LGSPRulePattern
	{
		private static Rule_mountRule instance = null;
		public static Rule_mountRule Instance { get { if (instance==null) { instance = new Rule_mountRule(); instance.initialize(); } return instance; } }

		public static NodeType[] mountRule_node_p_AllowedTypes = null;
		public static bool[] mountRule_node_p_IsAllowedType = null;
		public enum mountRule_NodeNums { @p, };
		public enum mountRule_EdgeNums { };
		public enum mountRule_SubNums { };
		public enum mountRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_mountRule()
#else
		private Rule_mountRule()
#endif
		{
			name = "mountRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_mountRule;
			PatternNode mountRule_node_p = new PatternNode((int) NodeTypes.@Process, "mountRule_node_p", "p", mountRule_node_p_AllowedTypes, mountRule_node_p_IsAllowedType, 5.5F, -1);
			pat_mountRule = new PatternGraph(
				"mountRule",
				"",
				false,
				new PatternNode[] { mountRule_node_p }, 
				new PatternEdge[] {  }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[1, 1] {
					{ true, },
				},
				new bool[0, 0] 			);
			mountRule_node_p.PointOfDefinition = pat_mountRule;

			patternGraph = pat_mountRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) mountRule_NodeNums.@p];
			Node_Resource node_r = Node_Resource.CreateNode(graph);
			Edge_token edge_t = Edge_token.CreateEdge(graph, node_r, node_p);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) mountRule_NodeNums.@p];
			Node_Resource node_r = Node_Resource.CreateNode(graph);
			Edge_token edge_t = Edge_token.CreateEdge(graph, node_r, node_p);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] { "r" };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "t" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_newRule : LGSPRulePattern
	{
		private static Rule_newRule instance = null;
		public static Rule_newRule Instance { get { if (instance==null) { instance = new Rule_newRule(); instance.initialize(); } return instance; } }

		public static NodeType[] newRule_node_p1_AllowedTypes = null;
		public static NodeType[] newRule_node_p2_AllowedTypes = null;
		public static bool[] newRule_node_p1_IsAllowedType = null;
		public static bool[] newRule_node_p2_IsAllowedType = null;
		public static EdgeType[] newRule_edge_n_AllowedTypes = null;
		public static bool[] newRule_edge_n_IsAllowedType = null;
		public enum newRule_NodeNums { @p1, @p2, };
		public enum newRule_EdgeNums { @n, };
		public enum newRule_SubNums { };
		public enum newRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_newRule()
#else
		private Rule_newRule()
#endif
		{
			name = "newRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_newRule;
			PatternNode newRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "newRule_node_p1", "p1", newRule_node_p1_AllowedTypes, newRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode newRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "newRule_node_p2", "p2", newRule_node_p2_AllowedTypes, newRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternEdge newRule_edge_n = new PatternEdge(newRule_node_p1, newRule_node_p2, true, (int) EdgeTypes.@next, "newRule_edge_n", "n", newRule_edge_n_AllowedTypes, newRule_edge_n_IsAllowedType, 1.0F, -1);
			pat_newRule = new PatternGraph(
				"newRule",
				"",
				false,
				new PatternNode[] { newRule_node_p1, newRule_node_p2 }, 
				new PatternEdge[] { newRule_edge_n }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			newRule_node_p1.PointOfDefinition = pat_newRule;
			newRule_node_p2.PointOfDefinition = pat_newRule;
			newRule_edge_n.PointOfDefinition = pat_newRule;

			patternGraph = pat_newRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p2 = match.Nodes[(int) newRule_NodeNums.@p2];
			LGSPNode node_p1 = match.Nodes[(int) newRule_NodeNums.@p1];
			LGSPEdge edge_n = match.Edges[(int) newRule_EdgeNums.@n];
			Node_Process node_p = Node_Process.CreateNode(graph);
			Edge_next edge_n2;
			if(edge_n.type == EdgeType_next.typeVar)
			{
				// re-using edge_n as edge_n2
				edge_n2 = (Edge_next) edge_n;
				graph.ReuseEdge(edge_n, node_p, null);
			}
			else
			{
				graph.Remove(edge_n);
				edge_n2 = Edge_next.CreateEdge(graph, node_p, node_p2);
			}
			Edge_next edge_n1 = Edge_next.CreateEdge(graph, node_p1, node_p);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p2 = match.Nodes[(int) newRule_NodeNums.@p2];
			LGSPNode node_p1 = match.Nodes[(int) newRule_NodeNums.@p1];
			LGSPEdge edge_n = match.Edges[(int) newRule_EdgeNums.@n];
			Node_Process node_p = Node_Process.CreateNode(graph);
			Edge_next edge_n2 = Edge_next.CreateEdge(graph, node_p, node_p2);
			Edge_next edge_n1 = Edge_next.CreateEdge(graph, node_p1, node_p);
			graph.Remove(edge_n);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] { "p" };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "n2", "n1" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_passRule : LGSPRulePattern
	{
		private static Rule_passRule instance = null;
		public static Rule_passRule Instance { get { if (instance==null) { instance = new Rule_passRule(); instance.initialize(); } return instance; } }

		public static NodeType[] passRule_node_r_AllowedTypes = null;
		public static NodeType[] passRule_node_p1_AllowedTypes = null;
		public static NodeType[] passRule_node_p2_AllowedTypes = null;
		public static bool[] passRule_node_r_IsAllowedType = null;
		public static bool[] passRule_node_p1_IsAllowedType = null;
		public static bool[] passRule_node_p2_IsAllowedType = null;
		public static EdgeType[] passRule_edge__edge0_AllowedTypes = null;
		public static EdgeType[] passRule_edge_n_AllowedTypes = null;
		public static bool[] passRule_edge__edge0_IsAllowedType = null;
		public static bool[] passRule_edge_n_IsAllowedType = null;
		public enum passRule_NodeNums { @r, @p1, @p2, };
		public enum passRule_EdgeNums { @_edge0, @n, };
		public enum passRule_SubNums { };
		public enum passRule_AltNums { };
		public static EdgeType[] passRule_neg_0_edge_req_AllowedTypes = null;
		public static bool[] passRule_neg_0_edge_req_IsAllowedType = null;
		public enum passRule_neg_0_NodeNums { @p1, @r, };
		public enum passRule_neg_0_EdgeNums { @req, };
		public enum passRule_neg_0_SubNums { };
		public enum passRule_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_passRule()
#else
		private Rule_passRule()
#endif
		{
			name = "passRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_passRule;
			PatternNode passRule_node_r = new PatternNode((int) NodeTypes.@Resource, "passRule_node_r", "r", passRule_node_r_AllowedTypes, passRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode passRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "passRule_node_p1", "p1", passRule_node_p1_AllowedTypes, passRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode passRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "passRule_node_p2", "p2", passRule_node_p2_AllowedTypes, passRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternEdge passRule_edge__edge0 = new PatternEdge(passRule_node_r, passRule_node_p1, true, (int) EdgeTypes.@token, "passRule_edge__edge0", "_edge0", passRule_edge__edge0_AllowedTypes, passRule_edge__edge0_IsAllowedType, 5.5F, -1);
			PatternEdge passRule_edge_n = new PatternEdge(passRule_node_p1, passRule_node_p2, true, (int) EdgeTypes.@next, "passRule_edge_n", "n", passRule_edge_n_AllowedTypes, passRule_edge_n_IsAllowedType, 5.5F, -1);
			PatternGraph passRule_neg_0;
			PatternEdge passRule_neg_0_edge_req = new PatternEdge(passRule_node_p1, passRule_node_r, true, (int) EdgeTypes.@request, "passRule_neg_0_edge_req", "req", passRule_neg_0_edge_req_AllowedTypes, passRule_neg_0_edge_req_IsAllowedType, 5.5F, -1);
			passRule_neg_0 = new PatternGraph(
				"neg_0",
				"passRule_",
				false,
				new PatternNode[] { passRule_node_p1, passRule_node_r }, 
				new PatternEdge[] { passRule_neg_0_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_passRule = new PatternGraph(
				"passRule",
				"",
				false,
				new PatternNode[] { passRule_node_r, passRule_node_p1, passRule_node_p2 }, 
				new PatternEdge[] { passRule_edge__edge0, passRule_edge_n }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { passRule_neg_0,  }, 
				new Condition[] {  }, 
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			passRule_node_r.PointOfDefinition = pat_passRule;
			passRule_node_p1.PointOfDefinition = pat_passRule;
			passRule_node_p2.PointOfDefinition = pat_passRule;
			passRule_edge__edge0.PointOfDefinition = pat_passRule;
			passRule_edge_n.PointOfDefinition = pat_passRule;
			passRule_neg_0_edge_req.PointOfDefinition = passRule_neg_0;

			patternGraph = pat_passRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) passRule_NodeNums.@r];
			LGSPNode node_p2 = match.Nodes[(int) passRule_NodeNums.@p2];
			LGSPEdge edge__edge0 = match.Edges[(int) passRule_EdgeNums.@_edge0];
			Edge_token edge_t;
			if(edge__edge0.type == EdgeType_token.typeVar)
			{
				// re-using edge__edge0 as edge_t
				edge_t = (Edge_token) edge__edge0;
				graph.ReuseEdge(edge__edge0, null, node_p2);
			}
			else
			{
				graph.Remove(edge__edge0);
				edge_t = Edge_token.CreateEdge(graph, node_r, node_p2);
			}
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) passRule_NodeNums.@r];
			LGSPNode node_p2 = match.Nodes[(int) passRule_NodeNums.@p2];
			LGSPEdge edge__edge0 = match.Edges[(int) passRule_EdgeNums.@_edge0];
			Edge_token edge_t = Edge_token.CreateEdge(graph, node_r, node_p2);
			graph.Remove(edge__edge0);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "t" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_releaseRule : LGSPRulePattern
	{
		private static Rule_releaseRule instance = null;
		public static Rule_releaseRule Instance { get { if (instance==null) { instance = new Rule_releaseRule(); instance.initialize(); } return instance; } }

		public static NodeType[] releaseRule_node_r_AllowedTypes = null;
		public static NodeType[] releaseRule_node_p_AllowedTypes = null;
		public static bool[] releaseRule_node_r_IsAllowedType = null;
		public static bool[] releaseRule_node_p_IsAllowedType = null;
		public static EdgeType[] releaseRule_edge_hb_AllowedTypes = null;
		public static bool[] releaseRule_edge_hb_IsAllowedType = null;
		public enum releaseRule_NodeNums { @r, @p, };
		public enum releaseRule_EdgeNums { @hb, };
		public enum releaseRule_SubNums { };
		public enum releaseRule_AltNums { };
		public static NodeType[] releaseRule_neg_0_node_m_AllowedTypes = null;
		public static bool[] releaseRule_neg_0_node_m_IsAllowedType = null;
		public static EdgeType[] releaseRule_neg_0_edge_req_AllowedTypes = null;
		public static bool[] releaseRule_neg_0_edge_req_IsAllowedType = null;
		public enum releaseRule_neg_0_NodeNums { @p, @m, };
		public enum releaseRule_neg_0_EdgeNums { @req, };
		public enum releaseRule_neg_0_SubNums { };
		public enum releaseRule_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_releaseRule()
#else
		private Rule_releaseRule()
#endif
		{
			name = "releaseRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_releaseRule;
			PatternNode releaseRule_node_r = new PatternNode((int) NodeTypes.@Resource, "releaseRule_node_r", "r", releaseRule_node_r_AllowedTypes, releaseRule_node_r_IsAllowedType, 5.5F, -1);
			PatternNode releaseRule_node_p = new PatternNode((int) NodeTypes.@Process, "releaseRule_node_p", "p", releaseRule_node_p_AllowedTypes, releaseRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge releaseRule_edge_hb = new PatternEdge(releaseRule_node_r, releaseRule_node_p, true, (int) EdgeTypes.@held_by, "releaseRule_edge_hb", "hb", releaseRule_edge_hb_AllowedTypes, releaseRule_edge_hb_IsAllowedType, 1.0F, -1);
			PatternGraph releaseRule_neg_0;
			PatternNode releaseRule_neg_0_node_m = new PatternNode((int) NodeTypes.@Resource, "releaseRule_neg_0_node_m", "m", releaseRule_neg_0_node_m_AllowedTypes, releaseRule_neg_0_node_m_IsAllowedType, 5.5F, -1);
			PatternEdge releaseRule_neg_0_edge_req = new PatternEdge(releaseRule_node_p, releaseRule_neg_0_node_m, true, (int) EdgeTypes.@request, "releaseRule_neg_0_edge_req", "req", releaseRule_neg_0_edge_req_AllowedTypes, releaseRule_neg_0_edge_req_IsAllowedType, 5.5F, -1);
			releaseRule_neg_0 = new PatternGraph(
				"neg_0",
				"releaseRule_",
				false,
				new PatternNode[] { releaseRule_node_p, releaseRule_neg_0_node_m }, 
				new PatternEdge[] { releaseRule_neg_0_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_releaseRule = new PatternGraph(
				"releaseRule",
				"",
				false,
				new PatternNode[] { releaseRule_node_r, releaseRule_node_p }, 
				new PatternEdge[] { releaseRule_edge_hb }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { releaseRule_neg_0,  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			releaseRule_node_r.PointOfDefinition = pat_releaseRule;
			releaseRule_node_p.PointOfDefinition = pat_releaseRule;
			releaseRule_edge_hb.PointOfDefinition = pat_releaseRule;
			releaseRule_neg_0_node_m.PointOfDefinition = releaseRule_neg_0;
			releaseRule_neg_0_edge_req.PointOfDefinition = releaseRule_neg_0;

			patternGraph = pat_releaseRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) releaseRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) releaseRule_NodeNums.@p];
			LGSPEdge edge_hb = match.Edges[(int) releaseRule_EdgeNums.@hb];
			Edge_release edge_rel = Edge_release.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_hb);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) releaseRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) releaseRule_NodeNums.@p];
			LGSPEdge edge_hb = match.Edges[(int) releaseRule_EdgeNums.@hb];
			Edge_release edge_rel = Edge_release.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_hb);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "rel" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_releaseStarRule : LGSPRulePattern
	{
		private static Rule_releaseStarRule instance = null;
		public static Rule_releaseStarRule Instance { get { if (instance==null) { instance = new Rule_releaseStarRule(); instance.initialize(); } return instance; } }

		public static NodeType[] releaseStarRule_node_p1_AllowedTypes = null;
		public static NodeType[] releaseStarRule_node_r1_AllowedTypes = null;
		public static NodeType[] releaseStarRule_node_p2_AllowedTypes = null;
		public static NodeType[] releaseStarRule_node_r2_AllowedTypes = null;
		public static bool[] releaseStarRule_node_p1_IsAllowedType = null;
		public static bool[] releaseStarRule_node_r1_IsAllowedType = null;
		public static bool[] releaseStarRule_node_p2_IsAllowedType = null;
		public static bool[] releaseStarRule_node_r2_IsAllowedType = null;
		public static EdgeType[] releaseStarRule_edge_rq_AllowedTypes = null;
		public static EdgeType[] releaseStarRule_edge_h1_AllowedTypes = null;
		public static EdgeType[] releaseStarRule_edge_h2_AllowedTypes = null;
		public static bool[] releaseStarRule_edge_rq_IsAllowedType = null;
		public static bool[] releaseStarRule_edge_h1_IsAllowedType = null;
		public static bool[] releaseStarRule_edge_h2_IsAllowedType = null;
		public enum releaseStarRule_NodeNums { @p1, @r1, @p2, @r2, };
		public enum releaseStarRule_EdgeNums { @rq, @h1, @h2, };
		public enum releaseStarRule_SubNums { };
		public enum releaseStarRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_releaseStarRule()
#else
		private Rule_releaseStarRule()
#endif
		{
			name = "releaseStarRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_releaseStarRule;
			PatternNode releaseStarRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "releaseStarRule_node_p1", "p1", releaseStarRule_node_p1_AllowedTypes, releaseStarRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode releaseStarRule_node_r1 = new PatternNode((int) NodeTypes.@Resource, "releaseStarRule_node_r1", "r1", releaseStarRule_node_r1_AllowedTypes, releaseStarRule_node_r1_IsAllowedType, 5.5F, -1);
			PatternNode releaseStarRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "releaseStarRule_node_p2", "p2", releaseStarRule_node_p2_AllowedTypes, releaseStarRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternNode releaseStarRule_node_r2 = new PatternNode((int) NodeTypes.@Resource, "releaseStarRule_node_r2", "r2", releaseStarRule_node_r2_AllowedTypes, releaseStarRule_node_r2_IsAllowedType, 5.5F, -1);
			PatternEdge releaseStarRule_edge_rq = new PatternEdge(releaseStarRule_node_p1, releaseStarRule_node_r1, true, (int) EdgeTypes.@request, "releaseStarRule_edge_rq", "rq", releaseStarRule_edge_rq_AllowedTypes, releaseStarRule_edge_rq_IsAllowedType, 5.5F, -1);
			PatternEdge releaseStarRule_edge_h1 = new PatternEdge(releaseStarRule_node_r1, releaseStarRule_node_p2, true, (int) EdgeTypes.@held_by, "releaseStarRule_edge_h1", "h1", releaseStarRule_edge_h1_AllowedTypes, releaseStarRule_edge_h1_IsAllowedType, 5.5F, -1);
			PatternEdge releaseStarRule_edge_h2 = new PatternEdge(releaseStarRule_node_r2, releaseStarRule_node_p2, true, (int) EdgeTypes.@held_by, "releaseStarRule_edge_h2", "h2", releaseStarRule_edge_h2_AllowedTypes, releaseStarRule_edge_h2_IsAllowedType, 5.5F, -1);
			pat_releaseStarRule = new PatternGraph(
				"releaseStarRule",
				"",
				false,
				new PatternNode[] { releaseStarRule_node_p1, releaseStarRule_node_r1, releaseStarRule_node_p2, releaseStarRule_node_r2 }, 
				new PatternEdge[] { releaseStarRule_edge_rq, releaseStarRule_edge_h1, releaseStarRule_edge_h2 }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[4, 4] {
					{ true, false, false, false, },
					{ false, true, false, false, },
					{ false, false, true, false, },
					{ false, false, false, true, },
				},
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				}
			);
			releaseStarRule_node_p1.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_node_r1.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_node_p2.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_node_r2.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_edge_rq.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_edge_h1.PointOfDefinition = pat_releaseStarRule;
			releaseStarRule_edge_h2.PointOfDefinition = pat_releaseStarRule;

			patternGraph = pat_releaseStarRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r1 = match.Nodes[(int) releaseStarRule_NodeNums.@r1];
			LGSPNode node_p2 = match.Nodes[(int) releaseStarRule_NodeNums.@p2];
			LGSPEdge edge_h1 = match.Edges[(int) releaseStarRule_EdgeNums.@h1];
			Edge_release edge_rl = Edge_release.CreateEdge(graph, node_r1, node_p2);
			graph.Remove(edge_h1);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r1 = match.Nodes[(int) releaseStarRule_NodeNums.@r1];
			LGSPNode node_p2 = match.Nodes[(int) releaseStarRule_NodeNums.@p2];
			LGSPEdge edge_h1 = match.Edges[(int) releaseStarRule_EdgeNums.@h1];
			Edge_release edge_rl = Edge_release.CreateEdge(graph, node_r1, node_p2);
			graph.Remove(edge_h1);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "rl" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_requestRule : LGSPRulePattern
	{
		private static Rule_requestRule instance = null;
		public static Rule_requestRule Instance { get { if (instance==null) { instance = new Rule_requestRule(); instance.initialize(); } return instance; } }

		public static NodeType[] requestRule_node_p_AllowedTypes = null;
		public static NodeType[] requestRule_node_r_AllowedTypes = null;
		public static bool[] requestRule_node_p_IsAllowedType = null;
		public static bool[] requestRule_node_r_IsAllowedType = null;
		public enum requestRule_NodeNums { @p, @r, };
		public enum requestRule_EdgeNums { };
		public enum requestRule_SubNums { };
		public enum requestRule_AltNums { };
		public static EdgeType[] requestRule_neg_0_edge_hb_AllowedTypes = null;
		public static bool[] requestRule_neg_0_edge_hb_IsAllowedType = null;
		public enum requestRule_neg_0_NodeNums { @r, @p, };
		public enum requestRule_neg_0_EdgeNums { @hb, };
		public enum requestRule_neg_0_SubNums { };
		public enum requestRule_neg_0_AltNums { };
		public static NodeType[] requestRule_neg_1_node_m_AllowedTypes = null;
		public static bool[] requestRule_neg_1_node_m_IsAllowedType = null;
		public static EdgeType[] requestRule_neg_1_edge_req_AllowedTypes = null;
		public static bool[] requestRule_neg_1_edge_req_IsAllowedType = null;
		public enum requestRule_neg_1_NodeNums { @p, @m, };
		public enum requestRule_neg_1_EdgeNums { @req, };
		public enum requestRule_neg_1_SubNums { };
		public enum requestRule_neg_1_AltNums { };

#if INITIAL_WARMUP
		public Rule_requestRule()
#else
		private Rule_requestRule()
#endif
		{
			name = "requestRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_requestRule;
			PatternNode requestRule_node_p = new PatternNode((int) NodeTypes.@Process, "requestRule_node_p", "p", requestRule_node_p_AllowedTypes, requestRule_node_p_IsAllowedType, 5.5F, -1);
			PatternNode requestRule_node_r = new PatternNode((int) NodeTypes.@Resource, "requestRule_node_r", "r", requestRule_node_r_AllowedTypes, requestRule_node_r_IsAllowedType, 1.0F, -1);
			PatternGraph requestRule_neg_0;
			PatternEdge requestRule_neg_0_edge_hb = new PatternEdge(requestRule_node_r, requestRule_node_p, true, (int) EdgeTypes.@held_by, "requestRule_neg_0_edge_hb", "hb", requestRule_neg_0_edge_hb_AllowedTypes, requestRule_neg_0_edge_hb_IsAllowedType, 5.5F, -1);
			requestRule_neg_0 = new PatternGraph(
				"neg_0",
				"requestRule_",
				false,
				new PatternNode[] { requestRule_node_r, requestRule_node_p }, 
				new PatternEdge[] { requestRule_neg_0_edge_hb }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			PatternGraph requestRule_neg_1;
			PatternNode requestRule_neg_1_node_m = new PatternNode((int) NodeTypes.@Resource, "requestRule_neg_1_node_m", "m", requestRule_neg_1_node_m_AllowedTypes, requestRule_neg_1_node_m_IsAllowedType, 5.5F, -1);
			PatternEdge requestRule_neg_1_edge_req = new PatternEdge(requestRule_node_p, requestRule_neg_1_node_m, true, (int) EdgeTypes.@request, "requestRule_neg_1_edge_req", "req", requestRule_neg_1_edge_req_AllowedTypes, requestRule_neg_1_edge_req_IsAllowedType, 5.5F, -1);
			requestRule_neg_1 = new PatternGraph(
				"neg_1",
				"requestRule_",
				false,
				new PatternNode[] { requestRule_node_p, requestRule_neg_1_node_m }, 
				new PatternEdge[] { requestRule_neg_1_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_requestRule = new PatternGraph(
				"requestRule",
				"",
				false,
				new PatternNode[] { requestRule_node_p, requestRule_node_r }, 
				new PatternEdge[] {  }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { requestRule_neg_0, requestRule_neg_1,  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[0, 0] 			);
			requestRule_node_p.PointOfDefinition = pat_requestRule;
			requestRule_node_r.PointOfDefinition = pat_requestRule;
			requestRule_neg_0_edge_hb.PointOfDefinition = requestRule_neg_0;
			requestRule_neg_1_node_m.PointOfDefinition = requestRule_neg_1;
			requestRule_neg_1_edge_req.PointOfDefinition = requestRule_neg_1;

			patternGraph = pat_requestRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) requestRule_NodeNums.@p];
			LGSPNode node_r = match.Nodes[(int) requestRule_NodeNums.@r];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p, node_r);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) requestRule_NodeNums.@p];
			LGSPNode node_r = match.Nodes[(int) requestRule_NodeNums.@r];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p, node_r);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "req" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_requestSimpleRule : LGSPRulePattern
	{
		private static Rule_requestSimpleRule instance = null;
		public static Rule_requestSimpleRule Instance { get { if (instance==null) { instance = new Rule_requestSimpleRule(); instance.initialize(); } return instance; } }

		public static NodeType[] requestSimpleRule_node_r_AllowedTypes = null;
		public static NodeType[] requestSimpleRule_node_p_AllowedTypes = null;
		public static bool[] requestSimpleRule_node_r_IsAllowedType = null;
		public static bool[] requestSimpleRule_node_p_IsAllowedType = null;
		public static EdgeType[] requestSimpleRule_edge_t_AllowedTypes = null;
		public static bool[] requestSimpleRule_edge_t_IsAllowedType = null;
		public enum requestSimpleRule_NodeNums { @r, @p, };
		public enum requestSimpleRule_EdgeNums { @t, };
		public enum requestSimpleRule_SubNums { };
		public enum requestSimpleRule_AltNums { };
		public static EdgeType[] requestSimpleRule_neg_0_edge_req_AllowedTypes = null;
		public static bool[] requestSimpleRule_neg_0_edge_req_IsAllowedType = null;
		public enum requestSimpleRule_neg_0_NodeNums { @p, @r, };
		public enum requestSimpleRule_neg_0_EdgeNums { @req, };
		public enum requestSimpleRule_neg_0_SubNums { };
		public enum requestSimpleRule_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_requestSimpleRule()
#else
		private Rule_requestSimpleRule()
#endif
		{
			name = "requestSimpleRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_requestSimpleRule;
			PatternNode requestSimpleRule_node_r = new PatternNode((int) NodeTypes.@Resource, "requestSimpleRule_node_r", "r", requestSimpleRule_node_r_AllowedTypes, requestSimpleRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode requestSimpleRule_node_p = new PatternNode((int) NodeTypes.@Process, "requestSimpleRule_node_p", "p", requestSimpleRule_node_p_AllowedTypes, requestSimpleRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge requestSimpleRule_edge_t = new PatternEdge(requestSimpleRule_node_r, requestSimpleRule_node_p, true, (int) EdgeTypes.@token, "requestSimpleRule_edge_t", "t", requestSimpleRule_edge_t_AllowedTypes, requestSimpleRule_edge_t_IsAllowedType, 5.5F, -1);
			PatternGraph requestSimpleRule_neg_0;
			PatternEdge requestSimpleRule_neg_0_edge_req = new PatternEdge(requestSimpleRule_node_p, requestSimpleRule_node_r, true, (int) EdgeTypes.@request, "requestSimpleRule_neg_0_edge_req", "req", requestSimpleRule_neg_0_edge_req_AllowedTypes, requestSimpleRule_neg_0_edge_req_IsAllowedType, 5.5F, -1);
			requestSimpleRule_neg_0 = new PatternGraph(
				"neg_0",
				"requestSimpleRule_",
				false,
				new PatternNode[] { requestSimpleRule_node_p, requestSimpleRule_node_r }, 
				new PatternEdge[] { requestSimpleRule_neg_0_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_requestSimpleRule = new PatternGraph(
				"requestSimpleRule",
				"",
				false,
				new PatternNode[] { requestSimpleRule_node_r, requestSimpleRule_node_p }, 
				new PatternEdge[] { requestSimpleRule_edge_t }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { requestSimpleRule_neg_0,  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			requestSimpleRule_node_r.PointOfDefinition = pat_requestSimpleRule;
			requestSimpleRule_node_p.PointOfDefinition = pat_requestSimpleRule;
			requestSimpleRule_edge_t.PointOfDefinition = pat_requestSimpleRule;
			requestSimpleRule_neg_0_edge_req.PointOfDefinition = requestSimpleRule_neg_0;

			patternGraph = pat_requestSimpleRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) requestSimpleRule_NodeNums.@p];
			LGSPNode node_r = match.Nodes[(int) requestSimpleRule_NodeNums.@r];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p, node_r);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p = match.Nodes[(int) requestSimpleRule_NodeNums.@p];
			LGSPNode node_r = match.Nodes[(int) requestSimpleRule_NodeNums.@r];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p, node_r);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "req" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_requestStarRule : LGSPRulePattern
	{
		private static Rule_requestStarRule instance = null;
		public static Rule_requestStarRule Instance { get { if (instance==null) { instance = new Rule_requestStarRule(); instance.initialize(); } return instance; } }

		public static NodeType[] requestStarRule_node_r1_AllowedTypes = null;
		public static NodeType[] requestStarRule_node_p1_AllowedTypes = null;
		public static NodeType[] requestStarRule_node_p2_AllowedTypes = null;
		public static NodeType[] requestStarRule_node_r2_AllowedTypes = null;
		public static bool[] requestStarRule_node_r1_IsAllowedType = null;
		public static bool[] requestStarRule_node_p1_IsAllowedType = null;
		public static bool[] requestStarRule_node_p2_IsAllowedType = null;
		public static bool[] requestStarRule_node_r2_IsAllowedType = null;
		public static EdgeType[] requestStarRule_edge_h1_AllowedTypes = null;
		public static EdgeType[] requestStarRule_edge_n_AllowedTypes = null;
		public static EdgeType[] requestStarRule_edge_h2_AllowedTypes = null;
		public static bool[] requestStarRule_edge_h1_IsAllowedType = null;
		public static bool[] requestStarRule_edge_n_IsAllowedType = null;
		public static bool[] requestStarRule_edge_h2_IsAllowedType = null;
		public enum requestStarRule_NodeNums { @r1, @p1, @p2, @r2, };
		public enum requestStarRule_EdgeNums { @h1, @n, @h2, };
		public enum requestStarRule_SubNums { };
		public enum requestStarRule_AltNums { };
		public static EdgeType[] requestStarRule_neg_0_edge_req_AllowedTypes = null;
		public static bool[] requestStarRule_neg_0_edge_req_IsAllowedType = null;
		public enum requestStarRule_neg_0_NodeNums { @p1, @r2, };
		public enum requestStarRule_neg_0_EdgeNums { @req, };
		public enum requestStarRule_neg_0_SubNums { };
		public enum requestStarRule_neg_0_AltNums { };

#if INITIAL_WARMUP
		public Rule_requestStarRule()
#else
		private Rule_requestStarRule()
#endif
		{
			name = "requestStarRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_requestStarRule;
			PatternNode requestStarRule_node_r1 = new PatternNode((int) NodeTypes.@Resource, "requestStarRule_node_r1", "r1", requestStarRule_node_r1_AllowedTypes, requestStarRule_node_r1_IsAllowedType, 5.5F, -1);
			PatternNode requestStarRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "requestStarRule_node_p1", "p1", requestStarRule_node_p1_AllowedTypes, requestStarRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode requestStarRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "requestStarRule_node_p2", "p2", requestStarRule_node_p2_AllowedTypes, requestStarRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternNode requestStarRule_node_r2 = new PatternNode((int) NodeTypes.@Resource, "requestStarRule_node_r2", "r2", requestStarRule_node_r2_AllowedTypes, requestStarRule_node_r2_IsAllowedType, 5.5F, -1);
			PatternEdge requestStarRule_edge_h1 = new PatternEdge(requestStarRule_node_r1, requestStarRule_node_p1, true, (int) EdgeTypes.@held_by, "requestStarRule_edge_h1", "h1", requestStarRule_edge_h1_AllowedTypes, requestStarRule_edge_h1_IsAllowedType, 5.5F, -1);
			PatternEdge requestStarRule_edge_n = new PatternEdge(requestStarRule_node_p2, requestStarRule_node_p1, true, (int) EdgeTypes.@next, "requestStarRule_edge_n", "n", requestStarRule_edge_n_AllowedTypes, requestStarRule_edge_n_IsAllowedType, 5.5F, -1);
			PatternEdge requestStarRule_edge_h2 = new PatternEdge(requestStarRule_node_r2, requestStarRule_node_p2, true, (int) EdgeTypes.@held_by, "requestStarRule_edge_h2", "h2", requestStarRule_edge_h2_AllowedTypes, requestStarRule_edge_h2_IsAllowedType, 5.5F, -1);
			PatternGraph requestStarRule_neg_0;
			PatternEdge requestStarRule_neg_0_edge_req = new PatternEdge(requestStarRule_node_p1, requestStarRule_node_r2, true, (int) EdgeTypes.@request, "requestStarRule_neg_0_edge_req", "req", requestStarRule_neg_0_edge_req_AllowedTypes, requestStarRule_neg_0_edge_req_IsAllowedType, 5.5F, -1);
			requestStarRule_neg_0 = new PatternGraph(
				"neg_0",
				"requestStarRule_",
				false,
				new PatternNode[] { requestStarRule_node_p1, requestStarRule_node_r2 }, 
				new PatternEdge[] { requestStarRule_neg_0_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			pat_requestStarRule = new PatternGraph(
				"requestStarRule",
				"",
				false,
				new PatternNode[] { requestStarRule_node_r1, requestStarRule_node_p1, requestStarRule_node_p2, requestStarRule_node_r2 }, 
				new PatternEdge[] { requestStarRule_edge_h1, requestStarRule_edge_n, requestStarRule_edge_h2 }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] { requestStarRule_neg_0,  }, 
				new Condition[] {  }, 
				new bool[4, 4] {
					{ true, false, false, false, },
					{ false, true, false, false, },
					{ false, false, true, false, },
					{ false, false, false, true, },
				},
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				}
			);
			requestStarRule_node_r1.PointOfDefinition = pat_requestStarRule;
			requestStarRule_node_p1.PointOfDefinition = pat_requestStarRule;
			requestStarRule_node_p2.PointOfDefinition = pat_requestStarRule;
			requestStarRule_node_r2.PointOfDefinition = pat_requestStarRule;
			requestStarRule_edge_h1.PointOfDefinition = pat_requestStarRule;
			requestStarRule_edge_n.PointOfDefinition = pat_requestStarRule;
			requestStarRule_edge_h2.PointOfDefinition = pat_requestStarRule;
			requestStarRule_neg_0_edge_req.PointOfDefinition = requestStarRule_neg_0;

			patternGraph = pat_requestStarRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p1 = match.Nodes[(int) requestStarRule_NodeNums.@p1];
			LGSPNode node_r2 = match.Nodes[(int) requestStarRule_NodeNums.@r2];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p1, node_r2);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_p1 = match.Nodes[(int) requestStarRule_NodeNums.@p1];
			LGSPNode node_r2 = match.Nodes[(int) requestStarRule_NodeNums.@r2];
			Edge_request edge_req = Edge_request.CreateEdge(graph, node_p1, node_r2);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "req" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_takeRule : LGSPRulePattern
	{
		private static Rule_takeRule instance = null;
		public static Rule_takeRule Instance { get { if (instance==null) { instance = new Rule_takeRule(); instance.initialize(); } return instance; } }

		public static NodeType[] takeRule_node_r_AllowedTypes = null;
		public static NodeType[] takeRule_node_p_AllowedTypes = null;
		public static bool[] takeRule_node_r_IsAllowedType = null;
		public static bool[] takeRule_node_p_IsAllowedType = null;
		public static EdgeType[] takeRule_edge_t_AllowedTypes = null;
		public static EdgeType[] takeRule_edge_req_AllowedTypes = null;
		public static bool[] takeRule_edge_t_IsAllowedType = null;
		public static bool[] takeRule_edge_req_IsAllowedType = null;
		public enum takeRule_NodeNums { @r, @p, };
		public enum takeRule_EdgeNums { @t, @req, };
		public enum takeRule_SubNums { };
		public enum takeRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_takeRule()
#else
		private Rule_takeRule()
#endif
		{
			name = "takeRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_takeRule;
			PatternNode takeRule_node_r = new PatternNode((int) NodeTypes.@Resource, "takeRule_node_r", "r", takeRule_node_r_AllowedTypes, takeRule_node_r_IsAllowedType, 5.5F, -1);
			PatternNode takeRule_node_p = new PatternNode((int) NodeTypes.@Process, "takeRule_node_p", "p", takeRule_node_p_AllowedTypes, takeRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge takeRule_edge_t = new PatternEdge(takeRule_node_r, takeRule_node_p, true, (int) EdgeTypes.@token, "takeRule_edge_t", "t", takeRule_edge_t_AllowedTypes, takeRule_edge_t_IsAllowedType, 1.0F, -1);
			PatternEdge takeRule_edge_req = new PatternEdge(takeRule_node_p, takeRule_node_r, true, (int) EdgeTypes.@request, "takeRule_edge_req", "req", takeRule_edge_req_AllowedTypes, takeRule_edge_req_IsAllowedType, 5.5F, -1);
			pat_takeRule = new PatternGraph(
				"takeRule",
				"",
				false,
				new PatternNode[] { takeRule_node_r, takeRule_node_p }, 
				new PatternEdge[] { takeRule_edge_t, takeRule_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			takeRule_node_r.PointOfDefinition = pat_takeRule;
			takeRule_node_p.PointOfDefinition = pat_takeRule;
			takeRule_edge_t.PointOfDefinition = pat_takeRule;
			takeRule_edge_req.PointOfDefinition = pat_takeRule;

			patternGraph = pat_takeRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) takeRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) takeRule_NodeNums.@p];
			LGSPEdge edge_t = match.Edges[(int) takeRule_EdgeNums.@t];
			LGSPEdge edge_req = match.Edges[(int) takeRule_EdgeNums.@req];
			Edge_held_by edge_hb = Edge_held_by.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_t);
			graph.Remove(edge_req);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) takeRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) takeRule_NodeNums.@p];
			LGSPEdge edge_t = match.Edges[(int) takeRule_EdgeNums.@t];
			LGSPEdge edge_req = match.Edges[(int) takeRule_EdgeNums.@req];
			Edge_held_by edge_hb = Edge_held_by.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_t);
			graph.Remove(edge_req);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "hb" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_unlockRule : LGSPRulePattern
	{
		private static Rule_unlockRule instance = null;
		public static Rule_unlockRule Instance { get { if (instance==null) { instance = new Rule_unlockRule(); instance.initialize(); } return instance; } }

		public static NodeType[] unlockRule_node_r_AllowedTypes = null;
		public static NodeType[] unlockRule_node_p_AllowedTypes = null;
		public static bool[] unlockRule_node_r_IsAllowedType = null;
		public static bool[] unlockRule_node_p_IsAllowedType = null;
		public static EdgeType[] unlockRule_edge_b_AllowedTypes = null;
		public static EdgeType[] unlockRule_edge_hb_AllowedTypes = null;
		public static bool[] unlockRule_edge_b_IsAllowedType = null;
		public static bool[] unlockRule_edge_hb_IsAllowedType = null;
		public enum unlockRule_NodeNums { @r, @p, };
		public enum unlockRule_EdgeNums { @b, @hb, };
		public enum unlockRule_SubNums { };
		public enum unlockRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_unlockRule()
#else
		private Rule_unlockRule()
#endif
		{
			name = "unlockRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_unlockRule;
			PatternNode unlockRule_node_r = new PatternNode((int) NodeTypes.@Resource, "unlockRule_node_r", "r", unlockRule_node_r_AllowedTypes, unlockRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode unlockRule_node_p = new PatternNode((int) NodeTypes.@Process, "unlockRule_node_p", "p", unlockRule_node_p_AllowedTypes, unlockRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge unlockRule_edge_b = new PatternEdge(unlockRule_node_r, unlockRule_node_p, true, (int) EdgeTypes.@blocked, "unlockRule_edge_b", "b", unlockRule_edge_b_AllowedTypes, unlockRule_edge_b_IsAllowedType, 5.5F, -1);
			PatternEdge unlockRule_edge_hb = new PatternEdge(unlockRule_node_r, unlockRule_node_p, true, (int) EdgeTypes.@held_by, "unlockRule_edge_hb", "hb", unlockRule_edge_hb_AllowedTypes, unlockRule_edge_hb_IsAllowedType, 5.5F, -1);
			pat_unlockRule = new PatternGraph(
				"unlockRule",
				"",
				false,
				new PatternNode[] { unlockRule_node_r, unlockRule_node_p }, 
				new PatternEdge[] { unlockRule_edge_b, unlockRule_edge_hb }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				}
			);
			unlockRule_node_r.PointOfDefinition = pat_unlockRule;
			unlockRule_node_p.PointOfDefinition = pat_unlockRule;
			unlockRule_edge_b.PointOfDefinition = pat_unlockRule;
			unlockRule_edge_hb.PointOfDefinition = pat_unlockRule;

			patternGraph = pat_unlockRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) unlockRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) unlockRule_NodeNums.@p];
			LGSPEdge edge_b = match.Edges[(int) unlockRule_EdgeNums.@b];
			LGSPEdge edge_hb = match.Edges[(int) unlockRule_EdgeNums.@hb];
			Edge_release edge_rel = Edge_release.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_b);
			graph.Remove(edge_hb);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) unlockRule_NodeNums.@r];
			LGSPNode node_p = match.Nodes[(int) unlockRule_NodeNums.@p];
			LGSPEdge edge_b = match.Edges[(int) unlockRule_EdgeNums.@b];
			LGSPEdge edge_hb = match.Edges[(int) unlockRule_EdgeNums.@hb];
			Edge_release edge_rel = Edge_release.CreateEdge(graph, node_r, node_p);
			graph.Remove(edge_b);
			graph.Remove(edge_hb);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "rel" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_unmountRule : LGSPRulePattern
	{
		private static Rule_unmountRule instance = null;
		public static Rule_unmountRule Instance { get { if (instance==null) { instance = new Rule_unmountRule(); instance.initialize(); } return instance; } }

		public static NodeType[] unmountRule_node_r_AllowedTypes = null;
		public static NodeType[] unmountRule_node_p_AllowedTypes = null;
		public static bool[] unmountRule_node_r_IsAllowedType = null;
		public static bool[] unmountRule_node_p_IsAllowedType = null;
		public static EdgeType[] unmountRule_edge_t_AllowedTypes = null;
		public static bool[] unmountRule_edge_t_IsAllowedType = null;
		public enum unmountRule_NodeNums { @r, @p, };
		public enum unmountRule_EdgeNums { @t, };
		public enum unmountRule_SubNums { };
		public enum unmountRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_unmountRule()
#else
		private Rule_unmountRule()
#endif
		{
			name = "unmountRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_unmountRule;
			PatternNode unmountRule_node_r = new PatternNode((int) NodeTypes.@Resource, "unmountRule_node_r", "r", unmountRule_node_r_AllowedTypes, unmountRule_node_r_IsAllowedType, 1.0F, -1);
			PatternNode unmountRule_node_p = new PatternNode((int) NodeTypes.@Process, "unmountRule_node_p", "p", unmountRule_node_p_AllowedTypes, unmountRule_node_p_IsAllowedType, 5.5F, -1);
			PatternEdge unmountRule_edge_t = new PatternEdge(unmountRule_node_r, unmountRule_node_p, true, (int) EdgeTypes.@token, "unmountRule_edge_t", "t", unmountRule_edge_t_AllowedTypes, unmountRule_edge_t_IsAllowedType, 5.5F, -1);
			pat_unmountRule = new PatternGraph(
				"unmountRule",
				"",
				false,
				new PatternNode[] { unmountRule_node_r, unmountRule_node_p }, 
				new PatternEdge[] { unmountRule_edge_t }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[2, 2] {
					{ true, false, },
					{ false, true, },
				},
				new bool[1, 1] {
					{ true, },
				}
			);
			unmountRule_node_r.PointOfDefinition = pat_unmountRule;
			unmountRule_node_p.PointOfDefinition = pat_unmountRule;
			unmountRule_edge_t.PointOfDefinition = pat_unmountRule;

			patternGraph = pat_unmountRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) unmountRule_NodeNums.@r];
			LGSPEdge edge_t = match.Edges[(int) unmountRule_EdgeNums.@t];
			graph.Remove(edge_t);
			graph.RemoveEdges(node_r);
			graph.Remove(node_r);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r = match.Nodes[(int) unmountRule_NodeNums.@r];
			LGSPEdge edge_t = match.Edges[(int) unmountRule_EdgeNums.@t];
			graph.Remove(edge_t);
			graph.RemoveEdges(node_r);
			graph.Remove(node_r);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] {  };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}

	public class Rule_waitingRule : LGSPRulePattern
	{
		private static Rule_waitingRule instance = null;
		public static Rule_waitingRule Instance { get { if (instance==null) { instance = new Rule_waitingRule(); instance.initialize(); } return instance; } }

		public static NodeType[] waitingRule_node_r2_AllowedTypes = null;
		public static NodeType[] waitingRule_node_p1_AllowedTypes = null;
		public static NodeType[] waitingRule_node_r1_AllowedTypes = null;
		public static NodeType[] waitingRule_node_p2_AllowedTypes = null;
		public static NodeType[] waitingRule_node_r_AllowedTypes = null;
		public static bool[] waitingRule_node_r2_IsAllowedType = null;
		public static bool[] waitingRule_node_p1_IsAllowedType = null;
		public static bool[] waitingRule_node_r1_IsAllowedType = null;
		public static bool[] waitingRule_node_p2_IsAllowedType = null;
		public static bool[] waitingRule_node_r_IsAllowedType = null;
		public static EdgeType[] waitingRule_edge_b_AllowedTypes = null;
		public static EdgeType[] waitingRule_edge_hb_AllowedTypes = null;
		public static EdgeType[] waitingRule_edge_req_AllowedTypes = null;
		public static bool[] waitingRule_edge_b_IsAllowedType = null;
		public static bool[] waitingRule_edge_hb_IsAllowedType = null;
		public static bool[] waitingRule_edge_req_IsAllowedType = null;
		public enum waitingRule_NodeNums { @r2, @p1, @r1, @p2, @r, };
		public enum waitingRule_EdgeNums { @b, @hb, @req, };
		public enum waitingRule_SubNums { };
		public enum waitingRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_waitingRule()
#else
		private Rule_waitingRule()
#endif
		{
			name = "waitingRule";
			isSubpattern = false;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}
		public override void initialize()
		{
			PatternGraph pat_waitingRule;
			PatternNode waitingRule_node_r2 = new PatternNode((int) NodeTypes.@Resource, "waitingRule_node_r2", "r2", waitingRule_node_r2_AllowedTypes, waitingRule_node_r2_IsAllowedType, 5.5F, -1);
			PatternNode waitingRule_node_p1 = new PatternNode((int) NodeTypes.@Process, "waitingRule_node_p1", "p1", waitingRule_node_p1_AllowedTypes, waitingRule_node_p1_IsAllowedType, 5.5F, -1);
			PatternNode waitingRule_node_r1 = new PatternNode((int) NodeTypes.@Resource, "waitingRule_node_r1", "r1", waitingRule_node_r1_AllowedTypes, waitingRule_node_r1_IsAllowedType, 5.5F, -1);
			PatternNode waitingRule_node_p2 = new PatternNode((int) NodeTypes.@Process, "waitingRule_node_p2", "p2", waitingRule_node_p2_AllowedTypes, waitingRule_node_p2_IsAllowedType, 5.5F, -1);
			PatternNode waitingRule_node_r = new PatternNode((int) NodeTypes.@Resource, "waitingRule_node_r", "r", waitingRule_node_r_AllowedTypes, waitingRule_node_r_IsAllowedType, 1.0F, -1);
			PatternEdge waitingRule_edge_b = new PatternEdge(waitingRule_node_r2, waitingRule_node_p1, true, (int) EdgeTypes.@blocked, "waitingRule_edge_b", "b", waitingRule_edge_b_AllowedTypes, waitingRule_edge_b_IsAllowedType, 5.5F, -1);
			PatternEdge waitingRule_edge_hb = new PatternEdge(waitingRule_node_r1, waitingRule_node_p1, true, (int) EdgeTypes.@held_by, "waitingRule_edge_hb", "hb", waitingRule_edge_hb_AllowedTypes, waitingRule_edge_hb_IsAllowedType, 5.5F, -1);
			PatternEdge waitingRule_edge_req = new PatternEdge(waitingRule_node_p2, waitingRule_node_r1, true, (int) EdgeTypes.@request, "waitingRule_edge_req", "req", waitingRule_edge_req_AllowedTypes, waitingRule_edge_req_IsAllowedType, 5.5F, -1);
			pat_waitingRule = new PatternGraph(
				"waitingRule",
				"",
				false,
				new PatternNode[] { waitingRule_node_r2, waitingRule_node_p1, waitingRule_node_r1, waitingRule_node_p2, waitingRule_node_r }, 
				new PatternEdge[] { waitingRule_edge_b, waitingRule_edge_hb, waitingRule_edge_req }, 
				new PatternGraphEmbedding[] {  }, 
				new Alternative[] {  }, 
				new PatternGraph[] {  }, 
				new Condition[] {  }, 
				new bool[5, 5] {
					{ true, false, false, false, false, },
					{ false, true, false, false, false, },
					{ false, false, true, false, false, },
					{ false, false, false, true, false, },
					{ false, false, false, false, true, },
				},
				new bool[3, 3] {
					{ true, false, false, },
					{ false, true, false, },
					{ false, false, true, },
				}
			);
			waitingRule_node_r2.PointOfDefinition = pat_waitingRule;
			waitingRule_node_p1.PointOfDefinition = pat_waitingRule;
			waitingRule_node_r1.PointOfDefinition = pat_waitingRule;
			waitingRule_node_p2.PointOfDefinition = pat_waitingRule;
			waitingRule_node_r.PointOfDefinition = pat_waitingRule;
			waitingRule_edge_b.PointOfDefinition = pat_waitingRule;
			waitingRule_edge_hb.PointOfDefinition = pat_waitingRule;
			waitingRule_edge_req.PointOfDefinition = pat_waitingRule;

			patternGraph = pat_waitingRule;
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r2 = match.Nodes[(int) waitingRule_NodeNums.@r2];
			LGSPNode node_p2 = match.Nodes[(int) waitingRule_NodeNums.@p2];
			LGSPNode node_r = match.Nodes[(int) waitingRule_NodeNums.@r];
			LGSPEdge edge_b = match.Edges[(int) waitingRule_EdgeNums.@b];
			Edge_blocked edge_bn;
			if(edge_b.type == EdgeType_blocked.typeVar)
			{
				// re-using edge_b as edge_bn
				edge_bn = (Edge_blocked) edge_b;
				graph.ReuseEdge(edge_b, null, node_p2);
			}
			else
			{
				graph.Remove(edge_b);
				edge_bn = Edge_blocked.CreateEdge(graph, node_r2, node_p2);
			}
			graph.RemoveEdges(node_r);
			graph.Remove(node_r);
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_r2 = match.Nodes[(int) waitingRule_NodeNums.@r2];
			LGSPNode node_p2 = match.Nodes[(int) waitingRule_NodeNums.@p2];
			LGSPNode node_r = match.Nodes[(int) waitingRule_NodeNums.@r];
			LGSPEdge edge_b = match.Edges[(int) waitingRule_EdgeNums.@b];
			Edge_blocked edge_bn = Edge_blocked.CreateEdge(graph, node_r2, node_p2);
			graph.Remove(edge_b);
			graph.RemoveEdges(node_r);
			graph.Remove(node_r);
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] { "bn" };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}


    public class Action_aux_attachResource : LGSPAction
    {
        public Action_aux_attachResource() {
            rulePattern = Rule_aux_attachResource.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 1, 0, 0+0);
        }

        public override string Name { get { return "aux_attachResource"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_aux_attachResource instance = new Action_aux_attachResource();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup aux_attachResource_node_p 
            int type_id_candidate_aux_attachResource_node_p = 0;
            for(LGSPNode head_candidate_aux_attachResource_node_p = graph.nodesByTypeHeads[type_id_candidate_aux_attachResource_node_p], candidate_aux_attachResource_node_p = head_candidate_aux_attachResource_node_p.typeNext; candidate_aux_attachResource_node_p != head_candidate_aux_attachResource_node_p; candidate_aux_attachResource_node_p = candidate_aux_attachResource_node_p.typeNext)
            {
                // NegativePattern 
                {
                    ++negLevel;
                    // Extend incoming aux_attachResource_neg_0_edge__edge0 from aux_attachResource_node_p 
                    LGSPEdge head_candidate_aux_attachResource_neg_0_edge__edge0 = candidate_aux_attachResource_node_p.inhead;
                    if(head_candidate_aux_attachResource_neg_0_edge__edge0 != null)
                    {
                        LGSPEdge candidate_aux_attachResource_neg_0_edge__edge0 = head_candidate_aux_attachResource_neg_0_edge__edge0;
                        do
                        {
                            if(!EdgeType_held_by.isMyType[candidate_aux_attachResource_neg_0_edge__edge0.type.TypeID]) {
                                continue;
                            }
                            // Implicit source aux_attachResource_neg_0_node_r from aux_attachResource_neg_0_edge__edge0 
                            LGSPNode candidate_aux_attachResource_neg_0_node_r = candidate_aux_attachResource_neg_0_edge__edge0.source;
                            if(!NodeType_Resource.isMyType[candidate_aux_attachResource_neg_0_node_r.type.TypeID]) {
                                continue;
                            }
                            // negative pattern found
                            --negLevel;
                            goto label0;
                        }
                        while( (candidate_aux_attachResource_neg_0_edge__edge0 = candidate_aux_attachResource_neg_0_edge__edge0.inNext) != head_candidate_aux_attachResource_neg_0_edge__edge0 );
                    }
                    --negLevel;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_aux_attachResource.aux_attachResource_NodeNums.@p] = candidate_aux_attachResource_node_p;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_aux_attachResource_node_p);
                    return matches;
                }
label0: ;
            }
            return matches;
        }
    }

    public class Action_blockedRule : LGSPAction
    {
        public Action_blockedRule() {
            rulePattern = Rule_blockedRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 3, 2, 0+0);
        }

        public override string Name { get { return "blockedRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_blockedRule instance = new Action_blockedRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup blockedRule_edge_hb 
            int type_id_candidate_blockedRule_edge_hb = 2;
            for(LGSPEdge head_candidate_blockedRule_edge_hb = graph.edgesByTypeHeads[type_id_candidate_blockedRule_edge_hb], candidate_blockedRule_edge_hb = head_candidate_blockedRule_edge_hb.typeNext; candidate_blockedRule_edge_hb != head_candidate_blockedRule_edge_hb; candidate_blockedRule_edge_hb = candidate_blockedRule_edge_hb.typeNext)
            {
                // Implicit source blockedRule_node_r from blockedRule_edge_hb 
                LGSPNode candidate_blockedRule_node_r = candidate_blockedRule_edge_hb.source;
                if(!NodeType_Resource.isMyType[candidate_blockedRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target blockedRule_node_p2 from blockedRule_edge_hb 
                LGSPNode candidate_blockedRule_node_p2 = candidate_blockedRule_edge_hb.target;
                if(!NodeType_Process.isMyType[candidate_blockedRule_node_p2.type.TypeID]) {
                    continue;
                }
                uint prev__candidate_blockedRule_node_p2;
                prev__candidate_blockedRule_node_p2 = candidate_blockedRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_blockedRule_node_p2.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Extend incoming blockedRule_edge_req from blockedRule_node_r 
                LGSPEdge head_candidate_blockedRule_edge_req = candidate_blockedRule_node_r.inhead;
                if(head_candidate_blockedRule_edge_req != null)
                {
                    LGSPEdge candidate_blockedRule_edge_req = head_candidate_blockedRule_edge_req;
                    do
                    {
                        if(!EdgeType_request.isMyType[candidate_blockedRule_edge_req.type.TypeID]) {
                            continue;
                        }
                        // Implicit source blockedRule_node_p1 from blockedRule_edge_req 
                        LGSPNode candidate_blockedRule_node_p1 = candidate_blockedRule_edge_req.source;
                        if(!NodeType_Process.isMyType[candidate_blockedRule_node_p1.type.TypeID]) {
                            continue;
                        }
                        if((candidate_blockedRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                            && candidate_blockedRule_node_p1==candidate_blockedRule_node_p2
                            )
                        {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_blockedRule.blockedRule_NodeNums.@p1] = candidate_blockedRule_node_p1;
                        match.Nodes[(int)Rule_blockedRule.blockedRule_NodeNums.@r] = candidate_blockedRule_node_r;
                        match.Nodes[(int)Rule_blockedRule.blockedRule_NodeNums.@p2] = candidate_blockedRule_node_p2;
                        match.Edges[(int)Rule_blockedRule.blockedRule_EdgeNums.@req] = candidate_blockedRule_edge_req;
                        match.Edges[(int)Rule_blockedRule.blockedRule_EdgeNums.@hb] = candidate_blockedRule_edge_hb;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_blockedRule_node_r.MoveInHeadAfter(candidate_blockedRule_edge_req);
                            graph.MoveHeadAfter(candidate_blockedRule_edge_hb);
                            candidate_blockedRule_node_p2.flags = candidate_blockedRule_node_p2.flags & ~prev__candidate_blockedRule_node_p2 | prev__candidate_blockedRule_node_p2;
                            return matches;
                        }
                    }
                    while( (candidate_blockedRule_edge_req = candidate_blockedRule_edge_req.inNext) != head_candidate_blockedRule_edge_req );
                }
                candidate_blockedRule_node_p2.flags = candidate_blockedRule_node_p2.flags & ~prev__candidate_blockedRule_node_p2 | prev__candidate_blockedRule_node_p2;
            }
            return matches;
        }
    }

    public class Action_giveRule : LGSPAction
    {
        public Action_giveRule() {
            rulePattern = Rule_giveRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 3, 2, 0+0);
        }

        public override string Name { get { return "giveRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_giveRule instance = new Action_giveRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup giveRule_edge_n 
            int type_id_candidate_giveRule_edge_n = 0;
            for(LGSPEdge head_candidate_giveRule_edge_n = graph.edgesByTypeHeads[type_id_candidate_giveRule_edge_n], candidate_giveRule_edge_n = head_candidate_giveRule_edge_n.typeNext; candidate_giveRule_edge_n != head_candidate_giveRule_edge_n; candidate_giveRule_edge_n = candidate_giveRule_edge_n.typeNext)
            {
                // Implicit source giveRule_node_p1 from giveRule_edge_n 
                LGSPNode candidate_giveRule_node_p1 = candidate_giveRule_edge_n.source;
                if(!NodeType_Process.isMyType[candidate_giveRule_node_p1.type.TypeID]) {
                    continue;
                }
                uint prev__candidate_giveRule_node_p1;
                prev__candidate_giveRule_node_p1 = candidate_giveRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_giveRule_node_p1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target giveRule_node_p2 from giveRule_edge_n 
                LGSPNode candidate_giveRule_node_p2 = candidate_giveRule_edge_n.target;
                if(!NodeType_Process.isMyType[candidate_giveRule_node_p2.type.TypeID]) {
                    candidate_giveRule_node_p1.flags = candidate_giveRule_node_p1.flags & ~prev__candidate_giveRule_node_p1 | prev__candidate_giveRule_node_p1;
                    continue;
                }
                if((candidate_giveRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                    && candidate_giveRule_node_p2==candidate_giveRule_node_p1
                    )
                {
                    candidate_giveRule_node_p1.flags = candidate_giveRule_node_p1.flags & ~prev__candidate_giveRule_node_p1 | prev__candidate_giveRule_node_p1;
                    continue;
                }
                // Extend incoming giveRule_edge_rel from giveRule_node_p1 
                LGSPEdge head_candidate_giveRule_edge_rel = candidate_giveRule_node_p1.inhead;
                if(head_candidate_giveRule_edge_rel != null)
                {
                    LGSPEdge candidate_giveRule_edge_rel = head_candidate_giveRule_edge_rel;
                    do
                    {
                        if(!EdgeType_release.isMyType[candidate_giveRule_edge_rel.type.TypeID]) {
                            continue;
                        }
                        // Implicit source giveRule_node_r from giveRule_edge_rel 
                        LGSPNode candidate_giveRule_node_r = candidate_giveRule_edge_rel.source;
                        if(!NodeType_Resource.isMyType[candidate_giveRule_node_r.type.TypeID]) {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_giveRule.giveRule_NodeNums.@r] = candidate_giveRule_node_r;
                        match.Nodes[(int)Rule_giveRule.giveRule_NodeNums.@p1] = candidate_giveRule_node_p1;
                        match.Nodes[(int)Rule_giveRule.giveRule_NodeNums.@p2] = candidate_giveRule_node_p2;
                        match.Edges[(int)Rule_giveRule.giveRule_EdgeNums.@rel] = candidate_giveRule_edge_rel;
                        match.Edges[(int)Rule_giveRule.giveRule_EdgeNums.@n] = candidate_giveRule_edge_n;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_giveRule_node_p1.MoveInHeadAfter(candidate_giveRule_edge_rel);
                            graph.MoveHeadAfter(candidate_giveRule_edge_n);
                            candidate_giveRule_node_p1.flags = candidate_giveRule_node_p1.flags & ~prev__candidate_giveRule_node_p1 | prev__candidate_giveRule_node_p1;
                            return matches;
                        }
                    }
                    while( (candidate_giveRule_edge_rel = candidate_giveRule_edge_rel.inNext) != head_candidate_giveRule_edge_rel );
                }
                candidate_giveRule_node_p1.flags = candidate_giveRule_node_p1.flags & ~prev__candidate_giveRule_node_p1 | prev__candidate_giveRule_node_p1;
            }
            return matches;
        }
    }

    public class Action_ignoreRule : LGSPAction
    {
        public Action_ignoreRule() {
            rulePattern = Rule_ignoreRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 1, 0+0);
        }

        public override string Name { get { return "ignoreRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_ignoreRule instance = new Action_ignoreRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup ignoreRule_edge_b 
            int type_id_candidate_ignoreRule_edge_b = 1;
            for(LGSPEdge head_candidate_ignoreRule_edge_b = graph.edgesByTypeHeads[type_id_candidate_ignoreRule_edge_b], candidate_ignoreRule_edge_b = head_candidate_ignoreRule_edge_b.typeNext; candidate_ignoreRule_edge_b != head_candidate_ignoreRule_edge_b; candidate_ignoreRule_edge_b = candidate_ignoreRule_edge_b.typeNext)
            {
                // Implicit source ignoreRule_node_r from ignoreRule_edge_b 
                LGSPNode candidate_ignoreRule_node_r = candidate_ignoreRule_edge_b.source;
                if(!NodeType_Resource.isMyType[candidate_ignoreRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target ignoreRule_node_p from ignoreRule_edge_b 
                LGSPNode candidate_ignoreRule_node_p = candidate_ignoreRule_edge_b.target;
                if(!NodeType_Process.isMyType[candidate_ignoreRule_node_p.type.TypeID]) {
                    continue;
                }
                // NegativePattern 
                {
                    ++negLevel;
                    // Extend incoming ignoreRule_neg_0_edge_hb from ignoreRule_node_p 
                    LGSPEdge head_candidate_ignoreRule_neg_0_edge_hb = candidate_ignoreRule_node_p.inhead;
                    if(head_candidate_ignoreRule_neg_0_edge_hb != null)
                    {
                        LGSPEdge candidate_ignoreRule_neg_0_edge_hb = head_candidate_ignoreRule_neg_0_edge_hb;
                        do
                        {
                            if(!EdgeType_held_by.isMyType[candidate_ignoreRule_neg_0_edge_hb.type.TypeID]) {
                                continue;
                            }
                            // Implicit source ignoreRule_neg_0_node_m from ignoreRule_neg_0_edge_hb 
                            LGSPNode candidate_ignoreRule_neg_0_node_m = candidate_ignoreRule_neg_0_edge_hb.source;
                            if(!NodeType_Resource.isMyType[candidate_ignoreRule_neg_0_node_m.type.TypeID]) {
                                continue;
                            }
                            // negative pattern found
                            --negLevel;
                            goto label1;
                        }
                        while( (candidate_ignoreRule_neg_0_edge_hb = candidate_ignoreRule_neg_0_edge_hb.inNext) != head_candidate_ignoreRule_neg_0_edge_hb );
                    }
                    --negLevel;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_ignoreRule.ignoreRule_NodeNums.@r] = candidate_ignoreRule_node_r;
                match.Nodes[(int)Rule_ignoreRule.ignoreRule_NodeNums.@p] = candidate_ignoreRule_node_p;
                match.Edges[(int)Rule_ignoreRule.ignoreRule_EdgeNums.@b] = candidate_ignoreRule_edge_b;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_ignoreRule_edge_b);
                    return matches;
                }
label1: ;
            }
            return matches;
        }
    }

    public class Action_killRule : LGSPAction
    {
        public Action_killRule() {
            rulePattern = Rule_killRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 3, 2, 0+0);
        }

        public override string Name { get { return "killRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_killRule instance = new Action_killRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup killRule_edge_n2 
            int type_id_candidate_killRule_edge_n2 = 0;
            for(LGSPEdge head_candidate_killRule_edge_n2 = graph.edgesByTypeHeads[type_id_candidate_killRule_edge_n2], candidate_killRule_edge_n2 = head_candidate_killRule_edge_n2.typeNext; candidate_killRule_edge_n2 != head_candidate_killRule_edge_n2; candidate_killRule_edge_n2 = candidate_killRule_edge_n2.typeNext)
            {
                uint prev__candidate_killRule_edge_n2;
                prev__candidate_killRule_edge_n2 = candidate_killRule_edge_n2.flags & LGSPEdge.IS_MATCHED<<negLevel;
                candidate_killRule_edge_n2.flags |= LGSPEdge.IS_MATCHED<<negLevel;
                // Implicit source killRule_node_p from killRule_edge_n2 
                LGSPNode candidate_killRule_node_p = candidate_killRule_edge_n2.source;
                if(!NodeType_Process.isMyType[candidate_killRule_node_p.type.TypeID]) {
                    candidate_killRule_edge_n2.flags = candidate_killRule_edge_n2.flags & ~prev__candidate_killRule_edge_n2 | prev__candidate_killRule_edge_n2;
                    continue;
                }
                uint prev__candidate_killRule_node_p;
                prev__candidate_killRule_node_p = candidate_killRule_node_p.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_killRule_node_p.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target killRule_node_p2 from killRule_edge_n2 
                LGSPNode candidate_killRule_node_p2 = candidate_killRule_edge_n2.target;
                if(!NodeType_Process.isMyType[candidate_killRule_node_p2.type.TypeID]) {
                    candidate_killRule_node_p.flags = candidate_killRule_node_p.flags & ~prev__candidate_killRule_node_p | prev__candidate_killRule_node_p;
                    candidate_killRule_edge_n2.flags = candidate_killRule_edge_n2.flags & ~prev__candidate_killRule_edge_n2 | prev__candidate_killRule_edge_n2;
                    continue;
                }
                if((candidate_killRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                    && candidate_killRule_node_p2==candidate_killRule_node_p
                    )
                {
                    candidate_killRule_node_p.flags = candidate_killRule_node_p.flags & ~prev__candidate_killRule_node_p | prev__candidate_killRule_node_p;
                    candidate_killRule_edge_n2.flags = candidate_killRule_edge_n2.flags & ~prev__candidate_killRule_edge_n2 | prev__candidate_killRule_edge_n2;
                    continue;
                }
                uint prev__candidate_killRule_node_p2;
                prev__candidate_killRule_node_p2 = candidate_killRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_killRule_node_p2.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Extend incoming killRule_edge_n1 from killRule_node_p 
                LGSPEdge head_candidate_killRule_edge_n1 = candidate_killRule_node_p.inhead;
                if(head_candidate_killRule_edge_n1 != null)
                {
                    LGSPEdge candidate_killRule_edge_n1 = head_candidate_killRule_edge_n1;
                    do
                    {
                        if(!EdgeType_next.isMyType[candidate_killRule_edge_n1.type.TypeID]) {
                            continue;
                        }
                        if((candidate_killRule_edge_n1.flags & LGSPEdge.IS_MATCHED<<negLevel) == LGSPEdge.IS_MATCHED<<negLevel
                            && candidate_killRule_edge_n1==candidate_killRule_edge_n2
                            )
                        {
                            continue;
                        }
                        // Implicit source killRule_node_p1 from killRule_edge_n1 
                        LGSPNode candidate_killRule_node_p1 = candidate_killRule_edge_n1.source;
                        if(!NodeType_Process.isMyType[candidate_killRule_node_p1.type.TypeID]) {
                            continue;
                        }
                        if((candidate_killRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                            && (candidate_killRule_node_p1==candidate_killRule_node_p
                                || candidate_killRule_node_p1==candidate_killRule_node_p2
                                )
                            )
                        {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_killRule.killRule_NodeNums.@p1] = candidate_killRule_node_p1;
                        match.Nodes[(int)Rule_killRule.killRule_NodeNums.@p] = candidate_killRule_node_p;
                        match.Nodes[(int)Rule_killRule.killRule_NodeNums.@p2] = candidate_killRule_node_p2;
                        match.Edges[(int)Rule_killRule.killRule_EdgeNums.@n1] = candidate_killRule_edge_n1;
                        match.Edges[(int)Rule_killRule.killRule_EdgeNums.@n2] = candidate_killRule_edge_n2;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_killRule_node_p.MoveInHeadAfter(candidate_killRule_edge_n1);
                            graph.MoveHeadAfter(candidate_killRule_edge_n2);
                            candidate_killRule_node_p2.flags = candidate_killRule_node_p2.flags & ~prev__candidate_killRule_node_p2 | prev__candidate_killRule_node_p2;
                            candidate_killRule_node_p.flags = candidate_killRule_node_p.flags & ~prev__candidate_killRule_node_p | prev__candidate_killRule_node_p;
                            candidate_killRule_edge_n2.flags = candidate_killRule_edge_n2.flags & ~prev__candidate_killRule_edge_n2 | prev__candidate_killRule_edge_n2;
                            return matches;
                        }
                    }
                    while( (candidate_killRule_edge_n1 = candidate_killRule_edge_n1.inNext) != head_candidate_killRule_edge_n1 );
                }
                candidate_killRule_node_p2.flags = candidate_killRule_node_p2.flags & ~prev__candidate_killRule_node_p2 | prev__candidate_killRule_node_p2;
                candidate_killRule_node_p.flags = candidate_killRule_node_p.flags & ~prev__candidate_killRule_node_p | prev__candidate_killRule_node_p;
                candidate_killRule_edge_n2.flags = candidate_killRule_edge_n2.flags & ~prev__candidate_killRule_edge_n2 | prev__candidate_killRule_edge_n2;
            }
            return matches;
        }
    }

    public class Action_mountRule : LGSPAction
    {
        public Action_mountRule() {
            rulePattern = Rule_mountRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 1, 0, 0+0);
        }

        public override string Name { get { return "mountRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_mountRule instance = new Action_mountRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup mountRule_node_p 
            int type_id_candidate_mountRule_node_p = 0;
            for(LGSPNode head_candidate_mountRule_node_p = graph.nodesByTypeHeads[type_id_candidate_mountRule_node_p], candidate_mountRule_node_p = head_candidate_mountRule_node_p.typeNext; candidate_mountRule_node_p != head_candidate_mountRule_node_p; candidate_mountRule_node_p = candidate_mountRule_node_p.typeNext)
            {
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_mountRule.mountRule_NodeNums.@p] = candidate_mountRule_node_p;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_mountRule_node_p);
                    return matches;
                }
            }
            return matches;
        }
    }

    public class Action_newRule : LGSPAction
    {
        public Action_newRule() {
            rulePattern = Rule_newRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 1, 0+0);
        }

        public override string Name { get { return "newRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_newRule instance = new Action_newRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup newRule_edge_n 
            int type_id_candidate_newRule_edge_n = 0;
            for(LGSPEdge head_candidate_newRule_edge_n = graph.edgesByTypeHeads[type_id_candidate_newRule_edge_n], candidate_newRule_edge_n = head_candidate_newRule_edge_n.typeNext; candidate_newRule_edge_n != head_candidate_newRule_edge_n; candidate_newRule_edge_n = candidate_newRule_edge_n.typeNext)
            {
                // Implicit source newRule_node_p1 from newRule_edge_n 
                LGSPNode candidate_newRule_node_p1 = candidate_newRule_edge_n.source;
                if(!NodeType_Process.isMyType[candidate_newRule_node_p1.type.TypeID]) {
                    continue;
                }
                uint prev__candidate_newRule_node_p1;
                prev__candidate_newRule_node_p1 = candidate_newRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_newRule_node_p1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target newRule_node_p2 from newRule_edge_n 
                LGSPNode candidate_newRule_node_p2 = candidate_newRule_edge_n.target;
                if(!NodeType_Process.isMyType[candidate_newRule_node_p2.type.TypeID]) {
                    candidate_newRule_node_p1.flags = candidate_newRule_node_p1.flags & ~prev__candidate_newRule_node_p1 | prev__candidate_newRule_node_p1;
                    continue;
                }
                if((candidate_newRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                    && candidate_newRule_node_p2==candidate_newRule_node_p1
                    )
                {
                    candidate_newRule_node_p1.flags = candidate_newRule_node_p1.flags & ~prev__candidate_newRule_node_p1 | prev__candidate_newRule_node_p1;
                    continue;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_newRule.newRule_NodeNums.@p1] = candidate_newRule_node_p1;
                match.Nodes[(int)Rule_newRule.newRule_NodeNums.@p2] = candidate_newRule_node_p2;
                match.Edges[(int)Rule_newRule.newRule_EdgeNums.@n] = candidate_newRule_edge_n;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_newRule_edge_n);
                    candidate_newRule_node_p1.flags = candidate_newRule_node_p1.flags & ~prev__candidate_newRule_node_p1 | prev__candidate_newRule_node_p1;
                    return matches;
                }
                candidate_newRule_node_p1.flags = candidate_newRule_node_p1.flags & ~prev__candidate_newRule_node_p1 | prev__candidate_newRule_node_p1;
            }
            return matches;
        }
    }

    public class Action_passRule : LGSPAction
    {
        public Action_passRule() {
            rulePattern = Rule_passRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 3, 2, 0+0);
        }

        public override string Name { get { return "passRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_passRule instance = new Action_passRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup passRule_edge_n 
            int type_id_candidate_passRule_edge_n = 0;
            for(LGSPEdge head_candidate_passRule_edge_n = graph.edgesByTypeHeads[type_id_candidate_passRule_edge_n], candidate_passRule_edge_n = head_candidate_passRule_edge_n.typeNext; candidate_passRule_edge_n != head_candidate_passRule_edge_n; candidate_passRule_edge_n = candidate_passRule_edge_n.typeNext)
            {
                // Implicit source passRule_node_p1 from passRule_edge_n 
                LGSPNode candidate_passRule_node_p1 = candidate_passRule_edge_n.source;
                if(!NodeType_Process.isMyType[candidate_passRule_node_p1.type.TypeID]) {
                    continue;
                }
                uint prev__candidate_passRule_node_p1;
                prev__candidate_passRule_node_p1 = candidate_passRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_passRule_node_p1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target passRule_node_p2 from passRule_edge_n 
                LGSPNode candidate_passRule_node_p2 = candidate_passRule_edge_n.target;
                if(!NodeType_Process.isMyType[candidate_passRule_node_p2.type.TypeID]) {
                    candidate_passRule_node_p1.flags = candidate_passRule_node_p1.flags & ~prev__candidate_passRule_node_p1 | prev__candidate_passRule_node_p1;
                    continue;
                }
                if((candidate_passRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                    && candidate_passRule_node_p2==candidate_passRule_node_p1
                    )
                {
                    candidate_passRule_node_p1.flags = candidate_passRule_node_p1.flags & ~prev__candidate_passRule_node_p1 | prev__candidate_passRule_node_p1;
                    continue;
                }
                // Extend incoming passRule_edge__edge0 from passRule_node_p1 
                LGSPEdge head_candidate_passRule_edge__edge0 = candidate_passRule_node_p1.inhead;
                if(head_candidate_passRule_edge__edge0 != null)
                {
                    LGSPEdge candidate_passRule_edge__edge0 = head_candidate_passRule_edge__edge0;
                    do
                    {
                        if(!EdgeType_token.isMyType[candidate_passRule_edge__edge0.type.TypeID]) {
                            continue;
                        }
                        // Implicit source passRule_node_r from passRule_edge__edge0 
                        LGSPNode candidate_passRule_node_r = candidate_passRule_edge__edge0.source;
                        if(!NodeType_Resource.isMyType[candidate_passRule_node_r.type.TypeID]) {
                            continue;
                        }
                        // NegativePattern 
                        {
                            ++negLevel;
                            // Extend outgoing passRule_neg_0_edge_req from passRule_node_p1 
                            LGSPEdge head_candidate_passRule_neg_0_edge_req = candidate_passRule_node_p1.outhead;
                            if(head_candidate_passRule_neg_0_edge_req != null)
                            {
                                LGSPEdge candidate_passRule_neg_0_edge_req = head_candidate_passRule_neg_0_edge_req;
                                do
                                {
                                    if(!EdgeType_request.isMyType[candidate_passRule_neg_0_edge_req.type.TypeID]) {
                                        continue;
                                    }
                                    if(candidate_passRule_neg_0_edge_req.target != candidate_passRule_node_r) {
                                        continue;
                                    }
                                    // negative pattern found
                                    --negLevel;
                                    goto label2;
                                }
                                while( (candidate_passRule_neg_0_edge_req = candidate_passRule_neg_0_edge_req.outNext) != head_candidate_passRule_neg_0_edge_req );
                            }
                            --negLevel;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_passRule.passRule_NodeNums.@r] = candidate_passRule_node_r;
                        match.Nodes[(int)Rule_passRule.passRule_NodeNums.@p1] = candidate_passRule_node_p1;
                        match.Nodes[(int)Rule_passRule.passRule_NodeNums.@p2] = candidate_passRule_node_p2;
                        match.Edges[(int)Rule_passRule.passRule_EdgeNums.@_edge0] = candidate_passRule_edge__edge0;
                        match.Edges[(int)Rule_passRule.passRule_EdgeNums.@n] = candidate_passRule_edge_n;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_passRule_node_p1.MoveInHeadAfter(candidate_passRule_edge__edge0);
                            graph.MoveHeadAfter(candidate_passRule_edge_n);
                            candidate_passRule_node_p1.flags = candidate_passRule_node_p1.flags & ~prev__candidate_passRule_node_p1 | prev__candidate_passRule_node_p1;
                            return matches;
                        }
label2: ;
                    }
                    while( (candidate_passRule_edge__edge0 = candidate_passRule_edge__edge0.inNext) != head_candidate_passRule_edge__edge0 );
                }
                candidate_passRule_node_p1.flags = candidate_passRule_node_p1.flags & ~prev__candidate_passRule_node_p1 | prev__candidate_passRule_node_p1;
            }
            return matches;
        }
    }

    public class Action_releaseRule : LGSPAction
    {
        public Action_releaseRule() {
            rulePattern = Rule_releaseRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 1, 0+0);
        }

        public override string Name { get { return "releaseRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_releaseRule instance = new Action_releaseRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup releaseRule_edge_hb 
            int type_id_candidate_releaseRule_edge_hb = 2;
            for(LGSPEdge head_candidate_releaseRule_edge_hb = graph.edgesByTypeHeads[type_id_candidate_releaseRule_edge_hb], candidate_releaseRule_edge_hb = head_candidate_releaseRule_edge_hb.typeNext; candidate_releaseRule_edge_hb != head_candidate_releaseRule_edge_hb; candidate_releaseRule_edge_hb = candidate_releaseRule_edge_hb.typeNext)
            {
                // Implicit source releaseRule_node_r from releaseRule_edge_hb 
                LGSPNode candidate_releaseRule_node_r = candidate_releaseRule_edge_hb.source;
                if(!NodeType_Resource.isMyType[candidate_releaseRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target releaseRule_node_p from releaseRule_edge_hb 
                LGSPNode candidate_releaseRule_node_p = candidate_releaseRule_edge_hb.target;
                if(!NodeType_Process.isMyType[candidate_releaseRule_node_p.type.TypeID]) {
                    continue;
                }
                // NegativePattern 
                {
                    ++negLevel;
                    // Extend outgoing releaseRule_neg_0_edge_req from releaseRule_node_p 
                    LGSPEdge head_candidate_releaseRule_neg_0_edge_req = candidate_releaseRule_node_p.outhead;
                    if(head_candidate_releaseRule_neg_0_edge_req != null)
                    {
                        LGSPEdge candidate_releaseRule_neg_0_edge_req = head_candidate_releaseRule_neg_0_edge_req;
                        do
                        {
                            if(!EdgeType_request.isMyType[candidate_releaseRule_neg_0_edge_req.type.TypeID]) {
                                continue;
                            }
                            // Implicit target releaseRule_neg_0_node_m from releaseRule_neg_0_edge_req 
                            LGSPNode candidate_releaseRule_neg_0_node_m = candidate_releaseRule_neg_0_edge_req.target;
                            if(!NodeType_Resource.isMyType[candidate_releaseRule_neg_0_node_m.type.TypeID]) {
                                continue;
                            }
                            // negative pattern found
                            --negLevel;
                            goto label3;
                        }
                        while( (candidate_releaseRule_neg_0_edge_req = candidate_releaseRule_neg_0_edge_req.outNext) != head_candidate_releaseRule_neg_0_edge_req );
                    }
                    --negLevel;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_releaseRule.releaseRule_NodeNums.@r] = candidate_releaseRule_node_r;
                match.Nodes[(int)Rule_releaseRule.releaseRule_NodeNums.@p] = candidate_releaseRule_node_p;
                match.Edges[(int)Rule_releaseRule.releaseRule_EdgeNums.@hb] = candidate_releaseRule_edge_hb;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_releaseRule_edge_hb);
                    return matches;
                }
label3: ;
            }
            return matches;
        }
    }

    public class Action_releaseStarRule : LGSPAction
    {
        public Action_releaseStarRule() {
            rulePattern = Rule_releaseStarRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 4, 3, 0+0);
        }

        public override string Name { get { return "releaseStarRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_releaseStarRule instance = new Action_releaseStarRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup releaseStarRule_edge_h1 
            int type_id_candidate_releaseStarRule_edge_h1 = 2;
            for(LGSPEdge head_candidate_releaseStarRule_edge_h1 = graph.edgesByTypeHeads[type_id_candidate_releaseStarRule_edge_h1], candidate_releaseStarRule_edge_h1 = head_candidate_releaseStarRule_edge_h1.typeNext; candidate_releaseStarRule_edge_h1 != head_candidate_releaseStarRule_edge_h1; candidate_releaseStarRule_edge_h1 = candidate_releaseStarRule_edge_h1.typeNext)
            {
                uint prev__candidate_releaseStarRule_edge_h1;
                prev__candidate_releaseStarRule_edge_h1 = candidate_releaseStarRule_edge_h1.flags & LGSPEdge.IS_MATCHED<<negLevel;
                candidate_releaseStarRule_edge_h1.flags |= LGSPEdge.IS_MATCHED<<negLevel;
                // Implicit source releaseStarRule_node_r1 from releaseStarRule_edge_h1 
                LGSPNode candidate_releaseStarRule_node_r1 = candidate_releaseStarRule_edge_h1.source;
                if(!NodeType_Resource.isMyType[candidate_releaseStarRule_node_r1.type.TypeID]) {
                    candidate_releaseStarRule_edge_h1.flags = candidate_releaseStarRule_edge_h1.flags & ~prev__candidate_releaseStarRule_edge_h1 | prev__candidate_releaseStarRule_edge_h1;
                    continue;
                }
                uint prev__candidate_releaseStarRule_node_r1;
                prev__candidate_releaseStarRule_node_r1 = candidate_releaseStarRule_node_r1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_releaseStarRule_node_r1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target releaseStarRule_node_p2 from releaseStarRule_edge_h1 
                LGSPNode candidate_releaseStarRule_node_p2 = candidate_releaseStarRule_edge_h1.target;
                if(!NodeType_Process.isMyType[candidate_releaseStarRule_node_p2.type.TypeID]) {
                    candidate_releaseStarRule_node_r1.flags = candidate_releaseStarRule_node_r1.flags & ~prev__candidate_releaseStarRule_node_r1 | prev__candidate_releaseStarRule_node_r1;
                    candidate_releaseStarRule_edge_h1.flags = candidate_releaseStarRule_edge_h1.flags & ~prev__candidate_releaseStarRule_edge_h1 | prev__candidate_releaseStarRule_edge_h1;
                    continue;
                }
                uint prev__candidate_releaseStarRule_node_p2;
                prev__candidate_releaseStarRule_node_p2 = candidate_releaseStarRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_releaseStarRule_node_p2.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Extend incoming releaseStarRule_edge_rq from releaseStarRule_node_r1 
                LGSPEdge head_candidate_releaseStarRule_edge_rq = candidate_releaseStarRule_node_r1.inhead;
                if(head_candidate_releaseStarRule_edge_rq != null)
                {
                    LGSPEdge candidate_releaseStarRule_edge_rq = head_candidate_releaseStarRule_edge_rq;
                    do
                    {
                        if(!EdgeType_request.isMyType[candidate_releaseStarRule_edge_rq.type.TypeID]) {
                            continue;
                        }
                        // Implicit source releaseStarRule_node_p1 from releaseStarRule_edge_rq 
                        LGSPNode candidate_releaseStarRule_node_p1 = candidate_releaseStarRule_edge_rq.source;
                        if(!NodeType_Process.isMyType[candidate_releaseStarRule_node_p1.type.TypeID]) {
                            continue;
                        }
                        if((candidate_releaseStarRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                            && candidate_releaseStarRule_node_p1==candidate_releaseStarRule_node_p2
                            )
                        {
                            continue;
                        }
                        // Extend incoming releaseStarRule_edge_h2 from releaseStarRule_node_p2 
                        LGSPEdge head_candidate_releaseStarRule_edge_h2 = candidate_releaseStarRule_node_p2.inhead;
                        if(head_candidate_releaseStarRule_edge_h2 != null)
                        {
                            LGSPEdge candidate_releaseStarRule_edge_h2 = head_candidate_releaseStarRule_edge_h2;
                            do
                            {
                                if(!EdgeType_held_by.isMyType[candidate_releaseStarRule_edge_h2.type.TypeID]) {
                                    continue;
                                }
                                if((candidate_releaseStarRule_edge_h2.flags & LGSPEdge.IS_MATCHED<<negLevel) == LGSPEdge.IS_MATCHED<<negLevel
                                    && candidate_releaseStarRule_edge_h2==candidate_releaseStarRule_edge_h1
                                    )
                                {
                                    continue;
                                }
                                // Implicit source releaseStarRule_node_r2 from releaseStarRule_edge_h2 
                                LGSPNode candidate_releaseStarRule_node_r2 = candidate_releaseStarRule_edge_h2.source;
                                if(!NodeType_Resource.isMyType[candidate_releaseStarRule_node_r2.type.TypeID]) {
                                    continue;
                                }
                                if((candidate_releaseStarRule_node_r2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                                    && candidate_releaseStarRule_node_r2==candidate_releaseStarRule_node_r1
                                    )
                                {
                                    continue;
                                }
                                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                                match.patternGraph = rulePattern.patternGraph;
                                match.Nodes[(int)Rule_releaseStarRule.releaseStarRule_NodeNums.@p1] = candidate_releaseStarRule_node_p1;
                                match.Nodes[(int)Rule_releaseStarRule.releaseStarRule_NodeNums.@r1] = candidate_releaseStarRule_node_r1;
                                match.Nodes[(int)Rule_releaseStarRule.releaseStarRule_NodeNums.@p2] = candidate_releaseStarRule_node_p2;
                                match.Nodes[(int)Rule_releaseStarRule.releaseStarRule_NodeNums.@r2] = candidate_releaseStarRule_node_r2;
                                match.Edges[(int)Rule_releaseStarRule.releaseStarRule_EdgeNums.@rq] = candidate_releaseStarRule_edge_rq;
                                match.Edges[(int)Rule_releaseStarRule.releaseStarRule_EdgeNums.@h1] = candidate_releaseStarRule_edge_h1;
                                match.Edges[(int)Rule_releaseStarRule.releaseStarRule_EdgeNums.@h2] = candidate_releaseStarRule_edge_h2;
                                matches.matchesList.PositionWasFilledFixIt();
                                // if enough matches were found, we leave
                                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                                {
                                    candidate_releaseStarRule_node_p2.MoveInHeadAfter(candidate_releaseStarRule_edge_h2);
                                    candidate_releaseStarRule_node_r1.MoveInHeadAfter(candidate_releaseStarRule_edge_rq);
                                    graph.MoveHeadAfter(candidate_releaseStarRule_edge_h1);
                                    candidate_releaseStarRule_node_p2.flags = candidate_releaseStarRule_node_p2.flags & ~prev__candidate_releaseStarRule_node_p2 | prev__candidate_releaseStarRule_node_p2;
                                    candidate_releaseStarRule_node_r1.flags = candidate_releaseStarRule_node_r1.flags & ~prev__candidate_releaseStarRule_node_r1 | prev__candidate_releaseStarRule_node_r1;
                                    candidate_releaseStarRule_edge_h1.flags = candidate_releaseStarRule_edge_h1.flags & ~prev__candidate_releaseStarRule_edge_h1 | prev__candidate_releaseStarRule_edge_h1;
                                    return matches;
                                }
                            }
                            while( (candidate_releaseStarRule_edge_h2 = candidate_releaseStarRule_edge_h2.inNext) != head_candidate_releaseStarRule_edge_h2 );
                        }
                    }
                    while( (candidate_releaseStarRule_edge_rq = candidate_releaseStarRule_edge_rq.inNext) != head_candidate_releaseStarRule_edge_rq );
                }
                candidate_releaseStarRule_node_p2.flags = candidate_releaseStarRule_node_p2.flags & ~prev__candidate_releaseStarRule_node_p2 | prev__candidate_releaseStarRule_node_p2;
                candidate_releaseStarRule_node_r1.flags = candidate_releaseStarRule_node_r1.flags & ~prev__candidate_releaseStarRule_node_r1 | prev__candidate_releaseStarRule_node_r1;
                candidate_releaseStarRule_edge_h1.flags = candidate_releaseStarRule_edge_h1.flags & ~prev__candidate_releaseStarRule_edge_h1 | prev__candidate_releaseStarRule_edge_h1;
            }
            return matches;
        }
    }

    public class Action_requestRule : LGSPAction
    {
        public Action_requestRule() {
            rulePattern = Rule_requestRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 0, 0+0);
        }

        public override string Name { get { return "requestRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_requestRule instance = new Action_requestRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup requestRule_node_r 
            int type_id_candidate_requestRule_node_r = 1;
            for(LGSPNode head_candidate_requestRule_node_r = graph.nodesByTypeHeads[type_id_candidate_requestRule_node_r], candidate_requestRule_node_r = head_candidate_requestRule_node_r.typeNext; candidate_requestRule_node_r != head_candidate_requestRule_node_r; candidate_requestRule_node_r = candidate_requestRule_node_r.typeNext)
            {
                // Lookup requestRule_node_p 
                int type_id_candidate_requestRule_node_p = 0;
                for(LGSPNode head_candidate_requestRule_node_p = graph.nodesByTypeHeads[type_id_candidate_requestRule_node_p], candidate_requestRule_node_p = head_candidate_requestRule_node_p.typeNext; candidate_requestRule_node_p != head_candidate_requestRule_node_p; candidate_requestRule_node_p = candidate_requestRule_node_p.typeNext)
                {
                    // NegativePattern 
                    {
                        ++negLevel;
                        // Extend outgoing requestRule_neg_0_edge_hb from requestRule_node_r 
                        LGSPEdge head_candidate_requestRule_neg_0_edge_hb = candidate_requestRule_node_r.outhead;
                        if(head_candidate_requestRule_neg_0_edge_hb != null)
                        {
                            LGSPEdge candidate_requestRule_neg_0_edge_hb = head_candidate_requestRule_neg_0_edge_hb;
                            do
                            {
                                if(!EdgeType_held_by.isMyType[candidate_requestRule_neg_0_edge_hb.type.TypeID]) {
                                    continue;
                                }
                                if(candidate_requestRule_neg_0_edge_hb.target != candidate_requestRule_node_p) {
                                    continue;
                                }
                                // negative pattern found
                                --negLevel;
                                goto label4;
                            }
                            while( (candidate_requestRule_neg_0_edge_hb = candidate_requestRule_neg_0_edge_hb.outNext) != head_candidate_requestRule_neg_0_edge_hb );
                        }
                        --negLevel;
                    }
                    // NegativePattern 
                    {
                        ++negLevel;
                        // Extend outgoing requestRule_neg_1_edge_req from requestRule_node_p 
                        LGSPEdge head_candidate_requestRule_neg_1_edge_req = candidate_requestRule_node_p.outhead;
                        if(head_candidate_requestRule_neg_1_edge_req != null)
                        {
                            LGSPEdge candidate_requestRule_neg_1_edge_req = head_candidate_requestRule_neg_1_edge_req;
                            do
                            {
                                if(!EdgeType_request.isMyType[candidate_requestRule_neg_1_edge_req.type.TypeID]) {
                                    continue;
                                }
                                // Implicit target requestRule_neg_1_node_m from requestRule_neg_1_edge_req 
                                LGSPNode candidate_requestRule_neg_1_node_m = candidate_requestRule_neg_1_edge_req.target;
                                if(!NodeType_Resource.isMyType[candidate_requestRule_neg_1_node_m.type.TypeID]) {
                                    continue;
                                }
                                // negative pattern found
                                --negLevel;
                                goto label5;
                            }
                            while( (candidate_requestRule_neg_1_edge_req = candidate_requestRule_neg_1_edge_req.outNext) != head_candidate_requestRule_neg_1_edge_req );
                        }
                        --negLevel;
                    }
                    LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                    match.patternGraph = rulePattern.patternGraph;
                    match.Nodes[(int)Rule_requestRule.requestRule_NodeNums.@p] = candidate_requestRule_node_p;
                    match.Nodes[(int)Rule_requestRule.requestRule_NodeNums.@r] = candidate_requestRule_node_r;
                    matches.matchesList.PositionWasFilledFixIt();
                    // if enough matches were found, we leave
                    if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                    {
                        graph.MoveHeadAfter(candidate_requestRule_node_p);
                        graph.MoveHeadAfter(candidate_requestRule_node_r);
                        return matches;
                    }
label4: ;
label5: ;
                }
            }
            return matches;
        }
    }

    public class Action_requestSimpleRule : LGSPAction
    {
        public Action_requestSimpleRule() {
            rulePattern = Rule_requestSimpleRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 1, 0+0);
        }

        public override string Name { get { return "requestSimpleRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_requestSimpleRule instance = new Action_requestSimpleRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup requestSimpleRule_edge_t 
            int type_id_candidate_requestSimpleRule_edge_t = 3;
            for(LGSPEdge head_candidate_requestSimpleRule_edge_t = graph.edgesByTypeHeads[type_id_candidate_requestSimpleRule_edge_t], candidate_requestSimpleRule_edge_t = head_candidate_requestSimpleRule_edge_t.typeNext; candidate_requestSimpleRule_edge_t != head_candidate_requestSimpleRule_edge_t; candidate_requestSimpleRule_edge_t = candidate_requestSimpleRule_edge_t.typeNext)
            {
                // Implicit source requestSimpleRule_node_r from requestSimpleRule_edge_t 
                LGSPNode candidate_requestSimpleRule_node_r = candidate_requestSimpleRule_edge_t.source;
                if(!NodeType_Resource.isMyType[candidate_requestSimpleRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target requestSimpleRule_node_p from requestSimpleRule_edge_t 
                LGSPNode candidate_requestSimpleRule_node_p = candidate_requestSimpleRule_edge_t.target;
                if(!NodeType_Process.isMyType[candidate_requestSimpleRule_node_p.type.TypeID]) {
                    continue;
                }
                // NegativePattern 
                {
                    ++negLevel;
                    // Extend outgoing requestSimpleRule_neg_0_edge_req from requestSimpleRule_node_p 
                    LGSPEdge head_candidate_requestSimpleRule_neg_0_edge_req = candidate_requestSimpleRule_node_p.outhead;
                    if(head_candidate_requestSimpleRule_neg_0_edge_req != null)
                    {
                        LGSPEdge candidate_requestSimpleRule_neg_0_edge_req = head_candidate_requestSimpleRule_neg_0_edge_req;
                        do
                        {
                            if(!EdgeType_request.isMyType[candidate_requestSimpleRule_neg_0_edge_req.type.TypeID]) {
                                continue;
                            }
                            if(candidate_requestSimpleRule_neg_0_edge_req.target != candidate_requestSimpleRule_node_r) {
                                continue;
                            }
                            // negative pattern found
                            --negLevel;
                            goto label6;
                        }
                        while( (candidate_requestSimpleRule_neg_0_edge_req = candidate_requestSimpleRule_neg_0_edge_req.outNext) != head_candidate_requestSimpleRule_neg_0_edge_req );
                    }
                    --negLevel;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_requestSimpleRule.requestSimpleRule_NodeNums.@r] = candidate_requestSimpleRule_node_r;
                match.Nodes[(int)Rule_requestSimpleRule.requestSimpleRule_NodeNums.@p] = candidate_requestSimpleRule_node_p;
                match.Edges[(int)Rule_requestSimpleRule.requestSimpleRule_EdgeNums.@t] = candidate_requestSimpleRule_edge_t;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_requestSimpleRule_edge_t);
                    return matches;
                }
label6: ;
            }
            return matches;
        }
    }

    public class Action_requestStarRule : LGSPAction
    {
        public Action_requestStarRule() {
            rulePattern = Rule_requestStarRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 4, 3, 0+0);
        }

        public override string Name { get { return "requestStarRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_requestStarRule instance = new Action_requestStarRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup requestStarRule_edge_h1 
            int type_id_candidate_requestStarRule_edge_h1 = 2;
            for(LGSPEdge head_candidate_requestStarRule_edge_h1 = graph.edgesByTypeHeads[type_id_candidate_requestStarRule_edge_h1], candidate_requestStarRule_edge_h1 = head_candidate_requestStarRule_edge_h1.typeNext; candidate_requestStarRule_edge_h1 != head_candidate_requestStarRule_edge_h1; candidate_requestStarRule_edge_h1 = candidate_requestStarRule_edge_h1.typeNext)
            {
                uint prev__candidate_requestStarRule_edge_h1;
                prev__candidate_requestStarRule_edge_h1 = candidate_requestStarRule_edge_h1.flags & LGSPEdge.IS_MATCHED<<negLevel;
                candidate_requestStarRule_edge_h1.flags |= LGSPEdge.IS_MATCHED<<negLevel;
                // Implicit source requestStarRule_node_r1 from requestStarRule_edge_h1 
                LGSPNode candidate_requestStarRule_node_r1 = candidate_requestStarRule_edge_h1.source;
                if(!NodeType_Resource.isMyType[candidate_requestStarRule_node_r1.type.TypeID]) {
                    candidate_requestStarRule_edge_h1.flags = candidate_requestStarRule_edge_h1.flags & ~prev__candidate_requestStarRule_edge_h1 | prev__candidate_requestStarRule_edge_h1;
                    continue;
                }
                uint prev__candidate_requestStarRule_node_r1;
                prev__candidate_requestStarRule_node_r1 = candidate_requestStarRule_node_r1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_requestStarRule_node_r1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Implicit target requestStarRule_node_p1 from requestStarRule_edge_h1 
                LGSPNode candidate_requestStarRule_node_p1 = candidate_requestStarRule_edge_h1.target;
                if(!NodeType_Process.isMyType[candidate_requestStarRule_node_p1.type.TypeID]) {
                    candidate_requestStarRule_node_r1.flags = candidate_requestStarRule_node_r1.flags & ~prev__candidate_requestStarRule_node_r1 | prev__candidate_requestStarRule_node_r1;
                    candidate_requestStarRule_edge_h1.flags = candidate_requestStarRule_edge_h1.flags & ~prev__candidate_requestStarRule_edge_h1 | prev__candidate_requestStarRule_edge_h1;
                    continue;
                }
                uint prev__candidate_requestStarRule_node_p1;
                prev__candidate_requestStarRule_node_p1 = candidate_requestStarRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_requestStarRule_node_p1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Extend incoming requestStarRule_edge_n from requestStarRule_node_p1 
                LGSPEdge head_candidate_requestStarRule_edge_n = candidate_requestStarRule_node_p1.inhead;
                if(head_candidate_requestStarRule_edge_n != null)
                {
                    LGSPEdge candidate_requestStarRule_edge_n = head_candidate_requestStarRule_edge_n;
                    do
                    {
                        if(!EdgeType_next.isMyType[candidate_requestStarRule_edge_n.type.TypeID]) {
                            continue;
                        }
                        // Implicit source requestStarRule_node_p2 from requestStarRule_edge_n 
                        LGSPNode candidate_requestStarRule_node_p2 = candidate_requestStarRule_edge_n.source;
                        if(!NodeType_Process.isMyType[candidate_requestStarRule_node_p2.type.TypeID]) {
                            continue;
                        }
                        if((candidate_requestStarRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                            && candidate_requestStarRule_node_p2==candidate_requestStarRule_node_p1
                            )
                        {
                            continue;
                        }
                        // Extend incoming requestStarRule_edge_h2 from requestStarRule_node_p2 
                        LGSPEdge head_candidate_requestStarRule_edge_h2 = candidate_requestStarRule_node_p2.inhead;
                        if(head_candidate_requestStarRule_edge_h2 != null)
                        {
                            LGSPEdge candidate_requestStarRule_edge_h2 = head_candidate_requestStarRule_edge_h2;
                            do
                            {
                                if(!EdgeType_held_by.isMyType[candidate_requestStarRule_edge_h2.type.TypeID]) {
                                    continue;
                                }
                                if((candidate_requestStarRule_edge_h2.flags & LGSPEdge.IS_MATCHED<<negLevel) == LGSPEdge.IS_MATCHED<<negLevel
                                    && candidate_requestStarRule_edge_h2==candidate_requestStarRule_edge_h1
                                    )
                                {
                                    continue;
                                }
                                // Implicit source requestStarRule_node_r2 from requestStarRule_edge_h2 
                                LGSPNode candidate_requestStarRule_node_r2 = candidate_requestStarRule_edge_h2.source;
                                if(!NodeType_Resource.isMyType[candidate_requestStarRule_node_r2.type.TypeID]) {
                                    continue;
                                }
                                if((candidate_requestStarRule_node_r2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                                    && candidate_requestStarRule_node_r2==candidate_requestStarRule_node_r1
                                    )
                                {
                                    continue;
                                }
                                // NegativePattern 
                                {
                                    ++negLevel;
                                    // Extend outgoing requestStarRule_neg_0_edge_req from requestStarRule_node_p1 
                                    LGSPEdge head_candidate_requestStarRule_neg_0_edge_req = candidate_requestStarRule_node_p1.outhead;
                                    if(head_candidate_requestStarRule_neg_0_edge_req != null)
                                    {
                                        LGSPEdge candidate_requestStarRule_neg_0_edge_req = head_candidate_requestStarRule_neg_0_edge_req;
                                        do
                                        {
                                            if(!EdgeType_request.isMyType[candidate_requestStarRule_neg_0_edge_req.type.TypeID]) {
                                                continue;
                                            }
                                            if(candidate_requestStarRule_neg_0_edge_req.target != candidate_requestStarRule_node_r2) {
                                                continue;
                                            }
                                            // negative pattern found
                                            --negLevel;
                                            goto label7;
                                        }
                                        while( (candidate_requestStarRule_neg_0_edge_req = candidate_requestStarRule_neg_0_edge_req.outNext) != head_candidate_requestStarRule_neg_0_edge_req );
                                    }
                                    --negLevel;
                                }
                                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                                match.patternGraph = rulePattern.patternGraph;
                                match.Nodes[(int)Rule_requestStarRule.requestStarRule_NodeNums.@r1] = candidate_requestStarRule_node_r1;
                                match.Nodes[(int)Rule_requestStarRule.requestStarRule_NodeNums.@p1] = candidate_requestStarRule_node_p1;
                                match.Nodes[(int)Rule_requestStarRule.requestStarRule_NodeNums.@p2] = candidate_requestStarRule_node_p2;
                                match.Nodes[(int)Rule_requestStarRule.requestStarRule_NodeNums.@r2] = candidate_requestStarRule_node_r2;
                                match.Edges[(int)Rule_requestStarRule.requestStarRule_EdgeNums.@h1] = candidate_requestStarRule_edge_h1;
                                match.Edges[(int)Rule_requestStarRule.requestStarRule_EdgeNums.@n] = candidate_requestStarRule_edge_n;
                                match.Edges[(int)Rule_requestStarRule.requestStarRule_EdgeNums.@h2] = candidate_requestStarRule_edge_h2;
                                matches.matchesList.PositionWasFilledFixIt();
                                // if enough matches were found, we leave
                                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                                {
                                    candidate_requestStarRule_node_p2.MoveInHeadAfter(candidate_requestStarRule_edge_h2);
                                    candidate_requestStarRule_node_p1.MoveInHeadAfter(candidate_requestStarRule_edge_n);
                                    graph.MoveHeadAfter(candidate_requestStarRule_edge_h1);
                                    candidate_requestStarRule_node_p1.flags = candidate_requestStarRule_node_p1.flags & ~prev__candidate_requestStarRule_node_p1 | prev__candidate_requestStarRule_node_p1;
                                    candidate_requestStarRule_node_r1.flags = candidate_requestStarRule_node_r1.flags & ~prev__candidate_requestStarRule_node_r1 | prev__candidate_requestStarRule_node_r1;
                                    candidate_requestStarRule_edge_h1.flags = candidate_requestStarRule_edge_h1.flags & ~prev__candidate_requestStarRule_edge_h1 | prev__candidate_requestStarRule_edge_h1;
                                    return matches;
                                }
label7: ;
                            }
                            while( (candidate_requestStarRule_edge_h2 = candidate_requestStarRule_edge_h2.inNext) != head_candidate_requestStarRule_edge_h2 );
                        }
                    }
                    while( (candidate_requestStarRule_edge_n = candidate_requestStarRule_edge_n.inNext) != head_candidate_requestStarRule_edge_n );
                }
                candidate_requestStarRule_node_p1.flags = candidate_requestStarRule_node_p1.flags & ~prev__candidate_requestStarRule_node_p1 | prev__candidate_requestStarRule_node_p1;
                candidate_requestStarRule_node_r1.flags = candidate_requestStarRule_node_r1.flags & ~prev__candidate_requestStarRule_node_r1 | prev__candidate_requestStarRule_node_r1;
                candidate_requestStarRule_edge_h1.flags = candidate_requestStarRule_edge_h1.flags & ~prev__candidate_requestStarRule_edge_h1 | prev__candidate_requestStarRule_edge_h1;
            }
            return matches;
        }
    }

    public class Action_takeRule : LGSPAction
    {
        public Action_takeRule() {
            rulePattern = Rule_takeRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 2, 0+0);
        }

        public override string Name { get { return "takeRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_takeRule instance = new Action_takeRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup takeRule_edge_req 
            int type_id_candidate_takeRule_edge_req = 5;
            for(LGSPEdge head_candidate_takeRule_edge_req = graph.edgesByTypeHeads[type_id_candidate_takeRule_edge_req], candidate_takeRule_edge_req = head_candidate_takeRule_edge_req.typeNext; candidate_takeRule_edge_req != head_candidate_takeRule_edge_req; candidate_takeRule_edge_req = candidate_takeRule_edge_req.typeNext)
            {
                // Implicit target takeRule_node_r from takeRule_edge_req 
                LGSPNode candidate_takeRule_node_r = candidate_takeRule_edge_req.target;
                if(!NodeType_Resource.isMyType[candidate_takeRule_node_r.type.TypeID]) {
                    continue;
                }
                // Extend outgoing takeRule_edge_t from takeRule_node_r 
                LGSPEdge head_candidate_takeRule_edge_t = candidate_takeRule_node_r.outhead;
                if(head_candidate_takeRule_edge_t != null)
                {
                    LGSPEdge candidate_takeRule_edge_t = head_candidate_takeRule_edge_t;
                    do
                    {
                        if(!EdgeType_token.isMyType[candidate_takeRule_edge_t.type.TypeID]) {
                            continue;
                        }
                        // Implicit target takeRule_node_p from takeRule_edge_t 
                        LGSPNode candidate_takeRule_node_p = candidate_takeRule_edge_t.target;
                        if(!NodeType_Process.isMyType[candidate_takeRule_node_p.type.TypeID]) {
                            continue;
                        }
                        if(candidate_takeRule_edge_req.source != candidate_takeRule_node_p) {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_takeRule.takeRule_NodeNums.@r] = candidate_takeRule_node_r;
                        match.Nodes[(int)Rule_takeRule.takeRule_NodeNums.@p] = candidate_takeRule_node_p;
                        match.Edges[(int)Rule_takeRule.takeRule_EdgeNums.@t] = candidate_takeRule_edge_t;
                        match.Edges[(int)Rule_takeRule.takeRule_EdgeNums.@req] = candidate_takeRule_edge_req;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_takeRule_node_r.MoveOutHeadAfter(candidate_takeRule_edge_t);
                            graph.MoveHeadAfter(candidate_takeRule_edge_req);
                            return matches;
                        }
                    }
                    while( (candidate_takeRule_edge_t = candidate_takeRule_edge_t.outNext) != head_candidate_takeRule_edge_t );
                }
            }
            return matches;
        }
    }

    public class Action_unlockRule : LGSPAction
    {
        public Action_unlockRule() {
            rulePattern = Rule_unlockRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 2, 0+0);
        }

        public override string Name { get { return "unlockRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_unlockRule instance = new Action_unlockRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup unlockRule_edge_b 
            int type_id_candidate_unlockRule_edge_b = 1;
            for(LGSPEdge head_candidate_unlockRule_edge_b = graph.edgesByTypeHeads[type_id_candidate_unlockRule_edge_b], candidate_unlockRule_edge_b = head_candidate_unlockRule_edge_b.typeNext; candidate_unlockRule_edge_b != head_candidate_unlockRule_edge_b; candidate_unlockRule_edge_b = candidate_unlockRule_edge_b.typeNext)
            {
                // Implicit source unlockRule_node_r from unlockRule_edge_b 
                LGSPNode candidate_unlockRule_node_r = candidate_unlockRule_edge_b.source;
                if(!NodeType_Resource.isMyType[candidate_unlockRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target unlockRule_node_p from unlockRule_edge_b 
                LGSPNode candidate_unlockRule_node_p = candidate_unlockRule_edge_b.target;
                if(!NodeType_Process.isMyType[candidate_unlockRule_node_p.type.TypeID]) {
                    continue;
                }
                // Extend outgoing unlockRule_edge_hb from unlockRule_node_r 
                LGSPEdge head_candidate_unlockRule_edge_hb = candidate_unlockRule_node_r.outhead;
                if(head_candidate_unlockRule_edge_hb != null)
                {
                    LGSPEdge candidate_unlockRule_edge_hb = head_candidate_unlockRule_edge_hb;
                    do
                    {
                        if(!EdgeType_held_by.isMyType[candidate_unlockRule_edge_hb.type.TypeID]) {
                            continue;
                        }
                        if(candidate_unlockRule_edge_hb.target != candidate_unlockRule_node_p) {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_unlockRule.unlockRule_NodeNums.@r] = candidate_unlockRule_node_r;
                        match.Nodes[(int)Rule_unlockRule.unlockRule_NodeNums.@p] = candidate_unlockRule_node_p;
                        match.Edges[(int)Rule_unlockRule.unlockRule_EdgeNums.@b] = candidate_unlockRule_edge_b;
                        match.Edges[(int)Rule_unlockRule.unlockRule_EdgeNums.@hb] = candidate_unlockRule_edge_hb;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            candidate_unlockRule_node_r.MoveOutHeadAfter(candidate_unlockRule_edge_hb);
                            graph.MoveHeadAfter(candidate_unlockRule_edge_b);
                            return matches;
                        }
                    }
                    while( (candidate_unlockRule_edge_hb = candidate_unlockRule_edge_hb.outNext) != head_candidate_unlockRule_edge_hb );
                }
            }
            return matches;
        }
    }

    public class Action_unmountRule : LGSPAction
    {
        public Action_unmountRule() {
            rulePattern = Rule_unmountRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 2, 1, 0+0);
        }

        public override string Name { get { return "unmountRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_unmountRule instance = new Action_unmountRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup unmountRule_edge_t 
            int type_id_candidate_unmountRule_edge_t = 3;
            for(LGSPEdge head_candidate_unmountRule_edge_t = graph.edgesByTypeHeads[type_id_candidate_unmountRule_edge_t], candidate_unmountRule_edge_t = head_candidate_unmountRule_edge_t.typeNext; candidate_unmountRule_edge_t != head_candidate_unmountRule_edge_t; candidate_unmountRule_edge_t = candidate_unmountRule_edge_t.typeNext)
            {
                // Implicit source unmountRule_node_r from unmountRule_edge_t 
                LGSPNode candidate_unmountRule_node_r = candidate_unmountRule_edge_t.source;
                if(!NodeType_Resource.isMyType[candidate_unmountRule_node_r.type.TypeID]) {
                    continue;
                }
                // Implicit target unmountRule_node_p from unmountRule_edge_t 
                LGSPNode candidate_unmountRule_node_p = candidate_unmountRule_edge_t.target;
                if(!NodeType_Process.isMyType[candidate_unmountRule_node_p.type.TypeID]) {
                    continue;
                }
                LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                match.patternGraph = rulePattern.patternGraph;
                match.Nodes[(int)Rule_unmountRule.unmountRule_NodeNums.@r] = candidate_unmountRule_node_r;
                match.Nodes[(int)Rule_unmountRule.unmountRule_NodeNums.@p] = candidate_unmountRule_node_p;
                match.Edges[(int)Rule_unmountRule.unmountRule_EdgeNums.@t] = candidate_unmountRule_edge_t;
                matches.matchesList.PositionWasFilledFixIt();
                // if enough matches were found, we leave
                if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                {
                    graph.MoveHeadAfter(candidate_unmountRule_edge_t);
                    return matches;
                }
            }
            return matches;
        }
    }

    public class Action_waitingRule : LGSPAction
    {
        public Action_waitingRule() {
            rulePattern = Rule_waitingRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 5, 3, 0+0);
        }

        public override string Name { get { return "waitingRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_waitingRule instance = new Action_waitingRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            const int MAX_NEG_LEVEL = 5;
            int negLevel = 0;
            // Lookup waitingRule_node_r 
            int type_id_candidate_waitingRule_node_r = 1;
            for(LGSPNode head_candidate_waitingRule_node_r = graph.nodesByTypeHeads[type_id_candidate_waitingRule_node_r], candidate_waitingRule_node_r = head_candidate_waitingRule_node_r.typeNext; candidate_waitingRule_node_r != head_candidate_waitingRule_node_r; candidate_waitingRule_node_r = candidate_waitingRule_node_r.typeNext)
            {
                uint prev__candidate_waitingRule_node_r;
                prev__candidate_waitingRule_node_r = candidate_waitingRule_node_r.flags & LGSPNode.IS_MATCHED<<negLevel;
                candidate_waitingRule_node_r.flags |= LGSPNode.IS_MATCHED<<negLevel;
                // Lookup waitingRule_edge_b 
                int type_id_candidate_waitingRule_edge_b = 1;
                for(LGSPEdge head_candidate_waitingRule_edge_b = graph.edgesByTypeHeads[type_id_candidate_waitingRule_edge_b], candidate_waitingRule_edge_b = head_candidate_waitingRule_edge_b.typeNext; candidate_waitingRule_edge_b != head_candidate_waitingRule_edge_b; candidate_waitingRule_edge_b = candidate_waitingRule_edge_b.typeNext)
                {
                    // Implicit source waitingRule_node_r2 from waitingRule_edge_b 
                    LGSPNode candidate_waitingRule_node_r2 = candidate_waitingRule_edge_b.source;
                    if(!NodeType_Resource.isMyType[candidate_waitingRule_node_r2.type.TypeID]) {
                        continue;
                    }
                    if((candidate_waitingRule_node_r2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                        && candidate_waitingRule_node_r2==candidate_waitingRule_node_r
                        )
                    {
                        continue;
                    }
                    uint prev__candidate_waitingRule_node_r2;
                    prev__candidate_waitingRule_node_r2 = candidate_waitingRule_node_r2.flags & LGSPNode.IS_MATCHED<<negLevel;
                    candidate_waitingRule_node_r2.flags |= LGSPNode.IS_MATCHED<<negLevel;
                    // Implicit target waitingRule_node_p1 from waitingRule_edge_b 
                    LGSPNode candidate_waitingRule_node_p1 = candidate_waitingRule_edge_b.target;
                    if(!NodeType_Process.isMyType[candidate_waitingRule_node_p1.type.TypeID]) {
                        candidate_waitingRule_node_r2.flags = candidate_waitingRule_node_r2.flags & ~prev__candidate_waitingRule_node_r2 | prev__candidate_waitingRule_node_r2;
                        continue;
                    }
                    uint prev__candidate_waitingRule_node_p1;
                    prev__candidate_waitingRule_node_p1 = candidate_waitingRule_node_p1.flags & LGSPNode.IS_MATCHED<<negLevel;
                    candidate_waitingRule_node_p1.flags |= LGSPNode.IS_MATCHED<<negLevel;
                    // Extend incoming waitingRule_edge_hb from waitingRule_node_p1 
                    LGSPEdge head_candidate_waitingRule_edge_hb = candidate_waitingRule_node_p1.inhead;
                    if(head_candidate_waitingRule_edge_hb != null)
                    {
                        LGSPEdge candidate_waitingRule_edge_hb = head_candidate_waitingRule_edge_hb;
                        do
                        {
                            if(!EdgeType_held_by.isMyType[candidate_waitingRule_edge_hb.type.TypeID]) {
                                continue;
                            }
                            // Implicit source waitingRule_node_r1 from waitingRule_edge_hb 
                            LGSPNode candidate_waitingRule_node_r1 = candidate_waitingRule_edge_hb.source;
                            if(!NodeType_Resource.isMyType[candidate_waitingRule_node_r1.type.TypeID]) {
                                continue;
                            }
                            if((candidate_waitingRule_node_r1.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                                && (candidate_waitingRule_node_r1==candidate_waitingRule_node_r
                                    || candidate_waitingRule_node_r1==candidate_waitingRule_node_r2
                                    )
                                )
                            {
                                continue;
                            }
                            // Extend incoming waitingRule_edge_req from waitingRule_node_r1 
                            LGSPEdge head_candidate_waitingRule_edge_req = candidate_waitingRule_node_r1.inhead;
                            if(head_candidate_waitingRule_edge_req != null)
                            {
                                LGSPEdge candidate_waitingRule_edge_req = head_candidate_waitingRule_edge_req;
                                do
                                {
                                    if(!EdgeType_request.isMyType[candidate_waitingRule_edge_req.type.TypeID]) {
                                        continue;
                                    }
                                    // Implicit source waitingRule_node_p2 from waitingRule_edge_req 
                                    LGSPNode candidate_waitingRule_node_p2 = candidate_waitingRule_edge_req.source;
                                    if(!NodeType_Process.isMyType[candidate_waitingRule_node_p2.type.TypeID]) {
                                        continue;
                                    }
                                    if((candidate_waitingRule_node_p2.flags & LGSPNode.IS_MATCHED<<negLevel) == LGSPNode.IS_MATCHED<<negLevel
                                        && candidate_waitingRule_node_p2==candidate_waitingRule_node_p1
                                        )
                                    {
                                        continue;
                                    }
                                    LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                                    match.patternGraph = rulePattern.patternGraph;
                                    match.Nodes[(int)Rule_waitingRule.waitingRule_NodeNums.@r2] = candidate_waitingRule_node_r2;
                                    match.Nodes[(int)Rule_waitingRule.waitingRule_NodeNums.@p1] = candidate_waitingRule_node_p1;
                                    match.Nodes[(int)Rule_waitingRule.waitingRule_NodeNums.@r1] = candidate_waitingRule_node_r1;
                                    match.Nodes[(int)Rule_waitingRule.waitingRule_NodeNums.@p2] = candidate_waitingRule_node_p2;
                                    match.Nodes[(int)Rule_waitingRule.waitingRule_NodeNums.@r] = candidate_waitingRule_node_r;
                                    match.Edges[(int)Rule_waitingRule.waitingRule_EdgeNums.@b] = candidate_waitingRule_edge_b;
                                    match.Edges[(int)Rule_waitingRule.waitingRule_EdgeNums.@hb] = candidate_waitingRule_edge_hb;
                                    match.Edges[(int)Rule_waitingRule.waitingRule_EdgeNums.@req] = candidate_waitingRule_edge_req;
                                    matches.matchesList.PositionWasFilledFixIt();
                                    // if enough matches were found, we leave
                                    if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                                    {
                                        candidate_waitingRule_node_r1.MoveInHeadAfter(candidate_waitingRule_edge_req);
                                        candidate_waitingRule_node_p1.MoveInHeadAfter(candidate_waitingRule_edge_hb);
                                        graph.MoveHeadAfter(candidate_waitingRule_edge_b);
                                        graph.MoveHeadAfter(candidate_waitingRule_node_r);
                                        candidate_waitingRule_node_p1.flags = candidate_waitingRule_node_p1.flags & ~prev__candidate_waitingRule_node_p1 | prev__candidate_waitingRule_node_p1;
                                        candidate_waitingRule_node_r2.flags = candidate_waitingRule_node_r2.flags & ~prev__candidate_waitingRule_node_r2 | prev__candidate_waitingRule_node_r2;
                                        candidate_waitingRule_node_r.flags = candidate_waitingRule_node_r.flags & ~prev__candidate_waitingRule_node_r | prev__candidate_waitingRule_node_r;
                                        return matches;
                                    }
                                }
                                while( (candidate_waitingRule_edge_req = candidate_waitingRule_edge_req.inNext) != head_candidate_waitingRule_edge_req );
                            }
                        }
                        while( (candidate_waitingRule_edge_hb = candidate_waitingRule_edge_hb.inNext) != head_candidate_waitingRule_edge_hb );
                    }
                    candidate_waitingRule_node_p1.flags = candidate_waitingRule_node_p1.flags & ~prev__candidate_waitingRule_node_p1 | prev__candidate_waitingRule_node_p1;
                    candidate_waitingRule_node_r2.flags = candidate_waitingRule_node_r2.flags & ~prev__candidate_waitingRule_node_r2 | prev__candidate_waitingRule_node_r2;
                }
                candidate_waitingRule_node_r.flags = candidate_waitingRule_node_r.flags & ~prev__candidate_waitingRule_node_r | prev__candidate_waitingRule_node_r;
            }
            return matches;
        }
    }


    public class MutexPimpedActions : LGSPActions
    {
        public MutexPimpedActions(LGSPGraph lgspgraph, IDumperFactory dumperfactory, String modelAsmName, String actionsAsmName)
            : base(lgspgraph, dumperfactory, modelAsmName, actionsAsmName)
        {
            InitActions();
        }

        public MutexPimpedActions(LGSPGraph lgspgraph)
            : base(lgspgraph)
        {
            InitActions();
        }

        private void InitActions()
        {
            actions.Add("aux_attachResource", (LGSPAction) Action_aux_attachResource.Instance);
            actions.Add("blockedRule", (LGSPAction) Action_blockedRule.Instance);
            actions.Add("giveRule", (LGSPAction) Action_giveRule.Instance);
            actions.Add("ignoreRule", (LGSPAction) Action_ignoreRule.Instance);
            actions.Add("killRule", (LGSPAction) Action_killRule.Instance);
            actions.Add("mountRule", (LGSPAction) Action_mountRule.Instance);
            actions.Add("newRule", (LGSPAction) Action_newRule.Instance);
            actions.Add("passRule", (LGSPAction) Action_passRule.Instance);
            actions.Add("releaseRule", (LGSPAction) Action_releaseRule.Instance);
            actions.Add("releaseStarRule", (LGSPAction) Action_releaseStarRule.Instance);
            actions.Add("requestRule", (LGSPAction) Action_requestRule.Instance);
            actions.Add("requestSimpleRule", (LGSPAction) Action_requestSimpleRule.Instance);
            actions.Add("requestStarRule", (LGSPAction) Action_requestStarRule.Instance);
            actions.Add("takeRule", (LGSPAction) Action_takeRule.Instance);
            actions.Add("unlockRule", (LGSPAction) Action_unlockRule.Instance);
            actions.Add("unmountRule", (LGSPAction) Action_unmountRule.Instance);
            actions.Add("waitingRule", (LGSPAction) Action_waitingRule.Instance);
        }

        public override String Name { get { return "MutexPimpedActions"; } }
        public override String ModelMD5Hash { get { return "ac160ef7c8b339e3f8207121808da3a1"; } }
    }
}