using System;
using System.Collections.Generic;
using System.Text;

namespace CompiladorAlgebraico
{
    class Parser
    {
        Scanner _scanner;
        Token _token;
        double _operationResult = 0;
        
        public double Parse(string operation)
        {
            _scanner = new Scanner(operation + (char)TokenType.EOF);
            _token = _scanner.GetToken();

            switch (_token.Tag)
            {
                case TokenType.Minus:
                case TokenType.LParen:
                case TokenType.Number:
                    _operationResult = E();
                    break;
                default:
                    break;
            }
            Match(TokenType.EOF);

            return _operationResult;
        }

        private int Match(TokenType tag)
        {
            TokenType previousTag = tag;
            int tokenValue = 0;
            if (_token.Tag == tag)
            {
                if (_token.Tag == TokenType.Number)
                {
                    tokenValue = Convert.ToInt32(_token.Value);
                }
                _token = _scanner.GetToken();


                if (previousTag == TokenType.Mult && previousTag == _token.Tag)
                {
                    throw new Exception("Syntactic Analyzer Error");
                }
                else if (previousTag == TokenType.Div && previousTag == _token.Tag)
                {
                    throw new Exception("Syntactic Analyzer Error");
                }
                else if (previousTag == TokenType.Plus && previousTag == _token.Tag)
                {
                    throw new Exception("Syntactic Analyzer Error");
                }

                return tokenValue;
            }
            else
            {
                throw new Exception("Syntactic Analyzer Error");
            }
        }

        private double E()
        {
            switch (_token.Tag)
            {
                case TokenType.Minus:
                case TokenType.LParen:
                case TokenType.Number:
                    _operationResult = T() + EPrime();
                    break;
                default:
                    _operationResult = 0;
                    break;
            }
            return _operationResult;
        }

        private double EPrime()
        {
            switch (_token.Tag)
            {
                case TokenType.Minus:
                    Match(TokenType.Minus);
                    _operationResult = - (T() + EPrime());
                    break;
                case TokenType.Plus:
                    Match(TokenType.Plus);
                    _operationResult = T() + EPrime();
                    break;
                default:
                    _operationResult = 0;
                    break;
            }
            return _operationResult;
        }

        private double T()
        {
            switch (_token.Tag)
            {
                case TokenType.Minus:
                case TokenType.LParen:
                case TokenType.Number:
                    _operationResult = F() * TPrime();
                    break;
                default:
                    _operationResult = 1;
                    break;
            }
            return _operationResult;
        }

        private double TPrime()
        {
            switch (_token.Tag)
            {
                case TokenType.Mult:
                    Match(TokenType.Mult);
                    _operationResult = (F() * TPrime());
                    break;
                case TokenType.Div:
                    Match(TokenType.Div);
                    _operationResult = (1 / (F() * (1 / TPrime())));
                    break;
                default:
                    _operationResult = 1;
                    break;
            }
            return _operationResult;
        }

        private double F()
        {
            switch (_token.Tag)
            {
                case TokenType.Minus:
                    Match(TokenType.Minus);
                    _operationResult = -R();
                    break;
                case TokenType.LParen:
                case TokenType.Number:
                    _operationResult = R();
                    break;
                default:
                    _operationResult = 0;
                    break;
            }
            return _operationResult;
        }

        private double R()
        {
            switch (_token.Tag)
            {
                case TokenType.Number:
                    _operationResult = Match(TokenType.Number);
                    break;
                case TokenType.LParen:
                    Match(TokenType.LParen);
                    _operationResult = E();
                    Match(TokenType.RParen);
                    break;
                default:
                    _operationResult = 0;
                    break;
            }
            return _operationResult;
        }

    }
}
