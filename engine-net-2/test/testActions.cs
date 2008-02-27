using System;
using System.Collections.Generic;
using System.Text;
using de.unika.ipd.grGen.libGr;
using de.unika.ipd.grGen.lgsp;
using de.unika.ipd.grGen.Model_test;

namespace de.unika.ipd.grGen.Action_test
{
	public class Rule_testRule : LGSPRulePattern
	{
		private static Rule_testRule instance = null;
		public static Rule_testRule Instance { get { if (instance==null) instance = new Rule_testRule(); return instance; } }

		public static NodeType[] testRule_node_a_AllowedTypes = null;
		public static NodeType[] testRule_node_f_AllowedTypes = null;
		public static NodeType[] testRule_node_m_AllowedTypes = null;
		public static bool[] testRule_node_a_IsAllowedType = null;
		public static bool[] testRule_node_f_IsAllowedType = null;
		public static bool[] testRule_node_m_IsAllowedType = null;
		public static EdgeType[] testRule_edge__edge0_AllowedTypes = null;
		public static EdgeType[] testRule_edge__edge1_AllowedTypes = null;
		public static bool[] testRule_edge__edge0_IsAllowedType = null;
		public static bool[] testRule_edge__edge1_IsAllowedType = null;
		public enum testRule_NodeNums { @a, @f, @m, };
		public enum testRule_EdgeNums { @_edge0, @_edge1, };
		public enum testRule_SubNums { };
		public enum testRule_AltNums { };

#if INITIAL_WARMUP
		public Rule_testRule()
#else
		private Rule_testRule()
#endif
		{
			name = "testRule";
			isSubpattern = false;

			PatternGraph pat_testRule;
			PatternNode testRule_node_a = new PatternNode((int) NodeTypes.@D231_4121, "testRule_node_a", "a", testRule_node_a_AllowedTypes, testRule_node_a_IsAllowedType, 5.5F, -1);
			PatternNode testRule_node_f = new PatternNode((int) NodeTypes.@B21, "testRule_node_f", "f", testRule_node_f_AllowedTypes, testRule_node_f_IsAllowedType, 5.5F, -1);
			PatternNode testRule_node_m = new PatternNode((int) NodeTypes.@D2211_2222_31, "testRule_node_m", "m", testRule_node_m_AllowedTypes, testRule_node_m_IsAllowedType, 5.5F, -1);
			PatternEdge testRule_edge__edge0 = new PatternEdge(testRule_node_a, testRule_node_f, (int) EdgeTypes.@Edge, "testRule_edge__edge0", "_edge0", testRule_edge__edge0_AllowedTypes, testRule_edge__edge0_IsAllowedType, 5.5F, -1);
			PatternEdge testRule_edge__edge1 = new PatternEdge(testRule_node_f, testRule_node_m, (int) EdgeTypes.@Edge, "testRule_edge__edge1", "_edge1", testRule_edge__edge1_AllowedTypes, testRule_edge__edge1_IsAllowedType, 5.5F, -1);
			pat_testRule = new PatternGraph(
				"testRule",
				"",
				new PatternNode[] { testRule_node_a, testRule_node_f, testRule_node_m }, 
				new PatternEdge[] { testRule_edge__edge0, testRule_edge__edge1 }, 
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
				},
				new bool[] {
					false, false, false, },
				new bool[] {
					false, false, },
				new bool[] {
					true, true, true, },
				new bool[] {
					true, true, }
			);
			testRule_node_a.PointOfDefinition = pat_testRule;
			testRule_node_f.PointOfDefinition = pat_testRule;
			testRule_node_m.PointOfDefinition = pat_testRule;
			testRule_edge__edge0.PointOfDefinition = pat_testRule;
			testRule_edge__edge1.PointOfDefinition = pat_testRule;

			patternGraph = pat_testRule;

			inputs = new GrGenType[] { };
			inputNames = new string[] { };
			outputs = new GrGenType[] { };
			outputNames = new string[] { };
		}


