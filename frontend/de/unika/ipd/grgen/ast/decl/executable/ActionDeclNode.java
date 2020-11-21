/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author shack, Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast.decl.executable;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.Set;
import java.util.Vector;

import de.unika.ipd.grgen.ast.BaseNode;
import de.unika.ipd.grgen.ast.CollectNode;
import de.unika.ipd.grgen.ast.IdentNode;
import de.unika.ipd.grgen.ast.PackageIdentNode;
import de.unika.ipd.grgen.ast.decl.DeclNode;
import de.unika.ipd.grgen.ast.decl.pattern.ConstraintDeclNode;
import de.unika.ipd.grgen.ast.decl.pattern.EdgeDeclNode;
import de.unika.ipd.grgen.ast.decl.pattern.NodeDeclNode;
import de.unika.ipd.grgen.ast.decl.pattern.VarDeclNode;
import de.unika.ipd.grgen.ast.expr.DeclExprNode;
import de.unika.ipd.grgen.ast.expr.ExprNode;
import de.unika.ipd.grgen.ast.model.type.EdgeTypeNode;
import de.unika.ipd.grgen.ast.model.type.InheritanceTypeNode;
import de.unika.ipd.grgen.ast.model.type.NodeTypeNode;
import de.unika.ipd.grgen.ast.pattern.PatternGraphLhsNode;
import de.unika.ipd.grgen.ast.type.DefinedMatchTypeNode;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.util.CollectResolver;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ir.executable.FilterAutoGenerated;
import de.unika.ipd.grgen.ir.executable.FilterAutoSupplied;
import de.unika.ipd.grgen.ir.executable.MatchingAction;
import de.unika.ipd.grgen.ir.expr.Expression;

/**
 * base class for actions = tests + rules
 */
public abstract class ActionDeclNode extends TopLevelMatcherDeclNode
{
	static {
		setName(ActionDeclNode.class, "action declaration");
	}

	protected CollectNode<BaseNode> returnFormalParametersUnresolved;
	public CollectNode<TypeNode> returnFormalParameters;
	protected ArrayList<FilterAutoDeclNode> filters;
	protected CollectNode<IdentNode> implementedMatchTypesUnresolved;
	protected CollectNode<DefinedMatchTypeNode> implementedMatchTypes;

	protected ActionDeclNode(IdentNode id, TypeNode type, PatternGraphLhsNode pattern,
			CollectNode<IdentNode> implementedMatchTypes, CollectNode<BaseNode> rets)
	{
		super(id, type, pattern);
		this.returnFormalParametersUnresolved = rets;
		becomeParent(this.returnFormalParametersUnresolved);
		implementedMatchTypesUnresolved = implementedMatchTypes;
		becomeParent(implementedMatchTypesUnresolved);
		this.filters = new ArrayList<FilterAutoDeclNode>();
	}

	public void addFilters(ArrayList<FilterAutoDeclNode> filters)
	{
		this.filters.addAll(filters);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(ident);
		children.add(getValidVersion(returnFormalParametersUnresolved, returnFormalParameters));
		children.add(pattern);
		children.add(getValidVersion(implementedMatchTypesUnresolved, implementedMatchTypes));
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("ident");
		childrenNames.add("ret");
		childrenNames.add("pattern");
		childrenNames.add("implementedMatchTypes");
		return childrenNames;
	}

	private static final CollectResolver<DefinedMatchTypeNode> matchTypeResolver =
			new CollectResolver<DefinedMatchTypeNode>(new DeclarationTypeResolver<DefinedMatchTypeNode>(DefinedMatchTypeNode.class));
	private static final CollectResolver<TypeNode> retTypeResolver =
			new CollectResolver<TypeNode>(new DeclarationTypeResolver<TypeNode>(TypeNode.class));

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		for(IdentNode mtid : implementedMatchTypesUnresolved.getChildren()) {
			if(!(mtid instanceof PackageIdentNode)) {
				fixupDefinition(mtid, mtid.getScope());
			}
		}
		implementedMatchTypes = matchTypeResolver.resolve(implementedMatchTypesUnresolved, this);
		returnFormalParameters = retTypeResolver.resolve(returnFormalParametersUnresolved, this);

