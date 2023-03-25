using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class ProductoDB
    {
        string cadena = "server=localhost; user=root; database=miticket; password=Euniceso.6;";

        public bool Insertar(Producto producto)
        {
            bool inserto = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" INSERT INTO producto VALUES ");
                sql.Append(" (@Codigo, @Precio, @Foto, @Tipo, @Marca, @Modelo);");

                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = producto.Codigo;
                        comando.Parameters.Add("@Tipo", MySqlDbType.VarChar, 45).Value = producto.Tipo;
                        comando.Parameters.Add("@Marca", MySqlDbType.VarChar, 45).Value = producto.Marca;
                        comando.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = producto.Foto;
                        comando.Parameters.Add("@Modelo", MySqlDbType.VarChar, 45).Value = producto.Modelo;
                        comando.ExecuteNonQuery();
                        inserto = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return inserto;
        }

        public bool Editar(Producto producto)
        {
            bool edito = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" UPDATE producto SET ");
                sql.Append(" Tipo = @Tipo, Marca = @Marca, Precio = @Precio, Foto = @Foto, Modelo = @Modelo");
                sql.Append(" WHERE Codigo = @Codigo;");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = producto.Codigo;
                        comando.Parameters.Add("@Tipo", MySqlDbType.VarChar, 45).Value = producto.Tipo;
                        comando.Parameters.Add("@Marca", MySqlDbType.VarChar, 45).Value = producto.Marca;
                        comando.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = producto.Precio;
                        comando.Parameters.Add("@Foto", MySqlDbType.LongBlob).Value = producto.Foto;
                        comando.Parameters.Add("@Modelo", MySqlDbType.VarChar, 45).Value = producto.Modelo;
                        comando.ExecuteNonQuery();
                        edito = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return edito;
        }

        public bool Eliminar(string codigo)
        {
            bool elimino = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" DELETE FROM producto ");
                sql.Append(" WHERE Codigo = @Codigo;");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = codigo;
                        comando.ExecuteNonQuery();
                        elimino = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return elimino;
        }

        public DataTable DevolverProductos()
        {
            DataTable dataTable = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dataTable.Load(dr);
                    }
                }
            }
            catch (Exception)
            {
            }
            return dataTable;
        }

        public byte[] DevolverImagen(string codigo)
        {
            byte[] imagen = new byte[0];
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT Foto FROM producto WHERE Codigo = @Codigo; ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = codigo;
                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            imagen = (byte[])dr["Foto"];
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return imagen;
        }

        public Producto DevolverProductoPorCodigo(string codigo)
        {
            Producto producto = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto WHERE Codigo = @Codigo; ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = codigo;
                        MySqlDataReader dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            producto = new Producto();

                            producto.Codigo = codigo;
                            producto.Tipo = dr["Tipo"].ToString();
                            producto.Modelo = dr["Modelo"].ToString();
                            producto.Precio = Convert.ToDecimal(dr["Precio"]);
                            producto.Marca = dr["EstaActivo"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return producto;
        }

        public DataTable DevolverPorDescripcion(string Modelo)
        {
            DataTable dataTable = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM producto WHERE Modelo LIKE '%" + Modelo + "%' ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dataTable.Load(dr);
                    }
                }
            }
            catch (Exception)
            {
            }
            return dataTable;
        }


    }
}
