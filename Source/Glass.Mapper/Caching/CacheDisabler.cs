using System;
using System.Web;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching
{
    public class CacheDisabler:IDisposable
    {

        public static bool CacheDisabled
        {
            get
            {
                return HttpContext.Current != null ? ReturnFromHttpContxt() : ReturnFromThreadStatic();
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    SetToHttpContext(value);
                }

                SetToThreadStatic(value);
            }
        }

        private static void SetToHttpContext(bool value)
        {
            HttpContext.Current.Items["Glass_CacheDisabled"] = value;
        }

        private static bool ReturnFromHttpContxt()
        {
            if (HttpContext.Current.Items.Contains("Glass_CacheDisabled"))
            {
                {
                    return (bool)HttpContext.Current.Items["Glass_CacheDisabled"];
                }
            }
            return false;
        }

        [ThreadStatic]
        private static bool _threadStaticCacheDisabled;

        private static void SetToThreadStatic(bool value)
        {
            _threadStaticCacheDisabled = value;
        }

        private static bool ReturnFromThreadStatic()
        {
            return _threadStaticCacheDisabled;
        }

        public CacheDisabler()
        {
            CacheDisabled = true;
        }

        public void Dispose()
        {
            CacheDisabled = false;
        }
    }
}
