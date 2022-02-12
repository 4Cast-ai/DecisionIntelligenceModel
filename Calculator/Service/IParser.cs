using System;
using System.Linq.Expressions;

namespace Calculator.Service
{
    public interface IParser<TResult>
    {
        Expression<Func<TResult>> Parse(string input);
    }
}
