/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections;
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
            if (args == null)
                return _container.Resolve<T>();

            return _container.Resolve<T>((IDictionary)args);
        }

        public T TryResolve<T>(IDictionary<string, object> args = null)
        {
            //TODO: Aaron This is bad need to look into a different way of doing this
            try
            {
                return Resolve<T>(args);
            }
            catch (Exception ex)
            {
                if (typeof(T) != typeof(AbstractObjectCacheConfiguration))
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



