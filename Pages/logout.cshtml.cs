using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor01.Pages
{
    public class logoutModel : PageModel
    {
        //public void OnGet()
        //{
        //}

        public IActionResult OnGet()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to login with message
            return RedirectToPage("/login", new { err = "You have been logged out." });
        }
    }
}
