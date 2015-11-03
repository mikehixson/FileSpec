using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class Repo2
    {
        private Dictionary<Type, Package> _registry;

        public Repo2()
        {
            _registry = new Dictionary<Type, Package>(0);
        }

        public void Add<T>(Package package)
        {
            Add(typeof(T), package);
        }

        public void Add(Type type, Package package)
        {
            _registry.Add(type, package);
        }

        public Package Get<T>() 
        {
            return Get(typeof(T));
        }

        public Package Get(Type type)
        {
            return _registry[type];   //todo: guard, null?
        }

        public KeyValuePair<Type, Package> Find(string hint)    // probably want to have Type on Package so we can avoid return KeyPair
        {
            return _registry.FirstOrDefault(p => p.Value.Predicate != null && p.Value.Predicate(hint));
        }

        public KeyValuePair<Type, Package> Find(string hint, Type type)    // probably want to have Type on Package so we can avoid return KeyPair
        {
            return _registry.FirstOrDefault(p => p.Value.Predicate != null && p.Value.Predicate(hint) && type.IsAssignableFrom(p.Key));
        }
    }
}
