using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class KlantsApiController : ApiController
    {
        private BankContext db = new BankContext();

        // GET: api/Klants1
        public IQueryable<Klant> GetKlanten()
        {
            return db.Klanten;
        }

        // GET: api/Klants1/5
        [ResponseType(typeof(Klant))]
        public IHttpActionResult GetKlant(int id)
        {
            Klant klant = db.Klanten.Find(id);
            if (klant == null)
            {
                return NotFound();
            }

            return Ok(klant);
        }

        // PUT: api/Klants1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKlant(int id, Klant klant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != klant.KlantId)
            {
                return BadRequest();
            }

            db.Entry(klant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Klants1
        [ResponseType(typeof(Klant))]
        public IHttpActionResult PostKlant(Klant klant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Klanten.Add(klant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = klant.KlantId }, klant);
        }

        // DELETE: api/Klants1/5
        [ResponseType(typeof(Klant))]
        public IHttpActionResult DeleteKlant(int id)
        {
            Klant klant = db.Klanten.Find(id);
            if (klant == null)
            {
                return NotFound();
            }

            db.Klanten.Remove(klant);
            db.SaveChanges();

            return Ok(klant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KlantExists(int id)
        {
            return db.Klanten.Count(e => e.KlantId == id) > 0;
        }
    }
}