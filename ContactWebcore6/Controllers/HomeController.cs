using ContactWebcore6.Data;
using ContactWebcore6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactWebcore6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersRolse _userRoleService;


        public HomeController(ILogger<HomeController> logger, IUsersRolse userRoleService)
        {
            _logger = logger;
            _userRoleService = userRoleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> EnsureRoleAndUsers()
        {
            await _userRoleService.EnsureAdminUserRole();
            return RedirectToAction("Index");

        }




    }
}