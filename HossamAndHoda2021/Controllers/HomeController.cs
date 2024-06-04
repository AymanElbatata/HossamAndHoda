using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HossamAndHoda2021.Models;

namespace HossamAndHoda2021.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        HossamAndHoda2021DBEntities mydb = new HossamAndHoda2021DBEntities();
        string WebsiteCompanyTXT = MenuList.WebsiteCompanyTXT();

        #region Index & ProductDetail
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string MYSearchTXTLayout = null, int type = 0)
        {
            try
            {
                var data = mydb.ProductTBL.Where(a=>a.IsDeleted==false).ToList();

                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/HTB1XTHmcPfguuRjSszcq6zb7FXaP.jpg";
                // END SEO

                foreach (var item in data)
                {
                    if (data.Count >= 1)
                    {
                        SEO_Metakeywords_Final += item.ProductNameAR + " ";
                        SEO_MetaDescription_Final += item.ProductDetails + " ";
                    }
                }

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/Index";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO

                if (type ==2)
                {
                    ViewBag.success = "شكرا، تم إرسال طلب الشراء بنجاح سيتم التواصل معكم فى أقرب وقت ممكن";
                }
                if (MYSearchTXTLayout != null && type == 1)
                {
                    var mysearch = mydb.ProductTBL.Where(a=> a.IsDeleted == false && (a.ProductAutoCode.Contains(MYSearchTXTLayout) || a.ProductNameAR.Contains(MYSearchTXTLayout) || a.ProductDetails.Contains(MYSearchTXTLayout))).ToList();
                    return View(mysearch);
                }
                return View(data);
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpPost]
        public ActionResult Index(string MYSearchTXTLayout = null)
        {
            try
            {

                return RedirectToAction("Index", "Home", new { MYSearchTXTLayout  = MYSearchTXTLayout, type = 1 });
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ProductDetail(int ProductID)
        {
            try
            {
                var check = mydb.ProductTBL.Where(a => a.IsDeleted == false && a.ProductID == ProductID).Any();

                if (check == false)
                {
                    return RedirectToAction("InvalidNotExists");
                }
                var data = mydb.ProductTBL.Find(ProductID);

                //START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/Product/" + data.ProductImage1URL;
                // END SEO

                SEO_Metakeywords_Final += data.ProductNameAR;
                SEO_MetaDescription_Final += data.ProductDetails;

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/ProductDetail";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO
                //

                if (data.ProductViews == null || data.ProductViews == 0)
                {
                    data.ProductViews = 1;
                }
                else
                {
                    data.ProductViews += 1;
                }
                mydb.Entry(data).State = EntityState.Modified;
                mydb.SaveChanges();

                return View(data);
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        #endregion

        #region Cart

        [HttpGet]
        [AllowAnonymous]
        [CheckSessionUsronline]
        public ActionResult Cart(int ProductID = 0)
        {
            try
            {
                int ORDERID = 0;
                if (Session["MyCurrentOrder"] != null)
                {
                    ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());

                }
                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/829-8299026_cart-png-photos-shopping-cart-icon-png.png";
                // END SEO

                SEO_Metakeywords_Final += "العربة";
                SEO_MetaDescription_Final += "عربة التسوق";

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/Cart";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO

                if (ProductID != 0)
                {
                    if (Session["MyCurrentOrder"] != null)
                    {
                        var getorder = mydb.OrderTBL.Find(ORDERID);
                        var getorderdetail = mydb.OrderDetailTBL.Where(a => a.IsDeleted ==  false && a.OrderIDD == ORDERID && a.ProductIDD == ProductID).Any();
                        if (!getorderdetail)
                        {
                            OrderDetailTBL myorderdetail = new OrderDetailTBL();
                            myorderdetail.IsDeleted = false;
                            myorderdetail.ProductIDD = ProductID;
                            myorderdetail.Unit = 1;
                            myorderdetail.Price = mydb.ProductTBL.Find(ProductID).ProductPrice;
                            myorderdetail.OrderIDD = ORDERID;
                            mydb.OrderDetailTBL.Add(myorderdetail);
                            mydb.SaveChanges();
                        }

                    }
                    else
                    {
                        HttpRequestBase request1 = Request;
                        string clientIP = GetClientMacAndIP.GetClientIpAddress(request1);

                        OrderTBL myorder = new OrderTBL();
                        myorder.OrderAutoCode = MySPECIALGUID.GetUniqueKey(6);
                        myorder.DateOfMaking = DateTime.Now;
                        myorder.UserIP = clientIP;
                        myorder.IsDeleted = false;
                        myorder.IsCompleted = false;
                        var CurrentOrder = mydb.OrderTBL.Add(myorder);
                        mydb.SaveChanges();
                        Session["MyCurrentOrder"] = CurrentOrder.OrderID;
                        ORDERID = CurrentOrder.OrderID;
                        OrderDetailTBL myorderdetail = new OrderDetailTBL();
                        myorderdetail.IsDeleted = false;
                        myorderdetail.ProductIDD = ProductID;
                        myorderdetail.Unit = 1;
                        myorderdetail.Price = mydb.ProductTBL.Find(ProductID).ProductPrice;
                        myorderdetail.OrderIDD = CurrentOrder.OrderID;
                        mydb.OrderDetailTBL.Add(myorderdetail);
                        mydb.SaveChanges();

                    }
                }

                var data = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID).ToList();
                int totalprice = 0;
                foreach (var item in data)
                {
                    totalprice += Convert.ToInt32(item.Price * item.Unit);
                }
                ViewBag.totalprice = totalprice;
                return View(data);
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cart(int ProductID = 0,  int deleteitem = 0)
        {
            try
            {
                int ORDERID = 0;
                if (Session["MyCurrentOrder"] != null)
                {
                    ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());

                }
                var CurrentOrderDetail = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID && a.ProductIDD == ProductID).SingleOrDefault();

                if (deleteitem == 1)
                {
                    CurrentOrderDetail.IsDeleted = true;
                }
                mydb.Entry(CurrentOrderDetail).State = EntityState.Modified;
                mydb.SaveChanges();
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddOrLessCurrentCartItems(int ProductID = 0, int addmore = 0, int addless = 0)
        {
            ReturnResultJSONCart result = new ReturnResultJSONCart();
            int ORDERID = 0;
            if (Session["MyCurrentOrder"] != null)
            {
               ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());
            }
            var CurrentOrderDetail = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID && a.ProductIDD == ProductID).SingleOrDefault();
            if (addmore == 1)
            {
                CurrentOrderDetail.Unit++;
            }
            else if (addless == 1)
            {
                CurrentOrderDetail.Unit--;
            }
            if (CurrentOrderDetail.Unit < 1)
            {
                CurrentOrderDetail.Unit = 1;
            }
            mydb.Entry(CurrentOrderDetail).State = EntityState.Modified;
            mydb.SaveChanges();

            var data = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID).ToList();
            int totalprice = 0;
            foreach (var item in data)
            {
                totalprice += Convert.ToInt32(item.Price * item.Unit);
            }
            result.TotalPrice = totalprice;
            result.Units = (int)CurrentOrderDetail.Unit;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCurrentCartItems(string GetCount = null)
        {
            int result = 0;
            if (GetCount == "Cart")
            {
                int ORDERID = 0;
                if (Session["MyCurrentOrder"] != null)
                {
                    ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());
                }
                var data = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID).ToList();
                result = data.Count();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OrderNow

        [HttpGet]
        [AllowAnonymous]
        [CheckSessionUsronline]
        public ActionResult OrderNow()
        {
            try
            {
                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/238-2388804_order-now-transparent-hd-png-download.png";
                // END SEO

                SEO_Metakeywords_Final += "'طلب شراء";
                SEO_MetaDescription_Final += "طلب شراء";

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/OrderNow";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO
                int ORDERID = 0;
                if (Session["MyCurrentOrder"] != null)
                {
                    ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());
                }
                var data = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID).ToList();
                int totalprice = 0;
                foreach (var item in data)
                {
                    totalprice += Convert.ToInt32(item.Price * item.Unit);
                }
                ViewBag.totalprice = totalprice;
                if (totalprice < 200)
                {
                    return RedirectToAction("InvalidNotAllowed");
                }
                ViewBag.OrderAutoCode = mydb.OrderTBL.Find(ORDERID).OrderAutoCode;
                return View();
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderNow(string name1, string Phone1, string Govenorate1, string City1, string Address1)
        {
            try
            {
                int ORDERID = 0;
                if (Session["MyCurrentOrder"] != null)
                {
                    ORDERID = Convert.ToInt32(Session["MyCurrentOrder"].ToString());
                }
                if (ORDERID > 0)
                {
                    var myorder = mydb.OrderTBL.Find(ORDERID);
                    myorder.UserName = name1;
                    myorder.UserPhone = Phone1;
                    myorder.UserGovernorate = Govenorate1;
                    myorder.UserCity = City1;
                    myorder.UserAddress = Address1;
                    myorder.DateOfMaking = DateTime.Now;
                    myorder.IsCompleted = true;
                    mydb.Entry(myorder).State = EntityState.Modified;
                    mydb.SaveChanges();
                    //

                    var data = mydb.OrderTBL.Find(ORDERID);
                    var orderdetails = mydb.OrderDetailTBL.Where(a => a.IsDeleted == false && a.OrderIDD == ORDERID).ToList();
                    int totalprice = 0;

                    string productsInRows = string.Empty;
                    int mycounter = 1;
                    foreach (var item in orderdetails)
                    {
                        string productsInRowFile = string.Empty;
                        var root1 = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(Server.MapPath(@"~/Content/MyOrderDetail.txt")))
                        {
                            string readFile = reader.ReadToEnd();
                            string StrContent = string.Empty;
                            StrContent = readFile;

                            StrContent = StrContent.Replace("[Counter]", mycounter.ToString());
                            StrContent = StrContent.Replace("[ProductAutoCode]", item.ProductTBL.ProductAutoCode);
                            StrContent = StrContent.Replace("[ProductNameAR]", item.ProductTBL.ProductNameAR);
                            StrContent = StrContent.Replace("[itemPrice]", item.Price.ToString());
                            StrContent = StrContent.Replace("[itemUnit]", item.Unit.ToString());
                            StrContent = StrContent.Replace("[itemUnititemPrice]", Convert.ToInt32(item.Unit * item.Price).ToString() );
                            productsInRowFile = StrContent.ToString();
                            productsInRows += productsInRowFile;
                        }
                        mycounter++;
                        totalprice += Convert.ToInt32(item.Price * item.Unit);
                    }
                    //

                    string emailto = "ayman_Elbatata@outlook.com";
                    string msgsubject = "Technical Support Department, A New Order-Request, ("+WebsiteCompanyTXT+")";

                    string body = string.Empty;
                    var root = AppDomain.CurrentDomain.BaseDirectory; using (var reader = new System.IO.StreamReader(Server.MapPath(@"~/Content/OrderNow.html")))
                    {
                        string readFile = reader.ReadToEnd();
                        string StrContent = string.Empty;
                        StrContent = readFile;
                        //Assing the field values in the template
                        StrContent = StrContent.Replace("[OrderAutoCode]", data.OrderAutoCode);
                        StrContent = StrContent.Replace("[OrderDateTime]", data.DateOfMaking.ToString());
                        StrContent = StrContent.Replace("[totalprice]", totalprice.ToString());
                        StrContent = StrContent.Replace("[ClientIP]", data.UserIP);
                        StrContent = StrContent.Replace("[UserName]", data.UserName);
                        StrContent = StrContent.Replace("[UserPhone]", data.UserPhone);
                        StrContent = StrContent.Replace("[UserGovernorate]", data.UserGovernorate);
                        StrContent = StrContent.Replace("[UserCity]", data.UserCity);
                        StrContent = StrContent.Replace("[UserAddress]", data.UserAddress);
                        StrContent = StrContent.Replace("[MyRowWithData]", productsInRows);
                        body = StrContent.ToString();
                    }
                    SendMessageToEmail.sendemailtosomeone(emailto, msgsubject, body);
                    Session["MyCurrentOrder"] = null;
                    return RedirectToAction("Index", new { type = 2});
                }

                return View();
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }
        #endregion

        #region LOGIN & LOGOUT
        [HttpGet]
        [AllowAnonymous]
        [CheckSessionUsronline]
        public ActionResult Login(int recopass = 0)
        {
            try
            {
                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/lifeegypt2.png";
                // END SEO
                SEO_Metakeywords_Final += "تسجيل دخول";
                SEO_MetaDescription_Final += "تسجيل دخول مستخدم";
                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/Login";
                ViewBag.OgImageURL = WebsiteCompanyTXT + "/Images/Main/mobile-login-concept-illustration_114360-83.jpg";
                // END SEO


                if (recopass == 1)
                {
                    ViewBag.Successful = "تم إرسال كلمة السر إلى بريدك الإلكترونى بنجاح";
                }
                return View();
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }
        }


        [HttpPost]
        [CheckSessionUsronline]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string loginmail = null, string loginpassword = null, string ReturnUrl = null)
        {
            try
            {

                if (!string.IsNullOrEmpty(loginmail) && !string.IsNullOrEmpty(loginpassword))
                {
                    bool user = userexists(loginmail, loginpassword);
                    if (user)
                    {
                        //.

                        var lst = mydb.UserTBL.Where(a => a.UserEmail == loginmail).SingleOrDefault();
                        if (lst.IsStopped == true)
                        {
                            ViewBag.message = "الحساب موقوف";
                            return View();
                        }
                        string MySession = startlogin(lst.UserID);
                        FormsAuthentication.SetAuthCookie(MySession, false);

                        string decodedUrl = "";
                        if (!string.IsNullOrEmpty(ReturnUrl))
                            decodedUrl = Server.UrlDecode(ReturnUrl);

                        //Login logic...

                        if (Url.IsLocalUrl(decodedUrl))
                        {
                            return Redirect(decodedUrl);
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "PRVEMP", new { Area = "" });
                        }

                    }
                }
                ViewBag.message = "غير مسجل";
                return View();
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }

        }

        //
        private bool userexists(string email, string password)
        {

            var lst = mydb.UserTBL.Where(a => a.UserEmail == email && a.UserPassword == password).Count();

            if (lst > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //

        //
        private string startlogin(int EmployeeID)
        {

            UserLoginTBL mymodel = new UserLoginTBL();
            mymodel.UserIDD = EmployeeID;
            mymodel.LoginTime = DateTime.Now;
            mymodel.LogoutTime = null;
            HttpRequestBase request1 = Request;
            mymodel.LoginIPAddress = GetClientMacAndIP.GetClientIpAddress(request1);
            mymodel.SessionUnique = MySPECIALGUID.GenerateGUID(151, 101);
            mydb.UserLoginTBL.Add(mymodel);
            mydb.SaveChanges();
            return mymodel.SessionUnique;
        }
        //


        [HttpGet]
        [Authorize]
        public ActionResult LogOut()
        {
            try
            {

                string currentsession = CurrentUserLoginData.getcurrentusrdataAuth(7);

                if (currentsession != null && currentsession != "0")
                {
                    var session = mydb.UserLoginTBL.Where(a => a.SessionUnique == currentsession).SingleOrDefault();

                    session.LogoutTime = DateTime.Now;

                    mydb.Entry(session).State = EntityState.Modified;
                }

                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }

        }
        //
        #endregion

        #region Forgot Password
        // RESET PASSWORD BY EMAIL
        [HttpGet]
        [AllowAnonymous]
        [CheckSessionUsronline]
        public ActionResult ForgotPassword()
        {
            try
            {

                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/mobile-login-concept-illustration_114360-83.jpg";
                SEO_Metakeywords_Final += "إسترجاع كلمة السر";
                SEO_MetaDescription_Final += "إسترجاع كلمة السر";
                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/ForgotPassword";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO
                // END SEO
                return View();

            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }
        }


        [HttpPost]
        [CheckSessionUsronline]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string loginmail = null)
        {
            try
            {

                if (!string.IsNullOrEmpty(loginmail))
                {
                    var userexist = mydb.UserTBL.Where(a => a.UserEmail == loginmail).Count();
                    if (userexist > 0)
                    {

                        var EMPDetail = mydb.UserTBL.Where(a => a.UserEmail == loginmail).SingleOrDefault();

                        if (EMPDetail.IsDeleted == true)
                        {
                            ViewBag.message = "الحساب موقوف";
                            return View();
                        }
                        else
                        {
                            HttpRequestBase request1 = Request;
                            string clientIP = GetClientMacAndIP.GetClientIpAddress(request1);

                            string emailto = EMPDetail.UserEmail;
                            string msgsubject = "Technical Support Department, Recovering Your Password, ("+WebsiteCompanyTXT+")";
                            string msgcontent = "Hello " + EMPDetail.UserFullName + ", " + " Due to your request, You Asked to recover your password for your account which in our Website. \n \n Your Email is: " + EMPDetail.UserEmail + "\n \n Your Password: " + EMPDetail.UserPassword + "\n \n Date of Making : " + DateTime.Now + "\n \n Client IP : " + clientIP + ". \n \n Now you can come back to our website and login easily.";
                            SendMessageToEmail.sendemailtosomeone(emailto, msgsubject, msgcontent);
                            return RedirectToAction("Login", "Home", new { Area = "", recopass = 1 });
                        }
                    }
                    else
                    {
                        ViewBag.message = "غير موجود";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "حدث خطأ";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return RedirectToAction("ErrorDesc");
            }

        }

        #endregion

        #region ContactUS
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionUsronline]
        public ActionResult ContactUS(string name1CTN = null, string Phone1CTN = null, string message1CTN = null, string PVTNum = null)
        {
            try
            {
                HttpRequestBase request1 = Request;
                string clientIP = GetClientMacAndIP.GetClientIpAddress(request1);

                string IncomingMyhash = HashMD5AYM.GenerateNewPasswordMD5("name14Test" + PVTNum + "message14Test");
                string MySecretHash = "C2027B4FE206DB9E7EC9A809D03AEBA3";

                ReturnResultJSON result = new ReturnResultJSON();
                if (!string.IsNullOrEmpty(name1CTN) && !string.IsNullOrEmpty(Phone1CTN) && !string.IsNullOrEmpty(message1CTN) && IncomingMyhash == MySecretHash)
                {
                    ContactUsTBL myobject = new ContactUsTBL();
                    myobject.DateOfMaking = DateTime.Now;
                    myobject.UserPhone = Phone1CTN;
                    myobject.UserName = name1CTN;
                    myobject.UserMessage = message1CTN;
                    myobject.IsDeleted = false;
                    myobject.UserIP = clientIP;
                    var data = mydb.ContactUsTBL.Add(myobject);
                    mydb.SaveChanges();

                    //string emailto = "lifegyptofficial@gmail.com";
                    string emailto = "ayman.fathy.elbatata@gmail.com";
                    string msgsubject = "Technical Support Department, A New Message-ContactUS, (https://www.HomeStyleEG.com.com)";
                    string msgcontent = "Hello " + "Mr Mahmoud" + ", " + " according to your request, Someone Sent a new message to your Website. \n \n His/Her Name is: " + data.UserName + "\n \n His/Her Phone is: " + data.UserPhone + "\n \n His/Her Message: " + message1CTN + "\n \n Date of Making : " + data.DateOfMaking + "\n \n Client IP : " + data.UserIP;
                    SendMessageToEmail.sendemailtosomeone(emailto, msgsubject, msgcontent);
                    result.Type = "OK";
                    result.Message = "تم إرسال الرسالة بنجاح";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Type = "NO";
                    result.Message = "حدث خطأ برجاء المحاولة مرة آخرى";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session["myerrordesc"] = ex;
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region InvalidNotExists & InvalidNotAllowed & ErrorDesc

        [HttpGet]
        [AllowAnonymous]
        public ActionResult InvalidNotExists()
        {
            try
            {
                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/error-404.png";
                // END SEO

                SEO_Metakeywords_Final += "غير موجود";
                SEO_MetaDescription_Final += "غير موجود";

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/InvalidNotExists";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO

                return View();
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult InvalidNotAllowed()
        {
            try
            {
                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/88181025-forbidden.jpg";
                // END SEO

                SEO_Metakeywords_Final += "غير متاح";
                SEO_MetaDescription_Final += "غير متاح";

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/InvalidNotAllowed";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO

                return View();
            }
            catch (Exception ex)
            {

                Session["myerrordesc"] = ex.InnerException;
                return RedirectToAction("ErrorDesc");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ErrorDesc()
        {

                // START SEO
                string SEO_MetaDescription_Final = "";
                string SEO_Metakeywords_Final = "";
                string SEO_OgImageURL = WebsiteCompanyTXT + "/Images/Main/remove-from-database-icon.png";
                // END SEO

                SEO_Metakeywords_Final += "حدث خطأ";
                SEO_MetaDescription_Final += "حدث خطأ";

                // START SEO
                ViewBag.MetaDescription = SEO_MetaDescription_Final;
                ViewBag.Metakeywords = SEO_Metakeywords_Final;
                ViewBag.OgTitle = SEO_Metakeywords_Final;
                ViewBag.Ogdescription = SEO_MetaDescription_Final;
                ViewBag.OgURL = WebsiteCompanyTXT + "/Home/ErrorDesc";
                ViewBag.OgImageURL = SEO_OgImageURL;
                // END SEO

                return View();
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