using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {

    public interface ITyped {
        TypingContext TypingContext(ISymbol symbol);

    }
}
