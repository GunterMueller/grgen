/**
 * @author shack
 * @version $Id$
 */
package de.unika.ipd.grgen.ir;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

import de.unika.ipd.grgen.util.GraphDumpVisitor;
import de.unika.ipd.grgen.util.GraphDumpable;
import de.unika.ipd.grgen.util.GraphDumpableProxy;
import de.unika.ipd.grgen.util.GraphDumper;
import de.unika.ipd.grgen.util.Walkable;

/**
 * A IR pretty graph dumper. 
 */
public class DumpVisitor extends GraphDumpVisitor {

	private class PrefixNode extends GraphDumpableProxy {
		private String prefix;
		
		public PrefixNode(GraphDumpable gd, String prefix) {
			super(gd);
			this.prefix = prefix;
		}
		
    /**
     * @see de.unika.ipd.grgen.util.GraphDumpable#getNodeId()
     */
    public String getNodeId() {
      return prefix + getGraphDumpable().getNodeId();
    }
    
    public String toString() {
    	return getNodeId();
    }
	}

	private void dumpGraph(Graph gr, String prefix) {
		Map prefixMap = new HashMap();
		Set nodes = gr.getNodes();
		
		debug.entering();
		dumper.beginSubgraph(gr);
		
		for(Iterator it = nodes.iterator(); it.hasNext();) {
			Node n = (Node) it.next();
			debug.report(NOTE, "node: " + n);
			PrefixNode pn = new PrefixNode(n, prefix);		
			prefixMap.put(n, pn);
			dumper.node(pn);
			
		}
		
		for(Iterator it = gr.getEdges().iterator(); it.hasNext();) {
			Edge edge = (Edge) it.next();
			PrefixNode from, to, e;
			
			e = new PrefixNode(edge, prefix);
			
			debug.report(NOTE, "true edge from: " + gr.getSource(edge)
			  + " to: " + gr.getTarget(edge));
			  
			from = (PrefixNode) prefixMap.get(gr.getSource(edge));
			to = (PrefixNode) prefixMap.get(gr.getTarget(edge));
			
			debug.report(NOTE, "edge from: " + from + " to: " + to);
			
			dumper.node(e);
			dumper.edge(from, e);
			dumper.edge(e, to);
		}

		Set homSet = new HashSet();
		Set processedNodes = new HashSet();
		
		for(Iterator it = nodes.iterator(); it.hasNext(); ) {
			Node n = (Node) it.next();
			homSet.clear();
			n.getHomomorphic(homSet);
			
			if(!homSet.isEmpty() && !processedNodes.contains(n)) {
				for(Iterator homIt = homSet.iterator(); homIt.hasNext();) {
					Node hom = (Node) homIt.next();
					PrefixNode from = (PrefixNode) prefixMap.get(n);
					PrefixNode to = (PrefixNode) prefixMap.get(hom);
					dumper.edge(from, to, "hom", GraphDumper.DASHED);  
				}
			}
			
			processedNodes.add(n);
		}
		
		dumper.endSubgraph();
		debug.leaving();
	}

  /**
   * @see de.unika.ipd.grgen.util.Visitor#visit(de.unika.ipd.grgen.util.Walkable)
   */
  public void visit(Walkable n) {
		assert n instanceof IR : "must have an ir object to dump";
		
		if(n instanceof Node || n instanceof Edge || n instanceof Graph)
			return;
			
		if(n instanceof Test) {
			Test test = (Test) n;
			dumper.beginSubgraph(test);
			dumpGraph(test.getPattern(), "");
			dumper.endSubgraph();
		
		} else if(n instanceof Rule) {
			Rule r = (Rule) n;
			dumper.beginSubgraph(r);
			dumpGraph(r.getLeft(), "l");
			dumpGraph(r.getRight(), "r");
			
			Iterator common = r.getCommonNodes().iterator();
			while(common.hasNext()) {
				Node node = (Node) common.next();
				PrefixNode left = new PrefixNode(node, "l");
				PrefixNode right = new PrefixNode(node, "r");
				
				dumper.edge(left, right, null, GraphDumper.DOTTED);
			}
			
			
			dumper.endSubgraph();
		} else 
			super.visit(n);
		
  }

}
