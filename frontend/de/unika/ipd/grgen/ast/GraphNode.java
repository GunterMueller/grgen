/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 5.0
 * Copyright (C) 2003-2020 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */

/**
 * @author shack
 */
package de.unika.ipd.grgen.ast;

import de.unika.ipd.grgen.ast.exprevals.*;
import de.unika.ipd.grgen.ast.containers.*;
import de.unika.ipd.grgen.ast.util.Checker;
import de.unika.ipd.grgen.ast.util.CollectChecker;
import de.unika.ipd.grgen.ast.util.CollectTripleResolver;
import de.unika.ipd.grgen.ast.util.DeclarationTripleResolver;
import de.unika.ipd.grgen.ast.util.SimpleChecker;
import de.unika.ipd.grgen.ast.util.Triple;
import de.unika.ipd.grgen.ir.Edge;
import de.unika.ipd.grgen.ir.Entity;
import de.unika.ipd.grgen.ir.exprevals.EvalStatements;
import de.unika.ipd.grgen.ir.Exec;
import de.unika.ipd.grgen.ir.exprevals.Expression;
import de.unika.ipd.grgen.ir.GraphEntity;
import de.unika.ipd.grgen.ir.exprevals.GraphEntityExpression;
import de.unika.ipd.grgen.ir.IR;
import de.unika.ipd.grgen.ir.exprevals.ImperativeStmt;
import de.unika.ipd.grgen.ir.exprevals.NeededEntities;
import de.unika.ipd.grgen.ir.Node;
import de.unika.ipd.grgen.ir.OrderedReplacements;
import de.unika.ipd.grgen.ir.PatternGraph;
import de.unika.ipd.grgen.ir.SubpatternDependentReplacement;
import de.unika.ipd.grgen.ir.SubpatternUsage;
import de.unika.ipd.grgen.ir.Variable;
import de.unika.ipd.grgen.parser.Coords;
import java.util.Collection;
import java.util.Collections;
import java.util.HashSet;
import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.Set;
import java.util.Vector;

/**
 * AST node that represents a graph pattern
 * as it appears within the replace/modify part of some rule
 * or to be used as base class for PatternGraphNode
 * representing the graph pattern of the pattern part of some rule
 */
public class GraphNode extends BaseNode
{
	static {
		setName(GraphNode.class, "graph");
	}

	protected CollectNode<BaseNode> connectionsUnresolved;
	protected CollectNode<BaseNode> connections = new CollectNode<BaseNode>();
	protected CollectNode<SubpatternUsageNode> subpatterns;
	protected CollectNode<SubpatternReplNode> subpatternRepls;
	protected CollectNode<OrderedReplacementsNode> orderedReplacements;
	protected CollectNode<EvalStatementsNode> yieldsEvals;
	protected CollectNode<ExprNode> returns;
	protected CollectNode<BaseNode> imperativeStmts;
	protected CollectNode<BaseNode> params;
	protected CollectNode<VarDeclNode> defVariablesToBeYieldedTo;

	protected boolean hasAbstractElements;

	// Cache variables
	protected Set<NodeDeclNode> nodes;
	protected Set<EdgeDeclNode> edges;

	/** context(action or pattern, lhs not rhs) in which this node occurs*/
	protected int context = 0;

	PatternGraphNode directlyNestingLHSGraph;

	protected String nameOfGraph;

	/**
	 * A new pattern node
	 * @param connections A collection containing connection nodes
	 */
	public GraphNode(String nameOfGraph, Coords coords,
			CollectNode<BaseNode> connections, CollectNode<BaseNode> params,
			CollectNode<SubpatternUsageNode> subpatterns, CollectNode<SubpatternReplNode> subpatternRepls,
			CollectNode<OrderedReplacementsNode> orderedReplacements, CollectNode<ExprNode> returns,
			CollectNode<BaseNode> imperativeStmts, int context, PatternGraphNode directlyNestingLHSGraph)
	{
		super(coords);
		this.nameOfGraph = nameOfGraph;
		this.connectionsUnresolved = connections;
		becomeParent(this.connectionsUnresolved);
		this.subpatterns = subpatterns;
		becomeParent(this.subpatterns);
		this.subpatternRepls = subpatternRepls;
		becomeParent(this.subpatternRepls);
		this.orderedReplacements = orderedReplacements;
		becomeParent(this.orderedReplacements);
		this.returns = returns;
		becomeParent(this.returns);
		this.imperativeStmts = imperativeStmts;
		becomeParent(imperativeStmts);
		this.params = params;
		becomeParent(this.params);
		this.context = context;

		// if we're constructed as part of a PatternGraphNode
		// then this code will be executed by the PatternGraphNode, so don't do it here
		if(directlyNestingLHSGraph != null) {
			this.directlyNestingLHSGraph = directlyNestingLHSGraph;
			// treat non-var parameters like connections
			addParamsToConnections(params);
		}
	}

