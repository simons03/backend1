using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebbApi.Data;
using WebbApi.Entities;
using WebbApi.Models.UserModels;

namespace WebbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SqlContext _context;

        public UsersController(SqlContext context)
        {
            _context = context;
        }










        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUsers>>> GetUsers()
        {
            var Users = new List<GetUsers>();

            foreach (var item in await _context.Users.ToListAsync())
            {
                Users.Add(new Models.UserModels.GetUsers
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    // Password = item.Password,
                    Admin = item.Admin,
                    UserAddressesId = item.UserAddressesId
                });
            }

            return Users;
        }






        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUsers>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);



            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var newUser = new GetUsers
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    // Password = user.Password,
                    Admin = user.Admin,
                    UserAddressesId = user.UserAddressesId
                };
                return newUser;

            }
        }







        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User model)
        {

            var Olduser = await _context.Users.FindAsync(id);


            if (id != Olduser.Id)
            {
                return BadRequest();
            }

            var user = new User
            {
                Id = Olduser.Id,
                FirstName = Olduser.FirstName,
                LastName = Olduser.LastName,
                Email = Olduser.Email,
                Password = Olduser.Password,
                Admin = model.Admin,
                UserAddressesId = Olduser.UserAddressesId
            };


            _context.Entry(Olduser).State = EntityState.Detached;


            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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




       

        
   


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
    //    public async Task<ActionResult<User>> PostUser(User user)
    //   {

            // VG DEL

           // return new NotFoundObjectResult(user);



            //if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName) && !string.IsNullOrWhiteSpace(user.Password)) 
            //{
                
            //    var exists = _context.UserAddresses.Where(x => x.Adress == user.UserAddresses.Adress).FirstOrDefault();
            //    var patternEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            //    // krav: En stor bokstav, En liten bokstav, Ett specialtecken, Minst 8 tecken långt, Max 15 tecken långt
            //    var patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";
            //    if (Regex.IsMatch(user.Email, patternEmail) && Regex.IsMatch(user.Password, patternPassword))
            //    {
            //        var emailExists = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();

            //        if (emailExists.Email == user.Email)
            //        {
            //            return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"Din Epostadress finns redan registrerad" }));
            //        }

            //        if (exists == null)
            //        {
            //            _context.Users.Add(user);
            //            await _context.SaveChangesAsync();
            //            return CreatedAtAction("GetUser", new { id = user.Id }, user);
            //        }
            //        var newUser = new User
            //        {
            //            FirstName = user.FirstName,
            //            LastName = user.LastName,
            //            Email = user.Email,
            //            Password = user.Password,
            //            Admin = user.Admin,
            //            UserAddressesId = exists.Id
            //        };
            //        _context.Users.Add(newUser);
            //        await _context.SaveChangesAsync();
            //        return CreatedAtAction("GetUser", new { id = user.Id }, newUser);
            //      //  return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"Ditt lösenord uppnår kraven för ett start lösenord" }));
            //    }
            //    else
            //    {
            //        return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"Din Email eller Lösernord uppfyller inte kraven" }));

            //    }
            //}
            //return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = $"All fields must contain values." }));
     //  }







        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
