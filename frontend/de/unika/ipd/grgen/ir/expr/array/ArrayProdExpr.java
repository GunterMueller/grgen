/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2021 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */
package de.unika.ipd.grgen.ir.expr.array;

import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.type.container.ArrayType;

public class ArrayProdExpr extends ArrayFunctionMethodInvocationBaseExpr
{
	public ArrayProdExpr(Expression targetExpr)
	{
		super("array prod expr", ((ArrayType)(targetExpr.getType())).valueType, targetExpr);
	}
}
