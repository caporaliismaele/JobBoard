using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{

    public class CandidateProfileController : Controller
    {
        private readonly JobBoardContext _context;
        private readonly UserManager<JobBoardUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CandidateProfileController(
            JobBoardContext context,
            UserManager<JobBoardUser> userManager,
            IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var existing = await _context.CandidateProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (existing != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // ✅ Passa sempre un modello vuoto alla view
            return View(new CandidateProfile());
        }

        [HttpPost]
        public async Task<IActionResult> CompleteProfile(CandidateProfile model, IFormFile cvFile)
        {

            // ✅ Log del valore della checkbox
            var user = await _userManager.GetUserAsync(User);

            // 🔍 LOG per test
            System.Diagnostics.Debug.WriteLine($"Privacy accepted? {model.AcceptPrivacyPolicy}");


            if (cvFile == null || Path.GetExtension(cvFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("cvFile", "Please upload a valid PDF.");
            }

            if (!model.AcceptPrivacyPolicy)
            {
                ModelState.AddModelError(nameof(model.AcceptPrivacyPolicy), "You must accept the privacy policy.");
            }

            if (!ModelState.IsValid)
            {
                // Log tutti gli errori
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error for '{key}': {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            // ✅ Salvataggio PDF
            var uploads = Path.Combine(_env.WebRootPath, "uploads", "Candidate");
            Directory.CreateDirectory(uploads); // crea la cartella se non esiste

            var fileName = Guid.NewGuid().ToString() + ".pdf";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(stream);
            }


            // ✅ Salvataggio in DB
            model.UserId = user.Id;
            model.CvFileName = fileName;

            _context.CandidateProfiles.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.CandidateProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
                return RedirectToAction("CompleteProfile");

            return View(profile);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CandidateProfile model, IFormFile? newCvFile)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            if (newCvFile != null)
            {
                // Elimina vecchio file
                // Percorso del file da eliminare nella cartella Candidate
                var oldFilePath = Path.Combine(_env.WebRootPath, "uploads", "Candidate", profile.CvFileName);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                // Salva nuovo file
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "Candidate");
                var fileName = Guid.NewGuid() + ".pdf";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newCvFile.CopyToAsync(stream);
                }

                profile.CvFileName = fileName;
            }

            // Aggiorna dati
            profile.Name = model.Name;
            profile.Surname = model.Surname;
            profile.Email = model.Email;
            profile.Telephone = model.Telephone;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "CANDIDATE")]
        public async Task<IActionResult> MyApplication()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.CandidateProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return NotFound(); // oppure un altro tipo di gestione
            }

            var applications = await _context.Candidatures
                .Where(c => c.CandidateId == user.Id)
                .Include(c => c.JobOffer)
                .ToListAsync();


            return View(applications);
        }






    }

}
