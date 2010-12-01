using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PublicQuestions.Model.Connection;

namespace PublicQuestions.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private IMessageRepository messageRepository;

        public HomeController()
        {
            this.messageRepository = new MessageRepository(MvcApplication.CurrentSession);
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            var categories = messageRepository.GetMessages();
            if (categories == null)
                return RedirectToAction("Create");
            ViewData["Messages"] = categories;
            return View(categories);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
