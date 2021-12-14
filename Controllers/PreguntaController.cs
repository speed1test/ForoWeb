using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ForoWeb.Models;
using System.Text.RegularExpressions;

namespace ForoWeb.Controllers;

public class PreguntaController: Controller {
	private readonly ILogger < PreguntaController > _logger;

	public PreguntaController(ILogger < PreguntaController > logger) {
		_logger = logger;
	} [Route("/Foro/{idPregunta}")]
	public IActionResult VerPregunta(int idPregunta) {
		if (UsuarioController.validarLogin(HttpContext.Session.GetString("usuario"))) {
			PreguntaModel pregunta = new PreguntaModel();
			List < RespuestaModel > respuestas = new List < RespuestaModel > ();
			pregunta = Pregunta.Repository.Pregunta.obtenerPregunta(idPregunta);
			ViewBag.contexto = pregunta.contextoPregunta;
			ViewBag.fecha = pregunta.actualizacionPregunta.ToString("MM/dd/yyyy");
			ViewBag.titulo = pregunta.tituloPregunta;
			ViewBag.usuario = pregunta.usuarioPregunta.nombreUsuario;
			ViewBag.login = HttpContext.Session.GetString("usuario");
			ViewBag.idPregunta = pregunta.idPregunta;
			ViewBag.boolRespuesta = pregunta.estadoPregunta;
			respuestas = Respuesta.Repository.Respuesta.obtenerRespuestas(idPregunta);
			ViewBag.respuestas = respuestas;
			//Console.WriteLine("Usuario 1: "+HttpContext.Session.GetString("usuario")+"y"+"Usuario 2:"+pregunta.usuarioPregunta.nombreUsuario+"F");
			if (HttpContext.Session.GetString("usuario") == pregunta.usuarioPregunta.nombreUsuario) {
				ViewBag.estado = 1;
			}
			else {
				ViewBag.estado = 0;
			}
			return View("/Views/Foro/Index.cshtml");
		}
		else {
			return RedirectPermanent("/");
		}
	} [Route("/Foro/Close/{idPregunta}")]
	public IActionResult cerrarPregunta(int idPregunta) {
		if (UsuarioController.validarLogin(HttpContext.Session.GetString("usuario"))) {
			try {
				PreguntaModel pregunta = new PreguntaModel();
				pregunta = Pregunta.Repository.Pregunta.obtenerPregunta(idPregunta);
				if (pregunta.usuarioPregunta.nombreUsuario == HttpContext.Session.GetString("usuario")) {
					Pregunta.Repository.Pregunta.inactivarPregunta(idPregunta);
				}
			}
			catch {

			}
			var routeValue = new RouteValueDictionary(new {
				action = "VerPregunta",
				controller = "Pregunta",
				idPregunta = idPregunta
			});
			return RedirectToRoute(routeValue);
		}
		else {
			return RedirectPermanent("/");
		}
	} [Route("/Foro/Crear")]
	public IActionResult RegistrarPregunta(string titulo, string contexto) {
		if (UsuarioController.validarLogin(HttpContext.Session.GetString("usuario"))) {
			string username = "";
			ViewBag.estadoGuardado = 2;
			try {
				if (HttpContext.Request.Method == "POST") {
					username = HttpContext.Session.GetString("usuario");
					if (username != "" && titulo != "" && contexto != "") {
						Pregunta.Repository.Pregunta.guardarPregunta(titulo, contexto, username);
						ViewBag.estadoGuardado = 1;
					}
				}
			}
			catch {
				ViewBag.estadoGuardado = 0;
			}
			return View("/Views/Foro/Crear.cshtml");
		}
		else {
			return RedirectPermanent("/");
		}
	}
}