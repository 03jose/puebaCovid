using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pruebaCovid.Models.ViewModel
{
    public class VMInformation
    {
       public List<Regions> lstRegion { get; set; }
       public List<Provinces> lstTable { get; set; }
        public List<Provinces> lstProvince { get; set; }
        public int region { get; set; }
    }


    public class ProvinceData
    {
        public int Id { get; set; }
        public int Cases { get; set; }
        public int Deaths { get; set; }
        public string Name { get; set; }

        public int regionId { get; set; }

    }

    public class DataToExport
    {
        public string Cases { get; set; }
        public string Deaths { get; set; }
        public string Name { get; set; }
    }
}