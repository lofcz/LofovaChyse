using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
     public class KnihovnaVouchery : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual bool Used { get; set; }
        public virtual int UsedId { get; set; }
        public virtual DateTime UsedDate { get; set; }
        public virtual int Type { get; set; }
    }
}
