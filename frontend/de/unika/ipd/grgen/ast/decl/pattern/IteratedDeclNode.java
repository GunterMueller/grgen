/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast.decl.pattern;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.BaseNode;
import de.unika.ipd.grgen.ast.IdentNode;
import de.unika.ipd.grgen.ast.decl.executable.ActionDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.FilterAutoGeneratedDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.FilterAutoDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.FilterAutoSuppliedDeclNode;
import de.unika.ipd.grgen.ast.pattern.PatternGraphNode;
import de.unika.ipd.grgen.ast.type.IteratedTypeNode;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.executable.FilterAutoGenerated;
import de.unika.ipd.grgen.ir.executable.FilterAutoSupplied;
import de.unika.ipd.grgen.ir.executable.Rule;
import de.unika.ipd.grgen.ir.pattern.PatternGraph;
import de.unika.ipd.grgen.ir.stmt.EvalStatements;

/**
 * AST node for an iterated pattern, maybe including replacements.
 */
public class IteratedDeclNode extends ActionDeclNode
{
	static {
		setName(IteratedDeclNode.class, "iterated");
	}

	public RhsDeclNode right;
	private IteratedTypeNode type;
	private int minMatches;
	private int maxMatches;
	private ArrayList<FilterAutoDeclNode> filters;

	/** Type for this declaration. */
	private static IteratedTypeNode iteratedType = new IteratedTypeNode();

	/**
	 * Make a new iterated rule.
	 * @param left The left hand side (The pattern to match).
	 * @param right The right hand side.
	 */
	public IteratedDeclNode(IdentNode id, PatternGraphNode left, RhsDeclNode right, int minMatches, int maxMatches)
	{
		super(id, iteratedType, left);
		this.right = right;
		becomeParent(this.right);
		this.minMatches = minMatches;
		this.maxMatches = maxMatches;
		this.filters = new ArrayList<FilterAutoDeclNode>();
	}

	public void addFilters(ArrayList<FilterAutoDeclNode> filters)
	{
		this.filters.addAll(filters);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(ident);
		children.add(getValidVersion(typeUnresolved, type));
		children.add(pattern);
		if(right != null)
			children.add(right);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("ident");
		childrenNames.add("type");
		childrenNames.add("pattern");
		if(right != null)
			childrenNames.add("right");
		return childrenNames;
	}

	private static final DeclarationTypeResolver<IteratedTypeNode> typeResolver =
			new DeclarationTypeResolver<IteratedTypeNode>(IteratedTypeNode.class);

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		type = typeResolver.resolve(typeUnresolved, this);

		return type != null
				&& resolveFilters(filters);
	}

	/**
	 * Check, if the rule type node is right.
	 * The children of a rule type are
	 * 1) a pattern for the left side.
	 * 2) a pattern for the right side.
	 * @see de.unika.ipd.grgen.ast.BaseNode#checkLocal()
	 */
	@Override
	protected boolean checkLocal()
	{
		if(right != null)
			right.warnElemAppearsInsideAndOutsideDelete(pattern);

		boolean leftHandGraphsOk = checkLeft();

		boolean noReturnInPatternOk = true;
		if(pattern.returns.size() > 0) {
			error.error(getCoords(), "No return statements in pattern parts of rules allowed");
			noReturnInPatternOk = false;
		}

		boolean noReturnInAlterntiveCaseReplacement = true;
		if(right != null) {
			if(right.graph.returns.size() > 0) {
				error.error(getCoords(), "No return statements in alternative cases allowed");
				noReturnInAlterntiveCaseReplacement = false;
			}
		}

		boolean abstr = true;
		boolean rhsReuseOk = true;
		boolean execParamsNotDeleted = true;
		boolean sameNumberOfRewriteParts = sameNumberOfRewriteParts(right, "iterated/multiple/optional");
		boolean noNestedRewriteParameters = true;
		if(right != null) {
			rhsReuseOk = checkRhsReuse(right);
			execParamsNotDeleted = checkExecParamsNotDeleted(right);
			noNestedRewriteParameters = noNestedRewriteParameters(right, "iterated/multiple/optional");
			abstr = noAbstractElementInstantiatedNestedPattern(right);
		}

		return leftHandGraphsOk
				& checkFilters(pattern, filters)
				& sameNumberOfRewriteParts
				& noNestedRewriteParameters
				& rhsReuseOk
				& noReturnInPatternOk
				& noReturnInAlterntiveCaseReplacement
				& execParamsNotDeleted
				& abstr;
	}

	public PatternGraphNode getLeft()
	{
		return pattern;
	}

	/**
	 * @see de.unika.ipd.grgen.ast.BaseNode#constructIR()
	 */
	@Override
	protected IR constructIR()
	{
		// return if the pattern graph already constructed the IR object
		// that may happen in recursive patterns (and other usages/references)
		if(isIRAlreadySet()) {
			return getIR();
		}

		Rule iteratedRule = new Rule(getIdentNode().getIdent(), minMatches, maxMatches);

		// mark this node as already visited
		setIR(iteratedRule);

		PatternGraph left = pattern.getPatternGraph();

		PatternGraph rightPattern = null;
		if(this.right != null) {
			rightPattern = this.right.getPatternGraph(left);
		}

		iteratedRule.initialize(left, rightPattern);

		constructImplicitNegs(left);
		constructIRaux(iteratedRule, right);

		// filters add themselves to the iterated rule when their IR is constructed
		for(FilterAutoDeclNode filter : filters) {
			if(filter instanceof FilterAutoSuppliedDeclNode) {
				((FilterAutoSuppliedDeclNode)filter).checkIR(FilterAutoSupplied.class);
			} else {
				((FilterAutoGeneratedDeclNode)filter).checkIR(FilterAutoGenerated.class);
			}
		}

		// add Eval statements to the IR
		if(this.right != null) {
			for(EvalStatements evalStatements : this.right.getRHSGraph().getYieldEvalStatements()) {
				iteratedRule.addEval(evalStatements);
			}
		}

		return iteratedRule;
	}

	@Override
	public IteratedTypeNode getDeclType()
	{
		assert isResolved();

		return iteratedType;
	}

	public static String getKindStr()
	{
		return "iterated node";
	}

	public static String getUseStr()
	{
		return "iterated";
	}
}