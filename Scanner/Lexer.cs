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
        private string[] _sourceCode;
        private string _currentLine;
        //private int _start = 0;
        private int _line = 1;
        private int _currentPosition = 0;
        private List<Token> _tokens = new List<Token>();

        /**public Lexer(string[] sourceCode)
        {
            _sourceCode = sourceCode;
            _currentLine = _sourceCode[_line-1];
        }**/

        public Lexer(){}

        public void AddSourceCode(string[] sourceCode)
        {
            _sourceCode = sourceCode;
            _currentLine = _sourceCode[_line - 1];
        }
      
        private void NextPosition() {
            _currentPosition++;
        }

        private char PeekNextCharacter()
        {
            if (_currentPosition + 1 >= _currentLine.Length)
            {
                return '\0';
            }

            return _currentLine[_currentPosition + 1];
        }


        private char GetCurrentCharacter()
        {
            if (_currentPosition >= _currentLine.Length)
            {
                return '\0';
            }

            return _currentLine[_currentPosition];
        }

        public List<Token> GetTokenList()
        {
       
            if (_sourceCode == null)
            {
                throw new NullReferenceException($"SourceCode was not added. Call AddSourceCode() method to initialise it.");
            }
            else
            {
                while (true)
                {
                    var token = GetToken();
                    _tokens.Add(token);

                    if (token.Type == TokenType.END_OF_CODE) break;
                }

                return _tokens;
            }
        }

        private bool IsEndOfLine()
        {
            return _currentPosition == _currentLine.Length;
        }

        private void NextLine()
        {
            if (!IsEndOfSourceCode())
            {
                _line++;
                _currentLine = _sourceCode[_line-1];
                _currentPosition = 0;
            }

        }

        private bool IsEndOfSourceCode()
        {
            if(_line == _sourceCode.Length)
            {
                return true;
            }
            return false;
        }

        private Token GetToken()
        {


            // reaching end of source code

            if (_currentPosition == _currentLine.Length && IsEndOfSourceCode())
            {
                return new Token(TokenType.END_OF_CODE, "\0", _currentPosition, null);
            }

            // reaching end of line

            if (IsEndOfLine())
            {
                NextLine();
            }

            // skip white space
            if (Char.IsWhiteSpace(GetCurrentCharacter()))
            {
                //var startPosition = _currentPosition;


                while (Char.IsWhiteSpace(GetCurrentCharacter()))
                {
                    NextPosition();
                }

                /**
                var lengthOfWhiteSpace = _currentPosition - startPosition;
                string textWhiteSpace = _currentLine.Substring(startPosition, lengthOfWhiteSpace);
                    
                return new Token(TokenType.WHITE_SPACE, textWhiteSpace, startPosition, null);
                **/
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
                string textNumber = _currentLine.Substring(startPosition, lengthOfNumber);

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

            // recognising operators
            if (GetCurrentCharacter() == '+') return new Token(TokenType.PLUS, "+", _currentPosition++, null);
                
            if (GetCurrentCharacter() == '-') return new Token(TokenType.MINUS, "-", _currentPosition++, null);
                
            if (GetCurrentCharacter() == '*') return new Token(TokenType.ASTERISK, "*", _currentPosition++, null);


            // recognising longer operators
            if (GetCurrentCharacter() == '/')
            {
                // skip line comment
                if (PeekNextCharacter() == '/')
                {
                    /**var startPosition = _currentPosition;

                    while (!IsEndOfLine()) 
                    {
                        NextPosition();
                    }**/

                    //int lengthOfComment = _currentPosition - startPosition;
                    //string textComment = _currentLine.Substring(startPosition, lengthOfComment);

                    NextLine();
                }
                else
                {
                    return new Token(TokenType.SLASH, "/", _currentPosition++, null);
                }
            }

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
                string textIdentifier = _currentLine.Substring(startPosition, lengthOfIdentifier);
                
                return new Token(TokenType.IDENTIFIER, textIdentifier, startPosition, null);
            }

            // recognising reserved words

            // unknown characters

            return new Token(TokenType.UNKNOWN_TOKEN, _currentLine.Substring(_currentPosition , 1), _currentPosition++, null);


        }

    }
}
