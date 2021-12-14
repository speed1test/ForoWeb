using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ForoWeb.Utils
{
    public class ejecucionSP
    {
        public static bool ejecutarSPNormal(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection)
        {
            bool flag = false;
            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            try
            {
                dataBaseConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                flag = true;
                throw;
            }
            finally
            {
                dataBaseConnection.Close();
            }

            return flag;
        }
        public static List<string> EjecutarProcedimientoConSalidasMultiples(string procedureName, List<SqlParameter> parameters, List<SqlParameter> parametersSalida, SqlConnection dataBaseConnection, ref bool huboError)
        {
            List<string> salidas = new List<string>();
            SqlCommand comando = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                comando.Parameters.AddRange(parameters.ToArray());
            }
            if (parametersSalida != null)
            {
                for (int i = 0; i < parametersSalida.Count; i++)
                {
                    parametersSalida[i].Direction = ParameterDirection.Output;
                }
                comando.Parameters.AddRange(parametersSalida.ToArray());
            }
            try
            {
                dataBaseConnection.Open();
                comando.ExecuteNonQuery();
                for (int i = 0; i < parametersSalida.Count; i++)
                {
                    salidas.Add(parametersSalida[i].Value.ToString());
                }
                huboError = false;
            }
            catch
            {
                huboError = true;
            }
            finally
            {
                dataBaseConnection.Close();
            }

            return salidas;
        }

        public static string EjecutarProcedimientoConSalidasSimple(string procedureName, List<SqlParameter> parameters, List<SqlParameter> parametersSalida, SqlConnection dataBaseConnection, ref bool huboError)
        {
            string salidas = string.Empty;
            SqlCommand comando = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                comando.Parameters.AddRange(parameters.ToArray());
            }
            if (parametersSalida != null)
            {
                for (int i = 0; i < parametersSalida.Count; i++)
                {
                    parametersSalida[i].Direction = ParameterDirection.Output;
                }
                comando.Parameters.AddRange(parametersSalida.ToArray());
            }
            try
            {
                dataBaseConnection.Open();
                comando.ExecuteNonQuery();
                salidas = parametersSalida[0].Value.ToString();
                huboError = false;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                huboError = true;
                throw;
            }
            finally
            {
                dataBaseConnection.Close();
            }

            return salidas;
        }
        public static void ExecuteSPWithNoDataReturn(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection, ref bool flag)
        {
            flag = true;
            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            try
            {
                dataBaseConnection.Open();
                command.ExecuteNonQuery();
            }
            catch { flag = false; }
            finally
            {
                dataBaseConnection.Close();
            }
        }


        public static void ExecuteSPWithNoDataReturnAndOutPutParameter(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection, string outputParamName, SqlDbType paramType, ref object valueReturn)
        {

            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlParameter output = new SqlParameter(outputParamName, paramType);
            output.Direction = ParameterDirection.Output;

            command.Parameters.Add(output);

            try
            {
                dataBaseConnection.Open();
                command.ExecuteNonQuery();
                valueReturn = output.Value;

            }
            catch { }
            finally
            {
                dataBaseConnection.Close();
            }
        }
        public static DataTable EjecutarSPConSalidas(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection, string outputParamName, SqlDbType paramType, ref object valueReturn)
        {
            DataTable tabla = new DataTable();
            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlParameter output = new SqlParameter(outputParamName, paramType, 50);
            output.Direction = ParameterDirection.Output;

            command.Parameters.Add(output);
            SqlDataAdapter adaptador = new SqlDataAdapter(command);
            try
            {
                dataBaseConnection.Open();
                command.ExecuteNonQuery();
                valueReturn = output.Value;
               // adaptador.Fill(tabla);
            }
            catch (Exception ex) { string error = ex.Message; }
            finally
            {
                dataBaseConnection.Close();
            }
            return tabla;
        }
        public static DataTable ExecuteSPWithDataReturn(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection)
        {
            DataTable dataTable = new DataTable();
            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {
                dataBaseConnection.Open();
                adapter.Fill(dataTable);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
            finally
            {
                dataBaseConnection.Close();
            }

            return dataTable;
        }


        public static DataSet ExecuteSPWithDataSetReturn(string procedureName, List<SqlParameter> parameters, SqlConnection dataBaseConnection)
        {
            DataSet dataSet = new DataSet();
            SqlCommand command = new SqlCommand(procedureName, dataBaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            try
            {
                dataBaseConnection.Open();
                adapter.Fill(dataSet);

            }
            catch { }
            finally
            {
                dataBaseConnection.Close();
            }

            return dataSet;
        }
    }
}