using System;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Collections.Generic;
using System.Collections;

namespace FtpCli.Packages.ClientWrapper
{

    public class Client : IClient
    {
        private SftpClient _client;
        private ConnectionInfo _connection;

        // Explicit constructor
        // Should be used in all cases
        public Client(string host, int port, string username, string password)
        {
             _connection = new ConnectionInfo(host, port, username,
                 new PasswordAuthenticationMethod(username, password)); 

             // All operations will be performed through the '_client'
             _client = new SftpClient(_connection);

             try {
                _client.Connect();
                Console.WriteLine("Connection successful!\n");
             } catch (Exception err) 
             {
                Console.WriteLine($"Unable to connect to {host}:{port}");
                throw new Exception(err.ToString());
             }
        }


        public bool IsConnected {
          get { return _client.IsConnected; }
        }

        public void Connect()
        {
            if(!this.IsConnected)
            {
               _client.Connect(); 
            }
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
        

        public void Rename(String source, String destination){
            _client.RenameFile(source,destination);
        }

        public void RemoteDeleteFile(string filename)
        {
            _client.DeleteFile(filename);
        }

        public void RemoteDeleteDirectory(string dirname)
        {
            _client.DeleteDirectory(dirname);

        }

        public void ChangePermissions(string path, short mode)
        {
            try{
                _client.ChangePermissions(path, mode);
            }
            catch{
                Console.WriteLine("File Not Found");
            }

        }

        public void ListRemote(string path)
        {
            try {
                foreach (var file in _client.ListDirectory(path)) {
                    Console.WriteLine(file.Name);
                }
            } catch {
                Console.WriteLine($"Could not find directory {path}");
            }
        }

        public void GetFile(string srcPath, string destPath)
        {
            try {
                using(var file = File.OpenWrite(destPath)) {
                    _client.DownloadFile(srcPath, file);
                }
                Console.WriteLine($"Wrote {srcPath} to {destPath}");
            } catch {
                File.Delete(destPath);
                Console.WriteLine($"Could not get file {srcPath} and write to {destPath}");
            }
        }

        public void GetMultiFiles(List<string> srcPath)
        {
            try {
                foreach (string src in srcPath) {
                    using(var file = File.OpenWrite(src)) {
                        _client.DownloadFile(src, file);
                    }
                    Console.WriteLine($"Wrote remote {src} to local {src}");
                }
            } catch {
                Console.WriteLine($"Could not get file {srcPath} and write to local");
            }
        }

        public void PutFile(string srcPath, string destPath) 
        {
            try {
                using(var file = File.OpenRead(destPath)) {
                    _client.UploadFile(file, srcPath);
                }
                Console.WriteLine($"Wrote {srcPath} to {destPath}");
            } catch(Exception e) {
                Console.WriteLine("Error Message:" + e.Message);
                Console.WriteLine($"Could not put file {srcPath} and write to {destPath}");
            }

        }

        public void PutDirectory(string srcPath, string destPath) 
        {
            try {
                //using(var file = File.OpenRead(destPath)) {
                //    _client.UploadFile(file, srcPath);
                //}
                UploadDirectory(_client,srcPath,destPath);
                Console.WriteLine($"Wrote {srcPath} to {destPath}");
            } catch(Exception e) {
                //Console.WriteLine("Error Message:" + e.Message);
                //Console.WriteLine($"Could not put file {srcPath} and write to {destPath}");
            }

        }


    void UploadDirectory(SftpClient client, string localPathup, string remotePathup)
    {
      Console.WriteLine("Uploading directory {0} to {1}", localPathup, remotePathup);

      IEnumerable infos = new DirectoryInfo(localPathup).EnumerateFileSystemInfos();
      foreach (FileSystemInfo info in infos)
      {
        if (info.Attributes.HasFlag(FileAttributes.Directory))
        {
          string subPath = remotePathup + "/" + info.Name;
          if (!client.Exists(subPath))
            {
                client.CreateDirectory(subPath);
            }
            UploadDirectory(client, info.FullName, remotePathup + "/" + info.Name);
        }
        else
        {
            using (Stream fileStream = new FileStream(info.FullName, FileMode.Open))
            {
                Console.WriteLine(
                    "Uploading {0} ({1:N0} bytes)",
                    info.FullName, ((FileInfo)info).Length);

                client.UploadFile(fileStream, remotePathup + "/" + info.Name);
            }
        }
    }
    }

    }
}
