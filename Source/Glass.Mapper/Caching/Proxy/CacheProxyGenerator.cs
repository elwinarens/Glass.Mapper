using System;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.ObjectCaching.Proxy;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Proxies;

namespace Glass.Mapper.Caching.Proxy
{

    public class CacheProxyGenerator
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();
        private static readonly ProxyGenerationOptions Options = new ProxyGenerationOptions(new CacheProxyGeneratorHook());


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
                    var subInterceptor = interceptors.First(x => x is InterfaceMethodInterceptor).CastTo<InterfaceMethodInterceptor>();

                    return Generator.CreateInterfaceProxyWithoutTarget(
                        type,
                        new CacheInterfaceMethodInterceptor(subInterceptor));
                        
                }
                return oldProxy;
            }
            return Generator.CreateClassProxy(type, Options, new CacheMethodInterceptor(originalTarget, args));
        }
    }
}
