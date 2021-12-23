using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using System.Linq;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public class Director
    {
        public static bool SaveDirectors(TBL_DIRECTORMASTER obj)
        {
            bool resp = false;

            if (obj != null)
                if (!string.IsNullOrEmpty(obj.OBJECTNAME))
                    resp = new TMCDBContext().fn_SaveDirectors(obj);

            return resp;
        }
        public static bool DeleteDirectors(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteDirectors(objID);

            return resp;
        }
        public static List<TBL_DIRECTORMASTER> GetAllDirectors(int ID = 0)
        {
            return new TMCDBContext().fn_getallDirectors();
        }

        public static directorModel GetSingleDirector_ByID(int ID)
        {
            return new TMCDBContext().fn_getallDirectors(ID).Select(x => new directorModel()
            {
                ID = x.ID,
                ImageURL = x.OBJECTIMGURL,
                Title = x.OBJECTNAME,
                DateCreated = x.DATECREATED.ToString(),
                Description = x.OBJECTDESCRIPTION
            }).FirstOrDefault();
        }
    }
}
