
using Microsoft.CodeAnalysis.MSBuild;

using System.Reflection;

using System;

using Model;
using Fame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RoslynMonoFamix;


namespace FamixTest {
    [TestClass]
    public class SampleSystemCSharpLoader {
        protected Repository metamodel = FamixModel.Metamodel();
        protected MooseImporter importer = null;

        [TestInitialize]
        public void LoadSampleSystem() {
            string path = Assembly.GetAssembly(typeof(SampleSystemCSharpLoader)).Location;
            path = path.Replace("FamixTest.dll", "");
            string solutionPath = path + "../../../SampleCode/SampleCode.sln";

            importer = MooseImporter.CSImporter();
            Repository metamodel = importer.import(solutionPath);
            metamodel.ExportMSEFile("SampleCode.mse");
        }

    }
}