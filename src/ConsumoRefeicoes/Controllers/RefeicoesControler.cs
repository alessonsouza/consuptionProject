using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using backend.Interfaces.Services;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NITGEN.SDK.NBioBSP;

namespace backend.Controllers
{
    [ApiController]
    [Route("/refeicoes")]
    [Authorize]
    public class RefeicoesController : ControllerBase
    {
        public readonly IRefeicoes _refeicoesService;
        public readonly IDigital _digitalService;
        public readonly IUser _userService;
        public readonly ILeitorService _leitorService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserDigitalReceived digital;
        private IActionResult teste;


        public RefeicoesController(IRefeicoes refeicoesService, IDigital digitalService, IUser userService, IHttpContextAccessor httpContextAccessor, ILeitorService leitorService)
        {
            _refeicoesService = refeicoesService;
            _digitalService = digitalService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _leitorService = leitorService;
        }

        [HttpGet("search-user/{matricula}", Name = "GetSearchUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        public async Task<IActionResult> GetSearchUser([FromRoute] int matricula)
        {
            Response response = new Response();
            try
            {
                var resp = await _userService.GetSearchUser(matricula);
                response.Success = true;
                response.Data = resp;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;

                return BadRequest(response);
            }
        }

        [HttpGet("search-item/{codigo}", Name = "GetSearchItem")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShowFoodItems))]
        public async Task<IActionResult> GetSearchItem([FromRoute] int codigo)
        {
            Response response = new Response();
            try
            {
                var resp = await _refeicoesService.GetSearchItem(codigo);
                response.Success = true;
                response.Data = resp;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;

                return BadRequest(response);
            }
        }

        [HttpGet("get-all-food/{competencia}", Name = "GetAllFood")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShowFoodItems))]
        public async Task<IActionResult> GetAllFood([FromRoute] string competencia)
        {
            Response response = new Response();
            try
            {
                var resp = await _refeicoesService.GetAllFood(competencia);
                string ids = "";
                foreach (var item in resp)
                {
                    if (String.IsNullOrEmpty(ids))
                    {
                        ids = item.USU_CODREF.ToString() + ',';
                    }
                    else
                    {

                        ids += item.USU_CODREF;
                        ids += ',';
                    }
                }

                response.Success = true;
                response.Data = resp;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;

                return BadRequest(response);
            }
        }


        [HttpGet("get-food-unused/{ids}", Name = "GetFoodUnused")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShowFoodItems))]
        public async Task<IActionResult> GetFoodUnused([FromRoute] string ids)
        {
            Response response = new Response();
            try
            {
                var resp = await _refeicoesService.GetFoodUnusedService(ids);
                // string ids = "";
                // foreach (var item in resp)
                // {
                //     if (String.IsNullOrEmpty(ids))
                //     {
                //         ids = item.USU_CODREF.ToString() + ',';
                //     }
                //     else
                //     {

                //         ids += item.USU_CODREF;
                //         ids += ',';
                //     }
                // }

                response.Success = true;
                response.Data = resp;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;

                return BadRequest(response);
            }
        }


        [HttpPost("submit", Name = "SaveOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        public object SaveOrder([FromBody] FoodItems post)
        {

            Response response = new Response();
            try
            {
                var resp = _refeicoesService.SaveOrder(post);
                response.Success = true;
                response.Data = resp;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;

                return BadRequest(response);
            }
        }

        [HttpGet("get-meal-by-id/{matricula}/{competencia}", Name = "GetMealById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public object GetMealById([FromRoute] int matricula, string competencia)
        {
            Response response = new Response();
            try
            {
                var resp = _refeicoesService.GetMealById(matricula, competencia);
                response.Success = true;
                response.Data = resp.Result;
                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("get-register/{matricula}/{competencia}", Name = "GetRegister")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public int GetRegister([FromRoute] int matricula, string competencia)
        {
            try
            {
                return _userService.GetRegister(matricula, competencia);

            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        [HttpGet("get-capturar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> Capturar()
        {
            Response response = new Response();
            try
            {
                var resp = await _digitalService.Comparar();
                var respAux = resp;

                foreach (var item in resp)
                {
                    item.DESDIGCONVERT = item.GetDigitalHash();
                }

                response.Success = true;
                response.Data = resp;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;
                return BadRequest(response);
            }
        }

        [HttpPost("get-comparar", Name = "Comparar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> Comparar([FromBody] UserDigital post)
        {
            uint ret;
            bool result;
            NBioAPI m_NBioAPI = new NBioAPI();

            NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
            NBioAPI.Type.HFIR hCapturedFIR1 = new NBioAPI.Type.HFIR();
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.Type.FIR_TEXTENCODE m1_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            m_NBioAPI.Capture(out hCapturedFIR);

            Response response = new Response();
            try
            {
                foreach (var item in post.DESDIGAUX)
                {
                    m_textFIR.TextFIR = item.DESDIGCONVERT;
                    ret = m_NBioAPI.VerifyMatch(hCapturedFIR, m_textFIR, out result, myPayload);
                    if (result)
                    {
                        digital = item;
                        response.Success = result;
                        response.Data = digital;
                    }
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Error = e.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("find-user")]
        public async Task<IActionResult> FindUser()
        {
            Response respose = new Response();

            string digitalCaptured = _leitorService.Capture();
            if (digitalCaptured == null)
            {
                respose.Success = false;
                return BadRequest(respose);
            }
            string numCad = _leitorService.Identify(digitalCaptured);
            UserDigital user = await _digitalService.GetUserByNumCad(numCad);
            if (user.NOMFUN != "")
            {
                respose.Success = true;
                respose.Data = user;

                return Ok(respose);

            }
            else
            {
                respose.Success = false;
                respose.Data = user;

                return BadRequest(respose);
            }

        }
    }
}