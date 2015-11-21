using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FileSpec.Converter;

namespace FileSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            Repo2 repo2 = new Repo2();

            SetupRaw(repo2);
            Raw(repo2);



        }



        /*
         * Imporovments:
         *  Split handler to smaller interfaces
         *  Accessor is now told what properties to use
         *  Decoupled from property attributes
         *  No need for an ordering integer, order is implied
         */





        public static void SetupRaw(Repo2 repo)
        {

            Package package1 = new Package
            {
                Writer = new DelimitedWriter(fieldDelimiter: "*"),
                //Reader = new DelimitedReader(fieldDelimiter: "*"),
                Predicate = s => s == "A",
                Create = () => new Test(),
                Mappings = new List<IMapping>
                {
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test, string>(r => r.RecordType), new StringConverter()),
                        Field = new PositionedField(0)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test, int>(r => r.MyInteger1), new NumberConverter()),
                        Field = new PositionedField(1)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test, DateTime>(r => r.MyDateTime), new DateTimeConverter()),
                        Field = new PositionedField(3)
                    }
                }
            };

            repo.Add<Test>(package1);

            Package package2 = new Package
            {
                Writer = new SimpleWriter(null),
                //Reader = new SimpleReader(null),
                Predicate = s => s == "B",
                Create = () => new Test2(),
                Mappings = new List<IMapping>
                {
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test2, string>(r => r.RecordType), new StringConverter()),
                        Field = new FixedLengthField(0, 3)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test2, int>(r => r.MyInteger1), new NumberConverter()),
                        Field = new FixedLengthField(3, 10)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test2, DateTime>(r => r.MyDateTime), new DateTimeConverter()),
                        Field = new FixedLengthField(15, 25)
                    }
                }
            };

            repo.Add<Test2>(package2);


            Package package3 = new Package
            {
                Writer = new DelimitedWriter(null),
                //Reader = new DelimitedReader(null),
                Predicate = s => s == "C",
                Create = () => new Test3(),
                Mappings = new List<IMapping>
                {
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test3, string>(r => r.RecordType), new StringConverter()),
                        Field = new PositionedField(0)    //  ineresting how this is compatible with NamedFields. Maybe there should be an interface to indicate this.
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test3, int>(r => r.MyInteger1), new NumberConverter()),
                        Field = new NamedField(1, "Integer1")
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Test3, DateTime>(r => r.MyDateTime), new DateTimeConverter()),
                        Field = new NamedField(2, "Date")
                    }
                }
            };

            repo.Add<Test3>(package3);


            Package package4 = new Package
            {
                Writer = new SimpleWriter(null),
                //Reader = new SimpleReader(null),
                Mappings = new List<IMapping>
                {
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Complex, int>(r => r.X), new NumberConverter(format: "000")),
                        Field = new FixedLengthField(3, 10)
                    },
                    new PropertyMapping()
                    {
                        Property = new Property(GetProperty<Complex, int>(r => r.Y), new NumberConverter(format: "000")),
                        Field = new FixedLengthField(15, 25)
                    }
                }
            };

            repo.Add<Complex>(package4);
        }


        public static void Raw(Repo2 repo)
        {
            Master master = new Master(repo);

            Test test1 = new Test { MyInteger1 = 12345, MyDateTime = DateTime.Now };

            RunRaw(master, test1);
            RunMany(master, test1);

            Test2 testA = new Test2 { MyInteger1 = 12345, MyDateTime = DateTime.Now };

            RunRaw(master, testA);

            Test3 testE = new Test3 { MyInteger1 = 12345, MyDateTime = DateTime.Now };

            RunRaw(master, testE);
            RunRawNamed<Test3>(master);

            Complex testC = new Complex { X = 15, Y = 97 };

            RunRaw(master, testC);

            RunManyBase<TestBase>(master, test1, testA, testE);
            RunManyMixed(master, test1, testA, testE);//, testC);

        }

        public static void RunRaw<T>(Master master, T test1) where T : new()
        {
            MemoryStream stream = new MemoryStream();
            TextWriter tw = new StreamWriter(stream);

            master.Write(test1, tw);
            
            string result = Encoding.UTF8.GetString(stream.GetBuffer());

            stream.Position = 0;
            DelimitedParser parser = new DelimitedParser(new Reader(stream), ',');

            T test2 = new T();
            master.Read(test2, parser);

            stream.Position = 0;

            T test3 = master.Read<T>(parser);
        }

        public static void RunRawNamed<T>(Master master) where T : new()
        {
            string result = "C,Date=04/11/1976,Integer2=4567,Integer1=1234\r\n";

            TextReader tr = new StringReader(result);
            IParser parser = null;

            T test2 = new T();
            master.Read(test2, parser);
        }

        // Every item is exactly the same T
        public static void RunMany<T>(Master master, T test1) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);

            master.WriteMany(Enumerable.Repeat(test1, 5), tw);

            string result = sb.ToString();

            TextReader tr = new StringReader(result);
            IParser parser = null;
            IEnumerable<T> test3 = master.ReadMany<T>(parser).ToArray();
        }

        // Every item derrives from T
        public static void RunManyBase<T>(Master master, params T[] records) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);

            master.WriteMany(records, tw);

            string result = sb.ToString();

            TextReader tr = new StringReader(result);
            IParser parser = null;
            IEnumerable<T> test3 = master.ReadMany<T>(parser).ToArray();
        }
        
        

        public static void RunManyMixed(Master master, params object[] records)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);

            master.WriteMany(records, tw);

            string result = sb.ToString();


            TextReader tr = new StringReader(result);
            IParser parser = null;
            IEnumerable test3 = master.ReadMany(parser).OfType<object>().ToArray();

            
        }



        private static PropertyInfo GetProperty<T, R>(Expression<Func<T, R>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            return memberExpression.Member as PropertyInfo;
        }       
    }













    public class TestBase
    {
        public string RecordType { get; set; }
    }

    public class Test : TestBase
    {        
        public int MyInteger1 { get; set; }
        public int MyInteger2 { get; set; }
        public DateTime MyDateTime { get; set; }
        public Complex Complex { get; set; }

        public string MyString { get; set; }

        public byte MyByte { get; set; }

        public Test()
        {
            RecordType = "A";
        }
    }

    public class Test2 : Test
    {
        public Test2()
        {
            RecordType = "B";
        }
    }

    public class Test3 : Test
    {
        public Test3()
        {
            RecordType = "C";
        }
    }

    public class Complex
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class ComplexAccess4 : IConverter<Complex>
    {
        public string GetString(Complex value)
        {
            return String.Format("{0}-{1}", value.X, value.Y);     // todo: how does comma effect record format?
        }

        public Complex GetValue(string value)
        {
            string[] parts = value.Split('-');
            return new Complex
            {
                X = Int32.Parse(parts[0]),
                Y = Int32.Parse(parts[1])
            };
        }
    }

}
