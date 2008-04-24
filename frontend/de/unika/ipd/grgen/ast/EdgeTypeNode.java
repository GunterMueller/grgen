/*
 GrGen: graph rewrite generator tool.
 Copyright (C) 2005  IPD Goos, Universit"at Karlsruhe, Germany

 This library is free software; you can redistribute it and/or
 modify it under the terms of the GNU Lesser General Public
 License as published by the Free Software Foundation; either
 version 2.1 of the License, or (at your option) any later version.

 This library is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public
 License along with this library; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */


/**
 * @author Sebastian Buchwald
 * @version $Id$
 */
package de.unika.ipd.grgen.ast;

import java.util.Collection;
import java.util.Map;
import java.util.Vector;

import de.unika.ipd.grgen.ast.util.CollectPairResolver;
import de.unika.ipd.grgen.ast.util.CollectResolver;
import de.unika.ipd.grgen.ast.util.DeclarationPairResolver;
import de.unika.ipd.grgen.ast.util.DeclarationTypeResolver;
import de.unika.ipd.grgen.ir.ConnAssert;
import de.unika.ipd.grgen.ir.EdgeType;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.MemberInit;

public abstract class EdgeTypeNode extends InheritanceTypeNode {
	static {
		setName(EdgeTypeNode.class, "edge type");
	}

	protected static final CollectPairResolver<BaseNode> bodyResolver = new CollectPairResolver<BaseNode>(
    		new DeclarationPairResolver<MemberDeclNode, MemberInitNode>(MemberDeclNode.class, MemberInitNode.class));

	protected static final CollectResolver<EdgeTypeNode> extendResolver = new CollectResolver<EdgeTypeNode>(
    		new DeclarationTypeResolver<EdgeTypeNode>(EdgeTypeNode.class));

	protected CollectNode<BaseNode> body;
	protected CollectNode<ConnAssertNode> cas;
	protected CollectNode<EdgeTypeNode> extend;

	/**
	 * Make a new edge type node.
	 * @param ext The collect node with all edge classes that this one extends.
	 * @param cas The collect node with all connection assertion of this type.
	 * @param body The body of the type declaration. It consists of basic
	 * declarations.
	 * @param modifiers The modifiers for this type.
	 * @param externalName The name of the external implementation of this type or null.
	 */
	public EdgeTypeNode(CollectNode<IdentNode> ext, CollectNode<ConnAssertNode> cas, CollectNode<BaseNode> body,
						int modifiers, String externalName) {
		this.extendUnresolved = ext;
		becomeParent(this.extendUnresolved);
		this.bodyUnresolved = body;
		becomeParent(this.bodyUnresolved);
		this.cas = cas;
		becomeParent(this.cas);
		setModifiers(modifiers);
		setExternalName(externalName);
	}

	/** returns children of this node */
	public Collection<BaseNode> getChildren() {
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(getValidVersion(extendUnresolved, extend));
		children.add(getValidVersion(bodyUnresolved, body));
		children.add(cas);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	public Collection<String> getChildrenNames() {
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("extends");
		childrenNames.add("body");
		childrenNames.add("cas");
		return childrenNames;
	}

	/** @see de.unika.ipd.grgen.ast.ScopeOwner#fixupDefinition(de.unika.ipd.grgen.ast.IdentNode) */
    public boolean fixupDefinition(IdentNode id) {
		assert isResolved();

		boolean found = super.fixupDefinition(id, false);

		if(!found) {
			for(InheritanceTypeNode inh : extend.getChildren()) {
				boolean result = inh.fixupDefinition(id);

				if(found && result) {
					error.error(getIdentNode().getCoords(), "Identifier " + id + " is ambiguous");
				}

				found = found || result;
			}
		}

		return found;
    }

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	protected boolean resolveLocal() {
		body = bodyResolver.resolve(bodyUnresolved, this);
		extend = extendResolver.resolve(extendUnresolved, this);

		return body != null && extend != null;
	}

	/**
	 * Get the edge type IR object.
	 * @return The edge type IR object for this AST node.
	 */
	public EdgeType getEdgeType() {
		return checkIR(EdgeType.class);
	}

	protected void doGetCompatibleToTypes(Collection<TypeNode> coll) {
		assert isResolved();

		for(EdgeTypeNode inh : extend.getChildren()) {
			coll.add(inh);
			coll.addAll(inh.getCompatibleToTypes());
		}
    }

	public Collection<EdgeTypeNode> getDirectSuperTypes() {
		assert isResolved();

	    return extend.getChildren();
    }

	@Override
    protected void getMembers(Map<String, DeclNode> members)
    {
    	assert isResolved();

    	for(BaseNode n : body.getChildren()) {
    		if(n instanceof DeclNode) {
    			DeclNode decl = (DeclNode)n;

    			DeclNode old=members.put(decl.getIdentNode().toString(), decl);
    			if(old!=null && !(old instanceof AbstractMemberDeclNode)) {
    				// TODO this should be part of a check (that return false)
    				error.error(decl.getCoords(), "member " + decl.toString() +" of " +
    								getUseString() + " " + getIdentNode() +
    								" already defined in " + old.getParents() + "." // TODO improve error message
    						   );
    			}
    		}
    	}
    }

	protected abstract void constructIR(EdgeType inhType);

	/**
     * @see de.unika.ipd.grgen.ast.BaseNode#constructIR()
     */
    protected IR constructIR()
    {
    	EdgeType et = new EdgeType(getDecl().getIdentNode().getIdent(),
    							   getIRModifiers(), getExternalName());

		for(BaseNode n : body.getChildren()) {
			if(n instanceof DeclNode) {
				DeclNode decl = (DeclNode)n;
				et.addMember(decl.getEntity());
			}
			else if(n instanceof MemberInitNode) {
				MemberInitNode mi = (MemberInitNode)n;
				et.addMemberInit(mi.checkIR(MemberInit.class));
			}
		}
		for(InheritanceTypeNode inh : extend.getChildren()) {
			et.addDirectSuperType(inh.getType());
		}

		// to check overwriting of attributes
		et.getAllMembers();

    	constructIR(et);

    	for(BaseNode n : cas.getChildren()) {
    		ConnAssertNode can = (ConnAssertNode)n;
    		et.addConnAssert(can.checkIR(ConnAssert.class));
    	}

    	return et;
    }
}
