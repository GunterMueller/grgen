/**
 * @author Sebastian Hack
 * @version $Id$
 */
package de.unika.ipd.grgen.util;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.prefs.AbstractPreferences;
import java.util.prefs.BackingStoreException;

/**
 * Own implementation of the Java preferences API, that does not use
 * a "OS backing store" but relies on importing and exporting the
 * preferences via xml files.
 * Also, If a preference is got, but was not in the tree, it is entered.
 */
public class MyPreferences extends AbstractPreferences {

	private Map prefs;
	private Map children;
	
  public MyPreferences(MyPreferences parent, String name) {
    super(parent, name);
		prefs = new HashMap();
		children = new HashMap();
  }

  /**
   * @see java.util.prefs.AbstractPreferences#putSpi(java.lang.String, java.lang.String)
   */
  protected void putSpi(String key, String value) {
    prefs.put(key, value);
  }

  /**
   * @see java.util.prefs.AbstractPreferences#getSpi(java.lang.String)
   */
  protected String getSpi(String key) {
    return (String) prefs.get(key);
  }

  /**
   * @see java.util.prefs.AbstractPreferences#removeSpi(java.lang.String)
   */
  protected void removeSpi(String key) {
  	prefs.remove(key);
  }

  /**
   * @see java.util.prefs.AbstractPreferences#removeNodeSpi()
   */
  protected void removeNodeSpi() throws BackingStoreException {
  	((MyPreferences) parent()).children.remove(name());
  }

  /**
   * @see java.util.prefs.AbstractPreferences#keysSpi()
   */
  protected String[] keysSpi() throws BackingStoreException {
  	String[] res = new String[prefs.size()];
  	int i = 0;
  	for(Iterator it = prefs.keySet().iterator(); it.hasNext(); i++)
  		res[i] = (String) it.next();
  		
    return res;
  }

  /**
   * @see java.util.prefs.AbstractPreferences#childrenNamesSpi()
   */
  protected String[] childrenNamesSpi() throws BackingStoreException {
    String[] res = new String[children.size()];
    int i = 0;
    
    for(Iterator it = children.keySet().iterator(); it.hasNext(); i++) 
    	res[i] = (String) it.next();
    	
    return res;
  }

  /**
   * @see java.util.prefs.AbstractPreferences#childSpi(java.lang.String)
   */
  protected AbstractPreferences childSpi(String child) {
    if(!children.containsKey(child))
    	children.put(child, new MyPreferences(this, child));

		return (AbstractPreferences) children.get(child);	
  }

  /**
   * @see java.util.prefs.AbstractPreferences#syncSpi()
   */
  protected void syncSpi() throws BackingStoreException {
  }

  /**
   * @see java.util.prefs.AbstractPreferences#flushSpi()
   */
  protected void flushSpi() throws BackingStoreException {
  }

  /**
   * @see java.util.prefs.Preferences#get(java.lang.String, java.lang.String)
   */
  public String get(String key, String value) {
    if(!prefs.containsKey(key))
    	prefs.put(key, value);
    	
    return super.get(key, value);
  }
}
