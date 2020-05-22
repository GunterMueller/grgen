/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt;

import de.unika.ipd.grgen.ir.*;
import de.unika.ipd.grgen.ir.expr.Expression;
import de.unika.ipd.grgen.ir.expr.graph.Visited;
import de.unika.ipd.grgen.ir.pattern.Variable;

/**
 * Represents a compound assignment var changed visited statement in the IR.
 */
public class CompoundAssignmentVarChangedVisited extends CompoundAssignmentVar
{
	/** The change assignment. */
	private Visited changedTarget;

	/** The operation of the change assignment */
	private int changedOperation;

	public CompoundAssignmentVarChangedVisited(Variable target,
			int compoundAssignmentType, Expression expr,
			int changedAssignmentType, Visited changedTarget)
	{
		super(target, compoundAssignmentType, expr);
		this.changedOperation = changedAssignmentType;
		this.changedTarget = changedTarget;
	}

	public Visited getChangedTarget()
	{
		return changedTarget;
	}

	public int getChangedOperation()
	{
		return changedOperation;
	}

	public String toString()
	{
		return super.toString()
				+ (changedOperation == UNION ? " |> " : changedOperation == INTERSECTION ? " &> " : " => ")
				+ changedTarget.toString();
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		super.collectNeededEntities(needs);

		changedTarget.collectNeededEntities(needs);
	}
}
