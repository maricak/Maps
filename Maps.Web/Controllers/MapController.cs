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
    [Authorize]
    public class MapController : Controller
    {
        private const int PAGE_SIZE = 10;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(int? page)
        {
            logger.InfoFormat("page={0}", page);
            try
            {
                var userId = User.Identity.GetUser().Id;
                using (var access = new DataAccess())
                {
                    var maps = access.Maps.Get(m => m.User.Id.Equals(userId), orderBy: m => m.OrderByDescending(i => i.CreationTime));
                    List<ListItemMapViewModel> models = new List<ListItemMapViewModel>();
                    foreach (var map in maps.ToList())
                    {
                        models.Add(new ListItemMapViewModel(map));
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
        public ActionResult ListItem(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("../Home/BadRequest");
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return PartialView("../Home/NotFound");
                    }
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return PartialView("../Home/Forbidden");
                    }
                    return PartialView(new ListItemMapViewModel(map));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView();
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
                    Map map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers").SingleOrDefault();
                    if (map == null)
                    {
                        return HttpNotFound();
                    }
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    return View(new DetailsMapViewModel(map));
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
                            return PartialView("AddMap", new ListItemMapViewModel(map));
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
                    return PartialView("../Home/BadRequest");
                }
                using (var access = new DataAccess())
                {
                    Map map = access.Maps.GetByID(id);
                    if (map == null)
                    {
                        return PartialView("../Home/NotFound");
                    }
                    if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return PartialView("../Home/Forbidden");
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
                            return PartialView("../Home/Forbidden");
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
                            return PartialView("ListItem", new ListItemMapViewModel(map));
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

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "Id")] DetailsMapViewModel model)
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
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            return PartialView("../Home/Forbidden");
                        }
                        else
                        {
                            access.Maps.Delete(model.Id);
                            access.Save();
                            return new EmptyResult();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView("ListItem", model);
        }

        public ActionResult DisplayMap(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("../Home/BadRequest");
                }
                using (var access = new DataAccess())
                {
                    var map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers,Layers.Data,Layers.Columns").SingleOrDefault();
                    if (map == null)
                    {
                        ModelState.AddModelError("", "Map does not exists.");
                    }
                    else if (map.User.Id != User.Identity.GetUser().Id && !map.IsPublic)
                    {
                        return PartialView("../Home/Forbidden");
                    }
                    else
                    {
                        return PartialView(new DisplayMapViewModel(map));
                    }
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
        public ActionResult SetPublic([Bind(Include = "IsPublic,Id")]PublicMapViewModel model)
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
                            return PartialView("../Home/NotFound");
                        }
                        map.IsPublic = model.IsPublic;
                        access.Maps.Update(map);
                        access.Save();
                        return PartialView(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult View(Guid? id)
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
                    if (!map.IsPublic)
                    {
                        return HttpNotFound();
                    }
                    return View(new ViewMapViewModel(map));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult Copy([Bind(Include = "Id")]CopyMapViewModel model)
        {
            var newMapId = Guid.NewGuid();
            try
            {
                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.Get(m => m.Id == model.Id, includeProperties: "Layers,Layers.Columns,Layers.Data").SingleOrDefault();
                        if (map == null)
                        {
                            return PartialView("../Home/NotFound");
                        }
                        var userId = User.Identity.GetUser().Id;
                        var user = access.Users.GetByID(userId);
                        if (user == null)
                        {
                            return PartialView("../Home/BadRequest");
                        }
                        Map newMap = new Map()
                        {
                            Id = newMapId,
                            CreationTime = DateTime.UtcNow,
                            Name = string.Concat("map", newMapId),
                            User = user,
                        };
                        access.Maps.Insert(newMap);
                        foreach (var layer in map.Layers)
                        {
                            Layer newLayer = new Layer()
                            {
                                Id = Guid.NewGuid(),
                                Map = newMap,
                                HasColumns = layer.HasColumns,
                                HasData = layer.HasData,
                                IsVisible = layer.IsVisible,
                                Name = layer.Name,
                                Icon = layer.Icon
                            };
                            access.Layers.Insert(newLayer);
                            foreach (var column in layer.Columns)
                            {
                                Column newColumn = new Column()
                                {
                                    Id = Guid.NewGuid(),
                                    DataType = column.DataType,
                                    HasChart = column.HasChart,
                                    Layer = newLayer,
                                    Name = column.Name
                                };
                                access.Columns.Insert(newColumn);
                            }
                            access.Save();
                            var dataList = new List<Entities.Data>();
                            foreach (var data in layer.Data)
                            {
                                Entities.Data newData = new Entities.Data()
                                {
                                    Id = Guid.NewGuid(),
                                    Layer = newLayer,
                                    Values = data.Values
                                };
                                dataList.Add(newData);
                            }
                            access.Data.BulkInsert(dataList);
                            access.Save();
                        }
                        ModelState.AddModelError("", "Successful copy!");
                        return PartialView();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                try
                {
                    using (var access = new DataAccess())
                    {
                        access.Maps.Delete(newMapId);
                    }
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc);
                }
            }
            return PartialView();
        }
    }
}
