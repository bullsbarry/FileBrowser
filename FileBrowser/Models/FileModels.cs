using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace FileBrowser.Models
{
    public class FSResult
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }
    }

    public class FileResult : FSResult
    {
        public long Length { get; set; }
    }

    public class DirectoryResult : FSResult
    {

    }

    public class ListResult
    {
        public DirectoryResult Current { get; set; }
        public List<FileResult> Files { get; set; } = new List<FileResult>();
        public List<DirectoryResult> Directories { get; set; } = new List<DirectoryResult>();
    }


    public class FileManager
    {
        private static string RootPath = ConfigurationManager.AppSettings["FileSystemRoot"];

        public static ListResult GetFileList(string Path)
        {
            var result = new ListResult();

            var listPath = RootPath + Path;
            var directoryInfo = new DirectoryInfo(listPath);

            if (directoryInfo.Exists)
            {
                result.Current = new DirectoryResult()
                {
                    Name = directoryInfo.Name,
                    Created = directoryInfo.CreationTime,
                    LastAccessTime = directoryInfo.LastAccessTime,
                    LastWriteTime = directoryInfo.LastWriteTime
                };

                foreach (var directory in directoryInfo.GetDirectories())
                {
                    result.Directories.Add(new DirectoryResult()
                    {
                        Name = directory.Name,
                        Created = directory.CreationTime,
                        LastAccessTime = directory.LastAccessTime,
                        LastWriteTime = directory.LastWriteTime
                    });
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    result.Files.Add(new FileResult()
                    {
                        Name = file.Name,
                        Created = file.CreationTime,
                        LastAccessTime = file.LastAccessTime,
                        LastWriteTime = file.LastWriteTime,
                        Length = file.Length
                    });
                }
            }

            return result;
        }
    }
}