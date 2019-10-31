using Fame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMonoFamix.ModelBuilder
{
    public class NamedEntityAccumulator<T> where T : FAMIX.Entity {
        private Dictionary<string, T> entities;

        public IList<T> RegisteredEntities () {
            return entities.Values.ToList();
        }
        public NamedEntityAccumulator()
        {
            entities = new Dictionary<string, T>();
        }

        public List<T> get()
        {
            return entities.Values.ToList();
        }

        public T Add(String qualifiedName, T entity)
        {
            try
            {
                entities.Add(qualifiedName, entity);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(qualifiedName);
                Console.WriteLine(e);
            } 
            return entity;
        }

        public T Named(String qualifiedName)
        {
            if (entities.ContainsKey(qualifiedName))
            {
                return entities[qualifiedName];
            }
            return default(T);
        }

        public Boolean has(String qualifiedName)
        {
            return entities.ContainsKey(qualifiedName);
        }

        public int size()
        {
            return entities.Count;
        }

        public Boolean isEmpty()
        {
            return entities.Count == 0;
        }
        public Tp EnsureEntityNamed<Tp> (string name, Func<Tp> func) where Tp: T{
            Tp type;
            if (this.has(name)) {
                return (Tp)this.Named(name);
            }
            type = func();
            this.Add(name, type);
            return type;
        }
    }
}
