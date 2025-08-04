using JobBoard.Areas.Identity.Data;
using JobBoard.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

public class ProfileCompletionMiddleware
{
    private readonly RequestDelegate _next;

    public ProfileCompletionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<JobBoardUser> userManager, JobBoardContext db)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (context.Response.StatusCode == 302 || context.Response.StatusCode == 301)
            {
                return;
            }


            if (user == null)
            {
                // Utente autenticato ma non presente nel database, può succedere ad esempio dopo un Drop-Database
                context.Response.Redirect("Home/index");
                return;
            }

            if (await userManager.IsInRoleAsync(user, "CANDIDATE"))
            {
                var candidateProfile = await db.CandidateProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (candidateProfile == null && !context.Request.Path.Value.Contains("CandidateProfile"))
                {
                    context.Response.Redirect("/CandidateProfile/CompleteProfile");
                    return;
                }
            }

            if (await userManager.IsInRoleAsync(user, "RECRUITER"))
            {
                var recruiterProfile = await db.RecruiterProfiles.FirstOrDefaultAsync(r => r.UserId == user.Id);
                if (recruiterProfile == null && !context.Request.Path.Value.Contains("RecruiterProfile"))
                {
                    context.Response.Redirect("/RecruiterProfile/CompleteProfile");
                    return;
                }

            }
        }

        await _next(context);
    }
}

