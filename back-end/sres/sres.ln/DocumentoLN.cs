using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sres.da;
using sres.be;
using System.Data;
using Oracle.DataAccess.Client;
using sres.ut;

namespace sres.ln
{
    public class DocumentoLN : BaseLN
    {
        DocumentoDA documentoDA = new DocumentoDA();
        public List<DocumentoBE> ListaBusquedaCaso(DocumentoBE entidad)
        {
            List<DocumentoBE> lista = new List<DocumentoBE>();

            try
            {
                cn.Open();
                lista = documentoDA.ListarBusquedaDocumento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return lista;
        }

        public DocumentoBE EliminarDocumento(DocumentoBE entidad)
        {
            DocumentoBE item = null;

            try
            {
                cn.Open();
                item = documentoDA.EliminarDocumento(entidad, cn);
            }
            finally { if (cn.State == ConnectionState.Open) cn.Close(); }

            return item;
        }
    }
}
