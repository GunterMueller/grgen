/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.0
 * Copyright (C) 2008 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under GPL v3 (see LICENSE.txt included in the packaging of this file)
 */

/**
 * @author Edgar Jakumeit
 * @version $Id: MapRemoveItemNode.java 23003 2008-10-19 00:13:05Z eja $
 */

package de.unika.ipd.grgen.ast;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ir.Expression;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.SetRemoveItem;
import de.unika.ipd.grgen.ir.Qualification;
import de.unika.ipd.grgen.parser.Coords;

public class SetRemoveItemNode extends ExprNode
{
	static {
		setName(SetRemoveItemNode.class, "set remove item");
	}

	MemberAccessExprNode target;
	ExprNode valueExpr;

	public SetRemoveItemNode(Coords coords, MemberAccessExprNode target, ExprNode valueExpr)
	{
		super(coords);
		this.target = becomeParent(target);
		this.valueExpr = becomeParent(valueExpr);
	}

	public Collection<? extends BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(target);
		children.add(valueExpr);
		return children;
	}

	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("target");
		childrenNames.add("valueExpr");
		return childrenNames;
	}

	protected boolean resolveLocal() {
		return true;
	}

	protected boolean checkLocal() {
		boolean success = true;
		MemberDeclNode targetDecl = target.getDecl();

		if (targetDecl.isConst()) {
			error.error(getCoords(), "removing items of a const set is not allowed");
			success = false;
		}
		TypeNode targetType = targetDecl.getDeclType();
		assert targetType instanceof SetTypeNode: target + " should have a set type";
		SetTypeNode targetSetType = (SetTypeNode) targetType;
		TypeNode valueType = targetSetType.valueType;
		TypeNode valueExprType = valueExpr.getType();

		if (!valueExprType.isEqual(valueType)) {
			valueExpr = becomeParent(valueExpr.adjustType(valueType, getCoords()));

			if (valueExpr == ConstNode.getInvalid()) {
				success = false;
			}
		}

		return success;
	}

	protected IR constructIR() {
		return new SetRemoveItem(target.checkIR(Qualification.class),
				valueExpr.checkIR(Expression.class));
	}
	
	public TypeNode getType() {
		return target.getDecl().getDeclType();
	}
	
	public MemberAccessExprNode getTarget() {
		return target;
	}
}