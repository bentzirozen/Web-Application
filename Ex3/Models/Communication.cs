using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ex3.Models
{
    class Comunication
    {

        NetworkStream stream;
        TcpClient client; // client
        private TcpListener server; // server
        private BinaryWriter writer;
        private BinaryReader reader; // reader
        public bool Stop { get; set; } = false;
        public bool Connected { get; set; } = false; // is the clinet connected?
        public string IP { get; set; }
        public int Port { get; set; }
        public TcpClient Client { get; set; }
        #region Singleton
        private static Comunication m_Instance = null;
        public static Comunication Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Comunication();
                }
                return m_Instance;
            }
        }
        #endregion

        // open server with ip and port
        public void Connect()
        {
            if (client != null) return;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), Port);
            Client = new TcpClient();
            client = Client;
            while (!client.Connected) // keep trynig to connect
            {
                try { client.Connect(ep); }
                catch (Exception) { }
            }
            Connected = true;
            writer = new BinaryWriter(client.GetStream());
            stream = client.GetStream();

        }
        public int read_from_simulator(string command)
        { //for empty fommands
            stream = Client.GetStream();
            string x = "";
            Byte[] data = Encoding.ASCII.GetBytes(command);
            stream.Write(data, 0, data.Length);
            // Receive the TcpServer.response.
            // Buffer to store the response bytes.
            data = new Byte[1024];
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = Encoding.ASCII.GetString(data, 0, bytes);
            x = Regex.Match(responseData, (@"[\-\+]\d+")).Value;
            if (x == "")
            {
                x = Regex.Match(responseData, (@"\d+")).Value;
            }
            return int.Parse(x);
        }
        public void CreateFile(string filePath)
        {
            StreamWriter streamWriter = new StreamWriter(filePath);
            string first = "lon";
            string second = "lat";
           // streamWriter.WriteLineAsync("aka"); // the writing needs to be done in another func.
            string check = string.Format("{0},{1}", first, second);
            streamWriter.WriteLineAsync(check);
            first = "157.26265959";
            second = "21.64784";
            check = string.Format("{0},{1}", first, second);
            streamWriter.WriteLineAsync(check);
            streamWriter.Close(); // closing will also be in it's own func.
        }
    }
}