using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Proxies
{
    public class InterfaceMethodInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;
        private readonly AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>> _runner;
        private bool _isLoaded;

        public Dictionary<string, object> Values { get; private set; }

        private IInvocation _invocation;

        public IInvocation Invocation
        {
            get { return _invocation; }
        }

        public InterfaceMethodInterceptor(ObjectConstructionArgs args,
                                     AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>>
                                         runner)
        {
            _args = args;
            _runner = runner;
            Values = new Dictionary<string, object>();
        }

        #region IInterceptor Members

        public virtual void Intercept(IInvocation invocation)
        {
            _invocation = invocation;
            //do initial gets
            if (!_isLoaded)
            {
                _runner.Run(_args);

                foreach (var property in _args.Configuration.Type.GetProperties())
                {
                    Values[property.Name] = property.GetValue(_args.Result, null);
                }
                _isLoaded = true;
            }

            if (invocation.Method.IsSpecialName)
            {
                if(invocation.Method.Name.StartsWith("get_") || invocation.Method.Name.StartsWith("set_")){
                    
                    string method = invocation.Method.Name.Substring(0, 4);
                    string name = invocation.Method.Name.Substring(4);
                    
                    if(method == "get_"){
                        var result = Values[name];
                        invocation.ReturnValue = result;
                    }
                    else if(method == "set_"){
                        Values[name] = invocation.Arguments[0];
                    }
                    else
                        throw new MapperException("Method with name {0}{1} on type {2} not supported.".Formatted(method, name, _args.AbstractTypeCreationContext.RequestedType.FullName));

                }
               
            }

        }



        #endregion
    }
}