	public void addEvals(CollectNode<EvalStatementsNode> yieldsEvals)
	{
		this.yieldsEvals = yieldsEvals;
		becomeParent(this.yieldsEvals);
	}

	public void addDefVariablesToBeYieldedTo(CollectNode<VarDeclNode> defVariablesToBeYieldedTo)
	{
		this.defVariablesToBeYieldedTo = defVariablesToBeYieldedTo;
	}

	/** returns children of this node */
	@Override
	public Collection<BaseNode> getChildren()
	{
		Vector<BaseNode> children = new Vector<BaseNode>();
		children.add(getValidVersion(connectionsUnresolved, connections));
		children.add(params);
		children.add(defVariablesToBeYieldedTo);
		children.add(subpatterns);
		children.add(subpatternRepls);
		children.add(orderedReplacements);
		children.add(yieldsEvals);
		children.add(returns);
		children.add(imperativeStmts);
		return children;
	}

	/** returns names of the children, same order as in getChildren */
	@Override
	public Collection<String> getChildrenNames()
	{
		Vector<String> childrenNames = new Vector<String>();
		childrenNames.add("connections");
		childrenNames.add("params");
		childrenNames.add("defVariablesToBeYieldedTo");
		childrenNames.add("subpatterns");
		childrenNames.add("subpatternReplacements");
		childrenNames.add("orderedReplacements");
		childrenNames.add("yieldsEvals");
		childrenNames.add("returns");
		childrenNames.add("imperativeStmts");
		return childrenNames;
	}

	private static final CollectTripleResolver<ConnectionNode, SingleNodeConnNode, SingleGraphEntityNode> connectionsResolver =
			new CollectTripleResolver<ConnectionNode, SingleNodeConnNode, SingleGraphEntityNode>(
					new DeclarationTripleResolver<ConnectionNode, SingleNodeConnNode, SingleGraphEntityNode>(
							ConnectionNode.class, SingleNodeConnNode.class, SingleGraphEntityNode.class));

	/** @see de.unika.ipd.grgen.ast.BaseNode#resolveLocal() */
	@Override
	protected boolean resolveLocal()
	{
		Triple<CollectNode<ConnectionNode>, CollectNode<SingleNodeConnNode>, CollectNode<SingleGraphEntityNode>> resolve =
				connectionsResolver.resolve(connectionsUnresolved);

		if(resolve != null) {
			if(resolve.first != null) {
				for(ConnectionNode conn : resolve.first.getChildren()) {
					connections.addChild(conn);
					if(!conn.resolve())
						return false;
					if(conn.getEdge().getDeclType().isAbstract()
							|| conn.getSrc().getDeclType().isAbstract()
							|| conn.getTgt().getDeclType().isAbstract())
						hasAbstractElements = true;
				}
			}

			if(resolve.second != null) {
				for(SingleNodeConnNode conn : resolve.second.getChildren()) {
					connections.addChild(conn);
					if(!conn.resolve())
						return false;
					if(conn.getNode().getDeclType().isAbstract())
						hasAbstractElements = true;
				}
			}

			if(resolve.third != null) {
				for(SingleGraphEntityNode ent : resolve.third.getChildren()) {
					// resolve the entity
					if(!ent.resolve()) {
						return false;
					}

					// add reused single node to connections
					if(ent.getEntityNode() != null) {
						connections.addChild(new SingleNodeConnNode(ent.getEntityNode()));
					}

					// add reused subpattern to subpatterns
					if(ent.getEntitySubpattern() != null) {
						subpatterns.addChild(ent.getEntitySubpattern());
					}
				}
			}

			becomeParent(connections);
			becomeParent(subpatterns);
		}

		boolean paramsOK = resolveParamVars();

		boolean subUsagesOK = resolveSubpatternUsages();

		replaceSubpatternReplacementsIntoOrderedReplacements();

		return resolve != null && paramsOK && subUsagesOK;
	}

