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
    public class LadderController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        [ResponseType(typeof(JObject))]
        public async Task<List<JObject>> GetLadder()
        {
            List<JObject> response = new List<JObject>();
            dynamic user = new JObject();

            user[] users = await db.users.ToArrayAsync();

            foreach (user u in users)
            {
                user = new JObject();
                user.Name = u.username;
                user.Points = u.user_profile.points;
                response.Add(user);
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

        private bool userExists(int id)
        {
            return db.users.Count(e => e.user_id == id) > 0;
        }
    }
}