using LeaveRequest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.Diagnostics;

//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.Objects.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using System.Web.Mvc;

namespace LeaveRequest.Controllers
{
    public class RequestFormController : Controller
    {
        private RequestFormDB _db = new RequestFormDB();

        public ActionResult GetAllForms(string Command, string lastName = "", string firstName = "")
        {
            //TempData["LNAME"] = lastName;
            //TempData["FNAME"] = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FirstName = firstName;

            if (lastName == "" || firstName == "")
            {
                TempData["STATUS"] = "Both last and first name are required";
                return PartialView("_AllForms", null);
            }

            // everyone can view their own created forms
            if (Command == "view")
            {
                TempData["ISMANAGER"] = "no";
                var forms = _db.RequestForms.Where(f => (f.LName.Equals(lastName) && f.FName.Equals(firstName)));
                return PartialView("_AllForms", forms);
            }

            // next is the logic for Commmand == "approve"

            // if it's manager then locate all forms to be signed by this manager.
            Manager mgr = _db.Managers.FirstOrDefault(m => (m.LName.Equals(lastName) && m.FName.Equals(firstName)));
            if (mgr != null)
            {
                TempData["ISMANAGER"] = "yes";

                var forms = new List<RequestForm>();
                List<RequestForm> listForms = _db.RequestForms.ToList();
                for (int i = 0; i < listForms.Count(); i++)
                {
                    var aForm = listForms.ElementAt(i);
                    if (aForm != null && aForm.NoMoreSigsNeeded == false)
                    {
                        Signature sig = aForm.ManagersSig.FirstOrDefault(s => s.SignedByManager == mgr.ID && s.IsSignature == false);
                        if (sig != null)
                        {
                            forms.Add(aForm);
                        }
                    }
                }
                return PartialView("_AllForms", forms);




                //var sigs = _db.Signatures.Where(s => s.SignedByManager == mgr.ID);
                //var forms = _db.RequestForms.Where(f => sigs.Contains(f.ID)));          //Select(f => f.ManagersSig.Where(s => s.SignedByManager == mgr.ID));
                ////var forms = allForms.Select(m => (m.ManagersSig.Select(s => s.SignedByManager == mgr.ID)));
                //(Enumerable.Range(sigs.First().SignedByManager, sigs.Last().SignedByManager))

                //var forms1 = from f in _db.RequestForms
                //             where (f.ID in [sigs.First().SignedByManager, sigs.Last().SignedByManager])
                //             select f;

                //var forms = new List<RequestForm>();
                //List<Signature> list = sigs.ToList();
                //for (int i = 0; i < list.Count(); i++)
                //{
                //    int id = list.ElementAt(i).;
                //    var aForm = _db.RequestForms.FirstOrDefault(f => (f.ID == id));
                //    if (aForm != null)
                //        forms.Add((RequestForm)aForm);
                //}
                //return PartialView("_AllForms", forms);
            }
            // if it's not a manager then locate all records that had been created by this person.
            else
            {
                TempData["ISMANAGER"] = "no";
                var forms = _db.RequestForms.Where(f => (f.LName.Equals(lastName) && f.FName.Equals(firstName)));
                return PartialView("_AllForms", forms);
            }
        }

        // "approve"
        public ActionResult GetView(string Command, int formId, string lastName = "", string firstName = "")
        {
            Manager mgr = _db.Managers.FirstOrDefault(m => (m.LName.Equals(lastName) && m.FName.Equals(firstName)));
            RequestForm form = _db.RequestForms.FirstOrDefault(f => (f.ID == formId));
            if (form == null)   // this is an employee who wants to create a form
            {
                //TempData["EDITMODE"] = "create";

                // can't find form
                return View();
            }
            else
            {
                // locate the last approving manager's ID from the RequestForm
                int approvingMgrID = 0;
                Signature lastSignedMgr = form.ManagersSig.Last();
                if (lastSignedMgr != null)
                    approvingMgrID = lastSignedMgr.SignedByManager;

                Manager approvingMgr = _db.Managers.FirstOrDefault(m => m.ID == approvingMgrID);
                PopulateLists(Command, lastName, firstName, form.PayPeriod, approvingMgr != null ? approvingMgr.FullName : "",
                              form.TypeOfLeave1, form.TypeOfLeave2, form.TypeOfLeave3);

                if (Command == "approve")
                    TempData["EDITMODE"] = "approve";
                else
                    TempData["EDITMODE"] = "view";

                TempData["ISMANAGER"] = (mgr != null) ? "yes" : "no";
                return View("Form", form);
            }
        }

