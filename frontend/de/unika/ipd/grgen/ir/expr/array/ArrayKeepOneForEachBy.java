/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */
package de.unika.ipd.grgen.ir.expr.array;

import de.unika.ipd.grgen.ir.Entity;
import de.unika.ipd.grgen.ir.NeededEntities;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.type.ArrayType;

public class ArrayKeepOneForEachBy extends Expression
{
	private Expression targetExpr;
	private Entity member;

	public ArrayKeepOneForEachBy(Expression targetExpr, Entity member)
	{
		super("array keep one for each by expr", (ArrayType)targetExpr.getType());
		this.targetExpr = targetExpr;
		this.member = member;
	}

	public Expression getTargetExpr()
	{
		return targetExpr;
	}

	public Entity getMember()
	{
		return member;
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		needs.add(this);
		targetExpr.collectNeededEntities(needs);
	}
}
