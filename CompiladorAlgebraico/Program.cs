using System;
using System.Globalization;

namespace CompiladorAlgebraico
{
    internal static class Program
    {
        /// <returns>0 si la expresion se evaluo, 1 si hubo un error de compilacion.</returns>
        private static int Main(string[] args)
        {
            // Sin argumentos se lee de la consola; la pausa final solo tiene sentido
            // cuando hay una persona al otro lado (no en un pipe ni en CI).
            bool interactive = args.Length == 0 && !Console.IsInputRedirected;
            string? expression;

            if (args.Length > 0)
            {
                expression = string.Join(" ", args);
            }
            else
            {
                if (interactive)
                {
                    Console.Write("Ingrese la expresion: ");
                }

                expression = Console.ReadLine();
            }

            if (expression is null)
            {
                return 0;
            }

            int exitCode = 0;

            try
            {
                double result = new Parser().Parse(expression);
                Console.WriteLine();
                Console.WriteLine("----------------------------");
                Console.WriteLine("El resultado es: " + result.ToString("G15", CultureInfo.InvariantCulture));
                Console.WriteLine("----------------------------");
            }
            catch (CompilerException ex)
            {
                Console.Error.WriteLine(ex.Message);
                exitCode = 1;
            }

            if (interactive)
            {
                Console.WriteLine();
                Console.Write("Presione Enter para salir.");
                Console.ReadLine();
            }

            return exitCode;
        }
    }
}
