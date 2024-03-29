﻿using System;
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
        
        private int _currentLinePosition = 0;
        private int _currentCharPosition = 0;

        private List<Token> _tokens;
        private readonly Dictionary<string, TokenType> _reservedWords = InitialiseReservedWords();

        /**public Lexer(string[] sourceCode)
        {
            _sourceCode = sourceCode;
            _currentLine = _sourceCode[_line-1];
        }**/

        public void AddSourceCode(string[] sourceCodeLines)
        {
            _tokens = new List<Token>();
            _sourceCodeLines = sourceCodeLines;
            _currentLineOfSourceCode = _sourceCodeLines[0];
        }

        private static Dictionary<string, TokenType> InitialiseReservedWords()
        {
            var reservedWords = new Dictionary<string, TokenType>();

            reservedWords.Add("if", TokenType.IF);
            reservedWords.Add("else", TokenType.ELSE);
            reservedWords.Add("return", TokenType.RETURN);
            reservedWords.Add("true", TokenType.TRUE);
            reservedWords.Add("false", TokenType.FALSE);
            reservedWords.Add("var", TokenType.VAR);
            reservedWords.Add("while", TokenType.WHILE);
            reservedWords.Add("for", TokenType.FOR);
            reservedWords.Add("class", TokenType.CLASS);
            reservedWords.Add("and", TokenType.AND);
            reservedWords.Add("or", TokenType.OR);
            reservedWords.Add("print", TokenType.PRINT);
            reservedWords.Add("this", TokenType.THIS);

            return reservedWords;
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
            if (!IsEndOfLine())
            {
                return _currentLineOfSourceCode[_currentCharPosition];
            }
            else
            {
                return '\0';
            }
        }

        public List<Token> GetTokenList()
        {
       
            if (_sourceCodeLines == null)
            {
                throw new NullReferenceException($"SourceCode was not added. Call AddSourceCode() method to initialise it.");
            }
            else
            {

                GenerateTokens();

                return _tokens;
            }
        }

        private bool IsEndOfLine()
        {
            // was the last char of line already tokenised ?
            return _currentCharPosition == _currentLineOfSourceCode.Length;
        }

        private bool IsEndOfSourceCode()
        {
            // is this the last line of code?
            if(_currentLinePosition == _sourceCodeLines.Length - 1)
            {
                return IsEndOfLine();
            }
            return false;
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

        private void GenerateTokens()
        {

            while (true)
            {
                if(IsEndOfSourceCode()) {
                    _tokens.Add(new Token(TokenType.END_OF_CODE, "\0", _currentCharPosition, null));
                    return;
                }
                if (IsEndOfLine())
                {
                    NextLine();
                }
                else
                {
                    switch (GetCurrentCharacter())
                    {
                        // ignoring white spaces
                        case ' ':
                            NextPosition();
                            break;
                        // recognising basic operators
                        case '+':
                            _tokens.Add(new Token(TokenType.PLUS, "+", _currentCharPosition++, null));
                            break;
                        case '-':
                            _tokens.Add(new Token(TokenType.MINUS, "-", _currentCharPosition++, null));
                            break;
                        case '*':
                            _tokens.Add(new Token(TokenType.ASTERISK, "*", _currentCharPosition++, null));
                            break;
                        // recognising longer operators
                        case '/':
                            if (PeekNextCharacter() == '/')
                            {
                                // skip entire line comment
                                NextLine();
                                break;
                            }
                            
                            _tokens.Add(new Token(TokenType.SLASH, "/", _currentCharPosition++, null));
                            break;
                        case '=':
                            if (PeekNextCharacter() == '=')
                            {
                                NextPosition();
                                _tokens.Add(new Token(TokenType.EQUALS_EQUALS, "==", _currentCharPosition++, null));
                                break;
                            }

                            _tokens.Add(new Token(TokenType.EQUALS, "=", _currentCharPosition++, null));
                            break;
                        case '>':
                            if (PeekNextCharacter() == '=')
                            {
                                NextPosition();
                                _tokens.Add(new Token(TokenType.GREATER_EQUALS, ">=", _currentCharPosition++, null));
                                break;
                            }
                            _tokens.Add(new Token(TokenType.GREATER, ">", _currentCharPosition++, null));
                            break;
                        case '<':
                            if (PeekNextCharacter() == '=')
                            {
                                NextPosition();
                                _tokens.Add(new Token(TokenType.LESS_EQUALS, "<=", _currentCharPosition++, null));
                                break;
                            }
                                
                            _tokens.Add(new Token(TokenType.LESS, "<", _currentCharPosition++, null));
                            break;
                        case '&':
                            if (PeekNextCharacter() == '&')
                            {
                                NextPosition();
                                _tokens.Add(new Token(TokenType.AND, "&&", _currentCharPosition++, null));
                                break;
                            }
                            _tokens.Add(new Token(TokenType.UNKNOWN_TOKEN, _currentLineOfSourceCode.Substring(_currentCharPosition, 1), _currentCharPosition++, null));
                            break;                            
                        case '|':
                            if (PeekNextCharacter() == '|')
                            {
                                NextPosition();
                                _tokens.Add(new Token(TokenType.OR, "||", _currentCharPosition++, null));
                                break;
                            }
                            _tokens.Add(new Token(TokenType.UNKNOWN_TOKEN, _currentLineOfSourceCode.Substring(_currentCharPosition, 1), _currentCharPosition++, null));
                            break;
                        // recognising punctuation
                        case '(':
                            _tokens.Add(new Token(TokenType.LEFT_PARENTHESES, "(", _currentCharPosition++, null));
                            break;
                        case ')':
                            _tokens.Add(new Token(TokenType.RIGHT_PARENTHESES, ")", _currentCharPosition++, null));
                            break;
                        case '{':
                            _tokens.Add(new Token(TokenType.LEFT_BRACES, "{", _currentCharPosition++, null));
                            break;
                        case '}':
                            _tokens.Add(new Token(TokenType.RIGHT_BRACES, "}", _currentCharPosition++, null));
                            break;
                        case '[':
                            _tokens.Add(new Token(TokenType.LEFT_BRACKETS, "[", _currentCharPosition++, null));
                            break;
                        case ']':
                            _tokens.Add(new Token(TokenType.RIGHT_BRACKETS, "]", _currentCharPosition++, null));
                            break;
                        case ',':
                            _tokens.Add(new Token(TokenType.COMMA, ",", _currentCharPosition++, null));
                            break;
                        case '.':
                            _tokens.Add(new Token(TokenType.DOT, ".", _currentCharPosition++, null));
                            break;
                        case ';':
                            _tokens.Add(new Token(TokenType.SEMICOLON, ";", _currentCharPosition++, null));
                            break;
                        case ':':
                            _tokens.Add(new Token(TokenType.COLON, ":", _currentCharPosition++, null));
                            break;
                        case '\0':
                            throw new Exception("GenerateTokens method - New line was not handled properly");
                        case '"':
                            // recognise strings
                            ScanString();
                        break;
                        default:
                            if (Char.IsDigit(GetCurrentCharacter()))
                            {
                                ScanIntegerNumber();
                                break;
                            }

                            if (Char.IsLetter(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
                            {
                                ScanIdentifier();
                                break;
                            }


                            _tokens.Add(new Token(TokenType.UNKNOWN_TOKEN, _currentLineOfSourceCode.Substring(_currentCharPosition, 1), _currentCharPosition++, null));
                            break;
                    }
                }
            }

            
        }

        private void ScanIdentifier()
        {
            var startPosition = _currentCharPosition;

            while (Char.IsLetterOrDigit(GetCurrentCharacter()) || GetCurrentCharacter() == '_')
            {
                NextPosition();
            }

            int lengthOfIdentifier = _currentCharPosition - startPosition;
            string textIdentifier = _currentLineOfSourceCode.Substring(startPosition, lengthOfIdentifier);

            if(_reservedWords.ContainsKey(textIdentifier))
            {
                TokenType tokenType = _reservedWords[textIdentifier];

                _tokens.Add(new Token(tokenType, textIdentifier, startPosition, null));
                return;
            }

            _tokens.Add(new Token(TokenType.IDENTIFIER, textIdentifier, startPosition, null));
        }

        private void ScanIntegerNumber()
        {
            int startPosition = _currentCharPosition;

            while (Char.IsDigit(GetCurrentCharacter()))
            {
                NextPosition();
            }

            int lengthOfNumber = _currentCharPosition - startPosition;
            string textNumber = _currentLineOfSourceCode.Substring(startPosition, lengthOfNumber);

            try
            {
                int numberValue = int.Parse(textNumber);
                _tokens.Add(new Token(TokenType.NUMBER, textNumber, startPosition, numberValue));
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Invalid integer at {startPosition} {ex}.");
            }
        }

        private void ScanString()
        {
            var stringStartPosition = _currentCharPosition;
            var stringStartLine = _currentLinePosition;

            TraverseString();
            NextPosition();


            // if is not on the string literal
            if (stringStartLine == _currentLinePosition)
            {
                int lengthOfstring = _currentCharPosition - stringStartPosition;
                string textIdentifier = _currentLineOfSourceCode.Substring(stringStartPosition, lengthOfstring);

                _tokens.Add(new Token(TokenType.STRING, textIdentifier, stringStartPosition, null));
            }
            else
            {
                ScanStringLiteral(stringStartLine, stringStartPosition);
            }
        }

        private void ScanStringLiteral(int stringStartLine, int stringStartPosition)
        {
            string[] stringLiteralArray = new string[(_currentLinePosition - stringStartLine)+1];
            int arrayIndex = 0;

            // go through each line of this string literal
            for (int line = stringStartLine; line <= _currentLinePosition; line++)
            {
                // first line of string literal
                if (line == stringStartLine)
                {
                    stringLiteralArray[arrayIndex] = _sourceCodeLines[line].Substring(stringStartPosition);
                    arrayIndex++;
                    continue;
                }
                // last line of string literal
                if (line == _currentLinePosition)
                {
                    stringLiteralArray[arrayIndex] = _sourceCodeLines[line].Substring(0, _currentCharPosition);
                    arrayIndex++;
                    continue;
                }
                else
                {
                    stringLiteralArray[arrayIndex] = _sourceCodeLines[line];
                    arrayIndex++;
                    continue;
                }

            }

            string stringLiteralIdentifier = string.Join(" ", stringLiteralArray);

            _tokens.Add(new Token(TokenType.STRING, stringLiteralIdentifier, stringStartPosition, null));
        }

        private void TraverseString()
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
                if(_currentCharPosition == _currentLineOfSourceCode.Length-1 ) break;

            }

            NextPosition();

            //NextPosition();

            // no end "
            /*if(IsEndOfLine() || GetCurrentCharacter() != '"')
            {
                throw new Exception("string is not corrects");

            }*/
        }

    }
}
