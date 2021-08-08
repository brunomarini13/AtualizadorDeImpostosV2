using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtualizadorDeImpostosV2.Entities.Enums;
using AtualizadorDeImpostosV2.Entities;
using System.IO;

namespace AtualizadorDeImpostosV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Atualizador de Impostos Versão 1.1");
            Console.WriteLine();
            Console.Write("Cole o endereço da pasta para a saída do arquivo de importação, e aperte Enter: ");
            string path = @Console.ReadLine();
            Console.WriteLine();
            Console.Write("Digite o código da conta de multa e aperte Enter: ");
            string contaMulta = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Digite o código da conta de juros e aperte Enter: ");
            string contaJuros = Console.ReadLine();
            Console.WriteLine();
            StreamWriter sw = new StreamWriter(path + @"\IMP.txt", true, Encoding.UTF8);
            string answer = "";
            int[] values = (int[])(Enum.GetValues(typeof(Impostos)));
            while (answer != "n")
            {
                for (int i = 1; i <= 8; i++)
                {
                    Console.WriteLine(values[i - 1] + " - " + (Enum.GetName(typeof(Impostos), i)));
                }
                Console.WriteLine();
                Console.Write("Digite um número, de 1 a 8, referente ao tributo a atualizar, conforme legenda acima, e aperte Enter: ");
                string a = Console.ReadLine();
                Impostos b = (Impostos)Enum.Parse(typeof(Impostos), a);
                Console.WriteLine();
                Console.Write("Digite a competência do tributo, conforme modelo \"02/2020\", e aperte enter: ");
                DateTime competencia = DateTime.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Digite o valor original da guia, e aperte enter: ");
                double valorOriginal = double.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Tributo já foi atualizado anteriormente? (Digite s ou n, e aperte enter) ");
                char ans = char.Parse(Console.ReadLine());
                Console.WriteLine();
                DateTime prevDate = competencia;
                if (ans == 's')
                {
                    Console.Write("Digite a data da última atualização efetuada, conforme modelo \"31/12/2020\", e aperte Enter: ");
                    prevDate = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine();
                }
                Console.Write("Digite a data final da atualização, conforme modelo \"31/12/2020\", e aperte Enter: ");
                DateTime dataFinal = DateTime.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write("Digite o código da conta do imposto e aperte Enter: ");
                string contaImposto = Console.ReadLine();

                ObjetoDeAtualização ob = new ObjetoDeAtualização(b, competencia, valorOriginal);
                List<ObjetoDeAtualização> lista = Calculadora.MyList(ob);

                //começa aqui
                foreach (ObjetoDeAtualização obj in lista)
                {
                    double[] multas = Calculadora.CalculaMulta(obj, dataFinal);
                    double[] juros = Calculadora.CalculaJuros(obj, dataFinal);
                    List<string> multasImp = new List<string>();
                    List<string> jurosImp = new List<string>();

                    int days = DateTime.DaysInMonth(obj.Vencimento.Year, obj.Vencimento.Month);
                    DateTime dataInicio = new DateTime(obj.Vencimento.Year, obj.Vencimento.Month, days);
                    DateTime temp = DateTime.Now;

                    for (int i = 0; i < multas.Length; i++)
                    {
                        temp = dataInicio.AddMonths(i);
                        days = DateTime.DaysInMonth(temp.Year, temp.Month);
                        DateTime temp2 = new DateTime(temp.Year, temp.Month, days);
                        string date = temp2.ToString("dd/MM/yyyy");
                        string unit = date + " - " + multas[i].ToString("F2");
                        multasImp.Add(unit);
                    }
                    for (int i = 0; i < juros.Length; i++)
                    {
                        temp = dataInicio.AddMonths(i);
                        days = DateTime.DaysInMonth(temp.Year, temp.Month);
                        DateTime temp2 = new DateTime(temp.Year, temp.Month, days);
                        string date = temp2.ToString("dd/MM/yyyy");
                        string unit = date + " - " + juros[i].ToString("F2");
                        jurosImp.Add(unit);
                    }

                    List<string> multaTestada = new List<string>();
                    List<string> jurosTestada = new List<string>();

                    for (int i = 0; i < multasImp.Count; i++)
                    {
                        string[] tempMulta = multasImp[i].Split(' ');
                        DateTime multaTeste = DateTime.Parse(tempMulta[0]);
                        double test = double.Parse(tempMulta[2]);
                        if (multaTeste.Ticks > prevDate.Ticks && test != 0)
                        {
                            multaTestada.Add(multasImp[i]);
                        }
                    }
                    for (int i = 0; i < jurosImp.Count; i++)
                    {
                        string[] tempJuros = jurosImp[i].Split(' ');
                        DateTime jurosTeste = DateTime.Parse(tempJuros[0]);
                        double test = double.Parse(tempJuros[2]);
                        if (jurosTeste.Ticks > prevDate.Ticks && test != 0)
                        {
                            jurosTestada.Add(jurosImp[i]);
                        }
                    }
                    Console.WriteLine();
                    List<Lancamento> lctos = new List<Lancamento>();
                    lctos.Clear();
                    string imposto = b.ToString();

                    for (int i = 0; i < jurosImp.Count; i++)
                    {
                        if (i < multaTestada.Count)
                        {
                            string[] tempMulta = multaTestada[i].Split(' ');
                            Lancamento l = new Lancamento(tempMulta[0] + ";", contaMulta + ";", contaImposto + ";", tempMulta[2] + ";", "" + ";", "VALOR REF. ATUALIZAÇÃO DE MULTA S/ " + imposto.ToUpper() + " COMPET. " + competencia.ToString("MM/yyyy") + " VCTO. " + obj.Vencimento.ToString("dd/MM/yyyy") + "." + ";", "1");
                            lctos.Add(l);
                        }
                        if (i < jurosTestada.Count)
                        {
                            string[] tempJuros = jurosTestada[i].Split(' ');
                            Lancamento l = new Lancamento(tempJuros[0] + ";", contaJuros + ";", contaImposto + ";", tempJuros[2] + ";", "" + ";", "VALOR REF. ATUALIZAÇÃO DE JUROS S/ " + imposto.ToUpper() + " COMPET. " + competencia.ToString("MM/yyyy") + " VCTO. " + obj.Vencimento.ToString("dd/MM/yyyy") + "." + ";", "1");
                            lctos.Add(l);
                        }
                    }

                    foreach (Lancamento l in lctos)
                    {
                        sw.WriteLine(l.ToString());
                        sw.Flush();
                    }
                    lctos.Clear();
                }
                // termina aqui

                Console.Write("Deseja realizar outra atualização? (Digite s ou n, e aperte Enter) ");
                answer = Console.ReadLine();
                Console.WriteLine();
            }
            Console.WriteLine("Atualização efetuada com sucesso! O arquivo de importação foi gerado.");
            Console.WriteLine("Para importar, siga o caminho no Domínio Contábil:");
            Console.WriteLine("Contabilidade --> Utilitários --> Importação --> Importador --> Importar");
            Console.WriteLine("============================================");
            Console.WriteLine("SUGESTÃO: Criar favorito do caminho acima");
            Console.WriteLine("============================================");
            Console.WriteLine("No conjunto de dados, selecione \"Lançamentos Contábeis (Partidas Simples/Múltiplas) (3.0) (Excel (3.0))\"");
            Console.WriteLine("Após, selecione o caminho do arquivo que foi gerado, conforme digitado nesse programa");
            Console.WriteLine("Por fim, selecionar o arquivo \"IMP.txt\" e clicar em \"Importar\"");
            Console.WriteLine("Pressione qualquer tecla para fechar o programa.");
            Console.ReadKey();
        }
    }
}
