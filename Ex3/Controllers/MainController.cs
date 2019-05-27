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
        static Timer timer;
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
            Random rnd = new Random();
            Comunication.Instance.IP = ip;
            Comunication.Instance.Port = port;
            Comunication.Instance.Connect();
            rate = refreshRate;
            int lon = Comunication.Instance.read_from_simulator("get /position/longitude-deg\r\n");
            int lat = Comunication.Instance.read_from_simulator("get /position/latitude-deg\r\n");
            if (refreshRate == 0)
            {
                Session["refresh"] = 0;
            }
            else
            {
                Session["refresh"] = 1;
                Session["rate"] = refreshRate;
            }
            Session["time"] = time;
            Session["lon"] = rnd.Next(1000);
            Session["lat"] = rnd.Next(1000);
            lonList.Add(lon.ToString());
            latList.Add(lat.ToString());
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
            Comunication.Instance.IP = ip;
            Comunication.Instance.Port = port;
            Comunication.Instance.Connect();
            rate = refreshRate;
            time = timeout;
            Session["rate"] = refreshRate;
            Session["timeout"] = timeout;
            string path = AppDomain.CurrentDomain.BaseDirectory + fileName + ".csv";
            Session["path"] = path;
            
            return View("~/Views/Main/save.cshtml");
        }
        [HttpPost]
        public void CreateFile()
        {
            for(int i = 0; i < 40; i++)
            {
                refresh();
                System.Threading.Thread.Sleep(250);
            }
            string filePath = Session["path"] as string;
            StreamWriter streamWriter = new StreamWriter(filePath);
            string first = "lon";
            string second = "lat";
            // streamWriter.WriteLineAsync("aka"); // the writing needs to be done in another func.
            string check = string.Format("{0},{1}", first, second);
            streamWriter.WriteLineAsync(check);
            for(int i = 0; i < lonList.Count; i++)
            {
                check = string.Format("{0},{1}", lonList[i], latList[i]);
                streamWriter.WriteLineAsync(check);
            }
           
            streamWriter.Close(); // closing will also be in it's own func.
        }

        public void refresh()
        {
            display(Comunication.Instance.IP, Comunication.Instance.Port, rate);
        }

    }
}