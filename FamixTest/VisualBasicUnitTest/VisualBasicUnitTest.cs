using System;
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

        public void Import (string text) {
            tree = VisualBasicSyntaxTree.ParseText(text);
            compilation = VisualBasicCompilation.Create("Test");
            compilation.AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location));
            compilation.AddSyntaxTrees(tree);
            model = compilation.GetSemanticModel(tree);
            importer = new VisualBasicMooseImporter();
            importer.Import(tree, model);
            MetaModel = importer.MetaModel;
        }
        

    }
}
