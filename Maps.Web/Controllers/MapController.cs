using Maps.Data;
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
        private const int PAGE_SIZE = 10;

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(int? page)
        {
            try
            {
                var userId = User.Identity.GetUser().Id;
                using (var access = new DataAccess())
                {
                    var maps = access.Maps.Get(m => m.User.Id.Equals(userId), orderBy: m => m.OrderByDescending(i => i.CreationTime));
                    List<DetailsMapViewModel> models = new List<DetailsMapViewModel>();
                    foreach (var map in maps.ToList())
                    {
                        models.Add(new DetailsMapViewModel(map));
                    }
                    return View(models.ToPagedList(page ?? 1, PAGE_SIZE));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return View();
        }

        [AjaxOnly]
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
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    return PartialView(new DetailsMapViewModel(map));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
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
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
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
                    Map map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers").SingleOrDefault();
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    var model = new DetailsMapViewModel(map);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return View();
        }

        [AjaxOnly]
        public ActionResult AddMap()
        {
            return PartialView(null);
        }

        [AjaxOnly]
        public ActionResult Create()
        {
            return PartialView();
        }

        [AjaxOnly]
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

                        if (user == null)
                        {
                            ModelState.AddModelError("", "Unknown user.");
                            return PartialView(model);
                        }

                        Map map = new Map
                        {
                            Name = model.Name,
                            Id = Guid.NewGuid(),
                            CreationTime = DateTime.UtcNow,
                            User = user,
                        };

                        var duplicateMap = access.Maps.Get(m => m.Name.Equals(map.Name) && m.User.Id == userId);
                        if (duplicateMap.Count() != 0)
                        {
                            ModelState.AddModelError("", "Map with name '" + map.Name + "' already exists.");
                        }
                        else
                        {
                            access.Maps.Insert(map);
                            access.Save();
                            return PartialView("AddMap", new DetailsMapViewModel(map));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex);
                }
            }
            return PartialView(model);
        }

        [AjaxOnly]
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
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    var model = new EditMapViewModel(map);
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView();
        }

        [AjaxOnly]
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
                        var userId = User.Identity.GetUser().Id;
                        var map = access.Maps.GetByID(model.Id);
                        if (map == null)
                        {
                            ModelState.AddModelError("", "Map does not exists.");
                        }
                        else if (map.User.Id != userId)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                        else
                        {
                            var sameNameMap = access.Maps.Get(m => m.Name.Equals(model.Name) && m.User.Id == userId).SingleOrDefault();
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
                ModelState.AddModelError("", ex);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Delete(DetailsMapViewModel model)
        {
            try
            {
                if (model == null || model.Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    var map = access.Maps.GetByID(model.Id);
                    if (map == null)
                    {
                        ModelState.AddModelError("", "Map does not exists.");
                    }
                    else if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        access.Maps.Delete(model.Id);
                        access.Save();
                        return new EmptyResult();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView("Details", model);
        }
    }
}
