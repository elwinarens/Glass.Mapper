using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Caching.CacheKeyResolving
{
    public abstract class CacheKey<TIdType> : IEquatable<CacheKey<TIdType>>, ICacheKey
    {
        protected CacheKey(TIdType id, TIdType revisionId, string database, Type type)
            : this()
        {
            Id = id;
            RevisionId = revisionId;
            Database = database;
            Type = type;
        }

        protected CacheKey()
        {
        }

        public TIdType Id { get; private set; }
        public TIdType RevisionId { get; private set; }
        public string Database { get; private set; }
        public Type Type { get; private set; }

        public override string ToString()
        {
            return "{0},{1},{2}".Formatted(RevisionId, Database, Type);
        }

        public abstract bool Equals(CacheKey<TIdType> other);
    }
}
