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
    public class RekeningsApiController : ApiController
    {
        private BankContext db = new BankContext();

        // GET: api/Rekenings1
        public IQueryable<Rekening> GetRekeningen()
        {
            return db.Rekeningen;
        }

        // GET: api/Rekenings1/5
        [ResponseType(typeof(Rekening))]
        public IHttpActionResult GetRekening(int id)
        {
            Rekening rekening = db.Rekeningen.Find(id);
            if (rekening == null)
            {
                return NotFound();
            }

            return Ok(rekening);
        }

        // PUT: api/Rekenings1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRekening(int id, Rekening rekening)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rekening.RekeningId)
            {
                return BadRequest();
            }

            db.Entry(rekening).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RekeningExists(id))
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

        // POST: api/Rekenings1
        [ResponseType(typeof(Rekening))]
        public IHttpActionResult PostRekening(Rekening rekening)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rekeningen.Add(rekening);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rekening.RekeningId }, rekening);
        }

        // DELETE: api/Rekenings1/5
        [ResponseType(typeof(Rekening))]
        public IHttpActionResult DeleteRekening(int id)
        {
            Rekening rekening = db.Rekeningen.Find(id);
            if (rekening == null)
            {
                return NotFound();
            }

            db.Rekeningen.Remove(rekening);
            db.SaveChanges();

            return Ok(rekening);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RekeningExists(int id)
        {
            return db.Rekeningen.Count(e => e.RekeningId == id) > 0;
        }
    }
}