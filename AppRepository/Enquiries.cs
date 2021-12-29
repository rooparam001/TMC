using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
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
        public static List<TBL_ENQUIRYMASTER> fn_getallUnSeenEnquiries(int ID = 0)
        {
            return new TMCDBContext().fn_getallUnSeenEnquiries(ID);
        }
    }
}
