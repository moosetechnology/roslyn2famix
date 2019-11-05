using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {

    public abstract class TypingContext : FAMIX.Entity {
        
        public TypingContext (ISymbol relatedSymbol) {
            this.RelatedSymbol = relatedSymbol;
        }
        public ISymbol RelatedSymbol { set; get; }
        public abstract void SetType(FAMIX.Type type);
        
        public static TypingContext Method(FAMIX.Method method, IMethodSymbol relatedSymbol) {
            return new MethodTypingContext(method, relatedSymbol);
        }
        public static TypingContext StructuralEntity (FAMIX.StructuralEntity entity, ISymbol symbol) {
            return new StructuralEntityTypingContext(entity, symbol);
        }

        public static TypingContext Inheritance(FAMIX.Inheritance entity, INamedTypeSymbol symbol) {
            return new InheritanceTypingContext(entity, symbol);
        }
    }

    public class MethodTypingContext : TypingContext {
        protected FAMIX.Method method;
       
        public MethodTypingContext(FAMIX.Method method, IMethodSymbol relatedSymbol) : base(relatedSymbol.ReturnType) {
            this.method = method;
        }
        public override void SetType(Type type) {
            method.returnType = type;
        }
    }

    public class InheritanceTypingContext : TypingContext {
        protected FAMIX.Inheritance entity;

        public InheritanceTypingContext(FAMIX.Inheritance entity, INamedTypeSymbol relatedSymbol) : base(relatedSymbol.BaseType) {
            this.entity = entity;
        }
        public override void SetType(Type type) {
            entity.SetSuperType(type);
        }
    }


    public class StructuralEntityTypingContext : TypingContext {
        protected FAMIX.StructuralEntity entity;

        public StructuralEntityTypingContext(FAMIX.StructuralEntity entity, ISymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = entity;
        }
        public override void SetType(Type type) {
            entity.declaredType = type;
        }
    }


    

}
