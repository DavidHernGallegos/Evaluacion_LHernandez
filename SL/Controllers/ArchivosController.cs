using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        [HttpGet]
        [Route("LeerArchivos")]
        public IActionResult LeerArchivos() {

            try
            {
                BL.Archivos.LeerArchivos();
                return Ok("Archivos Leidos");

            }
            catch(Exception ex) { 
                    
                return BadRequest(ex.Message);
            }

            
        }
       


    }
}
