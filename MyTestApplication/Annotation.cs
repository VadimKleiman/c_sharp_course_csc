using System;

namespace MyTestApplication
{
    public class Test : Attribute
    {
        public Type Expected { get; set; }
        public string Ignore { get; set; }
    }

    public class Before : Attribute { }

    public class After : Attribute { }

    public class BeforeClass : Attribute { }

    public class AfterClass : Attribute { }
}
