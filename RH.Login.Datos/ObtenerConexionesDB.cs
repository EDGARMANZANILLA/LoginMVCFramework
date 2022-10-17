using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Datos
{
    public class ObtenerConexionesDB
    {

        /************************************************************************************************/
        /**********************     al subir el proyecto hay que ********************************************************/
        /************************************************************************************************/
        public static string obtenerCadenaConexionProductiva()
        {
            //return @"Data Source=172.19.2.31; Initial Catalog=Nomina; User=sa; PassWord=s3funhwonre2";
            //cambio motor de base de datos local por problemas de red
            return @"Data Source=172.19.62.71; Initial Catalog=Nomina; User=sa; PassWord=dbadmin";
        }

        public static string obtenerCadenaConexionPrueba()
        {
            return @"Data Source=172.19.3.170; Initial Catalog=LoginCentralizado; User=sa; PassWord=dbadmin";
        }


    }
}
