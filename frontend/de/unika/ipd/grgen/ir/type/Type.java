/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author shack
 */

package de.unika.ipd.grgen.ir.type;

import java.util.Comparator;

import de.unika.ipd.grgen.ir.Ident;
import de.unika.ipd.grgen.ir.Identifiable;
import de.unika.ipd.grgen.ir.type.model.InheritanceType;

/**
 * Abstract base class for types.
 * Subclasses distinguished into primitive (string, int, boolean, ...) and compound
 */
public abstract class Type extends Identifiable
{
	/** helper class for comparing objects of type Type, used in compareTo, overwriting comparteTo of Identifiable */
	private static final Comparator<Type> COMPARATOR = new Comparator<Type>() {
		public int compare(Type t1, Type t2)
		{
			if(t1.isEqual(t2))
				return 0;

			if((t1 instanceof InheritanceType) && (t2 instanceof InheritanceType)) {
				int distT1 = ((InheritanceType)t1).getMaxDist();
				int distT2 = ((InheritanceType)t2).getMaxDist();

				if(distT1 < distT2)
					return -1;
				else if(distT1 > distT2)
					return 1;
			}

			return t1.getIdent().compareTo(t2.getIdent());
		}
	};

	public static final int IS_UNKNOWN = 0;
	public static final int IS_BYTE = 1;
	public static final int IS_SHORT = 2;
	public static final int IS_INTEGER = 3;
	public static final int IS_LONG = 4;
	public static final int IS_FLOAT = 5;
	public static final int IS_DOUBLE = 6;
	public static final int IS_BOOLEAN = 7;
	public static final int IS_STRING = 8;
	public static final int IS_TYPE = 9;
	public static final int IS_OBJECT = 10;
	public static final int IS_SET = 11;
	public static final int IS_MAP = 12;
	public static final int IS_ARRAY = 13;
	public static final int IS_DEQUE = 14;
	public static final int IS_UNTYPED_EXEC_VAR_TYPE = 15;
	public static final int IS_EXTERNAL_TYPE = 16;
	public static final int IS_GRAPH = 17;
	public static final int IS_MATCH = 18;
	public static final int IS_DEFINED_MATCH = 19;
	public static final int IS_NODE = 20;
	public static final int IS_EDGE = 21;

	/**
	 * Make a new type.
	 * @param name The name of the type (test, group, ...).
	 * @param ident The identifier used to declare that type.
	 */
	public Type(String name, Ident ident)
	{
		super(name, ident);
	}

	/**
	 * Decides, if two types are equal.
	 * @param t The other type.
	 * @return true, if the types are equal.
	 */
	public boolean isEqual(Type t)
	{
		return t == this;
	}

	/**
	 * Compute, if this type is castable to another type.
	 * You do not have to check, if <code>t == this</code>.
	 * @param t The other type.
	 * @return true, if this type is castable.
	 */
	protected boolean castableTo(Type t)
	{
		return false;
	}

	/**
	 * Checks, if this type is castable to another type.
	 * This method is final, to implement the castability, overwrite <code>castableTo</code>, which is called by this method.
	 * @param t The other type.
	 * @return true, if this type can be casted to <code>t</code>, false otherwise.
	 */
	public final boolean isCastableTo(Type t)
	{
		return isEqual(t) || castableTo(t);
	}

	/** @return true, if this type is a void type. */
	public boolean isVoid()
	{
		return false;
	}

	/** Return a classification of a type for the IR. */
	public int classify()
	{
		return IS_UNKNOWN;
	}

	static final Comparator<Type> getComparator()
	{
		return COMPARATOR;
	}

	public int compareTo(Identifiable id)
	{
		if(id instanceof Type) {
			return COMPARATOR.compare(this, (Type)id);
		}

		assert false;
		return super.compareTo(id);
	}

	public boolean isOrderableType()
	{
		if(classify() == IS_BYTE)
			return true;
		if(classify() == IS_SHORT)
			return true;
		if(classify() == IS_INTEGER) // includes ENUM
			return true;
		if(classify() == IS_LONG)
			return true;
		if(classify() == IS_FLOAT)
			return true;
		if(classify() == IS_DOUBLE)
			return true;
		if(classify() == IS_STRING)
			return true;
		if(classify() == IS_BOOLEAN)
			return true;
		return false;
	}

	public boolean isFilterableType()
	{
		if(isOrderableType())
			return true;
		if(classify() == IS_NODE)
			return true;
		if(classify() == IS_EDGE)
			return true;
		return false;
	}
}
