/**
 * @author shack
 * @version $Id$
 */
package de.unika.ipd.grgen.ir;

import de.unika.ipd.grgen.util.MultiIterator;
import java.util.Collections;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

/**
 * An class representing a node or an edge.
 */
public abstract class CompoundType extends Type {

	/**
	 * Collection containing all members.
	 * The members must be of type Entity.
	 */
	private List members = new LinkedList();

  /**
   * Make a new compound type.
   * @param name The name of the type.
   * @param ident The identifier used to declare this type.
   */
  public CompoundType(String name, Ident ident) {
    super(name, ident);
  }
	
  /**
   * Get all members of this compund type.
   * @return An iterator with all members.
   */
  public Iterator getMembers() {
  	return members.iterator();
  }

	/**
	 * Add a member to the compound type.
	 * @param member The entity to add.
	 */
	public void addMember(Entity member) {
		members.add(member);
		member.setOwner(this);
	}
	
	protected void canonicalizeLocal() {
		Collections.sort(members, Identifiable.COMPARATOR);
	}
  
  /**
   * @see de.unika.ipd.grgen.util.Walkable#getWalkableChildren()
   */
  public Iterator getWalkableChildren() {
  	return new MultiIterator(new Iterator[] {
  		super.getWalkableChildren(),
  		members.iterator()
  	});
  }
	
	public void addFields(Map fields) {
		super.addFields(fields);
		fields.put("members", members.iterator());
	}
	
	void addToDigest(StringBuffer sb) {
		sb.append(this);
		sb.append('[');
		
		int i = 0;
		for(Iterator it = members.iterator(); it.hasNext(); i++) {
			Entity ent = (Entity) it.next();
			if(i > 0)
				sb.append(',');
			sb.append(ent);
		}
		
		sb.append(']');
	}
	
}
