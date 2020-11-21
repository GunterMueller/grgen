/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt.procenv;

import de.unika.ipd.grgen.ir.NeededEntities;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.stmt.BuiltinProcedureInvocationBase;

public class RecordProc extends BuiltinProcedureInvocationBase
{
	private Expression toRecordExpr;

	public RecordProc(Expression toRecordExpr)
	{
		super("record procedure");
		this.toRecordExpr = toRecordExpr;
	}

	public Expression getToRecordExpr()
	{
		return toRecordExpr;
	}

	@Override
	public void collectNeededEntities(NeededEntities needs)
	{
		needs.needsGraph();
		toRecordExpr.collectNeededEntities(needs);
	}
}
