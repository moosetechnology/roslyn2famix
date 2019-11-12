using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using RoslynMonoFamix.ModelBuilder;

namespace FAMIX {

    public abstract class TypingContext : FAMIX.Entity {
        
        public TypingContext (ISymbol relatedSymbol) {
            this.RelatedSymbol = relatedSymbol;
        }
        private ISymbol RelatedSymbol { set; get; }

        public virtual ISymbol GetRelatedSymbol() {
            return RelatedSymbol;
        }

        public abstract void SetType(FAMIX.Type type);
        
        public static TypingContext Method(FAMIX.Method method, IMethodSymbol relatedSymbol) {
            return new MethodTypingContext(method, relatedSymbol);
        }
        public static TypingContext StructuralEntity (FAMIX.StructuralEntity entity, ISymbol symbol) {
            return new AttributeTypingContext(entity, symbol);
        }
        public static TypingContext StructuralEntityGroup(FAMIX.AttributeGroup entity, ISymbol symbol) {
            return new AttributeGroupTypingContext(entity, symbol);
        }
        public static TypingContext Inheritance(FAMIX.Inheritance entity, INamedTypeSymbol symbol) {
            return new InheritanceTypingContext(entity, symbol);
        }

        public virtual FAMIX.Type TypeUsing(VisualBasicModelBuilder importer) {
            FAMIX.Type type = importer.EnsureType(this.RelatedSymbol);
            this.SetType(type);
            return type;
        }

        public static NullContext NullContext() {
            return new NullContext();
        }
    }

    public class NullContext : TypingContext {
        public NullContext() : base(null) { }
        public override void SetType(Type type) {
         
        }
        public override FAMIX.Type TypeUsing(VisualBasicModelBuilder importer) {
            return null;
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

    public class AttributeTypingContext : TypingContext {
        protected FAMIX.StructuralEntity entity;

        public AttributeTypingContext(FAMIX.StructuralEntity entity, ISymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = entity;
        }
        public override void SetType(Type type) {
            entity.declaredType = type;
        }
    }

    public class AttributeGroupTypingContext : TypingContext {
        protected FAMIX.AttributeGroup entity;

        public AttributeGroupTypingContext(FAMIX.AttributeGroup entity, ISymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = entity;
        }
        public override void SetType(Type type) {
            entity.SetType(type);
        }
        public override FAMIX.Type TypeUsing(VisualBasicModelBuilder importer) {
            return entity.Type();
        }
    }



}
