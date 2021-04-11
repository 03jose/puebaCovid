using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pruebaCovid.Models
{
    public class Provinces
    {
        [Key]
        public int ProvinceId { get; set; }

        public int RegionId { get; set; }
        public string ProvName { get; set; }

        public int Cases { get; set; }
        public int Deaths { get; set; }
    }
}