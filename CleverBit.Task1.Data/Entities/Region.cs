using CleverBit.Task1.Data.Shared.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleverBit.Task1.Data.Entities
{
    public class Region : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
        public virtual Region Parent { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Region> Children { get; set; }
    }
}
