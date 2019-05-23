using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ex3
{
    class Comunication
    {

        NetworkStream stream;
        TcpClient client; // client
        private TcpListener server; // server

        private BinaryReader reader; // reader
        public bool Stop { get; set; } = false;
        public bool Connected { get; set; } = false; // is the clinet connected?

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
        public void Open(string ip, int port)
        {
            if (server == null)
            {
                server = new TcpListener(new IPEndPoint(IPAddress.Parse(ip), port));
                server.Start();
            }
        }
        public string[] read_from_simulator()
        {
            if (!Connected)
            {
                Connected = true;
                client = server.AcceptTcpClient();
                reader = new BinaryReader(client.GetStream());
            }
            char c;
            string commands = "";
            //read char by char into commands until enter
            while ((c = reader.ReadChar()) != '\n')
            {
                commands += c;

            }
            if (Connected)
            {
                //split by comma
                string[] param = commands.Split(',');
                string[] lon_lat = { param[0], param[1],param[21] }; // for lon and lat
                return lon_lat;
            }
            else
            {
                return null;
            }

        }
    }
}