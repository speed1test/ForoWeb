using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ForoWeb.Models;
using System.Text.RegularExpressions;

namespace ForoWeb.Controllers;

public class PreguntaController : Controller
{
    private readonly ILogger<PreguntaController> _logger;

    public PreguntaController(ILogger<PreguntaController> logger)
    {
        _logger = logger;
    }
    [Route("/Foro/{idPregunta}")]
    public IActionResult VerPregunta(int idPregunta)
    {
        PreguntaModel pregunta = new PreguntaModel();
        pregunta = Pregunta.Repository.Pregunta.obtenerPregunta(idPregunta);
        ViewBag.contexto = pregunta.contextoPregunta;
        ViewBag.fecha = pregunta.actualizacionPregunta.ToString("MM/dd/yyyy");
        ViewBag.titulo = pregunta.tituloPregunta;
        ViewBag.usuario = pregunta.usuarioPregunta.nombreUsuario;
        ViewBag.idPregunta = pregunta.idPregunta;
        ViewBag.respuestas = Respuesta.Repository.Respuesta.obtenerRespuestas(idPregunta);
        //Console.WriteLine("Usuario 1: "+HttpContext.Session.GetString("usuario")+"y"+"Usuario 2:"+pregunta.usuarioPregunta.nombreUsuario+"F");
        if(HttpContext.Session.GetString("usuario")==pregunta.usuarioPregunta.nombreUsuario){
            ViewBag.estado = 1;
        }
        else
        {
            ViewBag.estado = 0;
        }
        return View("/Views/Foro/Index.cshtml");
    }
}
