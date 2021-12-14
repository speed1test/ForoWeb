using System.Threading;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ForoWeb.Models;
using Microsoft.AspNetCore.Http;
using ForoWeb.Controllers;

namespace ForoWeb.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
    }
    [Route("/Registrar")]
    public IActionResult Registrar(String usuario, String contraseña)
    {
        //Console.WriteLine(Usuario.Repository.Usuario.validarUsuario("edwin1"));
        //Usuario.Repository.Usuario.registrarUsuario("n1g1","contraseña123");
        try
        {
            if(HttpContext.Request.Method == "POST")
            {
                if(Usuario.Repository.Usuario.validarUsuario(usuario))
                {
                    Usuario.Repository.Usuario.registrarUsuario(usuario, contraseña);
                    ViewBag.estadoRegistro = 1;
                }
                else
                {
                    ViewBag.estadoRegistro = 0;
                }
            }
        }
        catch
        {
            Error("Registrar");
        }
        return View("/Views/Login/Signup.cshtml");
    }
    [Route("/Login")]
    public IActionResult Acceder(string usuario, string contraseña)
    {
        try
        {
            if(HttpContext.Request.Method == "POST")
            {
                if(!Usuario.Repository.Usuario.validarUsuario(usuario))
                {
                    if(Usuario.Repository.Usuario.accederUsuario(usuario,contraseña))
                    {
                        ViewBag.login = 1;
                        HttpContext.Session.SetString("usuario", usuario);
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                        //ViewBag.usuario = HttpContext.Session.GetString("usuario");
                    }
                    else
                    {
                        ViewBag.estadoLogin = 0;
                    }
                }
                else
                {
                    ViewBag.estadoLogin = 1;
                }
            }
        }
        catch
        {

        }
        return View("/Views/Login/Index.cshtml");
    }
    [Route("/Logout")]
    public IActionResult Salir()
    {
        try
        {
            HttpContext.Session.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Problemas en el Logout...");
        }
        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }
    public void Error(String locateError)
    {
        string respuesta = "Parece que algo anda mal... en: "+locateError;
        Console.WriteLine(respuesta);
    }
}
