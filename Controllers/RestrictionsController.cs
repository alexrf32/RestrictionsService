using Microsoft.AspNetCore.Mvc;
using RestrictionService.Models;
using RestrictionService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace RestrictionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestrictionsController : ControllerBase
    {
        private readonly FirestoreService _firestoreService;

        public RestrictionsController(FirestoreService firestoreService)
        {
            _firestoreService = firestoreService;
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<List<Restriction>>> GetRestrictions(string studentId)
        {
            var restrictions = await _firestoreService.GetRestrictions(studentId);
            return Ok(restrictions);
        }

        [HttpPost]
        public async Task<ActionResult> AssignRestriction(Restriction restriction)
        {
            restriction.AssignedAt = Timestamp.GetCurrentTimestamp();
            await _firestoreService.AssignRestriction(restriction);
            return Ok();
        }
    }
}
