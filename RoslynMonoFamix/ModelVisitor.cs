﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Fame;
using Model;
using Microsoft.CodeAnalysis;

public class ModelVisitor : CSharpSyntaxWalker
{

    private Repository metamodel;
    private SemanticModel semanticModel;

    public ModelVisitor(Repository metamodel, SemanticModel semanticModel)
    {
        this.metamodel = metamodel;
        this.semanticModel = semanticModel;
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        Class aClass = metamodel.NewInstance<Class>("FAMIX.Class");
		
        string className = node.Identifier.ToString();
		aClass.Name = className;

		Console.WriteLine(className);
        base.VisitClassDeclaration(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        Method aMethod = metamodel.NewInstance<Method>("FAMIX.Method");
        string methodName = node.Identifier.ToString();
        Console.WriteLine("\t" + methodName);
        var methodSymbol = semanticModel.GetDeclaredSymbol(node);
        Console.WriteLine("\t\t" + methodSymbol.IsAbstract);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        foreach (var variable in node.Declaration.Variables)
        {
            Model.Attribute anAttribute = metamodel.NewInstance<Model.Attribute>("FAMIX.Attribute");
            string attributeName = variable.Identifier.ToString();
            Console.WriteLine("\t" + attributeName);
        }
        base.VisitFieldDeclaration(node);
    }

}