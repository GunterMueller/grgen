/**
 * @author Sebastian Hack
 * @version $Id$
 */
package de.unika.ipd.grgen.ast.util;

import de.unika.ipd.grgen.ast.*;

/**
 * An identifier resover. 
 * It's a framework for a resolver, that finds the AST node, that is
 * declared by an identifier and replaces the ident node by the resolved 
 * node. 
 */
public abstract class IdentResolver extends Resolver {

	/** 
	 * The class of the resolved node must be in the set, otherwise,
	 * an error is reported.
	 */
	private Class[] classes;

	/** A string with names of the classes, which are expected. */
	private String expectList;
	
	/** another auxilliary pretty printing string. */
	private String verb = "is";

	/**
	 * Make a new ident resolver.
	 * @param classes An array of classes of which the resolved ident
	 * must be an instance of.
	 */
	public IdentResolver(Class[] classes) {
		StringBuffer sb = new StringBuffer();
		this.classes = classes;
		
		if(classes.length > 1) {
			sb.append("(");
			verb = "are";
		}
		
		for(int i = 0; i < classes.length; i++) {
			sb.append((i > 0 ? ", " : "") + "\"" + 
				BaseNode.getName(classes[i]) + "\"");
		}
		
		if(classes.length > 1)
			sb.append(")");
			
		expectList = sb.toString();
	}

	/**
	 * @see de.unika.ipd.grgen.ast.util.Resolver#resolve(de.unika.ipd.grgen.ast.BaseNode, int)
	 * The function resolves and IdentNode to another node (probably 
	 * the declaration). If <code>n</code> at position <code>pos</code> 
	 * is not an IdentNode, the method returns true and the node is 
	 * considered as resolved. 
	 */	
	public boolean resolve(BaseNode n, int pos) {
		boolean res = true;

		debug.entering();		
		assert pos < n.children() : "position exceeds children count";

		debug.report(NOTE, "resolving position: " + pos);
		
		BaseNode c = n.getChild(pos);
		debug.report(NOTE, "child is a: " + c.getName() + " (" + c + ")");
		
		// Check, if the desired node is an identifier. 
		// If, not, everything is fine, and the method returns true.
		if(c instanceof IdentNode) {
			BaseNode get = resolveIdent((IdentNode) c);
			
			debug.report(NOTE, "resolved to a: " + get.getName());

			// Check, if the class of the resolved node is in the desired classes.
			// If that's true, replace the desired node with the resolved one.
			for(int i = 0; i < classes.length; i++) 
				if(classes[i].isInstance(get)) {
					n.replaceChild(pos, get);
					debug.report(NOTE, "child is now a: " + n.getChild(pos));
					return true;
				}
			
			res = false;
			reportError(n, "Identifier \"" + c + "\" is a \"" + get 
				+ "\" but " + expectList + " " + verb + " expected");
			
		} 
		
		// else
		//  reportError(n, "Expected an identifier, not a \"" + c.getName() + "\"");
		
		debug.leaving();
		return res;
	}
	
	/**
	 * Get the resolved AST node for an Identtifier.
	 * This can be the declaration, which the identifier occurs in, for
	 * example. See {@link DeclResolver} as an example.
	 * @param n The identifier.
	 * @return The resolved AST node.
	 */
	protected abstract BaseNode resolveIdent(IdentNode n);

}
