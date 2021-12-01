using System;
using System.Threading.Tasks;
using backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NITGEN.SDK.NBioBSP;

namespace backend.Controllers
{
    [ApiController]
    [Route("/fake")]
    [Authorize]
    public class FakeController : ControllerBase
    {
        public readonly IDigital _digitalService;
        public FakeController(IDigital digitalService)
        {
            _digitalService = digitalService;
        }

        [HttpGet("compare")]
        [AllowAnonymous]
        public async Task<IActionResult> Compare()
        {
            uint ret;
            bool result;

            NBioAPI m_NBioAPI = new NBioAPI();
            // m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();



            var resp = await _digitalService.Comparar();
            var respAux = resp;

            foreach (var item in resp)
            {
                string hashLeitor = "QVFBQUFCUUFBQUQwQUFBQUFRQVNBQUVBWWdBQUFBQUE1QUFBQUxKbDMwekFrUGdKN2NGYWJuWXB6NjFHTk56RXhkNFlHb0pCb3pySmZTeVN1S0R4V1REMTV3dHMxSUw4UjBaRkhuOG9aaTRMQUZEQzRZVW9SUUxIbzcwdFE3RGRYKndoMTFlM0VPU3BOMmxWUWFpTjl1LzR1YkRkSVRTNXU5WFRUVVc0cGtCSG1EYWVGanMwZmh6aVZMQW1GZlBJN3JRc3gxa29KOUtrY1U4cGxoRFhjaldRZjcxTVlySFQwY2syUS9TKjByTFV4MFdFcGc0RmtKb0o1cFF3c3JzUDZzbTlsSnFyQm0wWmx2M1ZVWUZrc1FJbEp6VlpyUUttd2RIVFhyajZrdlhubXNhc0d4VkxYUmFBQjV5YWJaMkZVMm1jeDl5T2RwZi8xZWQ0MXQ0NWZtZEhPbWFoL0YzenRlTi9pZw";
                string hash = item.DESDIGCONVERT;

                m_textFIR.TextFIR = hashLeitor;
                ret = m_NBioAPI.VerifyMatch(hCapturedFIR, m_textFIR, out result, myPayload);

                if (result)
                {
                    return Ok(item.NOMFUN);
                }
                Console.WriteLine(item.NOMFUN + " " + result);
            }

            return Ok("TESTE");
        }
    }
}