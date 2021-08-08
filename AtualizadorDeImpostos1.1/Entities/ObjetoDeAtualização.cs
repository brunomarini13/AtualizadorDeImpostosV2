using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtualizadorDeImpostosV2.Entities.Enums;

namespace AtualizadorDeImpostosV2.Entities
{
    class ObjetoDeAtualização
    {
        public Impostos NomeDoImposto { get; set; }
        public DateTime Competencia { get; set; }
        public double ValorPrincipal { get; set; }
        public DateTime Vencimento { get; set; }

        public ObjetoDeAtualização(Impostos nomeDoImposto, DateTime competencia, double valorPrincipal)
        {
            NomeDoImposto = nomeDoImposto;
            Competencia = competencia;
            ValorPrincipal = valorPrincipal;
        }

        public void CalcularVencimento(ObjetoDeAtualização obj)
        {
            DateTime temp = Competencia.AddMonths(1);
            string s = temp.ToString("dd/MM/yyyy");
            string[] temporario = s.Split('/');
            string mes = temporario[1];
            string ano = temporario[2];

            if (NomeDoImposto == Impostos.Simples)
            {
                string dia = "20";
                s = dia + "/" + mes + "/" + ano;
                temp = DateTime.Parse(s);
                while (temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    temp = temp.AddDays(1);
                }
            }
            else if (NomeDoImposto == Impostos.PIS || NomeDoImposto == Impostos.COFINS)
            {
                string dia = "25";
                s = dia + "/" + mes + "/" + ano;
                temp = DateTime.Parse(s);
                while (temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    temp = temp.AddDays(-1);
                }
            }
            else if (NomeDoImposto == Impostos.CSLL || NomeDoImposto == Impostos.IRPJ)
            {
                int day = DateTime.DaysInMonth(temp.Year, temp.Month);
                string dia = day.ToString();
                s = dia + "/" + mes + "/" + ano;
                temp = DateTime.Parse(s);
                while (temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    temp = temp.AddDays(-1);
                }
            }
            else
            {
                string dia = "20";
                s = dia + "/" + mes + "/" + ano;
                temp = DateTime.Parse(s);
                while (temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    temp = temp.AddDays(-1);
                }
            }
            Vencimento = temp;
        }
    }
}
