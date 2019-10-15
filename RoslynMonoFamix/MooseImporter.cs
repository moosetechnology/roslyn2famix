using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using System;
using FAMIX;
using Model;
using Fame;


namespace RoslynMonoFamix {

    /*
        The VisualBasicMooseImporter delegates to VBASTVisitor for loading 
     */
    public class VisualBasicMooseImporter : MooseImporter {
        public override void Visit(SyntaxNode node) {
            var visitor = new VBASTVisitor(semanticModel, importer);
            visitor.Visit(node);
        }
    }

    /*
        The VisualBasicMooseImporter delegates to CSharpVisitor for loading 
     */
    public class CSharpMooseImporter : MooseImporter {
        public override void Visit(SyntaxNode node) {
            var visitor = new CSharpASTVisitor(semanticModel, importer);
            visitor.Visit(node);
        }
    }

    /*
        The moose importer has the responsibility of opening a solution file, gather the documents / files, 
        creating a AST and visiting for loading all the information of this project into a Famix Metamodel   
     */
    public abstract class MooseImporter {
        internal SemanticModel semanticModel;
        internal InCSharp.InCSharpImporter importer;
        public Repository metamodel;

        /* Static constructors. Creates a VB or C# specific importer */
        public static MooseImporter VBImporter() {
            return new VisualBasicMooseImporter();
        }
        public static MooseImporter CSImporter() {
            return new CSharpMooseImporter();
        }

        /*
            Method to be defined according the kind of project. Nodes in each languages  are not compatible. 
         */
        public abstract void Visit(SyntaxNode node);


        /*
           Given the path to a solution file, import will open the solution and it internal projects. 
           For each of the documents inside this sucessive projects, its going to vist it AST representation 
         */
        public Repository import(string solutionPath) {
            //The code that causes the error goes here.
            metamodel = FamixModel.Metamodel();
            var msWorkspace = MSBuildWorkspace.Create();

            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
            string ignoreFolder = this.PrivateDirectoryNameFor(solutionPath);
            importer = new InCSharp.InCSharpImporter(metamodel, ignoreFolder);

            var documents = new List<Document>();
            for (int i = 0; i < solution.Projects.Count<Project>(); i++) {
                var project = solution.Projects.ElementAt<Project>(i);
                for (int j = 0; j < project.Documents.Count<Document>(); j++) {
                    var document = project.Documents.ElementAt<Document>(j);
                    if (document.SupportsSyntaxTree) {
                        this.log("(project " + (i + 1) + " / " + solution.Projects.Count<Project>() + ")");
                        this.logln("(document " + (j + 1) + " / " + project.Documents.Count<Document>() + " " + document.FilePath + ")");
                        var syntaxTree = document.GetSyntaxTreeAsync().Result;
                        var compilationAsync = project.GetCompilationAsync().Result;
                        semanticModel = compilationAsync.GetSemanticModel(syntaxTree);
                        this.Visit(syntaxTree.GetRoot());
                    }
                }
            }
            return metamodel;
        }

        /*
          Log functions are here just to be able to easily override and/or get rid of them 
        */
        private void log(string str) {
            System.Console.Write(str);
        }
        private void logln(string str) {
            System.Console.WriteLine(str);
        }

        /*
          Auxiliar function. Given a solution path (directory + file) it returns the name of the containing folder. 
        */
        private string PrivateDirectoryNameFor(string solutionPath) {
            Uri uri = null;
            try {
                uri = new Uri(solutionPath); ;
            } catch (UriFormatException e) {
                var currentFolder = new Uri(Environment.CurrentDirectory + "\\");
                uri = new Uri(currentFolder, solutionPath.Replace("\\", "/"));
                Console.WriteLine(e.StackTrace);
            }
            return Path.GetDirectoryName(uri.AbsolutePath);
        }
    }
   
}