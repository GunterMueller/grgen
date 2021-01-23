/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.2
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

// by Edgar Jakumeit

using de.unika.ipd.grGen.libGr;
using System;
using System.Diagnostics;

namespace de.unika.ipd.grGen.lgsp
{
    /// <summary>
    /// Class implementing objects in the libGr search plan backend (values of internal non-node/edge classes)
    /// </summary>
    [DebuggerDisplay("LGSPObject ({Type})")]
    public abstract class LGSPObject : IObject
    {
        /// <summary>
        /// The object type (class) of the object/value.
        /// </summary>
        public readonly ObjectType lgspType;

        /// <summary>
        /// Contains a unique id (intended use: filled at creation, never changed)
        /// </summary>
        public long uniqueId;


        /// <summary>
        /// Instantiates a LGSPObject object.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        protected LGSPObject(ObjectType objectType, long uniqueId)
        {
            lgspType = objectType;
            this.uniqueId = uniqueId;
        }

        /// <summary>
        /// The ObjectType of the object.
        /// </summary>
        public ObjectType Type
        {
            [DebuggerStepThrough]
            get { return lgspType; }
        }

        /// <summary>
        /// The BaseObjectType of the (base) object.
        /// </summary>
        BaseObjectType IBaseObject.Type
        {
            [DebuggerStepThrough]
            get { return lgspType; }
        }

        /// <summary>
        /// The InheritanceType of the attribute bearer.
        /// </summary>
        InheritanceType IAttributeBearer.Type
        {
            [DebuggerStepThrough]
            get { return lgspType; }
        }

        /// <summary>
        /// Returns true, if the class object is compatible to the given type.
        /// </summary>
        public bool InstanceOf(GrGenType otherType)
        {
            return lgspType.IsA(otherType);
        }

        /// <summary>
        /// Gets the unique id of the class object.
        /// </summary>
        /// <returns>The unique id of the class object.</returns>
        public long GetUniqueId()
        {
            return uniqueId;
        }

        /// <summary>
        /// Sets the unique id of the class object.
        /// You have to ensure consistency! (only meant for internal use.)
        /// </summary>
        public void SetUniqueId(long uniqueId)
        {
            this.uniqueId = uniqueId;
        }

        /// <summary>
        /// Gets the name of the class object.
        /// </summary>
        /// <returns>The name of the class object.</returns>
        public string GetObjectName()
        {
            return String.Format("%{0,00000000:X}", uniqueId);
        }

        /// <summary>
        /// Indexer that gives access to the attributes of the class object.
        /// </summary>
        public object this[string attrName]
        {
            get { return GetAttribute(attrName); }
            set { SetAttribute(attrName, value); }
        }

        /// <summary>
        /// Returns the attribute with the given attribute name.
        /// If the class type doesn't have an attribute with this name, a NullReferenceException is thrown.
        /// </summary>
        public abstract object GetAttribute(string attrName);

        /// <summary>
        /// Sets the attribute with the given attribute name to the given value.
        /// If the class type doesn't have an attribute with this name, a NullReferenceException is thrown.
        /// </summary>
        /// <param name="attrName">The name of the attribute.</param>
        /// <param name="value">The new value for the attribute. It must have the correct type.
        /// Otherwise a TargetException is thrown.</param>
        public abstract void SetAttribute(string attrName, object value);

        /// <summary>
        /// Resets all class object attributes to their initial values.
        /// </summary>
        public abstract void ResetAllAttributes();

        /// <summary>
        /// Creates a copy of this object.
        /// All attributes will be transfered to the new object.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public abstract IObject Clone(IGraph graph);

        /// <summary>
        /// Creates a copy of this (base) object.
        /// All attributes will be transfered to the new object.
        /// </summary>
        /// <returns>A copy of this (base) object.</returns>
        IBaseObject IBaseObject.Clone()
        {
            throw new Exception("Use IObject.Clone(IGraph graph)");
        }

        /// <summary>
        /// Returns whether the attributes of this object and that object are equal.
        /// If types are unequal the result is false, otherwise the conjunction of equality comparison of the attributes.
        /// </summary>
        public abstract bool AreAttributesEqual(IAttributeBearer that);

        /// <summary>
        /// Executes the function method given by its name.
        /// Throws an exception if the method does not exists or the parameters are of wrong types.
        /// </summary>
        /// <param name="actionEnv">The current action execution environment.</param>
        /// <param name="graph">The current graph.</param>
        /// <param name="name">The name of the function method to apply.</param>
        /// <param name="arguments">An array with the arguments to the method.</param>
        /// <returns>The return value of function application.</returns>
        public abstract object ApplyFunctionMethod(IActionExecutionEnvironment actionEnv, IGraph graph, string name, object[] arguments);

        /// <summary>
        /// Executes the procedure method given by its name.
        /// Throws an exception if the method does not exists or the parameters are of wrong types.
        /// </summary>
        /// <param name="actionEnv">The current action execution environment.</param>
        /// <param name="graph">The current graph.</param>
        /// <param name="name">The name of the procedure method to apply.</param>
        /// <param name="arguments">An array with the arguments to the method.</param>
        /// <returns>An array with the return values of procedure application. Only valid until the next call of this method.</returns>
        public abstract object[] ApplyProcedureMethod(IActionExecutionEnvironment actionEnv, IGraph graph, string name, object[] arguments);

        /// <summary>
        /// Returns the name of the type of this class.
        /// </summary>
        /// <returns>The name of the type of this class.</returns>
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
