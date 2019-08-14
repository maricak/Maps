﻿using Maps.Data;
using Maps.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize(Roles = "User")]
    public class MapController : Controller
    {
        private const int PAGE_SIZE = 20;

        public ActionResult Index(int? page)
        {
            try
            {
                var userId = User.Identity.GetUser().Id;
                using (var access = new DataAccess())
                {
                    var maps = access.Maps.Get(m => m.User.Id.Equals(userId), orderBy: m => m.OrderByDescending(i => i.CreationTime));
                    List<DetailsMapViewModel> models = new List<DetailsMapViewModel>();
                    foreach(var map in maps.ToList())
                    {
                        models.Add(new DetailsMapViewModel(map));                      
                    }
                    return View(models.ToPagedList(page ?? 1, PAGE_SIZE));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    return PartialView(new DetailsMapViewModel(map));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView();
        }

        public ActionResult MapDetails(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult SidebarPartial(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    var model = new DetailsMapViewModel(map);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult CreateButton()
        {
            return PartialView(null);
        }

        // GET: Maps/Create
        public ActionResult Create()
        {
            return PartialView();
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
                    using (var access = new DataAccess())
                    {
                        var userId = User.Identity.GetUser().Id;
                        var user = access.Users.GetByID(userId);

                        Map map = new Map
                        {
                            Name = model.Name,
                            Id = Guid.NewGuid(),
                            CreationTime = DateTime.UtcNow,
                            User = user,
                        };
                        var duplicateMap = access.Maps.Get(m => m.Name.Equals(map.Name));
                        if (duplicateMap.Count() != 0)
                        {
                            ModelState.AddModelError("", "Map with name '" + map.Name + "' already exists.");
                        }
                        else
                        {                            
                            access.Maps.Insert(map);
                            access.Save();
                            return PartialView("CreateButton", new DetailsMapViewModel(map));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return PartialView(model);
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    var model = new EditMapViewModel(map);
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreationTime")] EditMapViewModel model)
        {          
            try
            {
                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(model.Id);
                        if (map == null)
                        {
                            ModelState.AddModelError("", "Map does not exists.");
                        }
                        else
                        {
                            var sameNameMap = access.Maps.Get(m => m.Name.Equals(model.Name)).SingleOrDefault();
                            if (sameNameMap != null && !sameNameMap.Id.Equals(map.Id))
                            {
                                ModelState.AddModelError("", "Map with name '" + model.Name + "' already exists.");
                                return PartialView("Edit", model);
                            }
                            map.Name = model.Name;
                            access.Maps.Update(map);
                            access.Save();
                            return PartialView("Details", new DetailsMapViewModel(map));
                        }
                    }
                }
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DetailsMapViewModel model)
        {
            try
            {
                if(model == null || model.Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    access.Maps.Delete(model.Id);
                    access.Save();
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView("Details", model);
        }
    }
}
