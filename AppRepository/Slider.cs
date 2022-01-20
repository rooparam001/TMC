using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Linq;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class Slider
    {
        public static bool SaveSlider(slider_inputoutputmodel obj)
        {
            bool resp = false;
            foreach (string currImage in obj.OBJECTURL)
            {
                if (!string.IsNullOrEmpty(currImage))
                    resp = new TMCDBContext().fn_SaveSlider(new TBL_SLIDERMASTER()
                    {
                        OBJECTID = obj.OBJECTID,
                        OBJECTTYPE = (int)obj.ObjectType,
                        OBJECTURL = currImage,
                        DATECREATED = System.DateTime.Now
                    });
            }
            return resp;
        }

        public static bool DelSlider(int ObjID)
        {
            bool resp = false;
            resp = new TMCDBContext().fn_DelSlider(ObjID);
            return resp;
        }
        public static string GetCommaSeparated(int ObjectID, SliderObjectType objectType)
        {
            var respObj = "";
            if (ObjectID > 0)
            {
                respObj = (string.Join(",", new TMCDBContext().TBL_SLIDERMASTER.Where(x => x.OBJECTID == ObjectID && x.OBJECTTYPE == (int)objectType).Select(y => y.OBJECTURL.ToString())));
            }
            return respObj;
        }
    }
}
