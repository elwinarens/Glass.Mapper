using System;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching
{
    public class CacheDisabler:IDisposable
    {
        private readonly ObjectConstructionArgs _args;

        public CacheDisabler(ObjectConstructionArgs args)
        {
            this._args = args;
            args.DisableCache = true;
        }

        public void Dispose()
        {
            _args.DisableCache = false;
        }
    }
}
