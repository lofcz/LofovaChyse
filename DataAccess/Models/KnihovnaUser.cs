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
        public virtual int Age { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual bool DisplayAge { get; set; }
        public virtual bool DisplayName { get; set; }
        public virtual string Education { get; set; }
        public virtual string Hobbies { get; set; }
        public virtual string Literature { get; set; }
        public virtual string Music { get; set; }
        public virtual string Movies { get; set; }
        public virtual int Elan { get; set; }

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
        public virtual DateTime RestrictedTo { get; set; }
        public virtual int ProfileViews { get; set; }
        public virtual int TotalViews { get; set; }
        public virtual DateTime LastLogin { get; set; }
        public virtual string PrimaryIp { get; set; }
        public virtual int RateRemains { get; set; }

        public virtual bool DisplayJob { get; set; }
        public virtual bool DisplayEducation { get; set; }
        public virtual bool DisplayHobbies { get; set; }
        public virtual bool DisplayBooks { get; set; }
        public virtual bool DisplayMusic { get; set; }
        public virtual bool DisplayMovies { get; set; }
        public virtual bool DisplayProfileViews { get; set; }
        public virtual bool DisplayPrivateProfile { get; set; }
        public virtual bool DisplayTextNo { get; set; }
        public virtual bool DisplayStatisticsNo { get; set; }
        public virtual bool DisplayMedalsNo { get; set; }
        public virtual bool DisplayHelpNo { get; set; }


        // Internal shit
        public virtual string _ChatID { get; set; }
    }
}
