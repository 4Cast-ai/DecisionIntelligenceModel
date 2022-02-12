using Calculator.Service;

namespace Calculator.Engine
{
    public static class ParserExtensions
    {
        public static TResult Execute<TResult>(this IParser<TResult> parser, string input)
        {
            return parser.Parse(input).Compile().Invoke();
        }
    }
}
