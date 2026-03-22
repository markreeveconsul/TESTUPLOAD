using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ENTRYSYSTEM2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string FilePath = "Data/records.json";

        [BindProperty]
        public string StudentName { get; set; }

        [BindProperty]
        public string StudentId { get; set; }

        [BindProperty]
        public string Action { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
        public string Message { get; set; }

        // ===================== PAGE LOAD =====================
        public IActionResult OnGet()
        {
            // 1️⃣ Only allow logged-in admins
            var loggedIn = HttpContext.Session.GetString("LoggedIn");
            var role = HttpContext.Session.GetString("Role");

            if (loggedIn != "true" || role != "Admin")
            {
                return RedirectToPage("/Login/Index");
            }

            // 2️⃣ Load today’s records
            LoadRecords();
            return Page();
        }

        // ===================== FORM SUBMIT =====================
        public IActionResult OnPost()
        {
            // Ensure only admins can post
            var loggedIn = HttpContext.Session.GetString("LoggedIn");
            var role = HttpContext.Session.GetString("Role");

            if (loggedIn != "true" || role != "Admin")
            {
                return RedirectToPage("/Login/Index");
            }

            // Validate form input
            if (string.IsNullOrWhiteSpace(StudentName) || string.IsNullOrWhiteSpace(StudentId))
            {
                Message = "Please fill all fields.";
                LoadRecords();
                return Page();
            }

            // Load all records (history maintained)
            var allRecords = System.IO.File.Exists(FilePath)
                ? JsonSerializer.Deserialize<List<Record>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Record>()
                : new List<Record>();

            // Add new record
            allRecords.Add(new Record
            {
                Name = StudentName,
                StudentId = StudentId,
                Action = Action,
                Timestamp = DateTime.Now
            });

            // Save back to JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            System.IO.File.WriteAllText(FilePath, JsonSerializer.Serialize(allRecords, options));

            Message = $"Student {Action} recorded successfully!";

            // Reload today’s records
            LoadRecords();
            return Page();
        }

        // ===================== LOAD TODAY’S RECORDS =====================
        private void LoadRecords()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                Records = new List<Record>();
                return;
            }

            var allRecords = JsonSerializer.Deserialize<List<Record>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Record>();

            // Filter only today’s records
            Records = allRecords
                .Where(r => r.Timestamp.Date == DateTime.Now.Date)
                .OrderByDescending(r => r.Timestamp)
                .ToList();
        }

        // ===================== OPTIONAL LOGOUT =====================
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login/Index");
        }
    }

    public class Record
    {
        public string Name { get; set; }
        public string StudentId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}