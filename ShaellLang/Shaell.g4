grammar Shaell;

IF: 'if';
THEN: 'then';
ELSEIF: 'elseif';
ELSE: 'else';
END: 'end';
WHILE: 'while';
DO: 'do';
FOREACH: 'foreach';
FOR: 'for';
SWITCH: 'switch';
ON: 'on';
IN: 'in';
CASE: 'case';
RETURN: 'return';
CONTINUE: 'continue';
BREAK: 'break';
FUNCTION: 'fn';
GLOBAL: 'global';
ASYNC: 'async';
DEFER: 'defer';
LPAREN: '(';
RPAREN: ')';
LCURL: '{';
RCURL: '}';
LSQUACKET: '[';
RSQUACKET: ']';
COLON: ':';
DEREF: '@';
DOLLAR: '$';
LNOT: '!';
BNOT: '~';
MULT: '*';
DIV: '/';
MOD: '%';
PLUS: '+';
MINUS: '-';
LSHIFT: '<<';
RSHIFT: '>>';
LT: '<';
GT: '>';
GEQ: '>=';
LEQ: '<=';
EQ: '==';
NEQ: '!=';
BAND: '&';
BXOR: '^';
BOR: '|';
LAND: '&&';
LOR: '||';
NULLCOAL: '??';
PIPE: '->';
ASSIGN: '=';
COMMA: ',';
PLUSEQ: '+=';
MINUSEQ: '-=';
MULTEQ: '*=';
DIVEQ: '/=';
BANDEQ: '&=';
BXOREQ: '^=';
BOREQ: '|=';
MODEQ: '%=';
RSHIFTEQ: '>>=';
LSHIFTEQ: '<<=';
FILEIDENTFIER: [a-zA-Z_.][a-zA-Z0-9_.$]*;
VARIDENTFIER: DOLLAR [a-zA-Z0-9_.$]*;
NUMBER: [0-9]+('.'[0-9]+)?;
DQUOTE: '"';
SQUOTE: '\'';
FALSE: 'false';
TRUE: 'true';
STRINGLITERAL: '"' ~('"' | '\n')* '"';
COMMENT : '#' ~['\n']* {this._tokenStartCharPositionInLine == 0}?;

WHITESPACE: (' ' | '\t' | '\r' | '\n')+ -> skip;

/*
Lacks functions and comments
*/

prog: stmts;
stmts: stmt*;
stmt: ifStmt | forLoop | whileLoop | returnStatement | functionDefinition | expr;
boolean: TRUE | FALSE;
expr:  
    COMMENT # CommentExpr
    | STRINGLITERAL # StringLiteralExpr
    | NUMBER # NumberExpr
    | boolean # BooleanExpr
	| identifier # IdentifierExpr
	| LPAREN expr RPAREN # Parenthesis
	|<assoc=right> DEREF expr # DerefExpr
	|<assoc=right> LNOT expr # LnotExpr
	|<assoc=right> BNOT expr # BnotExpr
	|<assoc=right> MINUS expr # NegExpr
	|<assoc=right> PLUS expr # PosExpr
	| expr COLON identifier # IdentifierIndexExpr
	| expr LSQUACKET expr RSQUACKET # SubScriptExpr
	| expr LPAREN innerArgList RPAREN # FunctionCallExpr
	| expr DIV expr # DivExpr
	| expr MULT expr # MultExpr
    | expr PLUS expr # AddExpr
    | expr MINUS expr # MinusExpr
    | expr LT expr # LTExpr
    | expr LEQ expr # LEQExpr
    | expr GT expr # GTExpr
    | expr GEQ expr # GEQExpr
    | expr EQ expr # EQExpr
    | expr NEQ expr # NEQExpr
    | expr LAND expr # LANDExpr
    | expr LOR expr # LORExpr
    | expr PIPE expr # PIPEExpr
	|<assoc=right> expr ASSIGN expr # AssignExpr
	;



innerArgList: (expr (COMMA expr)*)?;
innerFormalArgList: (VARIDENTFIER (COMMA VARIDENTFIER)*)?;
identifier: FILEIDENTFIER | VARIDENTFIER;
ifStmt: IF expr THEN stmts (ELSE stmts)? END;
forLoop: FOR expr COMMA expr COMMA expr DO stmts END;
whileLoop: WHILE expr DO stmts END;
functionDefinition: FUNCTION VARIDENTFIER LPAREN innerFormalArgList RPAREN stmts END;
returnStatement: RETURN expr;

