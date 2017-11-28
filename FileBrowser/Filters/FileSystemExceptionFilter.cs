using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http.Filters;

namespace FileBrowser.Filters
{
    public class FileSystemExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Exception is DirectoryNotFoundException ||
               actionExecutedContext.Exception is FileNotFoundException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else if(actionExecutedContext.Exception is UnauthorizedAccessException ||
                    actionExecutedContext.Exception is SecurityException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}