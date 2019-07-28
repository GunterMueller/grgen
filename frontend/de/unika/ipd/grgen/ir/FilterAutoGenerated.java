/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2019 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author Edgar Jakumeit
 */

package de.unika.ipd.grgen.ir;

/**
 * An auto-generated filter.
 */
public class FilterAutoGenerated extends IR implements Filter {
	protected String name;
	protected String entity;

	/** The action we're a filter for */
	protected Rule action;

	public FilterAutoGenerated(String name, String entity) {
		super(name);
		this.name = name;
		this.entity = entity;
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
	
	public String getFilterEntity() {
		return entity;
	}
}
