header {
/*
 * GrGen: graph rewrite generator, compiling declarative graph rewrite rules into executable code
 * Copyright (C) 2005 Universitaet Karlsruhe, Institut fuer Programmstrukturen und Datenorganisation, LS Goos
 * licensed under GPL v3 (see LICENSE.txt included in the packaging of this file)
 */
 
	package de.unika.ipd.grgen.parser.antlr;

	import java.io.File;
	import java.util.Collection;
	import java.util.HashMap;
	import java.util.Iterator;
	import java.util.LinkedList;
	import java.util.Map;

	import de.unika.ipd.grgen.parser.*;
	import de.unika.ipd.grgen.ast.*;
	import de.unika.ipd.grgen.util.*;
}

/**
 * GrGen types grammar
 * @author Sebastian Hack, Daniel Grund, Rubino Geiss, Adam Szalkowski
 * @version $Id$
 */
class GRTypeParser extends GRBaseParser;
options {
	k=2;
	codeGenMakeSwitchThreshold = 2;
	codeGenBitsetTestThreshold = 3;
	defaultErrorHandler = true;
	buildAST = false;
	importVocab = GRBase;
}

text returns [ ModelNode model = null ]
	{
		CollectNode<ModelNode> modelChilds = new CollectNode<ModelNode>();
		CollectNode<IdentNode> types = new CollectNode<IdentNode>();
		IdentNode id = env.getDummyIdent();

		String modelName = Util.removeFileSuffix(Util.removePathPrefix(getFilename()), "gm");

		id = new IdentNode(
			env.define(ParserEnvironment.MODELS, modelName,
			new de.unika.ipd.grgen.parser.Coords(0, 0, getFilename())));
	}

	:   ( m:MODEL i:IDENT SEMI
			{ reportWarning(getCoords(m), "keyword \"model\" is deprecated"); }
		)?
		( usingDecl[modelChilds] )?
		typeDecls[types] EOF
		{
			if(modelChilds.getChildren().size() == 0)
				modelChilds.addChild(env.getStdModel());
			model = new ModelNode(id, types, modelChilds);
		}
	;

identList [ Collection<String> strings ]
	: fid:IDENT { strings.add(fid.getText()); }
		( COMMA sid:IDENT { strings.add(sid.getText()); } )*
	;

usingDecl [ CollectNode<ModelNode> modelChilds ]
	{ Collection<String> modelNames = new LinkedList<String>(); }

	: u:USING identList[modelNames] SEMI
		{
			for(Iterator<String> it = modelNames.iterator(); it.hasNext();)
			{
				String modelName = it.next();
				File modelFile = env.findModel(modelName);
				if ( modelFile == null ) {
					reportError(getCoords(u), "model \"" + modelName + "\" could not be found");
				} else {
					ModelNode model;
					model = env.parseModel(modelFile);
					modelChilds.addChild(model);
				}
			}
		}
	;

typeDecls [ CollectNode<IdentNode> types ]
	{ IdentNode type; }

	: (type=typeDecl { types.addChild(type); } )*
	;

typeDecl returns [ IdentNode res = env.getDummyIdent() ]
	: res=classDecl
	| res=enumDecl
	;

classDecl returns [ IdentNode res = env.getDummyIdent() ]
	{ int mods = 0; }

	: (mods=typeModifiers)? (res=edgeClassDecl[mods] | res=nodeClassDecl[mods])
	;

typeModifiers returns [ int res = 0; ]
	{ int mod = 0; }

	: (mod=typeModifier { res |= mod; })+
	;

typeModifier returns [ int res = 0; ]
	: ABSTRACT { res |= InheritanceTypeNode.MOD_ABSTRACT; }
	| CONST { res |= InheritanceTypeNode.MOD_CONST; }
	;

/**
 * An edge class decl makes a new type decl node with the declaring id and
 * a new edge type node as children
 */
