using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiddlewareHomeWork.DataAccess;
using MiddlewareHomeWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareHomeWork
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            this._next = next;
            
        }

        public async Task InvokeAsync(HttpContext context, UserContext _context)
        {
            var method = context.Request.Method;
            var DataLogin = context.Request.Query["login"];
            var DataPassword = context.Request.Query["password"];
            var login = DataLogin.ToString();
            var path = context.Request.Path;
           var password = DataPassword.ToString();
            if(path == "/api/Service" && method=="GET")
            {
                var users = _context.Users.ToList();
                foreach (var user in users)
                {
                    if (user.IsAuth)
                    {
                        await context.Response.WriteAsync("Info:\nLogin - "+user.Login+"\nPassword - " + user.Password);
                        user.IsAuth = false;
                        _context.SaveChanges();
                    }
                }
                
            }
           else if(method == "GET")
            {
            
            var users = _context.Users.ToList();

                foreach (var user in users)
                {
                    if (user.Login == login && user.Password == password)
                    {
                        //await _next.Invoke(context);
                        user.IsAuth = true;
                        _context.SaveChanges();
                await _next.Invoke(context);
            
                    }
                }
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Error - 401");
            }
            else if(method=="POST")
            {
                User user = new User { Login = login, Password = password };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}





