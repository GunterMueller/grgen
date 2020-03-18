/*
 * GrGen: graph rewrite generator tool -- release GrGen.NET 4.5
 * Copyright (C) 2003-2017 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos; and free programmers
 * licensed under LGPL v3 (see LICENSE.txt included in the packaging of this file)
 * www.grgen.net
 */
 
/*
 * GrGen sequence in rule specification language grammar for ANTLR 3
 * @author Sebastian Hack, Daniel Grund, Rubino Geiss, Adam Szalkowski, Veit Batz, Edgar Jakumeit, Sebastian Buchwald, Moritz Kroll
 * @version $Id: base.g 20237 2008-06-24 15:59:24Z eja $
*/

parser grammar EmbeddedExec;

@members {
	private OpNode makeOp(org.antlr.runtime.Token t) {
		return gParent.makeOp(t);
	}

	private OpNode makeBinOp(org.antlr.runtime.Token t, ExprNode op0, ExprNode op1) {
		return gParent.makeBinOp(t, op0, op1);
	}

	private OpNode makeUnOp(org.antlr.runtime.Token t, ExprNode op) {
		return gParent.makeUnOp(t, op);
	}

	protected ParserEnvironment env;

	public void setEnv(ParserEnvironment env) {
		this.env = env;
	}

	protected Coords getCoords(org.antlr.runtime.Token tok) {
		return new Coords(tok);
	}

	protected final void reportError(de.unika.ipd.grgen.parser.Coords c, String s) {
		gParent.hadError = true;
		env.getSystem().getErrorReporter().error(c, s);
	}

	public void displayRecognitionError(String[] tokenNames, RecognitionException e) {
        String hdr = getErrorHeader(e);
        String msg = getErrorMessage(e, tokenNames);
        reportError(new Coords(e), msg);
    }

	public void reportWarning(de.unika.ipd.grgen.parser.Coords c, String s) {
		env.getSystem().getErrorReporter().warning(c, s);
	}

	public String getFilename() {
		return env.getFilename();
	}
}

//////////////////////////////////////////
// Embedded XGRS / extended graph rewrite sequences
//////////////////////////////////////////

// todo: add more user friendly explicit error messages for % used after $ instead of implicit syntax error
// (a user choice $% override for the random flag $ is only available in the shell/debugger)

// note: sequences and expressions are right associative here, that's wrong but doesn't matter cause this is only a syntax checking pre pass
// in the backend, the operators are parsed with correct associativity (and with correct left-to-right, def-before-use order of variables)

sequenceInParameters [ ExecNode xg ] returns [ CollectNode<ExecVarDeclNode> res = new CollectNode<ExecVarDeclNode>() ]
	: LPAREN (sequenceParamList[res, xg])? RPAREN
	|
	;

sequenceOutParameters [ ExecNode xg ] returns [ CollectNode<ExecVarDeclNode> res = new CollectNode<ExecVarDeclNode>() ]
	: COLON LPAREN (sequenceParamList[res, xg])? RPAREN
	|
	;

sequenceParamList [ CollectNode<ExecVarDeclNode> params, ExecNode xg ]
	: p=seqEntityDecl[xg, false] { params.addChild(p); } ( COMMA p=seqEntityDecl[xg, false] { params.addChild(p); } )*
	;

sequence[ExecNode xg]
	: seqLazyOr[xg] ( DOLLAR THENLEFT {xg.append(" $<; ");} sequence[xg] | THENLEFT {xg.append(" <; ");} sequence[xg]
						| DOLLAR THENRIGHT {xg.append(" $;> ");} sequence[xg] | THENRIGHT {xg.append(" ;> ");} sequence[xg] )?
	;

seqLazyOr[ExecNode xg]
	: seqLazyAnd[xg] ( DOLLAR LOR {xg.append(" $|| ");} seqLazyOr[xg] | LOR {xg.append(" || ");} seqLazyOr[xg] )?
	;

seqLazyAnd[ExecNode xg]
	: seqStrictOr[xg] ( DOLLAR LAND {xg.append(" $&& ");} seqLazyAnd[xg] | LAND {xg.append(" && ");} seqLazyAnd[xg] )?
	;

seqStrictOr[ExecNode xg]
	: seqStrictXor[xg] ( DOLLAR BOR {xg.append(" $| ");} seqStrictOr[xg] | BOR {xg.append(" | ");} seqStrictOr[xg] )?
	;

seqStrictXor[ExecNode xg]
	: seqStrictAnd[xg] ( DOLLAR BXOR {xg.append(" $^ ");} seqStrictXor[xg] | BXOR {xg.append(" ^ ");} seqStrictXor[xg] )?
	;

seqStrictAnd[ExecNode xg]
	: seqNegOrIteration[xg] ( DOLLAR BAND {xg.append(" $& ");} seqStrictAnd[xg] | BAND {xg.append(" & ");} seqStrictAnd[xg] )?
	;

seqNegOrIteration[ExecNode xg]
	: NOT {xg.append("!");} seqIterSequence[xg] 
		(ASSIGN_TO {xg.append("=>");} seqEntity[xg] | BOR_TO {xg.append("|>");} seqEntity[xg] | BAND_TO {xg.append("&>");} seqEntity[xg])?
	| seqIterSequence[xg]
		(ASSIGN_TO {xg.append("=>");} seqEntity[xg] | BOR_TO {xg.append("|>");} seqEntity[xg] | BAND_TO {xg.append("&>");} seqEntity[xg])?
	;

seqIterSequence[ExecNode xg]
	: seqSimpleSequence[xg]
		(
			rsn=seqRangeSpecLoop { xg.append(rsn); }
		|
			STAR { xg.append("*"); }
		|
			PLUS { xg.append("+"); }
		)
	;

