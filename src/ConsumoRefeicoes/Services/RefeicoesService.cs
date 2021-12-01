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
            string sql = @" SELECT  distinct trf.Codref as USU_CODREF,
                                     trf.Desref as Descricao,
                                     trf.numemp as USU_NUMEMP, 
                                     acc.tipcol as USU_TIPCOL    
                                From r068trf  trf
                                Join r070acc acc ON trf.codref = acc.codref 
                                 AND trf.numemp = acc.numemp
                               where Trf.Codref = :codigo
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
            string sql = @"select USU_CODREF, count(*)soma, Descricao, USU_NUMEMP, USU_TIPCOL  from
                                  (select  trf.codref as USU_CODREF,  
                                        trf.codref as soma,
                                        trf.Desref as Descricao,
                                        trf.numemp as  USU_NUMEMP,
                                        acc.tipcol as USU_TIPCOL                                   
                                     From r068trf  trf
                                       left Join r070acc acc
                                        ON trf.codref = acc.codref 
                                       AND trf.numemp = acc.numemp 
                                     WHERE to_char(acc.datapu,'YYYYMMDD HH:mm') >= :dataInicio
                                     AND TO_CHAR(acc.datapu, 'YYYYMMDD HH:mm') < :dataFim
                                     AND acc.codrlg    = 20 
                                     and trf.USU_REFATI = 'S'
                                  ) GROUP BY USU_CODREF, Descricao, USU_NUMEMP, USU_TIPCOL ORDER BY soma desc";

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
            string sql2 = @" select  trf.codref as USU_CODREF,  
                                        1 as soma,
                                        trf.Desref as Descricao,
                                        trf.numemp as  USU_NUMEMP,
                                      1 as USU_TIPCOL                                   
                                     From r068trf  trf                                    
                                   where trf.codref not in (" + ids2.Replace("'", "") + @")
                                   and trf.USU_REFATI = 'S'";
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
            string sql = @"select *
                                 from USU_TCONREF
                                where usu_numcad = :matricula
                                  and usu_datcon = :data";

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
                            sql = @"Insert into USU_TCONREF (USU_NUMEMP, USU_TIPCOL, USU_NUMCAD, USU_DATCON, USU_HORCON, USU_TPCAPT) 
                                                        values (:USU_NUMEMP, :USU_TIPCOL, :USU_NUMCAD, :USU_DATCON, :USU_HORCON, :USU_TPCAPT)";
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
                            sql = @"Insert into USU_TCONFRI (USU_NUMEMP, USU_TIPCOL, USU_NUMCAD, USU_DATCON, USU_HORCON, USU_CODREF, USU_QTDREF, USU_TPCAPT) 
                                                    values (:USU_NUMEMP, :USU_TIPCOL, :USU_NUMCAD, :USU_DATCON, :USU_HORCON, :USU_CODREF, :USU_QTDREF, :USU_TPCAPT)";
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