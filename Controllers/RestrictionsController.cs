using Microsoft.AspNetCore.Mvc;
using RestrictionService.Models;
using RestrictionService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // 1. Obtener restricciones por studentId
        [HttpGet("{studentId}")]
        public async Task<ActionResult<List<Restriction>>> GetRestrictions(string studentId)
        {
            var restrictions = await _firestoreService.GetRestrictions(studentId);
            return Ok(restrictions);
        }

        // 2. Validar si el estudiante tiene restricciones
        [HttpGet("validate/{studentId}")]
        public async Task<ActionResult<bool>> ValidateStudent(string studentId)
        {
            var hasRestrictions = await _firestoreService.ValidateStudent(studentId);
            return Ok(hasRestrictions);
        }

        // 3. Asignar restricción
        [HttpPost]
        public async Task<ActionResult> AssignRestriction(string studentId, string reason)
        {
           
            await _firestoreService.AssignRestriction(studentId, reason);
            return Ok();
        }

        // 4. Retirar restricción
        [HttpDelete("{studentId}/{restrictionId}")]
        public async Task<ActionResult> RemoveRestriction(string studentId, string restrictionId)
        {
            
            await _firestoreService.RemoveRestriction(studentId, restrictionId);
            return Ok();
        }
    }
}
