using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaKomentare : IEntity
    {
        public virtual int Id { get; set; }

        [AllowHtml]
        public virtual string Content { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual int TopicId { get; set; }

        public virtual KnihovnaUser OwnerId{ get; set; }

        // TEMP VLATNOSTi
        public virtual  bool AlreadyRated { get; set; }
    }
}
       