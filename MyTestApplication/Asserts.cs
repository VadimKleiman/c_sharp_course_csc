namespace MyTestApplication
{
    public static class MyAssert
    {
        public static void EqualsValues<T>(T actual, T expected)
        {
            if (actual.Equals(expected))
            {
                return;
            }
            throw new TestException(actual, expected);
        }
    }
}