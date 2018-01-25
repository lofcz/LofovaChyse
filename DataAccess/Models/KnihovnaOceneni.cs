using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaOceneni : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int OdznakId { get; set; }
        public virtual int UserId { get; set; }
        public virtual DateTime DatumZiskani { get; set; }

        [AllowHtml]
        public virtual string Venovani { get; set; }
        [AllowHtml]
        public virtual string Text { get; set; }
        public virtual string Image { get; set; }
        [AllowHtml]
        public virtual string Name { get; set; }
    }
}
