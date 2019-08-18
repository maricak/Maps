using Maps.Data;
using Maps.Entities;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Maps.Controllers
{
    public class LayerController : Controller
    {
        [AjaxOnly]
        public ActionResult AddLayer(Guid mapId)
        {
            try
            {
                if (mapId == null)
                {
                    ModelState.AddModelError("", "Map id cannot be empty.");
                    return PartialView();
                }
                using (var access = new DataAccess())
                {
                    var map = access.Maps.GetByID(mapId);
                    if (map == null)
                    {
                        ModelState.AddModelError("", "Map does not exists.");
                        return PartialView();
                    }
                    else if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView(new DetailsLayerViewModel(mapId));
        }

        [AjaxOnly]
        public ActionResult Create(Guid mapId)
        {
            try
            {
                if (mapId == null)
                {
                    ModelState.AddModelError("", "Map id cannot be empty.");
                    return PartialView();
                }
                using (var access = new DataAccess())
                {
                    var map = access.Maps.GetByID(mapId);
                    if (map == null)
                    {
                        ModelState.AddModelError("", "Map does not exists.");
                        return PartialView();
                    }
                    else if (map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView(new CreateLayerViewModel(mapId));
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MapId,Name")] CreateLayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mapId = model.MapId;
                    if (mapId == null)
                    {
                        ModelState.AddModelError("", "Map id cannot be empty.");
                        return PartialView(model);
                    }
                    using (var access = new DataAccess())
                    {
                        var map = access.Maps.GetByID(mapId);
                        if (map == null)
                        {
                            ModelState.AddModelError("", "Map does not exists.");
                            return PartialView();
                        }
                        else if (map.User.Id != User.Identity.GetUser().Id)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                        Layer layer = new Layer
                        {
                            Name = model.Name,
                            Id = Guid.NewGuid(),
                            Map = map
                        };
                        var duplicateLayer = access.Layers.Get(l => l.Name.Equals(layer.Name) && l.Map.Id == mapId);
                        if (duplicateLayer.Count() != 0)
                        {
                            ModelState.AddModelError("", "Layer with name '" + layer.Name + "' already exists.");
                        }
                        else
                        {
                            access.Layers.Insert(layer);
                            access.Save();
                            return PartialView("AddLayer", new DetailsLayerViewModel(layer));
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
                    Layer layer = access.Layers.GetByID(id);
                    if (layer == null)
                    {
                        return HttpNotFound();
                    }
                    else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    return PartialView(new DetailsLayerViewModel(layer));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return PartialView();
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
                    Layer layer = access.Layers.GetByID(id);
                    if (layer == null)
                    {
                        return HttpNotFound();
                    }
                    else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    var model = new EditLayerViewModel(layer);
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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
                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        var layer = access.Layers.GetByID(model.Id);
                        if (layer == null)
                        {
                            ModelState.AddModelError("", "Layer does not exists.");
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                        else
                        {
                            var sameNameLayer = access.Layers.Get(l => l.Name.Equals(model.Name) && l.Map.Id == layer.Map.Id).SingleOrDefault();
                            if (sameNameLayer != null && !sameNameLayer.Id.Equals(layer.Id))
                            {
                                ModelState.AddModelError("", "Layer with name '" + model.Name + "' already exists.");
                                return PartialView(model);
                            }
                            layer.Name = model.Name;
                            access.Layers.Update(layer);
                            access.Save();
                            return PartialView("Details", new DetailsLayerViewModel(layer));
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
        [AjaxOnly]
        public ActionResult Delete(DetailsLayerViewModel model)
        {
            try
            {
                if (model == null || model.Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    var layer = access.Layers.GetByID(model.Id);
                    if (layer == null)
                    {
                        ModelState.AddModelError("", "Layer does not exists.");
                    }
                    else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        access.Layers.Delete(model.Id);
                        access.Save();
                        return new EmptyResult();
                    }
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
