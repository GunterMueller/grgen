/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2019 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

package de.unika.ipd.grgen.ast.exprevals;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.exprevals.ImportExpr;
import de.unika.ipd.grgen.parser.Coords;

/**
 * A node yielding a graph imported from a file.
 */
public class ImportExprNode extends ExprNode {
	static {
		setName(ImportExprNode.class, "import expr");
	}

	private ExprNode pathExpr;
		
	public ImportExprNode(Coords coords, ExprNode pathExpr) {
		super(coords);
		this.pathExpr = pathExpr;
		becomeParent(this.pathExpr);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(pathExpr);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("pathExpr");
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal() {
		return true;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal() {
		if(!(pathExpr.getType() instanceof StringTypeNode)) {
			pathExpr.reportError("string (with file path) expected as argument to import");
			return false;
		}
		return true;
	}

	@Override
	protected IR constructIR() {
		return new ImportExpr(pathExpr.checkIR(Expression.class), 
								getType().getType());
	}

	@Override
	public TypeNode getType() {
		return BasicTypeNode.graphType;
	}
}
