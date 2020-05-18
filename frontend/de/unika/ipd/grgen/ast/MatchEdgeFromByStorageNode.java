/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */
package de.unika.ipd.grgen.ast;

import de.unika.ipd.grgen.ast.exprevals.*;

public abstract class MatchEdgeFromByStorageNode extends EdgeDeclNode
{
	static {
		setName(MatchEdgeFromByStorageNode.class, "match edge from by storage decl");
	}

	protected BaseNode storageUnresolved;
	protected VarDeclNode storage = null;
	protected QualIdentNode storageAttribute = null;
	protected EdgeDeclNode storageGlobalVariable = null;

	protected MatchEdgeFromByStorageNode(IdentNode id, BaseNode type, int context, BaseNode storage,
			PatternGraphNode directlyNestingLHSGraph)
	{
		super(id, type, false, context, TypeExprNode.getEmpty(), directlyNestingLHSGraph);
		this.storageUnresolved = storage;
		becomeParent(this.storageUnresolved);
	}
	
	protected TypeNode getStorageType()
	{
		if(storage != null)
			return storage.getDeclType();
		else if(storageGlobalVariable != null)
			return storageGlobalVariable.getDeclType();
		else
			return storageAttribute.getDecl().getDeclType();
	}
}
