using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using DataAccess.Interface;

namespace DataAccess.Models
{
    public class KnihovnaUser : MembershipUser, IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual KnihovnaRole Role { get; set; }

        [AllowHtml]
        public virtual string WelcomeText { get; set; }

        public virtual int AuthLevel { get; set; }
        public virtual int Exp { get; set; }
        public virtual DateTime JoinedDateTime { get; set; }
        public virtual int PostsNumber { get; set; }
        public virtual int CommentsNumber { get; set; }
        public virtual int LikesNumber { get; set; }
        public virtual int Reputation { get; set; }
        public virtual double Money { get; set; }
        public virtual string ImageName { get; set; }
        public virtual int NeedJob { get; set; }
    }
}