        // "approve"
        [HttpPost]
        public ActionResult GetView(string Command, int formId, RequestForm dummyForm, string ApprovingManager,
                                  string typeOfLeave1, string typeOfLeave2, string typeOfLeave3, string EmployeeSignatureId,
                                  string ManagerId,
                                  string ManagerSignatureStr = "", bool IsSignature = false)
        {
            if (Command != "approve")
                return View();

            var reqForm = _db.RequestForms.FirstOrDefault(f => f.ID == formId);

            if (IsSignature == false ||
                (dummyForm.NoMoreSigsNeeded == false && ApprovingManager == "[Select a Manager]"))
            {
                if (IsSignature == false)
                    ModelState.AddModelError("", "Please approve this form");
                if (dummyForm.NoMoreSigsNeeded == false && ApprovingManager == "[Select a Manager]")
                    ModelState.AddModelError("", "Please select a Manager to approve");

                string last = "", first = "";
                ParseName(reqForm.FullName, ref last, ref first);
                reqForm.LName = last;
                reqForm.FName = first;

                // for some reason, reqForm.EmployeeSig, which is passed ack from the view's Form becomes null here???
                // so I need to bring back from the database
                if (reqForm.EmployeeSig == null)
                {
                    int id = Convert.ToInt32(EmployeeSignatureId);
                    var empSig = _db.Signatures.FirstOrDefault(m => m.ID == id);
                    reqForm.EmployeeSig = empSig;
                }

                PopulateLists("approve", reqForm.LName, reqForm.FName, reqForm.PayPeriod, ApprovingManager, typeOfLeave1, typeOfLeave2, typeOfLeave3);
                TempData["EDITMODE"] = "approve";

                return View("Form", reqForm);
            }

            if (ModelState.IsValid)
            {
                if (IsSignature && ManagerSignatureStr != null) // 1/ IsSignature property is true when mouse is pressed in signature area.
                {
                    // update the last signing manager & prepare for the next signing manager unless there's no need to sign anymore.
                    int id = Convert.ToInt32(ManagerId);

                    Signature approvedSig = reqForm.ManagersSig.FirstOrDefault(s => s.SignedByManager == id);       //_db.Signatures.FirstOrDefault(s => s.SignedByManager == id);
                    approvedSig.SignedDate = DateTime.Now;
                    approvedSig.SignatureStr = ManagerSignatureStr;
                    approvedSig.IsSignature = IsSignature;
                    ManagerSignatureStr = ManagerSignatureStr.Replace("data:image/png;base64,", "");      // The converted base64 string data have "data:image/png;base64," as additional data 
                    byte[] bytIn = Convert.FromBase64String(ManagerSignatureStr);                  // Converting base64 string to byte array.
                    approvedSig.SignatureGuid = SaveByteArray(bytIn);                               // Saving byte array in database and returning guid to SignatureGuid property.
                    _db.Entry(approvedSig).State = System.Data.EntityState.Modified;

                    // add the new signing manager
                    string last = "", first = "";
                    string signingMgrEmail = "";
                    if (dummyForm.NoMoreSigsNeeded == false)
                    {
                        ParseName(ApprovingManager, ref last, ref first);
                        Manager mgr = _db.Managers.FirstOrDefault(m => (m.LName.Equals(last) && m.FName.Equals(first)));
                        if (mgr == null)
                        {
                            ModelState.AddModelError("", "Can't locate the manager for the approval. Please contact the Admin.");
                            PopulateLists("create", reqForm.LName, reqForm.FName, reqForm.PayPeriod, ApprovingManager);
                            return View("Form", reqForm);
                        }

                        signingMgrEmail = mgr.Email;

                        Signature managerSig = new Signature();
                        managerSig.SignedByManager = mgr.ID;
                        managerSig.LName = last;
                        managerSig.FName = first;
                        reqForm.ManagersSig.Add(managerSig);
                    }

                    reqForm.NoMoreSigsNeeded = dummyForm.NoMoreSigsNeeded;
                    _db.Entry(reqForm).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();

                    // send the email to the next approving manager
                    if (signingMgrEmail != "")
                        SendMail(signingMgrEmail, reqForm.FullName, "");

                    // send notification to user when all sigs are collected
                    if (dummyForm.NoMoreSigsNeeded)
                    {
                        if (reqForm.Email != "")
                            SendMail(reqForm.Email, "", "The Leave Request has been approved");
                    }
                   
                }
            }
            return RedirectToAction("Confirm", "Home");
        }

