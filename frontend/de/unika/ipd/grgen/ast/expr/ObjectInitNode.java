/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ast.expr;

import java.util.Collection;
import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.expr.ExprNode;
import de.unika.ipd.grgen.ast.model.type.InternalObjectTypeNode;
import de.unika.ipd.grgen.ast.type.TypeNode;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.expr.AttributeInitialization;
import de.unika.ipd.grgen.ir.expr.InternalObjectInit;
import de.unika.ipd.grgen.ir.model.type.InternalObjectType;
import de.unika.ipd.grgen.parser.Coords;

public class ObjectInitNode extends ExprNode
{
	static {
		setName(ObjectInitNode.class, "internal object init");
	}

	private IdentNode objectTypeUnresolved;
	private InternalObjectTypeNode objectType;
	
	CollectNode<AttributeInitializationNode> attributeInits =
			new CollectNode<AttributeInitializationNode>();

	public ObjectInitNode(Coords coords, IdentNode objectType)
	{
		super(coords);
		this.objectTypeUnresolved = objectType;
	}

	public void addAttributeInitialization(AttributeInitializationNode attributeInit)
	{
		this.attributeInits.addChild(attributeInit);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(attributeInits);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("attributeInits");
		return childrenNames;
	}

	private static final DeclarationTypeResolver<InternalObjectTypeNode> objectTypeResolver =
			new DeclarationTypeResolver<InternalObjectTypeNode>(InternalObjectTypeNode.class);

	@Override
	protected boolean resolveLocal()
	{
		objectType = objectTypeResolver.resolve(objectTypeUnresolved, this);
		return objectType != null && objectType.resolve();
	}

	@Override
	protected boolean checkLocal()
	{
		return true;
	}

	@Override
	public TypeNode getType()
	{
		return getObjectType();
	}

	public InternalObjectTypeNode getObjectType()
	{
		assert(isResolved());
		return objectType;
	}

	@Override
	protected IR constructIR()
	{
		InternalObjectType type = objectType.checkIR(InternalObjectType.class);
		
		InternalObjectInit init = new InternalObjectInit(type);
		
		for(AttributeInitializationNode ain : attributeInits.getChildren()) {
			ain.objectInitIR = init;
			init.addAttributeInitialization(ain.checkIR(AttributeInitialization.class));
		}

		return init;
	}

	public InternalObjectInit getObjectInit()
	{
		return checkIR(InternalObjectInit.class);
	}

	public static String getKindStr()
	{
		return "internal object initialization";
	}
}