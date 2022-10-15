using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                            DATECREATED = DateTime.Now,
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
                            USERUPLOADEDWORK = obj.USERUPLOADEDWORK,
                            PROFILETYPEOF = obj.PROFILETYPEOF,
                            USERGENDER = obj.USERGENDER,
                            USERAGE = obj.USERAGE,
                            PROFILEPICTURE = obj.ImageURL,
                            ACCOUNTID = obj.AccountID,
                            ID = obj.ID,
                            WORKPROFILE = obj.WORKPROFILE
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
                        if (!string.IsNullOrEmpty(obj.USERCITY))
                        {
                            profileObj.USERCITYID = new TMCDBContext().fn_SaveCity(obj.USERCITY.Trim());
                        }

                        //setting user's role
                        if (!string.IsNullOrEmpty(obj.USERROLE))
                        {
                            profileObj.USERROLE = new TMCDBContext().fn_SaveRole(obj.USERROLE).ID;
                        }
                        //checking if profile already exists
                        //if (!new TMCDBContext().fn_ProfileVerifyifExists(profileObj))
                        {
                            //saving the final response
                            if (new TMCDBContext().fn_SaveProfiles(profileObj))
                            {
                                resp.respmessage = "Profile saved";
                                resp.respstatus = ResponseStatus.success;
                            }
                        }
                        /*else
                        {
                            resp.respmessage = "Profile already exists";
                            resp.respstatus = ResponseStatus.success;
                        }*/
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
        public static bool Delete(int objID)
        {
            bool resp = false;

            if (objID > 0)
                resp = new TMCDBContext().fn_DeleteProfile(objID);

            return resp;
        }
        public static List<profileMasterViewModel> GetAllProfiles(int city = 0, int role = 0, string gender = "", string language = "")
        {
            var resp = new List<profileMasterViewModel>();
            var respCopy = new List<profileMasterViewModel>();
            try
            {
                respCopy = new TMCDBContext().fn_getallProfiles()
                    .Where(x => (city == 0 ? true : x.USERCITYID == city) && (role == 0 ? true : x.USERROLE == role) && (string.IsNullOrEmpty(gender) ? true : (string.IsNullOrEmpty(x.USERGENDER) ? true : x.USERGENDER.ToLower() == gender.ToLower())) && x.ISENABLE)
                    .Select(x => new profileMasterViewModel()
                    {
                        ID = x.ID,
                        DATECREATED = x.DATECREATED.Value.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                        USEREMAIL = x.USEREMAIL,
                        USERROLE = new TMCDBContext().fn_GetRoleByID(x.USERROLE),
                        USERTITLE = x.USERTITLE,
                        PROFILETYPEOF = x.PROFILETYPEOF,
                        ImageURL = x.PROFILEPICTURE,
                        USERLANGUAGES = x.USERLANGUAGES,
                        AccountID = new TMCDBContext().fn_GetUserByEmail(x.USEREMAIL).ID
                    }).ToList();


                if (!string.IsNullOrEmpty(language))
                {
                    foreach (var currProfile in respCopy)
                    {
                        if (currProfile.USERLANGUAGES != null && currProfile.USERLANGUAGES.Split(',').Contains(language))
                            resp.Add(currProfile);
                    }
                }
                else
                    resp = respCopy;


            }
            catch (Exception ex) { resp = new List<profileMasterViewModel>(); }
            return resp;
        }
        public static profileMasterViewModel GetSingleProfile_ByID(int ID,string objName)
        {
            var resp = new profileMasterViewModel();
            var languageList = "";
            var cityID = 0;
            try
            {

                resp = new TMCDBContext().fn_getallProfiles(ID).Select(x => new profileMasterViewModel()
                {
                    ID = x.ID,
                    DATECREATED = x.DATECREATED.Value.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    USERCITY = x.USERCITYID.Value.ToString(),
                    USEREMAIL = x.USEREMAIL,
                    USERLANGUAGES = x.USERLANGUAGES,
                    USERROLE = new TMCDBContext().fn_GetRoleByID(x.USERROLE),
                    USERTOTALEXPINYEARS = x.USERTOTALEXPINYEARS,
                    USERAWARDS = x.USERAWARDS,
                    USERCERTIFICATES = x.USERCERTIFICATES,
                    USERDEGREEURL = x.USERDEGREEURL,
                    USERFLDOFEXCELLENCE = x.USERFLDOFEXCELLENCE,
                    USERLETTEROFREF = x.USERLETTEROFREF,
                    USERPRVWORKEXP = x.USERPRVWORKEXP,
                    USERTITLE = x.USERTITLE,
                    USERUPLOADEDWORK = x.USERUPLOADEDWORK,
                    PROFILETYPEOF = x.PROFILETYPEOF,
                    ImageURL = x.PROFILEPICTURE,
                    WORKPROFILE = x.WORKPROFILE
                }).FirstOrDefault();

                if (!string.IsNullOrEmpty(resp.USERCITY.Trim()))
                {
                    int.TryParse(resp.USERCITY.Trim(), out cityID);
                    if (cityID > 0)
                    {
                        resp.USERCITY = new TMCDBContext().fn_GetSingleCityByID(cityID).CITY;
                    }
                }

                if (!string.IsNullOrEmpty(resp.USERLANGUAGES))
                {
                    foreach (var currLang in resp.USERLANGUAGES.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(currLang))
                        {
                            var objID = 0;
                            int.TryParse(currLang, out objID);
                            if (objID > 0)
                            {
                                languageList += "," + new TMCDBContext().fn_GetSingleLanguageByID(objID).LANGUAGEVAL;
                            }

                        }
                    }
                    if (languageList.StartsWith(","))
                        languageList = languageList.Substring(1, languageList.Length - 1);
                    while (languageList.EndsWith(","))
                        languageList = languageList.Substring(0, languageList.Length - 1);
                }
                if (!string.IsNullOrEmpty(languageList))
                    resp.USERLANGUAGES = languageList;
                if(resp.USERTITLE != System.Web.HttpUtility.UrlDecode(objName))
                {
                    resp = new profileMasterViewModel();
                }
            }
            catch { resp = new profileMasterViewModel(); }
            return resp;
        }
        public static profileMasterViewModel GetSingleEditProfile_ByID(int ID)
        {
            var resp = new profileMasterViewModel();
            var languageList = "";
            var cityID = 0;
            try
            {

                resp = new TMCDBContext().fn_getallProfiles(ID).Select(x => new profileMasterViewModel()
                {
                    ID = x.ID,
                    DATECREATED = x.DATECREATED.Value.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    USERCITY = x.USERCITYID.Value.ToString(),
                    USEREMAIL = x.USEREMAIL,
                    USERLANGUAGES = x.USERLANGUAGES,
                    USERROLE = new TMCDBContext().fn_GetRoleByID(x.USERROLE),
                    USERTOTALEXPINYEARS = x.USERTOTALEXPINYEARS,
                    USERAWARDS = x.USERAWARDS,
                    USERCERTIFICATES = x.USERCERTIFICATES,
                    USERDEGREEURL = x.USERDEGREEURL,
                    USERFLDOFEXCELLENCE = x.USERFLDOFEXCELLENCE,
                    USERLETTEROFREF = x.USERLETTEROFREF,
                    USERPRVWORKEXP = x.USERPRVWORKEXP,
                    USERTITLE = x.USERTITLE,
                    USERUPLOADEDWORK = x.USERUPLOADEDWORK,
                    PROFILETYPEOF = x.PROFILETYPEOF,
                    ImageURL = x.PROFILEPICTURE,
                    USERAGE = x.USERAGE.Value,
                    AccountID = x.ACCOUNTID,
                    USERGENDER = x.USERGENDER,
                    WORKPROFILE = x.WORKPROFILE
                }).FirstOrDefault();

                resp.ContactNumber = new TMCDBContext().fn_GetUserByID(resp.AccountID).ContactNumber;

                if (!string.IsNullOrEmpty(resp.USERCITY.Trim()))
                {
                    int.TryParse(resp.USERCITY.Trim(), out cityID);
                    if (cityID > 0)
                    {
                        resp.USERCITY = new TMCDBContext().fn_GetSingleCityByID(cityID).CITY;
                    }
                }

                if (!string.IsNullOrEmpty(resp.USERLANGUAGES))
                {
                    foreach (var currLang in resp.USERLANGUAGES.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.IsNullOrEmpty(currLang))
                        {
                            var objID = 0;
                            int.TryParse(currLang, out objID);
                            if (objID > 0)
                            {
                                languageList += "," + new TMCDBContext().fn_GetSingleLanguageByID(objID).LANGUAGEVAL;
                            }

                        }
                    }
                    if (languageList.StartsWith(","))
                        languageList = languageList.Substring(1, languageList.Length - 1);
                    while (languageList.EndsWith(","))
                        languageList = languageList.Substring(0, languageList.Length - 1);
                }
                if (!string.IsNullOrEmpty(languageList))
                    resp.USERLANGUAGES = languageList;
            }
            catch { resp = new profileMasterViewModel(); }
            return resp;
        }
        public static TBL_PROFILESMASTER GetSingleProfile_ByAccountID(int ID)
        {
            return new TMCDBContext().fn_getProfile_ByAccountID(ID);
        }
    }
}
