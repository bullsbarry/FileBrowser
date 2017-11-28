using FileBrowser.Filters;
using FileBrowser.Models;
using FileBrowser.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileBrowser.Controllers
{
    [RoutePrefix("api/v1")]
    [FileSystemExceptionFilter]
    public class FileController : ApiController
    {
        /// <summary>
        /// Takes a file path and returns a list of files and directories on the server under that path.
        /// </summary>
        /// <param name="path">The path to list.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("files/{*path}")]
        public IHttpActionResult List(string path)
        {
            return Ok(FileManager.GetFileList(path));
        }

        [HttpGet]
        [Route("file/{*path}")]
        public IHttpActionResult GetFile(string path)
        {
            return new FileBytesResult(Path.GetFileName(path), FileManager.GetFile(path));
        }
    }
}
