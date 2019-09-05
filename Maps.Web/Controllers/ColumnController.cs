using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize]
    public class ColumnController : Controller
    {
        //[ChildActionOnly]
        //public ActionResult CreateSchema(Guid? id)
        //{
        //    try
        //    {
        //        if (id == null)
        //        {
        //            return PartialView("../Home/BadRequest");
        //        }
        //        using (var access = new DataAccess())
        //        {
        //            Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Columns").SingleOrDefault();
        //            if (layer == null)
        //            {
        //                return PartialView("../Home/NotFound");
        //            }
        //            if (layer.Map.User.Id != User.Identity.GetUser().Id)
        //            {
        //                return PartialView("../Home/Forbidden");
        //            }
        //            return PartialView(new CreateSchemaViewModel(layer));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex);
        //    }
        //    return PartialView();
        //}
        //public ActionResult AddColumn(CreateSchemaViewModel model)
        //{
        //    model.Columns.Add(new CreateColumnViewModel());
        //    return View("CreateSchema", model);
        //}

        //public ActionResult Delete(CreateSchemaViewModel model)
        //{
        //    return View("CreateSchema", model);
        //}
    }
}