seqSimpleSequence[ExecNode xg]
options { k = 4; }
	@init{
		CollectNode<BaseNode> returns = new CollectNode<BaseNode>();
	}
	
	// attention/todo: names are are only partly resolved!
	// -> using not existing types, not declared names outside of the return assignment of an action call 
	// will not be detected in the frontend; xgrs in the frontend are to a certain degree syntax only
	: (seqEntity[null] (ASSIGN | GE )) => lhs=seqEntity[xg] (ASSIGN { xg.append("="); } | GE { xg.append(">="); })
		(
			id=seqEntIdentUse LPAREN // deliver understandable error message for case of missing parenthesis at rule result assignment
				{ reportError(id.getCoords(), "the destination variable(s) of a rule result assignment must be enclosed in parenthesis"); }
		|
			(seqConstant[null]) => seqConstant[xg]
		|
			seqVarUse[xg]
		|
			d=DOLLAR MOD LPAREN seqTypeIdentUse RPAREN
			{ reportError(getCoords(d), "user input is only requestable in the GrShell, not at lgsp(libgr search plan backend)-level"); }
		|
			d=DOLLAR LPAREN 
			(
				n=NUM_INTEGER RPAREN { xg.append("$("); xg.append(n.getText()); xg.append(")"); }
				| f=NUM_DOUBLE RPAREN { xg.append("$("); xg.append(f.getText()); xg.append(")"); }
			)
		|
			LPAREN { xg.append('('); } sequence[xg] RPAREN { xg.append(')'); }
		)
	| seqVarDecl=seqEntityDecl[xg, true]
	| YIELD { xg.append("yield "); } lhsent=seqEntIdentUse { xg.append(lhsent); xg.addUsage(lhsent); } ASSIGN { xg.append('='); } 
		( (seqConstant[null]) => seqConstant[xg]
		| seqVarUse[xg]
		)
	| TRUE { xg.append("true"); }
	| FALSE { xg.append("false"); }
	| seqRulePrefixedSequence[xg, returns]
	| seqMultiRulePrefixedSequence[xg, returns]
	| (seqParallelCallRule[null, null]) => seqParallelCallRule[xg, returns]
	| seqMultiRuleAllCall[xg, returns, true]
	| DOUBLECOLON id=seqEntIdentUse { xg.append("::" + id); xg.addUsage(id); }
	| (( DOLLAR ( MOD )? )? LBRACE LT) => ( DOLLAR { xg.append("$"); } ( MOD { xg.append("\%"); } )? )?
		LBRACE LT { xg.append("{<"); } seqParallelCallRule[xg, returns] 
			(COMMA { xg.append(","); returns = new CollectNode<BaseNode>(); } seqParallelCallRule[xg, returns])* GT RBRACE { xg.append(">}"); }
	| DOLLAR { xg.append("$"); } ( MOD { xg.append("\%"); } )? 
		(LOR { xg.append("||"); } | LAND { xg.append("&&"); } | BOR { xg.append("|"); } | BAND { xg.append("&"); }) 
		LPAREN { xg.append("("); } sequence[xg] (COMMA { xg.append(","); } sequence[xg])* RPAREN { xg.append(")"); }
	| DOLLAR { xg.append("$"); } ( MOD { xg.append("\%"); } )? DOT { xg.append("."); } 
		LPAREN { xg.append("("); } f=NUM_DOUBLE { xg.append(f.getText() + " "); } sequence[xg] 
			(COMMA { xg.append(","); } f=NUM_DOUBLE { xg.append(f.getText() + " "); } sequence[xg])* RPAREN { xg.append(")"); }
	| LPAREN { xg.append("("); } sequence[xg] RPAREN { xg.append(")"); }
	| LT { xg.append(" <"); } sequence[xg] GT { xg.append("> "); }
	| l=SL { env.pushScope("backtrack/exec", getCoords(l)); } { xg.append(" <<"); } 
		seqParallelCallRule[xg, returns] (DOUBLE_SEMI|SEMI) { xg.append(";;"); } sequence[xg] { env.popScope(); } SR { xg.append(">> "); }
	| l=SL { env.pushScope("backtrack/exec", getCoords(l)); } { xg.append(" <<"); } 
		seqMultiRuleAllCall[xg, returns, false] (DOUBLE_SEMI|SEMI) { xg.append(";;"); } sequence[xg] { env.popScope(); } SR { xg.append(">> "); }
	| SL { xg.append(" <<"); } seqMultiRulePrefixedSequence[xg, returns] SR { xg.append(">> "); }
	| DIV { xg.append(" /"); } sequence[xg] DIV { xg.append("/ "); }
	| IF l=LBRACE { env.pushScope("if/exec", getCoords(l)); } { xg.append("if{"); } sequence[xg] s=SEMI 
		{ env.pushScope("if/then-part", getCoords(s)); } { xg.append("; "); } sequence[xg] { env.popScope(); }
		(SEMI { xg.append("; "); } sequence[xg])? { env.popScope(); } RBRACE { xg.append("}"); }
	| FOR l=LBRACE { env.pushScope("for/exec", getCoords(l)); } { xg.append("for{"); } seqEntity[xg] seqForSeqRemainder[xg, returns]
	| IN { xg.append("in "); } seqVarUse[xg] (d=DOT attr=IDENT { xg.append("."+attr.getText()); })? 
		LBRACE { xg.append("{"); } { env.pushScope("in subgraph sequence", getCoords(l)); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); } 
	| LBRACE { xg.append("{"); } { env.pushScope("sequence computation", getCoords(l)); }
		seqCompoundComputation[xg] (SEMI)? { env.popScope(); } RBRACE { xg.append("}"); } 
	;

seqForSeqRemainder[ExecNode xg, CollectNode<BaseNode> returns]
options { k = 4; }
	: (RARROW { xg.append(" -> "); } seqEntity[xg])? IN { xg.append(" in "); } seqEntity[xg]
			SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	| IN { xg.append(" in "); } { input.LT(1).getText().equals("adjacent") || input.LT(1).getText().equals("adjacentIncoming") || input.LT(1).getText().equals("adjacentOutgoing")
			|| input.LT(1).getText().equals("incident") || input.LT(1).getText().equals("incoming") || input.LT(1).getText().equals("outgoing")
			|| input.LT(1).getText().equals("reachable") || input.LT(1).getText().equals("reachableIncoming") || input.LT(1).getText().equals("reachableOutgoing")
			|| input.LT(1).getText().equals("reachableEdges") || input.LT(1).getText().equals("reachableEdgesIncoming") || input.LT(1).getText().equals("reachableEdgesOutgoing") 
			|| input.LT(1).getText().equals("boundedReachable") || input.LT(1).getText().equals("boundedReachableIncoming") || input.LT(1).getText().equals("boundedReachableOutgoing")
			|| input.LT(1).getText().equals("boundedReachableEdges") || input.LT(1).getText().equals("boundedReachableEdgesIncoming") || input.LT(1).getText().equals("boundedReachableEdgesOutgoing") 
			|| input.LT(1).getText().equals("nodes") || input.LT(1).getText().equals("edges")
		 }?
			i=IDENT LPAREN { xg.append(i.getText()); xg.append("("); }
			(expr1=seqExpression[xg] (COMMA { xg.append(","); } expr2=seqExpression[xg] 
				(COMMA { xg.append(","); } expr3=seqExpression[xg] (COMMA { xg.append(","); } expr4=seqExpression[xg])? )? 
					)? )?
			RPAREN { xg.append(")"); }
			SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	| IN { xg.append(" in "); } LBRACE { xg.append("{"); } seqIndex[xg] EQUAL { xg.append(" == "); } seqExpression[xg] 
		RBRACE { xg.append("}"); } SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	| IN { xg.append(" in "); } LBRACE { xg.append("{"); } 
		{ input.LT(1).getText().equals("ascending") || input.LT(1).getText().equals("descending") }? i=IDENT { xg.append(i.getText()); } 
		LPAREN { xg.append("("); } seqIndex[xg] ( seqRelOs[xg] seqExpression[xg]
				( COMMA { xg.append(","); } seqIndex[xg] seqRelOs[xg] seqExpression[xg] )? )? 
		RPAREN { xg.append(")"); } RBRACE { xg.append("}"); } SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	| IN LBRACK QUESTION { xg.append(" in [?"); } seqCallRule[xg, null, returns, true] RBRACK { xg.append("]"); }
			SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	| IN LBRACK { xg.append(" in ["); } left=seqExpression[xg] COLON { xg.append(" : "); } right=seqExpression[xg] RBRACK { xg.append("]"); }
			SEMI { xg.append("; "); } sequence[xg] { env.popScope(); } RBRACE { xg.append("}"); }
	;

seqCompoundComputation[ExecNode xg]
	: seqComputation[xg] (SEMI { xg.append(";"); } seqCompoundComputation[xg])?
	;

seqComputation[ExecNode xg]
	: (seqAssignTarget[null] (ASSIGN|GE)) => seqAssignTarget[xg] (ASSIGN { xg.append("="); } | GE { xg.append(">="); }) seqExpressionOrAssign[xg]
	| (seqEntityDecl[null,true]) => seqEntityDecl[xg, true]
	| (seqMethodCall[null]) => seqMethodCall[xg]
	| (seqProcedureCall[null]) => seqProcedureCall[xg]
	| LBRACE { xg.append("{"); } seqExpression[xg] RBRACE { xg.append("}"); }
	;

seqMethodCall[ExecNode xg]
	: seqVarUse[xg] d=DOT method=IDENT LPAREN { xg.append("."+method.getText()+"("); } 
			 ( seqExpression[xg] (COMMA { xg.append(","); } seqExpression[xg])* )? RPAREN { xg.append(")"); }
	;

