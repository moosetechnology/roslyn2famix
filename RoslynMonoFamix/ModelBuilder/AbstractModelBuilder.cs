using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMonoFamix.ModelBuilder {

    public class AbstractModelBuilder {
        public NamedEntityAccumulator<FAMIX.Namespace> Namespaces { get; set; }
        public NamedEntityAccumulator<FAMIX.Type> Types { get; set; }
        public NamedEntityAccumulator<FAMIX.Method> Methods { get; set; }
        public NamedEntityAccumulator<FAMIX.StructuralEntity> Attributes { get; set; }

        public SemanticModel model { get; set; }
        protected Fame.Repository repository;
        protected ImportingHelper helper = new ImportingHelper();
        protected string projectBaseFolder;

        public AbstractModelBuilder(Fame.Repository repository, string projectBaseFolder) {
          
            this.repository = repository;
            this.Methods = new NamedEntityAccumulator<FAMIX.Method>();
            this.Types = new NamedEntityAccumulator<FAMIX.Type>();
            this.Namespaces = new NamedEntityAccumulator<FAMIX.Namespace>();
            this.Attributes = new NamedEntityAccumulator<FAMIX.StructuralEntity>();
            this.projectBaseFolder = projectBaseFolder;
        }
        public T CreateNewEntity<T>(String typeName) {
            return repository.New<T>(typeName);
        }
        private void LinkWithInheritance(FAMIX.Class subClass, FAMIX.Class superClass) {
            FAMIX.Inheritance inheritance = CreateNewEntity<FAMIX.Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = subClass;
            inheritance.superclass = superClass;
            superClass.AddSubInheritance(inheritance);
            subClass.AddSuperInheritance(inheritance);
        }
        public IEnumerable<T> AllElementsOfType<T>() {
            return repository.GetElements().OfType<T>();
        }




    }
}
