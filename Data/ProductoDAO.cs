using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PrototipoCRUD.Models;

namespace PrototipoCRUD.Data
{
    public class ProductoDAO
    {
        private readonly string cadenaConexion;

        public ProductoDAO()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            cadenaConexion = config.GetConnectionString("PostgresConnection") ?? "";
        }
        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                string sql = "SELECT id, codigo_barras, nombre, precio, stock FROM productos ORDER BY id ASC";
                using (var comando = new NpgsqlCommand(sql, conexion))
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            Id = reader.GetInt32(0),
                            CodigoBarras = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Precio = reader.GetDecimal(3),
                            Stock = reader.GetInt32(4)
                        });
                    }
                }
            }
            return lista;
        }

        public Producto? ObtenerPorId(int id)
        {
            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                string sql = "SELECT id, codigo_barras, nombre, precio, stock FROM productos WHERE id = @id";
                using (var comando = new NpgsqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("id", id);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Producto
                            {
                                Id = reader.GetInt32(0),
                                CodigoBarras = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Precio = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Insertar(Producto p)
        {
            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                string sql = "INSERT INTO productos (codigo_barras, nombre, precio, stock) VALUES (@codigo, @nombre, @precio, @stock)";
                using (var comando = new NpgsqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("codigo", p.CodigoBarras);
                    comando.Parameters.AddWithValue("nombre", p.Nombre);
                    comando.Parameters.AddWithValue("precio", p.Precio);
                    comando.Parameters.AddWithValue("stock", p.Stock);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Producto p)
        {
            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                string sql = "UPDATE productos SET codigo_barras = @codigo, nombre = @nombre, precio = @precio, stock = @stock WHERE id = @id";
                using (var comando = new NpgsqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("id", p.Id);
                    comando.Parameters.AddWithValue("codigo", p.CodigoBarras);
                    comando.Parameters.AddWithValue("nombre", p.Nombre);
                    comando.Parameters.AddWithValue("precio", p.Precio);
                    comando.Parameters.AddWithValue("stock", p.Stock);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                string sql = "DELETE FROM productos WHERE id = @id";
                using (var comando = new NpgsqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("id", id);
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
