using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maps.Data;
using Maps.Entities;

namespace Maps.Controllers
{
    public class LayerController : Controller
    {
        private MapsDbContext db = new MapsDbContext();

        // GET: Layer
        public ActionResult Index(Guid? id)
        {
            return View(db.Layers.ToList());
        }

        // GET: Layer/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layer layer = db.Layers.Find(id);
            if (layer == null)
            {
                return HttpNotFound();
            }
            return View(layer);
        }

        // GET: Layer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Layer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Layer layer)
        {
            if (ModelState.IsValid)
            {
                layer.Id = Guid.NewGuid();
                db.Layers.Add(layer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(layer);
        }

        // GET: Layer/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layer layer = db.Layers.Find(id);
            if (layer == null)
            {
                return HttpNotFound();
            }
            return View(layer);
        }

        // POST: Layer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Layer layer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(layer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(layer);
        }

        // GET: Layer/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layer layer = db.Layers.Find(id);
            if (layer == null)
            {
                return HttpNotFound();
            }
            return View(layer);
        }

        // POST: Layer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Layer layer = db.Layers.Find(id);
            db.Layers.Remove(layer);
            db.SaveChanges();
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
