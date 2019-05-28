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

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        
        private static int rate;
        private static int time;
        const int MaxTimeInterval = 1000000;
        private static List<string> lonList = new List<string>();
        private static List<string> latList = new List<string>();
        private static int i;
        // 
        // GET: /display/ 
        [HttpGet]
        public ActionResult Index()
        {

            Session["lon"] = 200;
            Session["lat"] = 200;
            return View("~/Views/display/display.cshtml");
        }

        // 
        // GET: /display/Welcome/ 
        [HttpGet]
        public ActionResult display(string ip, int port,int refreshRate=0,int time=MaxTimeInterval)
        {
            Comunication info = Comunication.Instance;
            info.IP = ip;
            info.Port = port;
            info.Connect();
            rate = refreshRate;
            if (refreshRate == 0)
            {
                Session["refresh"] = 0;
            }
            else
            {
                Session["refresh"] = 1;
                Session["refreshRate"] = refreshRate;
            }
            Session["time"] = time;
            Session["lat"] = info.Lat;
            Session["lon"] = info.Lon;
            return View("~/Views/Main/display.cshtml");
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
            return ToXml(info);
        }

    }
}