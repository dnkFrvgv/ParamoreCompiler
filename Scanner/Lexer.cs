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
        private string[] _sourceCodeLines;
        private string _currentLineOfSourceCode;
        //private int _start = 0;
        private int _currentLinePosition = 0;
        private int _currentCharPosition = 0;
        private List<Token> _tokens = new List<Token>();

        /**public Lexer(string[] sourceCode)
        {
            _sourceCode = sourceCode;
            _currentLine = _sourceCode[_line-1];
        }**/

        public Lexer(){}

        public void AddSourceCode(string[] sourceCodeLines)
        {
            _sourceCodeLines = sourceCodeLines;
            _currentLineOfSourceCode = _sourceCodeLines[0];
        }
      
        private void NextPosition() {
            _currentCharPosition++;
        }

        private char PeekNextCharacter()
        {
            if (_currentCharPosition + 1 >= _currentLineOfSourceCode.Length)
            {
                return '\0';
            }

            return _currentLineOfSourceCode[_currentCharPosition + 1];
        }


        private char GetCurrentCharacter()
        {
            if (_currentCharPosition >= _currentLineOfSourceCode.Length)
            {
                return '\0';
            }

            return _currentLineOfSourceCode[_currentCharPosition];
        }

        public List<Token> GetTokenList()
        {
       
            if (_sourceCodeLines == null)
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
            return _currentCharPosition == _currentLineOfSourceCode.Length;
        }

        private void NextLine()
        {
            if (!IsEndOfSourceCode())
            {
                _currentLinePosition++;
                _currentLineOfSourceCode = _sourceCodeLines[_currentLinePosition];
                _currentCharPosition = 0;
            }

        }

        private bool IsEndOfSourceCode()
        {
            if(_currentLinePosition++ == _sourceCodeLines.Length)
            {
                // check if is the last char of this line
                return true;
            }
            return false;
        }

        private Token GetToken()
        {


            // reaching end of source code

            if (_currentCharPosition == _currentLineOfSourceCode.Length && IsEndOfSourceCode())
            {
                return new Token(TokenType.END_OF_CODE, "\0", _currentCharPosition, null);
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
                int startPosition = _currentCharPosition;

                while(Char.IsDigit(GetCurrentCharacter()))
                {
                    NextPosition();
                }
                 
                int lengthOfNumber = _currentCharPosition - startPosition;
                string textNumber = _currentLineOfSourceCode.Substring(startPosition, lengthOfNumber);

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
            if (GetCurrentCharacter() == '+') return new Token(TokenType.PLUS, "+", _currentCharPosition++, null);
                
            if (GetCurrentCharacter() == '-') return new Token(TokenType.MINUS, "-", _currentCharPosition++, null);
                
            if (GetCurrentCharacter() == '*') return new Token(TokenType.ASTERISK, "*", _currentCharPosition++, null);


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
                    return new Token(TokenType.SLASH, "/", _currentCharPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '=')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.EQUALS_EQUALS, "==", _currentCharPosition++, null);
                }
                else
                {
                    return new Token(TokenType.EQUALS, "=", _currentCharPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '>')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.GREATER_EQUALS, ">=", _currentCharPosition++, null);
                }
                else
                {
                    return new Token(TokenType.GREATER, ">", _currentCharPosition++, null);
                }
            }
            
            if (GetCurrentCharacter() == '<')
            {
                if (PeekNextCharacter() == '=')
                {
                    NextPosition();
                    return new Token(TokenType.LESS_EQUALS, "<=", _currentCharPosition++, null);
                }
                else
                {
                    return new Token(TokenType.LESS, "<", _currentCharPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '&')
            {
                if (PeekNextCharacter() == '&')
                {
                    NextPosition();
                    return new Token(TokenType.AND, "&&", _currentCharPosition++, null);
                }
            }

            if (GetCurrentCharacter() == '|')
            {
                if (PeekNextCharacter() == '|')
                {
                    NextPosition();
                    return new Token(TokenType.OR, "||", _currentCharPosition++, null);
                }
            }

            // recognising punctuation

            if (GetCurrentCharacter() == '(') return new Token(TokenType.LEFT_PARENTHESES, "(", _currentCharPosition++, null);
            if (GetCurrentCharacter() == ')') return new Token(TokenType.RIGHT_PARENTHESES, ")", _currentCharPosition++, null);
            if (GetCurrentCharacter() == '{') return new Token(TokenType.LEFT_BRACES, "{", _currentCharPosition++, null);
            if (GetCurrentCharacter() == '}') return new Token(TokenType.RIGHT_BRACES, "}", _currentCharPosition++, null);
            if (GetCurrentCharacter() == '[') return new Token(TokenType.LEFT_BRACKETS, "[", _currentCharPosition++, null);
            if (GetCurrentCharacter() == ']') return new Token(TokenType.RIGHT_BRACKETS, "]", _currentCharPosition++, null);
            if (GetCurrentCharacter() == ',') return new Token(TokenType.COMMA, ",", _currentCharPosition++, null);
            if (GetCurrentCharacter() == '.') return new Token(TokenType.DOT, ".", _currentCharPosition++, null);
            if (GetCurrentCharacter() == ';') return new Token(TokenType.SEMICOLON, ";", _currentCharPosition++, null);
            if (GetCurrentCharacter() == ':') return new Token(TokenType.COLON, ":", _currentCharPosition++, null);

            // recognising identifies

            if (Char.IsLetter(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
            {                
                var startPosition = _currentCharPosition;

                while (Char.IsLetterOrDigit(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
                {
                    NextPosition();
                }

                int lengthOfIdentifier = _currentCharPosition - startPosition;
                string textIdentifier = _currentLineOfSourceCode.Substring(startPosition, lengthOfIdentifier);
                
                return new Token(TokenType.IDENTIFIER, textIdentifier, startPosition, null);
            }

            // recognising reserved words

            // string literals

            if (GetCurrentCharacter() == '"')
            {

                var stringStartPosition = _currentCharPosition;
                var stringStartLine = _currentLinePosition;

                //NextPosition();
                CheckString();

                // if \n is not on the string
                if(stringStartLine == _currentLinePosition)
                {
                    int lengthOfstring = _currentCharPosition - stringStartPosition;
                    string textIdentifier = _currentLineOfSourceCode.Substring(stringStartPosition, lengthOfstring);

                    return new Token(TokenType.STRING, textIdentifier, stringStartPosition, null);
                }
                else
                {

                    // go through each line of this multiple line string
                    for(int i = stringStartLine; i < _currentLinePosition - stringStartLine; i++)
                    {

                    }
                }

            }

            // unknown characters

            return new Token(TokenType.UNKNOWN_TOKEN, _currentLineOfSourceCode.Substring(_currentCharPosition , 1), _currentCharPosition++, null);


        }

        public void CheckString()
        {
            while (PeekNextCharacter() != '"')
            {
                // line break
                if (PeekNextCharacter() == '\\')
                {
                    NextPosition();
                    if (PeekNextCharacter() == 'n')
                    {
                        NextLine();
                        continue;
                    }
                }

                NextPosition();
                if (IsEndOfLine()) break;

            }

            NextPosition();

            // no end "
            /*if(IsEndOfLine() || GetCurrentCharacter() != '"')
            {
                throw new Exception("string is not corrects");

            }*/
        }

    }
}
