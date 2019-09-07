using Maps.Data;
using Maps.Entities;
using Maps.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize]

    public class LayerController : Controller
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AjaxOnly]
        public ActionResult AddLayer(Guid? mapId)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- mapId={1}", User.Identity.GetUser().Id, mapId);

                if (mapId == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- mapId is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(mapId);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", mapId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                                      User.Identity.GetUser().Id, mapId);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new DetailsLayerViewModel(mapId.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return View(new DetailsLayerViewModel());
        }

        [AjaxOnly]
        public ActionResult Create(Guid? mapId)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- mapId={1}", User.Identity.GetUser().Id, mapId);

                if (mapId == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- mapId is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(mapId);
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", mapId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                                      User.Identity.GetUser().Id, mapId);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new CreateLayerViewModel(mapId.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return View(new CreateLayerViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MapId,Name")] CreateLayerViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    var mapId = model.MapId;
                    if (mapId == null)
                    {
                        logger.ErrorFormat("BAD_REQUEST -- mapId is null.");

                        ModelState.AddModelError("", Error.BAD_REQUEST);
                    }
                    else
                    {
                        using (var access = new DataAccess())
                        {
                            var map = access.Maps.GetByID(mapId);
                            if (map == null)
                            {
                                logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", mapId);

                                ModelState.AddModelError("", Error.NOT_FOUND);
                            }
                            else if (map.User.Id != User.Identity.GetUser().Id)
                            {
                                logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                                     User.Identity.GetUser().Id, mapId);

                                ModelState.AddModelError("", Error.FORBIDDEN);
                            }
                            else
                            {
                                Layer layer = new Layer
                                {
                                    Name = model.Name,
                                    Id = Guid.NewGuid(),
                                    Map = map
                                };
                                var duplicateLayer = access.Layers.Get(l => l.Name.Equals(layer.Name) && l.Map.Id == mapId);
                                if (duplicateLayer.Count() != 0)
                                {
                                    logger.ErrorFormat("UserId={0} -- Duplicate layer name", User.Identity.GetUser().Id);

                                    ModelState.AddModelError("", string.Format("Layer with name '{0}' already exists.", layer.Name));
                                }
                                else
                                {
                                    access.Layers.Insert(layer);
                                    access.Save();
                                    return PartialView("AddLayer", new DetailsLayerViewModel(layer));
                                }
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
        public ActionResult Details(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- mapId={1}", User.Identity.GetUser().Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Layer layer = access.Layers.GetByID(id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                     User.Identity.GetUser().Id, id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new DetailsLayerViewModel(layer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new DetailsLayerViewModel());
        }

        [AjaxOnly]
        public ActionResult Edit(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0} -- mapId={1}", User.Identity.GetUser().Id, id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        Layer layer = access.Layers.GetByID(id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                     User.Identity.GetUser().Id, id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView(new EditLayerViewModel(layer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView();
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] EditLayerViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.GetByID(model.Id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                User.Identity.GetUser().Id, model.Id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            var sameNameLayer = access.Layers.Get(l => l.Name.Equals(model.Name) && l.Map.Id == layer.Map.Id).SingleOrDefault();
                            if (sameNameLayer != null && !sameNameLayer.Id.Equals(layer.Id))
                            {
                                logger.ErrorFormat("UserId={0} -- Duplicate layer name", User.Identity.GetUser().Id);

                                ModelState.AddModelError("", string.Format("Layer with name '{0}' already exists.", layer.Name));
                            }
                            else
                            {
                                layer.Name = model.Name;
                                access.Layers.Update(layer);
                                access.Save();
                                return PartialView("Details", new DetailsLayerViewModel(layer));
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
        public ActionResult Delete([Bind(Include = "Id,Name")] DetailsHeaderLayerViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.GetByID(model.Id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                User.Identity.GetUser().Id, model.Id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            access.Layers.Delete(model.Id);
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

            return PartialView("Details", model);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadData([Bind(Include = "LayerId,DataFile")]LoadDataViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.Get(l => l.Id == model.LayerId, includeProperties: "Map,Columns").SingleOrDefault();
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.LayerId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                 User.Identity.GetUser().Id, model.LayerId);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else if (layer.HasData)
                        {
                            logger.ErrorFormat("UserId={0} -- Layer with id={1} already has data",
                                User.Identity.GetUser().Id, model.LayerId);

                            ModelState.AddModelError("", string.Format("Layer with id={0} already has data", model.LayerId));
                        }
                        else
                        {
                            var extension = Path.GetExtension(model.DataFile.FileName);
                            if (extension == ".json")
                            {
                                IList<string> messages = new List<string>();
                                JsonDataReader reader = new JsonDataReader();
                                if (!reader.LoadFile(model.DataFile.InputStream, layer, ref messages))
                                {
                                    foreach (var message in messages)
                                    {
                                        logger.ErrorFormat("USerId={0} -- Error message={1}", User.Identity.GetUser().Id, message);

                                        ModelState.AddModelError("", message);
                                    }
                                }
                                else
                                {
                                    layer.HasData = true;
                                    layer.Center["lng"] = 0;
                                    layer.Center["lat"] = 0;
                                    layer.Center["radius"] = 0;

                                    access.Layers.Update(layer);
                                    access.Save();

                                    logger.InfoFormat("UserId={0} -- successful data load.", User.Identity.GetUser().Id);

                                    ModelState.AddModelError("", "Successful data load!");

                                    return RedirectToAction("Filter", new { id = model.LayerId });
                                }
                            }
                            else
                            {
                                logger.ErrorFormat("UserId={0} -- Extension {1} is not supported", User.Identity.GetUser().Id, extension);

                                ModelState.AddModelError("", string.Format("Extension '{0}' is not supported", extension));
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
        public ActionResult TableDropdown(Guid? mapId)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (mapId == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- mapId is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.Get(m => m.Id == mapId, includeProperties: "Layers").SingleOrDefault();
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", mapId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                            User.Identity.GetUser().Id, mapId);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            IList<DropdownItemLayerViewModel> items = new List<DropdownItemLayerViewModel>();
                            foreach (var layer in map.Layers)
                            {
                                items.Add(new DropdownItemLayerViewModel(layer));
                            }

                            return PartialView(items);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new List<DropdownItemLayerViewModel>());
        }

        [AjaxOnly]
        public ActionResult ChartDropdown(Guid? mapId)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (mapId == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- mapId is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.Get(m => m.Id == mapId, includeProperties: "Layers").SingleOrDefault();
                        if (map == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Map with id={0} not found.", mapId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access map with id={1}.",
                            User.Identity.GetUser().Id, mapId);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            IList<DropdownItemLayerViewModel> items = new List<DropdownItemLayerViewModel>();
                            foreach (var layer in map.Layers)
                            {
                                items.Add(new DropdownItemLayerViewModel(layer));
                            }
                            return PartialView(items);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new List<DropdownItemLayerViewModel>());
        }

        public ActionResult Filter(Guid? id)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (id == null)
                {
                    logger.ErrorFormat("BAD_REQUEST -- id is null.");

                    ModelState.AddModelError("", Error.BAD_REQUEST);
                }
                else
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.Get(l => l.Id == id, includeProperties: "Columns").SingleOrDefault();
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            return PartialView(new FilterLayerViewModel(layer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new FilterLayerViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult SelectIcon([Bind(Include = "Icon,Id")]SelectIconLayerViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.GetByID(model.Id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            layer.Icon = model.Icon;
                            access.Layers.Update(layer);
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

        [AjaxOnly]
        [HttpPost]
        public ActionResult SetVisibility([Bind(Include = "IsVisible,Id")]VisibilityLayerViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.GetByID(model.Id);
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.Id);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else
                        {
                            layer.IsVisible = model.IsVisible;
                            access.Layers.Update(layer);
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
    }
}