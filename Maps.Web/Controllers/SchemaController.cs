using Maps.Data;
using Maps.Entities;
using Maps.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Maps.Controllers
{
    [Authorize]
    public class SchemaController : Controller
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NumColumnsForm([Bind(Include = "LayerId,NumColumns")]FormSchemaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        Layer layer = access.Layers.Get(l => l.Id == model.LayerId, includeProperties: "Map").SingleOrDefault();
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.LayerId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                User.Identity.GetUser().Id, layer.Id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            return PartialView("Create", new CreateSchemaViewModel(model.LayerId, model.NumColumns));
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
        public ActionResult Create([Bind(Include = "LayerId,Columns")]CreateSchemaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var access = new DataAccess())
                    {
                        Layer layer = access.Layers.Get(l => l.Id == model.LayerId, includeProperties: "Map").SingleOrDefault();
                        if (layer == null)
                        {
                            logger.ErrorFormat("NOT_FOUND -- Layer with id={0} not found.", model.LayerId);

                            ModelState.AddModelError("", Error.NOT_FOUND);
                        }
                        else if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            logger.ErrorFormat("FORBIDDEN -- User with id={0} cannot access layer with id={1}.",
                                User.Identity.GetUser().Id, layer.Id);

                            ModelState.AddModelError("", Error.FORBIDDEN);
                        }
                        else
                        {
                            IList<string> messages = new List<string>();
                            if (!SchemaUtils.CheckColumns(model.Columns, ref messages))
                            {
                                logger.InfoFormat("UserId={0} -- check columns failed", User.Identity.GetUser().Id);

                                foreach (var message in messages)
                                {
                                    ModelState.AddModelError("", message);
                                }
                            }
                            else
                            {
                                IList<Column> columns = new List<Column>();
                                foreach (var column in model.Columns)
                                {
                                    columns.Add(new Column()
                                    {
                                        Id = Guid.NewGuid(),
                                        DataType = column.DataType,
                                        Name = column.Name,
                                        Layer = layer,
                                        HasChart = column.HasChart
                                    });
                                }

                                access.Columns.BulkInsert(columns);
                                layer.HasColumns = true;
                                access.Layers.Update(layer);
                                access.Save();
                                return PartialView("../Layer/LoadData", new LoadDataViewModel(layer.Id));
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
    }
}