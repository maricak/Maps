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
        [ValidateAntiForgeryToken]
        public ActionResult Range(RangeFilterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                    using (var access = new DataAccess())
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
                            column.Filter["min"] = model.RangeStart;
                            column.Filter["max"] = model.RangeEnd;

                            access.Save();

                            logger.InfoFormat("UserId={0} -- successful RANGE/UNIQUE_LIST update", User.Identity.GetUser().Id);

                            ModelState.AddModelError("", "Successful filter update!");

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
        [ValidateAntiForgeryToken]
        public ActionResult Distance(DistanceFilterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                    using (var access = new DataAccess())
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
                            layer.Center["lat"] = model.Latitude;
                            layer.Center["lng"] = model.Longitude;
                            layer.Center["radius"] = model.Radius;
                            access.Layers.Update(layer);
                            access.Save();

                            logger.InfoFormat("UserId={0} -- successful DISTANCE update", User.Identity.GetUser().Id);

                            ModelState.AddModelError("", "Successful filter update!");

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
        [ValidateAntiForgeryToken]
        public ActionResult UniqueList(UniqueListFilterViewModel model)
        {
            try
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                // ModelState is not compatible with Dictionary and contains erros 'A value is required'.
                // To avoid there is no check ModelState.IsValid

                using (var access = new DataAccess())
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

                        foreach (var pair in model.UniqueValues)
                        {
                            column.Filter[pair.Key] = pair.Value;
                        }
                        access.Save();

                        logger.InfoFormat("UserId={0} -- successful RANGE/UNIQUE_LIST update", User.Identity.GetUser().Id);

                        ModelState.AddModelError("", "Successful filter update!");

                        return PartialView(model);
                    }
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