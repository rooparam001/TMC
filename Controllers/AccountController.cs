using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
        public ActionResult register(string UserName, string Email, string ContactNumber, string UPassword)
        {
            return View();
        }

        public ActionResult UpComingPlays() => View();
        public ActionResult Plays() => View();
        public ActionResult Directors() => View();
        public ActionResult ListYourPlay() => View();
        public ActionResult ListYourProfile() => View();
        public ActionResult GiveAway() => View();

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
        public async Task<JsonResult> SavePlay(List<IFormFile> thumbnailfiles, List<IFormFile> sliderfiles)
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
                    resp = new ajaxResponse()
                    {
                        data = null,
                        respmessage = ex.Message.ToString(),
                        respstatus = ResponseStatus.error
                    };
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
                                    if (Slider.DelSlider(relationModelObj.ID))
                                    {
                                        //saving play's slider images
                                        foreach (IFormFile currsource in sliderfiles)
                                        {
                                            string filename = ContentDispositionHeaderValue.Parse(currsource.ContentDisposition).FileName.Trim('"');
                                            filename = this.EnsureCorrectFilename(filename);
                                            using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename, "sliders")))
                                            {
                                                await currsource.CopyToAsync(output);
                                                Slider.SaveSlider(new slider_inputoutputmodel()
                                                {
                                                    OBJECTID = relationModelObj.ID,
                                                    ObjectType = SliderObjectType.Plays,
                                                    OBJECTURL = new string[] { filename }
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
    }
}
