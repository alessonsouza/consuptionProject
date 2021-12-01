using System;
using System.Collections.Generic;
using backend.Interfaces.Services;
using backend.Models;
using Microsoft.Extensions.Logging;
using NITGEN.SDK.NBioBSP;


namespace backend.Services
{
    public class LeitorService : ILeitorService
    {
        protected readonly ILogger<LeitorService> _logger;

        protected NBioAPI m_NBioAPI { get; set; }
        protected NBioAPI.Type.HFIR hCapturedFIR { get; set; }
        protected NBioAPI.Type.HFIR hCapturedFIR1 { get; set; }
        protected NBioAPI.Type.FIR_TEXTENCODE m_textFIR { get; set; }
        protected NBioAPI.Type.FIR_TEXTENCODE m1_textFIR { get; set; }
        protected NBioAPI.Type.FIR_PAYLOAD myPayload { get; set; }
        protected NBioAPI.IndexSearch m_IndexSearch { get; set; }

        public LeitorService(ILogger<LeitorService> logger)
        {
            _logger = logger;
            _logger.LogInformation("Inicializando leitor");

        }
        public void Initialize()
        {
            m_NBioAPI = new NBioAPI();
            NBioAPI.Type.INIT_INFO_0 initInfo0;
            uint ret = m_NBioAPI.GetInitInfo(out initInfo0);
            if (ret == NBioAPI.Error.NONE)
            {
                initInfo0.EnrollImageQuality = Convert.ToUInt32(50);
                initInfo0.VerifyImageQuality = Convert.ToUInt32(30);
                initInfo0.DefaultTimeout = Convert.ToUInt32(10000);
                initInfo0.SecurityLevel = (int)NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL - 1;
            }

            hCapturedFIR = new NBioAPI.Type.HFIR();
            m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            m1_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            myPayload = new NBioAPI.Type.FIR_PAYLOAD();
            m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            m_IndexSearch.InitEngine();

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);

        }
        public string Capture()
        {
            NBioAPI.Type.HFIR _hCapturedFIR = new NBioAPI.Type.HFIR();
            // NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();

            uint ret = m_NBioAPI.Capture(out _hCapturedFIR);
            if (ret == NBioAPI.Error.NONE)
            {
                //DisplayErrorMsg(ret);
                // m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                hCapturedFIR = _hCapturedFIR;
                m_NBioAPI.GetTextFIRFromHandle(_hCapturedFIR, out m_textFIR, true);
            }

            return m_textFIR.TextFIR;

        }

        public bool Compare(UserDigital user)
        {
            bool result;

            m_textFIR.TextFIR = user.DESDIGCONVERT;
            uint ret = m_NBioAPI.VerifyMatch(hCapturedFIR, m_textFIR, out result, myPayload);

            return result;
        }

        public void RegisterUsersDigital(IEnumerable<UserDigital> listUsers)
        {
            _logger.LogInformation("Carregando os usuários em memória");
            uint dataCount;
            uint qtRegs;

            NBioAPI.IndexSearch.FP_INFO[] fpInfo;
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();

            foreach (UserDigital user in listUsers)
            {
                m_textFIR.TextFIR = user.DESDIGCONVERT;
                uint ID = Convert.ToUInt32(user.NUMCAD);

                m_IndexSearch.AddFIR(m_textFIR, ID, out fpInfo);
            }


            qtRegs = m_IndexSearch.GetDataCount(out dataCount);
            _logger.LogInformation("Concluído o carregamento dos usuários em memória: " + dataCount + " registros");
        }

        public string Identify(string digital)
        {
            NBioAPI.IndexSearch.FP_INFO fpInfo2;
            NBioAPI.IndexSearch.CALLBACK_INFO_0 cbInfo0 = new NBioAPI.IndexSearch.CALLBACK_INFO_0();
            cbInfo0.CallBackFunction = new NBioAPI.IndexSearch.INDEXSEARCH_CALLBACK(myCallback);

            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            m_textFIR.TextFIR = digital;

            uint ret = m_IndexSearch.IdentifyData(hCapturedFIR, NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL, out fpInfo2, cbInfo0);
            if (ret == NBioAPI.Error.NONE)
            {
                //DisplayErrorMsg(ret);
                _logger.LogInformation(fpInfo2.ID.ToString());
                return fpInfo2.ID.ToString();
            }

            return "0";

        }

        public uint myCallback(ref NBioAPI.IndexSearch.CALLBACK_PARAM_0 cbParam0, IntPtr userParam)
        {
            //progressIdentify.Value = Convert.ToInt32(cbParam0.ProgressPos);
            return NBioAPI.IndexSearch.CALLBACK_RETURN.OK;
        }
    }
}