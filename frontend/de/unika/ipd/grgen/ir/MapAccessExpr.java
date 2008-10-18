/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.0
 * Copyright (C) 2008 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under GPL v3 (see LICENSE.txt included in the packaging of this file)
 */

/**
 * @author Moritz Kroll, Edgar Jakumeit
 * @version $Id: MapInit.java 22945 2008-10-16 16:02:13Z moritz $
 */

package de.unika.ipd.grgen.ir;

public class MapAccessExpr extends Expression {
	Expression targetExpr;
	Expression keyExpr;
	
	public MapAccessExpr(Expression targetExpr, Expression keyExpr) {
		super("map access expression", targetExpr.getType());
		this.targetExpr = targetExpr;
		this.keyExpr = keyExpr;
	}
	
	public void collectNeededEntities(NeededEntities needs) {
		keyExpr.collectNeededEntities(needs);
		targetExpr.collectNeededEntities(needs);
	}
	
	public Expression getTargetExpr() {
		return targetExpr;
	}
	
	public Expression getKeyExpr() {
		return keyExpr;
	}
}