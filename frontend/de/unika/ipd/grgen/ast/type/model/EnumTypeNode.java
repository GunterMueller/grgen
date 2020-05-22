/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Sebastian Hack
 */

package de.unika.ipd.grgen.ast.type.model;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.decl.model.EnumItemDeclNode;
import de.unika.ipd.grgen.ast.type.CompoundTypeNode;
import de.unika.ipd.grgen.ast.type.OperatorSignature;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.type.basic.BasicTypeNode;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.Ident;
import de.unika.ipd.grgen.ir.model.EnumItem;
import de.unika.ipd.grgen.ir.type.model.EnumType;

/**
 * An enumeration type AST node.
 */
public class EnumTypeNode extends CompoundTypeNode
{
	static {
		setName(EnumTypeNode.class, "enum type");
	}

	private CollectNode<EnumItemDeclNode> elements;

	/*
	 private static final OperatorSignature.Evaluator enumEvaluator =
	 new OperatorSignature.Evaluator() {
	 public ConstNode evaluate(Coords coords, OperatorSignature op,
	 ConstNode[] args) {
	
	 switch(op.getOpId()) {
	 case OperatorSignature.EQ:
	 return new BoolConstNode(coords, args[0].getValue().equals(args[1].getValue()));
	 case OperatorSignature.NE:
	 return new BoolConstNode(coords, !args[0].getValue().equals(args[1].getValue()));
	 }
	 return ConstNode.getInvalid();
	 }
	 };
	 */

	public EnumTypeNode(CollectNode<EnumItemDeclNode> body)
	{
		this.elements = body;
		becomeParent(this.elements);

		//enumerations can be used with the conditional operator
		OperatorSignature.makeOp(OperatorSignature.COND, this,
				new TypeNode[] { BasicTypeNode.booleanType, this, this }, OperatorSignature.condEvaluator);

		//the compatibility of the this enum type
		addCompatibility(this, BasicTypeNode.byteType);
		addCompatibility(this, BasicTypeNode.shortType);
		addCompatibility(this, BasicTypeNode.intType);
		addCompatibility(this, BasicTypeNode.longType);
		addCompatibility(this, BasicTypeNode.floatType);
		addCompatibility(this, BasicTypeNode.doubleType);
		addCompatibility(this, BasicTypeNode.stringType);
	}

	/*
	 protected void doGetCastableToTypes(Collection<TypeNode> coll) {
	 Object obj = BasicTypeNode.castableMap.get(this);
	 if(obj != null)
	 coll.addAll((Collection) obj);
	 }*/

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(elements);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("elements");
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.BaseNode#constructIR() */
	@Override
	protected IR constructIR()
	{
		Ident name = getIdentNode().checkIR(Ident.class);
		EnumType ty = new EnumType(name);

		for(EnumItemDeclNode item : elements.getChildren()) {
			EnumItem it = item.getItem();
			it.getValue().lateInit(ty, it);
			ty.addItem(it);
		}

		return ty;
	}

	@Override
	public String toString()
	{
		return "enum " + getIdentNode().toString();
	}

	public static String getKindStr()
	{
		return "enum type";
	}

	public static String getUseStr()
	{
		return "enum";
	}
}
