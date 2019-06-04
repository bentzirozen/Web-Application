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
using System.Web.UI;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        private static int index=0;
        private static string[] data = { };
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        // 
        // GET: /display/ip/port/refreshrate(?) 
        [HttpGet]
        public ActionResult display(string ip, int port, int refreshRate = 0)
        {
            IPAddress ipAdress;
            Comunication info = Comunication.Instance;
            //check its an ip and make the display for it
            if (IPAddress.TryParse(ip, out ipAdress))
            {
                //connect to simulator from getting data 
                info.IP = ip;
                info.Port = port;
                info.Connect();
                if (refreshRate == 0)
                {
                    //return view fo display with 2 parameters
                    return View("~/Views/Main/display2params.cshtml");
                }
                else
                {
                    Session["refreshRate"] = refreshRate;
                }
                //returnes view of 3 parameters with refresh rate
                return View("~/Views/Main/display.cshtml");
            }
            //in case its the display of file name and rate
            else
            {
                //set indexer fo data to the start of the array
                index = 0;
                //file name is first parameter ('ip')
                info.FilePath = AppDomain.CurrentDomain.BaseDirectory + ip + ".csv";
                data = System.IO.File.ReadAllLines(info.FilePath);
                //refresh rate is the second parameter ('port')
                Session["refreshRate"] = port;
                //number of data
                Session["data"] = data.Length;
                //return view of display from file
                return View("~/Views/Main/displayFromFile.cshtml");
            }
        }
        // 
        // GET: /save/ip/port/refreshrate/timeout/file name 
        [HttpGet]
        public ActionResult save(string ip,int port,int refreshRate,int timeout,string fileName)
        {
            Comunication info = Comunication.Instance;
            info.IP = ip;
            info.Port = port;
            info.Connect();
            Session["refreshRate"] = refreshRate;
            Session["timeout"] = timeout;
            info.FilePath = AppDomain.CurrentDomain.BaseDirectory + fileName + ".csv";
            //return view of save
            return View("~/Views/Main/save.cshtml");
        }
        //creates file from the name that given
        [HttpPost]
        public void OpenFile()
        {
            Comunication.Instance.CreateFile();
        }
        //writes to the file ( seperate from open - to deny intersections)
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
            //get all needed data from simulator
            Comunication info = Comunication.Instance;
            info.Lon = info.read_from_simulator("get /position/longitude-deg\r\n");
            info.Lat = info.read_from_simulator("get /position/latitude-deg\r\n");
            info.Throttle = info.read_from_simulator("get /controls/engines/current-engine/throttle\r\n");
            info.Rudder = info.read_from_simulator("get /controls/flight/rudder\r\n");
            return ToXml(info);
        }
        //close the file
        [HttpPost]
        public void CloseFile()
        {
            Comunication.Instance.CloseFile();
        }
        [HttpPost]
        public string getDataFromFile()
        {
            //get data from file until last data
            if (index == (data.Length - 1)){
                index--;
            }
            Comunication info = Comunication.Instance;
            string[] x = data[index].Split(',');
            //set all data in info and send to XML.
            info.Lon = x[0];
            info.Lat = x[1];
            info.Throttle = x[2];
            info.Rudder = x[3];
            index++;
            return ToXml(info);
        }

    }
}