        // create function 
        public ActionResult Index(string lastName = "", string firstName = "")
        {
            PopulateLists("create", lastName, firstName);

            ViewBag.LastName = lastName;
            ViewBag.FirstName = firstName;

            TempData["EDITMODE"] = "create";

            RequestForm f = new RequestForm();
            f.LName = lastName;
            f.FName = firstName;
            f.EmployeeSig = new Signature();
            f.EmployeeSig.SignedDate = DateTime.Now;
            return View("Form", f);

            //// is this a manager?
            //Manager mgr = _db.Managers.FirstOrDefault(m => (m.LName.Equals(lastName) && m.FName.Equals(firstName)));
            //if (mgr == null)       // not a manager. 
            //{
            //    // can we locate this person
            //    RequestForm employee = _db.RequestForms.FirstOrDefault(f => (f.LName.Equals(lastName) && f.FName.Equals(firstName)));
            //    if (employee == null)   // this is an employee who wants to create a form
            //    {
            //        PopulateLists();

            //        ViewBag.LastName = lastName;
            //        ViewBag.FirstName = firstName;

            //        TempData["EDITMODE"] = "create";
            //        return View();
            //    }
            //    else  // this is an employee who can only view the form
            //    {
            //        // locate the last approving manager's ID from the RequestForm
            //        int approvingMgrID = 0;
            //        Signature lastSignedMgr = employee.ManagersSig.Last();
            //        if (lastSignedMgr != null)
            //            approvingMgrID = lastSignedMgr.SignedByManager;

            //        Manager approvingMgr = _db.Managers.FirstOrDefault(m => m.ID == approvingMgrID);
            //        PopulateLists(employee.PayPeriod, approvingMgr != null ? approvingMgr.FullName : "",
            //                      employee.TypeOfLeave1, employee.TypeOfLeave2, employee.TypeOfLeave3);

            //        TempData["EDITMODE"] = "view";
            //        TempData["ISMANAGER"] = "no";
            //        return View(employee);
            //   }
            //}

            //TempData["EDITMODE"] = "view";
            //TempData["ISMANAGER"] = "yes";
            //return View();
        }

