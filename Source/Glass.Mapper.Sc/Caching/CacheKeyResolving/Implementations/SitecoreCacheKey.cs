using System;
using Glass.Mapper.Caching.CacheKeyResolving;

namespace Glass.Mapper.Sc.Caching.CacheKeyResolving.Implementations
{
    public class SitecoreCacheKey: CacheKey<Guid>
    {
        public SitecoreCacheKey(Guid id, Guid revisionId, string database, Type type)
            : base(id, revisionId, database, type)
        {
        }

        public SitecoreCacheKey()
        {
        }

        public override bool Equals(CacheKey<Guid> other)
        {
            return other.Id.Equals(this.Id) && other.RevisionId.Equals(this.RevisionId) && other.Database == this.Database && other.Type == this.Type;
        }
    }
}
