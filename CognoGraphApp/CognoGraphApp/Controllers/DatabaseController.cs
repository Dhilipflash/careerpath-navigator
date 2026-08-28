using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace CognoGraphApp.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly IDriver _driver;

        public DatabaseController(IDriver driver)
        {
            _driver = driver;
        }

        [HttpGet("/database/test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                await _driver.VerifyConnectivityAsync();

                return Content("CognoDB connected successfully!");
            }
            catch
            {
                return StatusCode(
                    500,
                    "CognoDB connection failed. Check URI, username, and password in User Secrets.");
            }
        }
    }
}