        // save from a create function 
        [HttpPost]
        public ActionResult Index(RequestForm reqForm, string ApprovingManager, 
                                  string typeOfLeave1, string typeOfLeave2, string typeOfLeave3,
                                  string SignatureStr = "", bool IsSignature = false)
        {
            if (IsSignature == false || ApprovingManager == "[Select a Manager]" || typeOfLeave1.Trim() == "")
            {
                if (IsSignature == false)
                    ModelState.AddModelError("", "Please sign before sending for approval");
                if (ApprovingManager == "[Select a Manager]")
                    ModelState.AddModelError("", "Please select a Manager to approve");
                if (typeOfLeave1.Trim() == "")
                    ModelState.AddModelError("", "Please select a Leave Type");

                string last = "", first = "";
                ParseName(reqForm.FullName, ref last, ref first);
                reqForm.LName = last;
                reqForm.FName = first;
                PopulateLists("create", reqForm.LName, reqForm.FName, reqForm.PayPeriod, ApprovingManager, typeOfLeave1, typeOfLeave2, typeOfLeave3);
                TempData["EDITMODE"] = "create";

                return View("Form", reqForm);
            }

            if (ModelState.IsValid)
            {
                if (IsSignature && SignatureStr != null) // 1/ IsSignature property is true when mouse is pressed in signature area.
                // 2/ JSignature converts signature area(no matter if there is blank signature) in base64 string data 
                //    that we are receiving in Signature property.
                // that causes problem while converting this string in byte array so removing this additional data.
                {
                    string last = "", first = "";
                    ParseName(reqForm.FullName, ref last, ref first);
                    reqForm.LName = last;
                    reqForm.FName = first;

                    // look for this manager so we can get its PK to asign it to RequestForm -> ManagersSig[]
                    last = "";
                    first = "";
                    ParseName(ApprovingManager, ref last, ref first);

                    Manager mgr = _db.Managers.FirstOrDefault(m => (m.LName.Equals(last) && m.FName.Equals(first)));
                    if (mgr == null)
                    {
                        ModelState.AddModelError("", "Can't locate the manager for the approval. Please contact the Admin.");
                        PopulateLists("create", reqForm.LName, reqForm.FName, reqForm.PayPeriod, ApprovingManager);
                        return View("Index", reqForm);
                    }

                    //if (reqForm.EmployeeSig != null)
                    //{
                    //    reqForm.EmployeeSig.SignatureStr = SignatureStr;
                    //    reqForm.EmployeeSig.IsSignature = IsSignature;
                    //    SignatureStr = SignatureStr.Replace("data:image/png;base64,", "");      // The converted base64 string data have "data:image/png;base64," as additional data 
                    //    byte[] bytIn = Convert.FromBase64String(SignatureStr);                  // Converting base64 string to byte array.
                    //    reqForm.EmployeeSig.SignatureGuid = SaveByteArray(bytIn);                               // Saving byte array in database and returning guid to SignatureGuid property.
                    //}
                    //else
                    //{

                    // add the employee signature
                        Signature sig = new Signature();
                        sig.LName = reqForm.LName;
                        sig.FName = reqForm.FName;
                        sig.SignedDate = DateTime.Now;
                        sig.SignatureStr = SignatureStr;
                        sig.IsSignature = IsSignature;
                        SignatureStr = SignatureStr.Replace("data:image/png;base64,", "");      // The converted base64 string data have "data:image/png;base64," as additional data 
                        byte[] bytIn = Convert.FromBase64String(SignatureStr);                  // Converting base64 string to byte array.
                        sig.SignatureGuid = SaveByteArray(bytIn);                               // Saving byte array in database and returning guid to SignatureGuid property.
                        reqForm.EmployeeSig = sig;
                    //}


                    //Manager mgr = new Manager();
                    //mgr.LName = last;
                    //mgr.FName = first;
                    //mgr.IsNotified = true;

                    Signature managerSig = new Signature();
                    managerSig.SignedByManager = mgr.ID;
                    managerSig.LName = last;
                    managerSig.FName = first;
                    reqForm.ManagersSig.Add(managerSig);

                    // use the name to construct the email for this form.
                    reqForm.Email = reqForm.FName.ElementAt(0) + reqForm.LName + "@aclibrary.org";

                    _db.RequestForms.Add(reqForm);
                    _db.SaveChanges();

                    // send the email to the next approving manager
                    SendMail(mgr.Email, reqForm.FullName, "");

                    // Displaying signature in image format.
                    //SignatureStr = "data:image/png;base64," + Convert.ToBase64String(bytIn);    // Converting byte array to base64 string and prepending the additional data
                    // that JSignature require it to convert it in image.
                }
            }

            return RedirectToAction("Confirm", "Home");
        }

