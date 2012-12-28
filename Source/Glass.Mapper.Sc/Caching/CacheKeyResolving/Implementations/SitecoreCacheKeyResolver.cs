using Glass.Mapper.Caching.CacheKeyResolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Caching.CacheKeyResolving.Implementations
{
    public class SitecoreCacheKeyResolver : AbstractCacheKeyResolver
    {
        public override CacheKey GetKey(Mapper.Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            var scTypeContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
            if (scTypeContext != null)
            {
                return new CacheKey(
                    new Guid(scTypeContext.Item.Fields[Sitecore.FieldIDs.Revision].Value),
                    scTypeContext.Item.Database.Name,
                    args.AbstractTypeCreationContext.RequestedType
                    );
            }

            throw  new CannotGenerateKeyException("Can not resolve Key");
        }
    }
}
