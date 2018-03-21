using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaVoucheryArchetyp : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Type { get; set; }

        [AllowHtml]
        public virtual string Name { get; set; }

        [AllowHtml]
        public virtual string Description { get; set; }
    }
}
