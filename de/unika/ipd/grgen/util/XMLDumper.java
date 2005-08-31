/**
 * XMLDumper.java
 *
 * @author Created by Omnicore CodeGuide
 */

package de.unika.ipd.grgen.util;

import java.io.PrintStream;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map;

public class XMLDumper {
	
	private final PrintStream ps;
	
	private int indent = 0;
	
	private final String indentString;
	
	private boolean printingAttributes = false;
	
	private final Collection<XMLDumpable> visited = new HashSet<XMLDumpable>();
	
	public XMLDumper(PrintStream ps) {
		this(ps, "  ");
	}

	public XMLDumper(PrintStream ps, String indentString) {
		this.ps = ps;
		this.indentString = indentString;
	}
	
	public void dump(XMLDumpable dumpable) {
		if(visited.contains(dumpable)) {
			dumpRef(dumpable);
			return;
		}
		
		visited.add(dumpable);

		Map<String, Object> fields = new HashMap<String, Object>();
		dumpable.addFields(fields);
		String tagName = dumpable.getTagName();
		
		indent();
		ps.print('<');
		ps.print(tagName);
		ps.print(" id=\"");
		ps.print(dumpable.getXMLId());
		ps.print('\"');

		for(Iterator<String> it = fields.keySet().iterator(); it.hasNext();) {
			Object obj = it.next();
			Object val = fields.get(obj);
			if(!(val instanceof Iterator)) {
				ps.print(' ');
				ps.print(obj);
				ps.print("=\"");
				ps.print(val);
				ps.print('\"');
				it.remove();
			}
		}

		if(!fields.isEmpty()) {
			ps.println('>');
			indent++;
			for(Iterator<String> it = fields.keySet().iterator(); it.hasNext();) {
				Object obj = it.next();
				Iterator<XMLDumpable> childs = (Iterator) fields.get(obj);
				String tag = obj.toString();

				if(childs.hasNext()) {
					indent();
					ps.print('<');
					ps.print(tag);
					ps.println('>');
					indent++;
					
					while(childs.hasNext()) {
						XMLDumpable d = childs.next();
						dump(d);
					}
					
					indent--;
					indent();
					ps.print("</");
					ps.print(tag);
					ps.println('>');
				}
			}
			indent--;
			indent();
			ps.print("</");
			ps.print(tagName);
			ps.println('>');
		} else
			ps.println("/>");
	}

	private void dumpRef(XMLDumpable dumpable) {
		indent();
		ps.print('<');
		ps.print(dumpable.getRefTagName());
		ps.print(" id=\"");
		ps.print(dumpable.getXMLId());
		ps.println("\"/>");
	}
	
	private final void indent() {
		for(int i = 0; i < indent; i++)
			ps.print(indentString);
	}
	
}

