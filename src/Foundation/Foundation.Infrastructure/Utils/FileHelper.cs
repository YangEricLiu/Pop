/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: FileHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for File
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/


using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public class FileHelper
    {
        private const uint ErrorLockViolation = 0x80070021;
        private const uint ErrorSharingViolation = 0x80070020;

        public static bool MoveFileToDirectory(string filePath, string targetDirectory)
        {
            //generate the target path first
            string targetPath = GenerateTargetPath(filePath, targetDirectory);

            if (targetPath == null)
                return false;

            return MoveFile(filePath, targetPath);
        }
        public static bool CopyFileToDirectory(string filePath, string targetDirectory)
        {
            //generate the target path first
            string targetPath = GenerateTargetPath(filePath, targetDirectory);

            if (targetPath == null)
                return false;

            return CopyFile(filePath, targetPath);
        }
        public static bool MoveFile(string filePath, string targetPath)
        {
            try
            {
                StringBuilder sb = new StringBuilder()
                    .Append("Trying to move file: ")
                    .Append(filePath)
                    .Append(" to target file: ")
                    .Append(targetPath)
                    .Append(".");
                LogHelper.LogInformation(sb.ToString());

                File.Move(filePath, targetPath);
                return true;
            }
            catch (IOException ioEx)
            {
                uint hresult = HrForException(ioEx);
                if (hresult == ErrorLockViolation
                    || hresult == ErrorSharingViolation)
                {
                    StringBuilder sb = new StringBuilder()
                        .Append("Can not move file: ")
                        .Append(filePath)
                        .Append(" to target file: ")
                        .Append(targetPath)
                        .Append("."); ;
                    LogHelper.LogError(sb.ToString());
                }
                else
                {
                    //LogHelper.LogException(ioEx, severity: LoggingSeverity.Fatal);
                }

                return false;
            }
            catch (System.Exception ex)
            {
                LogHelper.LogException(ex, severity: LoggingSeverity.Fatal);
                return false;
            }
        }
        public static bool CopyFile(string filePath, string targetPath)
        {
            try
            {
                StringBuilder sb = new StringBuilder()
                    .Append("Trying to copy file: ")
                    .Append(filePath)
                    .Append(" to target file: ")
                    .Append(targetPath)
                    .Append(".");
                LogHelper.LogInformation(sb.ToString());

                File.Copy(filePath, targetPath);
                return true;
            }
            catch (IOException ioEx)
            {
                uint hresult = HrForException(ioEx);
                if (hresult == ErrorLockViolation
                    || hresult == ErrorSharingViolation)
                {
                    StringBuilder sb = new StringBuilder()
                        .Append("Can not copy file: ")
                        .Append(filePath)
                        .Append(" to target file: ")
                        .Append(targetPath)
                        .Append("."); ;
                    LogHelper.LogError(sb.ToString());
                }
                else
                {
                    //LogHelper.LogException(ioEx, severity: LoggingSeverity.Fatal);
                }

                return false;
            }
            catch (System.Exception ex)
            {
                LogHelper.LogException(ex, severity: LoggingSeverity.Fatal);
                return false;
            }
        }
        private static uint HrForException(System.Exception ex1)
        {
            return unchecked((uint)System.Runtime.InteropServices.Marshal.GetHRForException(ex1));
        }

        public static void CreateDirectoryIfNotExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static string GenerateTargetPath(string filePath, string targetDirectory)
        {
            try
            {
                //judge whether the target file exists
                StringBuilder sb = new StringBuilder()
                    .Append(targetDirectory)
                    .Append(Path.GetFileName(filePath));

                // If the target file exists, generate the unique name of target file
                if (File.Exists(sb.ToString()))
                {
                    sb.Clear()
                        .Append(targetDirectory)
                        .Append(Path.GetFileNameWithoutExtension(filePath))
                        .Append("_")
                        .Append(Guid.NewGuid().ToString())
                        .Append(Path.GetExtension(filePath));
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                return null;
            }

        }

        public static void LogMessageForFile(LoggingSeverity severity, string message, string filePath)
        {
            StringBuilder sb = new StringBuilder()
                    .Append(message)
                    .Append(" File: ")
                    .Append(filePath)
                    .Append(".");
            switch (severity)
            {
                case LoggingSeverity.Information:
                    LogHelper.LogInformation(sb.ToString());
                    break;
                case LoggingSeverity.Error:
                    LogHelper.LogError(sb.ToString());
                    break;
                case LoggingSeverity.Fatal:
                    LogHelper.LogFatal(sb.ToString());
                    break;
            }

        }

        public static void WriteFileContent(string fileName, string content)
        {
            FileHelper.CreateDirectoryIfNotExist(Path.GetDirectoryName(fileName));

            StreamWriter writer = File.CreateText(fileName);
            writer.Write(content);
            writer.Flush();
            writer.Close();
        }

        public static byte[] ReadFileBytes(string fileName)
        {
            List<byte> content = new List<byte>();
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                int length = 512, bytesRead = 0;
                byte[] buffer = new byte[length];

                while ((bytesRead = stream.Read(buffer, 0, length)) > 0)
                {
                    content.AddRange(buffer);
                }
            }
            return content.ToArray();
        }

        public static void DeleteIfPossible(params string[] fileNames)
        {
            try
            {
                foreach (var fileName in fileNames)
                {
                    var file = new FileInfo(fileName);
                    file.Delete();
                }
            }
            catch
            {
            }
        }
    }
}