	private boolean resolveParamVars()
	{
		boolean paramsOK = true;

		for(BaseNode n : params.getChildren()) {
			if(!(n instanceof VarDeclNode))
				continue;

			VarDeclNode paramVar = (VarDeclNode)n;
			if(paramVar.resolve()) {
				if(!(paramVar.getDeclType() instanceof BasicTypeNode)
						&& !(paramVar.getDeclType() instanceof EnumTypeNode)
						&& !(paramVar.getDeclType() instanceof MapTypeNode)
						&& !(paramVar.getDeclType() instanceof SetTypeNode)
						&& !(paramVar.getDeclType() instanceof ArrayTypeNode)
						&& !(paramVar.getDeclType() instanceof DequeTypeNode)
						&& !(paramVar.getDeclType() instanceof ExternalTypeNode)) {
					paramVar.typeUnresolved.reportError("Type of variable \"" + paramVar.getIdentNode()
							+ "\" must be a basic type (like int or string), or an enum, or a container type (set|map|array|deque)"
							+ ("(not " + paramVar.getDeclType().getTypeName() + ")"));
					paramsOK = false;
				}
			} else
				paramsOK = false;
		}
		
		return paramsOK;
	}

	private boolean resolveSubpatternUsages()
	{
		boolean subUsagesOK = true;
		
		if((context & CONTEXT_LHS_OR_RHS) == CONTEXT_RHS) {
			for(SubpatternUsageNode subUsage : subpatterns.getChildren()) {
				if(subUsage.resolve()) {
					PatternGraphNode pattern = subUsage.getSubpatternDeclNode().getPattern();
					if(pattern.hasAbstractElements) {
						subUsage.reportError("Cannot instantiate pattern with abstract elements");
						subUsagesOK = false;
					}
				} else
					subUsagesOK = false;
			}
		}
		
		return subUsagesOK;
	}

	// replace subpattern replacement node placeholder just specifying position in ordered list 
	// by subpattern replacement node from unordered list with correct arguments
	// move missing replacement nodes to the begin of the ordered list, it is the base list for further processing
	private void replaceSubpatternReplacementsIntoOrderedReplacements()
	{
		Iterator<SubpatternReplNode> it = subpatternRepls.getChildren().iterator();
		while(it.hasNext()) {
			SubpatternReplNode subpatternRepl = it.next();
			for(OrderedReplacementsNode orderedRepls : orderedReplacements.getChildren()) {
				if(!orderedRepls.getChildren().isEmpty()) {
					Iterator<OrderedReplacementNode> subCand = orderedRepls.getChildren().iterator();
					OrderedReplacementNode orderedRepl = subCand.next();
					if(orderedRepl instanceof SubpatternReplNode) {
						SubpatternReplNode orderedSubpatternRepl = (SubpatternReplNode)orderedRepl;
						String orderedSubpatternReplName = orderedSubpatternRepl.getSubpatternIdent().toString();
						String subpatternReplName = subpatternRepl.getSubpatternIdent().toString();
						if(orderedSubpatternReplName.equals(subpatternReplName)) {
							subCand.remove();
							orderedRepls.addChild(subpatternRepl);
							it.remove();
						}
					}
				}
			}
		}
		for(int i = subpatternRepls.getChildren().size() - 1; i >= 0; --i) {
			SubpatternReplNode subpatternRepl = subpatternRepls.get(i);
			OrderedReplacementsNode orderedRepls = new OrderedReplacementsNode(subpatternRepl.getCoords(),
					subpatternRepl.getSubpatternIdent().getIdent().toString());
			orderedRepls.addChild(subpatternRepl);
			orderedReplacements.addChildAtFront(orderedRepls);
		}
		subpatternRepls.getChildren().clear();
	}

