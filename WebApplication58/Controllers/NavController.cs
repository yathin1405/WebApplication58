using WebApplication58.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication58.Controllers
{
    public class NavController : Controller
    {
        private ApplicationDbContext repository;

        public NavController(ApplicationDbContext repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> roomTypes = repository.roomTypes
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(roomTypes);
        }
    }
}