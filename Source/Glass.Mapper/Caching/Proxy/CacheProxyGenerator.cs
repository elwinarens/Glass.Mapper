using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Castle.DynamicProxy;
using Glass.Mapper.Caching.Proxy;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Proxies;

namespace Glass.Mapper.Caching.Proxy
{

    public class CacheProxyGenerator
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        private static readonly ProxyGenerationOptions Options =
            new ProxyGenerationOptions(new CacheProxyGeneratorHook());


        public static object CreateProxy(object originalTarget, ObjectConstructionArgs args)
        {
            Type type = originalTarget.GetType();

            //you can't proxy a proxy.
            if (originalTarget is IProxyTargetAccessor)
            {
                var oldProxy = originalTarget as IProxyTargetAccessor;
                var interceptors = oldProxy.GetInterceptors();
                if (interceptors.Any(x => x is InterfaceMethodInterceptor))
                {
                    var subInterceptor =
                        interceptors.First(x => x is InterfaceMethodInterceptor).CastTo<InterfaceMethodInterceptor>();

                    return Generator.CreateInterfaceProxyWithoutTarget(
                        type,
                        new CacheInterfaceMethodInterceptor(subInterceptor));

                }
                return oldProxy;
            }
            return Generator.CreateClassProxy(type, Options, new CacheMethodInterceptor(originalTarget, args));
        }



        //public static object CreateInstance(Type vmType)
        //{
        //    VerifyViewModelType(vmType);

        //    // Create everything required to get a module builder
        //    AssemblyName assemblyName = new AssemblyName("SmartViewModelDynamicAssembly");
        //    AppDomain domain = AppDomain.CurrentDomain;
        //    AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        //    //AssemblyBuilderAccess.RunAndSave);
        //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

        //    string dynamicTypeName = Assembly.CreateQualifiedName(vmType.AssemblyQualifiedName, "Smart" + vmType.Name);

        //    TypeBuilder typeBuilder = moduleBuilder.DefineType(dynamicTypeName,
        //                                                       TypeAttributes.Public | TypeAttributes.Class, vmType);

        //    MethodInfo raisePropertyChangedMethod = vmType.GetMethod("RaisePropertyChanged",
        //                                                                             BindingFlags.NonPublic |
        //                                                                             BindingFlags.Instance, null,
        //                                                                             new Type[] {typeof (string)}, null);

        //    foreach (PropertyInfo propertyInfo in FindNotifyPropertyChangCandidates(vmType))
        //        UpdateProperty(propertyInfo, typeBuilder, raisePropertyChangedMethod);

        //    Type dynamicType = typeBuilder.CreateType();

        //    return (T) Activator.CreateInstance(dynamicType);
        //}

        //private static void VerifyViewModelType(Type vmType)
        //{
        //    if (vmType.IsSealed)
        //        throw new InvalidOperationException("The specified view model type is not allowed to be sealed.");
        //}

        //private static IEnumerable<PropertyInfo> FindNotifyPropertyChangCandidates(Type vmType)
        //{
        //    return from p in vmType.GetProperties()
        //           where p.GetSetMethod() != null && p.GetSetMethod().IsVirtual &&
        //                 p.GetCustomAttributes(typeof (RaisePropertyChangedAttribute), false).Length > 0
        //           select p;
        //}

        //private static void UpdateProperty(PropertyInfo propertyInfo, TypeBuilder typeBuilder,
        //                                   MethodInfo raisePropertyChangedMethod)
        //{
        //    // Update the setter of the class
        //    PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name,
        //                                                                 PropertyAttributes.None,
        //                                                                 propertyInfo.PropertyType, null);

        //    // Create set method
        //    MethodBuilder builder = typeBuilder.DefineMethod("set_" + propertyInfo.Name,
        //                                                     MethodAttributes.Public | MethodAttributes.Virtual, null,
        //                                                     new Type[] {propertyInfo.PropertyType});
        //    builder.DefineParameter(1, ParameterAttributes.None, "value");
        //    ILGenerator generator = builder.GetILGenerator();

        //    // Add IL code for set method
        //    generator.Emit(OpCodes.Nop);
        //    generator.Emit(OpCodes.Ldarg_0);
        //    generator.Emit(OpCodes.Ldarg_1);
        //    generator.Emit(OpCodes.Call, propertyInfo.GetSetMethod());

        //    // Call property changed for object
        //    generator.Emit(OpCodes.Nop);
        //    generator.Emit(OpCodes.Ldarg_0);
        //    generator.Emit(OpCodes.Ldstr, propertyInfo.Name);
        //    generator.Emit(OpCodes.Callvirt, raisePropertyChangedMethod);
        //    generator.Emit(OpCodes.Nop);
        //    generator.Emit(OpCodes.Ret);
        //    propertyBuilder.SetSetMethod(builder);
        //}
    }
}
