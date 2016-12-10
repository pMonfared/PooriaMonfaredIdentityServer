﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Services.InMemory;

namespace PooriaMonfaredIdentityServer.DataLayer.Configuration
{
    internal class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "pooria",
                    Password = "newPass@1234",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "p@m.com"),
                        new Claim(JwtClaimTypes.Role, "Badmin")
                    }
                }
            };
        }
    }
}