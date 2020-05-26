/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast.decl.pattern;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.IdentNode;
import de.unika.ipd.grgen.ast.decl.DeclNode;
import de.unika.ipd.grgen.ast.type.AlternativeTypeNode;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.executable.Rule;
import de.unika.ipd.grgen.ir.pattern.Alternative;

/**
 * AST node that represents an alternative, containing the alternative graph patterns
 */
public class AlternativeNode extends DeclNode
{
	static {
		setName(AlternativeNode.class, "alternative");
	}

	/** Type for this declaration. */
	private static AlternativeTypeNode alternativeType = new AlternativeTypeNode();

	private Vector<AlternativeCaseNode> children = new Vector<AlternativeCaseNode>();

	public AlternativeNode(IdentNode id)
	{
		super(id, alternativeType);
	}

	public void addChild(AlternativeCaseNode n)
	{
		assert(!isResolved());
		becomeParent(n);
		children.add(n);
	}

	/** returns children of this node */
	@Override
	public Collection<AlternativeCaseNode> getChildren()
	{
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	protected Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		// nameless children
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		return true;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal()
	{
		if(children.isEmpty()) {
			this.reportError("alternative is empty");
			return false;
		}

		return true;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#constructIR() */
	@Override
	protected IR constructIR()
	{
		Alternative alternative = new Alternative(ident.getIdent());
		for(AlternativeCaseNode alternativeCaseNode : children) {
			Rule alternativeCaseRule = alternativeCaseNode.checkIR(Rule.class);
			alternative.addAlternativeCase(alternativeCaseRule);
		}
		return alternative;
	}

	@Override
	public AlternativeTypeNode getDeclType()
	{
		assert isResolved();

		return alternativeType;
	}
}