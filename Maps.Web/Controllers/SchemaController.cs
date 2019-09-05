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
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisplayForm([Bind(Include = "LayerId,NumColumns")]FormSchemaViewModel model)
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
                            return PartialView("../Home/NotFound");
                        }
                        if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            return PartialView("../Home/Forbidden");
                        }
                        return PartialView("Create", new CreateSchemaViewModel(model.LayerId, model.NumColumns));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
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
                            return PartialView("../Home/NotFound");
                        }
                        if (layer.Map.User.Id != User.Identity.GetUser().Id)
                        {
                            return PartialView("../Home/Forbidden");
                        }
                        IList<string> messages = new List<string>();
                        if (!SchemaUtils.CheckColumns(model.Columns, ref messages))
                        {
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
            }
            return PartialView(model);
        }
    }
}