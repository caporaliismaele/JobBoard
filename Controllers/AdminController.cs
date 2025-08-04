using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace JobBoard.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<JobBoardUser> _userManager;
        private readonly JobBoardContext _context;

        public AdminController(
            IWebHostEnvironment env,
            UserManager<JobBoardUser> userManager,
            JobBoardContext context)
        {
            _env = env;
            _userManager = userManager;
            _context = context;
        }


        public async Task<IActionResult> PendingRecruiters()
        {
            var recruiters = await _context.RecruiterProfiles
                .Where(r => !r.IsApproved)
                .Include(r => r.User)
                .ToListAsync();

            return View("~/Views/Admin/PendingRecruiters.cshtml", recruiters);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            var recruiter = await _context.RecruiterProfiles.FirstOrDefaultAsync(r => r.UserId == id);
            if (recruiter == null) return NotFound();

            recruiter.IsApproved = true;

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsApprovedRecruiter = true;
                await _userManager.UpdateAsync(user); // 🔁 Salva anche l'aggiornamento dell'utente
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Recruiter approved successfully.";
            return RedirectToAction("PendingRecruiters");
        }


        [HttpPost]
        public async Task<IActionResult> Reject(string id)
        {
            var recruiterProfile = await _context.RecruiterProfiles
                .FirstOrDefaultAsync(r => r.UserId == id);

            if (recruiterProfile != null)
            {
                // Elimina file associati
                var basePath = Path.Combine(_env.WebRootPath, "uploads", "Recruiter");

                void DeleteFile(string fileName)
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var path = Path.Combine(basePath, fileName);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                }

                DeleteFile(recruiterProfile.IdentityDocumentPath);
                DeleteFile(recruiterProfile.CompanyCertificatePath);
                DeleteFile(recruiterProfile.VatCertificatePath);

                _context.RecruiterProfiles.Remove(recruiterProfile);
            }

            // Rimuovi anche l'utente
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Recruiter account permanently deleted.";
            return RedirectToAction("PendingRecruiters");
        }



    }

}
