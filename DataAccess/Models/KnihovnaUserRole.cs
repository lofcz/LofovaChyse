using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaUserRole : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }
        public virtual DateTime DateFrom { get; set; }
        public virtual DateTime DateTo { get; set; }
        public virtual int Data { get; set; }
    }
}
