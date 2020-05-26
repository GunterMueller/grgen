/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.expr.numeric;

import de.unika.ipd.grgen.ir.NeededEntities;
import de.unika.ipd.grgen.ir.expr.Expression;

public class MinExpr extends Expression
{
	private Expression leftExpr;
	private Expression rightExpr;

	public MinExpr(Expression leftExpr, Expression rightExpr)
	{
		super("min expr", leftExpr.getType());
		this.leftExpr = leftExpr;
		this.rightExpr = rightExpr;
	}

	public Expression getLeftExpr()
	{
		return leftExpr;
	}

	public Expression getRightExpr()
	{
		return rightExpr;
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		leftExpr.collectNeededEntities(needs);
		rightExpr.collectNeededEntities(needs);
	}
}