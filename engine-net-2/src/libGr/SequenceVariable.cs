/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 2.6
 * Copyright (C) 2003-2010 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

using System;

namespace de.unika.ipd.grGen.libGr
{
    /// <summary>
    /// A variable declared/used within a sequence, 
    /// might be a sequence-local variable or a reference to a graph-global variable.
    /// It is first stored within the symbol table belonging to the sequence on sequence parsing,
    /// after parsing only on the heap, with references from the sequence AST pointing to it.
    /// </summary>
    public class SequenceVariable
    {
        public SequenceVariable(String name, String prefix, String type)
        {
            this.name = name;
            this.prefix = prefix;
            this.type = type;
            value = null;
            visited = false;
        }

        // the name of the variable
        public String Name { get { return name; } }
        // the nesting prefix of the variable
        public String Prefix { get { return prefix; } }
        // the type of the variable (a graph-global variable maps to "", a sequence-local to its type)
        public String Type { get { return type; } }

        // the variable value if the xgrs gets interpreted/executed
        // TODO: cast the value to the declared type on write, error check throwing exception
        // TODO: sequence can be used multiple times: sequence re-initialization is needed
        // davor? danach? dazwischen? dazwischen am besten, aber muss von hand gemacht werden. 
        // davor/danach k�nnten automatisch vor/nach Apply laufen
        public object Value { get { return value; } set { this.value = value; } }

        // gets the variable value, decides whether to query the graph-global or the sequence-lokal variables
        public object GetVariableValue(IGraph graph)
        {
            if(Type == "") {
                return graph.GetVariableValue(Name);
            } else {
                return Value;
            }
        }

        // sets the variable value, decides whether to update the graph-global or the sequence-lokal variables
        public void SetVariableValue(object value, IGraph graph)
        {
            if(Type == "") {
                graph.SetVariableValue(Name, value);
            } else {
                Value = value;
            }
        }

        // visited flag used in xgrs code generation
        public bool Visited { get { return visited; } set { this.visited = value; } }


        private String name;
        private String prefix;
        private String type;

        private object value;

        private bool visited;
    }
}