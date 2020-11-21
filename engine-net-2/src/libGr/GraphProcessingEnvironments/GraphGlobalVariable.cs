/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

// by Moritz Kroll

using System;

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// A named graph-global variable.
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// The name of the variable.
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The value pointed to by the variable.
        /// </summary>
        public object Value;

        /// <summary>
        /// Initializes a Variable instance.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value pointed to by the variable.</param>
        public Variable(String name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
