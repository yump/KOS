// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace kOS.Safe.Compilation.KS
{
    #region ParseTree
    [Serializable]
    public class ParseErrors : List<ParseError>
    {
    }

    [Serializable]
    public class ParseError
    {
        private string file;
        private string message;
        private int code;
        private int line;
        private int col;
        private int pos;
        private int length;

        public string File { get { return file; } }
        public int Code { get { return code; } }
        public int Line { get { return line; } }
        public int Column { get { return col; } }
        public int Position { get { return pos; } }
        public int Length { get { return length; } }
        public string Message { get { return message; } }

        // just for the sake of serialization
        public ParseError()
        {
        }

        public ParseError(string message, int code, ParseNode node) : this(message, code, node.Token)
        {
        }

        public ParseError(string message, int code, Token token) : this(message, code, token.File, token.Line, token.Column, token.StartPos, token.Length)
        {
        }

        public ParseError(string message, int code) : this(message, code, string.Empty, 0, 0, 0, 0)
        {
        }

        public ParseError(string message, int code, string file, int line, int col, int pos, int length)
        {
            this.file = file;
            this.message = message;
            this.code = code;
            this.line = line;
            this.col = col;
            this.pos = pos;
            this.length = length;
        }
    }

    // rootlevel of the node tree
    [Serializable]
    public partial class ParseTree : ParseNode
    {
        public ParseErrors Errors;

        public List<Token> Skipped;

        public ParseTree() : base(new Token(), "ParseTree")
        {
            Token.Type = TokenType.Start;
            Token.Text = "Root";
            Errors = new ParseErrors();
        }

        public string PrintTree()
        {
            StringBuilder sb = new StringBuilder();
            int indent = 0;
            PrintNode(sb, this, indent);
            return sb.ToString();
        }

        private void PrintNode(StringBuilder sb, ParseNode node, int indent)
        {
            
            string space = "".PadLeft(indent, ' ');

            sb.Append(space);
            sb.AppendLine(node.Text);

            foreach (ParseNode n in node.Nodes)
                PrintNode(sb, n, indent + 2);
        }
        
        /// <summary>
        /// this is the entry point for executing and evaluating the parse tree.
        /// </summary>
        /// <param name="paramlist">additional optional input parameters</param>
        /// <returns>the output of the evaluation function</returns>
        public object Eval(params object[] paramlist)
        {
            return Nodes[0].Eval(this, paramlist);
        }
    }

    [Serializable]
    [XmlInclude(typeof(ParseTree))]
    public partial class ParseNode
    {
        protected string text;
        protected List<ParseNode> nodes;
        
        public List<ParseNode> Nodes { get {return nodes;} }
        
        [XmlIgnore] // avoid circular references when serializing
        public ParseNode Parent;
        public Token Token; // the token/rule

        [XmlIgnore] // skip redundant text (is part of Token)
        public string Text { // text to display in parse tree 
            get { return text;} 
            set { text = value; }
        } 

        public virtual ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new ParseNode(token, text);
            node.Parent = this;
            return node;
        }

        protected ParseNode(Token token, string text)
        {
            this.Token = token;
            this.text = text;
            this.nodes = new List<ParseNode>();
        }

        protected object GetValue(ParseTree tree, TokenType type, int index)
        {
            return GetValue(tree, type, ref index);
        }

        protected object GetValue(ParseTree tree, TokenType type, ref int index)
        {
            object o = null;
            if (index < 0) return o;

            // left to right
            foreach (ParseNode node in nodes)
            {
                if (node.Token.Type == type)
                {
                    index--;
                    if (index < 0)
                    {
                        o = node.Eval(tree);
                        break;
                    }
                }
            }
            return o;
        }

        /// <summary>
        /// this implements the evaluation functionality, cannot be used directly
        /// </summary>
        /// <param name="tree">the parsetree itself</param>
        /// <param name="paramlist">optional input parameters</param>
        /// <returns>a partial result of the evaluation</returns>
        internal object Eval(ParseTree tree, params object[] paramlist)
        {
            object Value = null;

            switch (Token.Type)
            {
                case TokenType.Start:
                    Value = EvalStart(tree, paramlist);
                    break;
                case TokenType.instruction_block:
                    Value = Evalinstruction_block(tree, paramlist);
                    break;
                case TokenType.instruction:
                    Value = Evalinstruction(tree, paramlist);
                    break;
                case TokenType.lazyglobal_directive:
                    Value = Evallazyglobal_directive(tree, paramlist);
                    break;
                case TokenType.directive:
                    Value = Evaldirective(tree, paramlist);
                    break;
                case TokenType.set_stmt:
                    Value = Evalset_stmt(tree, paramlist);
                    break;
                case TokenType.if_stmt:
                    Value = Evalif_stmt(tree, paramlist);
                    break;
                case TokenType.until_stmt:
                    Value = Evaluntil_stmt(tree, paramlist);
                    break;
                case TokenType.fromloop_stmt:
                    Value = Evalfromloop_stmt(tree, paramlist);
                    break;
                case TokenType.unlock_stmt:
                    Value = Evalunlock_stmt(tree, paramlist);
                    break;
                case TokenType.print_stmt:
                    Value = Evalprint_stmt(tree, paramlist);
                    break;
                case TokenType.on_stmt:
                    Value = Evalon_stmt(tree, paramlist);
                    break;
                case TokenType.toggle_stmt:
                    Value = Evaltoggle_stmt(tree, paramlist);
                    break;
                case TokenType.wait_stmt:
                    Value = Evalwait_stmt(tree, paramlist);
                    break;
                case TokenType.when_stmt:
                    Value = Evalwhen_stmt(tree, paramlist);
                    break;
                case TokenType.onoff_stmt:
                    Value = Evalonoff_stmt(tree, paramlist);
                    break;
                case TokenType.onoff_trailer:
                    Value = Evalonoff_trailer(tree, paramlist);
                    break;
                case TokenType.stage_stmt:
                    Value = Evalstage_stmt(tree, paramlist);
                    break;
                case TokenType.clear_stmt:
                    Value = Evalclear_stmt(tree, paramlist);
                    break;
                case TokenType.add_stmt:
                    Value = Evaladd_stmt(tree, paramlist);
                    break;
                case TokenType.remove_stmt:
                    Value = Evalremove_stmt(tree, paramlist);
                    break;
                case TokenType.log_stmt:
                    Value = Evallog_stmt(tree, paramlist);
                    break;
                case TokenType.break_stmt:
                    Value = Evalbreak_stmt(tree, paramlist);
                    break;
                case TokenType.preserve_stmt:
                    Value = Evalpreserve_stmt(tree, paramlist);
                    break;
                case TokenType.declare_identifier_clause:
                    Value = Evaldeclare_identifier_clause(tree, paramlist);
                    break;
                case TokenType.declare_parameter_clause:
                    Value = Evaldeclare_parameter_clause(tree, paramlist);
                    break;
                case TokenType.declare_function_clause:
                    Value = Evaldeclare_function_clause(tree, paramlist);
                    break;
                case TokenType.declare_lock_clause:
                    Value = Evaldeclare_lock_clause(tree, paramlist);
                    break;
                case TokenType.declare_stmt:
                    Value = Evaldeclare_stmt(tree, paramlist);
                    break;
                case TokenType.return_stmt:
                    Value = Evalreturn_stmt(tree, paramlist);
                    break;
                case TokenType.switch_stmt:
                    Value = Evalswitch_stmt(tree, paramlist);
                    break;
                case TokenType.copy_stmt:
                    Value = Evalcopy_stmt(tree, paramlist);
                    break;
                case TokenType.rename_stmt:
                    Value = Evalrename_stmt(tree, paramlist);
                    break;
                case TokenType.delete_stmt:
                    Value = Evaldelete_stmt(tree, paramlist);
                    break;
                case TokenType.edit_stmt:
                    Value = Evaledit_stmt(tree, paramlist);
                    break;
                case TokenType.run_stmt:
                    Value = Evalrun_stmt(tree, paramlist);
                    break;
                case TokenType.compile_stmt:
                    Value = Evalcompile_stmt(tree, paramlist);
                    break;
                case TokenType.list_stmt:
                    Value = Evallist_stmt(tree, paramlist);
                    break;
                case TokenType.reboot_stmt:
                    Value = Evalreboot_stmt(tree, paramlist);
                    break;
                case TokenType.shutdown_stmt:
                    Value = Evalshutdown_stmt(tree, paramlist);
                    break;
                case TokenType.for_stmt:
                    Value = Evalfor_stmt(tree, paramlist);
                    break;
                case TokenType.unset_stmt:
                    Value = Evalunset_stmt(tree, paramlist);
                    break;
                case TokenType.arglist:
                    Value = Evalarglist(tree, paramlist);
                    break;
                case TokenType.expr:
                    Value = Evalexpr(tree, paramlist);
                    break;
                case TokenType.and_expr:
                    Value = Evaland_expr(tree, paramlist);
                    break;
                case TokenType.compare_expr:
                    Value = Evalcompare_expr(tree, paramlist);
                    break;
                case TokenType.arith_expr:
                    Value = Evalarith_expr(tree, paramlist);
                    break;
                case TokenType.multdiv_expr:
                    Value = Evalmultdiv_expr(tree, paramlist);
                    break;
                case TokenType.unary_expr:
                    Value = Evalunary_expr(tree, paramlist);
                    break;
                case TokenType.factor:
                    Value = Evalfactor(tree, paramlist);
                    break;
                case TokenType.suffix:
                    Value = Evalsuffix(tree, paramlist);
                    break;
                case TokenType.suffix_trailer:
                    Value = Evalsuffix_trailer(tree, paramlist);
                    break;
                case TokenType.suffixterm:
                    Value = Evalsuffixterm(tree, paramlist);
                    break;
                case TokenType.suffixterm_trailer:
                    Value = Evalsuffixterm_trailer(tree, paramlist);
                    break;
                case TokenType.function_trailer:
                    Value = Evalfunction_trailer(tree, paramlist);
                    break;
                case TokenType.array_trailer:
                    Value = Evalarray_trailer(tree, paramlist);
                    break;
                case TokenType.atom:
                    Value = Evalatom(tree, paramlist);
                    break;
                case TokenType.sci_number:
                    Value = Evalsci_number(tree, paramlist);
                    break;
                case TokenType.number:
                    Value = Evalnumber(tree, paramlist);
                    break;
                case TokenType.varidentifier:
                    Value = Evalvaridentifier(tree, paramlist);
                    break;
                case TokenType.identifier_led_stmt:
                    Value = Evalidentifier_led_stmt(tree, paramlist);
                    break;
                case TokenType.identifier_led_expr:
                    Value = Evalidentifier_led_expr(tree, paramlist);
                    break;

                default:
                    Value = Token.Text;
                    break;
            }
            return Value;
        }

        protected virtual object EvalStart(ParseTree tree, params object[] paramlist)
        {
            return "Could not interpret input; no semantics implemented.";
        }

        protected virtual object Evalinstruction_block(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalinstruction(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evallazyglobal_directive(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldirective(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalset_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalif_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaluntil_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalfromloop_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalunlock_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalprint_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalon_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaltoggle_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalwait_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalwhen_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalonoff_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalonoff_trailer(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalstage_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalclear_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaladd_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalremove_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evallog_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalbreak_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalpreserve_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldeclare_identifier_clause(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldeclare_parameter_clause(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldeclare_function_clause(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldeclare_lock_clause(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldeclare_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalreturn_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalswitch_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalcopy_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalrename_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaldelete_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaledit_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalrun_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalcompile_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evallist_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalreboot_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalshutdown_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalfor_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalunset_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalarglist(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalexpr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evaland_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalcompare_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalarith_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalmultdiv_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalunary_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalfactor(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalsuffix(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalsuffix_trailer(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalsuffixterm(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalsuffixterm_trailer(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalfunction_trailer(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalarray_trailer(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalatom(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalsci_number(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalnumber(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalvaridentifier(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalidentifier_led_stmt(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        protected virtual object Evalidentifier_led_expr(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }


    }
    
    #endregion ParseTree
}
