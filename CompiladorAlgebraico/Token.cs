namespace CompiladorAlgebraico
{
    /// <summary>Unidad lexica producida por el <see cref="Scanner"/>.</summary>
    public sealed class Token
    {
        public Token(TokenType tag, string value, int position)
        {
            Tag = tag;
            Value = value;
            Position = position;
        }

        /// <summary>Categoria del token.</summary>
        public TokenType Tag { get; }

        /// <summary>Texto exacto que ocupa el token en la entrada.</summary>
        public string Value { get; }

        /// <summary>Indice base 0 del primer caracter del token dentro de la entrada.</summary>
        public int Position { get; }

        public override string ToString() =>
            Tag == TokenType.EOF ? "el fin de la expresion" : "'" + Value + "'";
    }
}
