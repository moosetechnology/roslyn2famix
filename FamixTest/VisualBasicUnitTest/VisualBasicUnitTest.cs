using System;
using System.Linq;
using Fame;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynMonoFamix;

namespace FamixTest.VisualBasicUnitTest {
    public class VisualBasicUnitTest {
        public SyntaxTree tree;
        public Compilation compilation;
        public SemanticModel model;
        public VisualBasicMooseImporter importer;
        public Repository MetaModel; 

        public void AssertAmountElements (int amount) {
            Assert.AreEqual(amount, MetaModel.GetElements().Count);
        }
        public T GetElement<T>(int i) where T : FAMIX.Entity {
            return (T)MetaModel.GetElements()[i];
        }
        public void Import (string text) {

            tree = VisualBasicSyntaxTree.ParseText(text);

            VisualBasicCompilationOptions options = new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug).WithParseOptions(new VisualBasicParseOptions(kind:SourceCodeKind.Regular));
            options = options.WithConcurrentBuild(false);


            compilation = VisualBasicCompilation.Create("Test", new[] { tree });
            compilation = compilation.WithOptions(options);
            compilation.AddReferences( MetadataReference.CreateFromFile(typeof(object).Assembly.Location));


            model = compilation.GetSemanticModel(tree);
            importer = new VisualBasicMooseImporter();
            importer.Import(tree, model);
            MetaModel = importer.MetaModel;
        }

        public FAMIX.Method MethodOfSignature(String signature) {
          return  importer.AllElementsOfType<FAMIX.Method>().First(m => m.signature == signature);
        }
    }
}
