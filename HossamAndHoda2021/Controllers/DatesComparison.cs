using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HossamAndHoda2021.Controllers
{
    public class DatesComparison
    {

        public static bool DateEqualsORLessThanDate2(DateTime dateMin1, DateTime dateMax2, DateTime dateRow3)
        {
            
            if (dateMin1.Date == dateRow3.Date)
            {
               return true;
            }
            else if (dateMax2.Date == dateRow3.Date)
            {
                return true;
            }
            if (dateMin1 < dateRow3)
            {
                if (dateMax2 > dateRow3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool Date1LessThanDate2(DateTime date1, DateTime date2)
        {

            if (date1.Date == date2.Date)
            {
                return true;
            }
            else if (date1 < date2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}