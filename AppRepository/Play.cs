using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using TMC.DBConnections;
using System.Linq;
using System.Globalization;

namespace TMC.AppRepository
{
    public static class Play
    {
        public static bool SaveUpcomingPlay(TBL_UPCOMINGPLAYS obj)
        {
            bool resp = false;

            if (obj != null)
                resp = new TMCDBContext().fn_SaveUpcomingPlay(obj);

            return resp;
        }
        public static bool DeleteUpcomingPlay(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteUpcomingPlay(objID);

            return resp;
        }
        public static bool DeleteAllUpcomingPlay()
        {
            return new TMCDBContext().fn_DeleteAllPlays();
        }
        public static List<TBL_UPCOMINGPLAYS> fn_GetAllPlays()
        {
            return new TMCDBContext().fn_GetAllPlays();
        }

        public static TBL_PLAYSMASTER fn_SavePlay(TBL_PLAYSMASTER obj)
        {
            var resp = new TBL_PLAYSMASTER();

            if (obj != null)
                resp = new TMCDBContext().fn_SavePlay(obj);

            return resp;
        }
        public static bool DeletePlay(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeletePlay(objID);

            return resp;
        }
        public static bool fn_DeleteAllExistingPlays()
        {
            return new TMCDBContext().fn_DeleteAllExistingPlays();
        }
        public static List<TBL_PLAYSMASTER> fn_GetAllExistingPlays()
        {
            return new TMCDBContext().fn_GetAllExistingPlays();
        }
    }
}
