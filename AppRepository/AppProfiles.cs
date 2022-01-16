using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System.Collections.Generic;
using System.Linq;
using TMC.DBConnections;
using TMC.Models;

namespace TMC.AppRepository
{
    public static class AppProfiles
    {
        public static ajaxResponse SaveProfiles(profileMasterViewModel obj)
        {
            var resp = new ajaxResponse();

            try
            {
                if (obj != null)
                    if (!string.IsNullOrEmpty(obj.USERTITLE.Trim()))
                    {
                        var profileObj = new TBL_PROFILESMASTER()
                        {
                            DATECREATED = obj.DATECREATED,
                            ISENABLE = obj.ISENABLE,
                            USERAWARDS = obj.USERAWARDS,
                            USERCERTIFICATES = obj.USERCERTIFICATES,
                            USERDEGREEURL = obj.USERDEGREEURL,
                            USEREMAIL = obj.USEREMAIL,
                            USERFLDOFEXCELLENCE = obj.USERFLDOFEXCELLENCE,
                            USERLETTEROFREF = obj.USERLETTEROFREF,
                            USERPRVWORKEXP = obj.USERPRVWORKEXP,
                            USERTITLE = obj.USERTITLE,
                            USERTOTALEXPINYEARS = obj.USERTOTALEXPINYEARS,
                            USERUPLOADEDWORK = obj.USERUPLOADEDWORK
                        };

                        //setting user's languages
                        if (!string.IsNullOrEmpty(obj.USERLANGUAGES))
                        {
                            string _strLanguage = "";
                            foreach (var currLang in obj.USERLANGUAGES.Split(new char[] { ',', ';', '/' }, System.StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (!string.IsNullOrEmpty(currLang))
                                {
                                    _strLanguage += new TMCDBContext().fn_SaveLanguage(currLang.Trim()).ToString() + ",";
                                }
                            }
                            if (_strLanguage.StartsWith(","))
                                _strLanguage = _strLanguage.Substring(1, _strLanguage.Length);
                            while (_strLanguage.EndsWith(","))
                                _strLanguage = _strLanguage.Substring(0, _strLanguage.Length - 1);
                            if (!string.IsNullOrEmpty(_strLanguage))
                                profileObj.USERLANGUAGES = _strLanguage;
                        }

                        //setting user's city
                        if (!string.IsNullOrEmpty(obj.USERCITYID))
                        {
                            profileObj.USERCITYID = new TMCDBContext().fn_SaveCity(obj.USERCITYID.Trim());
                        }

                        //setting user's role
                        if (!string.IsNullOrEmpty(obj.USERROLE))
                        {
                            profileObj.USERROLE = new TMCDBContext().fn_SaveRole(obj.USERROLE).ID;
                        }
                        //checking if profile already exists
                        if (!new TMCDBContext().fn_ProfileVerifyifExists(profileObj))
                        {
                            //saving the final response
                            if (new TMCDBContext().fn_SaveProfiles(profileObj))
                            {
                                resp.respmessage = "Profile saved";
                                resp.respstatus = ResponseStatus.success;
                            }
                        }
                        else
                        {
                            resp.respmessage = "Profile already exists";
                            resp.respstatus = ResponseStatus.success;
                        }
                    }
            }
            catch
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = "Something went wrong",
                    respstatus = ResponseStatus.error
                };
            }

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
