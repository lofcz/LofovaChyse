using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaVoucheryArchetyp : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
