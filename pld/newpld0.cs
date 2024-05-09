
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;
using System.Windows.Forms;

namespace com.calitha.goldparser
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF         =  0, // (EOF)
        SYMBOL_ERROR       =  1, // (Error)
        SYMBOL_WHITESPACE  =  2, // Whitespace
        SYMBOL_MINUS       =  3, // '-'
        SYMBOL_EXCLAMEQ    =  4, // '!='
        SYMBOL_PERCENT     =  5, // '%'
        SYMBOL_LPAREN      =  6, // '('
        SYMBOL_RPAREN      =  7, // ')'
        SYMBOL_TIMES       =  8, // '*'
        SYMBOL_TIMESTIMES  =  9, // '**'
        SYMBOL_DIV         = 10, // '/'
        SYMBOL_LBRACE      = 11, // '{'
        SYMBOL_RBRACE      = 12, // '}'
        SYMBOL_PLUS        = 13, // '+'
        SYMBOL_LT          = 14, // '<'
        SYMBOL_EQ          = 15, // '='
        SYMBOL_EQEQ        = 16, // '=='
        SYMBOL_GT          = 17, // '>'
        SYMBOL_DECIDE      = 18, // decide
        SYMBOL_DO          = 19, // do
        SYMBOL_ID          = 20, // id
        SYMBOL_IF          = 21, // if
        SYMBOL_ITERATE     = 22, // iterate
        SYMBOL_NUM         = 23, // num
        SYMBOL_OTHERWISE   = 24, // otherwise
        SYMBOL_OVER        = 25, // over
        SYMBOL_UNTIL       = 26, // until
        SYMBOL_WHILE       = 27, // while
        SYMBOL_ASSIGNMENT  = 28, // <assignment>
        SYMBOL_BLOCK       = 29, // <block>
        SYMBOL_COND        = 30, // <cond>
        SYMBOL_CONDITIONAL = 31, // <conditional>
        SYMBOL_EXP         = 32, // <exp>
        SYMBOL_EXPR        = 33, // <expr>
        SYMBOL_FACTOR      = 34, // <factor>
        SYMBOL_IDENTIFIER  = 35, // <identifier>
        SYMBOL_LOOP        = 36, // <loop>
        SYMBOL_NUM2        = 37, // <num>
        SYMBOL_OP          = 38, // <op>
        SYMBOL_PROGRAM     = 39, // <program>
        SYMBOL_START       = 40, // <start>
        SYMBOL_STMT        = 41, // <stmt>
        SYMBOL_STMT_LIST   = 42, // <stmt_list>
        SYMBOL_TERM        = 43  // <term>
    };

    enum RuleConstants : int
    {
        RULE_START                                  =  0, // <start> ::= <program>
        RULE_PROGRAM                                =  1, // <program> ::= <stmt_list>
        RULE_STMT_LIST                              =  2, // <stmt_list> ::= <stmt>
        RULE_STMT_LIST2                             =  3, // <stmt_list> ::= <stmt> <stmt_list>
        RULE_STMT                                   =  4, // <stmt> ::= <assignment>
        RULE_STMT2                                  =  5, // <stmt> ::= <conditional>
        RULE_STMT3                                  =  6, // <stmt> ::= <loop>
        RULE_IDENTIFIER_ID                          =  7, // <identifier> ::= id
        RULE_ASSIGNMENT_EQ                          =  8, // <assignment> ::= <identifier> '=' <expr>
        RULE_EXPR_PLUS                              =  9, // <expr> ::= <expr> '+' <term>
        RULE_EXPR_MINUS                             = 10, // <expr> ::= <expr> '-' <term>
        RULE_EXPR                                   = 11, // <expr> ::= <term>
        RULE_TERM_TIMES                             = 12, // <term> ::= <term> '*' <factor>
        RULE_TERM_DIV                               = 13, // <term> ::= <term> '/' <factor>
        RULE_TERM_PERCENT                           = 14, // <term> ::= <term> '%' <factor>
        RULE_TERM                                   = 15, // <term> ::= <factor>
        RULE_FACTOR_TIMESTIMES                      = 16, // <factor> ::= <factor> '**' <exp>
        RULE_FACTOR                                 = 17, // <factor> ::= <exp>
        RULE_EXP_LPAREN_RPAREN                      = 18, // <exp> ::= '(' <expr> ')'
        RULE_EXP                                    = 19, // <exp> ::= <identifier>
        RULE_EXP2                                   = 20, // <exp> ::= <num>
        RULE_NUM_NUM                                = 21, // <num> ::= num
        RULE_CONDITIONAL_IF_LPAREN_RPAREN           = 22, // <conditional> ::= if '(' <cond> ')' <block>
        RULE_CONDITIONAL_IF_LPAREN_RPAREN_OTHERWISE = 23, // <conditional> ::= if '(' <cond> ')' <block> otherwise <block>
        RULE_CONDITIONAL_DECIDE_LPAREN_RPAREN       = 24, // <conditional> ::= decide '(' <expr> ')' <block>
        RULE_COND                                   = 25, // <cond> ::= <expr> <op> <expr>
        RULE_OP_LT                                  = 26, // <op> ::= '<'
        RULE_OP_GT                                  = 27, // <op> ::= '>'
        RULE_OP_EQEQ                                = 28, // <op> ::= '=='
        RULE_OP_EXCLAMEQ                            = 29, // <op> ::= '!='
        RULE_LOOP_ITERATE_LPAREN_OVER_RPAREN        = 30, // <loop> ::= iterate '(' <identifier> over <expr> ')' <block>
        RULE_LOOP_WHILE_LPAREN_RPAREN               = 31, // <loop> ::= while '(' <cond> ')' <block>
        RULE_LOOP_DO_UNTIL_LPAREN_RPAREN            = 32, // <loop> ::= do <block> until '(' <cond> ')'
        RULE_BLOCK                                  = 33, // <block> ::= <stmt>
        RULE_BLOCK_LBRACE_RBRACE                    = 34  // <block> ::= '{' <stmt_list> '}'
    };

    public class MyParser
    {
        private LALRParser parser;
        ListBox lst;
        ListBox ls;
        public MyParser(string filename, ListBox lst, ListBox ls)
        {
            FileStream stream = new FileStream(filename,
                                              FileMode.Open,
                                              FileAccess.Read,
                                              FileShare.Read);
            this.lst = lst;
            this.ls = ls;
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
        }

        public void Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
            }
        }

        private Object CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private Object CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAMEQ :
                //'!='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PERCENT :
                //'%'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMESTIMES :
                //'**'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACE :
                //'{'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RBRACE :
                //'}'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECIDE :
                //decide
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DO :
                //do
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ID :
                //id
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ITERATE :
                //iterate
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUM :
                //num
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OTHERWISE :
                //otherwise
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OVER :
                //over
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNTIL :
                //until
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ASSIGNMENT :
                //<assignment>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BLOCK :
                //<block>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COND :
                //<cond>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CONDITIONAL :
                //<conditional>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXP :
                //<exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPR :
                //<expr>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FACTOR :
                //<factor>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //<identifier>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOOP :
                //<loop>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUM2 :
                //<num>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_OP :
                //<op>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PROGRAM :
                //<program>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_START :
                //<start>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMT :
                //<stmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STMT_LIST :
                //<stmt_list>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TERM :
                //<term>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_START :
                //<start> ::= <program>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PROGRAM :
                //<program> ::= <stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT_LIST :
                //<stmt_list> ::= <stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT_LIST2 :
                //<stmt_list> ::= <stmt> <stmt_list>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT :
                //<stmt> ::= <assignment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT2 :
                //<stmt> ::= <conditional>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STMT3 :
                //<stmt> ::= <loop>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IDENTIFIER_ID :
                //<identifier> ::= id
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_ASSIGNMENT_EQ :
                //<assignment> ::= <identifier> '=' <expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_PLUS :
                //<expr> ::= <expr> '+' <term>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR_MINUS :
                //<expr> ::= <expr> '-' <term>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPR :
                //<expr> ::= <term>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TERM_TIMES :
                //<term> ::= <term> '*' <factor>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TERM_DIV :
                //<term> ::= <term> '/' <factor>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TERM_PERCENT :
                //<term> ::= <term> '%' <factor>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TERM :
                //<term> ::= <factor>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FACTOR_TIMESTIMES :
                //<factor> ::= <factor> '**' <exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FACTOR :
                //<factor> ::= <exp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXP_LPAREN_RPAREN :
                //<exp> ::= '(' <expr> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXP :
                //<exp> ::= <identifier>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXP2 :
                //<exp> ::= <num>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NUM_NUM :
                //<num> ::= num
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONAL_IF_LPAREN_RPAREN :
                //<conditional> ::= if '(' <cond> ')' <block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONAL_IF_LPAREN_RPAREN_OTHERWISE :
                //<conditional> ::= if '(' <cond> ')' <block> otherwise <block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CONDITIONAL_DECIDE_LPAREN_RPAREN :
                //<conditional> ::= decide '(' <expr> ')' <block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_COND :
                //<cond> ::= <expr> <op> <expr>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_LT :
                //<op> ::= '<'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_GT :
                //<op> ::= '>'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_EQEQ :
                //<op> ::= '=='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_OP_EXCLAMEQ :
                //<op> ::= '!='
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOOP_ITERATE_LPAREN_OVER_RPAREN :
                //<loop> ::= iterate '(' <identifier> over <expr> ')' <block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOOP_WHILE_LPAREN_RPAREN :
                //<loop> ::= while '(' <cond> ')' <block>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOOP_DO_UNTIL_LPAREN_RPAREN :
                //<loop> ::= do <block> until '(' <cond> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCK :
                //<block> ::= <stmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_BLOCK_LBRACE_RBRACE :
                //<block> ::= '{' <stmt_list> '}'
                //todo: Create a new object using the stored tokens.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '" + args.Token.ToString() + "'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '" + args.UnexpectedToken.ToString() + "In line:" + args.UnexpectedToken.Location.LineNr;
            lst.Items.Add(message);
            string m2 = "Expected token : " + args.ExpectedTokens.ToString();
            lst.Items.Add(m2);
            //todo: Report message to UI
        }
        private void TokenReadEvent(LALRParser pr, TokenReadEventArgs args)
        {

            string info = args.Token.Text + "         " + (SymbolConstants)args.Token.Symbol.Id;
            ls.Items.Add(info);
        }
    }
}
