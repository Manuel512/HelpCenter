using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpCenter.Models;

namespace HelpCenter.Controllers
{
    public class ImageController : Controller
    {
        private HelpCenter2Entities db = new HelpCenter2Entities();

        private string[] imgtype = new string[] { ".png", ".jpg", ".gif", ".bmp" };

        private int UserId = 1;

        // GET: Image
        public ActionResult Index()
        {
            return View(db.GetImages(null));
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
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
                        string img = System.IO.Path.GetFileName(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/img"), img);

                        if (!imgtype.Contains(ext))
                        {
                            ViewBag.Error = "There are images with a not valid format, Valid formats are: png, jpg, gif, bmp";
                            return View();
                        }
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Error = "The file " + img + " already exists";
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
                        string img = System.IO.Path.GetFileName(file.FileName);
                        string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Content/img"), img);

                        // file is uploaded
                        file.SaveAs(path);

                        db.NewImage(img, UserId);
                    }

                }
            }
            else
            {
                ViewBag.Error = "You must select files to upload"; ;
                return View();
            }

            // after successfully uploading redirect the user
            return RedirectToAction("Index", "Image");
        }

        // GET: Image/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_GetImages_Result Image = db.GetImages(id).FirstOrDefault();
            if (Image == null)
            {
                return HttpNotFound();
            }
            return View(Image);
        }

        // POST: Image/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, int id)
        {
            sp_GetImages_Result Image = db.GetImages(id).FirstOrDefault();

            if (Image == null)
            {
                return HttpNotFound();
            }

            if (file != null)
            {
                string ext = System.IO.Path.GetExtension(file.FileName);
                string img = System.IO.Path.GetFileName(file.FileName);
                string pathOld = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/img"), Image.Name);
                string pathNew = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/img"), img);

                if (!imgtype.Contains(ext))
                {
                    ViewBag.Error = "There are videos with a not valid format, Valid formats are: png, jpg, gif, bmp";
                    return View(Image);
                }
                if (System.IO.File.Exists(pathNew))
                {
                    ViewBag.Error = "The file " + img + " already exists";
                    return View(Image);
                }

                System.IO.File.Delete(pathOld);
                file.SaveAs(pathNew);
                db.UpdateImage(id, img, UserId);
                Image = db.GetImages(id).FirstOrDefault();
            }
            else
            {
                ViewBag.Error = "You must select files to upload";
                return View(Image);
            }

            return RedirectToAction("Edit", "Image", id);
        }

        // POST: Image/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sp_GetImages_Result Image = db.GetImages(id).FirstOrDefault();

            if (Image == null)
            {
                return HttpNotFound();
            }

            string path = System.IO.Path.Combine(Server.MapPath("~/Content/img"), Image.Name);
            System.IO.File.Delete(path);

            db.DeleteImage(id);

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
