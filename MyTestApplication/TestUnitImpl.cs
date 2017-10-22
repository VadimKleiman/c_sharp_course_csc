using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MyTestApplication
{
    public enum Result
    {
        OK_R,
        ERROR_R,
        IGNORE_R
    };

    public enum FType
    {
        BEFORE_FT,
        AFTER_FT,
        AFTERCLASS_FT,
        BEFORECLASS_FT,
        TEST_FT
    };

    public sealed class TestUnitImpl
    {
        public sealed class Status
        {
            public Status(string methodName,
                          Result code,
                          FType type,
                          long time,
                          string other)
            {
                MethodName = methodName;
                Code = code;
                Type = type;
                Time = time;
                Other = other;
            }

            public string MethodName { get; }
            public Result Code { get; }
            public FType Type { get; }
            public long Time { get; }
            public string Other { get; }
        }

        readonly string[] filePaths;

        public TestUnitImpl(string path)
        {
            filePaths = Directory.GetFiles(path,
                                           "*.dll",
                                           SearchOption.AllDirectories);
        }

        public List<Status> Start()
        {
            List<Status> result = new List<Status>();
            foreach (var module in filePaths)
            {
                Assembly dll = Assembly.LoadFile(module);
                foreach (var type in dll.GetTypes())
                {
                    var before = GetMethods(type, typeof(Before));
                    var after = GetMethods(type, typeof(After));
                    var beforeClass = GetMethods(type, typeof(BeforeClass));
                    var afterClass = GetMethods(type, typeof(AfterClass));
                    var methods = GetMethods(type, typeof(Test));
                    if (!methods.Any())
                    {
                        continue;
                    }
                    var instance = Activator.CreateInstance(type);
                    StartMethodType(result,
                                    beforeClass,
                                    instance,
                                    FType.BEFORECLASS_FT);
                    foreach (var m in methods)
                    {
                        StartMethodType(result,
                                        before,
                                        instance,
                                        FType.BEFORE_FT);
                        var nameArgs = m.GetCustomAttributesData()[0].NamedArguments;
                        if (nameArgs.Any() && nameArgs[0].TypedValue.Value is string)
                        {
                            result.Add(new Status(m.Name,
                                                  Result.IGNORE_R,
                                                  FType.TEST_FT,
                                                  0,
                                                  (string)nameArgs[0].TypedValue.Value));
                        }
                        else
                        {
                            StartMethodType(result,
                                            new List<MethodInfo> { m },
                                            instance,
                                            FType.TEST_FT);
                        }
                        StartMethodType(result,
                                        after,
                                        instance,
                                        FType.AFTER_FT);
                    }
                    StartMethodType(result,
                                    afterClass,
                                    instance,
                                    FType.AFTERCLASS_FT);
                }

            }
            return result;
        }

        private void StartMethodType(List<Status> result,
                             IEnumerable<MethodInfo> methods,
                             object instance,
                             FType type)
        {
            Stopwatch sw = null;
            sw = Stopwatch.StartNew();
            foreach (var method in methods)
            {
                try
                {
                    sw = Stopwatch.StartNew();
                    method.Invoke(instance, null);
                    result.Add(new Status(method.Name,
                                          Result.OK_R,
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
                        result.Add(new Status(method.Name,
                                          Result.OK_R,
                                          type,
                                          sw.ElapsedMilliseconds,
                                          null));
                    }
                    else
                    {
                        result.Add(new Status(method.Name,
                                          Result.ERROR_R,
                                          type,
                                          sw.ElapsedMilliseconds,
                                          ex.InnerException.ToString()));
                    }
                } 
                finally
                {
                    sw.Reset();
                }
            }
        }

        private MethodInfo[] GetMethods(Type instance, Type attr) 
            => instance.GetMethods().Where(method => method.IsDefined(attr)).ToArray();
    }
}
