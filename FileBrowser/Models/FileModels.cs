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
        public string Created { get; set; }
        public string LastAccessTime { get; set; }
        public string LastWriteTime { get; set; }
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
                    Created = directoryInfo.CreationTime.ToString("G"),
                    LastAccessTime = directoryInfo.LastAccessTime.ToString("G"),
                    LastWriteTime = directoryInfo.LastWriteTime.ToString("G")
                };

                foreach (var directory in directoryInfo.GetDirectories())
                {
                    result.Directories.Add(new DirectoryResult()
                    {
                        Name = directory.Name,
                        Created = directory.CreationTime.ToString("G"),
                        LastAccessTime = directory.LastAccessTime.ToString("G"),
                        LastWriteTime = directory.LastWriteTime.ToString("G")
                    });
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    result.Files.Add(new FileResult()
                    {
                        Name = file.Name,
                        Created = file.CreationTime.ToString("G"),
                        LastAccessTime = file.LastAccessTime.ToString("G"),
                        LastWriteTime = file.LastWriteTime.ToString("G"),
                        Length = file.Length
                    });
                }
            }

            return result;
        }
    }
}