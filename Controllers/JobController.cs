using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using JobBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace JobBoard.Controllers
{
    public class JobController : Controller
    {
        private readonly JobBoardContext _context;
        private readonly UserManager<JobBoardUser> _userManager;

        public JobController(JobBoardContext context, UserManager<JobBoardUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobOffers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("RECRUITER"))
            {
                var offers = await _context.JobOffers
                    .Where(j => j.RecruiterId == user.Id)
                    .ToListAsync();
                return View("~/Views/JobsRecruiter/Index.cshtml", offers);
            }

            // Candidate
            var allOffers = await _context.JobOffers.ToListAsync();
            return View("~/Views/JobsCandidate/Index.cshtml", allOffers);
        }

        public async Task<IActionResult> IndexForCandidate(int? selectedId, string? position, string? location)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.CandidateProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return NotFound(); // oppure un altro tipo di gestione
            }


            var alreadyApplied = false;
            if (selectedId.HasValue && user != null)
            {
                alreadyApplied = await _context.Candidatures
                    .AnyAsync(c => c.JobOfferId == selectedId && c.CandidateId == user.Id);
            }
            ViewBag.AlreadyApplied = alreadyApplied;


            // Prendi tutte le offerte
            var query = _context.JobOffers.AsQueryable();

            // Applica filtri se presenti
            if (!string.IsNullOrWhiteSpace(position))
            {
                query = query.Where(j => j.Position.Contains(position));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }

            var offers = await query
                .OrderByDescending(j => j.PublishedDate)
                .ToListAsync();

            // Se è stato selezionato un ID, prendi l'offerta corrispondente per mostrarne i dettagli
            var selectedOffer = selectedId.HasValue
                ? offers.FirstOrDefault(o => o.Id == selectedId.Value)
                : offers.FirstOrDefault();

            ViewBag.SelectedOffer = selectedOffer;
            ViewBag.SelectedId = selectedOffer?.Id;

            return View("~/Views/JobsCandidate/Index.cshtml", offers);
        }







        // GET: JobOffers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        // GET: JobOffers/Create
        public IActionResult Create()
        {
            return View("~/Views/JobsRecruiter/Create.cshtml");
        }


        // POST: JobOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOffer offer)
        {
            var user = await _userManager.GetUserAsync(User);
            offer.RecruiterId = user.Id;
            offer.PublishedDate = DateTime.Now;

            ModelState.Remove(nameof(offer.RecruiterId)); // Rimuove il vecchio stato, se esiste

            if (!ModelState.IsValid)
            {
                return View("~/Views/JobsRecruiter/Create.cshtml");
            }

            _context.JobOffers.Add(offer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = await _context.JobOffers.FindAsync(id);
            if (offer == null)
                return NotFound();

            return View("~/Views/JobsRecruiter/Edit.cshtml", offer);
        }





        // GET: JobOffers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobOffer offer)
        {
            if (id != offer.Id)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            offer.RecruiterId = user.Id;

            ModelState.Remove(nameof(offer.RecruiterId));

            if (!ModelState.IsValid)
                return View("~/Views/JobsRecruiter/Edit.cshtml", offer);

            try
            {
                _context.Update(offer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.JobOffers.Any(e => e.Id == offer.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }



        // POST: JobOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: JobOffers/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var offer = await _context.JobOffers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null) return NotFound();

            return View("~/Views/JobsRecruiter/Delete.cshtml", offer);
        }


        // POST: JobOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            if (jobOffer != null)
            {
                _context.JobOffers.Remove(jobOffer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOfferExists(int id)
        {
            return _context.JobOffers.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.CandidateProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return NotFound(); // oppure un altro tipo di gestione
            }


            var jobOffer = await _context.JobOffers.FindAsync(jobId);
            if (jobOffer == null)
            {
                return NotFound("Job offer not found.");
            }

            bool alreadyApplied = await _context.Candidatures
                .AnyAsync(c => c.CandidateId == user.Id && c.JobOfferId == jobId);

            if (!alreadyApplied)
            {
                var application = new Candidature
                {
                    JobOfferId = jobId,
                    CandidateId = user.Id,
                    ApplicationDate = DateTime.Now
                };

                _context.Candidatures.Add(application);
                await _context.SaveChangesAsync();
                TempData["Message"] = "You have successfully applied to this job.";
            }


            return RedirectToAction("IndexForCandidate", new { selectedId = jobId });
        }

        [Authorize(Roles = "RECRUITER")]
        public async Task<IActionResult> ViewCandidates(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            // Controllo che l'annuncio appartenga al recruiter loggato
            var offer = await _context.JobOffers
                .FirstOrDefaultAsync(j => j.Id == id && j.RecruiterId == user.Id);

            if (offer == null)
                return NotFound();

            // Recupera le candidature legate a quell'offerta
            var applicants = await _context.Candidatures
                .Include(c => c.Candidate)
                .Where(c => c.JobOfferId == id)
                .ToListAsync();


            ViewBag.JobTitle = offer.Title;
            return View("~/Views/JobsRecruiter/ViewCandidates.cshtml", applicants);
        }



    }
}
