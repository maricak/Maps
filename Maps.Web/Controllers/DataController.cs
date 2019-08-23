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
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (var access = new DataAccess())
                {
                    Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Data").SingleOrDefault();
                    if (layer == null)
                    {
                        return HttpNotFound();
                    }
                    if (layer.Map.User.Id != User.Identity.GetUser().Id)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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