using System;
namespace hw01
{
    class MainClass
    {
        public static void Main(string[] args) 
        {
            IStringSet stringSet = new StringSetImpl();
            stringSet.Add("abc");
            stringSet.Add("abcc");
            stringSet.Remove("abc");
            Console.WriteLine(stringSet.Contains("abc"));
            Console.WriteLine(stringSet.Size());
            Console.WriteLine(stringSet.HowManyStartsWithPrefix("abc"));
        }
    }
}
