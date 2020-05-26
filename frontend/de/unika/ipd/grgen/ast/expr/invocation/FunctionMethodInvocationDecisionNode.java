/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit, Moritz Kroll
 */

package de.unika.ipd.grgen.ast.expr.invocation;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.expr.BuiltinFunctionInvocationBaseNode;
import de.unika.ipd.grgen.ast.expr.ExprNode;
import de.unika.ipd.grgen.ast.expr.IdentExprNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayAsDequeNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayAsMapNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayAsSetNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayAsStringNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayAvgNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayDevNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayEmptyNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayExtractNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayIndexOfByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayIndexOfNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayIndexOfOrderedByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayIndexOfOrderedNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayKeepOneForEachByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayKeepOneForEachNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayLastIndexOfByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayLastIndexOfNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayMaxNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayMedNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayMedUnorderedNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayMinNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayOrderAscendingByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayOrderAscendingNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayOrderDescendingByNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayOrderDescendingNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayPeekNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayProdNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayReverseNode;
import de.unika.ipd.grgen.ast.expr.array.ArraySizeNode;
import de.unika.ipd.grgen.ast.expr.array.ArraySubarrayNode;
import de.unika.ipd.grgen.ast.expr.array.ArraySumNode;
import de.unika.ipd.grgen.ast.expr.array.ArrayVarNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeAsArrayNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeAsSetNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeEmptyNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeIndexOfNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeLastIndexOfNode;
import de.unika.ipd.grgen.ast.expr.deque.DequePeekNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeSizeNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeSubdequeNode;
import de.unika.ipd.grgen.ast.expr.map.MapAsArrayNode;
import de.unika.ipd.grgen.ast.expr.map.MapDomainNode;
import de.unika.ipd.grgen.ast.expr.map.MapEmptyNode;
import de.unika.ipd.grgen.ast.expr.map.MapPeekNode;
import de.unika.ipd.grgen.ast.expr.map.MapRangeNode;
import de.unika.ipd.grgen.ast.expr.map.MapSizeNode;
import de.unika.ipd.grgen.ast.expr.set.SetAsArrayNode;
import de.unika.ipd.grgen.ast.expr.set.SetEmptyNode;
import de.unika.ipd.grgen.ast.expr.set.SetPeekNode;
import de.unika.ipd.grgen.ast.expr.set.SetSizeNode;
import de.unika.ipd.grgen.ast.expr.string.StringAsArrayNode;
import de.unika.ipd.grgen.ast.expr.string.StringEndsWithNode;
import de.unika.ipd.grgen.ast.expr.string.StringIndexOfNode;
import de.unika.ipd.grgen.ast.expr.string.StringLastIndexOfNode;
import de.unika.ipd.grgen.ast.expr.string.StringLengthNode;
import de.unika.ipd.grgen.ast.expr.string.StringReplaceNode;
import de.unika.ipd.grgen.ast.expr.string.StringStartsWithNode;
import de.unika.ipd.grgen.ast.expr.string.StringSubstringNode;
import de.unika.ipd.grgen.ast.expr.string.StringToLowerNode;
import de.unika.ipd.grgen.ast.expr.string.StringToUpperNode;
import de.unika.ipd.grgen.ast.model.type.ExternalTypeNode;
import de.unika.ipd.grgen.ast.model.type.InheritanceTypeNode;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.type.basic.BasicTypeNode;
import de.unika.ipd.grgen.ast.type.basic.UntypedExecVarTypeNode;
import de.unika.ipd.grgen.ast.type.container.ArrayTypeNode;
import de.unika.ipd.grgen.ast.type.container.DequeTypeNode;
import de.unika.ipd.grgen.ast.type.container.MapTypeNode;
import de.unika.ipd.grgen.ast.type.container.SetTypeNode;
import de.unika.ipd.grgen.ir.IR;

public class FunctionMethodInvocationDecisionNode extends FunctionInvocationBaseNode
{
	static {
		setName(FunctionMethodInvocationDecisionNode.class, "function method invocation decision expression");
	}

	static TypeNode methodTypeNode = new TypeNode() {
		public Collection<BaseNode> getChildren()
		{
			Vector<BaseNode> children = new Vector<BaseNode>();
			// no children
			return children;
		}

		public Collection<String> getChildrenNames()
		{
			Vector<String> childrenNames = new Vector<String>();
			// no children
			return childrenNames;
		}
	};

