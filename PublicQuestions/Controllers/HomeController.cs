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
            ViewData["Message"] = "Public Questions!";
            var message = messageRepository.GetMessages();
            ViewData["Messages"] = message;
            return View(message);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
