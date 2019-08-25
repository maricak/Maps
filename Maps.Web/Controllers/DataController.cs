using Maps.Data;
using Maps.Entities;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Maps.Controllers
{
    public class DataController : Controller
    {
        [AjaxOnly]
        public ActionResult Table(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("../Home/BadRequest");
                }
                using (var access = new DataAccess())
                {
                    Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Data").SingleOrDefault();
                    if (layer == null)
                    {
                        return PartialView("../Home/NotFound");
                    }
                    if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return PartialView("../Home/Forbidden");
                    }
                    var model = new DetailsLayerViewModel(layer);
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
        public ActionResult Chart(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("../Home/BadRequest");
                }
                using (var access = new DataAccess())
                {
                    Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Data").SingleOrDefault();
                    if (layer == null)
                    {
                        return PartialView("../Home/NotFound");
                    }
                    if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return PartialView("../Home/Forbidden");
                    }
                    var model = new DetailsLayerViewModel(layer);
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView();
        }
    }
}