	private static final Checker connectionsChecker =
			new CollectChecker(new SimpleChecker(ConnectionCharacter.class));

	/**
	 * A pattern node contains just a collect node with connection nodes as its children.
	 * @see de.unika.ipd.grgen.ast.BaseNode#checkLocal()
	 */
	@Override
	protected boolean checkLocal()
	{
		boolean connCheck = connectionsChecker.check(connections, error);

		boolean isRhsEdgeUseOk = true;

		if(connCheck) {
			//check, that each named edge is only used once in a pattern
			isRhsEdgeUseOk = isRhsEdgeReuseOk();
		}

		return isRhsEdgeUseOk && connCheck && noExecStatementInEvalHere();
	}

	private boolean isRhsEdgeReuseOk()
	{
		boolean edgeUsage = true;
		HashSet<EdgeCharacter> edges = new HashSet<EdgeCharacter>();
		for(BaseNode n : connections.getChildren()) {
			ConnectionCharacter cc = (ConnectionCharacter)n;
			EdgeCharacter ec = cc.getEdge();

			// add() returns false iff edges already contains ec
			if (ec != null
					&& !(cc instanceof ConnectionNode
							&& cc.getSrc() instanceof DummyNodeDeclNode
							&& cc.getTgt() instanceof DummyNodeDeclNode)
					&& !edges.add(ec)) {
				((EdgeDeclNode) ec).reportError("Edge " + ec + " is used more than once in a graph of this action");
				edgeUsage = false;
			}
		}
		return edgeUsage;
	}

	protected boolean iteratedNotReferenced(String iterName)
	{
		boolean res = true;
		for(EvalStatementsNode yieldEvalStatements : yieldsEvals.getChildren()) {
			for(EvalStatementNode yieldEvalStatement : yieldEvalStatements.getChildren()) {
				res &= yieldEvalStatement.iteratedNotReferenced(iterName);
			}
		}
		return res;
	}

	protected boolean iteratedNotReferencedInDefElementInitialization(String iterName)
	{
		boolean res = true;
		for(VarDeclNode var : defVariablesToBeYieldedTo.getChildren()) {
			if(var.initialization != null)
				res &= var.initialization.iteratedNotReferenced(iterName);
		}
		return res;
	}

	boolean noExecStatementInEvalHere()
	{
		boolean result = true;
		for(OrderedReplacementsNode orderedRepls : orderedReplacements.getChildren()) {
			result &= orderedRepls.noExecStatement();
		}
		return result;
	}

	/**
	 * Get an iterator iterating over all connections characters in this pattern.
	 * These are the children of the collect node at position 0.
	 * @return The iterator.
	 */
	protected Collection<BaseNode> getConnections()
	{
		assert isResolved();

		return connections.getChildren();
	}

	/**
	 * Get a set of all nodes in this pattern.
	 * Use this function after this node has been checked with {@link #checkLocal()}
	 * to ensure, that the children have the right type.
	 * @return A set containing the declarations of all nodes occurring
	 * in this graph pattern.
	 */
	public Set<NodeDeclNode> getNodes()
	{
		assert isResolved();

		if(nodes != null)
			return nodes;

		LinkedHashSet<NodeDeclNode> coll = new LinkedHashSet<NodeDeclNode>();

		for(BaseNode n : connections.getChildren()) {
			ConnectionCharacter conn = (ConnectionCharacter)n;
			conn.addNodes(coll);
		}

		nodes = Collections.unmodifiableSet(coll);
		return nodes;
	}

	public Set<EdgeDeclNode> getEdges()
	{
		assert isResolved();

		if(edges != null)
			return edges;

		LinkedHashSet<EdgeDeclNode> coll = new LinkedHashSet<EdgeDeclNode>();

		for(BaseNode n : connections.getChildren()) {
			ConnectionCharacter conn = (ConnectionCharacter)n;
			conn.addEdge(coll);
		}

		edges = Collections.unmodifiableSet(coll);
		return edges;
	}

	public CollectNode<VarDeclNode> getDefVariablesToBeYieldedTo()
	{
		return defVariablesToBeYieldedTo;
	}

