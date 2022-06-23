using CleverBit.Task1.Data.Shared.Concrete;

namespace CleverBit.Task1.Data.Entities
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
    }
}
