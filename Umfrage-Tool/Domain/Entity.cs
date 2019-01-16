using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Entity
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}
