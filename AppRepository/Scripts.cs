using TMC.EntitesInterfaces.AppModels;
using TMC.EntitesInterfaces.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMC.AppRepository;
using TMC.DBConnections;

namespace TMC.AppRepository
{
    public class Scripts
    {
        public static TBL_SCRIPTSMASTER Save(scriptsViewModel obj)
        {
            var resp = new TBL_SCRIPTSMASTER();

            if (obj != null)
            {
                resp = new TMCDBContext().fn_SaveScripts(new TBL_SCRIPTSMASTER()
                {
                    CITY = (!string.IsNullOrEmpty(obj.CITY) ? new TMCDBContext().fn_SaveCity(obj.CITY.Trim()) : 0),
                    DATECREATED = System.DateTime.Now,                   
                    ISENABLE = obj.ISENABLE,                   
                    OBJTITLE = obj.OBJTITLE.Trim(),
                    ID = obj.ID,
                    ISPDF = obj.isPDF,
                    CREDITSLINK = obj.CREDITSLINK.Trim(),
                    CREDITSTITLE = obj.CREDITSTITLE.Trim(),
                    WRITERNAME = obj.WRITERNAME.Trim()
                });
            }

            return resp;
        }


        public static bool Delete(int ID)
        {
            var respObj = false;
            if (ID > 0)
            {
                return new TMCDBContext().fn_DeleteScripts(ID);
            }
            return respObj;
        }

       

        public static List<scriptsViewModel> getAll(int ID = 0, int city = 0, string searchTxt = "", int isPDF = 0)
        {
            var respObj = new List<scriptsViewModel>();
            try
            {
                respObj = new TMCDBContext().fn_getallScripts(ID, city, searchTxt, isPDF).Select(x => new scriptsViewModel()
                {
                    CITY = new TMCDBContext().fn_GetSingleCityByID(x.CITY).CITY,
                    DATECREATED = x.DATECREATED.ToString("dddd, dd MMMM yyyy"),
                    ENTEREDBY = x.ENTEREDBY,
                    ID = x.ID,                   
                    OBJTITLE = x.OBJTITLE,
                    CREDITSLINK = x.CREDITSLINK,
                    CREDITSTITLE = x.CREDITSTITLE,
                    OBJPICTURES = Slider.GetCommaSeparated(x.ID, SliderObjectType.Scripts),
                    WRITERNAME = x.WRITERNAME
                }).ToList();
            }
            catch { respObj = new List<scriptsViewModel>(); }
            return respObj;
        }

        public static scriptsViewModel getByID(int ID = 0)
        {
            var respObj = new scriptsViewModel();
            try
            {
                respObj = new TMCDBContext().fn_getallScripts(ID: ID).Select(x => new scriptsViewModel()
                {
                    CITY = new TMCDBContext().fn_GetSingleCityByID(x.CITY).CITY,
                    DATECREATED = x.DATECREATED.ToString("dddd, dd MMMM yyyy"),
                    ENTEREDBY = x.ENTEREDBY,
                    ID = x.ID,                    
                    ISENABLE = x.ISENABLE,                   
                    OBJTITLE = x.OBJTITLE,
                    CREDITSLINK = x.CREDITSLINK,
                    CREDITSTITLE = x.CREDITSTITLE,
                    OBJPICTURES = Slider.GetCommaSeparated(ID, SliderObjectType.Scripts),
                    WRITERNAME = x.WRITERNAME
                }).FirstOrDefault();
            }
            catch { respObj = new scriptsViewModel(); }
            return respObj;
        }
    }
}

