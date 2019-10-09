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
            metaRepo.RegisterType(typeof(CSharp.CSharpEvent));
            metaRepo.RegisterType(typeof(BehaviouralEntity));
            metaRepo.RegisterType(typeof(Class));
            metaRepo.RegisterType(typeof(Method));
            metaRepo.RegisterType(typeof(Type));
            metaRepo.RegisterType(typeof(CSharp.CSharpProperty));
            metaRepo.RegisterType(typeof(CSharp.CSharpPropertyAccessor));
            metaRepo.RegisterType(typeof(AnnotationType));
            metaRepo.RegisterType(typeof(AnnotationTypeAttribute));
            metaRepo.RegisterType(typeof(AnnotationInstance));
            metaRepo.RegisterType(typeof(AnnotationInstanceAttribute));
            metaRepo.RegisterType(typeof(Exception));
            metaRepo.RegisterType(typeof(CaughtException));
            metaRepo.RegisterType(typeof(ThrownException));
            metaRepo.RegisterType(typeof(CSharp.Delegate));
            metaRepo.RegisterType(typeof(CSharp.CSharpStruct));
            metaRepo.RegisterType(typeof(ParameterizableClass));
            metaRepo.RegisterType(typeof(ParameterizedType));
            metaRepo.RegisterType(typeof(ParameterType));
            metaRepo.RegisterType(typeof(Enum));
            metaRepo.RegisterType(typeof(SourceAnchor));
            metaRepo.RegisterType(typeof(MultipleFileAnchor));
            metaRepo.RegisterType(typeof(AbstractFileAnchor));
            metaRepo.RegisterType(typeof(FileAnchor));
            metaRepo.RegisterType(typeof(Namespace));
            return t.model;
        }
    }
}