	private ExprNode targetExpr;
	private IdentNode methodIdent;
	private IdentNode attributeIdent; // in most cases null, not used
	private FunctionOrBuiltinFunctionInvocationBaseNode result;

	public FunctionMethodInvocationDecisionNode(ExprNode targetExpr, IdentNode methodIdent, CollectNode<ExprNode> arguments,
			IdentNode attributeIdent)
	{
		super(methodIdent.getCoords(), arguments);
		this.targetExpr = becomeParent(targetExpr);
		this.methodIdent = becomeParent(methodIdent);
		this.attributeIdent = becomeParent(attributeIdent);
	}

	@Override
	public Collection<? extends BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(targetExpr);
		//children.add(methodIdent);	// HACK: We don't have a declaration, so avoid failure during check phase
		children.add(arguments);
		if(isResolved())
			children.add(result);
		return children;
	}

	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("targetExpr");
		//childrenNames.add("methodIdent");
		childrenNames.add("params");
		if(isResolved())
			childrenNames.add("result");
		return childrenNames;
	}

	protected boolean resolveLocal()
	{
		if(!targetExpr.resolve())
			return false;

		String methodName = methodIdent.toString();
		TypeNode targetType = targetExpr.getType();

		if(targetType == BasicTypeNode.stringType) {
			result = decideString(methodName);
		} else if(targetType instanceof MapTypeNode) {
			result = decideMap(methodName);
		} else if(targetType instanceof SetTypeNode) {
			result = decideSet(methodName);
		} else if(targetType instanceof ArrayTypeNode) {
			result = decideArray(methodName);
		} else if(targetType instanceof DequeTypeNode) {
			result = decideDeque(methodName);
		} else if(targetType instanceof InheritanceTypeNode && !(targetType instanceof ExternalTypeNode)) {
			if(targetExpr instanceof FunctionMethodInvocationDecisionNode) {
				reportError("method call chains are not supported, assign to a temporary def variable and invoke the method on it");
				return false;
			}
			result = new FunctionMethodInvocationExprNode(((IdentExprNode)targetExpr).getIdent(), methodIdent, arguments);
		} else if(targetType instanceof ExternalTypeNode) {
			result = new ExternalFunctionMethodInvocationExprNode(targetExpr, methodIdent, arguments);
		} else if(targetType instanceof UntypedExecVarTypeNode) {
			result = new UntypedFunctionMethodInvocationExprNode(methodIdent.getCoords(), arguments);
		} else {
			reportError(targetType.toString() + " does not have any function methods");
		}
		
		return result != null;
	}

	private BuiltinFunctionInvocationBaseNode decideString(String methodName)
	{
		switch(methodName) {
		case "length":
			if(arguments.size() != 0) {
				reportError("string.length() does not take any parameters.");
				return null;
			} else
				return new StringLengthNode(getCoords(), targetExpr);
		case "toUpper":
			if(arguments.size() != 0) {
				reportError("string.toUpper() does not take any parameters.");
				return null;
			} else
				return new StringToUpperNode(getCoords(), targetExpr);
		case "toLower":
			if(arguments.size() != 0) {
				reportError("string.toLower() does not take any parameters.");
				return null;
			} else
				return new StringToLowerNode(getCoords(), targetExpr);
		case "substring":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("string.substring(startIndex, length) takes two parameters, or one if the length is omitted.");
				return null;
			} else if(arguments.size() == 2)
				return new StringSubstringNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
			else
				return new StringSubstringNode(getCoords(), targetExpr, arguments.get(0));
		case "indexOf":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("string.indexOf(strToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new StringIndexOfNode(getCoords(), targetExpr, arguments.get(0));
			else
				return new StringIndexOfNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "lastIndexOf":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("string.lastIndexOf(strToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new StringLastIndexOfNode(getCoords(), targetExpr, arguments.get(0));
			else
				return new StringLastIndexOfNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "startsWith":
			if(arguments.size() != 1) {
				reportError("string.startsWith(strToSearchFor) takes one parameter.");
				return null;
			} else
				return new StringStartsWithNode(getCoords(), targetExpr, arguments.get(0));
		case "endsWith":
			if(arguments.size() != 1) {
				reportError("string.endsWith(strToSearchFor) takes one parameter.");
				return null;
			} else
				return new StringEndsWithNode(getCoords(), targetExpr, arguments.get(0));
		case "replace":
			if(arguments.size() != 3) {
				reportError("string.replace(startIndex, length, replaceStr) takes three parameters.");
				return null;
			} else
				return new StringReplaceNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1), arguments.get(2));
		case "asArray":
			if(arguments.size() != 1) {
				reportError("string.asArray(separator) takes one parameter.");
				return null;
			} else
				return new StringAsArrayNode(getCoords(), targetExpr, arguments.get(0));
		default:
			reportError("string does not have a function method named \"" + methodName + "\"");
			return null;
		}
	}

	private BuiltinFunctionInvocationBaseNode decideMap(String methodName)
	{
		switch(methodName) {
		case "size":
			if(arguments.size() != 0) {
				reportError("map<S,T>.size() does not take any parameters.");
				return null;
			} else
				return new MapSizeNode(getCoords(), targetExpr);
		case "empty":
			if(arguments.size() != 0) {
				reportError("map<S,T>.empty() does not take any parameters.");
				return null;
			} else
				return new MapEmptyNode(getCoords(), targetExpr);
		case "domain":
			if(arguments.size() != 0) {
				reportError("map<S,T>.domain() does not take any parameters.");
				return null;
			} else
				return new MapDomainNode(getCoords(), targetExpr);
		case "range":
			if(arguments.size() != 0) {
				reportError("map<S,T>.range() does not take any parameters.");
				return null;
			} else
				return new MapRangeNode(getCoords(), targetExpr);
		case "asArray":
			if(arguments.size() != 0) {
				reportError("map<int,T>.asArray() does not take any parameters.");
				return null;
			} else
				return new MapAsArrayNode(getCoords(), targetExpr);
		case "peek":
			if(arguments.size() != 1) {
				reportError("map<S,T>.peek(number in iteration sequence) takes one parameter.");
				return null;
			} else
				return new MapPeekNode(getCoords(), targetExpr, arguments.get(0));
		default:
			reportError("map<S,T> does not have a function method named \"" + methodName + "\"");
			return null;
		}
	}
	
	private BuiltinFunctionInvocationBaseNode decideSet(String methodName)
	{
		switch(methodName) {
		case "size":
			if(arguments.size() != 0) {
				reportError("set<T>.size() does not take any parameters.");
				return null;
			} else
				return new SetSizeNode(getCoords(), targetExpr);
		case "empty":
			if(arguments.size() != 0) {
				reportError("set<T>.empty() does not take any parameters.");
				return null;
			} else
				return new SetEmptyNode(getCoords(), targetExpr);
		case "peek":
			if(arguments.size() != 1) {
				reportError("set<T>.peek(number in iteration sequence) takes one parameter.");
				return null;
			} else
				return new SetPeekNode(getCoords(), targetExpr, arguments.get(0));
		case "asArray":
			if(arguments.size() != 0) {
				reportError("set<T>.asArray() takes no parameters.");
				return null;
			} else
				return new SetAsArrayNode(getCoords(), targetExpr);
		default:
			reportError("set<T> does not have a function method named \"" + methodName + "\"");
			return null;
		}
	}

	private BuiltinFunctionInvocationBaseNode decideArray(String methodName)
	{
		switch(methodName) {
		case "size":
			if(arguments.size() != 0) {
				reportError("array<T>.size() does not take any parameters.");
				return null;
			} else
				return new ArraySizeNode(getCoords(), targetExpr);
		case "empty":
			if(arguments.size() != 0) {
				reportError("array<T>.empty() does not take any parameters.");
				return null;
			} else
				return new ArrayEmptyNode(getCoords(), targetExpr);
		case "peek":
			if(arguments.size() != 0 && arguments.size() != 1) {
				reportError("array<T>.peek(index) takes one parameter; or none parameter returning the value from the end.");
				return null;
			} else {
				if(arguments.size() == 0)
					return new ArrayPeekNode(getCoords(), targetExpr);
				else
					return new ArrayPeekNode(getCoords(), targetExpr, arguments.get(0));
			}
		case "indexOf":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("array<T>.indexOf(valueToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new ArrayIndexOfNode(getCoords(), targetExpr, arguments.get(0));
			else
				return new ArrayIndexOfNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "indexOfBy":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("array<T>.indexOfBy<attribute>(valueToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new ArrayIndexOfByNode(getCoords(), targetExpr, attributeIdent, arguments.get(0));
			else
				return new ArrayIndexOfByNode(getCoords(), targetExpr, attributeIdent, arguments.get(0), arguments.get(1));
		case "indexOfOrdered":
			if(arguments.size() != 1) {
				reportError("array<T>.indexOfOrdered(valueToSearchFor) takes one parameter.");
				return null;
			} else
				return new ArrayIndexOfOrderedNode(getCoords(), targetExpr, arguments.get(0));
		case "indexOfOrderedBy":
			if(arguments.size() != 1) {
				reportError("array<T>.indexOfOrderedBy<attribute>(valueToSearchFor) takes one parameter.");
				return null;
			} else
				return new ArrayIndexOfOrderedByNode(getCoords(), targetExpr, attributeIdent, arguments.get(0));
		case "lastIndexOf":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("array<T>.lastIndexOf(valueToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new ArrayLastIndexOfNode(getCoords(), targetExpr, arguments.get(0));
			else
				return new ArrayLastIndexOfNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "lastIndexOfBy":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("array<T>.lastIndexOfBy<attribute>(valueToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new ArrayLastIndexOfByNode(getCoords(), targetExpr, attributeIdent, arguments.get(0));
			else
				return new ArrayLastIndexOfByNode(getCoords(), targetExpr, attributeIdent, arguments.get(0), arguments.get(1));
		case "subarray":
			if(arguments.size() != 2) {
				reportError("array<T>.subarray(startIndex, length) takes two parameters.");
				return null;
			} else
				return new ArraySubarrayNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "orderAscending":
			if(arguments.size() != 0) {
				reportError("array<T>.orderAscending() takes no parameters.");
				return null;
			} else
				return new ArrayOrderAscendingNode(getCoords(), targetExpr);
		case "orderDescending":
			if(arguments.size() != 0) {
				reportError("array<T>.orderDescending() takes no parameters.");
				return null;
			} else
				return new ArrayOrderDescendingNode(getCoords(), targetExpr);
		case "keepOneForEach":
			if(attributeIdent == null) {
				if(arguments.size() != 0) {
					reportError("array<T>.keepOneForEach() takes no parameters.");
					return null;
				} else
					return new ArrayKeepOneForEachNode(getCoords(), targetExpr);
			} else {
				if(arguments.size() != 0) {
					reportError("array<T>.keepOneForEach<attribute>() takes no parameters.");
					return null;
				} else
					return new ArrayKeepOneForEachByNode(getCoords(), targetExpr, attributeIdent);
			}
		case "orderAscendingBy":
			if(arguments.size() != 0) {
				reportError("array<T>.orderAscendingBy<attribute>() takes no parameters.");
				return null;
			} else
				return new ArrayOrderAscendingByNode(getCoords(), targetExpr, attributeIdent);
		case "orderDescendingBy":
			if(arguments.size() != 0) {
				reportError("array<T>.orderDescendingBy<attribute>() takes no parameters.");
				return null;
			} else
				return new ArrayOrderDescendingByNode(getCoords(), targetExpr, attributeIdent);
		case "reverse":
			if(arguments.size() != 0) {
				reportError("array<T>.reverse() takes no parameters.");
				return null;
			} else
				return new ArrayReverseNode(getCoords(), targetExpr);
		case "extract":
			if(arguments.size() != 0) {
				reportError("array<T>.extract<attribute>() takes no parameters.");
				return null;
			} else
				return new ArrayExtractNode(getCoords(), targetExpr, attributeIdent);
		case "asSet":
			if(arguments.size() != 0) {
				reportError("array<T>.asSet() takes no parameters.");
				return null;
			} else
				return new ArrayAsSetNode(getCoords(), targetExpr);
		case "asDeque":
			if(arguments.size() != 0) {
				reportError("array<T>.asDeque() takes no parameters.");
				return null;
			} else
				return new ArrayAsDequeNode(getCoords(), targetExpr);
		case "asMap":
			if(arguments.size() != 0) {
				reportError("array<T>.asMap() takes no parameters.");
				return null;
			} else
				return new ArrayAsMapNode(getCoords(), targetExpr);
		case "asString":
			if(arguments.size() != 1) {
				reportError("array<string>.asString(separator) takes one parameter.");
				return null;
			} else
				return new ArrayAsStringNode(getCoords(), targetExpr, arguments.get(0));
		case "sum":
			if(arguments.size() != 0) {
				reportError("array<T>.sum() takes no parameters.");
				return null;
			} else {
				return new ArraySumNode(getCoords(), targetExpr);
			}
		case "prod":
			if(arguments.size() != 0) {
				reportError("array<T>.prod() takes no parameters.");
				return null;
			} else {
				return new ArrayProdNode(getCoords(), targetExpr);
			}
		case "min":
			if(arguments.size() != 0) {
				reportError("array<T>.min() takes no parameters.");
				return null;
			} else {
				return new ArrayMinNode(getCoords(), targetExpr);
			}
		case "max":
			if(arguments.size() != 0) {
				reportError("array<T>.max() takes no parameters.");
				return null;
			} else {
				return new ArrayMaxNode(getCoords(), targetExpr);
			}
		case "avg":
			if(arguments.size() != 0) {
				reportError("array<T>.avg() takes no parameters.");
				return null;
			} else {
				return new ArrayAvgNode(getCoords(), targetExpr);
			}
		case "med":
			if(arguments.size() != 0) {
				reportError("array<T>.med() takes no parameters.");
				return null;
			} else {
				return new ArrayMedNode(getCoords(), targetExpr);
			}
		case "medUnordered":
			if(arguments.size() != 0) {
				reportError("array<T>.medUnordered() takes no parameters.");
				return null;
			} else {
				return new ArrayMedUnorderedNode(getCoords(), targetExpr);
			}
		case "var":
			if(arguments.size() != 0) {
				reportError("array<T>.var() takes no parameters.");
				return null;
			} else {
				return new ArrayVarNode(getCoords(), targetExpr);
			}
		case "dev":
			if(arguments.size() != 0) {
				reportError("array<T>.dev() takes no parameters.");
				return null;
			} else {
				return new ArrayDevNode(getCoords(), targetExpr);
			}
		default:
			reportError("array<T> does not have a function method named \"" + methodName + "\"");
			return null;
		}
	}
	
	private BuiltinFunctionInvocationBaseNode decideDeque(String methodName)
	{
		switch(methodName) {
		case "size":
			if(arguments.size() != 0) {
				reportError("deque<T>.size() does not take any parameters.");
				return null;
			} else
				return new DequeSizeNode(getCoords(), targetExpr);
		case "empty":
			if(arguments.size() != 0) {
				reportError("deque<T>.empty() does not take any parameters.");
				return null;
			} else
				return new DequeEmptyNode(getCoords(), targetExpr);
		case "peek":
			if(arguments.size() != 0 && arguments.size() != 1) {
				reportError("deque<T>.peek(index) takes one parameter; or none parameter returning the value from the begin.");
				return null;
			} else {
				if(arguments.size() == 0)
					return new DequePeekNode(getCoords(), targetExpr);
				else
					return new DequePeekNode(getCoords(), targetExpr, arguments.get(0));
			}
		case "indexOf":
			if(arguments.size() != 1 && arguments.size() != 2) {
				reportError("deque<T>.indexOf(valueToSearchFor) takes one parameter, or a second startIndex parameter.");
				return null;
			} else if(arguments.size() == 1)
				return new DequeIndexOfNode(getCoords(), targetExpr, arguments.get(0));
			else
				return new DequeIndexOfNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "lastIndexOf":
			if(arguments.size() != 1) {
				reportError("deque<T>.lastIndexOf(valueToSearchFor) takes one parameter.");
				return null;
			} else
				return new DequeLastIndexOfNode(getCoords(), targetExpr, arguments.get(0));
		case "subdeque":
			if(arguments.size() != 2) {
				reportError("deque<T>.subdeque(startIndex, length) takes two parameters.");
				return null;
			} else
				return new DequeSubdequeNode(getCoords(), targetExpr, arguments.get(0), arguments.get(1));
		case "asSet":
			if(arguments.size() != 0) {
				reportError("deque<T>.asSet() takes no parameters.");
				return null;
			} else
				return new DequeAsSetNode(getCoords(), targetExpr);
		case "asArray":
			if(arguments.size() != 0) {
				reportError("deque<T>.asArray() takes no parameters.");
				return null;
			} else
				return new DequeAsArrayNode(getCoords(), targetExpr);
		default:
			reportError("deque<T> does not have a function method named \"" + methodName + "\"");
			return null;
		}
	}

	@Override
	protected boolean checkLocal()
	{
		return true;
	}

	@Override
	public TypeNode getType()
	{
		return result.getType();
	}

	protected ExprNode getResult()
	{
		return result;
	}

	@Override
	protected IR constructIR()
	{
		return result.getIR();
	}
}