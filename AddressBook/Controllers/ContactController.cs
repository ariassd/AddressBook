using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    public class ContactController : ApiController
    {
        // GET api/values
        public JsonResult<Result<List<Contact>>> Get()
        {
            try
            {
                List<Contact> result = new List<Contact>();

                result = Contact.GetList();

                return Json(new Result<List<Contact>>
                {
                    Code = 200,
                    Message = "",
                    Content = result
                });
            } catch (Exception ex) {
                return Json(new Result<List<Contact>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Content = null
                });
            }

        }

        // GET api/values/<text>
        public JsonResult<Result<List<Contact>>> Get(string id)
        {
            try
            {
                List<Contact> result = new List<Contact>();

                result = new List<Contact>();
                result.Add(Contact.FindById(id));

                return Json(new Result<List<Contact>>
                {
                    Code = 200,
                    Message = "",
                    Content = result
                });
            }
            catch (Exception ex)
            {
                return Json(new Result<List<Contact>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Content = null
                });
            }
        }

        // GET api/values/<text>
        public JsonResult<Result<List<Contact>>> Get(string field, string text)
        {
            try
            {
                List<Contact> result = new List<Contact>();

                if (field == "name")
                {
                    result = Contact.FindByName(text);
                }

                return Json(new Result<List<Contact>>
                {
                    Code = 200,
                    Message = "",
                    Content = result
                });
            }
            catch (Exception ex)
            {
                return Json(new Result<List<Contact>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Content = null
                });
            }
        }

        // POST api/values for create
        public JsonResult<Result> Post([FromBody]Contact value)
        {
            try
            {
                string message = "";
                bool result = Contact.AddNew(value, out message);

                if (result)
                {
                    return Json(new Result
                    {
                        Code = 201,
                        Message = "New contact created"
                    });
                }
                else
                {
                    return Json(new Result
                    {
                        Code = 500,
                        Message = "An error has ocurred"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new Result
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        // PUT api/values/5 for update
        public JsonResult<Result> Put(string id, [FromBody]Contact value)
        {
            try
            {
                string message = "";
                value.Id = new MongoDB.Bson.ObjectId(id);
                bool result = Contact.Edit(value, out message);

                if (result)
                {
                    return Json(new Result
                    {
                        Code = 201,
                        Message = "Modified"
                    });
                }
                else
                {
                    return Json(new Result
                    {
                        Code = 500,
                        Message = "Not found"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new Result
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }

        // DELETE api/values/5
        public JsonResult<Result> Delete(string id)
        {
            try
            {
                string message = "";
                bool result = Contact.Delete(id, out message);

                if (result)
                {
                    return Json(new Result
                    {
                        Code = 200,
                        Message = "Contact has deleted"
                    });
                }
                else
                {
                    return Json(new Result
                    {
                        Code = 500,
                        Message = "An error has ocurred"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new Result
                {
                    Code = 500,
                    Message = ex.Message
                });
            }
        }
    }
}