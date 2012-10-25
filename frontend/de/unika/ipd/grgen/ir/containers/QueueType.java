/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 3.5
 * Copyright (C) 2003-2012 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.containers;

import de.unika.ipd.grgen.ir.*;

//TODO: there's a lot of code which could be handled in a common way regarding the containers set|map|array|queue 
//should be unified in abstract base classes and algorithms working on them

public class QueueType extends Type {
	Type valueType;

	public QueueType(Type valueType) {
		super("queue type", null);
		this.valueType = valueType;
	}

	public Type getValueType() {
		return valueType;
	}

	/** @see de.unika.ipd.grgen.ir.Type#classify() */
	public int classify() {
		return IS_QUEUE;
	}
}