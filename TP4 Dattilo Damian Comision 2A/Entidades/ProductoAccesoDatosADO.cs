using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Entidades
{
    public static class ProductoAccesoDatosADO
    {
        static string ConnectionString;
        static SqlCommand command;
        static SqlConnection connection;
        static ProductoAccesoDatosADO()
        {

            ConnectionString = @"Data Source = localhost\SQLEXPRESS;Initial Catalog=TP_4;Integrated Security=True";
            command = new SqlCommand();
            connection = new SqlConnection(ConnectionString);
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.Text;



        }

        public static Taller<Producto> Leer()
        {
            Taller<Producto> productos = new Taller<Producto>(7);

            try
            {
                command.Parameters.Clear();
                connection.Open();
                command.CommandText = "SELECT * FROM productos";
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if(dataReader["Categoria"].ToString() == "Microondas")
                    {
                        productos.Agregar(new Microondas((Int32.Parse(dataReader["ID"].ToString())), ((EMarca)Enum.Parse(typeof(EMarca), dataReader["Marca"].ToString())), ((EficienciaEnergetica)Enum.Parse(typeof(EficienciaEnergetica), dataReader["Eficiencia"].ToString())), Int32.Parse(dataReader["Porcentaje"].ToString()), ((ETamanio)Enum.Parse(typeof(ETamanio), dataReader["Caracteristica"].ToString())), Int32.Parse(dataReader["Precio"].ToString())));
                    }
                    else
                    {
                        productos.Agregar(new AireAcondicionado((Int32.Parse(dataReader["ID"].ToString())), ((EMarca)Enum.Parse(typeof(EMarca), dataReader["Marca"].ToString())), ((EficienciaEnergetica)Enum.Parse(typeof(EficienciaEnergetica), dataReader["Eficiencia"].ToString())), Int32.Parse(dataReader["Porcentaje"].ToString()), ((ETipo)Enum.Parse(typeof(ETipo), dataReader["Caracteristica"].ToString())), Int32.Parse(dataReader["Precio"].ToString())));
                    }
                }
                return productos;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void Guardar(Producto producto)
        {
            
            try
            {
                command.Parameters.Clear();
                connection.Open();

                command.CommandText = $"INSERT INTO Productos (ID, Categoria, Marca, Eficiencia, Porcentaje, Caracteristica, Precio) VALUES (@ID, @Categoria, @Marca, @Eficiencia, @Porcentaje, @Caracteristica, @Precio)";

                command.Parameters.AddWithValue("@Id", producto.Id);
                if(producto is Microondas)
                    command.Parameters.AddWithValue("@Categoria", "Microondas");
                else
                    command.Parameters.AddWithValue("@Categoria", "Aire Acondicionado");
                command.Parameters.AddWithValue("@Marca", producto.Marca);
                command.Parameters.AddWithValue("@Eficiencia", producto.Eficiencia);
                command.Parameters.AddWithValue("@Porcentaje", producto.PorcentajeDeFallas);
                if (producto is Microondas)
                    command.Parameters.AddWithValue("@Categoria", "Grande");
                else
                    command.Parameters.AddWithValue("@Categoria", "Frio");
                command.Parameters.AddWithValue("@Precio", producto.Precio);


                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

            finally
            {
                connection.Close();
            }
        }

        public static void Eliminar(int id)
        {
            try
            {
                command.Parameters.Clear();
                connection.Open();

                command.CommandText = $"DELETE FROM Productos WHERE ID = (@ID)";
                command.Parameters.AddWithValue("@ID", id);

                command.ExecuteNonQuery();
            }
            catch
            {
                throw;

            }

            finally
            {
                connection.Close();
            }
        }

        public static void Modificar(Producto nuevoProducto)
        {
            try
            {
                command.Parameters.Clear();
                connection.Open();

                command.CommandText = $"UPDATE Productos SET ID = @ID, Marca = @Marca, Eficiencia = @eficiencia, Porcentaje = @Porcentaje, Precio = @Precio WHERE ID = @ID";
                command.Parameters.AddWithValue("@Id", nuevoProducto.Id);
                command.Parameters.AddWithValue("@Marca", nuevoProducto.Marca);
                command.Parameters.AddWithValue("@Eficiencia", nuevoProducto.Eficiencia);
                command.Parameters.AddWithValue("@Porcentaje", nuevoProducto.PorcentajeDeFallas);
                command.Parameters.AddWithValue("@Precio", nuevoProducto.Precio);

                command.ExecuteNonQuery();
            }
            catch
            {
                throw;

            }

            finally
            {
                connection.Close();
            }
        }

        public static string AnalisisDeDatos()
        {
            int cont;


            try
            {
                StringBuilder sb = new StringBuilder();
                command.Parameters.Clear();
                connection.Open();

                command.CommandText = "SELECT * FROM productos WHERE Marca = (@Marca)";
                command.Parameters.AddWithValue("@Marca", "Philco");

                SqlDataReader dataReader = command.ExecuteReader();

                for(cont=0; (dataReader.Read()); cont++) { }
                sb.AppendLine($"Hay {cont} productos de la marca Philco");

                command.CommandText = "SELECT * FROM productos WHERE Porcentaje > 50";

                for (cont = 0; (dataReader.Read()); cont++) { }
                sb.AppendLine($"Hay {cont} productos cuyo porcentaje de fallas es mayor al 50%");

                return sb.ToString();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            /*sb.AppendLine($"Hay {this.ContadorDeMarca(marca)} productos de la marca {marca}");
            sb.AppendLine($"Los productos de la marca {marca} tienen un promedio del {this.PromedioDeFallas(marca)}% de falla, y su precio promedio es: {this.PrecioPromedio(marca)}");
            sb.AppendLine($"El promedio de fallas de todos los productos es de: {this.PromedioDeFallas(EMarca.Todas)}%");
            sb.AppendLine($"El precio promedio de todos los productos es de: {this.PrecioPromedio(EMarca.Todas)}%");
            sb.AppendLine($"Hay {this.ContadorEficiencia(EficienciaEnergetica.A)} productos con eficiencia energetica A");*/




        }
    }
}
