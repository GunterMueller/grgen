/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.5
 * Copyright (C) 2009 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 */

/**
 * @author Moritz Kroll, Edgar Jakumeit
 * @version $Id$
 */

package de.unika.ipd.grgen.ast;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ir.Expression;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.MapAccessExpr;
import de.unika.ipd.grgen.parser.Coords;

public class MapAccessExprNode extends ExprNode
// MAP TODO: hieraus einen operator machen
{
	static {
		setName(MapAccessExprNode.class, "map access expression");
	}

	private ExprNode targetExpr;
	private ExprNode keyExpr;

	public MapAccessExprNode(Coords coords, ExprNode targetExpr, ExprNode keyExpr)
	{
		super(coords);
		this.targetExpr = becomeParent(targetExpr);
		this.keyExpr = becomeParent(keyExpr);
	}

	@Override
	public Collection<? extends BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(targetExpr);
		children.add(keyExpr);
		return children;
	}

	@Override
	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("targetExpr");
		childrenNames.add("keyExpr");
		return childrenNames;
	}

	@Override
	protected boolean checkLocal() {
		boolean success = true;
		TypeNode targetType = targetExpr.getType();
		assert targetType instanceof MapTypeNode: targetExpr + " should have a map type";
		MapTypeNode targetMapType = (MapTypeNode) targetType;
		TypeNode keyType = targetMapType.keyType;
		TypeNode keyExprType = keyExpr.getType();

		if (!keyExprType.isEqual(keyType)) {
			keyExpr = becomeParent(keyExpr.adjustType(keyType, getCoords()));

			if (keyExpr == ConstNode.getInvalid()) {
				success = false;
			}
		}

		return success;
	}

	@Override
	public TypeNode getType() {
		TypeNode targetExprType = targetExpr.getType();
		assert targetExprType instanceof MapTypeNode: targetExprType + " should have a map type";
		MapTypeNode targetExprMapType = (MapTypeNode) targetExprType;

		return targetExprMapType.valueType;
	}

	@Override
	protected IR constructIR() {
		return new MapAccessExpr(targetExpr.checkIR(Expression.class),
				keyExpr.checkIR(Expression.class));
	}
}
