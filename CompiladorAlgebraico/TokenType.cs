using System;
using System.Collections.Generic;
using System.Text;

namespace CompiladorAlgebraico
{
    public enum TokenType
    {
        Plus = '+',
        Minus = '-',
        Mult = '*',
        Div = '/',
        LParen = '(',
        RParen = ')',
        EOF = (char)0,
        Number = (char)3
    }
}
