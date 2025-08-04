using JobBoard.Data;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobBoard.Areas.Identity.Data;


namespace JobBoard.Controllers
{
    [Authorize(Roles = "RECRUITER")]
    public class RecruiterProfileController : Controller
    {
        private readonly JobBoardContext _context;
        private readonly UserManager<JobBoardUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public RecruiterProfileController(JobBoardContext context, UserManager<JobBoardUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var existing = await _context.RecruiterProfiles.FirstOrDefaultAsync(r => r.UserId == user.Id);
            if (existing != null)
                return RedirectToAction("Index", "Home");

            var model = new RecruiterProfile(); // crea modello vuoto
            return View(model); // passa il modello alla vista
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(RecruiterProfile model, IFormFile IdentityDocument, IFormFile CompanyCertificate, IFormFile VatCertificate)
        {
            var user = await _userManager.GetUserAsync(User);
            model.UserId = user.Id; // ✅ Spostato prima della validazione

            ModelState.Remove(nameof(model.UserId));

            if (!model.AcceptPrivacyPolicy)
            {
                ModelState.AddModelError(nameof(model.AcceptPrivacyPolicy), "You must accept the privacy policy.");
            }

            // Validazioni PDF
            if (IdentityDocument == null || Path.GetExtension(IdentityDocument.FileName).ToLower() != ".pdf")
                ModelState.AddModelError("IdentityDocumentPath", "Please upload a valid identity document (PDF).");

            if (CompanyCertificate == null || Path.GetExtension(CompanyCertificate.FileName).ToLower() != ".pdf")
                ModelState.AddModelError("CompanyCertificatePath", "Please upload a valid company certificate (PDF).");

            if (VatCertificate == null || Path.GetExtension(VatCertificate.FileName).ToLower() != ".pdf")
                ModelState.AddModelError("VatCertificatePath", "Please upload a valid VAT certificate (PDF).");

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    foreach (var error in entry.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field: {key} - Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }


            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "Recruiter");
            Directory.CreateDirectory(uploadsPath);

            // Salva i file
            string SaveFile(IFormFile file)
            {
                var fileName = Guid.NewGuid() + ".pdf";
                var relativePath = Path.Combine("uploads", "Recruiter", fileName); 
                var filePath = Path.Combine(_env.WebRootPath, relativePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    file.CopyTo(stream);

                return relativePath.Replace("\\", "/"); 
            }


            model.IdentityDocumentPath = SaveFile(IdentityDocument);
            model.CompanyCertificatePath = SaveFile(CompanyCertificate);
            model.VatCertificatePath = SaveFile(VatCertificate);
            model.IsApproved = false;

            _context.RecruiterProfiles.Add(model);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Profile submitted for approval.";
            return RedirectToAction("Index", "Home");
        }

        public class RecruiterController : Controller
        {
            public IActionResult PendingApproval()
            {
                return View();
            }
        }


    }
}
