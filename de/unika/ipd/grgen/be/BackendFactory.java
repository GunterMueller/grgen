/**
 * @author shack
 * @version $Id$
 */
package de.unika.ipd.grgen.be;

/**
 * An interface for something that creates a backend.
 */
public interface BackendFactory {
	
	/**
	 * Create a new backend.
	 * @return A new backend.
	 */
	Backend getBackend() throws BackendException;
	
}