		public override IGraphElement[] Modify(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_a = match.Nodes[ (int) testRule_NodeNums.@a];
			LGSPNode node_f = match.Nodes[ (int) testRule_NodeNums.@f];
			LGSPNode node_m = match.Nodes[ (int) testRule_NodeNums.@m];
			Node_D2211_2222_31 node_are = (Node_D2211_2222_31) graph.Retype(node_a, NodeType_D2211_2222_31.typeVar);
			Node_D231_4121 node_fre = (Node_D231_4121) graph.Retype(node_f, NodeType_D231_4121.typeVar);
			Node_D11_2221 node_mre = (Node_D11_2221) graph.Retype(node_m, NodeType_D11_2221.typeVar);
			int var_i = 1234;
			graph.ChangingNodeAttribute(node_are, NodeType_D2211_2222_31.AttributeType_d2211_2222_31, node_are.@d2211_2222_31, var_i);
			node_are.@d2211_2222_31 = var_i;
			var_i = 5678;
			graph.ChangingNodeAttribute(node_fre, NodeType_D231_4121.AttributeType_d231_4121, node_fre.@d231_4121, var_i);
			node_fre.@d231_4121 = var_i;
			var_i = 9012;
			graph.ChangingNodeAttribute(node_mre, NodeType_D11_2221.AttributeType_d11_2221, node_mre.@d11_2221, var_i);
			node_mre.@d11_2221 = var_i;
			return EmptyReturnElements;
		}

