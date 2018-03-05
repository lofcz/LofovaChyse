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
        public virtual int ReplyId { get; set; }

        public virtual KnihovnaUser OwnerId{ get; set; }

        // TEMP VLATNOSTi
        public virtual  bool AlreadyRated { get; set; }
        public virtual int RatedType { get; set; }
        public virtual string AjaxUserText { get; set; }

        public enum Reakce
        {
            Super,
            Libi,
            Nadeje,
            Uzitecne
        }

        public virtual int[] PocetReakci { get; set; } = new[] {0, 0, 0, 0};

    }
}
       