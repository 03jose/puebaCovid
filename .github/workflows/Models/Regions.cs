using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pruebaCovid.Models
{
    public class Regions
    {
        [Key]
        public int RegionId { get; set; }
        public string Name { get; set; }
    }
}