using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ForoWeb.Models;
using Microsoft.AspNetCore.Http;
using ForoWeb.Controllers;
using System.Text.RegularExpressions;

namespace ForoWeb.Controllers;

public class UsuarioController: Controller {
	private readonly ILogger < UsuarioController > _logger;

	public UsuarioController(ILogger < UsuarioController > logger) {
		_logger = logger;
	} [Route("/Registrar")]
	public static bool validarUsername(string username) {
		bool flag = false;
		var tieneNumeros = new Regex(@"[0-9]+");
		var mayusculas = new Regex(@"[A-Z]+");
		var limite = new Regex(@".{1,10}");
		var minusculas = new Regex(@"[a-z]+");
		var simbolos = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
		if ((tieneNumeros.IsMatch(username) && minusculas.IsMatch(username)) || (!mayusculas.IsMatch(username) && limite.IsMatch(username) && minusculas.IsMatch(username) && !simbolos.IsMatch(username))) {
			flag = true;
		}
		return flag;
	}
	public static bool validarPassword(string password) {
		bool flag = false;
		var tieneNumeros = new Regex(@"[0-9]+");
		var mayusculas = new Regex(@"[A-Z]+");
		var limite = new Regex(@".{8,50}");
		var minusculas = new Regex(@"[a-z]+");
		var simbolos = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
		if (tieneNumeros.IsMatch(password) && mayusculas.IsMatch(password) && limite.IsMatch(password) && minusculas.IsMatch(password) && simbolos.IsMatch(password)) {
			flag = true;
		}
		return flag;
	}
	public IActionResult Registrar(String usuario, String contraseña) {
		if (!validarLogin(HttpContext.Session.GetString("usuario"))) {
			try {
				if (HttpContext.Request.Method == "POST") {
					if (validarUsername(usuario) && validarPassword(contraseña)) {
						if (Usuario.Repository.Usuario.validarUsuario(usuario)) {
							Usuario.Repository.Usuario.registrarUsuario(usuario, contraseña);
							ViewBag.estadoRegistro = 1;
						}
						else {
							ViewBag.estadoRegistro = 0;
						}
					}
                    else{
                        ViewBag.estadoRegistro = 2;
                    }
				}
			}
			catch {
				Error("Registrar");
			}
			return View("/Views/Login/Signup.cshtml");
		}
		else {
			return RedirectPermanent("/");
		}
	} [Route("/Login")]
	public IActionResult Acceder(string usuario, string contraseña) {
		if (!validarLogin(HttpContext.Session.GetString("usuario"))) {
			try {
				if (HttpContext.Request.Method == "POST") {
					if (!Usuario.Repository.Usuario.validarUsuario(usuario)) {
						if (Usuario.Repository.Usuario.accederUsuario(usuario, contraseña)) {
							ViewBag.login = 1;
							HttpContext.Session.SetString("usuario", usuario);
							Usuario.Repository.Usuario.registrarLog(usuario, obtenerIp(HttpContext));
							return RedirectToAction(actionName: "Index", controllerName: "Home");
							//ViewBag.usuario = HttpContext.Session.GetString("usuario");
						}
						else {
							ViewBag.estadoLogin = 0;
						}
					}
					else {
						ViewBag.estadoLogin = 1;
					}
				}
			}
			catch {}
			return View("/Views/Login/Index.cshtml");
		}
		else {
			return RedirectPermanent("/");
		}
	} [Route("/Logout")]
	public IActionResult Salir() {
		if (validarLogin(HttpContext.Session.GetString("usuario"))) {
			try {
				HttpContext.Session.Clear();
			}
			catch(Exception ex) {
				Console.WriteLine("Problemas en el Logout...");
			}
			return RedirectToAction(actionName: "Index", controllerName: "Home");
		}
		else {
			return RedirectPermanent("/");
		}
	}
	public void Error(String locateError) {
		string respuesta = "Parece que algo anda mal... en: " + locateError;
		Console.WriteLine(respuesta);
	}
	public static bool validarLogin(string username) {
		bool flag = false;
		try {
			if (username != null) {
				//Usuario.Repository.Usuario.registrarLog(username,obtenerIp(context));
				//Console.WriteLine("Prueba:"+username+")");
				//Console.WriteLine();
				flag = true;
			}
		}
		catch {
			Console.WriteLine("Problemas en la validación de usuario.");
		}
		return flag;
	}

	public static string obtenerIp(HttpContext context) {
		string ip = string.Empty;
		if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"])) {
			ip = context.Request.Headers["X-Forwarded-For"];
		}
		else {
			ip = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
		}
		return ip;
	} [Route("/Logs")]
	public IActionResult VerLogs() {
		if (validarLogin(HttpContext.Session.GetString("usuario"))) {
			ViewBag.logs = Usuario.Repository.Usuario.obtenerLog();
			return View("/Views/Log/Index.cshtml");
		}
		else {
			return RedirectPermanent("/");
		}
	}
}