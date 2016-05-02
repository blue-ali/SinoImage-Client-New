using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DocScanner.LibCommon
{
    public sealed class ReflectHelper
    {
        // Fields
        private static Hashtable allAttributes = new Hashtable();
        public static readonly BindingFlags AllMembers = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        private static Hashtable methodCache = new Hashtable();
        private static Hashtable topAttributes = new Hashtable();

        // Methods
        private ReflectHelper()
        {
        }

        public static object Construct(Type type)
        {
            ConstructorInfo constructor = GetConstructor(type);
            if (constructor == null)
            {
                throw new ApplicationException(type.FullName + " does not have a default constructor");
            }
            return constructor.Invoke(null);
        }

        public static object Construct(Type type, object[] arguments)
        {
            if (arguments == null)
            {
                return Construct(type);
            }
            Type[] types = Type.GetTypeArray(arguments);
            ConstructorInfo constructor = GetConstructor(type, types);
            if (constructor == null)
            {
                throw new ApplicationException(type.FullName + " does not have a suitable constructor");
            }
            return constructor.Invoke(arguments);
        }

        public static Attribute GetAttribute(ICustomAttributeProvider member, string attrName, bool inherit)
        {
            foreach (Attribute attribute in GetAttributes(member, inherit))
            {
                if (IsInstanceOfType(attrName, attribute))
                {
                    return attribute;
                }
            }
            return null;
        }

        public static Attribute[] GetAttributes(ICustomAttributeProvider member, bool inherit)
        {
            Hashtable hashtable = inherit ? allAttributes : topAttributes;
            if (hashtable.Contains(member))
            {
                return (hashtable[member] as Attribute[]);
            }
            object[] customAttributes = member.GetCustomAttributes(inherit);
            Attribute[] attributeArray = new Attribute[customAttributes.Length];
            int num = 0;
            foreach (Attribute attribute in customAttributes)
            {
                attributeArray[num++] = attribute;
            }
            hashtable[member] = attributeArray;
            return attributeArray;
        }

        public static Attribute[] GetAttributes(ICustomAttributeProvider member, string attrName, bool inherit)
        {
            ArrayList list = new ArrayList();
            foreach (Attribute attribute in GetAttributes(member, inherit))
            {
                if (IsInstanceOfType(attrName, attribute))
                {
                    list.Add(attribute);
                }
            }
            return (Attribute[])list.ToArray(typeof(Attribute));
        }

        public static ConstructorInfo GetConstructor(Type fixtureType)
        {
            return fixtureType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
        }

        public static ConstructorInfo GetConstructor(Type fixtureType, Type[] types)
        {
            return fixtureType.GetConstructor(AllMembers, null, types, null);
        }

        public static object GetFieldValue(object obj, string name)
        {
            return GetFieldValue(obj.GetType(), obj, name, AllMembers);
        }

        public static object GetFieldValue(Type t, object obj, string name, BindingFlags bindingFlags)
        {
            FieldInfo field = t.GetField(name, bindingFlags);
            if (field != null)
            {
                return field.GetValue(obj);
            }
            return null;
        }

        public static Dictionary<string, object> GetFieldValues(object obj)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            FieldInfo[] fields = obj.GetType().GetFields(AllMembers);
            foreach (FieldInfo info in fields)
            {
                dictionary[info.Name] = info.GetValue(obj);
            }
            return dictionary;
        }

        private static MethodInfo[] GetMethods(Type fixtureType)
        {
            if (methodCache.Contains(fixtureType))
            {
                return (methodCache[fixtureType] as MethodInfo[]);
            }
            MethodInfo[] methods = fixtureType.GetMethods(AllMembers);
            methodCache[fixtureType] = methods;
            return methods;
        }

        public static MethodInfo[] GetMethodsWithAttribute(Type fixtureType, string attributeName, bool inherit)
        {
            ArrayList list = new ArrayList();
            foreach (MethodInfo info in GetMethods(fixtureType))
            {
                if (HasAttribute(info, attributeName, inherit))
                {
                    list.Add(info);
                }
            }
            list.Sort(new BaseTypesFirstComparer());
            return (MethodInfo[])list.ToArray(typeof(MethodInfo));
        }

        public static MethodInfo GetNamedMethod(Type fixtureType, string methodName)
        {
            foreach (MethodInfo info in GetMethods(fixtureType))
            {
                if (info.Name == methodName)
                {
                    return info;
                }
            }
            return null;
        }

        public static MethodInfo GetNamedMethod(Type fixtureType, string methodName, string[] argTypes)
        {
            foreach (MethodInfo info in GetMethods(fixtureType))
            {
                if (!(info.Name == methodName))
                {
                    continue;
                }
                ParameterInfo[] parameters = info.GetParameters();
                if (parameters.Length == argTypes.Length)
                {
                    bool flag3 = true;
                    for (int i = 0; i < argTypes.Length; i++)
                    {
                        if (parameters[i].ParameterType.FullName != argTypes[i])
                        {
                            flag3 = false;
                            break;
                        }
                    }
                    if (flag3)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        public static PropertyInfo GetNamedProperty(Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetProperty(name, bindingFlags);
        }

        public static object GetPropertyValue(object obj, string name)
        {
            return GetPropertyValue(obj, name, BindingFlags.Public | BindingFlags.Instance);
        }

        public static object GetPropertyValue(object obj, string name, BindingFlags bindingFlags)
        {
            PropertyInfo info = GetNamedProperty(obj.GetType(), name, bindingFlags);
            if (info != null)
            {
                return info.GetValue(obj, null);
            }
            return null;
        }

        public static PropertyInfo GetPropertyWithAttribute(Type fixtureType, string attributeName)
        {
            foreach (PropertyInfo info in fixtureType.GetProperties(AllMembers))
            {
                if (HasAttribute(info, attributeName, true))
                {
                    return info;
                }
            }
            return null;
        }

        public static object GetStaticFieldValue(Type t, string name)
        {
            return GetFieldValue(t, null, name, AllMembers);
        }

        public static Dictionary<string, object> GetStaticFieldValues(Type t)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo info in fields)
            {
                dictionary[info.Name] = info.GetValue(null);
            }
            return dictionary;
        }

        public static bool HasAttribute(ICustomAttributeProvider member, string attrName, bool inherit)
        {
            foreach (Attribute attribute in GetAttributes(member, inherit))
            {
                if (IsInstanceOfType(attrName, attribute))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasInterface(Type fixtureType, string interfaceName)
        {
            foreach (Type type in fixtureType.GetInterfaces())
            {
                if (type.FullName == interfaceName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasMethodWithAttribute(Type fixtureType, string attributeName, bool inherit)
        {
            foreach (MethodInfo info in GetMethods(fixtureType))
            {
                if (HasAttribute(info, attributeName, inherit))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InheritsFrom(object obj, string typeName)
        {
            return InheritsFrom(obj.GetType(), typeName);
        }

        public static bool InheritsFrom(Type type, string typeName)
        {
            for (Type type2 = type; !(type2 == typeof(object)); type2 = type2.BaseType)
            {
                if (type2.FullName == typeName)
                {
                    return true;
                }
            }
            return false;
        }

        public static object InvokeMethod(MethodInfo method, object fixture)
        {
            return InvokeMethod(method, fixture, null);
        }

        public static object InvokeMethod(MethodInfo method, object fixture, params object[] args)
        {
            if (method != null)
            {
                try
                {
                    return method.Invoke(fixture, args);
                }
                catch (TargetInvocationException exception)
                {
                    Exception innerException = exception.InnerException;
                    throw new ApplicationException("Rethrown", innerException);
                }
            }
            return null;
        }

        public static bool IsInstanceOfType(string typeName, Attribute attr)
        {
            Type type = attr.GetType();
            return ((type.FullName == typeName) || InheritsFrom(type, typeName));
        }

        public static void SetFieldValue(object obj, string name, object value)
        {
            FieldInfo field = obj.GetType().GetField(name, AllMembers);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }

        public static void SetPropertyValue(object obj, string name, object value, BindingFlags bindingFlags)
        {
            PropertyInfo info = GetNamedProperty(obj.GetType(), name, bindingFlags);
            if (info != null)
            {
                info.SetValue(obj, value, null);
            }
        }

        // Nested Types
        private class BaseTypesFirstComparer : IComparer
        {
            // Methods
            public int Compare(object x, object y)
            {
                MethodInfo info = x as MethodInfo;
                MethodInfo info2 = y as MethodInfo;
                if ((info == null) || (info2 == null))
                {
                    return 0;
                }
                Type declaringType = info.DeclaringType;
                Type c = info2.DeclaringType;
                if (declaringType == c)
                {
                    return 0;
                }
                if (declaringType.IsAssignableFrom(c))
                {
                    return -1;
                }
                return 1;
            }
        }
    }

}