using System;

namespace CompiladorAlgebraico
{
    class Program
    {
        static void Main(string[] args)
        {
            string operation = Console.ReadLine();
            Parser parser = new Parser();
            Console.WriteLine();
            Console.WriteLine("----------------------------");
            Console.WriteLine("El resultado es: " + parser.Parse(operation).ToString());
            Console.WriteLine("----------------------------");
            Console.ReadLine();
        }
    }
}
