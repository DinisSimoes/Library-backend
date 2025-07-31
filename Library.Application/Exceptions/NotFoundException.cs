using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Application.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity) : base($"Entidade '{entity}' não encontrada.") { }

        public NotFoundException(string name, object key)
            : base($"Entidade '{name}' com a chave '{key}' não foi encontrada.") { }
    }
}
