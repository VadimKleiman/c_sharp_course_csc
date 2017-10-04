using System;

public sealed class TestException : Exception
{
    public TestException(Object actual, Object expected)
    {
        this.actual = actual;
        this.expected = expected;
    }

    public override string ToString()
    {
        return string.Format("[TestException: actual={0}, expected={1}]",
                             actual,
                             expected);
    }

    private Object actual;
    private Object expected;
}