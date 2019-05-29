using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Ex3.Models
{
    class Comunication
    {

        NetworkStream stream;
        TcpClient client; // client
        private TcpListener server; // server
        private BinaryWriter writer;
        private BinaryReader reader; // reader
        private string lon;
        private string lat;
        private StreamWriter streamWriter;
        private static Mutex mutex=new Mutex();
        private static bool file_open = false;
        public string Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
            }
        }
        public string Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }
        public bool Stop { get; set; } = false;
        public bool Connected { get; set; } = false; // is the clinet connected?
        public string IP { get; set; }
        public int Port { get; set; }
        public string FilePath { get; set; }
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
        public string read_from_simulator(string command)
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
            return x;
        }
        public void CreateFile()
        {
            streamWriter = new StreamWriter(FilePath);
            file_open = true;
            
        }
        public void WriteFile()
        {
            if (file_open == true)
            {
                mutex.WaitOne();
                {
                    string lon_lat = this.Lon.ToString() + "," + this.Lat.ToString();
                    streamWriter.WriteLineAsync(lon_lat);
                }
                mutex.ReleaseMutex();
            }
        }
        public void CloseFile()
        {
            mutex.WaitOne();
            {
                streamWriter.Close();
                file_open = false;

            }
            mutex.ReleaseMutex();
            
        }
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Data");
            writer.WriteElementString("lon", lon);
            writer.WriteElementString("lat", lat);
            writer.WriteEndElement();
        }
    }
}