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

namespace Pregunta.Repository
{
    public class Pregunta
    {
       public static List<PreguntaModel> obtenerPreguntas()
       {
           List<PreguntaModel> preguntas = new List<PreguntaModel>();
           try
           {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();
                List<SqlParameter> listadoParametros = new List<SqlParameter>
                {
                };
                List<SqlParameter> listadoParametrosSalida = new List<SqlParameter>
                {
                };
                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("sp_obtener_preguntas", listadoParametros, con);
                if (objetoSalida.Rows.Count > 0)
                {
                    for (int i = 0; i < objetoSalida.Rows.Count; i++)
                    {
                        PreguntaModel u = new PreguntaModel();
                        UsuarioModel usuario = new UsuarioModel();
                        u.idPregunta = Convert.ToInt32(objetoSalida.Rows[i]["IDPREGUNTA"]);
                        u.tituloPregunta = objetoSalida.Rows[i]["TITULOPREGUNTA"].ToString();
                        u.actualizacionPregunta = Convert.ToDateTime(objetoSalida.Rows[i]["ACTUALIZACIONPREGUNTA"]);
                        //u.usuarioPregunta = 1
                        preguntas.Add(u);
                    }
                }
           }
           catch
           {
               Console.WriteLine("Algo salio mal obteniendo las preguntas de la BD...");
           }
           return preguntas;
       }
       public static PreguntaModel obtenerPregunta(int idPregunta)
       {
           PreguntaModel preg = new PreguntaModel();
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
                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_obtener_pregunta", listadoParametros, con);
                if (objetoSalida.Rows.Count > 0)
                {
                    UsuarioModel usuario = new UsuarioModel();
                    string username = "";
                    preg.idPregunta = Convert.ToInt32(objetoSalida.Rows[0]["IDPREGUNTA"]);
                    preg.tituloPregunta = objetoSalida.Rows[0]["TITULOPREGUNTA"].ToString();
                    preg.contextoPregunta = objetoSalida.Rows[0]["CONTEXTOPREGUNTA"].ToString();
                    preg.estadoPregunta = Convert.ToBoolean(objetoSalida.Rows[0]["ESTADOPREGUNTA"]);
                    preg.actualizacionPregunta = Convert.ToDateTime(objetoSalida.Rows[0]["ACTUALIZACIONPREGUNTA"]);
                    username = Regex.Replace(objetoSalida.Rows[0]["NOMBREUSUARIO"].ToString(), @"\s", "");
                    usuario.nombreUsuario = username;
                    preg.usuarioPregunta = usuario;
                }
           }
           catch
           {
               Console.WriteLine("Algo salio mal mientras obteniendo los datos de la pregunta...");
           }
           return preg;
       }
       public static bool inactivarPregunta(int idPregunta)
       {
           bool flag = true;
           try{
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter idPre = new SqlParameter();
                idPre.ParameterName = "@idPregunta"; 
                idPre.Value = idPregunta; 

                List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    idPre
                };
                ejecucionSP.ExecuteSPWithNoDataReturn("sp_cambiar_estado", listadoParametros, con, ref flag);
           }
           catch{
               Console.WriteLine("Algo salio mal, cuando se intentaba cambiar el estado de la pregunta...");
           }
           return flag;
       }
       public static bool guardarPregunta(string tituloPregunta, string contextoPregunta, string nombreUsuario)
       {
           bool flag = false; 
           string username = "";
           try{
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter tituloPreg = new SqlParameter();
                tituloPreg.ParameterName = "@tituloPregunta"; 
                tituloPreg.Value = tituloPregunta.ToString(); 

                SqlParameter contextoPreg = new SqlParameter();
                contextoPreg.ParameterName = "@contextoPregunta"; 
                contextoPreg.Value = contextoPregunta.ToString();

                SqlParameter userPreg = new SqlParameter();
                userPreg.ParameterName = "@username"; 
                username = Regex.Replace(nombreUsuario, @"\s", "");
                userPreg.Value = nombreUsuario.ToString();

                SqlParameter fechaPreg = new SqlParameter();
                fechaPreg.ParameterName = "@fechaPregunta"; 
                fechaPreg.Value = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    tituloPreg,
                    contextoPreg,
                    userPreg,
                    fechaPreg
                };

                ejecucionSP.ExecuteSPWithNoDataReturn("sp_registrar_pregunta", listadoParametros, con, ref flag);
            }
            catch
            {
                Console.WriteLine("Algo salio mal registrando la pregunta... ");
            }
            return flag;
       }
       
    }
}