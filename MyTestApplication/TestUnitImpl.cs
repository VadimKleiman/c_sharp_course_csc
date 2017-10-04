using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MyTestApplication
{
    public sealed class TestUnitImpl
    {
        public sealed class Status
        {
            public Status(string MethodName,
                          Result Code,
                          FType Type,
                          long Time,
                          string Other)
            {
                this.MethodName = MethodName;
                this.Code = Code;
                this.Type = Type;
                this.Time = Time;
                this.Other = Other;
            }

            public string MethodName { get; set; }
            public Result Code { get; set; }
            public FType Type { get; set; }
            public long Time { get; set; }
            public string Other { get; set; }
        }

        public enum Result
        {
            R_OK,
            R_ERROR,
            R_IGNORE
        };

        public enum FType
        {
            FT_BEFORE,
            FT_AFTER,
            FT_AFTERCLASS,
            FT_BEFORECLASS,
            FT_TEST
        };

        string[] filePaths;

        public TestUnitImpl(string path)
        {
            filePaths = Directory.GetFiles(@path,
                                           "*.dll",
                                           SearchOption.AllDirectories);
        }

        void StartMethodType(List<Status> result,
                             IEnumerable<MethodInfo> methods,
                             object instance,
                             FType type)
        {
            Stopwatch sw = null;
            foreach (var method in methods)
            {
                try
                {
                    sw = Stopwatch.StartNew();
                    method.Invoke(instance, null);
                    sw.Stop();
                    result.Add(new Status(method.Name,
                                          Result.R_OK,
                                          type,
                                          sw.ElapsedMilliseconds,
                                          null));
                }
                catch (TargetInvocationException ex)
                {
                    var nameArgs = method.GetCustomAttributesData()[0].NamedArguments;
                    if (nameArgs.Any() && (Type)nameArgs[0]
                                .TypedValue.Value == ex.InnerException.GetType())
                    {
                        sw.Stop();
                        result.Add(new Status(method.Name,
                                          Result.R_OK,
                                          type,
                                          sw.ElapsedMilliseconds,
                                          null));
                    }
                    else
                    {
                        sw.Stop();
                        result.Add(new Status(method.Name,
                                          Result.R_ERROR,
                                          type,
                                          sw.ElapsedMilliseconds,
                                          ex.InnerException.ToString()));
                    }
                }
            }
        }

        public List<Status> Start()
        {
            List<Status> result = new List<Status>();
            foreach (var module in filePaths)
            {
                Assembly dll = Assembly.LoadFile(@module);
                foreach (var type in dll.GetTypes())
                {
                    var before = GetMethods(type, typeof(Before));
                    var after = GetMethods(type, typeof(After));
                    var beforeClass = GetMethods(type, typeof(BeforeClass));
                    var afterClass = GetMethods(type, typeof(AfterClass));
                    var methods = GetMethods(type, typeof(Test));
                    if (!methods.Any())
                        continue;
                    var instance = Activator.CreateInstance(type);
                    StartMethodType(result, 
                                    beforeClass, 
                                    instance, 
                                    FType.FT_BEFORECLASS);
                    foreach (var m in methods)
                    {
                        StartMethodType(result, 
                                        before, 
                                        instance, 
                                        FType.FT_BEFORE);
                        var nameArgs = m.GetCustomAttributesData()[0].NamedArguments;
                        if (nameArgs.Any() && nameArgs[0].TypedValue.Value is string)
                        {
                            result.Add(new Status(m.Name,
                                                  Result.R_IGNORE,
                                                  FType.FT_TEST,
                                                  0,
                                                  (string)nameArgs[0].TypedValue.Value));
                        }
                        else
                        {
                            StartMethodType(result, 
                                            new List<MethodInfo> { m }, 
                                            instance, 
                                            FType.FT_TEST);
                        }
                        StartMethodType(result, 
                                        after, 
                                        instance, 
                                        FType.FT_AFTER);
                    }
                    StartMethodType(result, 
                                    afterClass, 
                                    instance, 
                                    FType.FT_AFTERCLASS);
                }

            }
            return result;
        }

        IEnumerable<MethodInfo> GetMethods(Type instance, 
                                           Type attr)
        {
            var methods = from method in instance.GetMethods()
                          where method.IsDefined(attr)
                          select method;
            return methods;
        }
    }
}
