using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpCenter.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

        // GET: Editor/Videos
        public ActionResult Videos()
        {
            return View();
        }

        // GET: Editor/Pictures
        public ActionResult Pictures()
        {
            return View();
        }

        // GET: Editor/Files
        public ActionResult Files()
        {
            return View();
        }
    }
}