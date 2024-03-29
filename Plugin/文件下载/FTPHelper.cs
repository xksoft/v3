﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace 文件下载
{
    internal class FTPHelper
    {
      
        private string ftpServerIP;

        private string ftpRemotePath;

        private string ftpUserID;

        private string ftpPassword;

        private string ftpURI;



        /// <summary>

        /// 连接FTP

        /// </summary>

        /// <param name="FtpServerIP">FTP连接地址</param>

        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>

        /// <param name="FtpUserID">用户名</param>

        /// <param name="FtpPassword">密码</param>

        public FTPHelper(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)

        {

            ftpServerIP = FtpServerIP;

            ftpRemotePath = FtpRemotePath;

            ftpUserID = FtpUserID;

            ftpPassword = FtpPassword;
          
         
            ftpURI = ftpServerIP + ftpRemotePath ;
           
            if (!ftpURI.StartsWith("ftp://"))
            {
                ftpURI = "ftp://" + ftpURI;
            }


        }



        public  Boolean FtpUpload(string ftpPath, string localFile)
        {
            //检查目录是否存在，不存在创建   
            FtpCheckDirectoryExist(ftpPath);
            FileInfo fi = new FileInfo(localFile);
            FileStream fs = fi.OpenRead();
            long length = fs.Length;
            Console.WriteLine("文件下载：FTP调用["+ ftpServerIP + ftpPath + fi.Name + "]");
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpServerIP + ftpPath + fi.Name);
            req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.ContentLength = length;
            req.Timeout = 10 * 1000;
            try
            {
                Stream stream = req.GetRequestStream();
                int BufferLength = 2048; //2K      
                byte[] b = new byte[BufferLength];
                int i;
                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                {
                    stream.Write(b, 0, i);
                }
                stream.Close();
                stream.Dispose();
            }
            catch (Exception e)
            {
              throw new Exception("文件上传失败:"+e.Message);
                return false;
            }
            finally
            {
                fs.Close();
                req.Abort();
            }
            req.Abort();
            return true;
        }



        /// <summary>

        /// 下载

        /// </summary>

        /// <param name="filePath"></param>

        /// <param name="fileName"></param>

        public void Download(string filePath, string fileName)

        {

            FtpWebRequest reqFTP;

            try

            {

                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);



                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(ftpURI + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;

                int bufferSize = 2048;

                int readCount;

                byte[] buffer = new byte[bufferSize];



                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)

                {

                    outputStream.Write(buffer, 0, readCount);

                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                }



                ftpStream.Close();

                outputStream.Close();

                response.Close();

            }

            catch (Exception ex)

            {
                throw new Exception("文件下载失败："+ex.Message); 
             

            }

        }





        /// <summary>

        /// 删除文件

        /// </summary>

        /// <param name="fileName"></param>

        public void Delete(string fileName)

        {

            try

            {

                string uri = ftpURI + fileName;

                FtpWebRequest reqFTP;

                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(uri));



                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;



                string result = String.Empty;

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                long size = response.ContentLength;

                Stream datastream = response.GetResponseStream();

                StreamReader sr = new StreamReader(datastream);

                result = sr.ReadToEnd();

                sr.Close();

                datastream.Close();

                response.Close();

            }

            catch (Exception ex)

            {
                throw new Exception("文件["+fileName+"]删除失败：" + ex.Message); 
               

            }

        }



        /// <summary>

        /// 删除文件夹

        /// </summary>

        /// <param name="folderName"></param>

        public void RemoveDirectory(string folderName)

        {

            try

            {

                string uri = ftpURI + folderName;

                FtpWebRequest reqFTP;

                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(uri));



                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;



                string result = String.Empty;

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                long size = response.ContentLength;

                Stream datastream = response.GetResponseStream();

                StreamReader sr = new StreamReader(datastream);

                result = sr.ReadToEnd();

                sr.Close();

                datastream.Close();

                response.Close();

            }

            catch (Exception ex)

            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Delete Error --> " + ex.Message + "  文件名:" + folderName);

            }

        }



        /// <summary>

        /// 获取当前目录下明细(包含文件和文件夹)

        /// </summary>

        /// <returns></returns>

        public string[] GetFilesDetailList()

        {

            string[] downloadFiles;

            try

            {

                StringBuilder result = new StringBuilder();

                FtpWebRequest ftp;

                ftp = (FtpWebRequest) FtpWebRequest.Create(new Uri(ftpURI));

                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = ftp.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);



                //while (reader.Read() > 0)

                //{



                //}

                string line = reader.ReadLine();

                //line = reader.ReadLine();

                //line = reader.ReadLine();



                while (line != null)

                {

                    result.Append(line);

                    result.Append("\n");

                    line = reader.ReadLine();

                }

                result.Remove(result.ToString().LastIndexOf("\n"), 1);

                reader.Close();

                response.Close();

                return result.ToString().Split('\n');

            }

            catch (Exception ex)

            {

                downloadFiles = null;

                throw new Exception("获取目录失败：" + ex.Message); 

                

            }return downloadFiles;

        }



        /// <summary>

        /// 获取当前目录下文件列表(仅文件)

        /// </summary>

        /// <returns></returns>

        public string[] GetFileList(string mask)

        {

            string[] downloadFiles;

            StringBuilder result = new StringBuilder();

            FtpWebRequest reqFTP;

            try

            {

                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(ftpURI));

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                WebResponse response = reqFTP.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);



                string line = reader.ReadLine();

                while (line != null)

                {

                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")

                    {



                        string mask_ = mask.Substring(0, mask.IndexOf("*"));

                        if (line.Substring(0, mask_.Length) == mask_)

                        {

                            result.Append(line);

                            result.Append("\n");

                        }

                    }

                    else

                    {

                        result.Append(line);

                        result.Append("\n");

                    }

                    line = reader.ReadLine();

                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);

                reader.Close();

                response.Close();

                return result.ToString().Split('\n');

            }

            catch (Exception ex)

            {

                downloadFiles = null;

                if (ex.Message.Trim() != "远程服务器返回错误: (550) 文件不可用(例如，未找到文件，无法访问文件)。")

                {

                    Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileList Error --> " + ex.Message.ToString());

                }

                return downloadFiles;

            }

        }



        /// <summary>

        /// 获取当前目录下所有的文件夹列表(仅文件夹)

        /// </summary>

        /// <returns></returns>

        public string[] GetDirectoryList()

        {

            string[] drectory = GetFilesDetailList();

            string m = string.Empty;

            foreach (string str in drectory)

            {

                int dirPos = str.IndexOf("<DIR>");

                if (dirPos > 0)

                {

                    /*判断 Windows 风格*/

                    m += str.Substring(dirPos + 5).Trim() + "\n";

                }

                else if (str.Trim().Substring(0, 1).ToUpper() == "D")

                {

                    /*判断 Unix 风格*/

                    string dir = str.Substring(52).Trim();

                    if (dir != "." && dir != "..")

                    {

                        m += dir + "\n";

                    }

                }

            }



            char[] n = new char[] {'\n'};

            return m.Split(n);

        }


        //判断文件的目录是否存,不存则创建   
        public  void FtpCheckDirectoryExist(string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空     
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        FtpMakeDir(curDir);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("目录创建失败:" + e.Message);
                       
                    }
                }
            }
        }

        public  string FtpParseDirectory(string destFilePath)
        {
            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        //创建目录   
        public  Boolean FtpMakeDir(string localFile)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpServerIP + localFile);
            req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        } 



        /// <summary>

        /// 判断当前目录下指定的文件是否存在

        /// </summary>

        /// <param name="RemoteFileName">远程文件名</param>

        public bool FileExist(string RemoteFileName)

        {

            string[] fileList = GetFileList("*.*");

            foreach (string str in fileList)

            {

                if (str.Trim() == RemoteFileName.Trim())

                {

                    return true;

                }

            }

            return false;

        }



        /// <summary>

        /// 创建文件夹

        /// </summary>

        /// <param name="dirName"></param>

        public void MakeDir(string dirName)

        {

            FtpWebRequest reqFTP;

            try

            {

                // dirName = name of the directory to create.
                string uri = ftpURI +dirName.Replace("\\", "/");
                uri = uri.Replace("///", "/");
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)

            {

                throw new Exception("建立目录时发生错误：" + ex.Message);

            }

        }



        /// <summary>

        /// 获取指定文件大小

        /// </summary>

        /// <param name="filename"></param>

        /// <returns></returns>

        public long GetFileSize(string filename)

        {

            FtpWebRequest reqFTP;

            long fileSize = 0;

            try

            {

                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(ftpURI + filename));

                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                fileSize = response.ContentLength;



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)

            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileSize Error --> " + ex.Message);

            }

            return fileSize;

        }



        /// <summary>

        /// 改名

        /// </summary>

        /// <param name="currentFilename"></param>

        /// <param name="newFilename"></param>

        public void ReName(string currentFilename, string newFilename)

        {

            FtpWebRequest reqFTP;

            try

            {

                reqFTP = (FtpWebRequest) FtpWebRequest.Create(new Uri(ftpURI + currentFilename));

                reqFTP.Method = WebRequestMethods.Ftp.Rename;

                reqFTP.RenameTo = newFilename;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse) reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)

            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "ReName Error --> " + ex.Message);

            }

        }



        /// <summary>

        /// 移动文件

        /// </summary>

        /// <param name="currentFilename"></param>

        /// <param name="newFilename"></param>

        public void MovieFile(string currentFilename, string newDirectory)

        {

            ReName(currentFilename, newDirectory);

        }



        /// <summary>

        /// 切换当前目录

        /// </summary>

        /// <param name="DirectoryName"></param>

        /// <param name="IsRoot">true 绝对路径   false 相对路径</param>

        public void GotoDirectory(string DirectoryName, bool IsRoot)

        {

            if (IsRoot)

            {

                ftpRemotePath = DirectoryName;

            }

            else

            {

                ftpRemotePath += DirectoryName.Replace("//","/");

            }

            ftpURI =ftpURI + ftpRemotePath ;

        }



        /// <summary>

        /// 删除订单目录

        /// </summary>

        /// <param name="ftpServerIP">FTP 主机地址</param>

        /// <param name="folderToDelete">FTP 用户名</param>

        /// <param name="ftpUserID">FTP 用户名</param>

        /// <param name="ftpPassword">FTP 密码</param>

        public static void DeleteOrderDirectory(string ftpServerIP, string folderToDelete, string ftpUserID,
            string ftpPassword)
        {

            try
            {

                if (!string.IsNullOrEmpty(ftpServerIP) && !string.IsNullOrEmpty(folderToDelete) &&
                    !string.IsNullOrEmpty(ftpUserID) && !string.IsNullOrEmpty(ftpPassword))

                {

                    FTPHelper fw = new FTPHelper(ftpServerIP, folderToDelete, ftpUserID, ftpPassword);

                    //进入订单目录

                    fw.GotoDirectory(folderToDelete, true);

                    //获取规格目录

                    string[] folders = fw.GetDirectoryList();

                    foreach (string folder in folders)

                    {

                        if (!string.IsNullOrEmpty(folder) || folder != "")

                        {

                            //进入订单目录

                            string subFolder = folderToDelete + "/" + folder;

                            fw.GotoDirectory(subFolder, true);

                            //获取文件列表

                            string[] files = fw.GetFileList("*.*");

                            if (files != null)

                            {

                                //删除文件

                                foreach (string file in files)

                                {

                                    fw.Delete(file);

                                }

                            }

                            //删除冲印规格文件夹

                            fw.GotoDirectory(folderToDelete, true);

                            fw.RemoveDirectory(folder);

                        }

                    }



                    //删除订单文件夹

                    string parentFolder = folderToDelete.Remove(folderToDelete.LastIndexOf('/'));

                    string orderFolder = folderToDelete.Substring(folderToDelete.LastIndexOf('/') + 1);

                    fw.GotoDirectory(parentFolder, true);

                    fw.RemoveDirectory(orderFolder);

                }

                else

                {

                    throw new Exception("FTP 及路径不能为空！");

                }

            }

            catch (Exception ex)

            {

                throw new Exception("删除文件时发生错误，错误信息为：" + ex.Message);

            }

        }

    }
    



    public class Insert_Standard_ErrorLog

    {

        public static void Insert(string x, string y)

        {



        }

    }
}
