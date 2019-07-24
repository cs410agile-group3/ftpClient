using System;
using Renci.SshNet;
using Renci.SshNet.Sftp;

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
    }
}
