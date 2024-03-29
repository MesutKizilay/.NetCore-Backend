﻿using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using Core.Utilities.Messages;
using Core.CrossCuttingConcerns.Logging.SeriLog;
using System.Text.Json;

namespace Core.Aspects.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private readonly Log4NetServiceBase _log4NetServiceBase;
        private readonly SeriLogServiceBase _seriLogServiceBase;
        private readonly Type _type;

        public LogAspect(Type loggerService)
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

        protected override void OnBefore(IInvocation invocation)
        {
            if (_type.BaseType == typeof(Log4NetServiceBase))
            {
                _log4NetServiceBase.Info(GetLogDetail(invocation));
            }
            else
            {
                _seriLogServiceBase.Info(JsonSerializer.Serialize(GetLogDetail(invocation)));
            }
        }

        private LogDetail GetLogDetail(IInvocation invocation)
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

            var logDetail = new LogDetail
            {
                MethodName = invocation.Method.Name,
                LogParameters = logParameters
            };

            return logDetail;
        }
    }
}