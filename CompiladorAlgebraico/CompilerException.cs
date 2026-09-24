using System;

namespace CompiladorAlgebraico
{
    /// <summary>
    /// Error detectado al analizar o evaluar una expresion. Todos los errores del
    /// compilador llevan la posicion (base 0) del caracter donde se detectaron.
    /// </summary>
    public abstract class CompilerException : Exception
    {
        protected CompilerException(string kind, string detail, int position)
            : base(kind + " (posicion " + (position + 1) + "): " + detail)
        {
            Kind = kind;
            Detail = detail;
            Position = position;
        }

        /// <summary>Familia del error, por ejemplo <c>Lex Analyzer Error</c>.</summary>
        public string Kind { get; }

        /// <summary>Descripcion del error sin el prefijo ni la posicion.</summary>
        public string Detail { get; }

        /// <summary>Indice base 0 del caracter donde se detecto el error.</summary>
        public int Position { get; }
    }

    /// <summary>La entrada contiene un caracter que el scanner no reconoce.</summary>
    public sealed class LexicalException : CompilerException
    {
        public LexicalException(string detail, int position)
            : base("Lex Analyzer Error", detail, position)
        {
        }
    }

    /// <summary>La secuencia de tokens no forma una expresion valida.</summary>
    public sealed class SyntacticException : CompilerException
    {
        public SyntacticException(string detail, int position)
            : base("Syntactic Analyzer Error", detail, position)
        {
        }
    }

    /// <summary>La expresion esta bien formada pero no se puede evaluar.</summary>
    public sealed class EvaluationException : CompilerException
    {
        public EvaluationException(string detail, int position)
            : base("Evaluation Error", detail, position)
        {
        }
    }
}
