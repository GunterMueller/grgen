/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir;

import java.util.ArrayList;

/**
 * An auto-generated filter.
 */
public class FilterAutoGenerated extends IR implements Filter {
	protected String name;
	protected ArrayList<String> entities;

	/** The action we're a filter for */
	protected Rule action;

	public FilterAutoGenerated(String name, ArrayList<String> entities) {
		super(name);
		this.name = name;
		this.entities = entities;
	}		
	
	public void setAction(Rule action) {
		this.action = action;
	}
	
	public Rule getAction() {
		return action;
	}
	
	public String getFilterName() {
		return name;
	}
	
	public ArrayList<String> getFilterEntities() {
		return entities;
	}
	
	public String getSuffix() {
		StringBuilder sb = new StringBuilder();
		if(entities!=null && entities.size()!=0 && name != "auto") {
			sb.append("<");
			boolean first = true;
			for(String entity : entities) {
				if(first)
					first = false;
				else
					sb.append(",");
				sb.append(entity);
			}
			sb.append(">");
		}
		return sb.toString();
	}
	
	public String getUnderscoreSuffix() {
		StringBuilder sb = new StringBuilder();
		if(entities!=null && entities.size()!=0 && name != "auto") {
			for(String entity : entities) {
				sb.append("_");
				sb.append(entity);
			}
		}
		return sb.toString();
	}
}
