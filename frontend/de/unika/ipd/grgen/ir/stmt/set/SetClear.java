/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2021 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir.stmt.set;

import de.unika.ipd.grgen.ir.expr.Qualification;
import de.unika.ipd.grgen.ir.stmt.ContainerQualProcedureMethodInvocationBase;

public class SetClear extends ContainerQualProcedureMethodInvocationBase
{
	public SetClear(Qualification target)
	{
		super("set clear", target);
	}
}
