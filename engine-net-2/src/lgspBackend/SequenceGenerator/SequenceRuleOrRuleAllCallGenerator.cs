/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

// by Edgar Jakumeit, Moritz Kroll

using System;
using de.unika.ipd.grGen.libGr;
using COMP_HELPER = de.unika.ipd.grGen.lgsp.SequenceComputationGeneratorHelper;

namespace de.unika.ipd.grGen.lgsp
{
    class SequenceRuleOrRuleAllCallGenerator
    {
        internal readonly SequenceRuleCall seqRule;
        internal readonly SequenceGeneratorHelper helper;

        internal readonly RuleInvocation ruleInvocation;
        internal readonly SequenceExpression[] ArgumentExpressions;
        internal readonly SequenceVariable[] ReturnVars;
        internal readonly String specialStr;
        internal readonly String parameterDeclarations;
        internal readonly String parameters;
        internal readonly String matchingPatternClassName;
        internal readonly String patternName;
        internal readonly String ruleName;
        internal readonly String matchType;
        internal readonly String matchName;
        internal readonly String matchesType;
        internal readonly String matchesName;


        public SequenceRuleOrRuleAllCallGenerator(SequenceRuleCall seqRule, SequenceGeneratorHelper helper)
        {
            this.seqRule = seqRule;
            this.helper = helper;

            ruleInvocation = seqRule.RuleInvocation;
            ArgumentExpressions = seqRule.ArgumentExpressions;
            ReturnVars = seqRule.ReturnVars;
            specialStr = seqRule.Special ? "true" : "false";
            parameterDeclarations = null;
            parameters = null;
            if(ruleInvocation.Subgraph != null)
                parameters = helper.BuildParametersInDeclarations(ruleInvocation, ArgumentExpressions, out parameterDeclarations);
            else
                parameters = helper.BuildParameters(ruleInvocation, ArgumentExpressions);
            matchingPatternClassName = TypesHelper.GetPackagePrefixDot(ruleInvocation.Package) + "Rule_" + ruleInvocation.Name;
            patternName = ruleInvocation.Name;
            ruleName = "rule_" + TypesHelper.PackagePrefixedNameUnderscore(ruleInvocation.Package, ruleInvocation.Name);
            matchType = matchingPatternClassName + "." + NamesOfEntities.MatchInterfaceName(patternName);
            matchName = "match_" + seqRule.Id;
            matchesType = "GRGEN_LIBGR.IMatchesExact<" + matchType + ">";
            matchesName = "matches_" + seqRule.Id;
        }

        public void Emit(SourceBuilder source, SequenceGenerator seqGen, bool fireDebugEvents)
        {
            if(ruleInvocation.Subgraph != null)
            {
                source.AppendFront(parameterDeclarations + "\n");
                source.AppendFront("procEnv.SwitchToSubgraph((GRGEN_LIBGR.IGraph)" + helper.GetVar(ruleInvocation.Subgraph) + ");\n");
                source.AppendFront("graph = ((GRGEN_LGSP.LGSPActionExecutionEnvironment)procEnv).graph;\n");
            }

            source.AppendFront(matchesType + " " + matchesName + " = " + ruleName
                + ".Match(procEnv, " + (seqRule.SequenceType == SequenceType.RuleCall ? "1" : "procEnv.MaxMatches")
                + parameters + ");\n");
            for(int i = 0; i < seqRule.Filters.Count; ++i)
            {
                seqGen.EmitFilterCall(source, seqRule.Filters[i], patternName, matchesName);
            }

            if(fireDebugEvents)
                source.AppendFront("procEnv.Matched(" + matchesName + ", null, " + specialStr + ");\n");

            if(seqRule is SequenceRuleCountAllCall)
            {
                SequenceRuleCountAllCall seqRuleCountAll = (SequenceRuleCountAllCall)seqRule;
                source.AppendFront(helper.SetVar(seqRuleCountAll.CountResult, matchesName + ".Count"));
            }

            String insufficientMatchesCondition;
            if(seqRule is SequenceRuleAllCall
                && ((SequenceRuleAllCall)seqRule).ChooseRandom
                && ((SequenceRuleAllCall)seqRule).MinSpecified)
            {
                SequenceRuleAllCall seqRuleAll = (SequenceRuleAllCall)seqRule;
                String minMatchesVarName = "minmatchesvar_" + seqRuleAll.Id;
                source.AppendFrontFormat("int {0} = (int){1};\n", minMatchesVarName, helper.GetVar(seqRuleAll.MinVarChooseRandom));
                insufficientMatchesCondition = matchesName + ".Count < " + minMatchesVarName;
            }
            else
                insufficientMatchesCondition = matchesName + ".Count == 0";

            source.AppendFrontFormat("if({0}) {{\n", insufficientMatchesCondition);
            source.Indent();
            source.AppendFront(COMP_HELPER.SetResultVar(seqRule, "false"));
            source.Unindent();
            source.AppendFront("} else {\n");
            source.Indent();
            source.AppendFront(COMP_HELPER.SetResultVar(seqRule, "true"));
            source.AppendFront("procEnv.PerformanceInfo.MatchesFound += " + matchesName + ".Count;\n");
            if(fireDebugEvents)
                source.AppendFront("procEnv.Finishing(" + matchesName + ", " + specialStr + ");\n");

            SequenceRuleOrRuleAllCallRewritingGenerator rewritingGen = new SequenceRuleOrRuleAllCallRewritingGenerator(this);
            if(seqRule.SequenceType == SequenceType.RuleCall)
                rewritingGen.EmitRewritingRuleCall(source);
            else if(seqRule.SequenceType == SequenceType.RuleCountAllCall || !((SequenceRuleAllCall)seqRule).ChooseRandom) // seq.SequenceType == SequenceType.RuleAll
                rewritingGen.EmitRewritingRuleCountAllCallOrRuleAllCallNonRandom(source);
            else // seq.SequenceType == SequenceType.RuleAll && ((SequenceRuleAll)seqRule).ChooseRandom
                rewritingGen.EmitRewritingRuleAllCallRandom(source);

            if(fireDebugEvents)
                source.AppendFront("procEnv.Finished(" + matchesName + ", " + specialStr + ");\n");

            source.Unindent();
            source.AppendFront("}\n");

            if(ruleInvocation.Subgraph != null)
            {
                source.AppendFront("procEnv.ReturnFromSubgraph();\n");
                source.AppendFront("graph = ((GRGEN_LGSP.LGSPActionExecutionEnvironment)procEnv).graph;\n");
            }
        }
    }
}