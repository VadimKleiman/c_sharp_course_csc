using System;
using System.IO;

namespace MyTestApplication
{
    static class Program
    {
        private static void Main(string[] args)
        {
            string path = null;
            if (args.Length == 0)
            {
                path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                                    "TestModule");
                Console.WriteLine($"Default path: {path}");
            } else {
                path = args[0];
            }
            var a = new TestUnitImpl(path);
            var result = a.Start();
            string message = null;
            foreach (var r in result)
            {
                if (r.Type == FType.TEST_FT) 
                {
                    switch(r.Code)
                    {
                        case Result.OK_R: 
                            message = $"{r.MethodName}\tStatus: OK\tTime: {r.Time}";
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case Result.ERROR_R:
                            message = $"{r.MethodName}\tStatus: ERROR\tTime: {r.Time}\tMessage: {r.Other}";
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Result.IGNORE_R:
                            message = $"{r.MethodName}\tStatus: IGNORE\tTime: {r.Time}\tMessage: {r.Other}";
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                    }
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
            }
        }
    }
}
