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
                    ID = obj.ID,
                    ISPDF = obj.isPDF
                });
            }

            return resp;
        }

        public static bool Delete(int ID)
        {
            var respObj = false;
            if (ID > 0)
            {
                return new TMCDBContext().fn_DeleteGiveAway(ID);
            }
            return respObj;
        }

        public static bool Accept(int ID)
        {
            var respObj = false;
            if (ID > 0)
            {
                return new TMCDBContext().fn_AcceptGiveAway(ID);
            }
            return respObj;
        }

        public static List<giveawayViewModel> getAll(int ID = 0, int city = 0, string searchTxt = "", int isPDF = 0)
        {
            var respObj = new List<giveawayViewModel>();
            try
            {
                respObj = new TMCDBContext().fn_getallGiveaways(ID, city, searchTxt, isPDF).Select(x => new giveawayViewModel()
                {
                    CITY = new TMCDBContext().fn_GetSingleCityByID(x.CITY).CITY,
                    DATECREATED = x.DATECREATED.ToString("dddd, dd MMMM yyyy"),
                    ENTEREDBY = x.ENTEREDBY,
                    ID = x.ID,
                    ISACCEPTED = x.ISACCEPTED,
                    OBJTITLE = x.OBJTITLE,
                    OBJPICTURES = Slider.GetCommaSeparated(x.ID, SliderObjectType.GiveAway)
                }).ToList();
            }
            catch { respObj = new List<giveawayViewModel>(); }
            return respObj;
        }

        public static giveawayViewModel getByID(int ID = 0)
        {
            var respObj = new giveawayViewModel();
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
                }).FirstOrDefault();
            }
            catch { respObj = new giveawayViewModel(); }
            return respObj;
        }
    }
}
