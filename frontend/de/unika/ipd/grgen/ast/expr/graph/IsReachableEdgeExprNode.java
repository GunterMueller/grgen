/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

package de.unika.ipd.grgen.ast.expr.graph;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.expr.ExprNode;
import de.unika.ipd.grgen.ast.typedecl.BooleanTypeNode;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.exprevals.IsReachableEdgeExpr;
import de.unika.ipd.grgen.parser.Coords;

/**
 * An ast node telling whether an end edge can be reached from a start node, via incoming/outgoing/incident edges of given type, from/to a node of given type.
 */
public class IsReachableEdgeExprNode extends NeighborhoodQueryExprNode
{
	static {
		setName(IsReachableEdgeExprNode.class, "is reachable edge expr");
	}

	private ExprNode endEdgeExpr;


	public IsReachableEdgeExprNode(Coords coords, 
			ExprNode startNodeExpr, ExprNode endEdgeExpr,
			ExprNode incidentTypeExpr, int direction,
			ExprNode adjacentTypeExpr)
	{
		super(coords, startNodeExpr, incidentTypeExpr, direction, adjacentTypeExpr);
		this.endEdgeExpr = endEdgeExpr;
		becomeParent(this.endEdgeExpr);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(startNodeExpr);
		children.add(endEdgeExpr);
		children.add(incidentTypeExpr);
		children.add(adjacentTypeExpr);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("start node expr");
		childrenNames.add("end edge expr");
		childrenNames.add("incident type expr");
		childrenNames.add("adjacent type expr");
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		return true;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal()
	{
		if(!(startNodeExpr.getType() instanceof NodeTypeNode)) {
			reportError("first argument of isReachableEdge(.,.,.,.) must be a node");
			return false;
		}
		if(!(endEdgeExpr.getType() instanceof EdgeTypeNode)) {
			reportError("second argument of isReachableEdge(.,.,.,.) must be an edge");
			return false;
		}
		if(!(incidentTypeExpr.getType() instanceof EdgeTypeNode)) {
			reportError("third argument of isReachableEdge(.,.,.,.) must be an edge type");
			return false;
		}
		if(!(adjacentTypeExpr.getType() instanceof NodeTypeNode)) {
			reportError("fourth argument of isReachableEdge(.,.,.,.) must be a node type");
			return false;
		}
		return true;
	}

	@Override
	protected IR constructIR()
	{
		// assumes that the direction:int of the AST node uses the same values as the direction of the IR expression
		return new IsReachableEdgeExpr(startNodeExpr.checkIR(Expression.class),
				endEdgeExpr.checkIR(Expression.class),
				incidentTypeExpr.checkIR(Expression.class), direction,
				adjacentTypeExpr.checkIR(Expression.class),
				getType().getType());
	}

	@Override
	public TypeNode getType()
	{
		return BooleanTypeNode.booleanType;
	}
}