        public ActionResult Print(int id = 0, string lastName = "", string firstName = "")
        {
            RequestForm reqForm = _db.RequestForms.Find(id);
            if (reqForm == null)
            {
                TempData["STATUS"] = "Form not Found.";
                return RedirectToAction("Index", "Home");//, new { TIN = 0 });
            }

            // locate the last approving manager's ID from the RequestForm
            int approvingMgrID = 0;
            Signature lastSignedMgr = reqForm.ManagersSig.Last();
            if (lastSignedMgr != null)
                approvingMgrID = lastSignedMgr.SignedByManager;

            Manager approvingMgr = _db.Managers.FirstOrDefault(m => m.ID == approvingMgrID);
            PopulateLists("view", lastName, firstName, reqForm.PayPeriod, approvingMgr != null ? approvingMgr.FullName : "",
                          reqForm.TypeOfLeave1, reqForm.TypeOfLeave2, reqForm.TypeOfLeave3);

            TempData["EDITMODE"] = "view";
            TempData["ISMANAGER"] = "no";

            TempData["PRINTSTATUS"] = "The form has been saved to ";
            return View("Print", reqForm);
            

            //////return new ViewAsPdf(model);
        }

        public ActionResult PrintForm(int Id, string fullName = "")
        {
            return new ActionAsPdf(
                           "Print",
                           new { id = Id }) { FileName = fullName == "" ? Id.ToString() + "_LeaveRequest.pdf" : fullName + "_LeaveRequest.pdf" };
        }