seqExpressionOrAssign[ExecNode xg]
	: (seqAssignTarget[null] (ASSIGN|GE)) => seqAssignTarget[xg] (ASSIGN { xg.append("="); } | GE { xg.append(">="); }) seqExpressionOrAssign[xg]
	| seqExpression[xg] 
	;

seqAssignTarget[ExecNode xg]
	: (seqVarUse[null] DOT IDENT ) => seqVarUse[xg] d=DOT attr=IDENT { xg.append("."+attr.getText()); }
		(LBRACK { xg.append("["); } seqExpression[xg] RBRACK { xg.append("]"); })?
	| (seqVarUse[null] DOT VISITED) => seqVarUse[xg] DOT VISITED LBRACK { xg.append(".visited["); } seqExpression[xg] RBRACK { xg.append("]"); } 
	| (seqVarUse[null] LBRACK) => seqVarUse[xg] LBRACK { xg.append("["); } seqExpression[xg] RBRACK { xg.append("]"); }
	| seqVarUse[xg]
	| seqEntityDecl[xg, true]
	| YIELD { xg.append("yield "); } seqVarUse[xg]
	;

// todo: add expression value returns to remaining sequence expressions,
// as of now only some sequence expressions return an expression
// the expressions are needed for the argument expressions of rule/sequence calls,
// in all other places of the sequences we only need a textual emit of the constructs just parsed
seqExpression[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprLazyOr[xg] { res = exp; }
		( 
			q=QUESTION { xg.append("?"); } op1=seqExpression[xg] COLON { xg.append(":"); } op2=seqExpression[xg]
			{
				OpNode cond=makeOp(q);
				cond.addChild(exp);
				cond.addChild(op1);
				cond.addChild(op2);
				res=cond;
			}
		)?
	;

seqExprLazyOr[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprLazyAnd[xg] { res=exp; } ( t=LOR {xg.append(" || ");} exp2=seqExprLazyOr[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqExprLazyAnd[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprStrictOr[xg] { res=exp; } ( t=LAND {xg.append(" && ");} exp2=seqExprLazyAnd[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqExprStrictOr[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprStrictXor[xg] { res=exp; } ( t=BOR {xg.append(" | ");} exp2=seqExprStrictOr[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqExprStrictXor[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprStrictAnd[xg] { res=exp; } ( t=BXOR {xg.append(" ^ ");} exp2=seqExprStrictXor[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqExprStrictAnd[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprEquality[xg] { res=exp; } ( t=BAND {xg.append(" & ");} exp2=seqExprStrictAnd[xg] { res = makeBinOp(t, exp, exp2); })?
	;
	
seqEqOp[ExecNode xg] returns [ Token t = null ]
	: e=EQUAL {xg.append(" == "); t = e; }
	| n=NOT_EQUAL {xg.append(" != "); t = n; }
	| s=STRUCTURAL_EQUAL {xg.append(" ~~ "); t = s; }
	;

seqExprEquality[ExecNode xg] returns [ExprNode res = env.initExprNode()]
	: exp=seqExprRelation[xg] { res=exp; } ( t=seqEqOp[xg] exp2=seqExprEquality[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqRelOp[ExecNode xg] returns [ Token t = null ]
	: lt=LT {xg.append(" < "); t = lt; }
	| le=LE {xg.append(" <= "); t = le; }
	| gt=GT {xg.append(" > "); t = gt; }
	| ge=GE {xg.append(" >= "); t = ge; }
	| in=IN {xg.append(" in "); t = in; }
	;

seqRelOs[ExecNode xg] returns [ Token t = null ]
	: lt=LT {xg.append(" < "); t = lt; }
	| le=LE {xg.append(" <= "); t = le; }
	| gt=GT {xg.append(" > "); t = gt; }
	| ge=GE {xg.append(" >= "); t = ge; }
	;

seqExprRelation[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprAdd[xg] { res = exp; } 
		( t=seqRelOp[xg] exp2=seqExprRelation[xg] { res = makeBinOp(t, exp, exp2); })?
	;

seqExprAdd[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprMul[xg] { res = exp; } 
		( (t=PLUS {xg.append(" + ");} | t=MINUS {xg.append(" - ");}) exp2=seqExprAdd[xg]  { res = makeBinOp(t, exp, exp2); })?
	;

seqExprMul[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	: exp=seqExprUnary[xg] { res = exp; } 
		( (t=STAR {xg.append(" * ");} | t=DIV {xg.append(" / ");} | t=MOD {xg.append(" \% ");}) exp2=seqExprMul[xg]  { res = makeBinOp(t, exp, exp2); })?
	;

seqExprUnary[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	@init{ Token t = null; }
	: (LPAREN seqTypeIdentUse RPAREN) =>
		p=LPAREN {xg.append("(");} id=seqTypeIdentUse {xg.append(id);} RPAREN {xg.append(")");} op=seqExprBasic[xg]
		{
			res = new CastNode(getCoords(p), id, op);
		}
	| (n=NOT {t=n; xg.append("!");})? exp=seqExprBasic[xg] { if(t!=null) res = makeUnOp(t, exp); else res = exp; }
	| m=MINUS {xg.append("-");} exp=seqExprBasic[xg]
		{
			OpNode neg = new ArithmeticOpNode(getCoords(m), OperatorSignature.NEG);
			neg.addChild(exp);
			res = neg;
		}
	;

// todo: the seqVarUse[xg] casted to IdenNodes might be not simple variable identifiers, but global variables with :: prefix,
//  probably a distinction is needed
seqExprBasic[ExecNode xg] returns[ExprNode res = env.initExprNode()]
options { k = 4; }
	@init{
		CollectNode<BaseNode> returns = new CollectNode<BaseNode>();
		IdentNode id;
	}
	: owner=seqVarUseInExpr[xg] sel=seqExprSelector[owner, xg] { res = sel; }
	| {input.LT(1).getText().equals("this")}? i=IDENT { xg.append("this"); }
	| fc=seqFunctionCall[xg] { res = fc; }
	| DEF LPAREN { xg.append("def("); } seqVariableList[xg, returns] RPAREN { xg.append(")"); } 
	| a=AT LPAREN { xg.append("@("); } 
		(i=IDENT { xg.append(i.getText()); } | s=STRING_LITERAL { xg.append(s.getText()); }) RPAREN { xg.append(")"); }
	| rq=seqRuleQuery[xg, returns] sel=seqExprSelector[rq, xg] { res = sel; }
	| LPAREN { xg.append("("); } seqExpression[xg] RPAREN { xg.append(")"); } 
	| exp=seqConstantWithoutType[xg] { res = (ExprNode)exp; }
	| {env.test(ParserEnvironment.TYPES, input.LT(1).getText()) && !env.test(ParserEnvironment.ENTITIES, input.LT(1).getText())}? i=IDENT
		{
			id = new IdentNode(env.occurs(ParserEnvironment.TYPES, i.getText(), getCoords(i)));
			res = new IdentExprNode(id);
			xg.append(i.getText());
		}
	;

seqVarUseInExpr[ExecNode xg] returns[IdentExprNode res]
	: var=seqVarUse[xg] { res = new IdentExprNode(var); }
	;

seqExprSelector[ExprNode prefix, ExecNode xg] returns[ExprNode res = prefix]
options { k = 3; }
	@init{
		CollectNode<ExprNode> arguments = new CollectNode<ExprNode>();
	}
	: DOT method=IDENT LPAREN { xg.append("."+method.getText()+"("); } 
		( arg=seqExpression[xg] { arguments.addChild(arg); }
			(COMMA { xg.append(","); } arg=seqExpression[xg] { arguments.addChild(arg); })*
			)? RPAREN { xg.append(")"); }
		{ res = new MethodInvocationExprNode(prefix, new IdentNode(env.occurs(ParserEnvironment.ENTITIES, method.getText(), getCoords(method))), arguments, null); }
	| d=DOT attr=seqMemberIdentUse { xg.append("."+attr.getSymbol().getText()); }
			{ res = new MemberAccessExprNode(getCoords(d), prefix, attr); }
		sel=seqExprSelector[res, xg] { res = sel; }
	| DOT VISITED LBRACK 
		{ xg.append(".visited["); } seqExpression[xg] RBRACK { xg.append("]"); } // TODO: visited expr
	| l=LBRACK { xg.append("["); } key=seqExpression[xg] RBRACK { xg.append("]"); }
			{ res = new IndexedAccessExprNode(getCoords(l), prefix, key); } // array/deque/map access
	| // no selector
	;
	
seqProcedureCall[ExecNode xg]
	@init{
		CollectNode<BaseNode> returns = new CollectNode<BaseNode>();
	}
	// built-in procedure or user defined procedure, backend has to decide whether the call is valid
	: ( LPAREN {xg.append("(");} seqVariableList[xg, returns] RPAREN ASSIGN {xg.append(")=");} )?
		( p=IDENT DOUBLECOLON { xg.append(p.getText()); xg.append("::"); } )?
		( i=IDENT | i=EMIT | i=EMITDEBUG | i=DELETE) LPAREN { xg.append(i.getText()); xg.append("("); } 
			seqFunctionCallParameters[xg] RPAREN { xg.append(")"); }
	;

seqFunctionCall[ExecNode xg] returns[ExprNode res = env.initExprNode()]
	@init{
		boolean inPackage = false;
	}
	// built-in function or user defined function, backend has to decide whether the call is valid
	: ( p=IDENT DOUBLECOLON { xg.append(p.getText()); xg.append("::"); } )?
	  ( i=IDENT | i=COPY | i=NAMEOF | i=TYPEOF ) LPAREN { xg.append(i.getText()); xg.append("("); } params=seqFunctionCallParameters[xg] RPAREN { xg.append(")"); }
		{
			if( (i.getText().equals("now")) && params.getChildren().size()==0
				|| (i.getText().equals("nodes") || i.getText().equals("edges")) && params.getChildren().size()<=1
				|| (i.getText().equals("countNodes") || i.getText().equals("countEdges")) && params.getChildren().size()<=1
				|| (i.getText().equals("empty") || i.getText().equals("size")) && params.getChildren().size()==0
				|| (i.getText().equals("source") || i.getText().equals("target")) && params.getChildren().size()==1
				|| i.getText().equals("opposite") && params.getChildren().size()==2
				|| (i.getText().equals("nodeByName") || i.getText().equals("edgeByName")) && params.getChildren().size()>=1 && params.getChildren().size()<=2
				|| (i.getText().equals("nodeByUnique") || i.getText().equals("edgeByUnique")) && params.getChildren().size()>=1 && params.getChildren().size()<=2
				|| (i.getText().equals("incoming") || i.getText().equals("outgoing") || i.getText().equals("incident")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("adjacentIncoming") || i.getText().equals("adjacentOutgoing") || i.getText().equals("adjacent")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("reachableIncoming") || i.getText().equals("reachableOutgoing") || i.getText().equals("reachable")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("reachableEdgesIncoming") || i.getText().equals("reachableEdgesOutgoing") || i.getText().equals("reachableEdges")) && params.getChildren().size()>=1 && params.getChildren().size()<=3 
				|| (i.getText().equals("boundedReachableIncoming") || i.getText().equals("boundedReachableOutgoing") || i.getText().equals("boundedReachable")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("boundedReachableEdgesIncoming") || i.getText().equals("boundedReachableEdgesOutgoing") || i.getText().equals("boundedReachableEdges")) && params.getChildren().size()>=2 && params.getChildren().size()<=4 
				|| (i.getText().equals("boundedReachableWithRemainingDepthIncoming") || i.getText().equals("boundedReachableWithRemainingDepthOutgoing") || i.getText().equals("boundedReachableWithRemainingDepth")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("countIncoming") || i.getText().equals("countOutgoing") || i.getText().equals("countIncident")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("countAdjacentIncoming") || i.getText().equals("countAdjacentOutgoing") || i.getText().equals("countAdjacent")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("countReachableIncoming") || i.getText().equals("countReachableOutgoing") || i.getText().equals("countReachable")) && params.getChildren().size()>=1 && params.getChildren().size()<=3
				|| (i.getText().equals("countReachableEdgesIncoming") || i.getText().equals("countReachableEdgesOutgoing") || i.getText().equals("countReachableEdges")) && params.getChildren().size()>=1 && params.getChildren().size()<=3 
				|| (i.getText().equals("countBoundedReachableIncoming") || i.getText().equals("countBoundedReachableOutgoing") || i.getText().equals("countBoundedReachable")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("countBoundedReachableEdgesIncoming") || i.getText().equals("countBoundedReachableEdgesOutgoing") || i.getText().equals("countBoundedReachableEdges")) && params.getChildren().size()>=2 && params.getChildren().size()<=4 
				|| (i.getText().equals("isIncoming") || i.getText().equals("isOutgoing") || i.getText().equals("isIncident")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("isAdjacentIncoming") || i.getText().equals("isAdjacentOutgoing") || i.getText().equals("isAdjacent")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("isReachableIncoming") || i.getText().equals("isReachableOutgoing") || i.getText().equals("isReachable")) && params.getChildren().size()>=2 && params.getChildren().size()<=4
				|| (i.getText().equals("isReachableEdgesIncoming") || i.getText().equals("isReachableEdgesOutgoing") || i.getText().equals("isReachableEdges")) && params.getChildren().size()>=2 && params.getChildren().size()<=4 
				|| (i.getText().equals("isBoundedReachableIncoming") || i.getText().equals("isBoundedReachableOutgoing") || i.getText().equals("isBoundedReachable")) && params.getChildren().size()>=3 && params.getChildren().size()<=5
				|| (i.getText().equals("isBoundedReachableEdgesIncoming") || i.getText().equals("isBoundedReachableEdgesOutgoing") || i.getText().equals("isBoundedReachableEdges")) && params.getChildren().size()>=3 && params.getChildren().size()<=5 
				|| i.getText().equals("random") && params.getChildren().size()>=0 && params.getChildren().size()<=1
				|| i.getText().equals("canonize") && params.getChildren().size()==1
				|| (i.getText().equals("inducedSubgraph") || i.getText().equals("definedSubgraph")) && params.getChildren().size()==1
				|| (i.getText().equals("equalsAny") || i.getText().equals("equalsAnyStructurally")) && params.getChildren().size()==2
				|| (i.getText().equals("exists") || i.getText().equals("import")) && params.getChildren().size()==1
				|| i.getText().equals("copy") && params.getChildren().size()==1
				|| i.getText().equals("nameof") && (params.getChildren().size()==1 || params.getChildren().size()==0)
				|| i.getText().equals("uniqueof") && (params.getChildren().size()==1 || params.getChildren().size()==0)
			  )
			{
				IdentNode funcIdent = new IdentNode(env.occurs(ParserEnvironment.FUNCTIONS_AND_EXTERNAL_FUNCTIONS, i.getText(), getCoords(i)));
				res = new FunctionInvocationExprNode(funcIdent, params, env);
			} else {
				IdentNode funcIdent = inPackage ? 
					new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
						env.occurs(ParserEnvironment.FUNCTIONS_AND_EXTERNAL_FUNCTIONS, i.getText(), getCoords(i)))
					: new IdentNode(env.occurs(ParserEnvironment.FUNCTIONS_AND_EXTERNAL_FUNCTIONS, i.getText(), getCoords(i)));
				res = new FunctionOrExternalFunctionInvocationExprNode(funcIdent, params);
			}
		}
	;

seqFunctionCallParameters[ExecNode xg] returns [ CollectNode<ExprNode> params = new CollectNode<ExprNode>(); ]
	: (fromExpr=seqExpression[xg] { params.addChild(fromExpr); }
		(COMMA { xg.append(","); } fromExpr2=seqExpression[xg] { params.addChild(fromExpr2); } )* )?
	;

seqConstant[ExecNode xg] returns[ExprNode res = env.initExprNode()]
@init{ IdentNode id; }
	: seqConstantWithoutType[xg]
	| {env.test(ParserEnvironment.TYPES, input.LT(1).getText()) && !env.test(ParserEnvironment.ENTITIES, input.LT(1).getText())}? i=IDENT
		{
			id = new IdentNode(env.occurs(ParserEnvironment.TYPES, i.getText(), getCoords(i)));
			res = new IdentExprNode(id);
			xg.append(i.getText());
		}
	;
	
seqConstantWithoutType[ExecNode xg] returns[ExprNode res = env.initExprNode()]
options { k = 4; }
@init{ IdentNode id; }
	: b=NUM_BYTE { xg.append(b.getText()); res = new ByteConstNode(getCoords(b), Byte.parseByte(ByteConstNode.removeSuffix(b.getText()), 10)); }
	| sh=NUM_SHORT { xg.append(sh.getText()); res = new ShortConstNode(getCoords(sh), Short.parseShort(ShortConstNode.removeSuffix(sh.getText()), 10)); }
	| i=NUM_INTEGER { xg.append(i.getText()); res = new IntConstNode(getCoords(i), Integer.parseInt(i.getText(), 10)); }
	| l=NUM_LONG { xg.append(l.getText()); res = new LongConstNode(getCoords(l), Long.parseLong(LongConstNode.removeSuffix(l.getText()), 10)); }
	| f=NUM_FLOAT { xg.append(f.getText()); res = new FloatConstNode(getCoords(f), Float.parseFloat(f.getText())); }
	| d=NUM_DOUBLE { xg.append(d.getText()); res = new DoubleConstNode(getCoords(d), Double.parseDouble(d.getText())); }
	| s=STRING_LITERAL { xg.append(s.getText()); String buff = s.getText();
			// Strip the " from the string
			buff = buff.substring(1, buff.length() - 1);
			res = new StringConstNode(getCoords(s), buff); }
	| tt=TRUE { xg.append(tt.getText()); res = new BoolConstNode(getCoords(tt), true); }
	| ff=FALSE { xg.append(ff.getText()); res = new BoolConstNode(getCoords(ff), false); }
	| n=NULL { xg.append(n.getText()); res = new NullConstNode(getCoords(n)); }
	| MAP LT typeName=seqTypeIdentUse COMMA toTypeName=seqTypeIdentUse GT { xg.append("map<"+typeName+","+toTypeName+">"); } 
		e1=seqInitMapExpr[xg, MapTypeNode.getMapType(typeName, toTypeName)] { res = e1; }
	| SET LT typeName=seqTypeIdentUse GT { xg.append("set<"+typeName+">"); } 
		e2=seqInitSetExpr[xg, SetTypeNode.getSetType(typeName)] { res = e2; }
	| ARRAY LT typeName=seqTypeIdentUse GT { xg.append("array<"+typeName+">"); } 
		e3=seqInitArrayExpr[xg, ArrayTypeNode.getArrayType(typeName)] { res = e3; }
	| DEQUE LT typeName=seqTypeIdentUse GT { xg.append("deque<"+typeName+">"); } 
		e4=seqInitDequeExpr[xg, DequeTypeNode.getDequeType(typeName)] { res = e4; }
	| pen=IDENT d=DOUBLECOLON i=IDENT 
		{
			if(env.test(ParserEnvironment.PACKAGES, pen.getText()) || !env.test(ParserEnvironment.TYPES, pen.getText())) {
				id = new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, pen.getText(), getCoords(pen)), 
					env.occurs(ParserEnvironment.TYPES, i.getText(), getCoords(i)));
				res = new IdentExprNode(id);
			} else {
				res = new DeclExprNode(new EnumExprNode(getCoords(d), 
					new IdentNode(env.occurs(ParserEnvironment.TYPES, pen.getText(), getCoords(pen))),
					new IdentNode(env.occurs(ParserEnvironment.ENTITIES, i.getText(), getCoords(i)))));
			}
			xg.append(pen.getText() + "::" + i.getText());
		}
	| p=IDENT DOUBLECOLON en=IDENT d=DOUBLECOLON i=IDENT
		{
			res = new DeclExprNode(new EnumExprNode(getCoords(d), 
				new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)),
					env.occurs(ParserEnvironment.TYPES, en.getText(), getCoords(en))),
				new IdentNode(env.occurs(ParserEnvironment.ENTITIES, i.getText(), getCoords(i)))));
			xg.append(p.getText() + "::" + en.getText() + "::" + i.getText());
		}
	;

seqInitMapExpr [ExecNode xg, MapTypeNode mapType] returns [ ExprNode res = null ]
	@init{ MapInitNode mapInit = null; }
	: l=LBRACE { xg.append("{"); } { res = mapInit = new MapInitNode(getCoords(l), null, mapType); }
		( item1=seqMapItem[xg] { mapInit.addMapItem(item1); }
			( COMMA { xg.append(","); } item2=seqMapItem[xg] { mapInit.addMapItem(item2); } )*
		)?
	  RBRACE { xg.append("}"); }
	/*| l=LPAREN { xg.append("("); } value=seqExpression[xg]
		{ res = new MapCopyConstructorNode(getCoords(l), null, mapType, value); }
	  RPAREN { xg.append(")"); }*/
	;

seqInitSetExpr [ExecNode xg, SetTypeNode setType] returns [ ExprNode res = null ]
	@init{ SetInitNode setInit = null; }
	: l=LBRACE { xg.append("{"); } { res = setInit = new SetInitNode(getCoords(l), null, setType); }	
		( item1=seqSetItem[xg] { setInit.addSetItem(item1); }
			( COMMA { xg.append(","); } item2=seqSetItem[xg] { setInit.addSetItem(item2); } )*
		)?
	  RBRACE { xg.append("}"); }
	| l=LPAREN { xg.append("("); } value=seqExpression[xg]
		{ res = new SetCopyConstructorNode(getCoords(l), null, setType, value); }
	  RPAREN { xg.append(")"); }
	;

seqInitArrayExpr [ExecNode xg, ArrayTypeNode arrayType] returns [ ExprNode res = null ]
	@init{ ArrayInitNode arrayInit = null; }
	: l=LBRACK { xg.append("["); } { res = arrayInit = new ArrayInitNode(getCoords(l), null, arrayType); }	
		( item1=seqArrayItem[xg] { arrayInit.addArrayItem(item1); }
			( COMMA { xg.append(","); } item2=seqArrayItem[xg] { arrayInit.addArrayItem(item2); } )*
		)?
	  RBRACK { xg.append("]"); }
	/*| l=LPAREN { xg.append("("); } value=seqExpression[xg]
		{ res = new ArrayCopyConstructorNode(getCoords(l), null, arrayType, value); }
	  RPAREN { xg.append(")"); }*/
	;

seqInitDequeExpr [ExecNode xg, DequeTypeNode dequeType] returns [ ExprNode res = null ]
	@init{ DequeInitNode dequeInit = null; }
	: l=LBRACK { xg.append("["); } { res = dequeInit = new DequeInitNode(getCoords(l), null, dequeType); }	
		( item1=seqDequeItem[xg] { dequeInit.addDequeItem(item1); }
			( COMMA { xg.append(","); } item2=seqDequeItem[xg] { dequeInit.addDequeItem(item2); } )*
		)?
	  RBRACK { xg.append("]"); }
	/*| l=LPAREN { xg.append("("); } value=seqExpression[xg]
		{ res = new DequeCopyConstructorNode(getCoords(l), null, dequeType, value); }
	  RPAREN { xg.append(")"); }*/
	;

seqMapItem [ExecNode xg] returns [ MapItemNode res = null ]
	: key=seqExpression[xg] a=RARROW { xg.append("->"); } value=seqExpression[xg]
		{
			res = new MapItemNode(getCoords(a), key, value);
		}
	;

seqSetItem [ExecNode xg] returns [ SetItemNode res = null ]
	: value=seqExpression[xg]
		{
			res = new SetItemNode(value.getCoords(), value);
		}
	;

seqArrayItem [ExecNode xg] returns [ ArrayItemNode res = null ]
	: value=seqExpression[xg]
		{
			res = new ArrayItemNode(value.getCoords(), value);
		}
	;

seqDequeItem [ExecNode xg] returns [ DequeItemNode res = null ]
	: value=seqExpression[xg]
		{
			res = new DequeItemNode(value.getCoords(), value);
		}
	;

seqMultiRulePrefixedSequence[ExecNode xg, CollectNode<BaseNode> returns]
	@init{
		CollectNode<CallActionNode> ruleCalls = new CollectNode<CallActionNode>();
		CollectNode<BaseNode> filters = new CollectNode<BaseNode>();
	}

	: l=LBRACK LBRACK {xg.append("[[");} 
		seqRulePrefixedSequenceAtom[xg, ruleCalls, returns]
		( COMMA { xg.append(","); returns = new CollectNode<BaseNode>(); } seqRulePrefixedSequenceAtom[xg, ruleCalls, returns] )*
	  RBRACK RBRACK {xg.append("]]");}
	  (seqCallRuleFilter[xg, filters, true])*
		{
			xg.addMultiCallAction(new MultiCallActionNode(getCoords(l), ruleCalls, filters));
		}
	;

seqRulePrefixedSequence[ExecNode xg, CollectNode<BaseNode> returns]
	: LBRACK {xg.append("[");} seqRulePrefixedSequenceAtom[xg, null, returns] RBRACK {xg.append("]");}
	;

seqRulePrefixedSequenceAtom[ExecNode xg, CollectNode<CallActionNode> ruleCalls, CollectNode<BaseNode> returns]
	: FOR l=LBRACE { env.pushScope("ruleprefixedsequence/exec", getCoords(l)); } {xg.append("for{");} 
		seqCallRuleWithOptionalReturns[xg, ruleCalls, returns, false] SEMI {xg.append(";");}
			sequence[xg] { env.popScope(); } RBRACE {xg.append("}");}
	;

seqMultiRuleAllCall[ExecNode xg, CollectNode<BaseNode> returns, boolean isAllBracketed]
	@init{
		CollectNode<CallActionNode> ruleCalls = new CollectNode<CallActionNode>();
		CollectNode<BaseNode> filters = new CollectNode<BaseNode>();
	}

	: l=LBRACK LBRACK {xg.append("[[");} 
		seqCallRuleWithOptionalReturns[xg, ruleCalls, returns, isAllBracketed]
		( COMMA { xg.append(","); returns = new CollectNode<BaseNode>(); } seqCallRuleWithOptionalReturns[xg, ruleCalls, returns, isAllBracketed] )*
	  RBRACK RBRACK {xg.append("]]");}
	  (seqCallRuleFilter[xg, filters, true])*
		{
			xg.addMultiCallAction(new MultiCallActionNode(getCoords(l), ruleCalls, filters));
		}
	;
	
seqParallelCallRule[ExecNode xg, CollectNode<BaseNode> returns]
	: ( LPAREN {xg.append("(");} seqVariableList[xg, returns] RPAREN ASSIGN {xg.append(")=");} )?
		(	( DOLLAR {xg.append("$");} (MOD { xg.append("\%"); })? ( seqVarUse[xg] 
						(COMMA {xg.append(",");} (seqVarUse[xg] | STAR {xg.append("*");}))? )? )?
				LBRACK {xg.append("[");} 
				seqCallRule[xg, null, returns, true]
				RBRACK {xg.append("]");}
		| 
			COUNT {xg.append("count");}
				LBRACK {xg.append("[");} 
				seqCallRule[xg, null, returns, true]
				RBRACK {xg.append("]");}
		|
			seqCallRule[xg, null, returns, false]
		)
	;

seqRuleQuery[ExecNode xg, CollectNode<BaseNode> returns] returns[ExprNode res = env.initExprNode()]
	: LBRACK {xg.append("[");} 
		cre=seqCallRuleExpression[xg, null, returns, true] { res = cre; }
		RBRACK {xg.append("]");}
	;

seqCallRuleWithOptionalReturns[ExecNode xg, CollectNode<CallActionNode> ruleCalls, CollectNode<BaseNode> returns, boolean isAllBracketed]
	: (LPAREN {xg.append("(");} seqVariableList[xg, returns] RPAREN ASSIGN {xg.append(")=");})? seqCallRule[xg, ruleCalls, returns, isAllBracketed]
	;

seqCallRule[ExecNode xg, CollectNode<CallActionNode> ruleCalls, CollectNode<BaseNode> returns, boolean isAllBracketed]
	@init{
		CollectNode<BaseNode> params = new CollectNode<BaseNode>();
		CollectNode<BaseNode> filters = new CollectNode<BaseNode>();
	}
	
	: ( | MOD { xg.append("\%"); } | MOD QUESTION { xg.append("\%?"); } | QUESTION { xg.append("?"); } | QUESTION MOD { xg.append("?\%"); } )
		(seqVarUse[xg] DOT {xg.append(".");})?
		id=seqActionOrEntIdentUse {xg.append(id);}
		(LPAREN {xg.append("(");} (seqRuleParams[xg, params])? RPAREN {xg.append(")");})?
		(seqCallRuleFilter[xg, filters, false])*
		{
			CallActionNode ruleCall = new CallActionNode(id.getCoords(), id, params, returns, filters, isAllBracketed);
			xg.addCallAction(ruleCall);
			if(ruleCalls != null) { // must be added to MultiCallActionNode if used from multi rule all call or multi backtrack construct
				ruleCalls.addChild(ruleCall);
			}
		}
	;

seqCallRuleExpression[ExecNode xg, CollectNode<CallActionNode> ruleCalls, CollectNode<BaseNode> returns, boolean isAllBracketed]
		returns[ExprNode res = env.initExprNode()]
	@init{
		CollectNode<BaseNode> params = new CollectNode<BaseNode>();
		CollectNode<BaseNode> filters = new CollectNode<BaseNode>();
	}
	
	: ( QUESTION MOD { xg.append("?\%"); } | MOD QUESTION { xg.append("\%?"); } | QUESTION { xg.append("?"); } )
		(seqVarUse[xg] DOT {xg.append(".");})?
		id=seqActionOrEntIdentUse {xg.append(id);}
		(LPAREN {xg.append("(");} (seqRuleParams[xg, params])? RPAREN {xg.append(")");})?
		(seqCallRuleFilter[xg, filters, false])*
		{
			CallActionNode ruleCall = new CallActionNode(id.getCoords(), id, params, returns, filters, isAllBracketed);
			xg.addCallAction(ruleCall);
			if(ruleCalls != null) { // must be added to MultiCallActionNode if used from multi rule all call or multi backtrack construct
				ruleCalls.addChild(ruleCall);
			}
			res = new RuleQueryExprNode(id.getCoords(), ruleCall, new ArrayTypeNode(MatchTypeNode.getMatchTypeIdentNode(env, id)));
		}
	;

seqCallRuleFilter[ExecNode xg, CollectNode<BaseNode> filters, boolean isMatchClassFilter]
	: BACKSLASH { xg.append("\\"); } (p=IDENT DOUBLECOLON { xg.append(p.getText()); xg.append("::"); })? id=IDENT { xg.append(id.getText()); } 
		seqCallRuleFilterContinuation[xg, filters, isMatchClassFilter, p, id]
	;

seqCallRuleFilterContinuation[ExecNode xg, CollectNode<BaseNode> filters, boolean isMatchClassFilter, Token pin, Token idin]
	@init{
		CollectNode<BaseNode> params = new CollectNode<BaseNode>();
	}
	: DOT { xg.append("."); } seqCallMatchClassRuleFilterContinuation[xg, filters, isMatchClassFilter, pin, idin]
	| (LPAREN {xg.append("(");} (seqRuleParams[xg, params])? RPAREN {xg.append(")");})?
		{
			Token p = pin;
			Token filterId = idin;

			if(isMatchClassFilter)
				reportError(getCoords(filterId), "A match class specifier is required for filters of multi rule call or multi rule backtracking constructs.");

			if(filterId.getText().equals("keepFirst") || filterId.getText().equals("keepLast")
				|| filterId.getText().equals("removeFirst") || filterId.getText().equals("removeLast")
				|| filterId.getText().equals("keepFirstFraction") || filterId.getText().equals("keepLastFraction")
				|| filterId.getText().equals("removeFirstFraction") || filterId.getText().equals("removeLastFraction"))
			{
				if(params.size()!=1)
					reportError(getCoords(filterId), "The filter " + filterId.getText() + " expects 1 arguments.");
			}
			else if(filterId.getText().equals("auto"))
			{
				if(isMatchClassFilter)
					reportError(getCoords(filterId), "The auto filter is not available for multi rule call or multi rule backtracking constructs.");
				if(params.size()!=0)
					reportError(getCoords(filterId), "The filter " + filterId.getText() + " expects 0 arguments.");
			}
			else
			{
				IdentNode filter = p != null ? new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
														env.occurs(ParserEnvironment.ACTIONS, filterId.getText(), getCoords(filterId)))
												: new IdentNode(env.occurs(ParserEnvironment.ACTIONS, filterId.getText(), getCoords(filterId)));
				filters.addChild(filter);
			}
		}
	| LT { xg.append("<"); } seqFilterCallVariableList[xg] GT { xg.append("> "); }
		{
			Token p = pin;
			Token filterBase = idin;

			if(p != null)
				reportError(getCoords(filterBase), "No package specifier allowed for auto-generated filters.");
			if(isMatchClassFilter)
				reportError(getCoords(filterBase), "A match class specifier is required for filters of multi rule call or multi rule backtracking constructs.");

			if(!filterBase.getText().equals("orderAscendingBy") && !filterBase.getText().equals("orderDescendingBy") && !filterBase.getText().equals("groupBy")
				&& !filterBase.getText().equals("keepSameAsFirst") && !filterBase.getText().equals("keepSameAsLast") && !filterBase.getText().equals("keepOneForEach"))
			{
				reportError(getCoords(filterBase), "Unknown def-variable-based filter " + filterBase.getText() + "! Available are: orderAscendingBy, orderDescendingBy, groupBy, keepSameAsFirst, keepSameAsLast, keepOneForEach.");
			}
		}
	;

seqCallMatchClassRuleFilterContinuation[ExecNode xg, CollectNode<BaseNode> filters, boolean isMatchClassFilter, Token pmc, Token mc]
	@init{
		CollectNode<BaseNode> params = new CollectNode<BaseNode>();
	}
	: (p=IDENT DOUBLECOLON { xg.append(p.getText()); xg.append("::"); })? filterId=IDENT { xg.append(filterId.getText()); } 
		(LPAREN {xg.append("(");} (seqRuleParams[xg, params])? RPAREN {xg.append(")");})?
		{
			if(!isMatchClassFilter)
				reportError(getCoords(mc), "A match class specifier is only admissible for filters of multi rule call or multi rule backtracking constructs.");

			if(filterId.getText().equals("keepFirst") || filterId.getText().equals("keepLast")
				|| filterId.getText().equals("removeFirst") || filterId.getText().equals("removeLast")
				|| filterId.getText().equals("keepFirstFraction") || filterId.getText().equals("keepLastFraction")
				|| filterId.getText().equals("removeFirstFraction") || filterId.getText().equals("removeLastFraction"))
			{
				if(params.size()!=1)
					reportError(getCoords(filterId), "The filter " + filterId.getText() + " expects 1 arguments.");
			}
			else
			{
				IdentNode matchClass = pmc != null ? new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, pmc.getText(), getCoords(pmc)), 
														env.occurs(ParserEnvironment.TYPES, mc.getText(), getCoords(mc)))
													: new IdentNode(env.occurs(ParserEnvironment.TYPES, mc.getText(), getCoords(mc)));
				IdentNode matchClassFilter = p != null ? new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
														env.occurs(ParserEnvironment.ACTIONS, filterId.getText(), getCoords(filterId)))
													: new IdentNode(env.occurs(ParserEnvironment.ACTIONS, filterId.getText(), getCoords(filterId)));															
				filters.addChild(new MatchTypeQualIdentNode(getCoords(filterId), matchClass, matchClassFilter));
			}
		}
	| filterBase=IDENT LT { xg.append(filterBase.getText() + "<"); } seqFilterCallVariableList[xg] GT { xg.append("> "); }
		{
			if(!isMatchClassFilter)
				reportError(getCoords(mc), "A match class specifier is only admissible for filters of multi rule call or multi rule backtracking constructs.");

			if(!filterBase.getText().equals("orderAscendingBy") && !filterBase.getText().equals("orderDescendingBy") && !filterBase.getText().equals("groupBy")
				&& !filterBase.getText().equals("keepSameAsFirst") && !filterBase.getText().equals("keepSameAsLast") && !filterBase.getText().equals("keepOneForEach"))
			{
				reportError(getCoords(filterBase), "Unknown def-variable-based filter " + filterBase.getText() + "! Available are: orderAscendingBy, orderDescendingBy, groupBy, keepSameAsFirst, keepSameAsLast, keepOneForEach.");
			}
		}
	;

seqFilterCallVariableList[ExecNode xg]
	: filterVariable=IDENT { xg.append(filterVariable.getText()); }
		( COMMA {xg.append(",");} filterVariable=IDENT { xg.append(filterVariable.getText()); } )*
	;

seqRuleParam[ExecNode xg, CollectNode<BaseNode> parameters]
	: exp=seqExpression[xg] { parameters.addChild(exp); }
	;

seqRuleParams[ExecNode xg, CollectNode<BaseNode> parameters]
	: seqRuleParam[xg, parameters]	( COMMA {xg.append(",");} seqRuleParam[xg, parameters] )*
	;

seqVariableList[ExecNode xg, CollectNode<BaseNode> res]
	: child=seqEntity[xg] { res.addChild(child); }
		( COMMA { xg.append(","); } child=seqEntity[xg] { res.addChild(child); } )*
	;

seqVarUse[ExecNode xg] returns [IdentNode res = null]
	:
		id=seqEntIdentUse { res = id; xg.append(id); xg.addUsage(id); } // var of node, edge, or basic type
	|
		DOUBLECOLON id=seqEntIdentUse { res = id; xg.append("::" + id); xg.addUsage(id); } // global var of node, edge, or basic type		 
	;

seqEntity[ExecNode xg] returns [BaseNode res = null]
	:
		varUse=seqVarUse[xg] { res = varUse; }
	|
		seqVarDecl=seqEntityDecl[xg, true] { res = seqVarDecl; }
	;

seqEntityDecl[ExecNode xg, boolean emit] returns [ExecVarDeclNode res = null]
options { k = *; }
	:
		id=seqEntIdentDecl COLON type=seqTypeIdentUse // node decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, type);
			if(emit) xg.append(id.toString()+":"+type.toString());
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON MAP LT keyType=seqTypeIdentUse COMMA valueType=seqTypeIdentUse GT // map decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, MapTypeNode.getMapType(keyType, valueType));
			if(emit) xg.append(id.toString()+":map<"+keyType.toString()+","+valueType.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		(seqEntIdentDecl COLON MAP LT seqTypeIdentUse COMMA seqTypeIdentUse GE) =>
		id=seqEntIdentDecl COLON MAP LT keyType=seqTypeIdentUse COMMA valueType=seqTypeIdentUse // map decl; special to save user from splitting map<S,T>=x to map<S,T> =x as >= is GE not GT ASSIGN
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, MapTypeNode.getMapType(keyType, valueType));
			if(emit) xg.append(id.toString()+":map<"+keyType.toString()+","+valueType.toString());
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON SET LT type=seqTypeIdentUse GT // set decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, SetTypeNode.getSetType(type));
			if(emit) xg.append(id.toString()+":set<"+type.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		(seqEntIdentDecl COLON SET LT seqTypeIdentUse GE) => 
		id=seqEntIdentDecl COLON SET LT type=seqTypeIdentUse // set decl; special to save user from splitting set<S>=x to set<S> =x as >= is GE not GT ASSIGN
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, SetTypeNode.getSetType(type));
			if(emit) xg.append(id.toString()+":set<"+type.toString());
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON ARRAY LT type=seqTypeIdentUse GT // array decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, ArrayTypeNode.getArrayType(type));
			if(emit) xg.append(id.toString()+":array<"+type.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		(seqEntIdentDecl COLON ARRAY LT seqTypeIdentUse GE) => 
		id=seqEntIdentDecl COLON ARRAY LT type=seqTypeIdentUse // array decl; special to save user from splitting array<S>=x to array<S> =x as >= is GE not GT ASSIGN
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, ArrayTypeNode.getArrayType(type));
			if(emit) xg.append(id.toString()+":array<"+type.toString());
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON DEQUE LT type=seqTypeIdentUse GT // deque decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, DequeTypeNode.getDequeType(type));
			if(emit) xg.append(id.toString()+":deque<"+type.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		(seqEntIdentDecl COLON DEQUE LT seqTypeIdentUse GE) => 
		id=seqEntIdentDecl COLON DEQUE LT type=seqTypeIdentUse // deque decl; special to save user from splitting deque<S>=x to deque<S> =x as >= is GE not GT ASSIGN
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, DequeTypeNode.getDequeType(type));
			if(emit) xg.append(id.toString()+":deque<"+type.toString());
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON MATCH LT actionIdent=seqActionIdentUse GT // match decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, MatchTypeNode.getMatchTypeIdentNode(env, actionIdent));
			if(emit) xg.append(id.toString()+":match<"+actionIdent.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		id=seqEntIdentDecl COLON MATCH LT CLASS matchClassIdent=seqTypeIdentUse GT // match class decl
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, matchClassIdent);
			if(emit) xg.append(id.toString()+":match<class "+matchClassIdent.toString()+">");
			xg.addVarDecl(decl);
			res = decl;
		}
	|
		MINUS id=seqEntIdentDecl COLON type=seqTypeIdentUse RARROW // edge decl, interpreted grs don't use -:-> form
		{
			ExecVarDeclNode decl = new ExecVarDeclNode(id, type);
			if(emit) xg.append(decl.getIdentNode().getIdent() + ":" + decl.typeUnresolved);
			xg.addVarDecl(decl);
			res = decl;
		}
	;

seqIndex[ExecNode xg]
	: id=seqIndexIdentUse { xg.append(id.toString()); }
	;

seqEntIdentDecl returns [ IdentNode res = env.getDummyIdent() ]
	: i=IDENT 
		{ if(i!=null) res = new IdentNode(env.define(ParserEnvironment.ENTITIES, i.getText(), getCoords(i))); }
	;

seqTypeIdentUse returns [ IdentNode res = env.getDummyIdent() ]
options { k = 3; }
	: i=IDENT 
		{ if(i!=null) res = new IdentNode(env.occurs(ParserEnvironment.TYPES, i.getText(), getCoords(i))); }
	| p=IDENT DOUBLECOLON i=IDENT 
		{ if(i!=null) res = new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
				env.occurs(ParserEnvironment.TYPES, i.getText(), getCoords(i))); }
	;

seqEntIdentUse returns [ IdentNode res = env.getDummyIdent() ]
	: i=IDENT
		{ if(i!=null) res = new IdentNode(env.occurs(ParserEnvironment.ENTITIES, i.getText(), getCoords(i))); }
	;

seqActionIdentUse returns [ IdentNode res = env.getDummyIdent() ]
options { k = 3; }
	: i=IDENT
		{ if(i!=null) res = new IdentNode(env.occurs(ParserEnvironment.ACTIONS, i.getText(), getCoords(i))); }
	| p=IDENT DOUBLECOLON i=IDENT 
		{ if(i!=null) res = new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
				env.occurs(ParserEnvironment.ACTIONS, i.getText(), getCoords(i))); }
	;

seqActionOrEntIdentUse returns [ IdentNode res = env.getDummyIdent() ]
options { k = 3; }
	: i=IDENT
		{ if(i!=null) res = new AmbiguousIdentNode(env.occurs(ParserEnvironment.ACTIONS,
			i.getText(), getCoords(i)), env.occurs(ParserEnvironment.ENTITIES, i.getText(), getCoords(i))); }
	| p=IDENT DOUBLECOLON i=IDENT 
		{ if(i!=null) res = new PackageIdentNode(env.occurs(ParserEnvironment.PACKAGES, p.getText(), getCoords(p)), 
				env.occurs(ParserEnvironment.ACTIONS, i.getText(), getCoords(i))); }
	;

seqIndexIdentUse returns [ IdentNode res = env.getDummyIdent() ]
	: i=IDENT
		{ if(i!=null) res = new IdentNode(env.occurs(ParserEnvironment.INDICES, i.getText(), getCoords(i))); }
	;

seqMemberIdentUse returns [ IdentNode res = env.getDummyIdent() ]
	: i=IDENT
		{ if(i!=null) res = new IdentNode(env.occurs(ParserEnvironment.ENTITIES, i.getText(), getCoords(i))); }
	| r=REPLACE
		{ if(r!=null) r.setType(IDENT); res = new IdentNode(env.occurs(ParserEnvironment.ENTITIES,
			r.getText(), getCoords(r))); } // HACK: For string replace function... better choose another name?
	;

seqRangeSpecLoop returns [ RangeSpecNode res = null ]
	@init{
		lower = 1; upper = 1;
		de.unika.ipd.grgen.parser.Coords coords = de.unika.ipd.grgen.parser.Coords.getInvalid();
		// range allows [*], [+], [c:*], [c], [c:d]; no range equals 1:1
	}
	:
		(
			l=LBRACK { coords = getCoords(l); }
			(
				STAR { lower=0; upper=RangeSpecNode.UNBOUND; }
			|
				PLUS { lower=1; upper=RangeSpecNode.UNBOUND; }
			|
				lower=seqIntegerConst
				(
					COLON ( STAR { upper=RangeSpecNode.UNBOUND; } | upper=seqIntegerConst )
				|
					{ upper = lower; }
				)
			)
			RBRACK
		)?
		{ res = new RangeSpecNode(coords, lower, upper); }
	;

seqIntegerConst returns [ long value = 0 ]
	: i=NUM_INTEGER
		{ value = Long.parseLong(i.getText()); }
	;
