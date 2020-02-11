using System;
using System.IO;

namespace Fluid.Core.Sandbox
{
    internal class Program
    {
        public static string CurrentDirectory { get; set; } = Environment.CurrentDirectory;

        public static string TestFileDirectoryVirtualPath { get; set; } = "../../../../files/test/pictures/";

        private static void Main(string[] args)
        {
            var physicalPath = GetPhysicalDirectory(CurrentDirectory, TestFileDirectoryVirtualPath);

            
        }


        private static string GetPhysicalDirectory(string currentDirectory, string virtualPath)
        {
            var currentPath = currentDirectory;
            var directoryInfo = new DirectoryInfo(currentPath);

            do
            {
                virtualPath = virtualPath.Remove(0, 3);

                directoryInfo = directoryInfo?.Parent;
            } while (virtualPath.Contains("../"));

            var physicalPath = Path.Combine(directoryInfo?.FullName, virtualPath).Replace("/", "\\");

            return Directory.Exists(physicalPath) ? physicalPath : null;
        }
    }
}