using System.Collections.Generic;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    public class ObjectConstructionArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Context of the item being created
        /// </summary>
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }

        /// <summary>
        /// The configuration to use to load the object
        /// </summary>
        public AbstractTypeConfiguration Configuration { get; private set; }

        public AbstractObjectCacheConfiguration ObjectCacheConfiguration { get; private set; }

        public bool DisableCache { get; internal set; }

        public IAbstractService Service { get; private set; }

        public virtual object Result { get; set; }

        public ObjectOrigin ObjectOrigin { get; set; }

        public ObjectConstructionArgs() : base()
        { }

        public ObjectConstructionArgs(
            Context context, 
            AbstractTypeCreationContext abstractTypeCreationContext, 
            AbstractTypeConfiguration configuration,
            IAbstractService service)
            : base(context)
        {
            AbstractTypeCreationContext = abstractTypeCreationContext;
            Configuration = configuration;
            Service = service;
        }

        public ObjectConstructionArgs(
            Context context,
            AbstractTypeCreationContext abstractTypeCreationContext,
            AbstractTypeConfiguration configuration,
            IAbstractService service,
            AbstractObjectCacheConfiguration objectCacheConfiguration)
            : this(context, abstractTypeCreationContext, configuration, service)
        {
            ObjectCacheConfiguration = objectCacheConfiguration;
        }

       

     

      

    }
}
