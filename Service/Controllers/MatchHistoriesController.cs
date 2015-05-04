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
    public class MatchHistoriesController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        [ResponseType(typeof(JObject))]
        public async Task<List<JObject>> GetMatchHistories(string username)
        {
            List<JObject> response = new List<JObject>();
            dynamic match = new JObject();


            if (userExists(username))
            {
                user user = await (from u in db.users where u.username == username select u).FirstOrDefaultAsync();

                List<match> matches = await (from m in db.matches where m.player1 == user.username || m.player2 == user.username select m).ToListAsync();

                foreach (match m in matches)
                {
                    match = new JObject();
                    match.Player1 = m.player1;
                    match.Player2 = m.player2;
                    match.Winner = m.winner;
                    response.Add(match);
                }
            }

            return response;
        }

        private bool userExists(string username)
        {
            return db.users.Count(e => e.username.Equals(username)) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}