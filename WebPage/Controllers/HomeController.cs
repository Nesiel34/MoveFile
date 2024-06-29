using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebPage.Models;

namespace WebPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ButtonClick()
        {
            // כאן נבצע את הפעולות שברצונך שהכפתור יבצע
            // לדוגמה, שמירת נתונים או עיבוד כלשהו
            string batFilePath = @"D:\Libraries\Desktop\files\test.bat";

            // יצירת אובייקט ProcessStartInfo והגדרת התכונות שלו
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = batFilePath;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            try
            {
                using (Process process = Process.Start(processInfo))
                {
                    process.OutputDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
                    process.ErrorDataReceived += (sender, e) => { if (e.Data != null) Console.WriteLine("ERROR: " + e.Data); };

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    int exitCode = process.ExitCode;
                    Console.WriteLine($"BAT file exited with code {exitCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return View("Index"); // להחזיר את ה-View המתאים
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
