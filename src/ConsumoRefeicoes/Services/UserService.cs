using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Interfaces.Services;
using backend.Models;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class UserService : IUser
    {
        public readonly IConnectionFactory _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IConnectionFactory conn, IHttpContextAccessor httpContextAccessor)
        {
            _connection = conn;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetSearchUser(int matricula)
        {
            string sql = @"SELECT fun.nomfun as Name,
                                   fun.numcad as Matricula,
                                    fun.SITAFA as Situacao  
                              FROM r034fun fun
                             WHERE fun.numcad = :matricula";
            var param = new DynamicParameters();
            param.Add(":matricula", matricula);

            using (var conn = _connection.Connection())
            {
                try
                {
                    return await conn.QueryAsync<User>(sql, param);
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }

        public int GetRegister(int matricula, string data)
        {
            string sql = @" select count(*) from r070acc
                                where numemp = 1
                                and tipcol = 1
                                and numcad = :matricula
                                and datacc = :data
                                and codrlg <> 20"; ;

            var param = new DynamicParameters();
            param.Add(":matricula", matricula);
            param.Add(":data", data);
            using (var conn = _connection.Connection())
            {
                try
                {
                    return conn.ExecuteScalar<int>(sql, param);
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }

        }

    }
}