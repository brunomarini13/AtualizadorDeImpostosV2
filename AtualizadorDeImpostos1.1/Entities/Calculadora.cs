using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtualizadorDeImpostosV2.Entities
{
    static class Calculadora
    {
        public static DateTime DataFinalAtualizacao { get; set; }

        public static double SomatorioMulta { get; set; }


        public static double[] CalculaMulta(ObjetoDeAtualização obj, DateTime dataFinal)
        {
            SomatorioMulta = 0.0;
            DataFinalAtualizacao = dataFinal;
            int months = GetMonthsBetween(DataFinalAtualizacao, obj.Vencimento);
            if (obj.NomeDoImposto == Enums.Impostos.CSLL || obj.NomeDoImposto == Enums.Impostos.IRPJ)
            {
                months++;
            }
            double[] multas = new double[months + 1];
            double tempResult = 0.0;

            for (int i = 0; i <= months; i++)
            {
                if (i == 0)
                {
                    int days = DateTime.DaysInMonth(obj.Vencimento.Year, obj.Vencimento.Month);
                    int diff = days - obj.Vencimento.Day;
                    tempResult = diff * 0.0033 * obj.ValorPrincipal;
                }
                else
                {
                    DateTime temp = obj.Vencimento.AddMonths(i);
                    int days = DateTime.DaysInMonth(temp.Year, temp.Month);
                    tempResult = days * 0.0033 * obj.ValorPrincipal;
                }
                if (SomatorioMulta < (0.2 * obj.ValorPrincipal))
                {
                    if ((SomatorioMulta + tempResult) <= (0.2 * obj.ValorPrincipal))
                    {
                        multas[i] = tempResult;
                        SomatorioMulta += tempResult;
                    }
                    else
                    {
                        multas[i] = (0.2 * obj.ValorPrincipal) - SomatorioMulta;
                        SomatorioMulta += tempResult;
                    }
                }
                else
                {
                    multas[i] = 0.0;
                }
            }
            return multas;
        }

        public static double[] CalculaJuros(ObjetoDeAtualização obj, DateTime dataFinal)
        {
            int months = GetMonthsBetween(DataFinalAtualizacao, obj.Vencimento);
            if (obj.NomeDoImposto == Enums.Impostos.CSLL || obj.NomeDoImposto == Enums.Impostos.IRPJ)
            {
                months++;
            }
            double[] juros = new double[months + 1];
            juros[0] = 0.0;
            string[] temp = TaxasSelic.LerArquivo(@"C:\Users\bruno\Desktop\Temporária\SELIC.txt");
            string[,] temp2 = TaxasSelic.InserirDados(temp);
            DateTime calc;

            for (int i = 1; i <= months; i++)
            {
                calc = obj.Vencimento.AddMonths(i);
                string temp4 = calc.ToString("MM/yyyy");
                double taxaMes = (TaxasSelic.ChecarCompetencia(temp4, temp2)) / 10000;
                double value = taxaMes * obj.ValorPrincipal;
                juros[i] = value;
            }
            return juros;
        }

        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        public static List<ObjetoDeAtualização> MyList(ObjetoDeAtualização obj)
        {
            List<ObjetoDeAtualização> myList = new List<ObjetoDeAtualização>();
            if (obj.NomeDoImposto == Enums.Impostos.PIS && obj.Competencia.Month == 3 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("25/08/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.PIS && obj.Competencia.Month == 4 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("23/10/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.PIS && obj.Competencia.Month == 5 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("25/11/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.COFINS && obj.Competencia.Month == 3 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("25/08/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.COFINS && obj.Competencia.Month == 4 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("23/10/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.COFINS && obj.Competencia.Month == 5 && obj.Competencia.Year == 2020)
            {
                ObjetoDeAtualização novo = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo.Vencimento = DateTime.Parse("25/11/2020");
                myList.Add(novo);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 3 && obj.Competencia.Year == 2020)
            {
                Console.WriteLine();
                Console.Write("Digite a parte municipal/estadual do imposto (valor do ISS/ICMS): ");
                double part1 = double.Parse(Console.ReadLine());
                double part2 = obj.ValorPrincipal - part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("20/07/2020");
                novo2.Vencimento = DateTime.Parse("20/10/2020");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 4 && obj.Competencia.Year == 2020)
            {
                Console.WriteLine();
                Console.Write("Digite a parte municipal/estadual do imposto (valor do ISS/ICMS): ");
                double part1 = double.Parse(Console.ReadLine());
                double part2 = obj.ValorPrincipal - part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("20/08/2020");
                novo2.Vencimento = DateTime.Parse("20/11/2020");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 5 && obj.Competencia.Year == 2020)
            {
                Console.WriteLine();
                Console.Write("Digite a parte municipal/estadual do imposto (valor do ISS/ICMS): ");
                double part1 = double.Parse(Console.ReadLine());
                double part2 = obj.ValorPrincipal - part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("20/09/2020");
                novo2.Vencimento = DateTime.Parse("21/12/2020");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 3 && obj.Competencia.Year == 2021)
            {
                double part1 = obj.ValorPrincipal / 2.0;
                double part2 = part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("20/07/2021");
                novo2.Vencimento = DateTime.Parse("20/08/2021");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 4 && obj.Competencia.Year == 2021)
            {
                double part1 = obj.ValorPrincipal / 2.0;
                double part2 = part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("20/09/2021");
                novo2.Vencimento = DateTime.Parse("20/10/2021");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 5 && obj.Competencia.Year == 2021)
            {
                double part1 = obj.ValorPrincipal / 2.0;
                double part2 = part1;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part1);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, part2);
                novo1.Vencimento = DateTime.Parse("22/11/2021");
                novo2.Vencimento = DateTime.Parse("20/12/2021");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.Simples && obj.Competencia.Month == 1 && obj.Competencia.Year == 2021)
            {
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, obj.ValorPrincipal);
                novo1.Vencimento = DateTime.Parse("26/02/2021");
                myList.Add(novo1);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.INSS && obj.Competencia.Month == 3 && obj.Competencia.Year == 2020)
            {
                Console.WriteLine();
                Console.Write("Digite o valor do INSS Patronal (Parte Empresa): ");
                double partEmpresa = double.Parse(Console.ReadLine());
                double segurados = obj.ValorPrincipal - partEmpresa;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, segurados);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, partEmpresa);
                novo1.Vencimento = DateTime.Parse("20/04/2020");
                novo2.Vencimento = DateTime.Parse("20/08/2020");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else if (obj.NomeDoImposto == Enums.Impostos.INSS && obj.Competencia.Month == 4 && obj.Competencia.Year == 2020)
            {
                Console.WriteLine();
                Console.Write("Digite o valor do INSS Patronal (Parte Empresa): ");
                double partEmpresa = double.Parse(Console.ReadLine());
                double segurados = obj.ValorPrincipal - partEmpresa;
                ObjetoDeAtualização novo1 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, segurados);
                ObjetoDeAtualização novo2 = new ObjetoDeAtualização(obj.NomeDoImposto, obj.Competencia, partEmpresa);
                novo1.Vencimento = DateTime.Parse("20/05/2020");
                novo2.Vencimento = DateTime.Parse("20/10/2020");
                myList.Add(novo1);
                myList.Add(novo2);
            }
            else
            {
                obj.CalcularVencimento(obj);
                myList.Add(obj);
            }
            return myList;
        }
    }
}
