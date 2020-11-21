/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

package de.unika.ipd.grgen.ir.expr.numeric;

import de.unika.ipd.grgen.ir.NeededEntities;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.expr.invocation.BuiltinFunctionInvocationExpr;

public class AbsExpr extends BuiltinFunctionInvocationExpr
{
	private Expression expr;

	public AbsExpr(Expression expr)
	{
		super("abs expr", expr.getType());
		this.expr = expr;
	}

	public Expression getExpr()
	{
		return expr;
	}

	@Override
	public void collectNeededEntities(NeededEntities needs)
	{
		expr.collectNeededEntities(needs);
	}
}
