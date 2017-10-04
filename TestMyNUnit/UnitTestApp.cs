using System;
using System.Collections.Generic;
using Xunit;
using System.IO;

namespace TestMyNUnit
{
    public class UnitTest1
    {
        List<MyTestApplication.TestUnitImpl.Status> GetResult(string path)
        {
            MyTestApplication.TestUnitImpl unit = new MyTestApplication.TestUnitImpl(path);
            return unit.Start();
        }

        int GetCount(MyTestApplication.TestUnitImpl.FType ft, MyTestApplication.TestUnitImpl.Result r)
        {
            int fc = 0;
            foreach (var i in rTest)
            {
                if (i.Type == ft && i.Code == r)
                {
                    fc++;
                }
            }
            return fc;
        }

        List<MyTestApplication.TestUnitImpl.Status> rTest;

        public UnitTest1()
        {
            rTest = GetResult(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void TestAfter()
        {
            int fc = 0;
            int check = 4;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_AFTER,
                          MyTestApplication.TestUnitImpl.Result.R_OK);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestBefore()
        {
            int fc = 0;
            int check = 4;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_BEFORE,
                          MyTestApplication.TestUnitImpl.Result.R_OK);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestAfterClass()
        {
            int fc = 0;
            int check = 1;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_AFTERCLASS,
                          MyTestApplication.TestUnitImpl.Result.R_OK);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestBeforeClass()
        {
            int fc = 0;
            int check = 1;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_BEFORECLASS,
                          MyTestApplication.TestUnitImpl.Result.R_OK);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestOK()
        {
            int fc = 0;
            int check = 2;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_TEST,
                          MyTestApplication.TestUnitImpl.Result.R_OK);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestFail()
        {
            int fc = 0;
            int check = 1;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_TEST,
                          MyTestApplication.TestUnitImpl.Result.R_ERROR);
            Assert.Equal(fc, check);
        }

        [Fact]
        public void TestMethodTestIgnore()
        {
            int fc = 0;
            int check = 1;
            fc = GetCount(MyTestApplication.TestUnitImpl.FType.FT_TEST,
                          MyTestApplication.TestUnitImpl.Result.R_IGNORE);
            Assert.Equal(fc, check);
        }
    }
}
