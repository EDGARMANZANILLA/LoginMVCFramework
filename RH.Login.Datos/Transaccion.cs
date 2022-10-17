
using RH.Login.Datos.DBLogin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Datos
{
    public class Transaccion : IDisposable
    {
        private LoginCentralizadoEntities _contexto;
        public Transaccion()
        {
            _contexto = new LoginCentralizadoEntities();
        }
        internal LoginCentralizadoEntities Contexto
        {
            get
            {
                return _contexto;
            }
        }
        public void GuardarCambios()
        {
            _contexto.SaveChanges();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }






        /******************************************************************************************************************************************/
        /**************************************************  Metodos Asincronicos       **********************************************************/
        //public Transaccion()
        //{
        //    _contexto = new FoliacionInterfacesCargadasEntities();
        //}

        public async Task GuardarCambiosAsincronicos()
        {
            await _contexto.SaveChangesAsync();
        }



    }
}
