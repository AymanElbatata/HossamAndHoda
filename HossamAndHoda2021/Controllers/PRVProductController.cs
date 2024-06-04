using HossamAndHoda2021.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HossamAndHoda2021.Controllers
{
    [Authorize]
    public class PRVProductController : BaseController
    {
        // GET: PRVProduct
        HossamAndHoda2021DBEntities mydb = new HossamAndHoda2021DBEntities();

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


        [HttpPost]
        public JsonResult listAll(string txtENBTNSearch = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;
                if (txtENBTNSearch != null && txtENBTNSearch != "" && !string.IsNullOrEmpty(txtENBTNSearch))
                {
                    var lst = mydb.ProductTBL.Where(a => a.IsDeleted == false && (a.ProductAutoCode.Contains(txtENBTNSearch.Trim()) || a.ProductNameAR.Contains(txtENBTNSearch.Trim()) || a.ProductDetails.Contains(txtENBTNSearch.Trim()))).ToList();
                    return Json(new { Result = "OK", Records = lst.Skip(jtStartIndex).Take(jtPageSize), TotalRecordCount = lst.Count() });
                }
                else
                {
                    var lst = mydb.ProductTBL.Where(a => a.IsDeleted == false).OrderByDescending(a=>a.ProductViews).ToList();
                    return Json(new { Result = "OK", Records = lst.Skip(jtStartIndex).Take(jtPageSize), TotalRecordCount = lst.Count() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex });
            }
        }


        [HttpPost]
        public JsonResult CreateOne(ProductTBL model, HttpPostedFileBase ProductImage1URL = null, HttpPostedFileBase ProductImage2URL = null, HttpPostedFileBase ProductImage3URL = null)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;

                if (ModelState.IsValid)
                {
                    model.ProductAutoCode = MySPECIALGUID.GUIDOnlyNumber(6);
                    //

                    var reqexist = mydb.ProductTBL.Where(a => a.IsDeleted == false  && a.ProductNameAR == model.ProductNameAR
                    && a.ProductDetails == model.ProductDetails && a.ProductAutoCode == model.ProductAutoCode && a.ProductFactoryCode == model.ProductFactoryCode).Count();
                    if (reqexist == 0)
                    {
                        //
                        if (ProductImage1URL != null && ProductImage1URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage1URL))
                            {
                                model.ProductImage1URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage1URL, ProductImage1URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }

                        if (ProductImage2URL != null && ProductImage2URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage2URL))
                            {
                                model.ProductImage2URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage2URL, ProductImage2URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }
                        if (ProductImage3URL != null && ProductImage3URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage3URL))
                            {
                                model.ProductImage3URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage3URL, ProductImage3URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }
                        
                        //

                        model.IsDeleted = false;
                        model.ProductViews = 0;
                        model.DateOfMaking = DateTime.Now;
                        var data = mydb.ProductTBL.Add(model);
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
        public JsonResult UpdateOne(ProductTBL model, HttpPostedFileBase ProductImage1URL = null, HttpPostedFileBase ProductImage2URL = null, HttpPostedFileBase ProductImage3URL = null)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;

                if (ModelState.IsValid)
                {
                    //
                    //
                    var reqexist = mydb.ProductTBL.Where(a => a.IsDeleted == false && a.ProductID != model.ProductID && a.ProductNameAR == model.ProductNameAR           
                    && a.ProductDetails == model.ProductDetails && a.ProductAutoCode == model.ProductAutoCode && a.ProductFactoryCode == model.ProductFactoryCode).Count();
                    if (reqexist == 0)
                    {
                        // For Image and file

                        var mynewobject = mydb.ProductTBL.Find(model.ProductID);

                            model.ProductImage1URL = mynewobject.ProductImage1URL;

                            model.ProductImage2URL = mynewobject.ProductImage2URL;

                            model.ProductImage3URL = mynewobject.ProductImage3URL;

                        //check if  exist
                        if (ProductImage1URL != null && ProductImage1URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage1URL))
                            {
                                model.ProductImage1URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage1URL, ProductImage1URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }

                        if (ProductImage2URL != null && ProductImage2URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage2URL))
                            {
                                model.ProductImage2URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage2URL, ProductImage2URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }
                        if (ProductImage3URL != null && ProductImage3URL.ContentLength > 0)
                        {
                            if (FileImageUploadAYM.ImageISTrue(ProductImage3URL))
                            {
                                model.ProductImage3URL = FileImageUploadAYM.ADDIMAGE(model.ProductImage3URL, ProductImage3URL, "Product", 500, 400);
                            }
                            else
                            {
                                return Json(new { Result = "ERROR", Message = "The File must be a Image File." });
                            }
                        }                    
                        //


                        var data = mydb.ProductTBL.Find(model.ProductID);
                        data.ProductNameAR = model.ProductNameAR;
                        data.ProductDetails = model.ProductDetails;
                        data.ProductFactoryCode = model.ProductFactoryCode;
                        data.ProductPrice = model.ProductPrice;
                        data.ProductImage1URL = model.ProductImage1URL;
                        data.ProductImage2URL = model.ProductImage2URL;
                        data.ProductImage3URL = model.ProductImage3URL;
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
        public JsonResult DeleteOne(int ProductID)
        {
            try
            {
                mydb.Configuration.ProxyCreationEnabled = false;

                var newobject = mydb.ProductTBL.Find(ProductID);

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