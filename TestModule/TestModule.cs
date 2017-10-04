using System;

namespace TestModule
{
    public class FirstModule
    {
        public int CountBefore;
        public int CountAfter;
        public int CountBeforeClass;
        public int CountAfterClass;

        [Test]
        public void TestMethodTrue() => MyAssert.EqualsValues(1, 1);

        [Test]
        public void TestMethodFalse() => MyAssert.EqualsValues(1, 2);

        [Test(Expected = typeof(InvalidCastException))]
        public void TestException() => throw new InvalidCastException();

        [Test(Ignore = "Ingnore method")]
        public void TestIgnore() => throw new Exception();

        [Before]
        public void TestBefore()
        {
            ++CountBefore;
        }

        [After]
        public void TestAfter()
        {
            --CountAfter;
        }

        [BeforeClass]
        public void TestBeforeClass()
        {
            ++CountBeforeClass;
        }

        [AfterClass]
        public void TestAfterClass()
        {
            --CountAfterClass;
        }
    }


}
