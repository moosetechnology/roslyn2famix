using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using RoslynMonoFamix.ModelBuilder;
using System.Linq;

namespace FAMIX {

    public abstract class TypingContext : FAMIX.Entity {
        

        public TypingContext (ISymbol relatedSymbol) {
            this.RelatedSymbol = relatedSymbol;
        }
        private ISymbol RelatedSymbol { set; get; }

        public virtual ISymbol GetRelatedSymbol() {
            return RelatedSymbol;
        }

        protected abstract void SetType(FAMIX.Type type);
        
        public static TypingContext Method(FAMIX.Method method, IMethodSymbol relatedSymbol) {
            return new MethodTypingContext(method, relatedSymbol);
        }
        public static TypingContext StructuralEntity (FAMIX.StructuralEntity entity, ISymbol symbol) {
            return new AttributeTypingContext(entity, symbol);
        }
        public static TypingContext StructuralEntityGroup(FAMIX.StructuralEntityGroup entity, ISymbol symbol) {
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
        public static TypingContext TypeBoundary(TypeBoundary typeBoundary, INamedTypeSymbol symbol) {
            return new TypeBoundaryTypingContext(typeBoundary, symbol);
        }

        internal static TypingContext Implements(Implements implements) {
            return new ImplementsTypingContext(implements);
        }

        public static NullContext NullContext() {
            return new NullContext();
        }

     
    }

    public class NullContext : TypingContext {
        public NullContext() : base(null) { }
        protected override void SetType(Type type) {
         
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
        protected override void SetType(Type type) {
            method.returnType = type;
        }
    }

    public class TypeBoundaryTypingContext : TypingContext {
        protected FAMIX.TypeBoundary entity;

        public TypeBoundaryTypingContext(FAMIX.TypeBoundary typeBoundary, INamedTypeSymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = typeBoundary;
        }
        protected override void SetType(Type type) {
            entity.BoundaryType = type;
        }
    }

    public class InheritanceTypingContext : TypingContext {
        protected FAMIX.Inheritance entity;

        public InheritanceTypingContext(FAMIX.Inheritance entity, INamedTypeSymbol relatedSymbol) : base(relatedSymbol.BaseType) {
                this.entity = entity;
        }
        protected override void SetType(Type type) {
            entity.SetSuperType(type);
        }
    }

    public class ImplementsTypingContext : TypingContext {
        protected FAMIX.Implements entity;
        protected List<INamedTypeSymbol> relatedSymbols = new List<INamedTypeSymbol>();
        public ImplementsTypingContext(FAMIX.Implements entity) : base(null) {
            this.entity = entity;
        }

        public void AddSymbols(List<INamedTypeSymbol> symbols) {
            this.relatedSymbols.AddRange(symbols);
        }

        public override FAMIX.Type TypeUsing(VisualBasicModelBuilder importer) {
            List<FAMIX.Type> types = this.relatedSymbols.Select(s => importer.EnsureType(s)).ToList();
            this.SetTypes(types);
            return null;
        }
        protected void SetTypes(List<FAMIX.Type> types) {
            entity.SetImplementedInterface(types);
        }
        protected override void SetType(Type type) {
            throw new System.Exception("This context is a many-types context ");
        }
    }

    

    public class AttributeTypingContext : TypingContext {
        protected FAMIX.StructuralEntity entity;

        public AttributeTypingContext(FAMIX.StructuralEntity entity, ISymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = entity;
        }
        protected override void SetType(Type type) {
            entity.declaredType = type;
        }
    }

    public class AttributeGroupTypingContext : TypingContext {
        protected FAMIX.StructuralEntityGroup entity;

        public AttributeGroupTypingContext(FAMIX.StructuralEntityGroup entity, ISymbol relatedSymbol) : base(relatedSymbol) {
            this.entity = entity;
        }
        protected override void SetType(Type type) {
        }
        public override FAMIX.Type TypeUsing(VisualBasicModelBuilder importer) {
            return entity.Type();
        }
    }



}
