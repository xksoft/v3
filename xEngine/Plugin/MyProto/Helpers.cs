﻿using System;
using System.Diagnostics;
using System.Text;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto
{
    /// <summary>
    ///     Not all frameworks are created equal (fx1.1 vs fx2.0,
    ///     micro-framework, compact-framework,
    ///     silverlight, etc). This class simply wraps up a few things that would
    ///     otherwise make the real code unnecessarily messy, providing fallback
    ///     implementations if necessary.
    /// </summary>
    internal sealed class Helpers
    {
        private Helpers()
        {
        }

        public static StringBuilder AppendLine(StringBuilder builder)
        {
#if CF2
            return builder.Append("\r\n");
#elif FX11
            return builder.Append(Environment.NewLine);
#else
            return builder.AppendLine();
#endif
        }

        public static bool IsNullOrEmpty(string value)
        {
            // yes, FX11 lacks this!
            return value == null || value.Length == 0;
        }

        [Conditional("DEBUG")]
        public static void DebugWriteLine(string message, object obj)
        {
#if DEBUG
            string suffix;
            try
            {
                suffix = obj == null ? "(null)" : obj.ToString();
            }
            catch
            {
                suffix = "(exception)";
            }
            DebugWriteLine(message + ": " + suffix);
#endif
        }

        [Conditional("DEBUG")]
        public static void DebugWriteLine(string message)
        {
#if DEBUG
#if MF      
            Microsoft.SPOT.Debug.Print(message);
#else
            Debug.WriteLine(message);
#endif
#endif
        }

        [Conditional("TRACE")]
        public static void TraceWriteLine(string message)
        {
#if TRACE
#if MF
            Microsoft.SPOT.Trace.Print(message);
#elif SILVERLIGHT || MONODROID || CF2 || WINRT || IOS || PORTABLE
            System.Diagnostics.Debug.WriteLine(message);
#else
            Trace.WriteLine(message);
#endif
#endif
        }

        [Conditional("DEBUG")]
        public static void DebugAssert(bool condition, string message)
        {
#if DEBUG
            if (!condition)
            {
#if MF
                Microsoft.SPOT.Debug.Assert(false, message);
#else
                Debug.Assert(false, message);
            }
#endif
#endif
        }

        [Conditional("DEBUG")]
        public static void DebugAssert(bool condition, string message, params object[] args)
        {
#if DEBUG
            if (!condition) DebugAssert(false, string.Format(message, args));
#endif
        }

        [Conditional("DEBUG")]
        public static void DebugAssert(bool condition)
        {
#if DEBUG
#if MF
            Microsoft.SPOT.Debug.Assert(condition);
#else
            if (!condition && Debugger.IsAttached) Debugger.Break();
            Debug.Assert(condition);
#endif
#endif
        }

#if !NO_RUNTIME
        public static void Sort(int[] keys, object[] values)
        {
            // bubble-sort; it'll work on MF, has small code,
            // and works well-enough for our sizes. This approach
            // also allows us to do `int` compares without having
            // to go via IComparable etc, so win:win
            bool swapped;
            do
            {
                swapped = false;
                for (var i = 1; i < keys.Length; i++)
                {
                    if (keys[i - 1] > keys[i])
                    {
                        var tmpKey = keys[i];
                        keys[i] = keys[i - 1];
                        keys[i - 1] = tmpKey;
                        var tmpValue = values[i];
                        values[i] = values[i - 1];
                        values[i - 1] = tmpValue;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
#endif

        public static void BlockCopy(byte[] from, int fromIndex, byte[] to, int toIndex, int count)
        {
#if MF || WINRT
            Array.Copy(from, fromIndex, to, toIndex, count);
#else
            Buffer.BlockCopy(from, fromIndex, to, toIndex, count);
#endif
        }

        public static bool IsInfinity(float value)
        {
#if MF
            const float inf = (float)1.0 / (float)0.0, minf = (float)-1.0F / (float)0.0;
            return value == inf || value == minf;
#else
            return float.IsInfinity(value);
#endif
        }

#if WINRT
        internal static MemberInfo GetInstanceMember(TypeInfo declaringType, string name)
        {
            PropertyInfo prop = declaringType.GetDeclaredProperty(name);
            MethodInfo method;
            if (prop != null && (method = Helpers.GetGetMethod(prop, true, true)) != null && !method.IsStatic) return prop;

            FieldInfo field = declaringType.GetDeclaredField(name);
            if (field != null && !field.IsStatic) return field;

            return null;
        }
        internal static MethodInfo GetInstanceMethod(TypeInfo declaringType, string name)
        {
            foreach (MethodInfo method in declaringType.DeclaredMethods)
            {
                if (!method.IsStatic && method.Name == name)
                {
                    return method;
                }
            }
            return null;
        }
        internal static MethodInfo GetStaticMethod(TypeInfo declaringType, string name)
        {
            foreach (MethodInfo method in declaringType.DeclaredMethods)
            {
                if (method.IsStatic && method.Name == name)
                {
                    return method;
                }
            }
            return null;
        }
        internal static MethodInfo GetInstanceMethod(TypeInfo declaringType, string name, Type[] types)
        {
            if (types == null) types = EmptyTypes;
            foreach (MethodInfo method in declaringType.DeclaredMethods)
            {
                if (!method.IsStatic && method.Name == name)
                {
                    if(IsMatch(method.GetParameters(), types)) return method;
                }
            }
            return null;
        }
#else
        internal static MethodInfo GetInstanceMethod(Type declaringType, string name)
        {
            return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static MethodInfo GetStaticMethod(Type declaringType, string name)
        {
            return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        internal static MethodInfo GetInstanceMethod(Type declaringType, string name, Type[] types)
        {
            if (types == null) types = EmptyTypes;
#if PORTABLE
            MethodInfo method = declaringType.GetMethod(name, types);
            if (method != null && method.IsStatic) method = null;
            return method;
#else
            return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, types, null);
#endif
        }
#endif

        internal static bool IsSubclassOf(Type type, Type baseClass)
        {
#if WINRT
            return type.GetTypeInfo().IsSubclassOf(baseClass);
#else
            return type.IsSubclassOf(baseClass);
#endif
        }

        public static bool IsInfinity(double value)
        {
#if MF
            const double inf = (double)1.0 / (double)0.0, minf = (double)-1.0F / (double)0.0;
            return value == inf || value == minf;
#else
            return double.IsInfinity(value);
#endif
        }

        public static readonly Type[] EmptyTypes =
#if PORTABLE || WINRT || CF2 || CF35
            new Type[0];
#else
            Type.EmptyTypes;
#endif

#if WINRT
        private static readonly Type[] knownTypes = new Type[] {
                typeof(bool), typeof(char), typeof(sbyte), typeof(byte),
                typeof(short), typeof(ushort), typeof(int), typeof(uint),
                typeof(long), typeof(ulong), typeof(float), typeof(double),
                typeof(decimal), typeof(string),
                typeof(DateTime), typeof(TimeSpan), typeof(Guid), typeof(Uri),
                typeof(byte[]), typeof(System.Type)};
        private static readonly ProtoTypeCode[] knownCodes = new ProtoTypeCode[] {
            ProtoTypeCode.Boolean, ProtoTypeCode.Char, ProtoTypeCode.SByte, ProtoTypeCode.Byte,
            ProtoTypeCode.Int16, ProtoTypeCode.UInt16, ProtoTypeCode.Int32, ProtoTypeCode.UInt32,
            ProtoTypeCode.Int64, ProtoTypeCode.UInt64, ProtoTypeCode.Single, ProtoTypeCode.Double,
            ProtoTypeCode.Decimal, ProtoTypeCode.String,
            ProtoTypeCode.DateTime, ProtoTypeCode.TimeSpan, ProtoTypeCode.Guid, ProtoTypeCode.Uri,
            ProtoTypeCode.ByteArray, ProtoTypeCode.Type
        };

#endif

#if FEAT_IKVM
        public static ProtoTypeCode GetTypeCode(IKVM.Reflection.Type type)
        {
            TypeCode code = IKVM.Reflection.Type.GetTypeCode(type);
            switch (code)
            {
                case TypeCode.Empty:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return (ProtoTypeCode)code;
            }
            switch(type.FullName)
            {
                case "System.TimeSpan": return ProtoTypeCode.TimeSpan;
                case "System.Guid": return ProtoTypeCode.Guid;
                case "System.Uri": return ProtoTypeCode.Uri;
                case "System.Byte[]": return ProtoTypeCode.ByteArray;
                case "System.Type": return ProtoTypeCode.Type;
            }
            return ProtoTypeCode.Unknown;
        }
#endif

        public static ProtoTypeCode GetTypeCode(Type type)
        {
#if WINRT
            
            int idx = Array.IndexOf<Type>(knownTypes, type);
            if (idx >= 0) return knownCodes[idx];
            return type == null ? ProtoTypeCode.Empty : ProtoTypeCode.Unknown;
#else
            var code = Type.GetTypeCode(type);
            switch (code)
            {
                case TypeCode.Empty:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return (ProtoTypeCode) code;
            }
            if (type == typeof (TimeSpan)) return ProtoTypeCode.TimeSpan;
            if (type == typeof (Guid)) return ProtoTypeCode.Guid;
            if (type == typeof (Uri)) return ProtoTypeCode.Uri;
            if (type == typeof (byte[])) return ProtoTypeCode.ByteArray;
            if (type == typeof (Type)) return ProtoTypeCode.Type;

            return ProtoTypeCode.Unknown;
#endif
        }


#if FEAT_IKVM
        internal static IKVM.Reflection.Type GetUnderlyingType(IKVM.Reflection.Type type)
        {
            if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition().FullName == "System.Nullable`1")
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }
#endif

        internal static Type GetUnderlyingType(Type type)
        {
#if NO_GENERICS
            return null; // never a Nullable<T>, so always returns null
#else
            return Nullable.GetUnderlyingType(type);
#endif
        }

        internal static bool IsValueType(Type type)
        {
#if WINRT
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        internal static bool IsEnum(Type type)
        {
#if WINRT
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        internal static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == null) return null;
#if WINRT
            MethodInfo method = property.GetMethod;
            if (!nonPublic && method != null && !method.IsPublic) method = null;
            return method;
#else
            var method = property.GetGetMethod(nonPublic);
            if (method == null && !nonPublic && allowInternal)
            {
                // could be "internal" or "protected internal"; look for a non-public, then back-check
                method = property.GetGetMethod(true);
                if (method == null && !(method.IsAssembly || method.IsFamilyOrAssembly))
                {
                    method = null;
                }
            }
            return method;
#endif
        }

        internal static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
        {
            if (property == null) return null;
#if WINRT
            MethodInfo method = property.SetMethod;
            if (!nonPublic && method != null && !method.IsPublic) method = null;
            return method;
#else
            var method = property.GetSetMethod(nonPublic);
            if (method == null && !nonPublic && allowInternal)
            {
                // could be "internal" or "protected internal"; look for a non-public, then back-check
                method = property.GetGetMethod(true);
                if (method == null && !(method.IsAssembly || method.IsFamilyOrAssembly))
                {
                    method = null;
                }
            }
            return method;
#endif
        }

#if FEAT_IKVM
        internal static bool IsMatch(IKVM.Reflection.ParameterInfo[] parameters, IKVM.Reflection.Type[] parameterTypes)
        {
            if (parameterTypes == null) parameterTypes = Helpers.EmptyTypes;
            if (parameters.Length != parameterTypes.Length) return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i]) return false;
            }
            return true;
        }
#endif
#if WINRT
        private static bool IsMatch(ParameterInfo[] parameters, Type[] parameterTypes)
        {
            if (parameterTypes == null) parameterTypes = EmptyTypes;
            if (parameters.Length != parameterTypes.Length) return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i]) return false;
            }
            return true;
        }
        internal static ConstructorInfo GetConstructor(TypeInfo type, Type[] parameterTypes, bool nonPublic)
        {
            foreach (ConstructorInfo ctor in type.DeclaredConstructors)
            {
                if (!nonPublic && !ctor.IsPublic) continue;
                if (IsMatch(ctor.GetParameters(), parameterTypes)) return ctor;
            }
            return null;
        }
        internal static ConstructorInfo[] GetConstructors(TypeInfo typeInfo, bool nonPublic)
        {
            if (nonPublic) return System.Linq.Enumerable.ToArray(typeInfo.DeclaredConstructors);
            return System.Linq.Enumerable.ToArray(
                System.Linq.Enumerable.Where(typeInfo.DeclaredConstructors, x => x.IsPublic));
        }
        internal static PropertyInfo GetProperty(TypeInfo type, string name, bool nonPublic)
        {
            return type.GetDeclaredProperty(name);
        }