	/**
	 * Get the correctly casted IR object.
	 * @return The IR object.
	 */
	protected PatternGraph getGraph()
	{
		return checkIR(PatternGraph.class);
	}

	/**
	 * Construct the IR object.
	 * It is a Graph and all the connections (children of the pattern AST node) are put into it.
	 * @see de.unika.ipd.grgen.ast.BaseNode#constructIR()
	 */
	@Override
	protected IR constructIR()
	{
		PatternGraph gr = new PatternGraph(nameOfGraph, 0);
		gr.setDirectlyNestingLHSGraph(directlyNestingLHSGraph.getGraph());

		for(BaseNode n : connections.getChildren()) {
			ConnectionCharacter conn = (ConnectionCharacter)n;
			conn.addToGraph(gr);
		}

		for(VarDeclNode n : defVariablesToBeYieldedTo.getChildren()) {
			gr.addVariable(n.checkIR(Variable.class));
		}

		for(SubpatternUsageNode n : subpatterns.getChildren()) {
			gr.addSubpatternUsage(n.checkIR(SubpatternUsage.class));
		}

		for(OrderedReplacementsNode n : orderedReplacements.getChildren()) {
			gr.addOrderedReplacement((OrderedReplacements)n.getIR());
		}

		// add subpattern usage connection elements only mentioned there to the IR
		// (they're declared in an enclosing graph and locally only show up in the subpattern usage connection)
		for(OrderedReplacementsNode ors : orderedReplacements.getChildren()) {
			addSubpatternReplacementUsageArguments(gr, ors);
		}

		// don't add elements only mentioned in ordered replacements here to the pattern, it prevents them from being deleted
		// in general we must be cautious with adding stuff to rhs because of that problem

		// don't add elements only mentioned in typeof here to the pattern, it prevents them from being deleted
		// in general we must be cautious with adding stuff to rhs because of that problem

		Set<Node> nodesToAdd = new HashSet<Node>();
		Set<Edge> edgesToAdd = new HashSet<Edge>();

		// add elements which we could not be added before because their container was iterated over
		for(Node n : nodesToAdd)
			addNodeIfNotYetContained(gr, n);
		for(Edge e : edgesToAdd)
			addEdgeIfNotYetContained(gr, e);

		for(BaseNode imp : imperativeStmts.getChildren()) {
			gr.addImperativeStmt((ImperativeStmt)imp.getIR());
		}

		// add deferred exec elements only mentioned there to the IR
		// (they're declared in an enclosing graph and locally only show up in the deferred exec)
		for(ImperativeStmt impStmt : gr.getImperativeStmts()) {
			addElementsUsedInDeferredExec(gr, impStmt);
		}

		// ensure def to be yielded to elements are hom to all others
		// so backend doing some fake search planning for them is not scheduling checks for them
		for(Node node : gr.getNodes()) {
			if(node.isDefToBeYieldedTo())
				gr.addHomToAll(node);
		}
		for(Edge edge : gr.getEdges()) {
			if(edge.isDefToBeYieldedTo())
				gr.addHomToAll(edge);
		}

		return gr;
	}

	void addSubpatternReplacementUsageArguments(PatternGraph gr, OrderedReplacementsNode ors)
	{
		for(OrderedReplacementNode orderedReplNode : ors.getChildren()) {
			// TODO: what's with all the other ordered replacement operations containing entitites?
			if(!(orderedReplNode instanceof SubpatternReplNode))
				continue;
			SubpatternReplNode subpatternReplNode = (SubpatternReplNode)orderedReplNode;
			SubpatternDependentReplacement subpatternDepRepl = subpatternReplNode.checkIR(SubpatternDependentReplacement.class); 
			List<Expression> connections = subpatternDepRepl.getReplConnections();
			for(Expression expr : connections) {
				addSubpatternReplacementUsageArgument(gr, expr);
			}
		}
	}

