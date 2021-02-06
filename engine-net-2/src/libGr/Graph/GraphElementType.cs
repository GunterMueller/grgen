/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2021 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

// by Moritz Kroll, Edgar Jakumeit

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// A representation of a GrGen graph element type.
    /// </summary>
    public abstract class GraphElementType : InheritanceType
    {
        /// <summary>
        /// Initializes a GrGenType object.
        /// </summary>
        /// <param name="typeID">The type id for this GrGen type.</param>
        protected GraphElementType(int typeID)
            : base(typeID)
        {
        }

        /// <summary>
        /// True, if this type is a node type.
        /// </summary>
        public abstract bool IsNodeType { get; }
    }
}
