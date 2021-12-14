using System.Collections.Concurrent;
using System;
using System.ComponentModel.DataAnnotations;
namespace ForoWeb.Models
{
    public class UsuarioModel
    {
        public string nombreUsuario{get; set;}
        public string contraseñaUsuario{get; set;}
    }
}