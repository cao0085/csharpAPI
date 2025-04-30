using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using RestApiPractice.Providers;

namespace RestApiPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestFirestoreController : ControllerBase
    {
        private readonly FirestoreDb _firestoreDb;

        public TestFirestoreController(IFirestoreProvider firestoreProvider)
        {
            _firestoreDb = firestoreProvider.GetFirestoreDb();
            Console.WriteLine("TestFirestoreController");
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                // 嘗試讀取一個不存在的 collection
                var snap = await _firestoreDb.Collection("health_check").Limit(1).GetSnapshotAsync();
                return Ok("✅ Firestore connected successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Firestore connection failed: {ex.Message}");
            }
        }
    }
}