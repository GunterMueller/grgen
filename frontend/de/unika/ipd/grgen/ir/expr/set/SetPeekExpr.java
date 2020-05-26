/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.expr.set;

import de.unika.ipd.grgen.ir.NeededEntities;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.type.container.SetType;

public class SetPeekExpr extends Expression
{
	private Expression targetExpr, numberExpr;

	public SetPeekExpr(Expression targetExpr, Expression numberExpr)
	{
		super("set peek expr", ((SetType)(targetExpr.getType())).valueType);
		this.targetExpr = targetExpr;
		this.numberExpr = numberExpr;
	}

	public Expression getTargetExpr()
	{
		return targetExpr;
	}

	public Expression getNumberExpr()
	{
		return numberExpr;
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		needs.add(this);
		targetExpr.collectNeededEntities(needs);
		numberExpr.collectNeededEntities(needs);
	}
}