using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Interfaces.Services;
using backend.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using backend.Helper;

namespace backend.Services
{
    public class RefeicoesService : IRefeicoes
    {
        public readonly IConnectionFactory _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefeicoesService(IConnectionFactory conn, IHttpContextAccessor httpContextAccessor)
        {
            _connection = conn;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ShowFoodItems>> GetSearchItem(int codigo)
        {
            string sql = @" 
                            ";
            var param = new DynamicParameters();
            param.Add(":codigo", codigo);

            using (var conn = _connection.Connection())
            {
                try
                {
                    return await conn.QueryAsync<ShowFoodItems>(sql, param);
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }

        public async Task<IEnumerable<ShowFoodItems>> GetAllFood(string competencia)
        {
            var dataFim = CompetenciaHelper.GetEndDate(competencia); //"25/04/2021";
            var dataInicio = CompetenciaHelper.GetStartDate(competencia);
            string sql = @"";

            var param = new DynamicParameters();
            param.Add(":dataInicio", dataInicio);
            param.Add(":dataFim", dataFim);

            using (var conn = _connection.Connection())
            {
                try
                {
                    var resp = await conn.QueryAsync<ShowFoodItems>(sql, param);
                    return resp;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }

        }


        public async Task<IEnumerable<ShowFoodItems>> GetFoodUnusedService(string ids2)
        {
            string sql2 = @"                                     
                                  in (" + ids2.Replace("'", "") + @")
                                   = 'S'";
            using (var conn = _connection.Connection())
            {
                try
                {
                    var resp2 = await conn.QueryAsync<ShowFoodItems>(sql2);
                    return resp2;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }

        }
        public Task<IEnumerable<FoodItems>> GetMealById(int matricula, string data)
        {
            string sql = @"";

            var param = new DynamicParameters();
            param.Add(":matricula", matricula);
            param.Add(":data", data);
            using (var conn = _connection.Connection())
            {
                try
                {
                    return conn.QueryAsync<FoodItems>(sql, param);
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }
        }
        public object SaveOrder(FoodItems post)
        {
            var objt = DateTime.Now;
            var data = objt.Date;
            var user = _httpContextAccessor.HttpContext?.User;
            string userName = user.Identity.Name;
            string sql = "";
            int quantidade = 0;

            using (var conn = _connection.Connection())
            {
                try
                {
                    foreach (var item in post.Food)
                    {
                        if (item.USU_CODREF < 6)
                        {
                            sql = @"Insert into ";
                            var obj = new
                            {
                                USU_NUMEMP = item.USU_NUMEMP,
                                USU_TIPCOL = item.USU_TIPCOL,
                                USU_NUMCAD = post.USU_NUMCAD,
                                USU_DATCON = post.USU_DATCON,
                                USU_HORCON = post.USU_HORCON,
                                USU_TPCAPT = post.USU_TPCAPT
                            };
                            quantidade = conn.Execute(sql, obj);
                        }
                        else
                        {
                            sql = @"Insert into ";
                            var obj = new
                            {
                                USU_NUMEMP = item.USU_NUMEMP,
                                USU_TIPCOL = item.USU_TIPCOL,
                                USU_NUMCAD = post.USU_NUMCAD,
                                USU_DATCON = post.USU_DATCON,
                                USU_HORCON = post.USU_HORCON,
                                USU_CODREF = item.USU_CODREF,
                                USU_QTDREF = item.USU_QTDREF,
                                USU_TPCAPT = post.USU_TPCAPT
                            };
                            quantidade = conn.Execute(sql, obj);
                        }
                    }
                    return quantidade > 0;
                }
                catch (Exception e)
                {

                    throw new ApplicationException(e.Message);
                }
            }

        }

    }
}