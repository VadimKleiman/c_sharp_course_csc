using System;

public class Test : Attribute
{
	public Test() { }
	public Test(Type Expected)
	{
		this.Expected = Expected;
	}
	public Test(string Ignore)
	{
		this.Ignore = Ignore;
	}
	public Type Expected { get; set; }
	public string Ignore { get; set; }
}

public class Before : Attribute { }

public class After : Attribute { }

public class BeforeClass : Attribute { }

public class AfterClass : Attribute { }
