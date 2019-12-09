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
using RoslynMonoFamix.ModelBuilder;
using RoslynMonoFamix.Visitor;
using Microsoft.Build.Framework;

namespace RoslynMonoFamix {

    /*
        The VisualBasicMooseImporter delegates to VBASTVisitor for loading 
     */
    public class VisualBasicMooseImporter : MooseImporter {
        public override void InitializeForImport() {
            importer = new ModelBuilder.VisualBasicModelBuilder(_repository, ignoreFolder);
        }
        public override void Visit(SyntaxNode node) {
            var visitor = new VBImportingVisitor(importer as VisualBasicModelBuilder);
            visitor.Visit(node);
        }
    }

    /*
        The VisualBasicMooseImporter delegates to CSharpVisitor for loading 
     */
    public class CSharpMooseImporter : MooseImporter {
        public override void InitializeForImport() {
            importer = new ModelBuilder.InCSharpImporter(_repository, ignoreFolder);
        }
        public override void Visit(SyntaxNode node) {
            var visitor = new CSharpASTVisitor(importer as InCSharpImporter);
            visitor.Visit(node);
        }
    }

    /*
        The moose importer has the responsibility of opening a solution file, gather the documents / files, 
        creating a AST and visiting for loading all the information of this project into a Famix Metamodel   
     */
    public abstract class MooseImporter {
        
        internal ModelBuilder.AbstractModelBuilder importer;
        internal Repository _repository;
        internal string ignoreFolder;
        public Repository MetaModel { get { return _repository; } }
        
        public NamedEntityAccumulator<Method> Methods { get { return importer.Methods; } }
        public NamedEntityAccumulator<FAMIX.StructuralEntity> Attributes { get { return importer.Attributes; } }
        public NamedEntityAccumulator<FAMIX.Type> Types { get { return importer.Types; } }
        public IEnumerable<T> AllElementsOfType<T>() {
            return importer.AllElementsOfType<T>();
        }
        
        public MooseImporter() {
            _repository = FamixModel.Metamodel();
            ignoreFolder = "";
            this.InitializeForImport();
        }

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
        public abstract void InitializeForImport();

        /*
           Given the path to a solution file, import will open the solution and it internal projects. 
           For each of the documents inside this sucessive projects, its going to vist it AST representation 
         */
        public Repository ImportProject(string solutionPath) {
            //The code that causes the error goes here.

            var msWorkspace = MSBuildWorkspace.Create(new Dictionary<string, string>() { { "Configuration", "Debug" }, { "Platform", "AnyCPU" }, { "CheckForSystemRuntimeDependency", "true" } });

            var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
            ignoreFolder = this.PrivateDirectoryNameFor(solutionPath);

            this.InitializeForImport();
            MetadataReference mscoreLibReference =
           AssemblyMetadata
               .CreateFromFile(typeof(string).Assembly.Location)
               .GetReference();


            var documents = new List<Document>();
            for (int i = 0; i < solution.Projects.Count<Project>(); i++) {
                var project = solution.Projects.ElementAt<Project>(i);

                var compilationAsync = project.GetCompilationAsync().Result;
                compilationAsync.AddReferences(
                                      MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
                        ).AddReferences(
                                      MetadataReference.CreateFromFile(typeof(RequiredAttribute).Assembly.Location));
                var diagnostics = compilationAsync.GetDiagnostics();

                for (int j = 0; j < project.Documents.Count<Document>(); j++) {
                    var document = project.Documents.ElementAt<Document>(j);
                    if (document.SupportsSyntaxTree) {
                        this.log("(project " + (i + 1) + " / " + solution.Projects.Count<Project>() + ")");
                        this.logln("(document " + (j + 1) + " / " + project.Documents.Count<Document>() + " " + document.FilePath + ")");
                        var tree = document.GetSyntaxTreeAsync().Result;
                        var semanticModel = compilationAsync.GetSemanticModel(tree);
                        var model = semanticModel.GetDiagnostics();
                        this.Import(tree, semanticModel);
                    }
                }
            }
            return _repository;
        }
        public void Import(SyntaxTree tree, SemanticModel model) {
            importer.model = model;
            this.Visit(tree.GetRoot());
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
                uri = new Uri(solutionPath);
            } catch (UriFormatException e) {
                var currentFolder = new Uri(Environment.CurrentDirectory + "\\");
                uri = new Uri(currentFolder, solutionPath.Replace("\\", "/"));
                Console.WriteLine(e.StackTrace);
            }
            return Path.GetDirectoryName(uri.AbsolutePath);
        }
    }

}