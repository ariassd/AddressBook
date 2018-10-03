using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models;
using System.IO;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var list = Contact.GetList();
            return View(list);
        }

        public ActionResult GetList()
        {
            var list = Contact.GetList();
            return View(list);
        }

        public ActionResult ContactForm() 
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadContacts() {
            var list = Contact.GetList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddContact() {

            string message = "";
            bool result = true;

            var nper = new Contact
            {
                Name = Request.Form["name"],
                LastName = Request.Form["lastname"],
                Email = Request.Form.GetValues("email").ToList(),
                Notes = Request.Form["notes"],
                Phone = Request.Form.GetValues("phone").ToList(),
                Status = "ac"
            };

            //validations
            if (string.IsNullOrWhiteSpace(nper.Name))
            {
                result = false;
                message = "You must write a name";
            }
            if (result && string.IsNullOrWhiteSpace(nper.LastName))
            {
                result = false;
                message = "You must write a Last name";
            }
            if (result && nper.Email.Count == 0 && string.IsNullOrWhiteSpace(nper.Email[0]))
            {
                result = false;
                message = "You must write an Email";
            }
            if (result && nper.Phone.Count == 0 && string.IsNullOrWhiteSpace(nper.Phone[0]))
            {
                result = false;
                message = "You must write a phone";
            }
            //validations

            if (result)
            {
                result = Contact.AddNew(nper, out message);
            }

            return Json(new
            {
                Result = result ? "OK" : "Error",
                Message = message
            });
        }


        public ViewResult Edit()
        {
            string id = Request.Form["id"];
            Contact result = Contact.FindById(id);

            return View(result);
        }

        [HttpPost]
        public JsonResult EditContact()
        {

            string message = "";
            var nper = new Contact
            {
                Name = Request.Form["name"],
                LastName = Request.Form["lastname"],
                Email = Request.Form.GetValues("email").ToList(),
                Notes = Request.Form["notes"],
                Phone = Request.Form.GetValues("phone").ToList(),
                Id = new MongoDB.Bson.ObjectId(Request.Form["objectid"]),
                Status = "ac"
            };
            bool result = Contact.Edit(nper, out message);

            return Json(new
            {
                Result = result ? "OK" : "Error",
                Message = message
            });
        }


        [HttpPost]
        public JsonResult Delete()
        {
            string message = "";
            string id = Request.Form["id"];
            bool result = false;
            if (!string.IsNullOrWhiteSpace(id))
            {
                result = Contact.Delete(id, out message);
            }
            else
            {
                message = "Id not found";
            }
            return Json(new
            {
                Result = result ? "OK" : "Error",
                Message = message
            });
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            string message = "";
            string fileName = Request.Form["fileName"];
            string fileContent = Request.Form["fileContent"];
            DataSet dsResult = null;
            List<Contact> listOfContacts = new List<Contact>();
            int validContacts = 0;
            int noValidContacts = 0;
            bool result = false;

            try
            {

                if (!(fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx") ))  
                {
                    result = false;
                    message = "It seems the archive isn't an excel file";
                }
                else
                {
                    result = true;
                }

                if (result)
                {
                    byte[] file = System.Convert.FromBase64String(fileContent);

                    using (MemoryStream stream = new MemoryStream(file))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            dsResult = reader.AsDataSet(new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration { UseHeaderRow = true }
                            });
                        }
                    }

                    if (dsResult?.Tables[0] != null)
                    {
                        foreach (DataRow item in dsResult.Tables[0].Rows)
                        {
                            try
                            {
                                listOfContacts.Add(new Contact
                                {
                                    Name = item[0].ToString(),
                                    LastName = item[1].ToString(),
                                    Phone = item[2].ToString().Split(';').ToList(),
                                    Email = item[3].ToString().Split(';').ToList(),
                                    Notes = item[4].ToString(),
                                    Status = "ac"
                                });
                                validContacts++;
                            }
                            catch
                            {
                                noValidContacts++;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        result = false;
                        message = "We are very sorry, we don't found any contact.";
                    }
                }

            }
            catch
            {
                result = false;
                message = "There was a trouble reading the file. Be sure the format of the file is the required.";
            }

            if (result) result = Contact.Add(listOfContacts, out message);

            if (result)
            {
                message = $"The contacts was imported, there were {validContacts} valid contacts" 
                    + ((noValidContacts != 0) ? " and {noValidContacts} not valid contacts" : "");
            }
                                
            return Json(new
            {
                Result = result ? "OK" : "Error",
                Message = message
            });
        }

        public FileResult DownloadExcel()
        {
            byte[] result = null;

            List<Contact> contacts = Contact.GetList();
            string fileName = "contacts.xlsx";

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "Name", "Last Name", "Phone", "Email", "Notes" }
                  };

                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                var rows = new List<string[]>();
                foreach (var item in contacts)
                {
                    rows.Add(
                        new string[] {
                            item.Name,
                            item.LastName,
                            string.Join(";", item.Phone),
                            string.Join(";", item.Email),
                            item.Notes
                    });
                }

                string rowsRange = $"A2:{Char.ConvertFromUtf32(rows[0].Length + 64)}{rows.Count + 1}";
                worksheet.Cells[rowsRange].LoadFromArrays(rows);

                using (MemoryStream mStream = new MemoryStream())
                {
                    excel.SaveAs(mStream);
                    result = mStream.GetBuffer();
                }
            }
            return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult Api() {
            return View();
        }

        public ActionResult ApiDoc() {
            return View();
        }
    }
}
