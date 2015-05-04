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

namespace Service.Controllers
{
    public class LoginController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        // POST: api/Login
        [ResponseType(typeof(JObject))]
        public async Task<JObject> PostLogin(JObject user)
        {
            dynamic response = new JObject();
            if (user["username"] != null)
            {
                using (db = new stardeckEntities())
                {
                    string username = user["username"].ToString();

                    if (userExists(username))
                    {
                        user userFromDB = await (from u in db.users where u.username == username select u).FirstOrDefaultAsync();

                        string sentpass = user["password"].ToString();
                        string password = userFromDB.password;

                        if (password == sentpass)
                        {
                            response.response = 1;
                        }
                        else
                        {
                            response.response = 0;
                        }
                    }
                    else
                    {
                        response.response = -1;
                    }
                }
            }
            else
            {
                response.response = -2;
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

        private bool userExists(int id)
        {
            return db.users.Count(e => e.user_id == id) > 0;
        }
    }
}