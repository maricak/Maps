﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Maps.Models;
using Microsoft.AspNet.Identity;

namespace Maps.Controllers
{
    [Authorize(Roles = "User")]
    public class MapsController : Controller
    {
        //private MapsDbContext db = new MapsDbContext();

        // GET: Maps
        public ActionResult Index()
        {
            var userId = User.Identity.GetUser().Id;
            using (var db = new MapsDbContext())
            {
                return View(db.Maps.Where(m => m.User.Id.Equals(userId)).ToList());
            }
        }

        // GET: Maps/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new MapsDbContext())
            {
                Map map = db.Maps.Find(id);
                if (map == null)
                {
                    return HttpNotFound();
                }
                return View(map);
            }
        }

        // GET: Maps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] CreateMapViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MapsDbContext())
                    {
                        var userId = User.Identity.GetUserId();
                        var user = db.Users.Find(userId);
                        Map map = new Map
                        {
                            Id = Guid.NewGuid(),
                            CreationTime = DateTime.UtcNow,
                            User = user,
                        };
                        db.Maps.Add(map);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);   
                }
            }
            return View(model);
        }

        // GET: Maps/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Map map = db.Maps.Find(id);
        //    if (map == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(map);
        //}

        //// POST: Maps/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name")] Map map)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(map).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(map);
        //}

        //// GET: Maps/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Map map = db.Maps.Find(id);
        //    if (map == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(map);
        //}

        //// POST: Maps/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Map map = db.Maps.Find(id);
        //    db.Maps.Remove(map);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
