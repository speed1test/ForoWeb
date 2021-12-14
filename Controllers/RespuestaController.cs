using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ForoWeb.Models;

namespace ForoWeb.Controllers;

public class RespuestaController : Controller
{
    private readonly ILogger<RespuestaController> _logger;

    public RespuestaController(ILogger<RespuestaController> logger)
    {
        _logger = logger;
    }
    
    [Route("/Foro/POST")]
    public IActionResult RegistrarRespuesta(int idPregunta, string contexto)
    {
        string username = "";
        //string contexto = "";
        DateTime fecha = DateTime.Now;
        if(HttpContext.Request.Method == "POST")
        {
            username=HttpContext.Session.GetString("usuario");
            //Console.WriteLine(username+idPregunta+contexto+fecha);
            Respuesta.Repository.Respuesta.registrarRespuesta(idPregunta,username,contexto,fecha);
        }
        var routeValue = new RouteValueDictionary(new { action = "VerPregunta", controller = "Pregunta", idPregunta=idPregunta});
        return RedirectToRoute(routeValue);
        //return RedirectToAction(actionName:"VerPregunta", controllerName:"Pregunta", RouteAttribute-idPregunta="");
    }


}
