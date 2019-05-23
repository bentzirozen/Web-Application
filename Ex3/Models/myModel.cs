using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class myModel
    {
        private static myModel s_instance = null;
        public static myModel Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new myModel();
                }
                return s_instance;
            }
        }
        public string ip { get; set; }
        public string port { get; set; }
        public int time { get; set; }
        public void ReadData(string commands)
        {

        }

    }
}