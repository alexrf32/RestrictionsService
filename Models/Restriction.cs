using Google.Cloud.Firestore;

namespace RestrictionService.Models
{
    [FirestoreData]
    public class Restriction
    {
         [FirestoreProperty]
        public string? RestrictionId { get; set; } 
        
        [FirestoreProperty]
        public string? StudentId { get; set; }

        [FirestoreProperty]
        public string? Reason { get; set; }

        [FirestoreProperty]
        public Timestamp AssignedAt { get; set; } = Timestamp.FromDateTime(DateTime.UtcNow); 
    }
}
