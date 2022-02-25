using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class Enquiries
    {
        public static bool Save(TBL_ENQUIRYMASTER obj)
        {
            bool resp = false;

            if (obj != null)
                if (!string.IsNullOrEmpty(obj.EMAILADD) && !string.IsNullOrEmpty(obj.USERMESSAGE))
                    resp = new TMCDBContext().fn_SaveEnquiry(obj);

            return resp;
        }
        public static List<inquiryViewModel> fn_getallUnSeenEnquiries(int ID = 0)
        {
            var listObj = new TMCDBContext().fn_getallUnSeenEnquiries(ID);
            var resp = new List<inquiryViewModel>();
            resp = listObj.Select(x => new inquiryViewModel()
            {
                DATECREATED = x.DATECREATED.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                EMAILADD = x.EMAILADD,
                USERMESSAGE = x.USERMESSAGE,
                USERNAME = x.USERNAME,
                USERSUBJECT = x.USERSUBJECT
            }).ToList();
            return resp;
        }
    }
}
