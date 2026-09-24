namespace CompiladorAlgebraico
{
    /// <summary>Tipos de token que reconoce el <see cref="Scanner"/>.</summary>
    public enum TokenType
    {
        /// <summary>Literal numerico, entero o decimal (por ejemplo <c>42</c> o <c>3.14</c>).</summary>
        Number,
        Plus,
        Minus,
        Mult,
        Div,
        LParen,
        RParen,
        /// <summary>Fin de la entrada.</summary>
        EOF
    }
}
