using System.Collections;
using Castle.MicroKernel;
using Castle.Windsor;
using System.Collections.Generic;
using Glass.Mapper.Configuration;
using System.Linq;

namespace Glass.Mapper.CastleWindsor
{
    public class CastleDependencyResolver : IDependencyResolver
    {
        private WindsorContainer _container;

        public T Resolve<T>(IDictionary<string, object> args = null)
        {
            //TODO: Aaron This is bad need to look into a different way of doing this
            try
            {
                if (args == null)
                    return _container.Resolve<T>();

                return _container.Resolve<T>((IDictionary)args);
            }
            catch (ComponentNotFoundException ex)
            {
                if (typeof (T) != typeof (AbstractObjectCacheConfiguration))
                    throw;
            }

            return default(T);
        }

        public void Load(string contextName, IGlassConfiguration config)
        {

            var castleConfig = config as GlassCastleConfigBase;
            if(castleConfig == null)
                throw new MapperException("IGlassConfiguration is not of type GlassCastleConfigBase");

            _container = new WindsorContainer();
            castleConfig.Configure(_container, contextName);

        }


        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        public IEnumerable<T> ResolveAllInOrder<T>(string field)
        {
            return from t in _container.ResolveAll<T>()
                       orderby field + ", DEC"
                       select t;
        }
    }
}
