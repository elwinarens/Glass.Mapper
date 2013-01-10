using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public interface IDependencyResolver
    {
        T Resolve<T>(IDictionary<string, object> args = null);
        T TryResolve<T>(IDictionary<string, object> args = null);
        IEnumerable<T> ResolveAll<T>();
        IEnumerable<T> ResolveAllInOrder<T>(string field);
        void Load(string context, IGlassConfiguration config);
    }
}
