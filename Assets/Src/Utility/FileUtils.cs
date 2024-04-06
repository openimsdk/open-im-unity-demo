using System.IO;
using System;
using System.Text;
using UnityEngine;
namespace Dawn
{

    public class FileUtils
    {
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found:"
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        // public static string GetStrFromFile(string fullpath)
        // {
        //     FileInfo t = new FileInfo(fullpath);
        //     if (!t.Exists)
        //     {
        //         return "error";
        //     }
        //     StreamReader sr = null;
        //     sr = File.OpenText(fullpath);
        //     string line;
        //     StringBuilder content = new StringBuilder();
        //     while ((line = sr.ReadLine()) != null)
        //     {
        //         content.Append(line);
        //         content.Append("\n");
        //     }
        //     sr.Close();
        //     sr.Dispose();
        //     return content.ToString();
        // }

        public static string GetStrFromFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("File not Exist: " + path);
                return "";
            }
            return File.ReadAllText(path);
        }
        // 去掉BOM头
        public static UTF8Encoding utf8Encouding = new UTF8Encoding(false);
        public static byte[] GetUTF8FormStr(string data)
        {
            return utf8Encouding.GetBytes(data);
        }
        public static byte[] GetUTF8FromFile(string path)
        {
            return utf8Encouding.GetBytes(FileUtils.GetStrFromFile(path));
        }

        public static byte[] GetBytesFromFile(string path)
        {
            FileInfo t = new FileInfo(path);
            if (!t.Exists)
            {
                Debug.LogError(path + " not exist");
                return null;
            }
            return File.ReadAllBytes(path);
        }

        public static byte[] LoadBinFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader read = new BinaryReader(file);
            int count = (int)file.Length;
            byte[] buffer = new byte[count];
            read.Read(buffer, 0, buffer.Length);
            file.Close();
            read.Close();
            read.Dispose();
            return buffer;
        }

    }
}