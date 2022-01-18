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
    public class DigitalService : IDigital
    {
        public readonly IConnectionFactory _connection;
        public IEnumerable<UserDigital> AllUsers { get; set; }

        public DigitalService(IConnectionFactory conn)
        {
            _connection = conn;
        }

        public async Task<IEnumerable<UserDigital>> Comparar()
        {
            string sql = @"select   
                                                                                                             
                                "; ;

            using (var conn = _connection.Connection())
            {
                try
                {
                    var resp = await conn.QueryAsync<UserDigital>(sql);
                    AllUsers = resp;

                    return resp;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }

        public IEnumerable<UserDigital> GetAllUsers()
        {
            string sql = @"select                                                 
                                "; ;


            using (var conn = _connection.Connection())
            {
                try
                {
                    var resp = conn.Query<UserDigital>(sql);


                    return resp;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }

        public async Task<UserDigital> GetUserByNumCad(string numcad)
        {
            string sql = @"select   
                                                               
                                "; ;

            var param = new DynamicParameters();
            param.Add(":numcad", numcad);

            using (var conn = _connection.Connection())
            {
                try
                {
                    var resp = await conn.QueryFirstOrDefaultAsync<UserDigital>(sql, param);


                    return resp;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }
    }
}