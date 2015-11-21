using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSpec
{
    public class Master
    {
        private Repo2 _repo;

        public Master(Repo2 repo)
        {
            _repo = repo;
        }

        public Master(Type type, Package package)
        {
            _repo = new Repo2();
            _repo.Add(type, package);
        }

        public void Write<T>(T record, TextWriter writer)
        {
            Package package = _repo.Get<T>();

            package.Write(record, writer);
        }

        public void Write(object record, TextWriter writer)
        {
            Package package = _repo.Get(record.GetType());

            package.Write(record, writer);
        }

        public void WriteMany(IEnumerable records, TextWriter writer)
        {
            foreach (object record in records)
                Write(record, writer);
        }

        // how about passing in some callbacks for each record type. OH! how about making this observable?
        public void Read<T>(T record, IParser parser)
        {
            Package package = _repo.Get<T>();   //T is not always what we are looking for. T is only what should be returned?? <- if we do that predicate is required

            if (parser.Parse())
                package.Read(record, parser.Current);
        }



        public T Read<T>(IParser parser) where T : new() //todo: get rid of it
        {
            Package package = _repo.Get<T>();

            if (parser.Parse())
            {
                T record = new T();

                package.Read(record, parser.Current);

                return record;
            }

            return default(T);
        }





        // how do we know what type we are reading?
        // how can we accomplish this? need to be able to look at the reader and determine T
        public IEnumerable ReadMany(IParser parser)
        {
            while (parser.Parse())
            {
                // we can peek at the reader to get a hint of what package we need. 

                //int peek = reader.Peek();       // we need better peek capability in the underlying reader

                //if (peek == -1)
                //    yield break;

                char hint = parser.Current[0][0];
                KeyValuePair<Type, Package> pair = _repo.Find(hint.ToString()); //todo: find should take the whole record

                //object record = Activator.CreateInstance(pair.Key); // yuck! we need some support for this
                //object record = Instance.Of(pair.Key);  // We can make dynamic creation faster, if we dont have to lookup the creation delegate
                object record = pair.Value.Create();    // no look up! we could move create down into read since that is where record is used and delegate is defined.

                pair.Value.Read(record, parser.Current);

                yield return record;
            }
        }

        // eventhough we specify T, not every read will be T. Every item read can be DERRIVED from T. T is specified only for output putposes.
        // in some cases every item returned could be of type T
        // its probally the same to Call ReadMany().OfType<T>() here... But maybe we can use T to aid in looking up a package.
        public IEnumerable<T> ReadMany<T>(IParser parser) //where T : new()
        {
            //Package package = _repo.Get<T>();   // this only looks for 1 T specifically

            //while (true)
            //{
            //    T record = new T();       //we dont actually want a T, but a class that derrives from T
            //    bool read = package.Read(record, reader);

            //    if (read)
            //        yield return record;
            //    else
            //        yield break;
            //}



            while (parser.Parse())
            {
                //int peek = reader.Peek();       // we need better peek capability in the underlying reader.

                //if (peek == -1)
                //    yield break;

                char hint = parser.Current[0][0];
                KeyValuePair<Type, Package> pair = _repo.Find(hint.ToString(), typeof(T));

                //T record = (T)Activator.CreateInstance(pair.Key); // yuck! we need some support for this
                //T record = (T)Instance.Of(pair.Key);
                T record = (T)pair.Value.Create();

                pair.Value.Read(record, parser.Current);


                yield return record;

            }
        }


        //public T Read<T>(Reader reader) where T : new()
        //{
        //    Package package = _repo.Get<T>();

        //    T record = new T();

        //    package.Read(record, reader);

        //    return record;
        //}

        public IEnumerable<T> NewStuff<T>(IParser parser)
        {
            // parse the next record
            while (parser.Parse())
            {
                // find the package for this parsed record
                //KeyValuePair<Type, Package> pair = _repo.Find(parser.Current[0]);  // todo: find() should take the whole array
                //var package = pair.Value;

                var package = _repo.Get<T>();

                // create empty record
                T record = (T)package.Create();

                // pass record and parsed to package.read()
                package.Read(record, parser.Current);

                yield return record;    //todo: make our own enumertor for efficiency?
            }
        }
    }


    public class MyEnumerator<T> : IEnumerator<T>
    {
        private readonly IParser _parser;
        private readonly Package _package;
        private T _current;

        public MyEnumerator(IParser parser, Package package)
        {
            _parser = parser;
            _package = package;
        }

        public T Current
        {
            get { return _current; }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        object IEnumerator.Current
        {
            get { return _current; }
        }

        public bool MoveNext()
        {
            if (_parser.Parse())
            {
                // find the package for this parsed record
                //KeyValuePair<Type, Package> pair = _repo.Find(parser.Current[0]);  // todo: find() should take the whole array
                //var package = pair.Value;

                T record = (T)_package.Create();

                _package.Read(record, _parser.Current);

                _current = record;

                return true;
            }

            return false;
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }
    }
}
