//-------------------------------------------------------------------------------
// <copyright file="DynamicMethodFactory.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2009, Enkari, Ltd.
//   Copyright (c) 2009-2011 Ninject Project Contributors
//   Authors: Nate Kohari (nate@enkari.com)
//            Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

#if !NO_LCG
namespace Ninject.Extensions.Interception.Injection.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// A helper class that uses lightweight code generation to create dynamic methods.
    /// </summary>
    public static class DynamicMethodFactory
    {
        private static readonly Module Module = typeof(DynamicMethodFactory).Module;

        private static readonly ConstructorInfo TargetParameterCountExceptionConstructor = 
            typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes);

        /// <summary>
        /// Creates a new <see cref="Invoker"/> that calls the specified method in a
        /// late-bound manner.
        /// </summary>
        /// <param name="method">The method that the invoker should call.</param>
        /// <returns>A dynamic invoker that can call the specified method.</returns>
        public static Invoker CreateInvoker(MethodInfo method)
        {
            DynamicMethod callable = CreateDynamicInvoker();
            var info = new DelegateBuildInfo(method);

            ILGenerator il = callable.GetILGenerator();

            EmitCheckParameters(info, il, 1);
            var locals = EmitLoadParameters(info, il, 1);

            il.EmitCall(method.IsStatic ? OpCodes.Call : OpCodes.Callvirt, method, null);

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                if (info.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, method.ReturnType);
                }
            }

            EmitReturnRefValues(info, il, locals);

            il.Emit(OpCodes.Ret);
            return callable.CreateDelegate(typeof(Invoker)) as Invoker;
        }

        /// <summary>
        /// Creates a new <see cref="FactoryMethod"/> that calls the specified constructor in a
        /// late-bound manner.
        /// </summary>
        /// <param name="constructor">The constructor that the factory method should call.</param>
        /// <returns>A dynamic factory method that can call the specified constructor.</returns>
        public static FactoryMethod CreateFactoryMethod(ConstructorInfo constructor)
        {
            DynamicMethod callable = CreateDynamicFactoryMethod();
            var info = new DelegateBuildInfo(constructor);

            Type returnType = constructor.ReflectedType;
            ILGenerator il = callable.GetILGenerator();

            EmitCheckParameters(info, il, 0);
            EmitLoadParameters(info, il, 0);

            il.Emit(OpCodes.Newobj, constructor);

            if (info.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);
            return callable.CreateDelegate(typeof(FactoryMethod)) as FactoryMethod;
        }

        /// <summary>
        /// Creates a new <see cref="Getter"/> that gets the value of the specified field in a
        /// late-bound manner.
        /// </summary>
        /// <param name="field">The field that the getter should read from.</param>
        /// <returns>A dynamic getter that can read from the specified field.</returns>
        public static Getter CreateGetter( FieldInfo field )
        {
            DynamicMethod callable = CreateDynamicGetterMethod();

            Type returnType = field.FieldType;
            ILGenerator il = callable.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            EmitUnboxOrCast(il, field.DeclaringType);
            il.Emit(OpCodes.Ldfld, field);

            if (returnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Getter)) as Getter;
        }

        /// <summary>
        /// Creates a new <see cref="Getter"/> that gets the value of the specified property in a
        /// late-bound manner.
        /// </summary>
        /// <param name="property">The property that the getter should read from.</param>
        /// <returns>A dynamic getter that can read from the specified property.</returns>
        public static Getter CreateGetter(PropertyInfo property)
        {
            DynamicMethod callable = CreateDynamicGetterMethod();

            Type returnType = property.PropertyType;
            ILGenerator il = callable.GetILGenerator();
            MethodInfo method = property.GetGetMethod(true);

            il.Emit(OpCodes.Ldarg_0);
            EmitUnboxOrCast(il, property.DeclaringType);

            il.Emit(method.IsFinal ? OpCodes.Call : OpCodes.Callvirt, method);

            if (returnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Getter)) as Getter;
        }

        /// <summary>
        /// Creates a new <see cref="Setter"/> that sets the value of the specified field in a
        /// late-bound manner.
        /// </summary>
        /// <param name="field">The field that the setter should write to.</param>
        /// <returns>A dynamic setter that can write to the specified field.</returns>
        public static Setter CreateSetter( FieldInfo field )
        {
            DynamicMethod callable = CreateDynamicSetterMethod();

            Type returnType = field.FieldType;
            ILGenerator il = callable.GetILGenerator();

            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            EmitUnboxOrCast(il, returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            EmitUnboxOrCast(il, field.DeclaringType);
            il.Emit(OpCodes.Ldloc_0);

            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Setter)) as Setter;
        }

        /// <summary>
        /// Creates a new <see cref="Setter"/> that sets the value of the specified property in a
        /// late-bound manner.
        /// </summary>
        /// <param name="property">The property that the setter should write to.</param>
        /// <returns>A dynamic setter that can write to the specified property.</returns>
        public static Setter CreateSetter(PropertyInfo property)
        {
            DynamicMethod callable = CreateDynamicSetterMethod();

            Type returnType = property.PropertyType;
            ILGenerator il = callable.GetILGenerator();
            MethodInfo method = property.GetSetMethod(true);

            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            EmitUnboxOrCast(il, returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            EmitUnboxOrCast(il, property.DeclaringType);
            il.Emit(OpCodes.Ldloc_0);

            il.Emit(method.IsFinal ? OpCodes.Call : OpCodes.Callvirt, method);

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Setter)) as Setter;
        }

        private static DynamicMethod CreateDynamicInvoker()
        {
#if !NO_SKIP_VISIBILITY
            return new DynamicMethod(string.Empty, typeof(object), new[] { typeof(object), typeof(object[]) }, Module, true);
#else
            return new DynamicMethod( String.Empty, typeof(object), new[] { typeof(object), typeof(object[]) } );
#endif
        }

        private static DynamicMethod CreateDynamicFactoryMethod()
        {
#if !NO_SKIP_VISIBILITY
            return new DynamicMethod(string.Empty, typeof(object), new[] { typeof(object[]) }, Module, true);
#else
            return new DynamicMethod( String.Empty, typeof(object), new[] { typeof(object[]) } );
#endif
        }

        private static DynamicMethod CreateDynamicGetterMethod()
        {
#if !NO_SKIP_VISIBILITY
            return new DynamicMethod(string.Empty, typeof(object), new[] { typeof(object) }, Module, true);
#else
            return new DynamicMethod( String.Empty, typeof(object), new[] { typeof(object) } );
#endif
        }

        private static DynamicMethod CreateDynamicSetterMethod()
        {
#if !NO_SKIP_VISIBILITY
            return new DynamicMethod(string.Empty, typeof(void), new[] { typeof(object), typeof(object) }, Module, true);
#else
            return new DynamicMethod( String.Empty, typeof(void), new[] { typeof(object), typeof(object) } );
#endif
        }

        private static void EmitCheckParameters(DelegateBuildInfo info, ILGenerator il, int argumentArrayIndex)
        {
            Label beginLabel = il.DefineLabel();

            EmitLoadArg(il, argumentArrayIndex);
            il.Emit(OpCodes.Ldlen);
            EmitLoadInt(il, info.Parameters.Length);
            il.Emit(OpCodes.Beq, beginLabel);

            il.Emit(OpCodes.Newobj, TargetParameterCountExceptionConstructor);
            il.Emit(OpCodes.Throw);

            il.MarkLabel(beginLabel);
        }

        private static LocalBuilder[] EmitLoadParameters(DelegateBuildInfo info, ILGenerator il, int argumentArrayIndex)
        {
            if (!info.Method.IsStatic && !(info.Method is ConstructorInfo))
            {
                il.Emit(OpCodes.Ldarg_0);
                EmitUnboxOrCast(il, info.Method.DeclaringType);
            }

            var locals = new LocalBuilder[info.RefParameterIndexes.Count];
            var localIndex = 0;
            for (int index = 0; index < info.Parameters.Length; index++)
            {
                EmitLoadArg(il, argumentArrayIndex);
                EmitLoadInt(il, index);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitUnboxOrCast(il, info.ParameterTypes[index]);
                if (info.Parameters[index].ParameterType.IsByRef)
                {
                    locals[localIndex] = il.DeclareLocal(info.ParameterTypes[index], true);
                    il.Emit(OpCodes.Stloc, locals[localIndex]);
                    il.Emit(OpCodes.Ldloca_S, locals[localIndex++]);
                }
            }

            return locals;
        }

        private static void EmitReturnRefValues(DelegateBuildInfo info, ILGenerator il, LocalBuilder[] locals)
        {
            for (int i = 0; i < locals.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitLoadInt(il, info.RefParameterIndexes[i]);
                il.Emit(OpCodes.Ldloc, locals[i]);
                if (locals[i].LocalType.IsValueType)
                {
                    il.Emit(OpCodes.Box, locals[i].LocalType);
                }

                il.Emit(OpCodes.Stelem_Ref);
            }
        }

        private static void EmitUnboxOrCast(ILGenerator il, Type type)
        {
            il.Emit(type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, type);
        }

        private static void EmitLoadInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (value > -129 && value < 128)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }

        private static void EmitLoadArg(ILGenerator il, int index)
        {
            switch (index)
            {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index > -129 && index < 128)
                    {
                        il.Emit(OpCodes.Ldarg_S, (sbyte)index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }

                    break;
            }
        }

        #region Nested type: DelegateBuildInfo

        private class DelegateBuildInfo
        {
            public DelegateBuildInfo(ConstructorInfo ctor)
            {
                this.Method = ctor;
                this.ReturnType = ctor.ReflectedType;
                this.InitParameters();
            }

            public DelegateBuildInfo(MethodInfo method)
            {
                this.Method = method;
                this.ReturnType = method.ReturnType;
                this.RefParameterIndexes = new List<int>();
                this.InitParameters();
            }

            public IList<int> RefParameterIndexes { get; private set; } 

            public MethodBase Method { get; private set; }

            public Type ReturnType { get; private set; }

            public ParameterInfo[] Parameters { get; private set; }

            public Type[] ParameterTypes { get; private set; }

            private void InitParameters()
            {
                this.Parameters = this.Method.GetParameters();
                this.GetActualParameterTypes(this.Parameters);
            }

            private void GetActualParameterTypes(ParameterInfo[] parameters)
            {
                this.ParameterTypes = new Type[parameters.Length];

                for (int index = 0; index < parameters.Length; index++)
                {
                    Type type = parameters[index].ParameterType;
                    if (type.IsByRef)
                    {
                        this.ParameterTypes[index] = type.GetElementType();
                        this.RefParameterIndexes.Add(index);
                    }
                    else
                    {
                        this.ParameterTypes[index] = type;                        
                    }
                }
            }
        }

        #endregion
    }
}

#endif //!NO_LCG