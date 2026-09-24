using Xunit;

namespace CompiladorAlgebraico.Tests
{
    public class ScannerTests
    {
        private static TokenType[] Tags(string input)
        {
            var scanner = new Scanner(input);
            var tags = new System.Collections.Generic.List<TokenType>();

            Token token;
            do
            {
                token = scanner.GetToken();
                tags.Add(token.Tag);
            }
            while (token.Tag != TokenType.EOF);

            return tags.ToArray();
        }

        [Fact]
        public void ReconoceTodosLosOperadores()
        {
            Assert.Equal(
                new[]
                {
                    TokenType.Number, TokenType.Plus, TokenType.Minus, TokenType.Mult,
                    TokenType.Div, TokenType.LParen, TokenType.RParen, TokenType.EOF
                },
                Tags("1 + - * / ( )"));
        }

        [Fact]
        public void AgrupaLosDigitosEnUnSoloToken()
        {
            var scanner = new Scanner("1234 + 56");

            Token first = scanner.GetToken();
            Assert.Equal(TokenType.Number, first.Tag);
            Assert.Equal("1234", first.Value);

            Assert.Equal(TokenType.Plus, scanner.GetToken().Tag);

            Token second = scanner.GetToken();
            Assert.Equal(TokenType.Number, second.Tag);
            Assert.Equal("56", second.Value);
        }

        [Theory]
        [InlineData("3.14", "3.14")]
        [InlineData(".5", ".5")]
        [InlineData("10", "10")]
        public void ReconoceNumerosDecimales(string input, string expected)
        {
            Token token = new Scanner(input).GetToken();

            Assert.Equal(TokenType.Number, token.Tag);
            Assert.Equal(expected, token.Value);
        }

        [Fact]
        public void IgnoraLosEspaciosEnBlanco()
        {
            Assert.Equal(Tags("1+2"), Tags("  1  \t +  2   "));
        }

        /// <summary>
        /// El parser consulta el token final mas de una vez; el scanner no debe
        /// leer fuera de la entrada al hacerlo.
        /// </summary>
        [Fact]
        public void DevuelveEofDeFormaIndefinida()
        {
            var scanner = new Scanner("1");

            Assert.Equal(TokenType.Number, scanner.GetToken().Tag);

            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(TokenType.EOF, scanner.GetToken().Tag);
            }
        }

        [Fact]
        public void LaEntradaVaciaProduceEof()
        {
            Assert.Equal(TokenType.EOF, new Scanner("").GetToken().Tag);
            Assert.Equal(TokenType.EOF, new Scanner("   ").GetToken().Tag);
        }

        [Fact]
        public void RegistraLaPosicionDeCadaToken()
        {
            var scanner = new Scanner("12 + 3");

            Assert.Equal(0, scanner.GetToken().Position);
            Assert.Equal(3, scanner.GetToken().Position);
            Assert.Equal(5, scanner.GetToken().Position);
            Assert.Equal(6, scanner.GetToken().Position);
        }

        [Theory]
        [InlineData("2 & 3", 2)]
        [InlineData("a", 0)]
        [InlineData("1.", 1)]
        public void RechazaCaracteresNoReconocidos(string input, int position)
        {
            var scanner = new Scanner(input);

            var error = Assert.Throws<LexicalException>(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    scanner.GetToken();
                }
            });

            Assert.Equal(position, error.Position);
        }
    }
}
