using System;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.ObjectCaching.Proxy;
using Glass.Mapper.Proxies;

namespace Glass.Mapper.Caching.Proxy
{

    public class CacheProxyGenerator
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private static readonly ProxyGenerationOptions _options = new ProxyGenerationOptions(new CacheProxyGeneratorHook());


        public static object CreateProxy(object originalTarget)
        {
            Type type = originalTarget.GetType();

            object proxy = null ;

            //you can't proxy a proxy.
            if (originalTarget is IProxyTargetAccessor)
            {
                var oldProxy = originalTarget as IProxyTargetAccessor;
                var interceptors = oldProxy.GetInterceptors();
                if (interceptors.Any(x => x is InterfaceMethodInterceptor))
                {
                    var subInterceptor = interceptors.First(x => x is InterfaceMethodInterceptor).CastTo<InterfaceMethodInterceptor>();

                    return _generator.CreateInterfaceProxyWithoutTarget(
                        type,
                        new CacheInterfaceMethodInterceptor(subInterceptor));
                        
                }
                else if (interceptors.Any(x => x is ProxyClassInterceptor))
                {

                }

            }
            else
            {
                proxy = _generator.CreateClassProxy(type, _options, new CacheMethodInterceptor(originalTarget));
            }

            return proxy;
        }
    }
}
