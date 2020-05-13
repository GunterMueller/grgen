/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast.containers;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.exprevals.*;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.containers.DequeAsArrayExpr;
import de.unika.ipd.grgen.parser.Coords;

public class DequeAsArrayNode extends ContainerFunctionMethodInvocationBaseExprNode
{
	static {
		setName(DequeAsArrayNode.class, "deque as array expression");
	}

	private ArrayTypeNode arrayTypeNode;

	public DequeAsArrayNode(Coords coords, ExprNode targetExpr)
	{
		super(coords, targetExpr);
	}

	@Override
	public Collection<? extends BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(targetExpr);
		return children;
	}

	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("targetExpr");
		return childrenNames;
	}

	@Override
	protected boolean resolveLocal()
	{
		arrayTypeNode = new ArrayTypeNode(((DequeTypeNode)targetExpr.getType()).valueTypeUnresolved);
		return arrayTypeNode.resolve();
	}

	@Override
	protected boolean checkLocal()
	{
		TypeNode targetType = targetExpr.getType();
		if(!(targetType instanceof DequeTypeNode)) {
			targetExpr.reportError("This argument to deque as array expression must be of type deque<T>");
			return false;
		}
		return true;
	}

	@Override
	public TypeNode getType()
	{
		return arrayTypeNode;
	}

	@Override
	protected IR constructIR()
	{
		return new DequeAsArrayExpr(targetExpr.checkIR(Expression.class), getType().getType());
	}
}
