// $ANTLR : "grgen.g" -> "GRParser.java"$

    package de.unika.ipd.grgen.parser.antlr;

		import java.util.Iterator;
		import java.util.List;
		import java.util.LinkedList;
		import java.util.Map;
		import java.util.HashMap;
		
		import de.unika.ipd.grgen.parser.Symbol;
		import de.unika.ipd.grgen.parser.SymbolTable;
		import de.unika.ipd.grgen.parser.Scope;
    import de.unika.ipd.grgen.ast.*;
    import de.unika.ipd.grgen.util.report.*;
    import de.unika.ipd.grgen.Main;

public interface GRParserTokenTypes {
	int EOF = 1;
	int NULL_TREE_LOOKAHEAD = 3;
	int DECL_GROUP = 4;
	int DECL_TEST = 5;
	int DECL_TYPE = 6;
	int DECL_NODE = 7;
	int DECL_EDGE = 8;
	int DECL_BASIC = 9;
	int TYPE_NODE = 10;
	int TYPE_EDGE = 11;
	int SIMPLE_CONN = 12;
	int GROUP_CONN = 13;
	int TEST_BODY = 14;
	int PATTERN_BODY = 15;
	int SUBGRAPH_SPEC = 16;
	int CONN_DECL = 17;
	int CONN_CONT = 18;
	int MAIN = 19;
	int LITERAL_unit = 20;
	int SEMI = 21;
	int LITERAL_edge = 22;
	int LITERAL_class = 23;
	int LBRACE = 24;
	int RBRACE = 25;
	int RARROW = 26;
	int LITERAL_node = 27;
	int LITERAL_extends = 28;
	int COMMA = 29;
	int LBRACK = 30;
	int COLON = 31;
	int RBRACK = 32;
	int INTEGER = 33;
	int LITERAL_enum = 34;
	int LITERAL_group = 35;
	int LITERAL_test = 36;
	int LITERAL_rule = 37;
	int LPAREN = 38;
	int RPAREN = 39;
	int LITERAL_in = 40;
	int LITERAL_out = 41;
	int LITERAL_pattern = 42;
	int LITERAL_replace = 43;
	int LITERAL_redirect = 44;
	int LITERAL_eval = 45;
	int LITERAL_cond = 46;
	int MINUS = 47;
	int LARROW = 48;
	int NOTLARROW = 49;
	int NOTMINUS = 50;
	int TILDE = 51;
	int DOUBLECOLON = 52;
	int IDENT = 53;
	int ASSIGN = 54;
	int QUESTION = 55;
	int LOR = 56;
	int LAND = 57;
	int BOR = 58;
	int BXOR = 59;
	int BAND = 60;
	int EQUAL = 61;
	int NOT_EQUAL = 62;
	int LT = 63;
	int LE = 64;
	int GT = 65;
	int GE = 66;
	int SL = 67;
	int SR = 68;
	int BSR = 69;
	int PLUS = 70;
	int STAR = 71;
	int MOD = 72;
	int DIV = 73;
	int NOT = 74;
	int LITERAL_cast = 75;
	int NUM_DEC = 76;
	int NUM_HEX = 77;
	int STRING_LITERAL = 78;
	int LITERAL_true = 79;
	int LITERAL_false = 80;
	int DOT = 81;
	int WS = 82;
	int SL_COMMENT = 83;
	int ML_COMMENT = 84;
	int ESC = 85;
}
