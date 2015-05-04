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
    public class DecksController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        // GET: api/Decks/5
        [ResponseType(typeof(JObject))]
        public async Task<JObject> GetDeck(string username, int deckSlot)
        {
            dynamic response = new JObject();
            int user_id = -1;
            user_id = await (from u in db.users where u.username == username select u.user_id).FirstOrDefaultAsync();

            if (user_id == -1)
            {
                response.response = -1;
            }
            else
            {
                deck deck = await (from d in db.decks where d.user_id == user_id && d.deck_slot == deckSlot select d).FirstOrDefaultAsync();

                if (deck == null)
                {
                    response.response = -2;
                }
                else
                {
                    response.response = deck.deck_name + "," + deck.card_ids;
                }
            }

            return response;
        }

        // PUT: api/Decks/5
        [ResponseType(typeof(JObject))]
        public async Task<JObject> PutDeck(JObject info)
        {
            dynamic response = new JObject();
            int user_id = -1;
            deck newDeck = new deck();
            deck currentDeck;
            string username = info["username"].ToString();
            newDeck.card_ids = info["card_ids"].ToString();
            newDeck.deck_name = info["deck_name"].ToString();
            newDeck.deck_slot = Int32.Parse(info["deck_slot"].ToString());

            try
            {
                if (!userExists(username))
                {
                    response.response = -1;
                    return response;
                }
                else
                {
                    if (newDeck.deck_slot < 0 || newDeck.deck_slot > 7)
                    {
                        response.response = -2;
                        return response;
                    }
                    else
                    {
                        user_id = await (from u in db.users where u.username == username select u.user_id).FirstOrDefaultAsync();
                        newDeck.user_id = user_id;

                        if (DeckExists(user_id, newDeck.deck_slot))
                        {
                            currentDeck = await (from d in db.decks where d.deck_slot == newDeck.deck_slot && d.user_id == user_id select d).FirstOrDefaultAsync();

                            currentDeck.card_ids = newDeck.card_ids;
                            currentDeck.deck_name = newDeck.deck_name;

                            db.Entry(currentDeck).State = EntityState.Modified;
                            try
                            {
                                await db.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.error = ex.InnerException;
            }

            response.response = 0;

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

        private bool DeckExists(int id, int deckSlot)
        {
            return db.decks.Count(e => e.user_id == id && e.deck_slot == deckSlot) > 0;
        }

        private bool userExists(string username)
        {
            return db.users.Count(e => e.username.Equals(username)) > 0;
        }
    }
}