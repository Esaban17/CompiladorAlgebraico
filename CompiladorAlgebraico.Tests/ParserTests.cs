using Xunit;

namespace CompiladorAlgebraico.Tests
{
    public class ParserTests
    {
        private const int Precision = 10;

        private static double Eval(string expression) => new Parser().Parse(expression);

        [Theory]
        // Los ejemplos documentados en el README.
        [InlineData("2 + 3 * 4", 14)]
        [InlineData("(5 + 3) * 2", 16)]
        [InlineData("10 - 2 * 3", 4)]
        [InlineData("100 / (2 + 3)", 20)]
        [InlineData("-5 + 10", 5)]
        [InlineData("(2 + 3) * (4 - 1)", 15)]
        // Precedencia y agrupacion.
        [InlineData("1", 1)]
        [InlineData("((((1))))", 1)]
        [InlineData("2 + 3", 5)]
        [InlineData("10 - (3 - 2)", 9)]
        [InlineData("1 - 6 / 3", -1)]
        [InlineData("6 / 3 - 1", 1)]
        [InlineData("2 * 3 + 4 * 5", 26)]
        public void EvaluaExpresionesValidas(string expression, double expected)
        {
            Assert.Equal(expected, Eval(expression), Precision);
        }

        /// <summary>
        /// Regresion: <c>a - b - c</c> debe agruparse como <c>(a - b) - c</c>. La version
        /// anterior negaba toda la cola de la expresion y devolvia <c>a - b + c</c>.
        /// </summary>
        [Theory]
        [InlineData("1 - 2 - 3", -4)]
        [InlineData("10 - 3 - 2", 5)]
        [InlineData("2 - 3 + 4", 3)]
        [InlineData("20 - 5 + 5", 20)]
        [InlineData("1 + 2 - 3 + 4 - 5", -1)]
        [InlineData("1 - 2 + 3 - 4", -2)]
        [InlineData("1 - 2 * 3 - 4", -9)]
        public void LaSumaYLaRestaSonAsociativasPorLaIzquierda(string expression, double expected)
        {
            Assert.Equal(expected, Eval(expression), Precision);
        }

        [Theory]
        [InlineData("8 / 4 / 2", 1)]
        [InlineData("100 / 5 / 2 / 2", 5)]
        [InlineData("64 / 2 / 4 / 2", 4)]
        [InlineData("8 / 4 * 2", 4)]
        [InlineData("2 * 3 / 4 * 5", 7.5)]
        [InlineData("8 * 4 / 2", 16)]
        public void LaMultiplicacionYLaDivisionSonAsociativasPorLaIzquierda(string expression, double expected)
        {
            Assert.Equal(expected, Eval(expression), Precision);
        }

        [Theory]
        [InlineData("-5", -5)]
        [InlineData("--5", 5)]
        [InlineData("---5", -5)]
        [InlineData("2 - -3", 5)]
        [InlineData("-2 * -3", 6)]
        [InlineData("-(3 - 1)", -2)]
        [InlineData("-(-(-2))", -2)]
        public void AplicaElOperadorUnarioNegativo(string expression, double expected)
        {
            Assert.Equal(expected, Eval(expression), Precision);
        }

        [Theory]
        [InlineData("3.14", 3.14)]
        [InlineData("1.5 + 2.5", 4)]
        [InlineData("0.1 + 0.2", 0.3)]
        [InlineData("10 / 4", 2.5)]
        [InlineData(".5 + .5", 1)]
        [InlineData("2.5 * 4", 10)]
        public void EvaluaNumerosDecimales(string expression, double expected)
        {
            Assert.Equal(expected, Eval(expression), Precision);
        }

        /// <summary>
        /// Regresion: las ramas por defecto del parser anterior devolvian el elemento
        /// neutro (0 o 1) en vez de fallar, de modo que <c>2 +</c> se evaluaba como 3.
        /// </summary>
        [Theory]
        [InlineData("2 +")]
        [InlineData("5 -")]
        [InlineData("2 *")]
        [InlineData("2 /")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("+")]
        [InlineData("*")]
        [InlineData("()")]
        [InlineData("(1+2")]
        [InlineData("1+2)")]
        [InlineData("2 3")]
        [InlineData("(1+2)(3+4)")]
        [InlineData("1 + * 2")]
        [InlineData("1 ++ 2")]
        public void RechazaExpresionesMalFormadas(string expression)
        {
            Assert.Throws<SyntacticException>(() => Eval(expression));
        }

        [Theory]
        [InlineData("2 & 3")]
        [InlineData("2 $ 3")]
        [InlineData("abc")]
        [InlineData("1.")]
        public void RechazaCaracteresNoReconocidos(string expression)
        {
            Assert.Throws<LexicalException>(() => Eval(expression));
        }

        [Theory]
        [InlineData("1 / 0")]
        [InlineData("5 / (3 - 3)")]
        [InlineData("1 / 0.0")]
        public void RechazaLaDivisionPorCero(string expression)
        {
            Assert.Throws<EvaluationException>(() => Eval(expression));
        }

        [Fact]
        public void RechazaNumerosFueraDelRangoDeDouble()
        {
            string tooBig = "1" + new string('0', 400);

            Assert.Throws<EvaluationException>(() => Eval(tooBig));
        }

        [Fact]
        public void ElErrorIndicaLaPosicionDelCaracter()
        {
            var lexical = Assert.Throws<LexicalException>(() => Eval("2 & 3"));
            Assert.Equal(2, lexical.Position);

            var syntactic = Assert.Throws<SyntacticException>(() => Eval("1 + * 2"));
            Assert.Equal(4, syntactic.Position);
        }

        [Fact]
        public void LaMismaInstanciaSePuedeReutilizar()
        {
            var parser = new Parser();

            Assert.Equal(14, parser.Parse("2 + 3 * 4"), Precision);
            Assert.Equal(-4, parser.Parse("1 - 2 - 3"), Precision);
            Assert.Throws<SyntacticException>(() => parser.Parse("2 +"));
            Assert.Equal(5, parser.Parse("2 + 3"), Precision);
        }

        [Fact]
        public void AceptaNumerosGrandesDentroDelRango()
        {
            Assert.Equal(1e20, Eval("99999999999999999999 + 1"), Precision);
        }
    }
}