	private void addSubpatternReplacementUsageArgument(PatternGraph gr, Expression expr)
	{
		if(expr instanceof GraphEntityExpression) {
			GraphEntity connection = ((GraphEntityExpression)expr).getGraphEntity();
			if(connection instanceof Node) {
				addNodeIfNotYetContained(gr, (Node)connection);
			} else if(connection instanceof Edge) {
				addEdgeIfNotYetContained(gr, (Edge)connection);
			} else {
				assert(false);
			}
		} else {
			NeededEntities needs = new NeededEntities(false, false, true, false, false, false, false, false);
			expr.collectNeededEntities(needs);
			for(Variable neededVariable : needs.variables) {
				if(!gr.hasVar(neededVariable)) {
					gr.addVariable(neededVariable);
				}
			}
		}
	}

	void addElementsUsedInDeferredExec(PatternGraph gr, ImperativeStmt impStmt)
	{
		if(impStmt instanceof Exec) {
			Set<Entity> neededEntities = ((Exec)impStmt).getNeededEntities(false);
			for(Entity entity : neededEntities) {
				if(entity instanceof Node) {
					addNodeIfNotYetContained(gr, (Node)entity);
				} else if(entity instanceof Edge) {
					addEdgeIfNotYetContained(gr, (Edge)entity);
				} else {
					if(!gr.hasVar((Variable)entity)) {
						gr.addVariable((Variable)entity);
					}
				}
			}
		}
	}

	protected void addNodeIfNotYetContained(PatternGraph gr, Node neededNode)
	{
		if(!gr.hasNode(neededNode)) {
			gr.addSingleNode(neededNode);
			gr.addHomToAll(neededNode);
		}
	}

	protected void addEdgeIfNotYetContained(PatternGraph gr, Edge neededEdge)
	{
		if(!gr.hasEdge(neededEdge)) {
			gr.addSingleEdge(neededEdge); // TODO: maybe we lose context here
			gr.addHomToAll(neededEdge);
		}
	}

	protected void addParamsToConnections(CollectNode<BaseNode> params)
	{
		for(BaseNode n : params.getChildren()) {
			// directly nesting lhs pattern is null for parameters of lhs/rhs pattern
			// because it doesn't exist at the time the parameters are parsed -> patch it in here
			if(n instanceof VarDeclNode) {
				((VarDeclNode)n).directlyNestingLHSGraph = directlyNestingLHSGraph;
				continue;
			} else if(n instanceof SingleNodeConnNode) {
				SingleNodeConnNode sncn = (SingleNodeConnNode)n;
				((NodeDeclNode)sncn.nodeUnresolved).directlyNestingLHSGraph = directlyNestingLHSGraph;
			} else if(n instanceof ConstraintDeclNode) {
				((ConstraintDeclNode)n).directlyNestingLHSGraph = directlyNestingLHSGraph;
			} else {
				// don't need to adapt left/right nodes as only dummies
				// TODO casts checked?
				ConnectionNode cn = (ConnectionNode)n;
				((EdgeDeclNode)cn.edgeUnresolved).directlyNestingLHSGraph = directlyNestingLHSGraph;
			}

			connectionsUnresolved.addChild(n);
		}
	}

	protected Vector<DeclNode> getParamDecls()
	{
		Vector<DeclNode> res = new Vector<DeclNode>();

		for(BaseNode para : params.getChildren()) {
			if(para instanceof ConnectionNode) {
				ConnectionNode conn = (ConnectionNode)para;
				res.add(conn.getEdge().getDecl());
			} else if(para instanceof SingleNodeConnNode) {
				NodeDeclNode node = ((SingleNodeConnNode)para).getNode();
				res.add(node);
			} else if(para instanceof VarDeclNode) {
				res.add((VarDeclNode)para);
			} else
				throw new UnsupportedOperationException("Unsupported parameter (" + para + ")");
		}

		return res;
	}

	public Collection<EvalStatements> getYieldEvalStatements()
	{
		Collection<EvalStatements> ret = new LinkedList<EvalStatements>();

		for(EvalStatementsNode n : yieldsEvals.getChildren()) {
			ret.add(n.checkIR(EvalStatements.class));
		}

		return ret;
	}

	public Collection<OrderedReplacements> getOrderedReplacements()
	{
		Collection<OrderedReplacements> ret = new LinkedList<OrderedReplacements>();

		for(OrderedReplacementsNode n : orderedReplacements.getChildren()) {
			ret.add(n.checkIR(OrderedReplacements.class));
		}

		return ret;
	}
}
