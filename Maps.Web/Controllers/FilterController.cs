using Maps.Data;
using Maps.Entities;
using Maps.Utils;
using System;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize]
    public class FilterController : Controller
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AjaxOnly]
        [HttpPost]
        public ActionResult Update([Bind(Include = "IsVisible,Id,Name")]FilterViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        if (model.Type == FilterType.DISTANCE)
                        {
                            var layer = access.Layers.GetByID(model.Id);
                            if (layer == null)
                            {
                                logger.ErrorFormat("NOT_FOUND -- Column with id={0} not found.", model.Id);

                                ModelState.AddModelError("", Error.NOT_FOUND);
                            }
                            else
                            {
                                layer.IsFilterVisible = model.IsVisible;
                                access.Layers.Update(layer);
                                access.Save();

                                return PartialView("Filter", model);
                            }
                        }
                        else
                        {
                            var column = access.Columns.GetByID(model.Id);
                            if (column == null)
                            {
                                logger.ErrorFormat("NOT_FOUND -- Column with id={0} not found.", model.Id);

                                ModelState.AddModelError("", Error.NOT_FOUND);
                            }
                            else
                            {
                                column.IsFilterVisible = model.IsVisible;
                                access.Columns.Update(column);
                                access.Save();
                                ModelState.Clear();

                                return PartialView("Filter", model);
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

            return PartialView("Filter", model);
        }
    }
}