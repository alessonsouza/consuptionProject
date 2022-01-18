using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Interfaces.Services;
using backend.Models;
using Dapper;

namespace backend.Services.Security
{
    public class UserGeralService : IUserGeral
    {
        public readonly IConnectionFactory _connection;

        public UserGeralService(IConnectionFactory conn)
        {
            _connection = conn;
        }

        public async Task<IEnumerable<Users>> GetUsers(string username)
        {
            string sql = @"SELECT ";
            var param = new DynamicParameters();
            param.Add(":username", username);


            using (var conn = _connection.Connection())
            {
                try
                {
                    return await conn.QueryAsync<Users>(sql, param);
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }
    }
}