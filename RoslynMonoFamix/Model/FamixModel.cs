using Fame;
using Model;
using FAMIX;

namespace Model
{
    public class FamixModel
    {
        public static Repository Metamodel()
        {
            Tower t = new Fame.Tower();
            MetaRepository metaRepo = t.metamodel;
            metaRepo.RegisterType(typeof(Entity));
            metaRepo.RegisterType(typeof(SourcedEntity));
            metaRepo.RegisterType(typeof(ContainerEntity));
            metaRepo.RegisterType(typeof(NamedEntity));
            metaRepo.RegisterType(typeof(Attribute));
            metaRepo.RegisterType(typeof(Net.Event));
            metaRepo.RegisterType(typeof(BehaviouralEntity));
            metaRepo.RegisterType(typeof(Class));
            metaRepo.RegisterType(typeof(Method));
            metaRepo.RegisterType(typeof(Type));
            metaRepo.RegisterType(typeof(Net.Property));
            metaRepo.RegisterType(typeof(Net.PropertyAccessor));
            metaRepo.RegisterType(typeof(AnnotationType));
            metaRepo.RegisterType(typeof(AnnotationTypeAttribute));
            metaRepo.RegisterType(typeof(AnnotationInstance));
            metaRepo.RegisterType(typeof(AnnotationInstanceAttribute));
            metaRepo.RegisterType(typeof(Exception));
            metaRepo.RegisterType(typeof(CaughtException));
            metaRepo.RegisterType(typeof(ThrownException));
            metaRepo.RegisterType(typeof(Net.Delegate));
            metaRepo.RegisterType(typeof(Net.Struct));
            metaRepo.RegisterType(typeof(ParameterizableClass));
            metaRepo.RegisterType(typeof(ArgumentType));
            metaRepo.RegisterType(typeof(ParameterType));
            metaRepo.RegisterType(typeof(Enum));
            metaRepo.RegisterType(typeof(SourceAnchor));
            metaRepo.RegisterType(typeof(MultipleFileAnchor));
            metaRepo.RegisterType(typeof(AbstractFileAnchor));
            metaRepo.RegisterType(typeof(FileAnchor));
            metaRepo.RegisterType(typeof(Namespace));
            metaRepo.RegisterType(typeof(ParameterizableMethod));
            metaRepo.RegisterType(typeof(TypeBoundary));
            metaRepo.RegisterType(typeof(Implements));
            metaRepo.RegisterType(typeof(ControlFlowStructure));
            
            return t.model;
        }
    }
}