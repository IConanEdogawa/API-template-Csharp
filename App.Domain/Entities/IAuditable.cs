using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public interface IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; } 
        public DateTimeOffset UpdatedDate { get; set;}
        public DateTimeOffset DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
