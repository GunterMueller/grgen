package de.unika.ipd.grgen.ast.typedecl;

import java.util.Vector;

import de.unika.ipd.grgen.ast.*;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ast.util.Resolver;

public abstract class FunctionDeclBaseNode extends DeclNode
{
	protected BaseNode retUnresolved;
	public TypeNode ret;

	public FunctionDeclBaseNode(IdentNode n, BaseNode t)
	{
		super(n, t);
	}

	private static final Resolver<TypeNode> retTypeResolver =
			new DeclarationTypeResolver<TypeNode>(TypeNode.class);

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		ret = retTypeResolver.resolve(retUnresolved, this);
		return ret != null;
	}

	public abstract Vector<TypeNode> getParameterTypes();

	public TypeNode getReturnType()
	{
		assert isResolved();

		return ret;
	}
}