#else

        internal static ConstructorInfo GetConstructor(Type type, Type[] parameterTypes, bool nonPublic)
        {
#if PORTABLE
    // pretty sure this will only ever return public, but...
            ConstructorInfo ctor = type.GetConstructor(parameterTypes);
            return (ctor != null && (nonPublic || ctor.IsPublic)) ? ctor : null;
#else
            return type.GetConstructor(
                nonPublic
                    ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    : BindingFlags.Instance | BindingFlags.Public,
                null, parameterTypes, null);
#endif
        }

        internal static ConstructorInfo[] GetConstructors(Type type, bool nonPublic)
        {
            return type.GetConstructors(
                nonPublic
                    ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    : BindingFlags.Instance | BindingFlags.Public);
        }

        internal static PropertyInfo GetProperty(Type type, string name, bool nonPublic)
        {
            return type.GetProperty(name,
                nonPublic
                    ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    : BindingFlags.Instance | BindingFlags.Public);
        }
#endif


        internal static object ParseEnum(Type type, string value)
        {
#if FEAT_IKVM
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (string.Equals(field.Name, value, StringComparison.OrdinalIgnoreCase)) return field.GetRawConstantValue();
            }
            throw new ArgumentException("Enum value could not be parsed: " + value + ", " + type.FullName);
#else
            return Enum.Parse(type, value, true);
