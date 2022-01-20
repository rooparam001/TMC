using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using System.Linq;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public static class GiveAways
    {
        public static TBL_GIVEAWAYMASTER Save(giveawayViewModel obj)
        {
            var resp = new TBL_GIVEAWAYMASTER();

            if (obj != null)
            {
                resp = new TMCDBContext().fn_SaveGiveaway(new TBL_GIVEAWAYMASTER()
                {
                    CITY = (!string.IsNullOrEmpty(obj.CITY) ? new TMCDBContext().fn_SaveCity(obj.CITY.Trim()) : 0),
                    DATECREATED = System.DateTime.Now,
                    ISACCEPTED = obj.ISACCEPTED,
                    ISENABLE = obj.ISENABLE,
                    OBJAVAILABILITY = obj.OBJAVAILABILITY.Trim(),
                    OBJCONTACTDETAILS = obj.OBJCONTACTDETAILS.Trim(),
                    OBJTITLE = obj.OBJTITLE.Trim(),
                    ID = obj.ID
                });
            }

            return resp;
        }

        public static bool Delete(int ID)
        {
            var respObj = false;
            if (ID > 0)
            {
                var obj = new TBL_GIVEAWAYMASTER();
                obj = new TMCDBContext().fn_getallGiveaways(ID).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.ID > 0)
                    {
                        obj.ISENABLE = false;
                        new TMCDBContext().fn_SaveGiveaway(obj);
                        respObj = true;
                    }
                }
            }
            return respObj;
        }

        public static List<giveawayViewModel> getAllOrByID(int ID = 0)
        {
            var respObj = new List<giveawayViewModel>();
            try
            {
                respObj = new TMCDBContext().fn_getallGiveaways(ID: ID).Select(x => new giveawayViewModel()
                {
                    CITY = new TMCDBContext().fn_GetSingleCityByID(x.CITY).CITY,
                    DATECREATED = x.DATECREATED.ToString("dddd, dd MMMM yyyy"),
                    ENTEREDBY = x.ENTEREDBY,
                    ID = x.ID,
                    ISACCEPTED = x.ISACCEPTED,
                    ISENABLE = x.ISENABLE,
                    OBJAVAILABILITY = x.OBJAVAILABILITY,
                    OBJCONTACTDETAILS = x.OBJCONTACTDETAILS,
                    OBJTITLE = x.OBJTITLE,
                    OBJPICTURES = Slider.GetCommaSeparated(ID, SliderObjectType.GiveAway)
                }).ToList();
            }
            catch { respObj = new List<giveawayViewModel>(); }
            return respObj;
        }
    }
}
