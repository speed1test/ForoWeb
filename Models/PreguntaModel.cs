using System.Collections.Concurrent;
using System;
using System.ComponentModel.DataAnnotations;
namespace ForoWeb.Models
{
    public class PreguntaModel
    {
        public int idPregunta {get; set;}
        public string tituloPregunta {get; set;}
        public bool estadoPregunta {get; set;}
        public UsuarioModel usuarioPregunta {get; set;}
        public DateTime actualizacionPregunta {get; set;}
        public string contextoPregunta {get; set;}
        
    }
}