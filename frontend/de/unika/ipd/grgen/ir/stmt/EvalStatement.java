/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2021 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Moritz Kroll, Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt;

import de.unika.ipd.grgen.ir.*;
import de.unika.ipd.grgen.ir.pattern.Edge;
import de.unika.ipd.grgen.ir.pattern.Node;
import de.unika.ipd.grgen.ir.pattern.OrderedReplacement;
import de.unika.ipd.grgen.ir.pattern.RetypedEdge;
import de.unika.ipd.grgen.ir.pattern.RetypedNode;
import de.unika.ipd.grgen.ir.pattern.Variable;

// too much dedicated assignment statements depending on LHS
// -> TODO: replace by one assignment statement plus LHS expressions denoting the target (two needed because of change assignment with two LHS expressions)
public abstract class EvalStatement extends IR implements OrderedReplacement
{
	EvalStatement next; // may contain following statement, generated by optimization pass breaking up expressions

	public EvalStatement(String name)
	{
		super(name);
	}

	public EvalStatement getNext()
	{
		return next;
	}

	public void setNext(EvalStatement next)
	{
		this.next = next;
	}

	/**
	 * Method collectNeededEntities extracts the nodes, edges, and variables occurring in this EvalStatement.
	 * We don't collect global variables (::-prefixed), as no entities and no processing are needed for them at all, they are only accessed.
	 * @param needs A NeededEntities instance aggregating the needed elements.
	 */
	public void collectNeededEntities(NeededEntities needs)
	{
		// default implementation for statements without children that need to be collected
	}

	public static boolean isGlobalVariable(Entity entity)
	{
		if(entity instanceof Node && !(entity instanceof RetypedNode)) {
			return ((Node)entity).directlyNestingLHSGraph == null;
		} else if(entity instanceof Edge && !(entity instanceof RetypedEdge)) {
			return ((Edge)entity).directlyNestingLHSGraph == null;
		} else if(entity instanceof Variable) {
			return ((Variable)entity).directlyNestingLHSGraph == null;
		}
		return false;
	}
}
