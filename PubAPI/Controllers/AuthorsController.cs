using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PublisherData;
using PublisherDomain;

namespace PubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly PubContext _context;

        public AuthorsController(PubContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        //{
        //    return await _context.Authors.ToListAsync();
        //    //return await _context.Authors.Include(a => a.Books).ToListAsync();

        //}

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            //var authorList = await _context.Authors.ToListAsync();
            //var authorDTOList = new List<AuthorDTO>();
            //foreach (var author in authorList)
            //{
            //    authorDTOList.Add(AuthorToDTO(author));
            //}
            //return authorDTOList;

            return await _context.Authors
                .Select(a => new AuthorDTO
                {
                    AuthorId = a.AuthorId,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Author>> GetAuthor(int id)
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            //return author;
            return AuthorToDTO(author);
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDTO)
        {
            if(id != authorDTO.AuthorId)
            {
                return BadRequest();
            }

            Author author = AuthorFromDTO(authorDTO);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDTO)
        {
            var author = AuthorFromDTO(authorDTO);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAuthor", new {id = author.AuthorId}, AuthorToDTO(author));
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound(); // NotFound() part of ControllerBase
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }

        private static AuthorDTO AuthorToDTO(Author author)
        {
            return new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName
            };

        }

        private static Author AuthorFromDTO(AuthorDTO authorDTO)
        {
            return new Author
            {
                AuthorId = authorDTO.AuthorId,
                FirstName = authorDTO.FirstName,
                LastName = authorDTO.LastName
            };

        }
    }
}
