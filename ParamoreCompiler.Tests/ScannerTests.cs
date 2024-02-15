using Scanner;
using Scanner.Enums;

namespace ParamoreCompiler.Tests
{
    public class Scanner
    {
        [Fact]
        public void TestBasicString()
        {
            // arrange 
            string[] lines = new string[1]{"\"this is code\""};

            int numberOfTokens = 1;
            int endOfCodeToken = 1;

            var scanner = new Lexer();
            scanner.AddSourceCode(lines);

            // act
            var tokenlist = scanner.GetTokenList();

            // assert
            Assert.Equal(numberOfTokens + endOfCodeToken, tokenlist.Count());
            Assert.Equal(TokenType.STRING, tokenlist.First().Type);

        }
    }
}