using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;

namespace Glass.Mapper.Caching.Proxy
{
    public class CacheLazyObjectInterceptor: IInterceptor
    {
        Dictionary<string, object> _values = new Dictionary<string, object>();

        LazyObjectInterceptor _subInterceptor;

        public CacheLazyObjectInterceptor(LazyObjectInterceptor subInterceptor)
        {
            _subInterceptor = subInterceptor;
        }

        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    if (method == "get_" && _values.ContainsKey(name))
                    {
                        invocation.ReturnValue = _values[name];
                        return;
                    }
                    else if (method == "set_")
                    {
                        _values[name] = invocation.Arguments[0];
                        return;
                    }
                }

            }

            _subInterceptor.Intercept(invocation);
        } 
    }
}
