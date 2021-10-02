using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CompiladorAlgebraico
{
    public class Scanner
    {
        private string _operation = "";
        private int _index = 0;
        private int _state = 0;

        public Scanner(string operation)
        {
            _operation = operation + (char)TokenType.EOF;
            _index = 0;
            _state = 0;
        }

        public Token GetToken()
        {
            Token result = new Token() { Value = Convert.ToString((char)0) };
            bool tokenFound = false;
            var numberValidation = new Regex("^[0-9]+$");

            while (!tokenFound)
            {
                char peek = _operation[_index];
                char lookHead;

                //Look a Head
                if (_index < (_operation.Length - 1))
                {
                    lookHead = _operation[_index + 1];
                }
                else
                {
                    lookHead = (char)0;
                }

                //Whitespace Remove
                while (char.IsWhiteSpace(peek))
                {
                    _index++;
                    peek = _operation[_index];
                    lookHead = _operation[_index + 1];
                }

                switch (_state)
                {
                    case 0:
                        if (numberValidation.IsMatch(Convert.ToString(peek)))
                        {
                            result.Tag = TokenType.Number;
                            result.Value = Convert.ToString(peek);
                            if (!numberValidation.IsMatch(Convert.ToString(lookHead)))
                            {
                                tokenFound = true;
                            }
                            else
                            {
                                _state = 1;
                            }
                        }

                        switch (peek)
                        {
                            case (char)TokenType.LParen:
                            case (char)TokenType.RParen:
                            case (char)TokenType.Plus:
                            case (char)TokenType.Minus:
                            case (char)TokenType.Mult:
                            case (char)TokenType.Div:
                            case (char)TokenType.EOF:
                                tokenFound = true;
                                result.Tag = (TokenType)peek;
                                break;
                            default:
                                break;
                        }// SWITCH - peek
                        break;
                    case 1:
                        if (numberValidation.IsMatch(Convert.ToString(peek)))
                        {
                            result.Value += Convert.ToString(peek);

                            if (!numberValidation.IsMatch(Convert.ToString(lookHead)))
                            {
                                tokenFound = true;
                            }
                        }
                        else
                        {
                            switch (peek)
                            {
                                case (char)TokenType.LParen:
                                case (char)TokenType.RParen:
                                case (char)TokenType.Plus:
                                case (char)TokenType.Minus:
                                case (char)TokenType.Mult:
                                case (char)TokenType.Div:
                                case (char)TokenType.EOF:
                                    tokenFound = true;
                                    result.Tag = (TokenType)peek;
                                    result.Value = Convert.ToString(peek);
                                    break;
                                default:
                                    throw new Exception("Lex Analyzer Error");
                            }// SWITCH - peek
                        }
                        break;
                    default:
                        break;
                }// SWITCH - state
                _index++;
            } // WHILE - tokenFound
            _state = 0;
            return result;
        }


    }
}
