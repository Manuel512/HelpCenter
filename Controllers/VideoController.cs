using HelpCenter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HelpCenter.Controllers
{
    public class VideoController : Controller
    {
        private HelpCenterDBContext db = new HelpCenterDBContext();
        HelpCenter2Entities sp = new HelpCenter2Entities();

        private string[] vidtype = new string[] { ".mp4", ".webm", ".ogg", ".wav" };

        private int UserId = 1;

        // GET: Video
        public ActionResult Index()
        {

            return View(sp.GetVideos(null));
        }

        // GET: Video/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Video/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] files)
        {


            if (files.Length > 0)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        string ext = System.IO.Path.GetExtension(file.FileName);
                        string vid = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/videos"), vid);

                        if (!vidtype.Contains(ext))
                        {
                            ViewBag.Error = "There are videos with a not valid format, Valid formats are: mp4,webm,ogg,wav";
                            return View();
                        }
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Error = "The file " + vid + " already exists";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "You must select files to upload";
                        return View();
                    }
                }

                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string vid = System.IO.Path.GetFileName(file.FileName);
                        string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/videos"), vid);

                        // file is uploaded
                        file.SaveAs(path);

                        sp.NewVideo(vid, UserId);
                    }

                }
            }
            else
            {
                ViewBag.Error = "You must select files to upload"; ;
                return View();
            }

            // after successfully uploading redirect the user
            return RedirectToAction("Index", "Video");
        }


        // GET: Video/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sp_GetVideos_Result Video = sp.GetVideos(id).FirstOrDefault();

            if (Video == null)
            {
                return HttpNotFound();
            }

            return View(Video);
        }

        // POST: Video/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, int id)
        {
            sp_GetVideos_Result Video = sp.GetVideos(id).FirstOrDefault();

            if (Video == null)
            {
                return HttpNotFound();
            }

            if (file != null)
            {
                string ext = System.IO.Path.GetExtension(file.FileName);
                string vid = System.IO.Path.GetFileName(file.FileName);
                string pathOld = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/videos"), Video.Name);
                string pathNew = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/videos"), vid);

                if (!vidtype.Contains(ext))
                {
                    ViewBag.Error = "There are videos with a not valid format, Valid formats are: mp4,webm,ogg,wav";
                    return View(Video);
                }
                if (System.IO.File.Exists(pathNew))
                {
                    ViewBag.Error = "The file " + vid + " already exists";
                    return View(Video);
                }

                System.IO.File.Delete(pathOld);
                file.SaveAs(pathNew);
                sp.UpdateVideo(id, vid, UserId);
                Video = sp.GetVideos(id).FirstOrDefault();
            }
            else
            {
                ViewBag.Error = "You must select files to upload";
                return View(Video);
            }

            return RedirectToAction("Edit", "Video", id);
        }


        // POST: Video/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sp_GetVideos_Result Video = sp.GetVideos(id).FirstOrDefault();

            if (Video == null)
            {
                return HttpNotFound();
            }

            string path = System.IO.Path.Combine(Server.MapPath("~/Content/videos"), Video.Name);
            System.IO.File.Delete(path);

            sp.DeleteVideo(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
