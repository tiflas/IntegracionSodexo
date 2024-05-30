using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntegracionSodexo.Model
{
    internal class Connection
    {
        public static SqlConnection Obtenerconexion()
        {
            //string connection = @"Data Source=LAPTOP-S82MCILV\SQLEXPRESS;Initial Catalog=PruebaIconstruye;Integrated Security=True;TrustServerCertificate=True;";
            //SqlConnection conn = new SqlConnection(@"Data Source=172.30.5.14\BDCCI01;Initial Catalog=dbMarketplaceColombia;User ID=Usr_Consultas;Password=Acceso2017");
            SqlConnection conn = new SqlConnection(@"Data Source=172.30.5.14\BDCCI01;Initial Catalog=dbMarketplaceColombia;User ID=Usr_Consultas;Password=Acceso2017");
            try
            {
                conn.Open();
            }
            catch
            {
                Console.WriteLine("Se ha presentado problemas de conexión");
            }

            return conn;
        }

        //string connection = @"Data Source=DESKTOP-1K5BR1A\SQLEXPRESS;Initial Catalog=PruebaIconstruye;Integrated Security=True;TrustServerCertificate=True;";
    }
}
