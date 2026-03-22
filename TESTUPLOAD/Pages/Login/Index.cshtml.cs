using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ENTRYSYSTEM2.Pages.Login
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        // Path to JSON storing admin accounts
        private string UsersFilePath => "Data/Admins.json";

        public IActionResult OnPost()
        {
            // Load admins from JSON
            var users = new List<UserAccount>();
            if (System.IO.File.Exists(UsersFilePath))
            {
                var json = System.IO.File.ReadAllText(UsersFilePath);
                users = JsonSerializer.Deserialize<List<UserAccount>>(json) ?? new List<UserAccount>();
            }

            // Check credentials
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                // Set session to allow admin access
                HttpContext.Session.SetString("LoggedIn", "true");
                HttpContext.Session.SetString("Role", user.Role); // Always "Admin"
                return RedirectToPage("/Index"); // go to main page
            }
            else
            {
                Message = "Invalid username or password!";
                return Page();
            }
        }
    }

    // Admin user model
    public class UserAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Always "Admin"
    }
}