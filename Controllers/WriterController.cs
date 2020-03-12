using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndyBooks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriterController : ControllerBase
    {
        private IndyBooksDataContext _db;
        public WriterController(IndyBooksDataContext db) {_db = db;}

        // GET: api/Writer
        [HttpGet]
        public ActionResult<IEnumerable<Writer>> Get()
        {
            var writers = _db.Writers.Select(w => new Writer
            {
                Id = w.Id,
                Name = w.Name
            }).ToList();

            return writers;
         }

        // GET: api/Writer/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(long id)
        {
            var writer = _db.Writers.Find(id);

            if (writer == null)
                return NotFound();
            else
                return new JsonResult(writer);
        }

        // POST: api/Writer
        [HttpPost]
        public IActionResult Post([FromBody] Writer writer)
        {
            _db.Writers.Add(new Writer
            {
                Name = writer.Name
            });
            _db.SaveChanges();

            return Accepted();
        }

        // PUT: api/Writer/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Writer writer)
        {
            var writerToUpdate = _db.Writers.FirstOrDefault(w => w.Id == id);
        
            if(writerToUpdate != null)
            {
                writerToUpdate.Name = writer.Name;
                _db.Update(writerToUpdate);
                _db.SaveChanges();
                return Accepted(writerToUpdate);
            }
            else
            {
                return BadRequest();
            }

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var writerToDelete = _db.Writers.FirstOrDefault(a => a.Id == id);
            _db.Writers.Remove(writerToDelete);
            _db.SaveChanges();
        }

        [HttpGet]
        public IActionResult Get(int id, int bookCount)
        {

            var writer = _db.Writers.Include(w => 
            new { 
                bookCount = w.Books.Count()
                })
                .Where(a => a.Id == id);

            return new JsonResult(writer);
                
            
        }
    }
}
