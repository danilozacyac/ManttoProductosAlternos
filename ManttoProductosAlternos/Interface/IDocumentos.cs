using System;
using System.Collections.Generic;
using System.Linq;
using ManttoProductosAlternos.Dto;

namespace ManttoProductosAlternos.Interface
{
    public interface IDocumentos
    {
        DocumentoTO GetDocumentoPorIus(long ius);
        void SetDocumento(DocumentoTO documento, int idTipoDocumento);
        List<DocumentoTO> GetDocumentosRelacionados(int idTipoDocumento);
        void DeleteDocumento(long ius, int idTipoDocumento);
    }
}
