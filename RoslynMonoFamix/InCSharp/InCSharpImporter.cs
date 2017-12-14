﻿using Fame;
using Microsoft.CodeAnalysis;
using FAMIX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMonoFamix.InCSharp
{
    public class InCSharpImporter
    {
        private Repository repository;

        public InCSharpImporter(Repository repository)
        {
            this.repository = repository;
            this.Methods = new NamedEntityAccumulator<Method>();
            this.Types = new NamedEntityAccumulator<FAMIX.Type>();
            this.Attributes = new NamedEntityAccumulator<FAMIX.Attribute>();
        }

        public NamedEntityAccumulator<FAMIX.Type> Types { get; set; }
        public NamedEntityAccumulator<Method> Methods { get; set; }
        public NamedEntityAccumulator<FAMIX.Attribute> Attributes { get; set; }

        public Method EnsureMethod(String methodFullName, IMethodSymbol aMethod)
        {
            if (Methods.has(methodFullName))
                return Methods.Named(methodFullName);
            
            Method method = repository.NewInstance<Method>("FAMIX.Method");
            Methods.Add(methodFullName, method);
            return method;
        }

        public FAMIX.Type EnsureType(String fullName, INamedTypeSymbol aType, String typeKind)
        {
            if (Types.has(fullName))
                return Types.Named(fullName);

            FAMIX.Type type = repository.NewInstance<FAMIX.Type>(typeKind);
            
            Types.Add(fullName, type);
            return type;
        }

        public FAMIX.Attribute EnsureAttribute (String attributeFullName, IFieldSymbol field)
        {
            if (Attributes.has(attributeFullName))
                return Attributes.Named(attributeFullName);
            FAMIX.Attribute attribute = repository.NewInstance<FAMIX.Attribute>("FAMIX.Attribute");

            attribute.name = field.Name;
            Attributes.Add(attributeFullName, attribute);
            return attribute;
        }

        public T CreateNewAssociation<T>(String typeName) => repository.NewInstance<T>(typeName);
    }
}
