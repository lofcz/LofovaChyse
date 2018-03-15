using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Dao;
using DataAccess.Models;

namespace LofovaChyse.Class
{
    public class UserStats
    {
        public static void NewPost(KnihovnaUser user)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            user.PostsNumber++;
            d.Update(user);
        }

        public static void NewComment(KnihovnaUserDao d, KnihovnaUser user)
        {
            user.CommentsNumber++;
            d.Update(user);
        }

        public static void NewRating(KnihovnaUserDao d, KnihovnaUser user)
        {
            user.LikesNumber++;
            d.Update(user);
        }

        public static bool CanRate(KnihovnaUser user)
        {
            return user.RateRemains > 0;
        }

        public static void NewView(KnihovnaUser user)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            user.TotalViews++;
            d.Update(user);
        }

        public static void NewProfileView(KnihovnaUser user)
        {
            KnihovnaUserDao d = new KnihovnaUserDao();
            user.ProfileViews++;
            d.Update(user);
        }

    }
}