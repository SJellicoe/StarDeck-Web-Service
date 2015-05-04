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
    public class WinLossController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        [ResponseType(typeof(JObject))]
        public async Task<JObject> GetWinLoss(string username)
        {
            dynamic response = new JObject();

            if (!userExists(username))
            {
                response.response = -1;
            }
            else
            {
                user_profile userProfile = await (from u in db.users where u.username == username select u.user_profile).FirstOrDefaultAsync();
                response.Wins = userProfile.wins;
                response.Loses = userProfile.loses;
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
    }
}