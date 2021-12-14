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
    public class LogModel{
        public int idLog {get; set;}
        public UsuarioModel usuarioLog {get; set;}
        public DateTime fechaLog {get; set;}
        public string ipLog {get; set;}

    }
}