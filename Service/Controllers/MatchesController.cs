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
    public class MatchesController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        [ResponseType(typeof(JObject))]
        public async Task<JObject> Getmatch(int id)
        {
            dynamic response = new JObject();
            match match = await db.matches.FindAsync(id);
            if (match == null)
            {
                return response.response = -1;
            }

            return response.response = 1;
        }

        [ResponseType(typeof(JObject))]
        public async Task<JObject> Postmatch(JObject match)
        {
            dynamic response = new JObject();
            try
            {
                match newMatch = new match();
                user winningUser = new user();
                user losingUser = new user();

                newMatch.player1 = match["Player1"].ToString();
                newMatch.player2 = match["Player2"].ToString();
                if (match["winner"].ToString() != null)
                {
                    newMatch.winner = match["winner"].ToString();
                }
                newMatch.match_date = DateTime.Today;
                newMatch.classes = match["classes"].ToString();

                if (newMatch.winner != null)
                {

                    if (newMatch.player1 == newMatch.winner)
                    {
                        winningUser = await (from u in db.users where u.username == newMatch.player1 select u).FirstOrDefaultAsync();
                        losingUser = await (from u in db.users where u.username == newMatch.player2 select u).FirstOrDefaultAsync();
                    }
                    else if (newMatch.player2 == newMatch.winner)
                    {
                        winningUser = await (from u in db.users where u.username == newMatch.player2 select u).FirstOrDefaultAsync();
                        losingUser = await (from u in db.users where u.username == newMatch.player1 select u).FirstOrDefaultAsync();
                    }

                    winningUser.user_profile.wins += 1;
                    winningUser.user_profile.points += 1;
                    losingUser.user_profile.loses += 1;
                    db.Entry(winningUser).State = EntityState.Modified;
                    db.Entry(losingUser).State = EntityState.Modified;



                    try
                    {
                        db.matches.Add(newMatch);
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        response.response = -1;
                    }
                }
            }
            catch (Exception e)
            {
                response.response = -1;
            }

            response.response = 1;

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

        private bool matchExists(int id)
        {
            return db.matches.Count(e => e.match_id == id) > 0;
        }
    }
}