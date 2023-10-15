using Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class ParamoreCompiler
    {
        public ParamoreCompiler(string FilePath)
        {
            RunFromFile(FilePath);
        }

        public ParamoreCompiler()
        {
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

        private void Run(string SourceCode)
        {
            Lexer lexer = new Lexer(SourceCode);


            lexer.Tokenise();

            List<Token> tokens = lexer.GetTokens();

            Console.WriteLine(tokens.Count());


            foreach (Token token in tokens)
            {
                Console.WriteLine($"Token ** {token.Type} ** {token.Word}");
            }
        }
    }
}
