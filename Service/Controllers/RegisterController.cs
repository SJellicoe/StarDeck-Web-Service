using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Service;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace Service.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RegisterController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        // POST: api/Register
        [ResponseType(typeof(JObject))]
        public async Task<JObject> Postuser(JObject User)
        {
            dynamic response = new JObject();
            user user = new user();

            try
            {
                user.username = User["username"].ToString();
                user.user_email = User["email"].ToString();
                if (userExists(user.username))
                {
                    response.response = -1;
                }
                else if (emailIsTaken(user.user_email))
                {
                    response.response = -2;
                }
                else
                {
                    user.password = User["password"].ToString();
                    user.country = User["country"].ToString();
                    user.DOB = (DateTime)User["DOB"];
                    user.profile_pic = User["profile_pic"].ToString();

                    db.users.Add(user);
                    await db.SaveChangesAsync();

                    if (userExists(user.username))
                    {
                        response.response = 1;
                    }
                    else
                    {
                        response.response = -3;
                    }
                }
            }
            catch(Exception e)
            {
                response.response = -4;
                return response;
            }

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(string username)
        {
            return db.users.Count(e => e.username.Equals(username)) > 0;
        }

        private bool emailIsTaken(string email)
        {
            return db.users.Count(e => e.user_email.Equals(email)) > 0;
        }

        private bool userExists(int id)
        {
            return db.users.Count(e => e.user_id == id) > 0;
        }
    }
}