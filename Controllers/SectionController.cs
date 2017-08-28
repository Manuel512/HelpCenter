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
    public class SectionController : Controller
    {
        private HelpCenter2Entities db = new HelpCenter2Entities();
        private int UserId = 1;

        // GET: Section
        public ActionResult Index()
        {
            IList<sp_CountSectionsByModule_Result> Sections = db.CountSectionsByModule(null).ToList();
            ViewBag.OrderNum = Sections;
            return View(db.GetSections(null, null, true));
        }


        // GET: Section/Create
        public ActionResult Create()
        {
            return View(db.GetModules(null));
        }

        // POST: Section/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Module_id, string[] Section)
        {
            foreach (var item in Section)
            {
                if (item.Length == 0)
                {
                    ViewBag.Error = "Sections Name cant be Empty";
                    return View(db.GetModules(null));
                }
            }

            foreach (var item in Section)
            {
                item.Replace("'", "`");

                db.NewSection(Module_id, item, UserId);
            }

            return RedirectToAction("Index");
        }

        // GET: Section/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Modules = db.GetModules(null);

            sp_GetSections_Result Section = db.GetSections(id, null, null).FirstOrDefault();

            if (Section.Section == null)
            {
                return HttpNotFound();
            }
            return View(Section);
        }

        // POST: Section/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, int Module_id, string Section, bool Active)
        {
            if (Section.Length == 0)
            {
                ViewBag.Error = "Sections Name cant be Empty";
                return View(db.GetModules(null));
            }

            Section.Replace("'", "`");

            db.UpdateSection(id, UserId, Section, Module_id, Active);

            return RedirectToAction("Index");
        }

        // GET: Section/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sp_GetSections_Result Section = db.GetSections(id,null,null).FirstOrDefault();
            if (Section == null)
            {
                return HttpNotFound();
            }
            return View(Section);
        }

        public ActionResult Change(int? id,int old_Order, int new_Order, int Module_id)
        {
            string error = "";
            sp_UpdateSectionOrder_Result id2 = new sp_UpdateSectionOrder_Result();
            try
            {
                id2 = db.UpdateSectionOrder(id, Module_id, UserId, old_Order, new_Order).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw;
            }

            return Json(new
            {
                success = "true",
                Section_id = id2.Section_id,
                error = error
            });
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
