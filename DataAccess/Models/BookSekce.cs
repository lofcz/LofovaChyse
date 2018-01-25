using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class BookSekce : IEntity
    {
        public virtual int Id { get; set; }

        [AllowHtml]
        public virtual string Name { get; set; }

        public virtual int ParentId { get; set; }
        public virtual string ImageName { get; set; }
        public virtual int RenderPriority { get; set; }
        public virtual string DebugName { get; set; }

        /*
         * 0 / NULL: Bez
         * 1:        Mod / Adm
         */
        public virtual int Restrikce { get; set; }

        // --------------------- Interní
        public virtual string Subs { get; set; }
        public virtual ActionResult SubsAction { get; set; }
    }
}
