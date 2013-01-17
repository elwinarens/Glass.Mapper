using System;
using Castle.DynamicProxy;

namespace Glass.Mapper.Caching.Proxy
{
    public class CacheProxyGeneratorHook : IProxyGenerationHook
    {
            #region IProxyGenerationHook Members

            public void MethodsInspected()
            {
            }

            public void NonVirtualMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
            {
                throw new NotSupportedException("Type {0} contains non-virtual member {1}".Formatted(type.FullName, memberInfo.Name));
            }

            public bool ShouldInterceptMethod(Type type, System.Reflection.MethodInfo methodInfo)
            {
                return true;
            }

            #endregion

            #region IProxyGenerationHook Members


            public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
            {
                //  throw new MapperException("Could not proxy type {0} with member {1}".Formatted(type.FullName, memberInfo.Name));
            }

            #endregion
    }
}
