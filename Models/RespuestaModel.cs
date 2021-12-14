using System.Resources;
using System.Collections.Concurrent;
using System;
using System.ComponentModel.DataAnnotations;
namespace ForoWeb.Models
{
    public class RespuestaModel
    {
        public int idRespuesta {get; set;}
        public string contextoRespuesta {get; set;}
        public UsuarioModel usuarioRespuesta {get; set;}
        public DateTime actualizacionRespuesta {get; set;}
    }
}