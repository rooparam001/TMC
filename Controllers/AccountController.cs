using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using TMC.AppRepository;
using TMC.Models;

namespace TMC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Verify() => View();
        public ActionResult login() => View();
        [HttpGet]
        public JsonResult register(string UserName, string UserEmail, string ContactNumber, string UPassword)
        {
            var resp = new ajaxResponse();

            var outputModelData = new registerloginUserViewModel()
            {
                ContactNumber = ContactNumber,
                Email = UserEmail,
                Password = UPassword,
                UserName = UserName
            };
            outputModelData = AppUsers.Save(outputModelData);
            resp = new ajaxResponse()
            {
                data = null,
                respmessage = (outputModelData.UserStatus ? "You're a registered user now, please proceed via login." : outputModelData.validationMessage),
                respstatus = (outputModelData.UserStatus ? ResponseStatus.success : ResponseStatus.error)
            };
            return Json(resp);
        }
        [HttpGet]
        public JsonResult login(string Email, string UPassword)
        {
            var resp = new ajaxResponse();
            var obj = new registerloginUserViewModel()
            {
                Email = Email,
                Password = UPassword
            };
            obj = AppUsers.GetUserByEmailPassword(obj);
            if (obj.UserStatus)
            {
                var _accountObj = new AccountMaster();
                _accountObj = AppUsers.fn_GetUserByEmail(Email.Trim());
                if (_accountObj != null)
                {
                    if (!string.IsNullOrEmpty(_accountObj.Role))
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, _accountObj.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.Role, _accountObj.Role));
                        identity.AddClaim(new Claim("UserID", _accountObj.ID.ToString()));

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                            IsPersistent = true
                        };

                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                    }
                    else
                        obj = new registerloginUserViewModel()
                        {
                            UserStatus = false,
                            validationMessage = "Invalid User found."
                        };
                }
                else
                    obj = new registerloginUserViewModel()
                    {
                        UserStatus = false,
                        validationMessage = "Invalid User found."
                    };
            }
            resp = new ajaxResponse()
            {
                data = null,
                respmessage = (obj.UserStatus ? "" : obj.validationMessage),
                respstatus = (obj.UserStatus ? ResponseStatus.success : ResponseStatus.error)
            };
            return Json(resp);
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult UpComingPlays() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult Plays() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult Profiles() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult GiveAway() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult Directors() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult Tickets() => View();
        [Authorize]
        public ActionResult ListYourPlay() => View();
        public ActionResult ListYourProfile() => View();
        [Authorize]
        public ActionResult ListYourGiveAway() => View();
        [Authorize(Roles = "ADMINISTRATOR")]
        public ActionResult HomePageSettings() => View();
        [Authorize]
        public ActionResult message() => View();

        #region UpComing Plays region
        [HttpGet]
        public JsonResult DeleteUpcomingPlay(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = Play.DeleteUpcomingPlay(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Play deleted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllUpcomingPlay()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllPlays(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult DeleteAllUpcomingPlay()
        {
            bool resp = false;
            resp = Play.DeleteAllUpcomingPlay();
            return Json(new ajaxResponse()
            {
                data = null,
                respmessage = (resp ? "All the plays are deleted" : "Something went wrong, please try again later."),
                respstatus = (resp ? ResponseStatus.success : ResponseStatus.error)
            }); ;
        }

        [HttpPost]
        public async Task<JsonResult> SaveUpcomingPlay(List<IFormFile> files)
        {
            var resp = new ajaxResponse();
            var relationModelObj = new TBL_UPCOMINGPLAYS();
            try
            {
                resp.data = null;

                relationModelObj = new TBL_UPCOMINGPLAYS()
                {
                    PLAYDATE = Request.Form["PLAYDATE"],
                    PLAYTIME = Request.Form["PLAYTIME"],
                    TICKETSBUYLINK = Request.Form["TICKETSBUYLINK"],
                    TITLE = Request.Form["TITLE"],
                    VIEWEVENTLINK = Request.Form["VIEWEVENTLINK"],
                    ISENABLE = true
                };

                try
                {
                    IFormFile source = files[0];
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    filename = this.EnsureCorrectFilename(filename);
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "upcomingplays")))
                        await source.CopyToAsync(output);

                    relationModelObj.IMAGEURL = filename;
                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }

                try
                {
                    Play.SaveUpcomingPlay(relationModelObj);
                    resp.respmessage = "Play saved";
                    resp.respstatus = ResponseStatus.success;
                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = ex.Message.ToString(),
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }
        #endregion

        #region Existing Plays region
        [HttpGet]
        public JsonResult DeletePlay(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = Play.DeletePlay(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Play deleted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllPlays()
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetAllExistingPlays().Select(x => new accountPlayModel()
                {
                    id = x.ID,
                    Actor = x.ACTOR,
                    DateCreated = x.DATECREATED.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    Director = x.DIRECTOR,
                    ImageURL = x.IMAGEURL,
                    Title = x.TITLE,
                    Writer = x.WRITER
                }).ToList(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetSinglePlay(int objID)
        {
            var resp = new ajaxResponse()
            {
                data = Play.fn_GetSinglePlayByID(objID),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult DeleteAllExistingPlays()
        {
            bool resp = false;
            resp = Play.fn_DeleteAllExistingPlays();
            return Json(new ajaxResponse()
            {
                data = null,
                respmessage = (resp ? "All the plays are deleted" : "Something went wrong, please try again later."),
                respstatus = (resp ? ResponseStatus.success : ResponseStatus.error)
            }); ;
        }

        [HttpPost]
        public async Task<JsonResult> SavePlay(List<IFormFile> thumbnailfiles, List<IFormFile> sliderfiles, List<IFormFile> censorcertificate, List<IFormFile> techrider)
        {
            var resp = new ajaxResponse();
            var relationModelObj = new TBL_PLAYSMASTER();
            try
            {
                resp.data = null;
                int.TryParse(Request.Form["NUMBER_OF_SHOWS"], out int _noofshows);

                relationModelObj = new TBL_PLAYSMASTER()
                {
                    TITLE = Request.Form["TITLE"],
                    ISENABLE = true,
                    //ABOUT_THEATRE_LINK = Request.Form["ABOUT_THEATRE_LINK"],
                    ACTOR = Request.Form["ACTOR"],
                    DIRECTOR = Request.Form["DIRECTOR"],
                    NUMBER_OF_SHOWS = _noofshows,
                    PREMIERDATE = Request.Form["PREMIERDATE"],
                    SYNOPSIS = Request.Form["SYNOPSIS"],
                    TRAILERLINK = Request.Form["TRAILERLINK"],
                    WRITER = Request.Form["WRITER"],
                    Genre = Request.Form["GENRE"],
                    LANGAUAGE = Request.Form["LANGUAGE"],
                    AGESUITABLEFOR = Request.Form["SUITABLEFORAGE"],
                    DURATION = Request.Form["DURATION"],
                    DATECREATED = DateTime.Now,
                    CITY = Request.Form["CITY"],
                    ID = Convert.ToInt16(string.IsNullOrEmpty(Request.Form["ID"]) ? 0 : Request.Form["ID"]),
                    SYNOPSIS_SOCIALHANDLES = (string.IsNullOrEmpty(Request.Form["SYNOPSISFORSOCIALHANDLES"]) ? "" : Request.Form["SYNOPSISFORSOCIALHANDLES"].ToString()),
                    CASTNCREDIT = (string.IsNullOrEmpty(Request.Form["CASTNCREDIT"]) ? "" : Request.Form["CASTNCREDIT"].ToString()),
                    GROUPFACEBOOK_HANDLEURL = (string.IsNullOrEmpty(Request.Form["FACEBOOKHANDLEURL"]) ? "" : Request.Form["FACEBOOKHANDLEURL"].ToString()),
                    GROUPTWITTER_HANDLEURL = (string.IsNullOrEmpty(Request.Form["TWITTERHANDLEURL"]) ? "" : Request.Form["TWITTERHANDLEURL"].ToString()),
                    GROUPINSTAGARAM_HANDLEURL = (string.IsNullOrEmpty(Request.Form["INSTAGRAMHANDLEURL"]) ? "" : Request.Form["INSTAGRAMHANDLEURL"].ToString()),
                    GROUPINFO = (string.IsNullOrEmpty(Request.Form["GROUPINFO"]) ? "" : Request.Form["GROUPINFO"].ToString()),
                    GROUPTITLE = (string.IsNullOrEmpty(Request.Form["GROUPTITLE"]) ? "" : Request.Form["GROUPTITLE"].ToString()),
                    PLAYLINK = (string.IsNullOrEmpty(Request.Form["PLAYLINK"]) ? "" : Request.Form["PLAYLINK"].ToString())
                };


                try
                {
                    //saving play's thumbnail
                    if (thumbnailfiles != null)
                    {
                        if (thumbnailfiles.Count > 0)
                        {
                            IFormFile source = thumbnailfiles[0];
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "allplays")))
                                await source.CopyToAsync(output);

                            relationModelObj.IMAGEURL = filename;
                        }
                    }
                }
                catch (Exception ex)
                {
                    relationModelObj.IMAGEURL = "";
                }

                try
                {
                    //saving play's censor certificate
                    if (censorcertificate != null)
                    {
                        if (censorcertificate.Count > 0)
                        {
                            foreach (var currFile in censorcertificate)
                            {
                                string filename = ContentDispositionHeaderValue.Parse(currFile.ContentDisposition).FileName.Trim('"');
                                filename = this.EnsureCorrectFilename(filename);
                                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "allplays")))
                                    await currFile.CopyToAsync(output);

                                relationModelObj.CENSORCERTIFICATE = filename + ",";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    relationModelObj.CENSORCERTIFICATE = "";
                }

                try
                {
                    //saving play's thumbnail
                    if (techrider != null)
                    {
                        if (techrider.Count > 0)
                        {
                            foreach (var currFile in techrider)
                            {
                                string filename = ContentDispositionHeaderValue.Parse(currFile.ContentDisposition).FileName.Trim('"');
                                filename = this.EnsureCorrectFilename(filename);
                                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "allplays")))
                                    await currFile.CopyToAsync(output);

                                relationModelObj.TECHRIDER = filename + ",";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    relationModelObj.TECHRIDER = "";
                }

                try
                {
                    relationModelObj = Play.fn_SavePlay(relationModelObj);
                    if (relationModelObj != null)
                    {
                        if (relationModelObj.ID > 0)
                        {
                            if (sliderfiles != null)
                            {
                                if (sliderfiles.Count > 0)
                                {
                                    if (Slider.Delete_ByObjectID(relationModelObj.ID))
                                    {
                                        //saving play's slider images
                                        foreach (IFormFile currsource in sliderfiles)
                                        {
                                            string filename = ContentDispositionHeaderValue.Parse(currsource.ContentDisposition).FileName.Trim('"');
                                            filename = this.EnsureCorrectFilename(filename);
                                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "sliders")))
                                            {
                                                await currsource.CopyToAsync(output);
                                                Slider.Save(new slider_inputoutputmodel()
                                                {
                                                    OBJECTID = relationModelObj.ID,
                                                    ObjectType = SliderObjectType.Plays,
                                                    SliderLst = new List<sliderViewModel>()
                                                    {

                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            resp.respmessage = "Play saved";
                            resp.respstatus = ResponseStatus.success;
                        }
                        else
                            resp = new ajaxResponse()
                            {
                                data = null,
                                respmessage = "Something went wrong.",
                                respstatus = ResponseStatus.error
                            };
                    }
                    else
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Something went wrong.",
                            respstatus = ResponseStatus.error
                        };

                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = ex.Message.ToString(),
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }
        #endregion

        #region Directors region
        [HttpPost]
        public async Task<JsonResult> SaveDirector(List<IFormFile> thumbnailfiles)
        {
            var resp = new ajaxResponse();
            try
            {
                var inputDirectorObj = new TBL_DIRECTORMASTER()
                {
                    DATECREATED = DateTime.Now,
                    OBJECTNAME = Request.Form["TITLE"],
                    OBJECTDESCRIPTION = Request.Form["DESCRIPTION"],
                };

                try
                {
                    //saving directors's thumbnail
                    IFormFile source = thumbnailfiles[0];
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    filename = this.EnsureCorrectFilename(filename);
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "directors")))
                        await source.CopyToAsync(output);

                    inputDirectorObj.OBJECTIMGURL = filename;

                    var isSave = Director.SaveDirectors(inputDirectorObj);
                    if (isSave)
                    {
                        resp.respmessage = "Director saved";
                        resp.respstatus = ResponseStatus.success;
                    }
                    else
                    {
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Something went wrong",
                            respstatus = ResponseStatus.error
                        };
                    }
                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
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
            return Json(resp);
        }

        [HttpGet]
        public JsonResult DeleteDirector(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = Director.DeleteDirectors(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Director deleted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllDirectors()
        {
            var resp = new ajaxResponse()
            {
                data = Director.GetAllDirectors().Select(x => new directorModel()
                {
                    ID = x.ID,
                    ImageURL = x.OBJECTIMGURL,
                    Title = x.OBJECTNAME,
                    DateCreated = x.DATECREATED.ToString("dddd dd MMMM", CultureInfo.CreateSpecificCulture("en-US")),
                    Description = x.OBJECTDESCRIPTION
                }).ToList(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult Getirector_ByID(int ID)
        {
            var resp = new ajaxResponse()
            {
                data = Director.GetSingleDirector_ByID(ID),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }
        #endregion

        #region Profile region
        [HttpPost]
        public async Task<JsonResult> SaveProfile(List<IFormFile> fuDegree, List<IFormFile> fuLetterofRef, List<IFormFile> fuCertificates, List<IFormFile> fuAwardsAchiev, List<IFormFile> fuUploadWork, List<IFormFile> fuProfilePicture)
        {
            var resp = new ajaxResponse();
            try
            {
                var inputProfileObj = new profileMasterViewModel()
                {
                    ISENABLE = true,
                    USERCITY = Request.Form["CITY"],
                    USEREMAIL = Request.Form["EMAILID"],
                    USERLANGUAGES = Request.Form["LANGUAGES"],
                    USERFLDOFEXCELLENCE = Request.Form["FLDOFEXCELLENCE"],
                    USERPRVWORKEXP = Request.Form["PREVWORKDET"],
                    USERROLE = Request.Form["ROLEID"],
                    USERTITLE = Request.Form["FULLNAME"],
                    USERTOTALEXPINYEARS = Request.Form["EXPYRS"],
                    PROFILETYPEOF = Request.Form["PROFILETYPEOF"],
                    USERAGE = (string.IsNullOrEmpty(Request.Form["USERAGE"]) ? 0 : Convert.ToInt32(Request.Form["USERAGE"].ToString())),
                    USERGENDER = Request.Form["USERGENDER"]
                };

                var outputModelData = new registerloginUserViewModel()
                {
                    ContactNumber = Request.Form["CONTACTNUMBER"],
                    Email = Request.Form["EMAILID"],
                    Password = Request.Form["PASSWORD"],
                    UserName = Request.Form["FULLNAME"],
                    userrole = inputProfileObj.USERROLE
                };


                try
                {
                    //adding new account based on the information
                    outputModelData = AppUsers.Save(outputModelData);

                    if (!outputModelData.UserStatus)
                    {
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = outputModelData.validationMessage,
                            respstatus = ResponseStatus.error
                        };
                    }
                    else
                    {
                        //saving user's degree(s)
                        foreach (var currFile in fuDegree)
                        {
                            IFormFile source = currFile;
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                await source.CopyToAsync(output);

                            inputProfileObj.USERDEGREEURL += filename + ",";
                        }

                        //saving user's letter(s) of reference
                        foreach (var currFile in fuLetterofRef)
                        {
                            IFormFile source = currFile;
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                await source.CopyToAsync(output);

                            inputProfileObj.USERLETTEROFREF += filename + ",";
                        }

                        //saving user's letter(s) of reference
                        foreach (var currFile in fuCertificates)
                        {
                            IFormFile source = currFile;
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                await source.CopyToAsync(output);

                            inputProfileObj.USERCERTIFICATES += filename + ",";
                        }

                        //saving user's letter(s) of reference
                        foreach (var currFile in fuAwardsAchiev)
                        {
                            IFormFile source = currFile;
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                await source.CopyToAsync(output);

                            inputProfileObj.USERAWARDS += filename + ",";
                        }

                        //saving user's letter(s) of reference
                        foreach (var currFile in fuUploadWork)
                        {
                            IFormFile source = currFile;
                            string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                            filename = this.EnsureCorrectFilename(filename);
                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                await source.CopyToAsync(output);

                            inputProfileObj.USERUPLOADEDWORK += filename + ",";
                        }

                        //saving user's profile picture
                        if (fuProfilePicture != null)
                        {
                            if (fuProfilePicture.Count > 0)
                            {
                                var currFile = fuProfilePicture[0];
                                IFormFile source = currFile;
                                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                                filename = this.EnsureCorrectFilename(filename);
                                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "ProfileData")))
                                    await source.CopyToAsync(output);

                                inputProfileObj.ImageURL = filename;
                            }
                        }
                        resp = AppProfiles.SaveProfiles(inputProfileObj);
                    }


                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
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
            return Json(resp);
        }

        [HttpGet]
        public JsonResult DeleteProfile(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = AppProfiles.Delete(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Profile deactivated";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllProfiles()
        {
            var respModel = new List<profileMasterViewModel>();
            respModel = AppProfiles.GetAllProfiles();
            var resp = new ajaxResponse()
            {
                data = respModel,
                respstatus = ResponseStatus.success,
                respmessage = (respModel.Count > 0 ? "Success" : "Something went wrong,please try again later.")
            };

            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetProfile_ByID(int objID)
        {
            var resp = new ajaxResponse()
            {
                data = AppProfiles.GetSingleProfile_ByID(objID),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }
        #endregion

        #region Give away region
        [HttpGet]
        public JsonResult DeleteGiveaway(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = GiveAways.Delete(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Giveaway deleted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult AcceptGiveaway(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = GiveAways.Accept(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Giveaway accepted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllGiveaways()
        {
            var resp = new ajaxResponse()
            {
                data = GiveAways.getAll(),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetSingleGiveaway(int objID)
        {
            var resp = new ajaxResponse()
            {
                data = GiveAways.getByID(objID),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpPost]
        public async Task<JsonResult> SaveGiveaway(List<IFormFile> thumbnailfiles)
        {
            var resp = new ajaxResponse();
            var relationModelObj = new giveawayViewModel();
            var giveawayModelObj = new TBL_GIVEAWAYMASTER();
            var isPDF = false;
            try
            {
                resp.data = null;
                int.TryParse(Request.Form["NUMBER_OF_SHOWS"], out int _noofshows);

                if (thumbnailfiles != null)
                {
                    if (thumbnailfiles.Count > 0)
                    {
                        foreach (IFormFile currsource in thumbnailfiles)
                        {
                            string filename = ContentDispositionHeaderValue.Parse(currsource.ContentDisposition).FileName.Trim('"');
                            if (filename.Contains(".pdf"))
                                isPDF = true;
                        }
                    }
                }

                relationModelObj = new giveawayViewModel()
                {
                    OBJTITLE = (string.IsNullOrEmpty(Request.Form["TITLE"]) ? "" : Request.Form["TITLE"].ToString()),
                    CITY = (string.IsNullOrEmpty(Request.Form["CITY"]) ? "" : Request.Form["CITY"].ToString()),
                    OBJAVAILABILITY = (string.IsNullOrEmpty(Request.Form["AVAILABILITY"]) ? "" : Request.Form["AVAILABILITY"].ToString()),
                    OBJCONTACTDETAILS = (string.IsNullOrEmpty(Request.Form["CONTACTDETAILS"]) ? "" : Request.Form["CONTACTDETAILS"].ToString()),
                    ISACCEPTED = false,
                    ISENABLE = true,
                    isPDF = isPDF
                };


                try
                {
                    giveawayModelObj = GiveAways.Save(relationModelObj);
                    if (giveawayModelObj != null)
                    {
                        if (giveawayModelObj.ID > 0)
                        {
                            if (thumbnailfiles != null)
                            {
                                if (thumbnailfiles.Count > 0)
                                {
                                    if (Slider.Delete_ByObjectID(giveawayModelObj.ID))
                                    {
                                        //saving play's slider images
                                        foreach (IFormFile currsource in thumbnailfiles)
                                        {
                                            string filename = ContentDispositionHeaderValue.Parse(currsource.ContentDisposition).FileName.Trim('"');
                                            filename = this.EnsureCorrectFilename(filename);
                                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "sliders")))
                                            {
                                                await currsource.CopyToAsync(output);
                                                Slider.Save(new slider_inputoutputmodel()
                                                {
                                                    OBJECTID = giveawayModelObj.ID,
                                                    ObjectType = SliderObjectType.GiveAway,
                                                    SliderLst = new List<sliderViewModel>() { new sliderViewModel() {
                                                    Description="",
                                                    SliderImgURL=filename
                                                    }}
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            resp.respmessage = "Giveaway saved";
                            resp.respstatus = ResponseStatus.success;
                        }
                        else
                            resp = new ajaxResponse()
                            {
                                data = null,
                                respmessage = "Something went wrong.",
                                respstatus = ResponseStatus.error
                            };
                    }
                    else
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Something went wrong.",
                            respstatus = ResponseStatus.error
                        };

                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = ex.Message.ToString(),
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }
        #endregion

        #region Home Page Settings
        [HttpGet]
        public JsonResult DeleteSlider(string objID)
        {
            int modelID = 0;
            int.TryParse(objID, out modelID);
            var resp = new ajaxResponse()
            {
                respmessage = "Something went wrong.",
                respstatus = ResponseStatus.error
            };
            if (modelID > 0)
            {
                resp.data = Slider.Delete_ByID(modelID);
                resp.respstatus = ResponseStatus.success;
                resp.respmessage = "Slider deleted";
            }
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllHomePageSliders()
        {
            var resp = new ajaxResponse()
            {
                data = Slider.GetCommaSeparated_ListModel(0, SliderObjectType.MainPage),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpPost]
        public async Task<JsonResult> SaveHomePageSlider(List<IFormFile> thumbnailfiles)
        {
            var resp = new ajaxResponse();
            var relationModelObj = new TBL_HOMEPAGESETTINGS();
            try
            {
                resp.data = null;
                int.TryParse(Request.Form["NUMBER_OF_SHOWS"], out int _noofshows);

                relationModelObj = new TBL_HOMEPAGESETTINGS()
                {
                    OBJECTTITLE = (string.IsNullOrEmpty(Request.Form["TITLE"]) ? "" : Request.Form["TITLE"].ToString()),
                    OBJECTDESCRIPTION = "",
                    OBJECTTYPE = (int)HomePageSettingObjectType.Slider,
                    ISENABLE = true
                };


                try
                {
                    relationModelObj = HomePage.Save(relationModelObj);
                    if (relationModelObj != null)
                    {
                        if (relationModelObj.ID > 0)
                        {
                            if (thumbnailfiles != null)
                            {
                                if (thumbnailfiles.Count > 0)
                                {
                                    //saving play's slider images
                                    foreach (IFormFile currsource in thumbnailfiles)
                                    {
                                        string filename = ContentDispositionHeaderValue.Parse(currsource.ContentDisposition).FileName.Trim('"');
                                        filename = this.EnsureCorrectFilename(filename);
                                        using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "sliders")))
                                        {
                                            await currsource.CopyToAsync(output);
                                            Slider.Save(new slider_inputoutputmodel()
                                            {
                                                OBJECTID = relationModelObj.ID,
                                                ObjectType = SliderObjectType.MainPage,
                                                SliderLst = new List<sliderViewModel>() { new sliderViewModel() {
                                                    Description=(string.IsNullOrEmpty(Request.Form["DESCRIPTION"]) ? "" : Request.Form["DESCRIPTION"].ToString()),
                                                    SliderImgURL=filename,
                                                    Title=(string.IsNullOrEmpty(Request.Form["TITLE"]) ? "" : Request.Form["DESCRIPTITLETION"].ToString())
                                                    }}
                                            });
                                        }
                                    }
                                }
                            }

                            resp.respmessage = "Setting saved";
                            resp.respstatus = ResponseStatus.success;
                        }
                        else
                            resp = new ajaxResponse()
                            {
                                data = null,
                                respmessage = "Something went wrong.",
                                respstatus = ResponseStatus.error
                            };
                    }
                    else
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Something went wrong.",
                            respstatus = ResponseStatus.error
                        };

                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = ex.Message.ToString(),
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }
        #endregion

        #region Inquiries
        [HttpGet]
        public JsonResult GetAllInquiries()
        {
            var respModel = new List<inquiryViewModel>();
            respModel = Enquiries.fn_getallUnSeenEnquiries();
            var resp = new ajaxResponse()
            {
                data = respModel,
                respstatus = ResponseStatus.success,
                respmessage = (respModel.Count > 0 ? "Success" : "Something went wrong,please try again later.")
            };

            return Json(resp);
        }
        #endregion

        #region Chat region

        [HttpGet]
        public JsonResult LoadNewGroup(int userID)
        {
            var resp = new ajaxResponse()
            {
                data = ChatService.SaveGroup(new ChatServiceMessageListModel()
                {
                    SenderID = _getuserLoggedinID(),
                    UserID = userID
                }),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllChat(int groupID)
        {
            var resp = new ajaxResponse()
            {
                data = ChatService.GetGroupChat(groupID, _getuserLoggedinID()),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpGet]
        public JsonResult GetAllContactList()
        {
            var resp = new ajaxResponse()
            {
                data = ChatService.GetAll(_getuserLoggedinID()),
                respstatus = ResponseStatus.success
            };
            return Json(resp);
        }

        [HttpPost]
        public JsonResult SaveChat()
        {
            var resp = new ajaxResponse();
            var relationModelObj = new ChatServiceMessageListModel();
            try
            {
                resp.data = null;

                relationModelObj = new ChatServiceMessageListModel()
                {
                    ChatMessage = Request.Form["ChatMessage"],
                    GroupID = Convert.ToInt32(Request.Form["GroupID"]),
                    SenderID = _getuserLoggedinID(),
                    UserID = 0
                };

                try
                {
                    if (ChatService.SaveChat(relationModelObj))
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "",
                            respstatus = ResponseStatus.success
                        };
                    else
                        resp = new ajaxResponse()
                        {
                            data = null,
                            respmessage = "Message could not be sent.",
                            respstatus = ResponseStatus.error
                        };
                }
                catch (Exception ex)
                {
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
                }
            }
            catch (Exception ex)
            {
                resp = new ajaxResponse()
                {
                    data = null,
                    respmessage = ex.Message.ToString(),
                    respstatus = ResponseStatus.error
                };

            }

            return Json(resp);
        }
        #endregion

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            filename = DateTime.Now.Ticks.ToString() + filename;

            return filename;
        }
        private string GetPathAndFilename(string filename, string fileType)
        {
            switch (fileType)
            {
                case "upcomingplays":
                    {
                        return _webHostEnvironment.WebRootPath + ApplicationStaticProps.appBasePath_UpcomingPlay_Image + filename;
                    }
                case "allplays":
                    {
                        return _webHostEnvironment.WebRootPath + ApplicationStaticProps.appBasePath_Play_Image + filename;
                    }
                case "sliders":
                    {
                        return _webHostEnvironment.WebRootPath + ApplicationStaticProps.appBasePath_Sliders_Image + filename;
                    }
                case "directors":
                    {
                        return _webHostEnvironment.WebRootPath + ApplicationStaticProps.appBasePath_Director_Image + filename;
                    }
                case "ProfileData":
                    {
                        return _webHostEnvironment.WebRootPath + ApplicationStaticProps.appBasePath_Profile_Data + filename;
                    }
                default:
                    {
                        return _webHostEnvironment.WebRootPath + filename;
                    }
            };
        }
        [HttpGet]
        public JsonResult validateUserEmail(string email)
        {
            return Json(true);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Verify));
        }

        public int _getuserLoggedinID()
        {
            var userID = 0;
            int.TryParse(User.FindFirst("UserID").Value, out userID);
            return userID;
        }
    }
}