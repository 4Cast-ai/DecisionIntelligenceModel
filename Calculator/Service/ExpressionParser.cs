using Sprache;
using System;
using System.Linq.Expressions;

namespace Calculator.Service
{

    public class ExpressionParser : IParser<double>
    {
        public Expression<Func<double>> Parse(string input)
        {
            var result = Syntax.ParseLambda(new Input(input));

            if (!result.WasSuccessful)
            {
                throw new Exception(result.Message);
            }

            return result.Value;
        }
    }
}
