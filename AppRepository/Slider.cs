using TMC.EntitesInterfaces.AppModels;
using TMC.EntitesInterfaces.DBEntities;
using System.Linq;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class Slider
    {
        public static bool Save(slider_inputoutputmodel obj)
        {
            bool resp = false;
            foreach (var currSlider in obj.SliderLst)
            {
                if (!string.IsNullOrEmpty(currSlider.SliderImgURL))
                    resp = new TMCDBContext().fn_SaveSlider(new TBL_SLIDERMASTER()
                    {
                        OBJECTID = obj.OBJECTID,
                        OBJECTTYPE = (int)obj.ObjectType,
                        OBJECTURL = currSlider.SliderImgURL,
                        OBJECTDESCRIPTION = currSlider.Description.Trim(),
                        DATECREATED = System.DateTime.Now
                    });
            }
            return resp;
        }

        public static bool Delete_ByObjectID(int ObjID)
        {
            bool resp = false;
            resp = new TMCDBContext().fn_DelSlider(ObjID);
            return resp;
        }
        public static bool Delete_ByID(int ObjID)
        {
            bool resp = false;
            resp = new TMCDBContext().fn_DelSlider(ObjID, false);
            return resp;
        }
        public static string GetCommaSeparated(int ObjectID, SliderObjectType objectType)
        {
            var respObj = "";
            respObj = (string.Join(",", new TMCDBContext().TBL_SLIDERMASTER.Where(x => (ObjectID > 0 ? x.OBJECTID == ObjectID : true) && x.OBJECTTYPE == (int)objectType).Select(y => y.OBJECTURL.ToString())));
            return respObj;
        }
        public static sliderListViewModel GetCommaSeparated_ListModel(int ObjectID, SliderObjectType objectType)
        {
            var respObj = new sliderListViewModel();
            try
            {
                respObj.lst = new TMCDBContext().TBL_SLIDERMASTER.Where(x => (ObjectID > 0 ? x.OBJECTID == ObjectID : true) && x.OBJECTTYPE == (int)objectType).Select(y => new sliderViewModel()
                {
                    ID = y.ID,
                    SliderImgURL = y.OBJECTURL,
                    Description = y.OBJECTDESCRIPTION,
                    Title = y.OBJECTTITLE
                }).ToList();
            }
            catch { respObj = new sliderListViewModel(); }

            return respObj;
        }
    }
}
