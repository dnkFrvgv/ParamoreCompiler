using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Scanner.Enums;
using Scanner.Interfaces;



namespace Scanner
{
    public class Lexer : ILexer
    {
        private readonly string _sourceCode;
        //private int _start = 0;
        //private int line = 1;
        private int _currentPosition = 0;
        private List<Token> _tokens = new List<Token>();

        public Lexer(string sourceCode)
        {
            _sourceCode = sourceCode;
        }

      
        private void NextPosition() {
            _currentPosition++;
        }

        private char PeekNextCharacter()
        {
            if (_currentPosition + 1 >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[_currentPosition + 1];
        }


        private char GetCurrentCharacter()
        {
            if (_currentPosition >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[_currentPosition];
        }

        public List<Token> GetTokenList()
        {

            while (true)
            {
                var token = GetToken();
                _tokens.Add(token);
                if (token.Type == TokenType.END_OF_CODE) break;              
            }

            return _tokens;
        }

        private Token GetToken()
        {

            // reaching end of code

            if (_currentPosition == _sourceCode.Length)
            {
                return new Token(TokenType.END_OF_CODE, "\0", _currentPosition, null);
            }

            // recognising integer numbers

            if (Char.IsDigit(GetCurrentCharacter()))
            {
                int startPosition = _currentPosition;

                while(Char.IsDigit(GetCurrentCharacter()))
                {
                    NextPosition();
                }
                 
                int lengthOfNumber = _currentPosition - startPosition;
                string textNumber = _sourceCode.Substring(startPosition, lengthOfNumber);

                try
                {
                    int numberValue = int.Parse(textNumber);
                    return new Token(TokenType.NUMBER, textNumber, startPosition, numberValue);
                }
                catch (FormatException ex) 
                { 
                    Console.WriteLine($"Invalid integer at {startPosition} {ex}.");
                }

            }

            if (Char.IsWhiteSpace(GetCurrentCharacter()))
            {
                var startPosition = _currentPosition;

                    
                while (Char.IsWhiteSpace(GetCurrentCharacter()))
                {
                    NextPosition();
                }

                var lengthOfWhiteSpace = _currentPosition - startPosition;
                string textWhiteSpace = _sourceCode.Substring(startPosition, lengthOfWhiteSpace);
                    
                return new Token(TokenType.WHITE_SPACE, textWhiteSpace, startPosition, null);
                   
            }
       
            // recognising operators
            if (GetCurrentCharacter() == '+') return new Token(TokenType.PLUS, "+", _currentPosition++, null);
                
            if (GetCurrentCharacter() == '-') return new Token(TokenType.MINUS, "-", _currentPosition++, null);
                
            if (GetCurrentCharacter() == '*') return new Token(TokenType.ASTERISK, "*", _currentPosition++, null);
                
            if (GetCurrentCharacter() == '/') return new Token(TokenType.SLASH, "/", _currentPosition++, null);
            
            // recognising longer operators

            if (GetCurrentCharacter() == '=')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.EQUALS_EQUALS, "==", _currentPosition++, null);
                }
                else
                {
                    return new Token(TokenType.EQUALS, "=", _currentPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '>')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.GREATER_EQUALS, ">=", _currentPosition++, null);
                }
                else
                {
                    return new Token(TokenType.GREATER, ">", _currentPosition++, null);
                }
            }
            
            if (GetCurrentCharacter() == '<')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.LESS_EQUALS, "<=", _currentPosition++, null);
                }
                else
                {
                    return new Token(TokenType.LESS, "<", _currentPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '&')
            {
                if (PeekNextCharacter() == '&')
                {
                    NextPosition();
                    return new Token(TokenType.AND, "&&", _currentPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '|')
            {
                if (PeekNextCharacter() == '|')
                {
                    NextPosition();
                    return new Token(TokenType.OR, "||", _currentPosition++, null);
                }
            }

            // recognising punctuation

            if (GetCurrentCharacter() == '(') return new Token(TokenType.LEFT_PARENTHESES, "(", _currentPosition++, null);
            if (GetCurrentCharacter() == ')') return new Token(TokenType.RIGHT_PARENTHESES, ")", _currentPosition++, null);
            if (GetCurrentCharacter() == '{') return new Token(TokenType.LEFT_BRACES, "{", _currentPosition++, null);
            if (GetCurrentCharacter() == '}') return new Token(TokenType.RIGHT_BRACES, "}", _currentPosition++, null);
            if (GetCurrentCharacter() == '[') return new Token(TokenType.LEFT_BRACKETS, "[", _currentPosition++, null);
            if (GetCurrentCharacter() == ']') return new Token(TokenType.RIGHT_BRACKETS, "]", _currentPosition++, null);
            if (GetCurrentCharacter() == ',') return new Token(TokenType.COMMA, ",", _currentPosition++, null);
            if (GetCurrentCharacter() == '.') return new Token(TokenType.DOT, ".", _currentPosition++, null);
            if (GetCurrentCharacter() == ';') return new Token(TokenType.SEMICOLON, ";", _currentPosition++, null);
            if (GetCurrentCharacter() == ':') return new Token(TokenType.COLON, ":", _currentPosition++, null);

            // recognising identifies

            if (Char.IsLetter(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
            {                
                var startPosition = _currentPosition;

                while (Char.IsLetterOrDigit(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
                {
                    NextPosition();
                }

                int lengthOfIdentifier = _currentPosition - startPosition;
                string textIdentifier = _sourceCode.Substring(startPosition, lengthOfIdentifier);
                
                return new Token(TokenType.IDENTIFIER, textIdentifier, startPosition, null);
            }

            // recognising reserved words

            // unknown characters

            return new Token(TokenType.UNKNOWN_TOKEN, _sourceCode.Substring(_currentPosition , 1), _currentPosition++, null);
        }

    }
}
