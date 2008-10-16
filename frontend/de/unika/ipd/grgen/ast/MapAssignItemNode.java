/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.0
 * Copyright (C) 2008 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under GPL v3 (see LICENSE.txt included in the packaging of this file)
 */

/**
 * @author Moritz Kroll, Edgar Jakumeit
 * @version $Id$
 */

package de.unika.ipd.grgen.ast;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.parser.Coords;

public class MapAssignItemNode extends EvalStatementNode
{
	static {
		setName(MapAssignItemNode.class, "map assign item");
	}
	
	QualIdentNode target;
	ExprNode keyExpr;
    ExprNode valueExpr;
	
	public MapAssignItemNode(Coords coords, QualIdentNode target, ExprNode keyExpr,
	                         ExprNode valueExpr)
	{
		super(coords);
		this.target = becomeParent(target);
		this.keyExpr = becomeParent(keyExpr);
		this.valueExpr = becomeParent(valueExpr);
	}

	public Collection<? extends BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(target);
		children.add(keyExpr);
		children.add(valueExpr);
		return children;
	}

	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("target");
		childrenNames.add("keyExpr");
		childrenNames.add("valueExpr");
		return childrenNames;
	}

	protected boolean resolveLocal() {
		return true;
	}

	protected boolean checkLocal() {
		return true;		// MAP TODO
	}
}
