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

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.BaseNode;
import de.unika.ipd.grgen.ast.IdentNode;
import de.unika.ipd.grgen.ast.decl.executable.OperatorDeclNode;
import de.unika.ipd.grgen.ast.expr.ExprNode;
import de.unika.ipd.grgen.ast.model.decl.IndexDeclNode;
import de.unika.ipd.grgen.ast.model.type.InheritanceTypeNode;
import de.unika.ipd.grgen.ast.pattern.PatternGraphNode;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.util.DeclarationResolver;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.model.Index;
import de.unika.ipd.grgen.ir.pattern.IndexAccessOrdering;
import de.unika.ipd.grgen.ir.pattern.Node;

public class MatchNodeByIndexAccessOrderingDeclNode extends MatchNodeByIndexDeclNode
{
	static {
		setName(MatchNodeByIndexAccessOrderingDeclNode.class, "match node by index access ordering decl");
	}

	private boolean ascending;
	private OperatorDeclNode.Operator comp;
	private ExprNode expr;
	private OperatorDeclNode.Operator comp2;
	private ExprNode expr2;

	public MatchNodeByIndexAccessOrderingDeclNode(IdentNode id, BaseNode type, int context,
			boolean ascending, IdentNode index,
			OperatorDeclNode.Operator comp, ExprNode expr,
			OperatorDeclNode.Operator comp2, ExprNode expr2,
			PatternGraphNode directlyNestingLHSGraph)
	{
		super(id, type, context, index, directlyNestingLHSGraph);
		this.ascending = ascending;
		this.comp = comp;
		this.expr = expr;
		becomeParent(this.expr);
		this.comp2 = comp2;
		this.expr2 = expr2;
		becomeParent(this.expr);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(ident);
		children.add(getValidVersion(typeUnresolved, typeNodeDecl, typeTypeDecl));
		children.add(constraints);
		children.add(getValidVersion(indexUnresolved, index));
		if(expr != null)
			children.add(expr);
		if(expr2 != null)
			children.add(expr2);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("ident");
		childrenNames.add("type");
		childrenNames.add("constraints");
		childrenNames.add("index");
		if(expr != null)
			childrenNames.add("expression");
		if(expr2 != null)
			childrenNames.add("expression2");
		return childrenNames;
	}

	private static DeclarationResolver<IndexDeclNode> indexResolver =
			new DeclarationResolver<IndexDeclNode>(IndexDeclNode.class);

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		boolean successfullyResolved = super.resolveLocal();
		index = indexResolver.resolve(indexUnresolved, this);
		successfullyResolved &= index != null;
		if(expr != null)
			successfullyResolved &= expr.resolve();
		if(expr2 != null)
			successfullyResolved &= expr2.resolve();
		return successfullyResolved;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal()
	{
		boolean res = super.checkLocal();
		if((context & CONTEXT_LHS_OR_RHS) == CONTEXT_RHS) {
			reportError("Can't employ match node by index on RHS");
			return false;
		}
		if(expr != null) {
			TypeNode expectedIndexAccessType = index.getExpectedAccessType();
			TypeNode indexAccessType = expr.getType();
			if(!indexAccessType.isCompatibleTo(expectedIndexAccessType)) {
				String expTypeName = expectedIndexAccessType.getTypeName();
				String typeName = indexAccessType.getTypeName();
				ident.reportError("Cannot convert type used in accessing index from \"" + typeName
						+ "\" to \"" + expTypeName + "\" in match node by index access");
				return false;
			}
			if(expr2 != null) {
				TypeNode indexAccessType2 = expr2.getType();
				if(!indexAccessType.isCompatibleTo(expectedIndexAccessType)) {
					String expTypeName = expectedIndexAccessType.getTypeName();
					String typeName = indexAccessType2.getTypeName();
					ident.reportError("Cannot convert type used in accessing index from \"" + typeName
							+ "\" to \"" + expTypeName + "\" in match node by index access");
					return false;
				}
			}
		}
		TypeNode expectedEntityType = getDeclType();
		InheritanceTypeNode entityType = index.getType();
		if(!entityType.isCompatibleTo(expectedEntityType) && !expectedEntityType.isCompatibleTo(entityType)) {
			String expTypeName = expectedEntityType.getTypeName();
			String typeName = entityType.getTypeName();
			ident.reportError("Cannot convert index type from \"" + typeName
					+ "\" to pattern element type \"" + expTypeName + "\" in match node by index access");
			return false;
		}
		if(comp == OperatorDeclNode.Operator.LT || comp == OperatorDeclNode.Operator.LE) {
			if(expr2 != null && (comp2 == OperatorDeclNode.Operator.LT || comp2 == OperatorDeclNode.Operator.LE)) {
				reportError("Match node by index does not support two lower bounds");
				return false;
			}
		}
		if(comp == OperatorDeclNode.Operator.GT || comp == OperatorDeclNode.Operator.GE) {
			if(expr2 != null && (comp2 == OperatorDeclNode.Operator.GT || comp2 == OperatorDeclNode.Operator.GE)) {
				reportError("Match node by index does not support two upper bounds");
				return false;
			}
		}
		return res;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#constructIR() */
	@Override
	protected IR constructIR()
	{
		if(isIRAlreadySet()) { // break endless recursion in case of cycle in usage
			return getIR();
		}

		Node node = (Node)super.constructIR();

		setIR(node);

		if(expr != null)
			expr = expr.evaluate();
		if(expr2 != null)
			expr2 = expr2.evaluate();;
		node.setIndex(new IndexAccessOrdering(index.checkIR(Index.class), ascending,
				comp, expr != null ? expr.checkIR(Expression.class) : null,
				comp2, expr2 != null ? expr2.checkIR(Expression.class) : null));
		return node;
	}
}
