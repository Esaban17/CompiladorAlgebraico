using System.Globalization;

namespace CompiladorAlgebraico
{
    /// <summary>
    /// Analizador sintactico descendente recursivo que evalua la expresion mientras la reconoce.
    ///
    /// Gramatica (los operadores binarios son asociativos por la izquierda):
    ///
    ///     E -> T (('+' | '-') T)*
    ///     T -> F (('*' | '/') F)*
    ///     F -> '-' F | R
    ///     R -> number | '(' E ')'
    ///
    /// La repeticion con <c>*</c> reemplaza a los no terminales E' y T' de la version
    /// recursiva pura: acumular en un bucle es lo que garantiza la asociatividad por la
    /// izquierda, de modo que <c>1 - 2 - 3</c> se agrupa como <c>(1 - 2) - 3</c>.
    /// </summary>
    public sealed class Parser
    {
        private Scanner _scanner = new Scanner(string.Empty);
        private Token _token = new Token(TokenType.EOF, string.Empty, 0);

        /// <summary>Analiza y evalua una expresion algebraica.</summary>
        /// <exception cref="LexicalException">La entrada contiene un caracter no reconocido.</exception>
        /// <exception cref="SyntacticException">La expresion esta mal formada.</exception>
        /// <exception cref="EvaluationException">La expresion es valida pero no se puede evaluar.</exception>
        public double Parse(string operation)
        {
            _scanner = new Scanner(operation);
            _token = _scanner.GetToken();

            if (_token.Tag == TokenType.EOF)
            {
                throw new SyntacticException("la expresion esta vacia", _token.Position);
            }

            double result = E();
            Expect(TokenType.EOF);
            return result;
        }

        /// <summary>Consume el token actual si coincide con <paramref name="tag"/>; si no, falla.</summary>
        private Token Expect(TokenType tag)
        {
            if (_token.Tag != tag)
            {
                throw new SyntacticException(
                    "se esperaba " + Describe(tag) + " pero se encontro " + _token,
                    _token.Position);
            }

            Token matched = _token;
            _token = _scanner.GetToken();
            return matched;
        }

        /// <summary>E -> T (('+' | '-') T)*</summary>
        private double E()
        {
            double value = T();

            while (_token.Tag == TokenType.Plus || _token.Tag == TokenType.Minus)
            {
                TokenType op = _token.Tag;
                Expect(op);
                double right = T();
                value = op == TokenType.Plus ? value + right : value - right;
            }

            return value;
        }

        /// <summary>T -> F (('*' | '/') F)*</summary>
        private double T()
        {
            double value = F();

            while (_token.Tag == TokenType.Mult || _token.Tag == TokenType.Div)
            {
                TokenType op = _token.Tag;
                int position = _token.Position;
                Expect(op);
                double right = F();

                if (op == TokenType.Mult)
                {
                    value *= right;
                }
                else if (right == 0)
                {
                    throw new EvaluationException("division por cero", position);
                }
                else
                {
                    value /= right;
                }
            }

            return value;
        }

        /// <summary>F -> '-' F | R</summary>
        private double F()
        {
            if (_token.Tag == TokenType.Minus)
            {
                Expect(TokenType.Minus);
                return -F();
            }

            return R();
        }

        /// <summary>R -> number | '(' E ')'</summary>
        private double R()
        {
            switch (_token.Tag)
            {
                case TokenType.Number:
                    return ToDouble(Expect(TokenType.Number));

                case TokenType.LParen:
                    Expect(TokenType.LParen);
                    double inner = E();
                    Expect(TokenType.RParen);
                    return inner;

                default:
                    throw new SyntacticException(
                        "se esperaba un numero o '(' pero se encontro " + _token,
                        _token.Position);
            }
        }

        private static double ToDouble(Token number)
        {
            double value = double.Parse(number.Value, NumberStyles.Float, CultureInfo.InvariantCulture);

            if (double.IsInfinity(value))
            {
                throw new EvaluationException(
                    "el numero '" + number.Value + "' esta fuera del rango de double",
                    number.Position);
            }

            return value;
        }

        private static string Describe(TokenType tag)
        {
            switch (tag)
            {
                case TokenType.Number: return "un numero";
                case TokenType.Plus: return "'+'";
                case TokenType.Minus: return "'-'";
                case TokenType.Mult: return "'*'";
                case TokenType.Div: return "'/'";
                case TokenType.LParen: return "'('";
                case TokenType.RParen: return "')'";
                case TokenType.EOF: return "el fin de la expresion";
                default: return tag.ToString();
            }
        }
    }
}
