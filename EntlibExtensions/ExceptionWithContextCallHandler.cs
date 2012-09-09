using System;
using System.Collections;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace ScottyApps.Utilities.EntlibExtensions
{
    public class ExceptionWithContextCallHandler : ExceptionCallHandler
    {
        public ExceptionWithContextCallHandler(ExceptionPolicyImpl exceptionPolicy)
            : base(exceptionPolicy) { }

        public ExceptionWithContextCallHandler(ExceptionPolicyImpl exceptionPolicy, int order)
            : base(exceptionPolicy, order)
        {
        }

        /// <summary>
        /// Processes the method call.
        /// </summary>
        /// <remarks>This handler does nothing before the call. If there is an exception
        /// returned, it runs the exception through the Exception Handling Application Block.</remarks>
        /// <param name="input"><see cref="IMethodInvocation"/> with information about the call.</param>
        /// <param name="getNext">delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target, or the (possibly changed) exceptions.</returns>
        public new IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            var result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                try
                {
                    DumpMethodParameters(input, result.Exception.Data);
                    bool rethrow = this.ExceptionPolicy.HandleException(result.Exception);
                    if (!rethrow)
                    {
                        // Exception is being swallowed
                        result.ReturnValue = null;
                        result.Exception = null;

                        if (input.MethodBase.MemberType == MemberTypes.Method)
                        {
                            var method = (MethodInfo)input.MethodBase;
                            if (method.ReturnType != typeof(void))
                            {
                                result.Exception =
                                    new InvalidOperationException(
                                        Resources.CantSwallowNonVoidReturnMessage);
                            }
                        }
                    }
                    // Otherwise the original exception will be returned to the previous handler
                }
                catch (Exception ex)
                {
                    // New exception was returned
                    result.Exception = ex;
                }
            }
            return result;
        }

        private void DumpMethodParameters(IMethodInvocation input, IDictionary dic)
        {
            if (input.Inputs == null || input.Inputs.Count <= 0) return;
            for (var i = 0; i < input.Inputs.Count; i++)
            {
                // TODO the parameter value (such as collection type) can be flattened with JSON format
                dic.Add(input.Inputs.ParameterName(i), input.Inputs[i]);
            }
        }
    }
}