        //------------------ Private ---------------------------------
        private void SendMail(string email, string fullName, string msg)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("LeaveRequest-Notification@aclibrary.org", "Leave Request Notification");
            message.To.Add(new MailAddress(email));
            message.Subject = msg != "" ? msg : "A leave request is waiting for your approval!";
            message.Body = msg != "" ? msg : "Please approve the leave request for " + fullName;
            SmtpClient client = new SmtpClient("bender.aclibrary.org", 25);
            client.EnableSsl = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                TempData["OBJECT"] = null;
                TempData["STATUS"] = e.Message;
            }
        }

        private void ParseName(string full, ref string last, ref string first)
        {
            int commaInd = full.IndexOf(',');
            last = full.Substring(0, commaInd);
            first = full.Substring(commaInd + 1, full.Length - last.Length - 1);
            last = last.Trim();
            first = first.Trim();
        }

        private void PopulateLists(string Command, string lastName, string firstName,
                                   string selectedPayPeriod = "", string selectedApprovingManager = "",
                                   string typeOfLeave1 = "", string typeOfLeave2 = "", string typeOfLeave3 = "")
        {
            var payPeriod = _db.PayPeriods.Select(p => new SelectListItem
            {
                Text = p.Period,
                Selected = (p.Period.Equals(selectedPayPeriod, StringComparison.OrdinalIgnoreCase)) ? true : false,
            });
            ViewBag.PayPeriod = payPeriod;

            //var approvingManager = _db.Managers.Select(m => new SelectListItem
            //{
            //    Text = m.LName + ", " + m.FName,
            //    Selected = (selectedApprovingManager.Equals(m.LName + ", " + m.FName, StringComparison.OrdinalIgnoreCase)) ? true : false,
            //});

            List<SelectListItem> approvingManager = new List<SelectListItem>();
            approvingManager.Add(new SelectListItem
            {
                Text = "[Select a Manager]",
            });
            string emplName = lastName + ", " + firstName;
            foreach (var m in _db.Managers)
            {
                string name = m.LName + ", " + m.FName;
                if (Command.Equals("create", StringComparison.OrdinalIgnoreCase) ||
                    Command.Equals("view", StringComparison.OrdinalIgnoreCase) ||
                    (name.Equals(selectedApprovingManager, StringComparison.OrdinalIgnoreCase) == false &&
                    name.Equals(emplName, StringComparison.OrdinalIgnoreCase) == false))
                {
                    approvingManager.Add(new SelectListItem
                                            {
                                                Text = name,
                                                Selected = (selectedApprovingManager.Equals(m.LName + ", " + m.FName, StringComparison.OrdinalIgnoreCase)) ? true : false,
                                            }
                                        );
                }
            }
            ViewBag.ApprovingManager = approvingManager;

            //var leave1 = _db.LeaveTypes.Select(t => new SelectListItem
            //{
            //    Text = t.Type,
            //    Selected = (typeOfLeave1.Equals(t.Type, StringComparison.OrdinalIgnoreCase)) ? true : false,
            //});
            List<SelectListItem> leave1 = new List<SelectListItem>();
            leave1.Add(new SelectListItem
            {
                Text = "",
            });
            foreach (var t in _db.LeaveTypes)
            {
                leave1.Add(new SelectListItem
                            {
                                Text = t.Type,
                                Selected = typeOfLeave1 != null ? ((typeOfLeave1.Equals(t.Type, StringComparison.OrdinalIgnoreCase)) ? true : false) : false,
                            });
            }

            List<SelectListItem> leave2 = new List<SelectListItem>();
            leave2.Add(new SelectListItem
            {
                Text = "",
            });
            foreach (var t in _db.LeaveTypes)
            {
                leave2.Add(new SelectListItem
                {
                    Text = t.Type,
                    Selected = typeOfLeave2 != null ? ((typeOfLeave2.Equals(t.Type, StringComparison.OrdinalIgnoreCase)) ? true : false) : false,
                });
            }

            List<SelectListItem> leave3 = new List<SelectListItem>();
            leave3.Add(new SelectListItem
            {
                Text = "",
            });
            foreach (var t in _db.LeaveTypes)
            {
                leave3.Add(new SelectListItem
                {
                    Text = t.Type,
                    Selected = typeOfLeave3 != null ? ((typeOfLeave3.Equals(t.Type, StringComparison.OrdinalIgnoreCase)) ? true : false) : false,
                });
            }

            ViewBag.typeOfLeave1 = leave1;
            ViewBag.typeOfLeave2 = leave2;
            ViewBag.typeOfLeave3 = leave3;
        }

        private Guid SaveByteArray(byte[] bytIn)
        {
            // Byte array can be saved in database and primary key should be returned to have reference of this signature.
            return Guid.NewGuid();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        ////---------------------------------------------------------------------------------------------------------------
        //// GET api/RequestForm
        //public IEnumerable<RequestForm> GetRequestForms()
        //{
        //    return _db.RequestForms.AsEnumerable();
        //}

        //// GET api/RequestForm/5
        //public RequestForm GetRequestForm(int id)
        //{
        //    RequestForm requestform = }.RequestForms.Find(id);
        //    if (requestform == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return requestform;
        //}

        //// PUT api/RequestForm/5
        //public HttpResponseMessage PutRequestForm(int id, RequestForm requestform)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    if (id != requestform.ID)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    db.Entry(requestform).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        //// POST api/RequestForm
        //public HttpResponseMessage PostRequestForm(RequestForm requestform)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RequestForms.Add(requestform);
        //        db.SaveChanges();

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, requestform);
        //        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = requestform.ID }));
        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}

        //// DELETE api/RequestForm/5
        //public HttpResponseMessage DeleteRequestForm(int id)
        //{
        //    RequestForm requestform = db.RequestForms.Find(id);
        //    if (requestform == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }

        //    db.RequestForms.Remove(requestform);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, requestform);
        //}

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}