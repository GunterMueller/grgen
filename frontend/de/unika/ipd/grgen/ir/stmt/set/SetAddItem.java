/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt.set;

import de.unika.ipd.grgen.ir.*;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.expr.Qualification;
import de.unika.ipd.grgen.ir.stmt.ContainerQualProcedureMethodInvocationBase;

public class SetAddItem extends ContainerQualProcedureMethodInvocationBase
{
	Expression valueExpr;

	public SetAddItem(Qualification target, Expression valueExpr)
	{
		super("set add item", target);
		this.valueExpr = valueExpr;
	}

	public Expression getValueExpr()
	{
		return valueExpr;
	}

	@Override
	public void collectNeededEntities(NeededEntities needs)
	{
		super.collectNeededEntities(needs);
		
		valueExpr.collectNeededEntities(needs);
	}
}
