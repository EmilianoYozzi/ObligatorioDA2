namespace Exceptions
{
    public class QueryException : Exception
    {
        public QueryException(Exception inner) : base(("Cannot communicate with Database" + inner.Message), inner) { }

    }
}