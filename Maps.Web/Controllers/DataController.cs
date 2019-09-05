using Maps.Data;
using Maps.Entities;
using Maps.Utils;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AjaxOnly]
        public ActionResult Table(Guid? id)
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
                        Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Data").SingleOrDefault();
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
                            return PartialView(new TableDataViewModel(layer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new TableDataViewModel());
        }

        [AjaxOnly]
        public ActionResult Chart(Guid? id)
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
                        Layer layer = access.Layers.Get(l => l.Id == id, includeProperties: "Map,Data,Columns").SingleOrDefault();
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
                            return PartialView(new ChartDataViewModel(layer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("", ex);
                ModelState.AddModelError("", Error.ERROR);
            }

            return PartialView(new ChartDataViewModel());
        }
    }
}