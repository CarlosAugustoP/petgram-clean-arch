using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Domain.Models.UserAggregate;
namespace Domain.Models
{
    public class Preference {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int DogMeter { get; set; }
        public int CatMeter { get; set; }
        public int BirdMeter { get; set; }
        public int FishMeter { get; set; }
        public int ReptileMeter { get; set; }
        public int RodentMeter { get; set; }
        public int InsectMeter { get; set; }
    }
    
}