#endif
        }


        internal static MemberInfo[] GetInstanceFieldsAndProperties(Type type, bool publicOnly)
        {
#if WINRT
            System.Collections.Generic.List<MemberInfo> members = new System.Collections.Generic.List<MemberInfo>();
            foreach(FieldInfo field in type.GetRuntimeFields())
            {
                if(field.IsStatic) continue;
                if(field.IsPublic || !publicOnly) members.Add(field);
            }
            foreach(PropertyInfo prop in type.GetRuntimeProperties())
            {
                MethodInfo getter = Helpers.GetGetMethod(prop, true, true);
                if(getter == null || getter.IsStatic) continue;
                if(getter.IsPublic || !publicOnly) members.Add(prop);
            }
            return members.ToArray();
#else
            var flags = publicOnly
                ? BindingFlags.Public | BindingFlags.Instance
                : BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var props = type.GetProperties(flags);
            var fields = type.GetFields(flags);
            var members = new MemberInfo[fields.Length + props.Length];
            props.CopyTo(members, 0);
            fields.CopyTo(members, props.Length);
            return members;
#endif
        }

        internal static Type GetMemberType(MemberInfo member)
        {
#if WINRT || PORTABLE
            PropertyInfo prop = member as PropertyInfo;
            if (prop != null) return prop.PropertyType;
            FieldInfo fld = member as FieldInfo;
            return fld == null ? null : fld.FieldType;
#else
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) member).PropertyType;
                default:
                    return null;
            }
#endif
        }

        internal static bool IsAssignableFrom(Type target, Type type)
        {
#if WINRT
            return target.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
#else
            return target.IsAssignableFrom(type);
#endif
        }
    }

    /// <summary>
    ///     Intended to be a direct map to regular TypeCode, but:
    ///     - with missing types
    ///     - existing on WinRT
    /// </summary>
    internal enum ProtoTypeCode
    {
        Empty = 0,
        Unknown = 1, // maps to TypeCode.Object
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Single = 13,
        Double = 14,
        Decimal = 15,
        DateTime = 16,
        String = 18,

        // additions
        TimeSpan = 100,
        ByteArray = 101,
        Guid = 102,
        Uri = 103,
        Type = 104
    }
}