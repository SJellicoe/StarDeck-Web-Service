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
    public class ClassHistoryController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        // GET: api/Classes/5
        public async Task<JArray> GetClassHistory(string username)
        {
            List<JObject> response = new List<JObject>();
            dynamic Valdorn = new JObject();
            dynamic Lyarn = new JObject();
            dynamic Ozar = new JObject();
            dynamic Draziri = new JObject();

            Valdorn.Name = "Valdorn";
            Lyarn.Name = "Lyarn";
            Ozar.Name = "Ozar";
            Draziri.Name = "Draziri";
            Valdorn.Plays = 0;
            Lyarn.Plays = 0;
            Ozar.Plays = 0;
            Draziri.Plays = 0;

            if (userExists(username))
            {
                try
                {
                    user user = await (from u in db.users where u.username == username select u).FirstOrDefaultAsync();

                    List<match> matches = await (from m in db.matches where m.player1 == user.username || m.player2 == user.username select m).ToListAsync();

                    foreach (match m in matches)
                    {
                        if (m.player1 == username)
                        {
                            switch (m.classes.Split('|')[0])
                            {
                                case "Valdorn":
                                    Valdorn.Plays = Valdorn.Plays + 1;
                                    break;
                                case "Lyarn":
                                    Lyarn.Plays = Lyarn.Plays + 1;
                                    break;
                                case "Ozar":
                                    Ozar.Plays = Ozar.Plays + 1;
                                    break;
                                case "Draziri":
                                    Draziri.Plays = Draziri.Plays + 1;
                                    break;
                            }
                        }
                        else
                        {
                            switch (m.classes.Split('|')[1])
                            {
                                case "Valdorn":
                                    Valdorn.Plays = Valdorn.Plays + 1;
                                    break;
                                case "Lyarn":
                                    Lyarn.Plays = Lyarn.Plays + 1;
                                    break;
                                case "Ozar":
                                    Ozar.Plays = Ozar.Plays + 1;
                                    break;
                                case "Draziri":
                                    Draziri.Plays = Draziri.Plays + 1;
                                    break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    
                }
            }

            response.Add(Valdorn);
            response.Add(Lyarn);
            response.Add(Ozar);
            response.Add(Draziri);

            return JArray.FromObject(response);
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

        private bool userExists(int id)
        {
            return db.users.Count(e => e.user_id == id) > 0;
        }
    }
}