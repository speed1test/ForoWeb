using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ForoWeb.Models;
using ForoWeb.Utils;
using ForoWeb.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.RegularExpressions;
namespace Respuesta.Repository
{
    public class Respuesta
    {
       public static bool registrarRespuesta(int idPregunta, string username, string contexto, DateTime fecha)
       {
           bool flag = false;
           //var flag = new Object();
           /*try{*/
           RespuestaModel respuesta = new RespuestaModel();
           SqlConnection con2 = new SqlConnection();
           con2.ConnectionString = Global.getConnectionString2();

           SqlParameter idPreg = new SqlParameter();
           idPreg.ParameterName = "@idPregunta"; 
           idPreg.Value = idPregunta.ToString();

           SqlParameter user = new SqlParameter();
           user.ParameterName = "@username"; 
           user.Value = username.ToString();

           SqlParameter context = new SqlParameter();
           context.ParameterName = "@contextoRespuesta"; 
           context.Value = contexto.ToString();

           SqlParameter date = new SqlParameter();
           date.ParameterName = "@fechaRespuesta"; 
           //date.Value = "2021-10-11 00:00:00";
           date.Value = fecha.ToString("MM/dd/yyyy HH:mm:ss");

            Console.WriteLine(idPregunta+username+contexto+fecha);

           List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    idPreg,
                    user,
                    context,
                    date
            };
            Console.WriteLine("Me ejecute");
            //Console.WriteLine(ejecucionSP.EjecutarSPConSalidas("dbo.sp_registrar_ans" , listadoParametros , con2, "@idPregunta" ,SqlDbType.Int ,ref flag));
            //var flag2 = new Object();
            ejecucionSP.ExecuteSPWithNoDataReturn("sp_registrar_respuesta", listadoParametros, con2, ref flag);
           /*}
           catch
           {

           }*/
           return flag;
       }
       public static List<RespuestaModel> obtenerRespuestas(int idPregunta)
       {
           RespuestaModel resp = new RespuestaModel();
           List<RespuestaModel> objetos = new List<RespuestaModel>();
           try
           {
               SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter idPreg = new SqlParameter();
                idPreg.ParameterName = "@idPregunta";
                idPreg.Value = idPregunta;
                List<SqlParameter> listadoParametros = new List<SqlParameter>{
                    idPreg
                };
                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_obtenter_respuestas", listadoParametros, con);
                if (objetoSalida.Rows.Count > 0)
                {
                    
                    for (int i = 0; i < objetoSalida.Rows.Count ; i++)
                    {
                        UsuarioModel usuario = new UsuarioModel();
                        string username = "";
                        resp.idRespuesta = Convert.ToInt32(objetoSalida.Rows[i]["IDRESPUESTA"]);
                        resp.contextoRespuesta = objetoSalida.Rows[i]["CONTEXTORESPUESTA"].ToString();
                        resp.actualizacionRespuesta = Convert.ToDateTime(objetoSalida.Rows[i]["ACTUALIZACIONRESPUESTA"]);
                        username = Regex.Replace(objetoSalida.Rows[i]["NOMBREUSUARIO"].ToString(), @"\s", "");
                        usuario.nombreUsuario = username;
                        resp.usuarioRespuesta = usuario;
                        //Console.WriteLine(resp.contextoRespuesta);
                        objetos.Add(resp);
                    }

                }
           }
           catch
           {
               Console.WriteLine("Algo salio mal mientras obteniendo los datos de la pregunta...");
           }
           return objetos;
       }
    }
}