		public override IGraphElement[] ModifyNoReuse(LGSPGraph graph, LGSPMatch match)
		{
			LGSPNode node_a = match.Nodes[ (int) testRule_NodeNums.@a];
			LGSPNode node_f = match.Nodes[ (int) testRule_NodeNums.@f];
			LGSPNode node_m = match.Nodes[ (int) testRule_NodeNums.@m];
			Node_D2211_2222_31 node_are = (Node_D2211_2222_31) graph.Retype(node_a, NodeType_D2211_2222_31.typeVar);
			Node_D231_4121 node_fre = (Node_D231_4121) graph.Retype(node_f, NodeType_D231_4121.typeVar);
			Node_D11_2221 node_mre = (Node_D11_2221) graph.Retype(node_m, NodeType_D11_2221.typeVar);
			int var_i = 1234;
			graph.ChangingNodeAttribute(node_are, NodeType_D2211_2222_31.AttributeType_d2211_2222_31, node_are.@d2211_2222_31, var_i);
			node_are.@d2211_2222_31 = var_i;
			var_i = 5678;
			graph.ChangingNodeAttribute(node_fre, NodeType_D231_4121.AttributeType_d231_4121, node_fre.@d231_4121, var_i);
			node_fre.@d231_4121 = var_i;
			var_i = 9012;
			graph.ChangingNodeAttribute(node_mre, NodeType_D11_2221.AttributeType_d11_2221, node_mre.@d11_2221, var_i);
			node_mre.@d11_2221 = var_i;
			return EmptyReturnElements;
		}
		private static String[] addedNodeNames = new String[] {  };
		public override String[] AddedNodeNames { get { return addedNodeNames; } }
		private static String[] addedEdgeNames = new String[] {  };
		public override String[] AddedEdgeNames { get { return addedEdgeNames; } }
	}


    public class Action_testRule : LGSPAction
    {
        public Action_testRule() {
            rulePattern = Rule_testRule.Instance;
            patternGraph = rulePattern.patternGraph;
            DynamicMatch = myMatch; matches = new LGSPMatches(this, 3, 2, 0+0);
        }

        public override string Name { get { return "testRule"; } }
        private LGSPMatches matches;

        public static LGSPAction Instance { get { return instance; } }
        private static Action_testRule instance = new Action_testRule();

        public LGSPMatches myMatch(LGSPGraph graph, int maxMatches, IGraphElement[] parameters)
        {
            matches.matchesList.Clear();
            // Lookup testRule_edge__edge1 
            int edge_type_id_testRule_edge__edge1 = 1;
            for(LGSPEdge edge_head_testRule_edge__edge1 = graph.edgesByTypeHeads[edge_type_id_testRule_edge__edge1], edge_cur_testRule_edge__edge1 = edge_head_testRule_edge__edge1.typeNext; edge_cur_testRule_edge__edge1 != edge_head_testRule_edge__edge1; edge_cur_testRule_edge__edge1 = edge_cur_testRule_edge__edge1.typeNext)
            {
                bool edge_cur_testRule_edge__edge1_prevIsMatched = edge_cur_testRule_edge__edge1.isMatched;
                edge_cur_testRule_edge__edge1.isMatched = true;
                // Implicit source testRule_node_f from testRule_edge__edge1 
                LGSPNode node_cur_testRule_node_f = edge_cur_testRule_edge__edge1.source;
                if(!NodeType_B21.isMyType[node_cur_testRule_node_f.type.TypeID]) {
                    edge_cur_testRule_edge__edge1.isMatched = edge_cur_testRule_edge__edge1_prevIsMatched;
                    continue;
                }
                // Implicit target testRule_node_m from testRule_edge__edge1 
                LGSPNode node_cur_testRule_node_m = edge_cur_testRule_edge__edge1.target;
                if(!NodeType_D2211_2222_31.isMyType[node_cur_testRule_node_m.type.TypeID]) {
                    edge_cur_testRule_edge__edge1.isMatched = edge_cur_testRule_edge__edge1_prevIsMatched;
                    continue;
                }
                // Extend incoming testRule_edge__edge0 from testRule_node_f 
                LGSPEdge edge_head_testRule_edge__edge0 = node_cur_testRule_node_f.inhead;
                if(edge_head_testRule_edge__edge0 != null)
                {
                    LGSPEdge edge_cur_testRule_edge__edge0 = edge_head_testRule_edge__edge0;
                    do
                    {
                        if(!EdgeType_Edge.isMyType[edge_cur_testRule_edge__edge0.type.TypeID]) {
                            continue;
                        }
                        if(edge_cur_testRule_edge__edge0.isMatched
                            && edge_cur_testRule_edge__edge0==edge_cur_testRule_edge__edge1
                            )
                        {
                            continue;
                        }
                        // Implicit source testRule_node_a from testRule_edge__edge0 
                        LGSPNode node_cur_testRule_node_a = edge_cur_testRule_edge__edge0.source;
                        if(!NodeType_D231_4121.isMyType[node_cur_testRule_node_a.type.TypeID]) {
                            continue;
                        }
                        LGSPMatch match = matches.matchesList.GetNextUnfilledPosition();
                        match.patternGraph = rulePattern.patternGraph;
                        match.Nodes[(int)Rule_testRule.testRule_NodeNums.@a] = node_cur_testRule_node_a;
                        match.Nodes[(int)Rule_testRule.testRule_NodeNums.@f] = node_cur_testRule_node_f;
                        match.Nodes[(int)Rule_testRule.testRule_NodeNums.@m] = node_cur_testRule_node_m;
                        match.Edges[(int)Rule_testRule.testRule_EdgeNums.@_edge0] = edge_cur_testRule_edge__edge0;
                        match.Edges[(int)Rule_testRule.testRule_EdgeNums.@_edge1] = edge_cur_testRule_edge__edge1;
                        matches.matchesList.PositionWasFilledFixIt();
                        // if enough matches were found, we leave
                        if(maxMatches > 0 && matches.matchesList.Count >= maxMatches)
                        {
                            node_cur_testRule_node_f.MoveInHeadAfter(edge_cur_testRule_edge__edge0);
                            graph.MoveHeadAfter(edge_cur_testRule_edge__edge1);
                            edge_cur_testRule_edge__edge1.isMatched = edge_cur_testRule_edge__edge1_prevIsMatched;
                            return matches;
                        }
                    }
                    while( (edge_cur_testRule_edge__edge0 = edge_cur_testRule_edge__edge0.inNext) != edge_head_testRule_edge__edge0 );
                }
                edge_cur_testRule_edge__edge1.isMatched = edge_cur_testRule_edge__edge1_prevIsMatched;
            }
            return matches;
        }
    }


    public class testActions : LGSPActions
    {
        public testActions(LGSPGraph lgspgraph, IDumperFactory dumperfactory, String modelAsmName, String actionsAsmName)
            : base(lgspgraph, dumperfactory, modelAsmName, actionsAsmName)
        {
            InitActions();
        }

        public testActions(LGSPGraph lgspgraph)
            : base(lgspgraph)
        {
            InitActions();
        }

        private void InitActions()
        {
            actions.Add("testRule", (LGSPAction) Action_testRule.Instance);
        }

        public override String Name { get { return "testActions"; } }
        public override String ModelMD5Hash { get { return "bb4aa07ae20ecd46a442b4d2a48258b3"; } }
    }
}