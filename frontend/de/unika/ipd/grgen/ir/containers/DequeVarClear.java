/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.0
 * Copyright (C) 2003-2013 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.containers;

import de.unika.ipd.grgen.ir.*;
import de.unika.ipd.grgen.ir.exprevals.*;

public class DequeVarClear extends EvalStatement {
	Variable target;

	public DequeVarClear(Variable target) {
		super("deque var clear");
		this.target = target;
	}

	public Variable getTarget() {
		return target;
	}

	public void collectNeededEntities(NeededEntities needs)
	{
		if(!isGlobalVariable(target))
			needs.add(target);

		if(getNext()!=null) {
			getNext().collectNeededEntities(needs);
		}
	}
}