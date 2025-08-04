using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Areas.Identity.Data;

// Add profile data for application users by adding properties to the JobBoardUser class
public class JobBoardUser : IdentityUser
{

    public bool IsApprovedRecruiter { get; set; } = false;

}

