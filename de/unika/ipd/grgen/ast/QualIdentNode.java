/**
 * @author Sebastian Hack
 * @version $Id$
 */
package de.unika.ipd.grgen.ast;

import de.unika.ipd.grgen.parser.Coords;

/**
 * A operator node for the identifier qualification.
 * This node treats expressions like:
 * a.b.c.d 
 */
public class QualIdentNode extends DeclExprNode {

	static {
		setName(QualIdentNode.class, "Qual");
	}

	private DeclNode decl;

  /** Index of the owner node. */
	private static final int OWNER = 0;
	
	/** Index of the member node. */
	private static final int MEMBER = 1;
	
	private static final String[] childrenNames = {
		"owner", "member"
	};
		
  /**
   * Make a new identifier qualify node.
   * @param coords The coordinates.
   */
  public QualIdentNode(Coords coords, BaseNode owner, BaseNode member) {
  	super(coords);
  	setChildrenNames(childrenNames);
  	addChild(owner);
  	addChild(member);
		addResolver(OWNER, identExprResolver);
		addResolver(MEMBER, identExprResolver);		
  }

	/**
	 * This AST node implies an other way of name resolution.
	 * First of all, the left hand side (lhs) has to be resolved. It must be 
	 * a declaration and its type must be an instance of {@link ScopeOwner},
	 * since qualification can only be done, if the lhs owns a scope.
	 * 
	 * Then the right side (rhs) is tought to search the declarations 
	 * of its identifiers in the scope owned by the lhs. This is done
	 * via {@link ExprNode#fixupDeclaration(ScopeOwner)}.
	 * 
	 * Then, the rhs contains the rhs' ident nodes contains the 
	 * right declarations and can be resolved either.  
	 * @see de.unika.ipd.grgen.ast.BaseNode#resolve()
	 */
	protected boolean resolve() {
		boolean res = false;
		ExprNode member = (ExprNode) getChild(MEMBER);
		
		identExprResolver.resolve(this, OWNER);
		BaseNode owner = getChild(OWNER);
		
		res = owner.getResolve();
		
		if(owner instanceof DeclNode) {
			TypeNode ownerType = (TypeNode) ((DeclNode) owner).getDeclType();
		
			if(ownerType instanceof ScopeOwner) {
				member.fixupDeclaration((ScopeOwner) ownerType);
				identExprResolver.resolve(this, MEMBER);
				res = getChild(MEMBER).getResolve();
			} else { 
				reportError("Left hand side of . does not own a scope");
				res = false;
			}
		
		} else {
			reportError("Left hand side of . must be a declaration");
			res = false;
		}			
		
		setResolved(res);
		return res;
	}
	
	
	
	/**
	 * The declaration 
	 * @see de.unika.ipd.grgen.ast.DeclExprNode#resolveDecl()
	 */
	protected DeclNode resolveDecl() {
		assertResolved();
		
		DeclNode member = (DeclNode) getChild(MEMBER);
		return member;
	}

  /**
   * @see de.unika.ipd.grgen.ast.BaseNode#check()
   */
  protected boolean check() {
  	// TODO: Correct implementation here!
    return true;
  }

}
