using System;

namespace CompiladorAlgebraico
{
    /// <summary>
    /// Analizador lexico: recorre la entrada una sola vez y la convierte en tokens.
    /// Una vez consumida la entrada devuelve <see cref="TokenType.EOF"/> de forma
    /// indefinida, de modo que el parser puede consultar el token final sin riesgo.
    /// </summary>
    public sealed class Scanner
    {
        private readonly string _input;
        private int _index;

        public Scanner(string input)
        {
            _input = input ?? string.Empty;
            _index = 0;
        }

        /// <summary>Devuelve el siguiente token de la entrada.</summary>
        /// <exception cref="LexicalException">La entrada contiene un caracter no reconocido.</exception>
        public Token GetToken()
        {
            while (_index < _input.Length && char.IsWhiteSpace(_input[_index]))
            {
                _index++;
            }

            if (_index >= _input.Length)
            {
                return new Token(TokenType.EOF, string.Empty, _index);
            }

            int start = _index;
            char peek = _input[_index];

            if (char.IsDigit(peek) || peek == '.')
            {
                return ReadNumber(start);
            }

            _index++;
            switch (peek)
            {
                case '+': return new Token(TokenType.Plus, "+", start);
                case '-': return new Token(TokenType.Minus, "-", start);
                case '*': return new Token(TokenType.Mult, "*", start);
                case '/': return new Token(TokenType.Div, "/", start);
                case '(': return new Token(TokenType.LParen, "(", start);
                case ')': return new Token(TokenType.RParen, ")", start);
                default:
                    throw new LexicalException("caracter no reconocido '" + peek + "'", start);
            }
        }

        /// <summary>Consume un literal numerico: digitos, con parte decimal opcional.</summary>
        private Token ReadNumber(int start)
        {
            while (_index < _input.Length && char.IsDigit(_input[_index]))
            {
                _index++;
            }

            if (_index < _input.Length && _input[_index] == '.')
            {
                int dot = _index;
                _index++;

                if (_index >= _input.Length || !char.IsDigit(_input[_index]))
                {
                    throw new LexicalException("se esperaban digitos despues del punto decimal", dot);
                }

                while (_index < _input.Length && char.IsDigit(_input[_index]))
                {
                    _index++;
                }
            }

            return new Token(TokenType.Number, _input.Substring(start, _index - start), start);
        }
    }
}
