using System.Collections.Generic;
using Xunit;
using System.IO;
using MyTestApplication;
using System.Linq;

namespace TestMyNUnit
{
    public class UnitTest1
    {

        private readonly List<TestUnitImpl.Status> rTest;

        private List<TestUnitImpl.Status> GetResult(string path)
        {
            TestUnitImpl unit = new TestUnitImpl(path);
            return unit.Start();
        }

        private int GetCount(FType fileType, Result result) 
            => rTest.Count(i => i.Type == fileType && i.Code == result);

        public UnitTest1()
        {
            rTest = GetResult(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void TestAfter()
        {
            int check = 4;
            int fc = GetCount(FType.AFTER_FT, Result.OK_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestBefore()
        {
            int check = 4;
            int fc = GetCount(FType.BEFORE_FT, Result.OK_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestAfterClass()
        {
            int check = 1;
            int fc = GetCount(FType.AFTERCLASS_FT, Result.OK_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestBeforeClass()
        {
            int check = 1;
            int fc = GetCount(FType.BEFORECLASS_FT, Result.OK_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestOK()
        {
            int check = 2;
            int fc = GetCount(FType.TEST_FT, Result.OK_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestFail()
        {
            int check = 1;
            int fc = GetCount(FType.TEST_FT, Result.ERROR_R);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestIgnore()
        {
            int check = 1;
            int fc = GetCount(FType.TEST_FT, Result.IGNORE_R);
            Assert.Equal(fc, check);
        }
    }
}
