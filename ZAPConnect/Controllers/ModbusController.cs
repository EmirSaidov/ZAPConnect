using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ZAPConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModbusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(JObject payload)
        {
            var parsedAddress = JObject.Parse(payload.ToString())["address"];
            var parsedSlaveID = JObject.Parse(payload.ToString())["slaveid"];
            var parsedPort = JObject.Parse(payload.ToString())["port"];
            var parsedPoints = JObject.Parse(payload.ToString())["points"];

            if (string.IsNullOrWhiteSpace(parsedAddress.ToString()) || string.IsNullOrWhiteSpace(parsedSlaveID.ToString()) ||
                string.IsNullOrWhiteSpace(parsedPort.ToString()) || string.IsNullOrWhiteSpace(parsedPoints.ToString()))
            {
                return BadRequest();
            }
            return Ok(ModbusHandler.ModbusSerialRtuMasterReadRegisters(parsedPort.ToString(),
                    (byte)parsedSlaveID, (ushort)parsedAddress, (ushort)parsedPoints));
        }

        [HttpGet]
        public IActionResult GetTCP(JObject payload)
        {
           
            return Ok(ModbusHandler.ModbusTcpMasterReadInputs());
        }


        [HttpPost]
        public IActionResult Post(JObject payload)
        {
            var parsedData = JObject.Parse(payload.ToString())["data"];
            var parsedAddress = JObject.Parse(payload.ToString())["address"];
            var parsedSlaveID = JObject.Parse(payload.ToString())["slaveid"];
            var parsedPort = JObject.Parse(payload.ToString())["port"];

            if (string.IsNullOrWhiteSpace(parsedAddress.ToString()) || string.IsNullOrWhiteSpace(parsedSlaveID.ToString()) ||
                string.IsNullOrWhiteSpace(parsedPort.ToString()) || string.IsNullOrWhiteSpace(parsedData.ToString()))
            {
                return BadRequest();
            }
            ModbusHandler.ModbusSerialRtuMasterWriteRegisters(parsedPort.ToString(),(byte)parsedSlaveID, (ushort)parsedAddress, (ushort)parsedData);
            return Ok();
        }
    }
}
