 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace RoslynMonoFamix.Visitor {
    

    class VisualBasicStackedVisitor : VisualBasicSyntaxWalker {
        protected System.Collections.Stack stack;

        public VisualBasicStackedVisitor  () {
            stack = new System.Collections.Stack();
        } 
        public T CurrentContextIfNone<T>(Func<T> IfNone) where T : FAMIX.Entity {
            if (stack.Count == 0 || !(stack.Peek() is T)) {
                return IfNone();
            }
            return stack.Peek() as T;
        }

        public T CurrentContext<T>() where T : FAMIX.Entity {
            return this.CurrentContextIfNone<T>(() => throw new System.Exception("Empty Stack!"));
        }

        public FAMIX.Method CurrentMethod() {
            return (FAMIX.Method) stack.ToArray().First(m => m is FAMIX.Method);
        }
        public T CurrentContextOrNull<T>() where T : FAMIX.Entity {
            return this.CurrentContextIfNone<T>(() => null);
        }
        public void PushContext(FAMIX.Entity context) {
            stack.Push(context);
        }
        public FAMIX.Entity PopContext() {
            if (stack.Count == 2) {
                var a = "";
            }
            return stack.Pop() as FAMIX.Entity;
        }


    }
}
