using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.SeriLog;
using System.Text.Json;

namespace Core.Aspects.Autofac.Exception
{
    public class ExceptionLogAspect : MethodInterception
    {
        private readonly Log4NetServiceBase _log4NetServiceBase;
        private readonly SeriLogServiceBase _seriLogServiceBase;
        private readonly Type _type;

        public ExceptionLogAspect(Type loggerService)
        {
            _type = loggerService;


            if (loggerService.BaseType != typeof(Log4NetServiceBase) && loggerService.BaseType != typeof(SeriLogServiceBase))
            {
                throw new System.Exception(AspectMessages.WrongLoggerType);
            }
            else if (loggerService.BaseType == typeof(Log4NetServiceBase))
            {
                _log4NetServiceBase = (Log4NetServiceBase)Activator.CreateInstance(loggerService);
            }
            else
            {
                _seriLogServiceBase = (SeriLogServiceBase)Activator.CreateInstance(loggerService);
            }
        }

        protected override void OnException(IInvocation invocation, System.Exception e)
        {
            LogDetailWithException logDetailWithException = GetLogDetail(invocation);
            logDetailWithException.ExceptionMessage = e.Message;

            if (_type.BaseType == typeof(Log4NetServiceBase))
            {
                _log4NetServiceBase.Error(logDetailWithException);
            }
            else
            {
                _seriLogServiceBase.Error(JsonSerializer.Serialize(logDetailWithException));
            }
        }

        private LogDetailWithException GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();

            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name
                });
            }

            var logDetailWithException = new LogDetailWithException
            {
                MethodName = invocation.Method.Name,
                LogParameters = logParameters
            };

            return logDetailWithException;
        }
    }
}
