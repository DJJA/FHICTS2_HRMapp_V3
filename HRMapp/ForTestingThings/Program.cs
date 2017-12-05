using System;

namespace ForTestingThings
{
    class Program
    {
        enum TestNum
        {
            bla1,
            bla2,
            bla3,
            bla4
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine((int)TestNum.bla2);
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
