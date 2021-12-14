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
namespace Usuario.Repository {
	public class Usuario {
		public static void registrarUsuario(string usuario, string contraseña) {
			bool flag = true;
			try {
				SqlConnection con = new SqlConnection();
				con.ConnectionString = Global.getConnectionString();

				SqlParameter username = new SqlParameter();
				username.ParameterName = "@username";
				username.Value = usuario;

				SqlParameter password = new SqlParameter();
				password.ParameterName = "@password";
				password.Value = contraseña;

				List < SqlParameter > listadoParametros = new List < SqlParameter > () {
					username,
					password
				};

				//DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_validar_usuario" , listadoParametros , con);
				ejecucionSP.ExecuteSPWithNoDataReturn("sp_registrar_usuario", listadoParametros, con, ref flag);
			}
			catch {
				Console.WriteLine("Hubo un problema...");
			}
		}
		public static bool validarUsuario(string nombreDeUsuario) {
			bool flag = true;
			Usuario usuario = new Usuario();
			try {
				SqlConnection con = new SqlConnection();
				con.ConnectionString = Global.getConnectionString();

				SqlParameter username = new SqlParameter();
				username.ParameterName = "@username";
				username.Value = nombreDeUsuario;

				List < SqlParameter > listadoParametros = new List < SqlParameter > () {
					username
				};

				DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_validar_usuario", listadoParametros, con);

				if (objetoSalida.Rows.Count > 0) {
					return false;
				}
			}
			catch {
				Console.WriteLine("Algo salio mal...");
			}
			return flag;
		}
		public static bool accederUsuario(string nombreDeUsuario, string contraseña) {
			bool flag = false;
			Usuario usuario = new Usuario();
			try {
				SqlConnection con = new SqlConnection();
				con.ConnectionString = Global.getConnectionString();

				SqlParameter username = new SqlParameter();
				username.ParameterName = "@username";
				username.Value = nombreDeUsuario;

				SqlParameter password = new SqlParameter();
				password.ParameterName = "@password";
				password.Value = contraseña;

				List < SqlParameter > listadoParametros = new List < SqlParameter > () {
					username,
					password
				};

				DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_login_usuario", listadoParametros, con);

				if (objetoSalida.Rows.Count > 0) {
					Console.WriteLine("Todo de maravilla");
					return true;
				}

			}
			catch {
				Console.WriteLine("Credenciales invalidas... ");
			}
			return flag;
		}
		public static UsuarioModel obtenerUsuario(string usuario) {
			UsuarioModel objeto = new UsuarioModel();
			try {
				SqlConnection con = new SqlConnection();
				con.ConnectionString = Global.getConnectionString();
				SqlParameter username = new SqlParameter();
				username.ParameterName = "@username";
				username.Value = usuario;
				List < SqlParameter > listadoParametros = new List < SqlParameter > {
					username
				};
				DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_obtener_usuario", listadoParametros, con);
				if (objetoSalida.Rows.Count > 0) {
					objeto.nombreUsuario = objetoSalida.Rows[0]["NOMBREUSUARIO"].ToString();
				}
			}
			catch {
				Console.WriteLine("Parece que hubo un problema obteniendo el usuario");
			}
			return objeto;
		}
		public static bool registrarLog(string nombreDeUsuario, string ipUsuario) {
			bool flag = true;
			try {
				SqlConnection con = new SqlConnection();
				con.ConnectionString = Global.getConnectionString();

				SqlParameter username = new SqlParameter();
				username.ParameterName = "@username";
				username.Value = Regex.Replace(nombreDeUsuario, @"\s", "");
                //username.Value = "edwin";

				SqlParameter ip = new SqlParameter();
				ip.ParameterName = "@ipUsuario";
				ip.Value = ipUsuario.ToString();
                //ip.Value = "127.0.0.1";

				SqlParameter fecha = new SqlParameter();
				fecha.ParameterName = "@fechaUsuario";
				fecha.Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                //fecha.Value = "2021-12-12 00:00:01";

                //Console.WriteLine("Reporte: "+nombreDeUsuario+ipUsuario+fecha.Value);

				List < SqlParameter > listadoParametros = new List < SqlParameter > () {
					username,
                    fecha,
					ip
				};

				DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("dbo.sp_registrar_log", listadoParametros, con);
				if (objetoSalida.Rows.Count > 0) {
					flag = false;
				}
			}
			catch {
				Console.WriteLine("El Log no se registro de forma exitosa");
			}
			return flag;
		}
        public static List<LogModel> obtenerLog()
        {
            List<LogModel> objetos = new List<LogModel>();
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
                DataTable objetoSalida = ejecucionSP.ExecuteSPWithDataReturn("sp_obtener_logs", listadoParametros, con);
                if (objetoSalida.Rows.Count > 0)
                {
                    for (int i = 0; i < objetoSalida.Rows.Count; i++)
                    {
                        LogModel u = new LogModel();
                        UsuarioModel usuario = new UsuarioModel();
                        u.idLog = Convert.ToInt32(objetoSalida.Rows[i]["IDLOG"]);
                        u.ipLog = objetoSalida.Rows[i]["IPLOG"].ToString();
                        u.fechaLog = Convert.ToDateTime(objetoSalida.Rows[i]["FECHALOG"]);
                        usuario.nombreUsuario = objetoSalida.Rows[i]["NOMBREUSUARIO"].ToString();
                        u.usuarioLog = usuario;
                        //u.usuarioPregunta = 1
                        objetos.Add(u);
                    }
                }
            }
            catch
            {

            }
            return objetos;
        }
		
	}
}