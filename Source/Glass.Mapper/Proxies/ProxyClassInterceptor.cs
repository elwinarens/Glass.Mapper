using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Proxies
{
    public class ProxyClassInterceptor : IInterceptor
    {
        private readonly ObjectConstructionArgs _args;
        private object _actual;
        private readonly AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>> _runner;

        public ProxyClassInterceptor(ObjectConstructionArgs args,
                                     AbstractPipelineRunner<AbstractPipelineArgs, IPipelineTask<AbstractPipelineArgs>>
                                         runner)
        {
            _args = args;
            _runner = runner;
        }


        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            //create class
            if (_actual == null)
            {
                _runner.Run(_args);
                _actual = _args.Result;
            }

            invocation.ReturnValue = invocation.Method.Invoke(_actual, invocation.Arguments);

        }

        #endregion


    }
}
