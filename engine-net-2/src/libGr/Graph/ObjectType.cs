/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

// by Edgar Jakumeit

using System;
using System.Diagnostics;

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// A representation of a GrGen object type, i.e. class of internal non-node/edge values.
    /// </summary>
    public abstract class ObjectType : InheritanceType
    {
        /// <summary>
        /// Constructs an ObjectType (non-graph-element internal class) instance with the given type ID.
        /// </summary>
        /// <param name="typeID">The unique type ID.</param>
        protected ObjectType(int typeID)
            : base(typeID)
        {
        }

        /// <summary>
        /// This ObjectType describes classes whose real .NET interface type is named as returned (fully qualified).
        /// </summary>
        public abstract String ObjectInterfaceName { get; }

        /// <summary>
        /// This ObjectType describes classes whose real .NET class type is named as returned (fully qualified).
        /// It might be null in case this type IsAbstract.
        /// </summary>
        public abstract String ObjectClassName { get; }

        /// <summary>
        /// Creates an object according to this type.
        /// </summary>
        /// <returns>The created object.</returns>
        public abstract IObject CreateObject();

        /// <summary>
        /// Creates an object according to this type and copies all
        /// common attributes from the given object.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <returns>The created object.</returns>
        public abstract IObject CreateObjectWithCopyCommons(IObject oldValue);

        /// <summary>
        /// Array containing this type first and following all sub types
        /// </summary>
        public ObjectType[] subOrSameTypes;
        /// <summary>
        /// Array containing all direct sub types of this type.
        /// </summary>
        public ObjectType[] directSubTypes;
        /// <summary>
        /// Array containing this type first and following all super types
        /// </summary>
        public ObjectType[] superOrSameTypes;
        /// <summary>
        /// Array containing all direct super types of this type.
        /// </summary>
        public ObjectType[] directSuperTypes;

        /// <summary>
        /// Array containing this type first and following all sub types
        /// </summary>
        public new ObjectType[] SubOrSameTypes
        {
            [DebuggerStepThrough]
            get { return subOrSameTypes; }
        }

        /// <summary>
        /// Array containing all direct sub types of this type.
        /// </summary>
        public new ObjectType[] DirectSubTypes
        {
            [DebuggerStepThrough]
            get { return directSubTypes; }
        }

        /// <summary>
        /// Array containing this type first and following all super types
        /// </summary>
        public new ObjectType[] SuperOrSameTypes
        {
            [DebuggerStepThrough]
            get { return superOrSameTypes; }
        }

        /// <summary>
        /// Array containing all direct super types of this type.
        /// </summary>
        public new ObjectType[] DirectSuperTypes
        {
            [DebuggerStepThrough]
            get { return directSuperTypes; }
        }

        /// <summary>
        /// Tells whether the given type is the same or a subtype of this type
        /// </summary>
        public abstract bool IsMyType(int typeID);

        /// <summary>
        /// Tells whether this type is the same or a subtype of the given type
        /// </summary>
        public abstract bool IsA(int typeID);

        /// <summary>
        /// The annotations of the class type
        /// </summary>
        public abstract Annotations Annotations { get; }
    }
}