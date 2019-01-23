using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Entity
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}
