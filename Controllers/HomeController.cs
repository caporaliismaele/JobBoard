using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using JobBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobBoardContext _context;
        private readonly UserManager<JobBoardUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<JobBoardUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<JobBoardUser> userManager,
            SignInManager<JobBoardUser> signInManager,
            JobBoardContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    // Utente autenticato ma non presente nel DB (es. dopo Drop-Database)
                    // Possiamo fare il logout o mostrare un messaggio amichevole
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                if (User.IsInRole("ADMIN"))
                {

                    return RedirectToAction("PendingRecruiters", "Admin");
                }

                if (User.IsInRole("RECRUITER"))
                {

                    if (!user.IsApprovedRecruiter)
                    {
                        return View("~/Views/JobsRecruiter/PendingApproval.cshtml");

                    }

                    // Mostra le offerte
                    var offers = await _context.JobOffers
                        .Where(j => j.RecruiterId == user.Id)
                        .ToListAsync();

                    return View("~/Views/JobsRecruiter/Index.cshtml", offers);
                }

                if (User.IsInRole("CANDIDATE"))
                {
                    var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

                    if (profile == null)
                    {
                        return RedirectToAction("CompleteProfile", "CandidateProfile");
                    }

                    var offers = await _context.JobOffers.ToListAsync();
                    return View("~/Views/JobsCandidate/Index.cshtml", offers);
                }
            }

            // Utente non autenticato → homepage pubblica
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
    }
}
