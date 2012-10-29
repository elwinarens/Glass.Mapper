using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines.ObjectCaching
{
    public class ObjectCachingException : ApplicationException
    {
        public ObjectCachingException(string message)
            : base(message)
        {
        }
    }
}
