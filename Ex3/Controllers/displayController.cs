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
        public ActionResult display(string ip, int port,int refreshRate=0)
        {
            Comunication.Instance.Connect(ip, port);
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
            Session["lon"] = lon+180;
            Session["lat"] = lat+90;
            return View("~/Views/display/display.cshtml");
        }
    }
}