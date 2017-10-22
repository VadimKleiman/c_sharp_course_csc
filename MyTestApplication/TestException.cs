using System;

namespace MyTestApplication
{
    public sealed class TestException : Exception
    {
        public TestException(Object actual, Object expected)
        {
            _actual = actual;
            _expected = expected;
        }

        public override string ToString()
        {
            return string.Format($"[TestException: actual={_actual}, expected={_expected}]");
        }

        private Object _actual;
        private Object _expected;
    }
}