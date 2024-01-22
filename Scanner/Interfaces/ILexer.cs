﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Interfaces
{
    public interface ILexer
    {
        List<Token> GetTokenList();
        void AddSourceCode(string[] sourceCode);
    }
}
