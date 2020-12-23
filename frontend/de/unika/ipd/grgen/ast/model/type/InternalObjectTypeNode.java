/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */
package de.unika.ipd.grgen.ast.model.type;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.BaseNode;
import de.unika.ipd.grgen.ast.CollectNode;
import de.unika.ipd.grgen.ast.IdentNode;
import de.unika.ipd.grgen.ast.decl.ConstructorDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.FunctionDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.OperatorDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.OperatorEvaluator;
import de.unika.ipd.grgen.ast.decl.executable.ProcedureDeclNode;
import de.unika.ipd.grgen.ast.decl.executable.OperatorDeclNode.Operator;
import de.unika.ipd.grgen.ast.expr.array.ArrayInitNode;
import de.unika.ipd.grgen.ast.expr.deque.DequeInitNode;
import de.unika.ipd.grgen.ast.expr.map.MapInitNode;
import de.unika.ipd.grgen.ast.expr.set.SetInitNode;
import de.unika.ipd.grgen.ast.model.MemberInitNode;
import de.unika.ipd.grgen.ast.model.decl.MemberDeclNode;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.type.basic.BasicTypeNode;
import de.unika.ipd.grgen.ast.util.CollectResolver;
import de.unika.ipd.grgen.ast.util.DeclarationResolver;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.model.type.InternalObjectType;

/**
 * A class representing an (internal non-node/edge) object type (i.e. a class)
 */
public class InternalObjectTypeNode extends InheritanceTypeNode
{
	static {
		setName(InternalObjectTypeNode.class, "internal object type");
	}

	public static InternalObjectTypeNode internalObjectType;

	private CollectNode<InternalObjectTypeNode> extend;

	/**
	 * Create a new (internal) object type (i.e. class)
	 * @param ext The collect node containing the object types which are extended by this type.
	 * @param body the collect node with body declarations
	 * @param modifiers Type modifiers for this type.
	 */
	public InternalObjectTypeNode(CollectNode<IdentNode> ext, CollectNode<BaseNode> body, int modifiers)
	{
		this.extendUnresolved = ext;
		becomeParent(this.extendUnresolved);
		this.bodyUnresolved = body;
		becomeParent(this.bodyUnresolved);
		setModifiers(modifiers);
		setExternalName(null);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(getValidVersion(extendUnresolved, extend));
		children.add(getValidVersion(bodyUnresolved, body));
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("extends");
		childrenNames.add("body");
		return childrenNames;
	}

	private static final CollectResolver<InternalObjectTypeNode> extendResolver =
			new CollectResolver<InternalObjectTypeNode>(new DeclarationTypeResolver<InternalObjectTypeNode>(InternalObjectTypeNode.class));

	private static final CollectResolver<BaseNode> bodyResolver = new CollectResolver<BaseNode>(
			new DeclarationResolver<BaseNode>(MemberDeclNode.class, MemberInitNode.class, ConstructorDeclNode.class,
					MapInitNode.class, SetInitNode.class, ArrayInitNode.class, DequeInitNode.class,
					FunctionDeclNode.class, ProcedureDeclNode.class));

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		OperatorDeclNode.makeOp(Operator.COND, this, new TypeNode[] { BasicTypeNode.booleanType, this, this }, OperatorEvaluator.condEvaluator);
		OperatorDeclNode.makeBinOp(Operator.EQ, BasicTypeNode.booleanType, this, this, OperatorEvaluator.emptyEvaluator);
		OperatorDeclNode.makeBinOp(Operator.NE, BasicTypeNode.booleanType, this, this, OperatorEvaluator.emptyEvaluator);

		body = bodyResolver.resolve(bodyUnresolved, this);
		extend = extendResolver.resolve(extendUnresolved, this);

		// Initialize direct sub types
		if(extend != null) {
			for(InheritanceTypeNode type : extend.getChildren()) {
				type.addDirectSubType(this);
			}
		}

		return body != null && extend != null;
	}

	/**
	 * Get the IR internal type for this AST node.
	 * @return The correctly casted IR internal type.
	 */
	public InternalObjectType getInternalObjectType()
	{
		return checkIR(InternalObjectType.class);
	}

	/**
	 * Construct IR object for this AST node.
	 * @see de.unika.ipd.grgen.ast.BaseNode#constructIR()
	 */
	@Override
	protected IR constructIR()
	{
		if(isIRAlreadySet()) { // break endless recursion in case of a member of class or container of class type
			return getIR();
		}

		InternalObjectType ot = new InternalObjectType(getDecl().getIdentNode().getIdent(), getIRModifiers());

		setIR(ot);

		constructIR(ot);

		return ot;
	}

	@Override
	protected CollectNode<? extends InheritanceTypeNode> getExtends()
	{
		return extend;
	}

	@Override
	public void doGetCompatibleToTypes(Collection<TypeNode> coll)
	{
		assert isResolved();

		for(InternalObjectTypeNode inh : extend.getChildren()) {
			coll.add(inh);
			coll.addAll(inh.getCompatibleToTypes());
		}
		
		coll.add(BasicTypeNode.typeType); // ~~ addCompatibility(this, BasicTypeNode.typeType);
	}

	public static String getKindStr()
	{
		return "object of internal class";
	}

	@Override
	public Collection<InternalObjectTypeNode> getDirectSuperTypes()
	{
		assert isResolved();

		return extend.getChildren();
	}
}
