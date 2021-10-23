using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Verify()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        public ActionResult register()
        {
            return View();
        }
    }
}
