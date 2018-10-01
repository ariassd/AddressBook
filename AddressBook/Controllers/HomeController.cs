using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using AddressBook.Models;

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
                Email = Request.Form["email"],
                Notes = Request.Form["notes"],
                Phone = Request.Form["phone"],
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
            if (result && string.IsNullOrWhiteSpace(nper.Email))
            {
                result = false;
                message = "You must write an Email";
            }
            if (result && string.IsNullOrWhiteSpace(nper.Phone))
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
                Email = Request.Form["email"],
                Notes = Request.Form["notes"],
                Phone = Request.Form["phone"],
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
            string id = Request.Form["contactsß"];
            bool result = false;
            if (!string.IsNullOrWhiteSpace(id))
            {
                //result = Contact.Delete(id, out message);
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

        public ActionResult Api() {
            return View();
        }

        public ActionResult ApiDoc() {
            return View();
        }
    }
}
