using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FileBrowser.Results
{
    public class FileBytesResult : IHttpActionResult
    {
        private readonly string name;
        private readonly byte[] contents;

        public FileBytesResult(string FileName, byte[] Contents)
        {
            name = FileName;
            contents = Contents;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(contents),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(name));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = name
            };

            return Task.FromResult(response);
        }
    }
}