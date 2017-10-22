using System;
using MyTestApplication;
using System.Threading;

namespace TestModule
{
    public class FirstModule
    {
        private int CountBefore;
        private int CountAfter;
        private int CountBeforeClass;
        private int CountAfterClass;

        [Test]
        public void TestMethodTrue()
        {
            Thread.Sleep(150);
            MyAssert.EqualsValues(1, 1);
        }

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
            ++CountAfter;
        }

        [BeforeClass]
        public void TestBeforeClass()
        {
            ++CountBeforeClass;
        }

        [AfterClass]
        public void TestAfterClass()
        {
            ++CountAfterClass;
        }

        public void Empty() {}
    }
}
