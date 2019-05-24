using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class displayController : Controller
    {
        // 
        // GET: /display/ 

        public ActionResult Index()
        {
            return View();
        }

        // 
        // GET: /display/Welcome/ 

        public string Welcome(string ip, int port = 5409)
        {
            Comunication.Instance.Open(ip, port);
            string[] lon_lat = Comunication.Instance.read_from_simulator();
            return HttpUtility.HtmlEncode("lon: " + lon_lat[0] + ", lat: " + lon_lat[1]+", rudder: "+lon_lat[2]);
        }
    }
}