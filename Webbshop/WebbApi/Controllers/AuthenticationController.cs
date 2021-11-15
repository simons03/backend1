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
using WebbApi.Models.authentication;
using WebbApi.Models.UserModels;

namespace WebbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SqlContext _context;

        public AuthenticationController(SqlContext context)
        {
            _context = context;
        }


        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {
            if(!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
            {
                var _userExists = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                if(_userExists == null)
                {
                    UserAddress userAddress = new UserAddress();
                    userAddress = await _context.UserAddresses.Where(x => x.Adress == model.Address && x.City == model.City).FirstOrDefaultAsync();

                    if (userAddress == null)
                    {
                        var _address = _context.UserAddresses.Add(new UserAddress
                        {
                            Adress = model.Address,
                            Zip = model.Zip,
                            City = model.City
                        });
                        await _context.SaveChangesAsync();
                        userAddress = await _context.UserAddresses.Where(x => x.Adress == model.Address && x.City == model.City).FirstOrDefaultAsync();
                    }

                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Admin = model.Admin,
                        UserAddressesId = userAddress.Id
                    };




                    // Här kan det vara problem kan behöva mer saker...
                    // return CreatedAtAction("SignIn", user);

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();


                    var signInModel = new signinModel
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    var loggin = await SignIn(signInModel);

                    return CreatedAtAction("SignIn", loggin.Result);

                }
                else
                {
                    return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = "A user with the same email address aldready exists" }));
                }
            }
            else
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = "All required fields are not set" }));
            }
        }




        // POST: api/Authentication
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("SignIn")]
        public async Task<ActionResult<User>> SignIn(signinModel model)
        {

            var user = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if(user != null)
            {
                if(user.Password == model.Password)
                {

                    return new OkObjectResult(JsonConvert.SerializeObject(new { userId = user.Id, Admin = user.Admin, sessionId = Guid.NewGuid().ToString() }));
                }

                return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = "Email or password is incorrect" }));

            }
            return new BadRequestObjectResult(JsonConvert.SerializeObject(new { message = "Email or password is incorrect" }));
        }

    }
}