		return returnFormalParameters != null
				& implementedMatchTypes != null
				& resolveFilters(filters);
	}

	/**
	 * Check if actual return arguments are conformant to the formal return parameters.
	 */
	protected boolean checkReturns(CollectNode<ExprNode> returnArgs)
	{
		boolean res = true;

		int declaredNumRets = returnFormalParameters.size();
		int actualNumRets = returnArgs.size();
		for(int i = 0; i < Math.min(declaredNumRets, actualNumRets); i++) {
			ExprNode retExpr = returnArgs.get(i);
			TypeNode retDeclType = returnFormalParameters.get(i);

			res &= checkReturns(i, retExpr, retDeclType);
		}

		//check the number of returned elements
		if(actualNumRets != declaredNumRets) {
			res = false;
			if(declaredNumRets == 0) {
				returnArgs.reportError("No return values declared for rule \"" + ident + "\"");
			} else if(actualNumRets == 0) {
				reportError("Missing return statement for rule \"" + ident + "\"");
			} else {
				returnArgs.reportError("Return statement has wrong number of parameters");
			}
		}

		return res;
	}

	private boolean checkReturns(int i, ExprNode retExpr, TypeNode retDeclType)
	{
		boolean res = true;

		TypeNode retExprType = retExpr.getType();
		TypeNode retParameterType = returnFormalParameters.get(i);

		if(!retExprType.isCompatibleTo(retDeclType)) {
			res = false;
			String exprTypeName = retExprType.getTypeName();
			String parameterTypeName = retParameterType.getTypeName();
			ident.reportError("Cannot convert " + (i + 1) + ". return parameter from \"" + exprTypeName
					+ "\" to \"" + parameterTypeName + "\"");
			return res;
		}

		if(!(retExpr instanceof DeclExprNode))
			return res;
		ConstraintDeclNode retElem = ((DeclExprNode)retExpr).getConstraintDeclNode();
		if(retElem == null)
			return res;

		InheritanceTypeNode declaredRetType = retElem.getDeclType();

		Set<? extends ConstraintDeclNode> homSet;
		if(retElem instanceof NodeDeclNode)
			homSet = pattern.getHomomorphic((NodeDeclNode)retElem);
		else
			homSet = pattern.getHomomorphic((EdgeDeclNode)retElem);

		for(ConstraintDeclNode homElem : homSet) {
			if(homElem == retElem)
				continue;

			ConstraintDeclNode retypedElem = homElem.getRetypedElement();
			if(retypedElem == null)
				continue;

			InheritanceTypeNode retypedElemType = retypedElem.getDeclType();
			if(retypedElemType.isA(declaredRetType))
				continue;

			res = false;
			retExpr.reportError("Return parameter \"" + retElem.getIdentNode() + "\" is homomorphic to \""
					+ homElem.getIdentNode() + "\", which gets retyped to the incompatible type \""
					+ retypedElemType.getTypeName() + "\"");
			return res;
		}

		return res;
	}

	@Override
	protected boolean checkLocal()
	{
		return checkLeft()
				& checkFilters(pattern, filters)
				& checkMatchTypesImplemented();
	}

	public boolean checkMatchTypesImplemented()
	{
		boolean isOk = true;

		for(DefinedMatchTypeNode matchType : implementedMatchTypes.getChildren()) {
			isOk &= checkMatchTypeImplemented(matchType);
		}

		return isOk;
	}

	public boolean checkMatchTypeImplemented(DefinedMatchTypeNode matchType)
	{
		boolean isOk = true;

		String actionName = getIdentNode().toString();
		String matchTypeName = matchType.getIdentNode().toString();

		HashMap<String, NodeDeclNode> knownNodes = new HashMap<String, NodeDeclNode>();
		for(NodeDeclNode node : pattern.getNodes()) {
			knownNodes.put(node.getIdentNode().toString(), node);
		}
		for(NodeDeclNode node : matchType.getNodes()) {
			isOk = checkNodeImplemented(node, actionName, matchTypeName, knownNodes);
		}

		HashMap<String, EdgeDeclNode> knownEdges = new HashMap<String, EdgeDeclNode>();
		for(EdgeDeclNode edge : pattern.getEdges()) {
			knownEdges.put(edge.getIdentNode().toString(), edge);
		}
		for(EdgeDeclNode edge : matchType.getEdges()) {
			isOk = checkEdgeImplemented(edge, actionName, matchTypeName, knownEdges);
		}

		HashMap<String, VarDeclNode> knownVariables = new HashMap<String, VarDeclNode>();
		for(VarDeclNode var : pattern.getDefVariablesToBeYieldedTo().getChildren()) {
			knownVariables.put(var.getIdentNode().toString(), var);
		}
		for(DeclNode varCand : pattern.getParamDecls()) {
			if(!(varCand instanceof VarDeclNode))
				continue;
			VarDeclNode var = (VarDeclNode)varCand;
			knownVariables.put(var.getIdentNode().toString(), var);
		}
		for(VarDeclNode var : matchType.getVariables()) {
			isOk = checkVariableImplemented(var, actionName, matchTypeName, knownVariables);
		}

		return isOk;
	}

	private boolean checkNodeImplemented(NodeDeclNode node,
			String actionName, String matchTypeName, HashMap<String, NodeDeclNode> knownNodes)
	{
		boolean isOk = true;

		String nodeName = node.getIdentNode().toString();
		if(!knownNodes.containsKey(nodeName)) {
			getIdentNode().reportError("Action " + actionName + " does not implement the node " + nodeName
					+ " expected from " + matchTypeName);
			isOk = false;
		} else {
			NodeDeclNode nodeFromPattern = knownNodes.get(nodeName);
			NodeTypeNode type = node.getDeclType();
			NodeTypeNode typeOfNodeFromPattern = nodeFromPattern.getDeclType();
			if(!type.isEqual(typeOfNodeFromPattern)) {
				getIdentNode().reportError("The type of the node " + nodeName + " from the action " + actionName
						+ " does not equal the type of the node from the match class " + matchTypeName
						+ ". In the match class, " + type.getTypeName() + " is declared, but in the pattern, "
						+ typeOfNodeFromPattern.getTypeName() + " is declared.");
				isOk = false;
			}
			if(nodeFromPattern.defEntityToBeYieldedTo && !node.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The node " + nodeName + " from the action " + actionName
						+ " is a def to be yielded to node, while it is a node to be matched (or received as input to the pattern) in the match class "
						+ matchTypeName);
				isOk = false;
			}
			if(!nodeFromPattern.defEntityToBeYieldedTo && node.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The node " + nodeName + " from the action " + actionName
						+ " is a node to be matched (or received as input to the pattern), while it is a def to be yielded to node in the match class "
						+ matchTypeName);
				isOk = false;
			}
		}

		return isOk;
	}

	private boolean checkEdgeImplemented(EdgeDeclNode edge,
			String actionName, String matchTypeName, HashMap<String, EdgeDeclNode> knownEdges)
	{
		boolean isOk = true;

		String edgeName = edge.getIdentNode().toString();
		if(!knownEdges.containsKey(edgeName)) {
			getIdentNode().reportError("Action " + actionName + " does not implement the edge " + edgeName
					+ " expected from " + matchTypeName);
			isOk = false;
		} else {
			EdgeDeclNode edgeFromPattern = knownEdges.get(edgeName);
			EdgeTypeNode type = edge.getDeclType();
			EdgeTypeNode typeOfEdgeFromPattern = edgeFromPattern.getDeclType();
			if(!type.isEqual(typeOfEdgeFromPattern)) {
				getIdentNode().reportError("The type of the edge " + edgeName + " from the action " + actionName
						+ " does not equal the type of the edge from the match class " + matchTypeName
						+ ". In the match class, " + type.getTypeName() + " is declared, but in the pattern, "
						+ typeOfEdgeFromPattern.getTypeName() + " is declared.");
				isOk = false;
			}
			if(edgeFromPattern.defEntityToBeYieldedTo && !edge.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The edge " + edgeName + " from the action " + actionName
						+ " is a def to be yielded to edge, while it is an edge to be matched (or received as input to the pattern) in the match class "
						+ matchTypeName);
				isOk = false;
			}
			if(!edgeFromPattern.defEntityToBeYieldedTo && edge.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The edge " + edgeName + " from the action " + actionName
						+ " is an edge to be matched (or received as input to the pattern), while it is a def to be yielded to edge in the match class "
						+ matchTypeName);
				isOk = false;
			}
		}

		return isOk;
	}

	private boolean checkVariableImplemented(VarDeclNode var,
			String actionName, String matchTypeName, HashMap<String, VarDeclNode> knownVariables)
	{
		boolean isOk = true;

		String varName = var.getIdentNode().toString();
		if(!knownVariables.containsKey(varName)) {
			getIdentNode().reportError("Action " + actionName + " does not implement the variable " + varName
					+ " expected from " + matchTypeName);
			isOk = false;
		} else {
			VarDeclNode varFromPattern = knownVariables.get(varName);
			TypeNode type = var.getDeclType();
			TypeNode typeOfVarFromPattern = varFromPattern.getDeclType();
			if(!type.isEqual(typeOfVarFromPattern)) {
				getIdentNode().reportError("The type of the variable " + varName + " from the action " + actionName
						+ " does not equal the type of the variable from the match class " + matchTypeName
						+ ". In the match class, " + type.getTypeName() + " is declared, but in the pattern, "
						+ typeOfVarFromPattern.getTypeName() + " is declared.");
				isOk = false;
			}
			if(varFromPattern.defEntityToBeYieldedTo && !var.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The variable " + varName + " from the action " + actionName
						+ " is a def to be yielded to var, while it is a var to be received as input to the pattern in the match class "
						+ matchTypeName);
				isOk = false;
			}
			if(!varFromPattern.defEntityToBeYieldedTo && var.defEntityToBeYieldedTo) {
				getIdentNode().reportError("The variable " + varName + " from the action " + actionName
						+ " is a variable to be received as input to the pattern, while it is a def to be yielded to var in the match class "
						+ matchTypeName);
				isOk = false;
			}
		}

		return isOk;
	}

	public Collection<DefinedMatchTypeNode> getImplementedMatchClasses()
	{
		return implementedMatchTypes.getChildren();
	}

	protected void constructIRaux(MatchingAction constructedMatchingAction, CollectNode<ExprNode> aReturns)
	{
		// add Params to the IR
		addParams(constructedMatchingAction);

		// add Return-Params to the IR
		for(ExprNode aReturnAST : aReturns.getChildren()) {
			ExprNode evaluatedReturn = aReturnAST.evaluate();
			Expression aReturn = evaluatedReturn.checkIR(Expression.class);
			// actual return-parameter
			constructedMatchingAction.addReturn(aReturn);
		}

		// filters add themselves to the rule when their IR is constructed
		for(FilterAutoDeclNode filter : filters) {
			if(filter instanceof FilterAutoSuppliedDeclNode) {
				((FilterAutoSuppliedDeclNode)filter).checkIR(FilterAutoSupplied.class);
			} else { //if(filter instanceof FilterAutoGeneratedNode)
				((FilterAutoGeneratedDeclNode)filter).checkIR(FilterAutoGenerated.class);
			}
		}
	}

	public static String getKindStr()
	{
		return "action";
	}
}
