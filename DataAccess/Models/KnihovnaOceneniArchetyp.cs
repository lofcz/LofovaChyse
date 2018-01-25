using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaOceneniArchetyp : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int OdznakId { get; set; }
        [AllowHtml]
        public virtual string Text { get; set; }
        public virtual string Image { get; set; }
        [AllowHtml]
        public virtual string Name { get; set; }
    }
}
