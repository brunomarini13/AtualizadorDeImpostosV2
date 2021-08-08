using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtualizadorDeImpostosV2.Entities
{
    class Lancamento
    {
        public string Data { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string Valor { get; set; }
        public string CodHistorico { get; set; }
        public string ComplementoHistorico { get; set; }
        public string IniciaLote { get; set; }

        public Lancamento(string data, string debito, string credito, string valor, string codHistorico, string complementoHistorico, string iniciaLote)
        {
            Data = data;
            Debito = debito;
            Credito = credito;
            Valor = valor;
            CodHistorico = codHistorico;
            ComplementoHistorico = complementoHistorico;
            IniciaLote = iniciaLote;
        }

        public override string ToString()
        {
            string s = Data + Debito + Credito + Valor + CodHistorico + ComplementoHistorico + IniciaLote;
            return s;
        }
    }
}
