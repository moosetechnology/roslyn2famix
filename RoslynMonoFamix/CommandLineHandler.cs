using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;
using System.IO;
using Model;
using System.Text;
using Fame;
using FAMIX;
using RoslynMonoFamix.ModelBuilder;

namespace RoslynMonoFamix {
    class CommandLineHandler {
        private const string CSHARP = "C#";
        private const string VISUALBASIC = "VB";

        static void Main(string[] args) {
            ValidateArgs(args);
            string language = args[0];
            string solutionPath = args[1];
            string exportPath = args[2];

            string path = Assembly.GetAssembly(typeof(CommandLineHandler)).Location;
            Console.WriteLine("--->>>" + path);
            path = path.Replace("RoslynMonoFamix.exe", "");

            MooseImporter importer = null;
            if (language.Equals(VISUALBASIC)) {
                importer = MooseImporter.VBImporter();
            } else {
                importer = MooseImporter.CSImporter();
            }
            try {
                Repository metamodel = importer.ImportProject(solutionPath);
                metamodel.ExportMSEFile(exportPath);
            } catch (ReflectionTypeLoadException ex) {
                Console.WriteLine(BuildErrorStringMessage(ex));
                //Display or log the error based on your application.
            }
        }

   
        private static string BuildErrorStringMessage(ReflectionTypeLoadException ex) {
            StringBuilder sb = new StringBuilder();
            foreach (System.Exception exSub in ex.LoaderExceptions) {
                sb.AppendLine(exSub.Message);
                FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                if (exFileNotFound != null) {
                    if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
                        sb.AppendLine("Fusion Log:");
                        sb.AppendLine(exFileNotFound.FusionLog);
                    }
                }
                sb.AppendLine();
            }
            return  sb.ToString();

        }
        private static void ValidateArgs (string[] args) {
            if (args.Length != 3 ) {
                throw new System.Exception(" Error on arguments. {VB|C#} path/to/sln path/to/mse/destination was expected");
            }

        }
    }
}
