using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApi.Interface;
using LoginApi.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly EshoppingContext _context;
        private readonly IJWTManagerRepository _iJWTManagerRepository;

        public LoginController(EshoppingContext context, IJWTManagerRepository iJWTManagerRepository)
        {
            _context = context;
            _iJWTManagerRepository = iJWTManagerRepository;
        }

        [Authorize]
        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblLogin>>> GetTblLogins()
        {
          if (_context.TblLogins == null)
          {
              return NotFound();
          }
            return await _context.TblLogins.ToListAsync();
        }
        [Authorize]
        // GET: api/Login/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblLogin>> GetTblLogin(int id)
        {
          if (_context.TblLogins == null)
          {
              return NotFound();
          }
            var tblLogin = await _context.TblLogins.FindAsync(id);

            if (tblLogin == null)
            {
                return NotFound();
            }

            return tblLogin;
        }

        // PUT: api/Login/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTblLogin(int id, TblLogin tblLogin)
        {
            if (id != tblLogin.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblLogin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblLoginExists(id))
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

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TblLogin>> PostTblLogin(TblLogin tblLogin)
        {
          if (_context.TblLogins == null)
          {
              return Problem("Entity set 'EshoppingContext.TblLogins'  is null.");
          }
            _context.TblLogins.Add(tblLogin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblLogin", new { id = tblLogin.Id }, tblLogin);
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTblLogin(int id)
        {
            if (_context.TblLogins == null)
            {
                return NotFound();
            }
            var tblLogin = await _context.TblLogins.FindAsync(id);
            if (tblLogin == null)
            {
                return NotFound();
            }

            _context.TblLogins.Remove(tblLogin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblLoginExists(int id)
        {
            return (_context.TblLogins?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost("get-token")]
        public async Task<IActionResult> GetToken(LoginViewModel login)
        {
            var token = _iJWTManagerRepository.Authenticate(login);
            if (!String.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
