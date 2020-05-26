/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Sebastian Hack
 */

package de.unika.ipd.grgen.ast.pattern;

import java.util.Collection;
import java.util.Set;
import java.util.Vector;

import de.unika.ipd.grgen.ast.BaseNode;
import de.unika.ipd.grgen.ast.decl.pattern.EdgeDeclNode;
import de.unika.ipd.grgen.ast.decl.pattern.NodeDeclNode;
import de.unika.ipd.grgen.ast.model.type.NodeTypeNode;
import de.unika.ipd.grgen.ast.util.Checker;
import de.unika.ipd.grgen.ast.util.DeclarationResolver;
import de.unika.ipd.grgen.ast.util.TypeChecker;
import de.unika.ipd.grgen.ir.pattern.Graph;

/**
 * AST node representing nodes
 * that occur without any edge connection to the rest of the graph.
 * children: NODE:NodeDeclNode|IdentNode
 */
public class SingleNodeConnNode extends ConnectionCharacter
{
	static {
		setName(SingleNodeConnNode.class, "single node");
	}

	private NodeDeclNode node;
	public BaseNode nodeUnresolved;

	public SingleNodeConnNode(BaseNode node)
	{
		super(node.getCoords());
		this.nodeUnresolved = node;
		becomeParent(this.nodeUnresolved);
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(getValidVersion(nodeUnresolved, node));
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("node");
		return childrenNames;
	}

	private static final DeclarationResolver<NodeDeclNode> nodeResolver =
			new DeclarationResolver<NodeDeclNode>(NodeDeclNode.class); // optional

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		boolean res = fixupDefinition(nodeUnresolved, nodeUnresolved.getScope());
		if(!res)
			return false;

		node = nodeResolver.resolve(nodeUnresolved, this);
		return node != null;
	}

	/** Get the node child of this node.
	 * @return The node child. */
	public NodeDeclNode getNode()
	{
		assert isResolved();

		return node;
	}

	/** @see de.unika.ipd.grgen.ast.pattern.ConnectionCharacter#addToGraph(de.unika.ipd.grgen.ir.pattern.Graph) */
	public void addToGraph(Graph gr)
	{
		assert isResolved();

		gr.addSingleNode(node.getNode());
	}

	private static Checker nodeChecker = new TypeChecker(NodeTypeNode.class);

	/** @see de.unika.ipd.grgen.ast.BaseNode#checkLocal() */
	@Override
	protected boolean checkLocal()
	{
		return nodeChecker.check(node, error);
	}

	/** @see de.unika.ipd.grgen.ast.pattern.ConnectionCharacter#addEdge(java.util.Set) */
	public void addEdge(Set<EdgeDeclNode> set)
	{
	}

	public EdgeCharacter getEdge()
	{
		return null;
	}

	public NodeCharacter getSrc()
	{
		assert isResolved();

		return node;
	}

	public void setSrc(NodeDeclNode src)
	{
	}

	public NodeCharacter getTgt()
	{
		return null;
	}

	public void setTgt(NodeDeclNode tgt)
	{
	}

	/** @see de.unika.ipd.grgen.ast.pattern.ConnectionCharacter#addNodes(java.util.Set) */
	public void addNodes(Set<NodeDeclNode> set)
	{
		assert isResolved();

		set.add(node);
	}

	public static String getKindStr()
	{
		return "single node connection";
	}

	public static String getUseStr()
	{
		return "SingleNodeConnNode";
	}
}