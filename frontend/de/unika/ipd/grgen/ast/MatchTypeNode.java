/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.util.DeclarationResolver;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.MatchType;
import de.unika.ipd.grgen.ir.Rule;
import de.unika.ipd.grgen.parser.ParserEnvironment;
import de.unika.ipd.grgen.parser.Symbol;
import de.unika.ipd.grgen.parser.Symbol.Occurrence;

public class MatchTypeNode extends DeclaredTypeNode {
	static {
		setName(MatchTypeNode.class, "match type");
	}

	@Override
	public String getName() {
		return "match<" + actionUnresolved.toString() + "> type";
	}

	public static IdentNode defineMatchType(ParserEnvironment env, IdentNode actionIdent) {
		String actionString = actionIdent.toString();
		String matchTypeString = "match<" + actionString + ">";
		IdentNode matchTypeIdentNode = new IdentNode(env.define(ParserEnvironment.TYPES, matchTypeString, actionIdent.getCoords()));
		MatchTypeNode matchTypeNode = new MatchTypeNode(actionIdent);
		TypeDeclNode typeDeclNode = new TypeDeclNode(matchTypeIdentNode, matchTypeNode); 
		matchTypeIdentNode.setDecl(typeDeclNode);
		return matchTypeIdentNode;
	}
	
	public static IdentNode getMatchTypeIdentNode(ParserEnvironment env, IdentNode actionIdent) {
		Occurrence actionOccurrence = actionIdent.occ;
		Symbol actionSymbol = actionOccurrence.getSymbol();
		String actionString = actionSymbol.getText();
		String matchTypeString = "match<" + actionString + ">";
		if(actionIdent instanceof PackageIdentNode) {
			PackageIdentNode packageActionIdent = (PackageIdentNode)actionIdent;
			Occurrence packageOccurrence = packageActionIdent.owningPackage;
			Symbol packageSymbol = packageOccurrence.getSymbol();
			return new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, packageSymbol.getText(), packageOccurrence.getCoords()), 
					env.occurs(ParserEnvironment.TYPES, matchTypeString, actionOccurrence.getCoords()));
		} else {
			return new IdentNode(env.occurs(ParserEnvironment.TYPES, matchTypeString, actionOccurrence.getCoords()));
		}
	}
	
	private IdentNode actionUnresolved;
	protected TestDeclNode action;

	// the match type node instances are created in ParserEnvironment as needed
	public MatchTypeNode(IdentNode actionIdent) {
		actionUnresolved = becomeParent(actionIdent);
	}

	@Override
	public Collection<BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		// no children
		return children;
	}

	@Override
	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		// no children
		return childrenNames;
	}

	private static final DeclarationResolver<TestDeclNode> actionResolver = new DeclarationResolver<TestDeclNode>(TestDeclNode.class);

	@Override
	protected boolean resolveLocal() {
		if(!(actionUnresolved instanceof PackageIdentNode)) {
			fixupDefinition((IdentNode)actionUnresolved, actionUnresolved.getScope());
		}
		action = actionResolver.resolve(actionUnresolved, this);
		if(action == null) return false;
		return true;
	}
	
	public TestDeclNode getTest() {
		assert(isResolved());
		return action;
	}

	/** Returns the IR object for this match type node. */
	public MatchType getMatchType() {
		return checkIR(MatchType.class);
	}

	@Override
	protected IR constructIR() {
		Rule matchAction = action.getAction();

		// return if the keyType or valueType construction already constructed the IR object
		if (isIRAlreadySet()) {
			return (MatchType)getIR();
		}

		return new MatchType(matchAction);
	}
}
