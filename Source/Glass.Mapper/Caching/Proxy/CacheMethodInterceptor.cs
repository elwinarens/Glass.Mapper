using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Glass.Mapper.Caching.CacheKeyResolving;
using Glass.Mapper.Caching.ObjectCaching;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching.Proxy
{
    public class CacheMethodInterceptor : IInterceptor
    {
        private Dictionary<string, object> _values;
        private object _originalTarget;
        private ICacheKey _cacheKey;

        public static DateTime LastUpdated { get; set; }

        private DateTime _lastUpdated;


        static CacheMethodInterceptor()
        {
            LastUpdated = DateTime.Now;
        }

        public CacheMethodInterceptor(object originalTarget, ObjectConstructionArgs args)
        {
            _values = new Dictionary<string, object>();
            _originalTarget = originalTarget;
            _lastUpdated = DateTime.Now;
            _cacheKey = args.CacheKey;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            if (LastUpdated > _lastUpdated)
            {
                var newCacheKey =
                    Context.Default.ObjectCacheConfiguration.ObjectCache.GetLatestCacheKey(_cacheKey.GetId());
                if (!newCacheKey.Equals(_cacheKey))
                {
                    _originalTarget = Context.Default.ObjectCacheConfiguration.ObjectCache.GetObject(newCacheKey);
                    _values = new Dictionary<string, object>();
                    _cacheKey = newCacheKey;
                }
            }

            if (invocation.Method.IsSpecialName)
            {
                if (invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_"))
                {
                    //Must be a property

                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);

                    //if the dictionary contains the name then a value must have been set
                    if (method == "get_")
                    {
                        if (_values.ContainsKey(name))
                        {
                            invocation.ReturnValue = _values[name];
                            return;
                        }
                        _values[name] = invocation.Method.Invoke(_originalTarget, invocation.Arguments);
                        invocation.ReturnValue = _values[name];
                        return;
                    }
                    
                    if (method == "set_")
                    {
                        _values[name] = invocation.Arguments[0];
                        return;
                    }
                }
            }
                

            invocation.ReturnValue = invocation.Method.Invoke(_originalTarget, invocation.Arguments);

        }



        #endregion
    }
}
