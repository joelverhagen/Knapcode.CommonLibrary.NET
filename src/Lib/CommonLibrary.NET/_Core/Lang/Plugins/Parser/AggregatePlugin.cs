﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.AST;
using ComLib.Lang.Helpers;
using ComLib.Lang.Types;
using ComLib.Lang.Parsing;
// </lang:using>

namespace ComLib.Lang.Plugins
{

    /* *************************************************************************
    <doc:example>	
    // Aggregate plugin allows sum, min, max, avg, count aggregate functions to 
    // be applied to lists of objects.
    
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    var result = 0;
    
    // Example 1: Using format sum of <expression>
    result = sum of numbers;
    result = avg of numbers;
    result = min of numbers;
    result = max of numbers;
    result = count of numbers;
    
    // Example 2: Using format sum(<expression>)
    result = sum( numbers );
    result = avg( numbers );
    result = min( numbers );
    result = max( numbers );
    result = count( numbers );    
    </doc:example>
    ***************************************************************************/
    /// <summary>
    /// Combinator for handling comparisons.
    /// </summary>
    public class AggregatePlugin : ExprPlugin
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public AggregatePlugin()
        {
            this.IsAutoMatched = true;
            this.StartTokens = new string[] 
            { 
                "avg", "min", "max", "sum", "count", "number", 
                "Avg", "Min", "Max", "Sum", "Count", "Number"
            };
        }


        /// <summary>
        /// Whether or not this parser can handle the supplied token.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            var next = _tokenIt.Peek().Token;
            if (next == Tokens.LeftParenthesis || string.Compare(next.Text, "of", StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;
            return false;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get { return "( avg | min | max | sum | count | number ) ( ( '(' <expression> ')' ) | ( of <expression> ) )"; }
        }


        /// <summary>
        /// Examples
        /// </summary>
        public override string[] Examples
        {
            get
            {
                return new string[]
                {
                    "min( numbers )",
                    "Min( numbers )",
                    "min of numbers",
                    "Min of numbers"
                };
            }
        }


        /// <summary>
        /// run step 123.
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {
            // avg min max sum count
            string aggregate = _tokenIt.NextToken.Token.Text.ToLower();

            var next = _tokenIt.Peek().Token;
            Expr exp = null;

            // 1. sum( <expression> )
            if (next == Tokens.LeftParenthesis)
            {
                _tokenIt.Advance(2);
                exp = _parser.ParseExpression(Terminators.ExpParenthesisEnd, passNewLine: false);
                _tokenIt.Expect(Tokens.RightParenthesis);
            }
            // 2. sum of <expression>
            else if (string.Compare(next.Text, "of", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                _tokenIt.Advance(2);
                exp = _parser.ParseExpression(null, false, true, passNewLine: false);
            }
            
            var aggExp = new AggregateExpr(aggregate, exp);
            return aggExp;
        }
    }



    /// <summary>
    /// Expression to represent a Linq like query.
    /// </summary>
    public class AggregateExpr : Expr
    {        
        private string _aggregateType;
        private Expr _source;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="aggregateType">sum avg min max count total</param>
        /// <param name="source"></param>
        public AggregateExpr(string aggregateType, Expr source)
        {
            InitBoundary(true, ")");
            _aggregateType = aggregateType;
            _source = source;
        }


        /// <summary>
        /// Evaluate the aggregate expression.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            var dataSource = _source.Evaluate() as LObject;
            ExceptionHelper.NotNull(this, dataSource, "aggregation(min/max)");
            
            List<object> items = null;

            // Get the right type of list.
            if (dataSource.Type == LTypes.Array)
                items = dataSource.GetValue() as List<object>;
            else
                throw new NotSupportedException(_aggregateType + " not supported for list type of " + dataSource.GetType());

            double val = 0;
            if (_aggregateType == "sum")
                val = items.Sum(item => GetValue(item));

            else if (_aggregateType == "avg")
                val = items.Average(item => GetValue(item));

            else if (_aggregateType == "min")
                val = items.Min(item => GetValue(item));

            else if (_aggregateType == "max")
                val = items.Max(item => GetValue(item));

            else if (_aggregateType == "count" || _aggregateType == "number")
                val = items.Count;

            return new LNumber(val);
        }


        private double GetValue(object item)
        {
            // Check 1: Null
            if (item == LObjects.Null) return 0;
            var lobj = (LObject) item;

            // Check 2: Number ? ok
            if (lobj.Type == LTypes.Number) return ((LNumber) lobj).Value;

            return 0;
        }
    }
}