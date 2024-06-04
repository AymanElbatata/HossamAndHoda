using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using HossamAndHoda2021.Models;

namespace HossamAndHoda2021.Controllers
{
    public static class CurrentUserLoginData
    {

        private static HossamAndHoda2021DBEntities mydb = new HossamAndHoda2021DBEntities();


        public static string getcurrentusrdataAuth(int type)
        {
            string result = "0";
            string currentsession = HttpContext.Current.User.Identity.Name;
            if (currentsession == "")
            {
                return result;
            }
            var usr = getcurrentuserData(currentsession);

            // User ID
            if (type == 1 && usr.UserID > 0)
            {
                result = usr.UserID.ToString();
            }
            // User Email
            else if (type == 3 && usr.UserID > 0)
            {
                result = usr.UserEmail;
            }
            else if (type == 4 && usr.UserID > 0)
            {
                result = usr.UserFullName;
            }

            else if (type == 7 && usr.UserID > 0)
            {
                result = currentsession;
            }
            return result;
        }

        private static UserTBL getcurrentuserData(string currentsession = null)
        {
            var usr = new UserTBL();

            if (!string.IsNullOrEmpty(currentsession))
            {
                var MYSESSION = mydb.UserLoginTBL.Where(a => a.SessionUnique == currentsession).SingleOrDefault();
                usr = mydb.UserTBL.Where(a => a.UserID == MYSESSION.UserIDD).SingleOrDefault();
            }
            return usr;
        }

        public static int UnitsAndPrice ( int units, int price)
        {
            return units * price;
        }

    }
}