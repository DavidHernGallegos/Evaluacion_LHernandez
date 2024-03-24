using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ArchivosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Vista()
        {
            return View();
        }

        public IActionResult Archivos()
        {
           

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5071/api/Archivos/LeerArchivos");
                var responseTask = client.GetAsync($"");
                responseTask.Wait();
                var respuesta = responseTask.Result;
                if (respuesta.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se han Leido los archivos, se movieron a la carpeta Procesados";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Mensaje = "No se han leido los archivos";
                    return PartialView("Modal");
                }
                
            }
        }

    }
}
