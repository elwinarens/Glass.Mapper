using System;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching
{
    public class CacheDisabler:IDisposable
    {
        private readonly ObjectConstructionArgs _args;
        private readonly IAbstractService _abstractService;

        public CacheDisabler(ObjectConstructionArgs args)
        {
            this._args = args;
            args.DisableCache = true;
        }

        public CacheDisabler(IAbstractService abstractService)
        {
            this._abstractService = abstractService;
            _abstractService.DisableCache = true;
        }

        public void Dispose()
        {
            if(_args != null)
            _args.DisableCache = false;

            if(_abstractService != null)
                _abstractService.DisableCache = false;
        }
    }
}
