using Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scanner.Interfaces;

namespace Compiler
{
    public class ParamoreCompiler
    {
        //private ILexer _lexer;

        public ParamoreCompiler(string FilePath)
        {
            RunFromFile(FilePath);
        }

        public ParamoreCompiler()
        {
            // _lexer = lexer;
            RunPrompt();
        }

        private void RunFromFile(string FilePath)
        {
            try
            {
                string sourceCode = File.ReadAllText(FilePath);
                Run(sourceCode);

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
                Console.Write("** ");

                var code = Console.ReadLine();

                if (string.IsNullOrEmpty(code))
                {
                    break;
                }

                Run(code);
            }
        }

        private void Run(string sourceCode)
        {

            Lexer lexer = new Lexer(sourceCode);

            var tokens = lexer.GetTokenList();

            foreach(var token in tokens)
            {
                Console.WriteLine($"Token ** {token.Type} ** {token.Lexeme}");
            }

        }
    }
}
