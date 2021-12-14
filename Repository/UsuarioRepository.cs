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
namespace Usuario.Repository
{
    public class Usuario
    {
        public static void registrarUsuario(string usuario, string contraseña)
        {
            bool flag = true;
            try{
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter username = new SqlParameter();
                username.ParameterName = "@username"; 
                username.Value = usuario; 

                SqlParameter password = new SqlParameter();
                password.ParameterName = "@password"; 
                password.Value = contraseña; 

                List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    username,
                    password
                };

                //DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_validar_usuario" , listadoParametros , con);
                ejecucionSP.ExecuteSPWithNoDataReturn("sp_registrar_usuario", listadoParametros, con, ref flag);
            }
            catch
            {
                Console.WriteLine("Hubo un problema...");
            }
        }
        public static bool validarUsuario(string nombreDeUsuario)
        {
            bool flag = true; 
            Usuario usuario = new Usuario();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter username = new SqlParameter();
                username.ParameterName = "@username"; 
                username.Value = nombreDeUsuario; 

                List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    username
                };

                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_validar_usuario" , listadoParametros , con);

                if (objetoSalida.Rows.Count > 0)
                {
                    return false; 
                }
            }
            catch
            {
                Console.WriteLine("Algo salio mal...");
            }
            return flag;
        }
        public static bool accederUsuario(string nombreDeUsuario, string contraseña)
        {
            bool flag = false;
            Usuario usuario = new Usuario(); 
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();

                SqlParameter username = new SqlParameter();
                username.ParameterName = "@username"; 
                username.Value = nombreDeUsuario; 

                SqlParameter password = new SqlParameter();
                password.ParameterName = "@password"; 
                password.Value = contraseña; 

                List<SqlParameter> listadoParametros = new List<SqlParameter>(){
                    username,
                    password
                };

                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_login_usuario" , listadoParametros , con);

                if (objetoSalida.Rows.Count > 0)
                {
                    Console.WriteLine("Todo de maravilla");
                    return true; 
                }

            }
            catch
            {
                Console.WriteLine("Credenciales invalidas... ");
            }
            return flag;
        }
        public static UsuarioModel obtenerUsuario(string usuario)
        {
            UsuarioModel objeto = new UsuarioModel();
            try{
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Global.getConnectionString();
                SqlParameter username = new SqlParameter();
                username.ParameterName = "@username";
                username.Value = usuario;
                List<SqlParameter> listadoParametros = new List<SqlParameter>
                {
                    username
                };
                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_obtener_usuario", listadoParametros, con);
                if (objetoSalida.Rows.Count > 0)
                {
                    objeto.nombreUsuario = objetoSalida.Rows[0]["NOMBREUSUARIO"].ToString();
                }
            }
            catch
            {
                Console.WriteLine("Parece que hubo un problema obteniendo el usuario");
            }
            return objeto;
        }
    }
}