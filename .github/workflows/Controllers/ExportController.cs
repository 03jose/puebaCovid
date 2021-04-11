using ClosedXML.Excel;
using Microsoft.Owin.Logging;
using pruebaCovid.DAL;
using pruebaCovid.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace pruebaCovid.Controllers
{
    public class ExportController : Controller
    {
        private pruebaCovidContext db = new pruebaCovidContext();
        public static ILogger logger = null;
        // GET: Export
        //export functions
        [HttpPost]
        public ActionResult ExportDoc(string typeExport, int region)
        {
            try
            {
                IEnumerable<DataToExport> cells = GetCells(region);

                string fileName = "Top 10 Covid";
                byte[] file;
                if (typeExport == "csv")
                {
                    XLWorkbook workbook = new XLWorkbook();
                    IXLWorksheet worksheet = workbook.Worksheets.Add(fileName);

                    worksheet.Cell(1, 1).InsertData(cells); //insert titles to first row             
                    worksheet.Columns().AdjustToContents();
                    worksheet.Rows("1").Style.Font.Bold = true;
                    worksheet.Columns().Style.Alignment.WrapText = true;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        file = memoryStream.ToArray();
                    }
                    workbook.Dispose();

                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
                else if (typeExport == "xml")
                {
                    XmlWriterSettings settings = new XmlWriterSettings()
                    {
                        Indent = true,
                        IndentChars = ("    "),
                        CloseOutput = true,
                        OmitXmlDeclaration = true,
                    };
                    using (var ms = new MemoryStream())
                    {
                        using (XmlWriter writer = XmlWriter.Create(fileName + ".xml", settings))
                        {
                            string colum0 = region == 0 ? "Region" : "Province";
                            foreach (var cel in cells)
                            {
                                writer.WriteStartElement("Covid");
                                writer.WriteElementString(colum0, cel.Name);
                                writer.WriteElementString("CASES", cel.Cases.ToString());
                                writer.WriteElementString("DEATHS", cel.Deaths.ToString());
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                        file = ms.GetBuffer();
                    }
                    return File(file, System.Net.Mime.MediaTypeNames.Text.Xml, fileName + ".xml");
                }
                else
                {
                    var data = cells.ToString();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                    var output = new FileContentResult(bytes, "application/octet-stream");
                    output.FileDownloadName = fileName + ".txt";

                    return output;
                }
            }
            catch (Exception e)
            {
                logger.WriteError(e.Message);
                throw;
            }

        }

        public IEnumerable<DataToExport> GetCells(int region)
        {
            try
            {
                List<DataToExport> lstTable = new List<DataToExport>();
                if (region == 0)
                {
                    lstTable = db.Provinces.GroupBy(l => l.RegionId)
                        .Select(x => new DataToExport()
                        {
                            Cases = x.Sum(y => y.Cases).ToString(),
                            Name = x.First().ProvName,
                            Deaths = x.Sum(y => y.Deaths).ToString()
                        }).OrderByDescending(x => x.Cases).Take(10).ToList();
                    DataToExport item = new DataToExport()
                    {
                        Cases = "CASES",
                        Name = "REGIONS",
                        Deaths = "DEATHS"
                    };

                    lstTable.Insert(0, item);
                }
                else
                {
                    lstTable = db.Provinces.Where(x => x.RegionId == region)
                        .OrderByDescending(x => x.Cases).Take(10)
                        .Select(x => new DataToExport()
                        {
                            Cases = x.Cases.ToString(),
                            Name = x.ProvName,
                            Deaths = x.Deaths.ToString()
                        }).ToList();
                    DataToExport item = new DataToExport()
                    {
                        Cases = "CASES",
                        Name = "PROVINCE",
                        Deaths = "DEATHS"
                    };

                    lstTable.Insert(0, item);
                }

                return lstTable;

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
