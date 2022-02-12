using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Infrastructure.Helpers;
using Calculator.Service;
using Model.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System;

namespace Calculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalcApiController : ControllerBase
    {
        private readonly Service.CalculatorService _calcService;

        public CalcApiController(CalculatorService calcService)
        {
            _calcService = calcService;
        }

        [HttpPost("RunCalc")]
        public async Task<CalculateTreeData> RunCalc(Dictionary<string, object> data)
        {
            Dictionary<string, CalculateTreeData> flatRefModel = null;
            bool outSource = false;

            int i = 0;
            foreach (var d in data)
            {
                byte[] byteArray = Convert.FromBase64String(d.Value.ToString());
                string jsonBack = Encoding.UTF8.GetString(byteArray);
                string decompress = Util.Unzip(jsonBack);

                if (i == 0)
                    outSource = JsonConvert.DeserializeObject<bool>(decompress);

                if (i == 1)
                    flatRefModel = JsonConvert.DeserializeObject<Dictionary<string, CalculateTreeData>>(decompress);
                i++;
            }

            var tree = _calcService.RunCalc(flatRefModel, outSource, false);
            return await Task.FromResult(tree);
        }

        [HttpPost("RunCalcRef")]
        public async Task<CalculateTreeData> RunCalcRef(Dictionary<string, object> data)
        {
            Dictionary<string, CalculateTreeData> flatRefModel = null;

            int i = 0;
            foreach (var d in data)
            {
                byte[] byteArray = Convert.FromBase64String(d.Value.ToString());
                string jsonBack = Encoding.UTF8.GetString(byteArray);
                string decompress = Util.Unzip(jsonBack);

                if (i == 0)
                    flatRefModel = JsonConvert.DeserializeObject<Dictionary<string, CalculateTreeData>>(decompress);
                i++;
            }

            var tree = _calcService.RunCalc(flatRefModel, false, true);
            return await Task.FromResult(tree);
        }
    }
}
