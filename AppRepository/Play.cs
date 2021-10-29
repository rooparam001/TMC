using EntitesInterfaces.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class Play
    {
        public static bool SaveUpcomingPlay(TBL_UPCOMINGPLAYS obj)
        {
            bool resp = false;

            if (obj != null)
            {
                resp = new TMCDBContext().fn_SaveUpcomingPlay(obj);
            }
            return resp;
        }
        public static bool DeleteUpcomingPlay(int objID)
        {
            bool resp = false;

            if (objID > 0)
            {
                resp = new TMCDBContext().fn_DeleteUpcomingPlay(objID);
            }
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
    }
}
