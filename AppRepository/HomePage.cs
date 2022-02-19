using EntitesInterfaces.DBEntities;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class HomePage
    {
        public static bool SaveSetting(TBL_HOMEPAGESETTINGS obj)
        {
            bool resp = false;

            if (obj != null)
                if (!string.IsNullOrEmpty(obj.OBJECTDESCRIPTION) && !string.IsNullOrEmpty(obj.OBJECTTITLE))
                    resp = new TMCDBContext().fn_SaveHomePageSetting(obj);

            return resp;
        }
        public static bool DeleteSetting(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteHomePageSetting(objID);

            return resp;
        }
    }
}
