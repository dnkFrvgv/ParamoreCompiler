using Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scanner.Interfaces;
using Scanner.Enums;

namespace Compiler
{
    public class ParamoreCompiler
    {
        private ILexer _lexer;
        private bool _hasError = false;

        public ParamoreCompiler(string FilePath, ILexer lexer)
        {
            _lexer = lexer;
            RunFromFile(FilePath);
        }

        public ParamoreCompiler(ILexer lexer)
        {
            _lexer = lexer;
            RunPrompt();
        }

        private void RunFromFile(string FilePath)
        {
            try
            {
                string[] sourceCode = File.ReadAllLines(FilePath);
                Run(sourceCode);

            }
            catch (NullReferenceException e ){
                Console.WriteLine($"{e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }

        private void RunPrompt()
        {
            while (true)
            {
                if (_hasError) break;

                Console.Write("** ");

                var code = Console.ReadLine();

                if (code != null)
                {
                    string[] sourceCode = new string[1] { code };
                    Run(sourceCode);
                }


                if (string.IsNullOrEmpty(code))
                {
                    break;
                }
            }
        }

        private void Run(string[] sourceCode)
        {

            _lexer.AddSourceCode(sourceCode);
            var tokens = _lexer.GetTokenList();

            foreach(var token in tokens)
            {
                if (token.Type == TokenType.UNKNOWN_TOKEN)
                {
                    Error(token.Position, "Unknown character", token.Lexeme);
                    break;

                }
                Console.WriteLine($"Token ** {token.Type} ** {token.Lexeme}");
            }

        }

        private void Error(int line, string message, object literalValue)
        {
            Console.WriteLine($"\"[line " + literalValue + "] Error:" + message);
            _hasError = true;
        }
    }
}
