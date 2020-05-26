/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt.array;

import de.unika.ipd.grgen.ir.*;
import de.unika.ipd.grgen.ir.executable.ProcedureBase;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.pattern.Variable;
import de.unika.ipd.grgen.ir.stmt.ProcedureInvocationBase;

public class ArrayVarAddItem extends ProcedureInvocationBase
{
	Variable target;
	Expression valueExpr;
	Expression indexExpr;

	public ArrayVarAddItem(Variable target, Expression valueExpr, Expression indexExpr)
	{
		super("array var add item");
		this.target = target;
		this.valueExpr = valueExpr;
		this.indexExpr = indexExpr;
	}

	public Variable getTarget()
	{
		return target;
	}

	public Expression getValueExpr()
	{
		return valueExpr;
	}

	public Expression getIndexExpr()
	{
		return indexExpr;
	}

	public ProcedureBase getProcedureBase()
	{
		return null; // dummy needed for interface, not accessed because the type of the class already defines the procedure method
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		if(!isGlobalVariable(target))
			needs.add(target);

		getValueExpr().collectNeededEntities(needs);

		if(getIndexExpr() != null)
			getIndexExpr().collectNeededEntities(needs);

		if(getNext() != null) {
			getNext().collectNeededEntities(needs);
		}
	}
}