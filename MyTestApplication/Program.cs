using System;

namespace MyTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter path to modules!");
                Environment.Exit(-1);
            }
            TestUnitImpl a = new TestUnitImpl(args[0]);
            var result = a.Start();
            foreach(var r in result)
            {
                if (r.Code == TestUnitImpl.Result.R_OK &&
                    r.Type == TestUnitImpl.FType.FT_TEST)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0}\tStatus: OK\tTime: {1}",
                                      r.MethodName,
                                      r.Time);
                    Console.ResetColor();
                }
                else if (r.Code == TestUnitImpl.Result.R_ERROR &&
                        r.Type == TestUnitImpl.FType.FT_TEST)
                {
					Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}\tStatus: ERROR\tTime: {1}\tMessage: {2}",
									  r.MethodName,
									  r.Time,
                                      r.Other);
					Console.ResetColor();
                }
                else if (r.Code == TestUnitImpl.Result.R_IGNORE &&
						r.Type == TestUnitImpl.FType.FT_TEST)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("{0}\tStatus: IGNORE\tTime: {1}\tMessage: {2}",
									  r.MethodName,
									  r.Time,
									  r.Other);
					Console.ResetColor();
                }
            }
        }
    }
}
