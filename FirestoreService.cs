using Google.Cloud.Firestore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using RestrictionService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace RestrictionService.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ILogger<FirestoreService> _logger;

        public FirestoreService(ILogger<FirestoreService> logger)
        {
            _logger = logger;

            var credential = GoogleCredential.FromFile("serviceAccountKey.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }

            _firestoreDb = FirestoreDb.Create("restrictions-service");
        }

        // 1. Obtener restricciones por studentId
        public async Task<List<Restriction>> GetRestrictions(string studentId)
        {
            try
            {
                CollectionReference restrictionsRef = _firestoreDb.Collection("restrictions");
                Query query = restrictionsRef.WhereEqualTo("studentId", studentId);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                var restrictions = new List<Restriction>();

                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    restrictions.Add(documentSnapshot.ConvertTo<Restriction>());
                }

                return restrictions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting restrictions: {ex.Message}");
                throw;
            }
        }

        // 2. Validar si un estudiante tiene alguna restricción
        public async Task<bool> ValidateStudent(string studentId)
        {
            try
            {
                var restrictions = await GetRestrictions(studentId);
                return restrictions.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating student: {ex.Message}");
                throw;
            }
        }

        // 3. Asignar restricción
        public async Task AssignRestriction(string studentId, string reason)
        {
            try
            {
                CollectionReference restrictionsRef = _firestoreDb.Collection("restrictions");
                var newRestriction = new Restriction
                {
                    StudentId = studentId,
                    Reason = reason,
                    AssignedAt = Timestamp.GetCurrentTimestamp(),
                    RestrictionId = Guid.NewGuid().ToString() 
                };

                await restrictionsRef.AddAsync(newRestriction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning restriction: {ex.Message}");
                throw;
            }
        }

        // 4. Retirar restricción por studentId y restrictionId
        public async Task RemoveRestriction(string studentId, string restrictionId)
        {
            try
            {
                CollectionReference restrictionsRef = _firestoreDb.Collection("restrictions");
                Query query = restrictionsRef.WhereEqualTo("studentId", studentId).WhereEqualTo("restrictionId", restrictionId);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    await documentSnapshot.Reference.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing restriction: {ex.Message}");
                throw;
            }
        }
    }
}
