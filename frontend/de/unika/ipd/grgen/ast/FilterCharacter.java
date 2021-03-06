/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2021 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast;

import de.unika.ipd.grgen.ast.decl.executable.ActionDeclNode;

/**
 * AST interface representing filters
 */
public interface FilterCharacter
{
	// returns the name of the filter (plain name without entity in case of an auto-generated filter)
	String getFilterName();

	// returns the action the filter applies to
	ActionDeclNode getActionNode();
}
