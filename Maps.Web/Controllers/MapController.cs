using Maps.Data;
using Maps.Entities;
using Maps.Utils;
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
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int PAGE_SIZE = 10;

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(int? page)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- page={1}", User.Identity.GetUser().Id, page);

                var userId = User.Identity.GetUser().Id;
                using (var access = new DataAccess())
                {
                    var maps = access.Maps.Get(m => m.User.Id.Equals(userId), orderBy: m => m.OrderByDescending(i => i.CreationTime)).ToList();
                    List<ListItemMapViewModel> models = new List<ListItemMapViewModel>();
                    foreach (var map in maps)
                    {
                        models.Add(new ListItemMapViewModel(map));
                    }

                    return View(models.ToPagedList(page ?? 1, PAGE_SIZE));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return View(new List<ListItemMapViewModel>());
        }

        [AjaxOnly]
        public ActionResult ListItem(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- id={1}", User.Identity.GetUser().Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Map map = access.Maps.GetByID(id);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                               User.Identity.GetUser().Id, id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new ListItemMapViewModel(map));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new ListItemMapViewModel());
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- id={1}", User.Identity.GetUser().Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Map map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers").SingleOrDefault();
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", id);

                            return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.", User.Identity.GetUser().Id, id);

                            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                        else
                        {
                            return View(new DetailsMapViewModel(map));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return View(new DetailsMapViewModel());
        }

        [AjaxOnly]
        public ActionResult AddMap()
        {
            logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

            return PartialView(null);
        }

        [AjaxOnly]
        public ActionResult Create()
        {
            logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

            return PartialView();
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] CreateMapViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var userId = User.Identity.GetUser().Id;
                        var user = access.Users.GetByID(userId);

                        if (user == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- User with id={0} not found.", userId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
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
                                logger.ErrorFormat("UserId={0} -- Duplicate map name", User.Identity.GetUser().Id);

                                ModelState.AddModelError("", string.Format("Map with name '{0}' already exists.", map.Name));
                            }
                            else
                            {
                                access.Maps.Insert(map);
                                access.Save();
                                return PartialView("AddMap", new ListItemMapViewModel(map));
                            }
                        }
                    }
                }
                else
                {
                    logger.ErrorFormat("UserId={0} -- Model state is invalid", User.Identity.GetUser().Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(model);
        }

        [AjaxOnly]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- id={1}", User.Identity.GetUser().Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Map map = access.Maps.GetByID(id);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                               User.Identity.GetUser().Id, id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new EditMapViewModel(map));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new EditMapViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreationTime")] EditMapViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var userId = User.Identity.GetUser().Id;
                        var map = access.Maps.GetByID(model.Id);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != userId)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                                User.Identity.GetUser().Id, model.Id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            var sameNameMap = access.Maps.Get(m => m.Name.Equals(model.Name) && m.User.Id == userId).SingleOrDefault();
                            if (sameNameMap != null && !sameNameMap.Id.Equals(map.Id))
                            {
                                logger.ErrorFormat("UserId={0} -- Duplicate map name", User.Identity.GetUser().Id);

                                ModelState.AddModelError("", string.Format("Map with name '{0}' already exists.", map.Name));
                            }
                            else
                            {
                                map.Name = model.Name;
                                access.Maps.Update(map);
                                access.Save();
                                return PartialView("ListItem", new ListItemMapViewModel(map));
                            }
                        }
                    }
                }
                else
                {
                    logger.ErrorFormat("UserId={0} -- Model state is invalid", User.Identity.GetUser().Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
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
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(model.Id);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("UserId={0} -- Duplicate map name", User.Identity.GetUser().Id);

                            ModelState.AddModelError("", string.Format("Map with name '{0}' already exists.", map.Name));
                        }
                        else
                        {
                            access.Maps.Delete(model.Id);
                            access.Save();
                            return new EmptyResult();
                        }
                    }
                }
                else
                {
                    logger.ErrorFormat("UserId={0} -- Model state is invalid", User.Identity.GetUser().Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView("ListItem", model);
        }

        [AllowAnonymous]
        public ActionResult DisplayMap(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser()?.Id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers,Layers.Data,Layers.Columns").SingleOrDefault();
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (!User.Identity.IsAuthenticated && !map.IsPublic)
                        {
                            logger.ErrorFormat("FORBIDDEN -- NOT AUTHENTICATED user cannot access map with id={1}.", id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else if (User.Identity.IsAuthenticated && map.User.Id != User.Identity.GetUser().Id && !map.IsPublic)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                                    User.Identity.GetUser().Id, id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new DisplayMapViewModel(map));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new DisplayMapViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult SetPublic([Bind(Include = "IsPublic,Id")]PublicMapViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(model.Id);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            map.IsPublic = model.IsPublic;
                            access.Maps.Update(map);
                            access.Save();
                            return PartialView(model);
                        }
                    }
                }
                else
                {
                    logger.ErrorFormat("UserId={0} -- Model state is invalid", User.Identity.GetUser().Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult View(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- id={1}", User.Identity.GetUser()?.Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Map map = access.Maps.Get(m => m.Id == id, includeProperties: "Layers").SingleOrDefault();
                        if (map == null || !map.IsPublic)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            return View(new ViewMapViewModel(map));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return View(new ViewMapViewModel());
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
                        if (map == null || !map.IsPublic)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            var userId = User.Identity.GetUser().Id;
                            var user = access.Users.GetByID(userId);
                            if (user == null)
                            {
                                logger.ErrorFormat("NOT_FOUND -- User with id={0} not found.", userId);

                                ModelState.AddModelError("", Error.NOT_FOUND);
                            }
                            else
                            {
                                Map newMap = new Map()
                                {
                                    Id = newMapId,
                                    CreationTime = DateTime.UtcNow,
                                    Name = string.Concat("map", newMapId),
                                    User = user,
                                };
                                access.Maps.Insert(newMap);

                                logger.InfoFormat("UserId={0} -- copy map object from map with id={1} to map with id={2}",
                                    User.Identity.GetUser().Id, map.Id, newMapId); ;

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

                                    logger.InfoFormat("UserId={0} -- copy layer with id={1} with columns from map with id={2} to map with id={3}",
                                        User.Identity.GetUser().Id, layer.Id, map.Id, newMapId);

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

                                    logger.InfoFormat("UserId={0} -- copy data from layer with id={1} to layer with id={2}",
                                        User.Identity.GetUser().Id, layer.Id, newLayer.Id);
                                }

                                ModelState.AddModelError("", "Successful copy!");
                                return PartialView(model);
                            }
                        }
                    }
                }
                else
                {
                    logger.ErrorFormat("UserId={0} -- Model state is invalid", User.Identity.GetUser().Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
                try
                {
                    using (var access = new DataAccess())
                    {
                        logger.InfoFormat("UserId={0} -- Delete new map with id={1} after failure", newMapId);

                        access.Maps.Delete(newMapId);
                    }
                }
                catch (Exception exc)
                {
                    logger.Fatal("", exc);
                    ModelState.AddModelError("", Error.ERROR);
                }
            }

            return PartialView(model);
        }
    }
}
