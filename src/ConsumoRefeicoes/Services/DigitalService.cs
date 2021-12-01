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
                                    xap_blob_to_clob(BIO.DESDIG) as DESDIGCONVERT,
                                    FUN.NUMCAD,
                                    FUN.NOMFUN,
                                    BIO.FABBIO,
                                    BIO.TIPTEM
                                from R034BIO BIO
                                JOIN R034FUN FUN ON (FUN.NUMEMP = BIO.NUMEMP AND FUN.TIPCOL = BIO.TIPCOL AND FUN.NUMCAD = BIO.NUMCAD)                                                                          
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
            string sql = @"select   DESDIG,                                    
                                    FUN.NUMCAD || BIO.TIPTEM  NUMCAD,
                                    FUN.NOMFUN,
                                    BIO.FABBIO,
                                    BIO.TIPTEM
                                from R034BIO BIO
                                JOIN R034FUN FUN ON (FUN.NUMEMP = BIO.NUMEMP AND FUN.TIPCOL = BIO.TIPCOL AND FUN.NUMCAD = BIO.NUMCAD)
                                where fun.sitafa <> 7                                              
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
                                    FUN.NUMCAD,
                                    FUN.NOMFUN,
                                    BIO.FABBIO,
                                    BIO.TIPTEM
                                from R034BIO BIO
                                JOIN R034FUN FUN ON (FUN.NUMEMP = BIO.NUMEMP AND FUN.TIPCOL = BIO.TIPCOL AND FUN.NUMCAD = BIO.NUMCAD)                                              
                                WHERE FUN.NUMCAD || BIO.TIPTEM = :numcad                             
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