edgeClassDecl[int modifiers] returns [ IdentNode res = env.getDummyIdent() ]
	{
		IdentNode id;
		CollectNode<BaseNode> body = null;
		CollectNode<IdentNode> ext;
		CollectNode<ConnAssertNode> cas;
		String externalName = null;
		boolean arbitrary = false;
		boolean undirected = false;
	}

	:	(
			ARBITRARY
			{
				arbitrary = true;
				modifiers |= InheritanceTypeNode.MOD_ABSTRACT;
			}
		|	DIRECTED // do nothing, that's default
		|	UNDIRECTED { undirected = true; }
		)?
		EDGE CLASS id=typeIdentDecl (LT externalName=fullQualIdent GT)?
	  	ext=edgeExtends[id, arbitrary, undirected] cas=connectAssertions pushScope[id]
		(
			LBRACE body=edgeClassBody RBRACE
		|	SEMI
			{ body = new CollectNode<BaseNode>(); }
		)
		{
			EdgeTypeNode et;
			if (arbitrary) {
				et = new ArbitraryEdgeTypeNode(ext, cas, body, modifiers, externalName);
			}
			else {
				if (undirected) {
					et = new UndirectedEdgeTypeNode(ext, cas, body, modifiers, externalName);
				} else {
					et = new DirectedEdgeTypeNode(ext, cas, body, modifiers, externalName);
				}
			}
			id.setDecl(new TypeDeclNode(id, et));
			res = id;
		}
		popScope
  ;

nodeClassDecl[int modifiers] returns [ IdentNode res = env.getDummyIdent() ]
	{
		IdentNode id;
		CollectNode<BaseNode> body = null;
		CollectNode<IdentNode> ext;
		String externalName = null;
	}

	: 	NODE CLASS id=typeIdentDecl (LT externalName=fullQualIdent GT)?
	  	ext=nodeExtends[id] pushScope[id]
		(
			LBRACE body=nodeClassBody RBRACE
		|	SEMI
			{ body = new CollectNode<BaseNode>(); }
		)
		{
			NodeTypeNode nt = new NodeTypeNode(ext, body, modifiers, externalName);
			id.setDecl(new TypeDeclNode(id, nt));
			res = id;
		}
		popScope
	;

validIdent returns [ String id = "" ]
	:	i:~GT
		{
			if(i.getType() != IDENT && !env.isKeyword(i.getText()))
				throw new SemanticException(i.getText() + " is not a valid identifier",
					getFilename(), i.getLine(), i.getColumn());
			id = i.getText();
		}
	;

fullQualIdent returns [ String id = "", id2 = "" ]
	:	id=validIdent
	 	(DOT id2=validIdent { id += "." + id2; })*
	;

connectAssertions returns [ CollectNode<ConnAssertNode> c = new CollectNode<ConnAssertNode>() ]
	: CONNECT connectAssertion[c]
		( COMMA connectAssertion[c] )*
	|
	;

connectAssertion [ CollectNode<ConnAssertNode> c ]
	{
		IdentNode src, tgt;
		RangeSpecNode srcRange, tgtRange;
	}

	: src=typeIdentUse srcRange=rangeSpec RARROW
		tgt=typeIdentUse tgtRange=rangeSpec
			{ c.addChild(new ConnAssertNode(src, srcRange, tgt, tgtRange)); }
	;

edgeExtends [IdentNode clsId, boolean arbitrary, boolean undirected] returns [ CollectNode<IdentNode> c = new CollectNode<IdentNode>() ]
	: EXTENDS edgeExtendsCont[clsId, c, undirected]
	|	{
			if (arbitrary) {
				c.addChild(env.getArbitraryEdgeRoot());
			} else {
				if(undirected) {
					c.addChild(env.getUndirectedEdgeRoot());
				} else {
					c.addChild(env.getDirectedEdgeRoot());
				}
			}
		}
	;

edgeExtendsCont [ IdentNode clsId, CollectNode<IdentNode> c, boolean undirected ]
	{
		IdentNode e;
	}

	: e=typeIdentUse
		{
			if ( ! ((IdentNode)e).toString().equals(clsId.toString()) )
				c.addChild(e);
			else
				reportError(e.getCoords(), "A class must not extend itself");
		}
	(COMMA e=typeIdentUse
		{
			if ( ! ((IdentNode)e).toString().equals(clsId.toString()) )
				c.addChild(e);
			else
				reportError(e.getCoords(), "A class must not extend itself");
		}
	)*
		{
			if (c.getChildren().size() == 0) {
				if (undirected) {
					c.addChild(env.getUndirectedEdgeRoot());
				} else {
					c.addChild(env.getDirectedEdgeRoot());
				}
			}
		}
	;

nodeExtends [ IdentNode clsId ] returns [ CollectNode<IdentNode> c = new CollectNode<IdentNode>() ]
	: EXTENDS nodeExtendsCont[clsId, c]
	|	{ c.addChild(env.getNodeRoot()); }
	;

