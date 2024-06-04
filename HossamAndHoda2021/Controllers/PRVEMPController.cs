using HossamAndHoda2021.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HossamAndHoda2021.Controllers
{
    [Authorize]
    public class PRVEMPController : BaseController
    {
        HossamAndHoda2021DBEntities mydb = new HossamAndHoda2021DBEntities();

        // GET: PRVEMP
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }
        }

       

        [HttpPost]
        public JsonResult listAll(string txtENBTNSearch = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;
                if (txtENBTNSearch != null && txtENBTNSearch != "" && !string.IsNullOrEmpty(txtENBTNSearch))
                {
                    var lst = mydb.UserTBL.Where(a => a.IsDeleted == false && (a.UserFullName.Contains(txtENBTNSearch.Trim()) || a.UserEmail.Contains(txtENBTNSearch.Trim()) )).ToList();
                    return Json(new { Result = "OK", Records = lst.Skip(jtStartIndex).Take(jtPageSize), TotalRecordCount = lst.Count() });
                }
                else
                {
                    var lst = mydb.UserTBL.Where(a => a.IsDeleted == false).ToList();
                    return Json(new { Result = "OK", Records = lst.Skip(jtStartIndex).Take(jtPageSize), TotalRecordCount = lst.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex });
            }
        }


        [HttpPost]
        public JsonResult CreateOne(UserTBL model)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;

                if (ModelState.IsValid)
                {
                    //
                    var reqexist = mydb.UserTBL.Where(a => a.IsDeleted == false && (a.UserFullName == model.UserFullName
                    || a.UserEmail == model.UserEmail)).Count();
                    if (reqexist == 0)
                    {
                        model.IsDeleted = false;
                        var data = mydb.UserTBL.Add(model);
                        mydb.SaveChanges();
                        return Json(new { Result = "OK", Record = data });
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "There is a record with the same name" });
                    }
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "Sorry, there is a missing in the DB" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex });

            }
        }


        [HttpPost]
        public JsonResult UpdateOne(UserTBL model)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;

                if (ModelState.IsValid)
                {
                    //check if  exist
                    //
                    //
                    var reqexist = mydb.UserTBL.Where(a => a.IsDeleted == false && a.UserID != model.UserID && (a.UserEmail == model.UserEmail
                    || a.UserFullName == model.UserFullName)).Count();
                    if (reqexist == 0)
                    {
                        var data = mydb.UserTBL.Find(model.UserID);
                        data.UserFullName = model.UserFullName;
                        data.UserEmail = model.UserEmail;
                        data.UserPassword = model.UserPassword;
                        data.IsStopped = model.IsStopped;
                        data.IsDeleted = false;
                        mydb.Entry(data).State = EntityState.Modified;
                        //mydb.Category1TBL.DefaultIfEmpty(model);
                        mydb.SaveChanges();
                        return Json(new { Result = "OK", Record = data });
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "There is a record with the same name" });
                    }
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "Sorry, there is a missing in the DB" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex });
            }
        }


        [HttpPost]
        public JsonResult DeleteOne(int UserID)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;
                var newobject = mydb.UserTBL.Find(UserID);
                int currentUSR = Convert.ToInt32(CurrentUserLoginData.getcurrentusrdataAuth(1));
                if (UserID == currentUSR)
                {
                    return Json(new { Result = "ERROR", Message = "Sorry, You can't delete the current user!." });
                }
                // Send row to be deleted
                if (newobject != null)
                {
                    newobject.IsDeleted = true;
                    mydb.Entry(newobject).State = EntityState.Modified;
                    mydb.SaveChanges();
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "Sorry, there is a missing in the DB" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex });
            }
        }



        #region Clean DB
        [HttpPost]
        public JsonResult CleanMyDB(string CleanOK = null)
        {
            ReturnResultJSON result = new ReturnResultJSON();
            int counter = 0;
            try
            {
                DateTime mydata = DateTime.Now;
                mydata.AddDays(-1);
                var AllOrders = mydb.OrderTBL.Where(a => a.IsCompleted == false && DatesComparison.Date1LessThanDate2(Convert.ToDateTime(a.DateOfMaking), mydata) == true).Any();
                if (AllOrders)
                {
                    var AllDeletedOrders = mydb.OrderTBL.Where(a => a.IsCompleted == false && DatesComparison.Date1LessThanDate2(Convert.ToDateTime(a.DateOfMaking), mydata) == true).ToList();
                    foreach (var item in AllDeletedOrders)
                    {
                        var AllOrdersDetails = mydb.OrderDetailTBL.Where(a => a.OrderIDD == item.OrderID).ToList();
                        foreach (var item2 in AllOrdersDetails)
                        {
                            mydb.OrderDetailTBL.Remove(item2);
                            mydb.SaveChanges();
                        }
                    }
                    foreach (var item in AllDeletedOrders)
                    {
                        mydb.OrderTBL.Remove(item);
                        mydb.SaveChanges();
                        counter++;
                    }
                }

                result.Type = "OK";
                result.Message = counter.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Type = "NO";
                result.Message = "Error";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mydb.Dispose();
            }
            base.Dispose(disposing);
        }




    }
}