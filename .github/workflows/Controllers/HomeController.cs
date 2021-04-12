using Microsoft.Owin.Logging;
using pruebaCovid.DAL;
using pruebaCovid.Models;
using pruebaCovid.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace pruebaCovid.Controllers
{
    public class HomeController : Controller
    {
        private pruebaCovidContext db = new pruebaCovidContext();
        public static ILogger logger = null;

        public ActionResult Index()
        {
            List<Regions> lstRegion = new List<Regions>()
            {
                new Regions()
                {
                  RegionId = 0,
                  Name = "Regions"
                }
            };

            List<Provinces> lstProvince = new List<Provinces>()
            {
                new Provinces()
                {
                  ProvinceId = 0,
                  ProvName = "New Province"
                }
            };

            lstProvince.AddRange(db.Provinces.ToList());
            lstRegion.AddRange(db.Regions.ToList());

            var lstTable = db.Provinces.GroupBy(l => l.RegionId).ToList()
                        .Select(x => new Provinces()
                        {
                            Cases = x.Sum(y => y.Cases),
                            ProvName = x.First().ProvName,
                            Deaths = x.Sum(y => y.Deaths)
                        }).OrderByDescending(x => x.Cases).Take(10).ToList();

            VMInformation Inforamtion = new VMInformation()
            {
                lstRegion = lstRegion,
                lstTable = lstTable,
                region = 0,
                lstProvince = lstProvince
            };

            return View(Inforamtion);
        }

        [HttpPost]
        public JsonResult SaveRegion(Regions newRegion)
        {
            try
            {
                Regions region0 = new Regions();
                string status = string.Empty;
                if (newRegion.RegionId == 0)
                {
                    region0 = new Regions()
                    {
                        Name = newRegion.Name
                    };
                    db.Regions.Add(region0);
                    status = "added";
                }
                else
                {
                    region0 = db.Regions.FirstOrDefault(x => x.RegionId == newRegion.RegionId);
                    region0.Name = newRegion.Name;
                    status = "edited";
                }
                db.SaveChanges();
                return Json(new { error = false, message = "The Region was " + status + "success" });
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Error to save Region " });
            }
        }


        #region save functions
        [HttpPost]
        public JsonResult SaveProvince(ProvinceData information)
        {
            try
            {
                Provinces province = new Provinces();
                string status = string.Empty;

                if (information.Id == 0)
                {
                    province = new Provinces()
                    {
                        Cases = information.Cases,
                        Deaths = information.Deaths,
                        ProvName = information.Name
                    };
                    db.Provinces.Add(province);
                    status = "added";
                }
                else
                {
                    province = db.Provinces.FirstOrDefault(x => x.ProvinceId == information.Id);
                    province.Deaths = information.Deaths;
                    province.Cases = information.Cases;
                    status = "edited";

                }
                db.SaveChanges();
                return Json(new { error = false, message = "The Province was " + status + "success" });

            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Error to save Province" });
            }
        }

        //Get data infomation for fill the table
        [HttpPost]
        public JsonResult GetRegionOrProvince(int region)
        {
            try
            {
                List<Regions> lstregions = new List<Regions>();
                List<Provinces> lstTable = new List<Provinces>();
                if (region == 0)
                {
                    lstregions = db.Regions.ToList();
                    lstTable = db.Provinces.GroupBy(l => l.RegionId).ToList()
                        .Select(x => new Provinces()
                        {
                            Cases = x.Sum(y => y.Cases),
                            ProvName = x.First().ProvName,
                            Deaths = x.Sum(y => y.Deaths)
                        }).OrderByDescending(x => x.Cases).Take(10).ToList();
                }
                else
                {
                    var provincesList = db.Provinces.Where(x => x.RegionId == region)
                        .OrderByDescending(x => x.Cases).Take(10);
                    lstregions = provincesList
                        .Select(x => new Regions
                        {
                            RegionId = x.ProvinceId,
                            Name = x.ProvName
                        }).ToList();
                    lstTable = provincesList.ToList();
                }

                return Json(new
                {
                    error = false,
                    lstregions = lstregions,
                    lstTable = lstTable
                });

            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Error to get Province" });
            }
        }
        #endregion

        public JsonResult GetRegion(int regionid)
        {
            try
            {
                return Json(new { error = false, Name = db.Regions.FirstOrDefault(x => x.RegionId == regionid).Name });
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Error to get Region information" });
            }
        }

        public JsonResult GetProvince(int provinceId)
        {
            try
            {
                return Json(new { error = false, province = db.Provinces.FirstOrDefault(x => x.ProvinceId == provinceId) });
            }
            catch (Exception e)
            {
                return Json(new { error = true, message = "Error to get Region information" });
            }
        }

    }
       
}