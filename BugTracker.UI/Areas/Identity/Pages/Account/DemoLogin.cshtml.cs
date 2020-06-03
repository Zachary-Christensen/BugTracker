using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BugTracker.UI.Areas.Identity.Pages.Account
{
    public class DemoLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DemoLoginModel(
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

        }

        public void PopulateRoles()
        {
            ViewData["Roles"] = _roleManager.Roles.ToList().FindAll(
                delegate (IdentityRole role1)
                {
                    return role1.Name.StartsWith("Demo");
                });
        }

        public void OnGet()
        {
            PopulateRoles();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Role, "DemoUser1!", false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            PopulateRoles();
            return Page();
        }
    }
}
