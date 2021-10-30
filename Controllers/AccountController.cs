using EntitesInterfaces.DBEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IActionResult Verify()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }
        public ActionResult register(string UserName, string Email, string ContactNumber, string UPassword)
        {
            return View();
        }

        public ActionResult UpComingPlays() => View();

        [HttpGet]
        public JsonResult validateUserEmail(string email)
        {
            return Json(true);
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
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
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

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return _webHostEnvironment.WebRootPath + "\\blogs\\UpcomingPlays\\" + filename;
        }

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
    }
}
