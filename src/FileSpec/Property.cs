using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FileSpec.Converter;

namespace FileSpec
{
    public class Property : IProperty
    {
        private Func<object, string> _get;
        private Action<string, object> _set;

        private PropertyInfo _property;
        private object _propertyAccess;     // Does this need to be here?

        public Property(PropertyInfo property, object propertyAccess)
        {
            _property = property;
            _propertyAccess = propertyAccess;
        }

        public string Get(object record)
        {
            if (_get == null)
                _get = BuildGetDelegate();

            return _get(record);
        }

        public void Set(object record, string value)
        {
            if (_set == null)
                _set = BuildSetDelegate();

            _set(value, record);
        }

        private object GetPropertyAccess()
        {
            return _propertyAccess;
        }

        private Func<object, string> BuildGetDelegate()
        {
            string methodName = String.Format("{0}+{1}+Get", this.GetType().FullName, _property.Name);
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, typeof(string), new Type[] { this.GetType(), typeof(object) }, this.GetType(), true);

            MethodInfo propertyAccessMethod = this.GetType().GetMethod("GetPropertyAccess", BindingFlags.Instance | BindingFlags.NonPublic);

            ILGenerator il = dynamicMethod.GetILGenerator();

            MethodInfo getPropertyMethod = _property.GetGetMethod();
            Type propertyType = getPropertyMethod.ReturnType;

            Type interfaceType = typeof(IConverter<>).MakeGenericType(propertyType);
            MethodInfo conversionMethod = _propertyAccess.GetType().GetInterfaceMap(interfaceType).InterfaceMethods.Single(m => m.Name == "GetString");

            // -- Get FieldHandler --
            // push method argument 0 (this)
            il.Emit(OpCodes.Ldarg_0);

            // call method to get PropertAccess, pop 1, push handler
            il.Emit(OpCodes.Call, propertyAccessMethod);

            // -- Get poperty value --
            // push method argument 1 (T record)
            il.Emit(OpCodes.Ldarg_1);

            // call get property method, pop 1, push property value
            il.Emit(OpCodes.Call, getPropertyMethod);

            // call convert to string method, pop 2, push result
            il.Emit(OpCodes.Call, conversionMethod);

            // return
            il.Emit(OpCodes.Ret);

            return (Func<object, string>)dynamicMethod.CreateDelegate(typeof(Func<object, string>), this);
        }


        private Action<string, object> BuildSetDelegate()
        {
            string methodName = String.Format("{0}+{1}+Set", this.GetType().FullName, _property.Name);
            DynamicMethod dynamicMethod = new DynamicMethod(methodName, null, new Type[] { this.GetType(), typeof(string), typeof(object) }, this.GetType(), true);

            MethodInfo propertyAccessMethod = this.GetType().GetMethod("GetPropertyAccess", BindingFlags.Instance | BindingFlags.NonPublic);
            

            ILGenerator il = dynamicMethod.GetILGenerator();

            MethodInfo setPropertyMethod = _property.GetSetMethod();
            Type propertyType = setPropertyMethod.GetParameters()[0].ParameterType;

            Type interfaceType = typeof(IConverter<>).MakeGenericType(propertyType);
            MethodInfo conversionMethod = _propertyAccess.GetType().GetInterfaceMap(interfaceType).InterfaceMethods.Single(m => m.Name == "GetValue");




            //il.BeginExceptionBlock();


            // push method argument 2 (T)
            il.Emit(OpCodes.Ldarg_2);


            // -- Get FieldHandler --
            // push method argument 0 (this)
            il.Emit(OpCodes.Ldarg_0);

            // push property index
            //il.Emit(OpCodes.Ldc_I4, i);

            // call method to get PropertyAccess, pop 1, push handler
            il.Emit(OpCodes.Call, propertyAccessMethod);


            // -- Get array value --
            // push method argument 1 (string)
            il.Emit(OpCodes.Ldarg_1);

            // push property index 
            //il.Emit(OpCodes.Ldc_I4, i);

            // push array value, pop 2
            //il.Emit(OpCodes.Ldelem_Ref);

            // store property value in a local variable, pop 1
            //il.Emit(OpCodes.Stloc, propertyValue);

            // push property value
            //il.Emit(OpCodes.Ldloc, propertyValue);


            // -- Assign property value --
            // call convert from string method, pop 2, push result
            il.Emit(OpCodes.Call, conversionMethod);

            // call set property method, pop 2
            il.Emit(OpCodes.Call, setPropertyMethod);


            // -- Throw helpful exception --
            // push exception
            //il.BeginCatchBlock(typeof(Exception));

            // push property name
            //il.Emit(OpCodes.Ldstr, _descriptions[i].Property.Name);

            // push property value
            //il.Emit(OpCodes.Ldloc, propertyValue);

            // call throw exception method, pop 3
            //il.Emit(OpCodes.Call, propertyExceptionMethod);


            //il.EndExceptionBlock();


            // return
            il.Emit(OpCodes.Ret);

            return (Action<string, object>)dynamicMethod.CreateDelegate(typeof(Action<string, object>), this);
        }
    }
}
