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
    public class CardsController : ApiController
    {
        private stardeckEntities db = new stardeckEntities();

        // GET: api/Cards
        public async Task<JArray> GetCards()
        {
            List<JObject> retCards = new List<JObject>();
            dynamic card = new JObject();

            try
            {
                card[] cards = await db.cards.ToArrayAsync<card>();

                foreach (card c in cards)
                {
                    card = new JObject();
                    card.card_id = c.card_id;
                    card.card_name = c.card_name;
                    card.card_type = c.card_type;
                    card.cost = c.cost;
                    switch (c.card_type)
                    {
                        case 1:
                            card.attack = c.fighter.attack;
                            card.health = c.fighter.health;
                            card.shield = c.fighter.shield;
                            break;
                        case 2:
                            card.damage = c.weapon.damage;
                            card.health = c.weapon.health;
                            break;
                        case 3:
                            card.shield_restore = c.healing.shield_restore;
                            card.hull_restore = c.healing.hull_restore;
                            card.health = c.healing.health;
                            break;
                    }
                    retCards.Add(card);
                }
            }
            catch (Exception e)
            {
                card.message = e.InnerException;
                retCards.Add(card);
            }

            return JArray.FromObject(retCards);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardExists(int id)
        {
            return db.cards.Count<card>(e => e.card_id == id) > 0;
        }
    }
}