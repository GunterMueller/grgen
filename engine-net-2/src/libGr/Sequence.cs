/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.5
 * Copyright (C) 2009 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 */

using System;
using System.Collections.Generic;

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// Specifies the actual subtype used for a Sequence.
    /// </summary>
    public enum SequenceType
    {
        LazyOr, LazyAnd, StrictOr, Xor, StrictAnd, Not, Min, MinMax,
        Rule, RuleAll, Def, True, False, VarPredicate,
        AssignVarToVar, AssignElemToVar, AssignSequenceResultToVar,
        Transaction
    }

    /// <summary>
    /// A sequence object with references to child sequences.
    /// </summary>
    public abstract class Sequence
    {
        /// <summary>
        /// A common random number generator for all sequence objects.
        /// It uses a time-dependent seed.
        /// </summary>
        public static Random randomGenerator = new Random();

        /// <summary>
        /// The type of the sequence (e.g. LazyOr or Transaction)
        /// </summary>
        public SequenceType SequenceType;

        /// <summary>
        /// Initializes a new Sequence object with the given sequence type.
        /// </summary>
        /// <param name="seqType">The sequence type.</param>
        public Sequence(SequenceType seqType)
        {
            SequenceType = seqType;
        }

        /// <summary>
        /// Applies this sequence.
        /// </summary>
        /// <param name="graph">The graph on which this sequence is to be applied.
        ///     The rules will only be chosen during the Sequence object instantiation, so
        ///     exchanging rules will have no effect for already existing Sequence objects.</param>
        /// <returns>True, iff the sequence succeeded</returns>
        public bool Apply(IGraph graph)
        {
            graph.EnteringSequence(this);
            bool res = ApplyImpl(graph);
            graph.ExitingSequence(this);
            return res;
        }

        /// <summary>
        /// Applies this sequence. This function represents the actual implementation of the sequence.
        /// </summary>
        /// <param name="graph">The graph on which this sequence is to be applied.</param>
        /// <returns>True, iff the sequence succeeded</returns>
        protected abstract bool ApplyImpl(IGraph graph);

        /// <summary>
        /// Enumerates all child sequence objects
        /// </summary>
        public abstract IEnumerable<Sequence> Children { get; }

        /// <summary>
        /// The precedence of this operator. Zero is the highest priority, int.MaxValue the lowest.
        /// Used to add needed parentheses for printing sequences
        /// </summary>
        public abstract int Precedence { get; }

        /// <summary>
        /// A string symbol representing this sequence type.
        /// </summary>
        public abstract String Symbol { get; }
    }

    /// <summary>
    /// A Sequence with a Special flag
    /// </summary>
    public abstract class SequenceSpecial : Sequence
    {
        /// <summary>
        /// The "Special" flag. Usage is implementation specific.
        /// GrShell uses this flag to indicate breakpoints when in debug mode and
        /// to dump matches when in normal mode.
        /// </summary>
        public bool Special;

        /// <summary>
        /// Initializes a new instance of the SequenceSpecial class.
        /// </summary>
        /// <param name="special">The initial value for the "Special" flag.</param>
        /// <param name="seqType">The sequence type.</param>
        public SequenceSpecial(bool special, SequenceType seqType)
            : base(seqType)
        {
            Special = special;
        }
    }

    /// <summary>
    /// A sequence consisting of a unary operator and another sequence.
    /// </summary>
    public abstract class SequenceUnary : Sequence
    {
        public Sequence Seq;

        public SequenceUnary(Sequence seq, SequenceType seqType) : base(seqType)
        {
            Seq = seq;
        }

        public override IEnumerable<Sequence> Children
        {
            get { yield return Seq; }
        }
    }

    /// <summary>
    /// A sequence consisting of a binary operator and two sequences.
    /// </summary>
    public abstract class SequenceBinary : Sequence
    {
        public Sequence Left, Right;
        public bool Randomize;

        public SequenceBinary(Sequence left, Sequence right, bool random, SequenceType seqType)
            : base(seqType)
        {
            Left = left;
            Right = right;
            Randomize = random;
        }

        public override IEnumerable<Sequence> Children
        {
            get { yield return Left; yield return Right; }
        }
    }

    public class SequenceLazyOr : SequenceBinary
    {
        public SequenceLazyOr(Sequence left, Sequence right, bool random)
            : base(left, right, random, SequenceType.LazyOr)
        {
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            if(Randomize && randomGenerator.Next(2) == 1)
                return Right.Apply(graph) || Left.Apply(graph);
            else
                return Left.Apply(graph) || Right.Apply(graph);
        }

        public override int Precedence { get { return 0; } }
        public override string Symbol { get { return "||"; } }
    }

    public class SequenceLazyAnd : SequenceBinary
    {
        public SequenceLazyAnd(Sequence left, Sequence right, bool random)
            : base(left, right, random, SequenceType.LazyAnd)
        {
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            if(Randomize && randomGenerator.Next(2) == 1)
                return Right.Apply(graph) && Left.Apply(graph);
            else
                return Left.Apply(graph) && Right.Apply(graph);
        }

        public override int Precedence { get { return 1; } }
        public override string Symbol { get { return "&&"; } }
    }

    public class SequenceStrictOr : SequenceBinary
    {
        public SequenceStrictOr(Sequence left, Sequence right, bool random)
            : base(left, right, random, SequenceType.StrictOr)
        {
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            if(Randomize && randomGenerator.Next(2) == 1)
                return Right.Apply(graph) | Left.Apply(graph);
            else
                return Left.Apply(graph) | Right.Apply(graph);
        }

        public override int Precedence { get { return 2; } }
        public override string Symbol { get { return "|"; } }
    }

    public class SequenceXor : SequenceBinary
    {
        public SequenceXor(Sequence left, Sequence right, bool random)
            : base(left, right, random, SequenceType.Xor)
        {
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            if(Randomize && randomGenerator.Next(2) == 1)
                return Right.Apply(graph) ^ Left.Apply(graph);
            else
                return Left.Apply(graph) ^ Right.Apply(graph);
        }

        public override int Precedence { get { return 3; } }
        public override string Symbol { get { return "^"; } }
    }

    public class SequenceStrictAnd : SequenceBinary
    {
        public SequenceStrictAnd(Sequence left, Sequence right, bool random)
            : base(left, right, random, SequenceType.StrictAnd)
        {
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            if(Randomize && randomGenerator.Next(2) == 1)
                return Right.Apply(graph) & Left.Apply(graph);
            else
                return Left.Apply(graph) & Right.Apply(graph);
        }

        public override int Precedence { get { return 4; } }
        public override string Symbol { get { return "&"; } }
    }

    public class SequenceNot : SequenceUnary
    {
        public SequenceNot(Sequence seq) : base(seq, SequenceType.Not) {}

        protected override bool ApplyImpl(IGraph graph)
        {
            return !Seq.Apply(graph);
        }

        public override int Precedence { get { return 5; } }
        public override string Symbol { get { return "!"; } }
    }

    public class SequenceMin : SequenceUnary
    {
        public long Min;

        public SequenceMin(Sequence seq, long min) : base(seq, SequenceType.Min)
        {
            Min = min;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            long i = 0;
            while(Seq.Apply(graph))
                i++;
            return i >= Min;
        }

        public override int Precedence { get { return 6; } }
        public override string Symbol { get { return "[" + Min + ":*]"; } }
    }

    public class SequenceMinMax : SequenceUnary
    {
        public long Min, Max;

        public SequenceMinMax(Sequence seq, long min, long max) : base(seq, SequenceType.MinMax)
        {
            Min = min;
            Max = max;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            long i;
            for(i = 0; i < Max; i++)
            {
                if(!Seq.Apply(graph)) break;
            }
            return i >= Min;
        }

        public override int Precedence { get { return 6; } }
        public override string Symbol { get { return "[" + Min + ":" + Max + "]"; } }
    }

    public class SequenceRule : SequenceSpecial
    {
        public RuleObject RuleObj;
        public bool Test;

        public SequenceRule(RuleObject ruleObj, bool special, bool test)
            : base(special, SequenceType.Rule)
        {
            RuleObj = ruleObj;
            Test = test;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            return graph.ApplyRewrite(RuleObj, 0, 1, Special, Test) > 0;
        }

        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }

        protected String GetRuleString()
        {
            String sym = "";
            if(RuleObj.ReturnVars.Length > 0 && RuleObj.ReturnVars[0] != null)
                sym = "(" + String.Join(", ", RuleObj.ReturnVars) + ")=";
            sym += RuleObj.Action.Name;
            if(RuleObj.ParamVars.Length > 0)
                sym += "(" + String.Join(", ", RuleObj.ParamVars) + ")";
            return sym;
        }

        public override string Symbol      
        {
            get
            {
                String prefix;
                if(Special)
                {
                    if(Test) prefix = "%?";
                    else prefix = "%";
                }
                else
                {
                    if(Test) prefix = "?";
                    else prefix = "";
                }
                return prefix + GetRuleString();
            }
        }
    }

    public class SequenceRuleAll : SequenceRule
    {
		public int NumChooseRandom;

        public SequenceRuleAll(RuleObject ruleObj, bool special, bool test, int numChooseRandom)
            : base(ruleObj, special, test)
        {
            SequenceType = SequenceType.RuleAll;
			NumChooseRandom = numChooseRandom;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
			if(NumChooseRandom <= 0)
				return graph.ApplyRewrite(RuleObj, -1, -1, Special, Test) > 0;
			else
			{
                // TODO: Code duplication! Compare with BaseGraph.ApplyRewrite.

				int curMaxMatches = graph.MaxMatches;

				object[] parameters;
				if(RuleObj.ParamVars.Length > 0)
				{
					parameters = RuleObj.Parameters;
                    for(int i = 0; i < RuleObj.ParamVars.Length; i++)
                    {
                        // If this parameter is not constant, the according ParamVars entry holds the
                        // name of a variable to be used for the parameter.
                        // Otherwise the parameters entry remains unchanged (it already contains the constant)
                        if(RuleObj.ParamVars[i] != null)
                            parameters[i] = graph.GetVariableValue(RuleObj.ParamVars[i]);
                    }
				}
				else parameters = null;

				if(graph.PerformanceInfo != null) graph.PerformanceInfo.StartLocal();
				IMatches matches = RuleObj.Action.Match(graph, curMaxMatches, parameters);
				if(graph.PerformanceInfo != null)
				{
					graph.PerformanceInfo.StopMatch();              // total match time does NOT include listeners anymore
					graph.PerformanceInfo.MatchesFound += matches.Count;
				}

				graph.Matched(matches, Special);
				if(matches.Count == 0) return false;

				if(Test) return false;

				graph.Finishing(matches, Special);

				if(graph.PerformanceInfo != null) graph.PerformanceInfo.StartLocal();

				object[] retElems = null;
				for(int i = 0; i < NumChooseRandom; i++)
				{
					if(i != 0) graph.RewritingNextMatch();
					IMatch match = matches.RemoveMatch(randomGenerator.Next(matches.Count));
					retElems = matches.Producer.Modify(graph, match);
					if(graph.PerformanceInfo != null) graph.PerformanceInfo.RewritesPerformed++;
				}
				if(retElems == null) retElems = BaseGraph.NoElems;

				for(int i = 0; i < RuleObj.ReturnVars.Length; i++)
					graph.SetVariableValue(RuleObj.ReturnVars[i], retElems[i]);
				if(graph.PerformanceInfo != null) graph.PerformanceInfo.StopRewrite();            // total rewrite time does NOT include listeners anymore

				graph.Finished(matches, Special);

				return true;
			}
        }

        public override string Symbol
        { 
            get 
            {
                String prefix;
				if(NumChooseRandom > 0)
					prefix = "$" + NumChooseRandom;
				else
					prefix = "";
                if(Special)
                {
                    if(Test) prefix += "[%?";
                    else prefix += "[%";
                }
                else
                {
                    if(Test) prefix += "[?";
                    else prefix += "[";
                }
                return prefix + GetRuleString() + "]"; 
            }
        }
    }

    public class SequenceDef : Sequence
    {
        public String[] DefVars;

        public SequenceDef(String[] defVars)
            : base(SequenceType.Def)
        {
            DefVars = defVars;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            foreach(String defVar in DefVars)
                if(graph.GetVariableValue(defVar) == null) 
                    return false;

            return true;
        }

        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return "def(" + String.Join(", ", DefVars) + ")"; } }
    }

    public class SequenceTrue : SequenceSpecial
    {
        public SequenceTrue(bool special)
            : base(special, SequenceType.True)
        {
        }

        protected override bool ApplyImpl(IGraph graph) { return true; }
        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return Special ? "%true" : "true"; } }
    }

    public class SequenceFalse : SequenceSpecial
    {
        public SequenceFalse(bool special)
            : base(special, SequenceType.False)
        {
        }

        protected override bool ApplyImpl(IGraph graph) { return false; }
        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return Special ? "%false" : "false"; } }
    }

    public class SequenceVarPredicate : SequenceSpecial
    {
        public String PredicateVar;

        public SequenceVarPredicate(String varName, bool special)
            : base(special, SequenceType.VarPredicate)
        {
            PredicateVar = varName;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            object val = graph.GetVariableValue(PredicateVar);
            if(val is bool) return (bool) val;

            throw new InvalidOperationException("The variable '" + PredicateVar + "' is not boolean!");
        }
        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return PredicateVar; } }
    }

    public class SequenceAssignVarToVar : Sequence
    {
        public String DestVar;
        public String SourceVar;

        public SequenceAssignVarToVar(String destVar, String sourceVar)
            : base(SequenceType.AssignVarToVar)
        {
            DestVar = destVar;
            SourceVar = sourceVar;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            graph.SetVariableValue(DestVar, graph.GetVariableValue(SourceVar));
            return true;                    // Semantics changed! Now always returns true, as it is always successful!
        }

        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return DestVar + "=" + SourceVar; } }
    }

    public class SequenceAssignElemToVar : Sequence
    {
        public String DestVar;
        public IGraphElement Element;

        public SequenceAssignElemToVar(String destVar, IGraphElement elem)
            : base(SequenceType.AssignElemToVar)
        {
            DestVar = destVar;
            Element = elem;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            graph.SetVariableValue(DestVar, Element);
            return true;                    // Semantics changed! Now always returns true, as it is always successful!
        }

        public override IEnumerable<Sequence> Children { get { yield break; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return DestVar + "=[<someelem>]"; } }
    }

    public class SequenceAssignSequenceResultToVar : Sequence
    {
        public String DestVar;
        public Sequence Seq;

        public SequenceAssignSequenceResultToVar(String destVar, Sequence sequence)
            : base(SequenceType.AssignSequenceResultToVar)
        {
            DestVar = destVar;
            Seq = sequence;
        }

        protected override bool ApplyImpl(IGraph graph)
        {
            bool result = Seq.Apply(graph);
            graph.SetVariableValue(DestVar, result);
            return result;
        }

        public override IEnumerable<Sequence> Children { get { yield return Seq; } }
        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return DestVar + "="; } }
    }

    public class SequenceTransaction : SequenceUnary
    {
        public SequenceTransaction(Sequence seq) : base(seq, SequenceType.Transaction) { }

        protected override bool ApplyImpl(IGraph graph)
        {
            int transactionID = graph.TransactionManager.StartTransaction();
            int oldRewritesPerformed;

            if(graph.PerformanceInfo != null) oldRewritesPerformed = graph.PerformanceInfo.RewritesPerformed;
            else oldRewritesPerformed = -1;

            bool res = Seq.Apply(graph);

            if(res) graph.TransactionManager.Commit(transactionID);
            else
            {
                graph.TransactionManager.Rollback(transactionID);
                if(graph.PerformanceInfo != null)
                    graph.PerformanceInfo.RewritesPerformed = oldRewritesPerformed;
            }

            return res;
        }

        public override int Precedence { get { return 7; } }
        public override string Symbol { get { return "<>"; } }
    }
}