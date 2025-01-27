﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maps.Entities
{
    /// <summary>
    /// Entity that represents one row in the Layer table.
    /// </summary>
    [Table("Layer")]

    public class Layer
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Icon { get; set; }

        /// <summary>
        /// Layer has a map it belongs to.
        /// </summary>
        public virtual Map Map { get; set; }

        public bool HasData { get; set; }

        public bool HasColumns { get; set; }

        public bool IsVisible { get; set; }

        /// <summary>
        /// Represents filtering based on marker position. This object has values for 
        /// latitude, longitude and radius.
        /// </summary>
        public string Center_
        {
            get { return Center != null ? JsonConvert.SerializeObject(Center) : null; }
            set { Center = string.IsNullOrEmpty(value) ? new JObject() : JsonConvert.DeserializeObject<JObject>(value); }
        }

        [NotMapped]
        public JObject Center { get; set; }

        /// <summary>
        /// If this filed is set filtering above is visible on the map.
        /// </summary>
        public bool IsFilterVisible { get; set; }

        /// <summary>
        /// Layer has a collection of its data.
        /// </summary>
        public ICollection<Data> Data { get; set; }

        /// <summary>
        /// Layer has a collection of its columns.
        /// </summary>
        public ICollection<Column> Columns { get; set; }

        public Layer()
        {
            Icon = "red-dot";
            IsVisible = true;
        }
    }
}