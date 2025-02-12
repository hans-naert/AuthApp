using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<IdentityUser> Users { get; set; } = new();

        [BindProperty]
        public string SelectedUserId { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedRole { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();

        public async Task OnGet()
        {
            Users = _userManager.Users.ToList();
            Roles = _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<IActionResult> OnPostAssignRole()
        {
            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, SelectedRole);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Role '{SelectedRole}' assigned to {user.Email}";
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveRole()
        {
            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, SelectedRole);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Role '{SelectedRole}' removed from {user.Email}";
                }
            }
            return RedirectToPage();
        }
    }
}
