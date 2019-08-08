using Maps.Data;
using Maps.Entities;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize(Roles = "User")]
    public class MapController : Controller
    {
        private const int PAGE_SIZE = 20;

        // GET: Maps
        public ActionResult Index(int? page)
        {
            try
            {
                var userId = User.Identity.GetUser().Id;
                using (var access = new DataAccess())
                {
                    var maps = access.Maps.Get(m => m.User.Id.Equals(userId));
                    return View(maps.ToList().ToPagedList(page ?? 1, PAGE_SIZE));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        // GET: Maps/Details/5
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
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        //GET: Maps/Edit/5
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
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        // POST: Maps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] EditMapViewModel model)
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
                            model.UpdateMap(ref map);
                            access.Maps.Update(map);
                            access.Save();
                            return RedirectToAction("Index");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        // GET: Maps/Delete/5
        public ActionResult Delete(Guid? id)
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
                    return View(map);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                using(var access = new DataAccess())
                {
                    access.Maps.Delete(id);
                    access.Save();
            return RedirectToAction("Index");
                }
            } catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }
    }
}
