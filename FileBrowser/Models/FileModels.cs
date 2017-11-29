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
        public string Path { get; set; }
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

        public static byte[] GetFile(string FilePath)
        {
            var filePath = Path.Combine(RootPath, FilePath);
            return File.ReadAllBytes(filePath);
        }

        public static ListResult GetFileList(string DirectoryPath, string Search)
        {
            var result = new ListResult();

            var listPath = Path.Combine(RootPath, DirectoryPath ?? string.Empty);
            var directoryInfo = new DirectoryInfo(listPath);

            if (directoryInfo.Exists)
            {
                result.Current = new DirectoryResult()
                {
                    Name = directoryInfo.Name,
                    Path = directoryInfo.FullName.Replace(RootPath, "/"),
                    Created = directoryInfo.CreationTime.ToString("G"),
                    LastAccessTime = directoryInfo.LastAccessTime.ToString("G"),
                    LastWriteTime = directoryInfo.LastWriteTime.ToString("G")
                };

                var directories = string.IsNullOrEmpty(Search) ? 
                    directoryInfo.GetDirectories() : directoryInfo.GetDirectories(Search);

                result.Directories.AddRange(
                    directories.Select(directory => new DirectoryResult()
                    {
                        Name = directory.Name,
                        Path = directory.FullName.Replace(RootPath, "/"),
                        Created = directory.CreationTime.ToString("G"),
                        LastAccessTime = directory.LastAccessTime.ToString("G"),
                        LastWriteTime = directory.LastWriteTime.ToString("G")
                    })
                );

                var files = string.IsNullOrEmpty(Search) ?
                    directoryInfo.GetFiles() : directoryInfo.GetFiles(Search);

                result.Files.AddRange(
                    files.Select(file => new FileResult()
                    {
                        Name = file.Name,
                        Path = file.FullName.Replace(RootPath, "/"),
                        Created = file.CreationTime.ToString("G"),
                        LastAccessTime = file.LastAccessTime.ToString("G"),
                        LastWriteTime = file.LastWriteTime.ToString("G"),
                        Length = file.Length
                    })
                );
            }

            return result;
        }

        public static void AddFile(string FilePath, byte[] Contents)
        {
            var path = Path.Combine(RootPath, FilePath);
            if(File.Exists(path))
            {
                throw new UnauthorizedAccessException("A file with that name already exists");
            }

            File.WriteAllBytes(path, Contents);
        }
    }
}