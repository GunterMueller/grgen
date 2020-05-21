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
import de.unika.ipd.grgen.ast.typedecl.SetTypeNode;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.exprevals.IncidentEdgeExpr;
import de.unika.ipd.grgen.parser.Coords;

/**
 * A node yielding the incident/incoming/outgoing edges of a node.
 */
public class IncidentEdgeExprNode extends NeighborhoodQueryExprNode
{
	static {
		setName(IncidentEdgeExprNode.class, "incident edge expr");
	}

	private SetTypeNode setTypeNode;


	public IncidentEdgeExprNode(Coords coords,
			ExprNode startNodeExpr,
			ExprNode incidentTypeExpr, int direction,
			ExprNode adjacentTypeExpr)
	{
		super(coords, startNodeExpr, incidentTypeExpr, direction, adjacentTypeExpr);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(startNodeExpr);
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
		childrenNames.add("incident type expr");
		childrenNames.add("adjacent type expr");
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		setTypeNode = new SetTypeNode(getEdgeRootOfMatchingDirectedness(incidentTypeExpr));
		return setTypeNode.resolve();
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal()
	{
		TypeNode startNodeType = startNodeExpr.getType();
		if(!(startNodeType instanceof NodeTypeNode)) {
			reportError("first argument of incidentEdges(.,.,.) must be a node");
			return false;
		}
		TypeNode incidentType = incidentTypeExpr.getType();
		if(!(incidentType instanceof EdgeTypeNode)) {
			reportError("second argument of incidentEdges(.,.,.) must be an edge type");
			return false;
		}
		TypeNode adjacentType = adjacentTypeExpr.getType();
		if(!(adjacentType instanceof NodeTypeNode)) {
			reportError("third argument of incidentEdges(.,.,.) must be a node type");
			return false;
		}
		return true;
	}

	@Override
	protected IR constructIR()
	{
		// assumes that the direction:int of the AST node uses the same values as the direction of the IR expression
		return new IncidentEdgeExpr(startNodeExpr.checkIR(Expression.class),
				incidentTypeExpr.checkIR(Expression.class), direction,
				adjacentTypeExpr.checkIR(Expression.class),
				getType().getType());
	}

	@Override
	public TypeNode getType()
	{
		return setTypeNode;
	}
}