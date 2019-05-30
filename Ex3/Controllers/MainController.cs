using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;
using System.Text;
using System.Xml;
using System.IO;
using System.Timers;
using System.Net;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        
        private static int rate;
        private static int time;
        private static int index=0;
        private static string[] data = { };
        private static List<string> lonList = new List<string>();
        private static List<string> latList = new List<string>();
        private static int i;
        // 
        // GET: /display/ 
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        // 
        // GET: /display/Welcome/ 
        [HttpGet]
        public ActionResult display(string ip, int port, int refreshRate = 0)
        {
            IPAddress ipAdress;
            Comunication info = Comunication.Instance;
            //check its an ip and make the display for it
            if (IPAddress.TryParse(ip, out ipAdress))
            {
                info.IP = ip;
                info.Port = port;
                info.Connect();
                rate = refreshRate;
                Session["fromSimulatior"] = 1;
                if (refreshRate == 0)
                {
                    return View("~/Views/Main/display2params.cshtml");
                }
                else
                {
                    Session["refreshRate"] = refreshRate;
                }
                Session["time"] = time;
                return View("~/Views/Main/display.cshtml");
            }
            //in case its the display of file name and rate
            else
            {
                index = 0;
                info.FilePath = AppDomain.CurrentDomain.BaseDirectory + ip + ".csv";
                data = System.IO.File.ReadAllLines(info.FilePath);
                Session["refreshRate"] = port;
                return View("~/Views/Main/displayFromFile.cshtml");
            }
        }
       

        [HttpGet]
        public ActionResult dispaly(string fileName,int rate)
        {
            return View();
        }
        [HttpGet]
        public ActionResult save(string ip,int port,int refreshRate,int timeout,string fileName)
        {
            Comunication info = Comunication.Instance;
            info.IP = ip;
            info.Port = port;
            info.Connect();
            rate = refreshRate;
            time = timeout;
            Session["refreshRate"] = refreshRate;
            Session["timeout"] = timeout;
            info.FilePath = AppDomain.CurrentDomain.BaseDirectory + fileName + ".csv";
            return View("~/Views/Main/save.cshtml");
        }
        [HttpPost]
        public void OpenFile()
        {
            Comunication.Instance.CreateFile();
        }
        [HttpPost]
        public void WriteFile()
        {
            Comunication.Instance.WriteFile();
        }

        private string ToXml(Comunication info)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();

            info.ToXml(writer);

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
        [HttpPost]
        public string getData()
        {
            Comunication info = Comunication.Instance;
            info.Lon = info.read_from_simulator("get /position/longitude-deg\r\n");
            info.Lat = info.read_from_simulator("get /position/latitude-deg\r\n");
            info.Throttle = info.read_from_simulator("get /controls/engines/current-engine/throttle\r\n");
            info.Rudder = info.read_from_simulator("get /controls/flight/rudder\r\n");
            return ToXml(info);
        }
        [HttpPost]
        public void CloseFile()
        {
            Comunication.Instance.CloseFile();
        }
        [HttpPost]
        public string getDataFromFile()
        {
            if (index >= (data.Length - 1)){
                return null;
            }
            Comunication info = Comunication.Instance;

            string[] x = data[index].Split(',');
            info.Lon = x[0];
            info.Lat = x[1];
            info.Throttle = x[2];
            info.Rudder = x[3];
            index++;
            return ToXml(info);
        }

    }
}