nodeExtendsCont [IdentNode clsId, CollectNode<IdentNode> c ]
	{ IdentNode n; }

	: n=typeIdentUse
		{
			if ( ! ((IdentNode)n).toString().equals(clsId.toString()) )
				c.addChild(n);
			else
				reportError(n.getCoords(), "A class must not extend itself");
		}
	(COMMA n=typeIdentUse
		{
			if ( ! ((IdentNode)n).toString().equals(clsId.toString()) )
				c.addChild(n);
			else
				reportError(n.getCoords(), "A class must not extend itself");
		}
	)*
		{ if ( c.getChildren().size() == 0 ) c.addChild(env.getNodeRoot()); }
	;

nodeClassBody returns [ CollectNode<BaseNode> c = new CollectNode<BaseNode>() ]
	{
		BaseNode b;
	}

	:	(
			(
				b=basicDecl { c.addChild(b); }
				(
					b=initExprDecl[((DeclNode)b).getIdentNode()] { c.addChild(b); }
				)?
			|
				b=initExpr { c.addChild(b); }
			) SEMI
		)*
	;

edgeClassBody returns [ CollectNode<BaseNode> c = new CollectNode<BaseNode>() ]
	{
		BaseNode b;
	}

	:	(
			(
				b=basicDecl { c.addChild(b); }
				(
					b=initExprDecl[((DeclNode)b).getIdentNode()] { c.addChild(b); }
				)?
			|
				b=initExpr { c.addChild(b); }
			) SEMI
		)*
	;

enumDecl returns [ IdentNode res = env.getDummyIdent() ]
	{
		IdentNode id;
		CollectNode<EnumItemNode> c = new CollectNode<EnumItemNode>();
	}

	: ENUM id=typeIdentDecl pushScope[id]
		LBRACE enumList[id, c]
		{
			TypeNode enumType = new EnumTypeNode(c);
			id.setDecl(new TypeDeclNode(id, enumType));
			res = id;
		}
		RBRACE popScope
	;

enumList[ IdentNode enumType, CollectNode<EnumItemNode> collect ]
	{
		int pos = 0;
		ExprNode init;
	}

	: init=enumItemDecl[enumType, collect, env.getZero(), pos++]
		( COMMA init=enumItemDecl[enumType, collect, init, pos++] )*
	;

enumItemDecl [ IdentNode type, CollectNode<EnumItemNode> coll, ExprNode defInit, int pos ]
				returns [ ExprNode res = env.initExprNode() ]
	{
		IdentNode id;
		ExprNode init = null;
		ExprNode value;
	}

	: id=entIdentDecl	( ASSIGN init=expr[true] )? //'true' means that expr initializes an enum item
		{
			if(init != null) {
				value = init;
			} else {
				value = defInit;
			}
			EnumItemNode memberDecl = new EnumItemNode(id, type, value, pos);
			id.setDecl(memberDecl);
			coll.addChild(memberDecl);
			OpNode add = new ArithmeticOpNode(id.getCoords(), OperatorSignature.ADD);
			add.addChild(value);
			add.addChild(env.getOne());
			res = add;
		}
	;

basicDecl returns [ MemberDeclNode res = null ]
	{
		IdentNode id = env.getDummyIdent();
		IdentNode type;
		MemberDeclNode decl;
		boolean isConst = false;
	}

	:	(
			ABSTRACT ( CONST { isConst = true; } )? id=entIdentDecl
			{
				res = new AbstractMemberDeclNode(id, isConst);
			}
		|
			( CONST { isConst = true; } )? id=entIdentDecl COLON type=typeIdentUse
			{
				decl = new MemberDeclNode(id, type, isConst);
				id.setDecl(decl);
				res = decl;
			}
		)
	;

initExpr returns [ MemberInitNode res = null ]
	{
		IdentNode id;
		ExprNode e = env.initExprNode();
	}

	: id=entIdentUse a:ASSIGN e=expr[false]
		{
			res = new MemberInitNode(getCoords(a), id, e);
		}
	;

initExprDecl[IdentNode id] returns [ MemberInitNode res = null ]
	{
		ExprNode e;
	}

	: a:ASSIGN e=expr[false]
		{
			res = new MemberInitNode(getCoords(a), id, e);
		}
	;


