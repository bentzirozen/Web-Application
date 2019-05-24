using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class displayController : Controller
    {
        private Comunication info = new Comunication();
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
        public ActionResult display(string ip, int port)
        {
            Comunication.Instance.Connect(ip, port);
            int lon = Comunication.Instance.read_from_simulator("get /position/longitude-deg\r\n");
            int lat = Comunication.Instance.read_from_simulator("get /position/latitude-deg\r\n");
            int tr = Comunication.Instance.read_from_simulator("get /controls/engines/current-engine/throttle\r\n");
            Session["lon"] = lon+180;
            Session["lat"] = lat;
            Session["thr"] = tr;
            return View("~/Views/display/display.cshtml");
        }
    }
}