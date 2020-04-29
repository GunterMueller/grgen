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

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.exprevals.*;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.containers.ArrayMinExpr;
import de.unika.ipd.grgen.parser.Coords;

public class ArrayMinNode extends ArrayAccumulationMethodNode
{
	static {
		setName(ArrayMinNode.class, "array min");
	}

	public ArrayMinNode(Coords coords, ExprNode targetExpr)
	{
		super(coords, targetExpr);
	}

	@Override
	protected boolean checkLocal() {
		TypeNode targetType = targetExpr.getType();
		if(!(targetType instanceof ArrayTypeNode)) {
			targetExpr.reportError("This argument to array min expression must be of type array<T>");
			return false;
		}
		return true;
	}

	@Override
	public TypeNode getType() {
		return BasicTypeNode.getOperationType(((ArrayTypeNode)targetExpr.getType()).valueType);
	}

	@Override
	public boolean isValidTargetTypeOfAccumulation(TypeNode type) {
		return type.isEqual(BasicTypeNode.doubleType) || type.isEqual(BasicTypeNode.floatType)
				|| type.isEqual(BasicTypeNode.longType) || type.isEqual(BasicTypeNode.intType);
	}

	@Override
	protected IR constructIR() {
		return new ArrayMinExpr(targetExpr.checkIR(Expression.class));
	}
}
