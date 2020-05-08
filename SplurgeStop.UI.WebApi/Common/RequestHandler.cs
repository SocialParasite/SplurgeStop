﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SplurgeStop.UI.WebApi.Common
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleCommand<T>(T request, Func<T, Task> handler)
        {
            try
            {
                Log.Logger.Debug("Handling HTTP request of type {type}", typeof(T).Name);
                await handler(request);
                return new OkResult();
            }
            catch (Exception e)
            {
                //Log.Logger.Error(e, "Error handling the command");
                //return new BadRequestObjectResult(new
                //{
                //    error = e.Message,
                //    stackTrace = e.StackTrace
                //});
                throw;
            }
        }

        public static async Task<IActionResult> HandleQuery<TModel>(Func<Task<TModel>> query)
        {
            try
            {
                return new OkObjectResult(await query());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}
