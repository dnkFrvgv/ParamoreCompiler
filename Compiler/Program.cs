using Compiler;
using Scanner;


var filepath = "C:/Users/domis/OneDrive/Documentos/CodeLearning/ParamoreCompiler/test.txt";

Lexer lexer = new Lexer();
ParamoreCompiler compiler = new ParamoreCompiler(filepath, lexer);