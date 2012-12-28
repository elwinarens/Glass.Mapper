using Castle.Windsor;

namespace Glass.Mapper.CastleWindsor
{
    public abstract class GlassCastleConfigBase : IGlassConfiguration
    {
        public abstract void Configure(WindsorContainer container, string contextName);
    }
}
