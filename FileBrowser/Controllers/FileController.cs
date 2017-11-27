using FileBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileBrowser.Controllers
{
    [RoutePrefix("api/v1/files")]
    public class FileController : ApiController
    {
        private const string RootPath = "";

        /// <summary>
        /// Takes a file path and returns a list of files and directories on the server under that path.
        /// </summary>
        /// <param name="path">The path to list.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{*path}")]
        public IHttpActionResult List(string path)
        {
            try
            {
                return Ok(FileManager.GetFileList(path));
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
