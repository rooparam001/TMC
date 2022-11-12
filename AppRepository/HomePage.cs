using TMC.EntitesInterfaces.DBEntities;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class HomePage
    {
        public static TBL_HOMEPAGESETTINGS Save(TBL_HOMEPAGESETTINGS obj)
        {
            if (obj != null)
                if (!string.IsNullOrEmpty(obj.OBJECTTITLE) && obj.OBJECTTYPE > 0)
                    obj = new TMCDBContext().fn_SaveHomePageSetting(obj);

            return obj;
        }
        public static bool Delete(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteHomePageSetting(objID);

            return resp;
        }
    }
}
