using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace WebFox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnsafeSqliInjection : ControllerBase
    {
        [HttpGet("{id}")]
        public string DoSqli(string id)
        {
            string conString = "I AM a connection String";
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE userId = '" + id + "'"))
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    cmd.Connection = con;
                    SqlDataReader reader = cmd.ExecuteReader();
                    string res = "";
                    while (reader.Read())
                    {
                        res += reader["userName"];
                    }
                    return res;
                }
            }
        }

        [HttpGet("{username}")]
        public string DoSqli(string username)
        {
            string insecure_query = "SELECT * FROM users WHERE userId = '" + username + "'";
            insecure_command = sqcontext.Database.ExecuteSqlCommand(insecure_query);
        }
    }

    public class SafeSqli
    {
        // This is not deterministic.
        // Maybe username is not a user parameter, but only called within the code
        public string Sqli(string username)
        {
            string insecure_query = "SELECT * FROM users WHERE userId = '" + username + "'";
            insecure_command = sqcontext.Database.ExecuteSqlCommand(insecure_query);
        }
    }
}
