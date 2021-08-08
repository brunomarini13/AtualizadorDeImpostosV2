using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AtualizadorDeImpostosV2.Entities.Exceptions;

namespace AtualizadorDeImpostosV2.Entities
{
    public static class TaxasSelic
    {
        public static string[] LerArquivo(string s)
        {
            string[] lines = File.ReadAllLines(s);
            return lines;
        }

        public static string[,] InserirDados(string[] linha)
        {
            string[,] vet = new string[linha.Length, 2];
            for (int i = 0; i < linha.Length; i++)
            {
                string[] composicao = linha[i].Split(' ');
                vet[i, 0] = composicao[0];
                vet[i, 1] = composicao[1];
            }
            return vet;
        }

        public static double ChecarCompetencia(string input, string[,] vet)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < vet.GetLength(0); i++)
            {
                temp.Add(vet[i, 0]);
            }
            for (int j = 0; j < vet.GetLength(0); j++)
            {
                if (temp[j].Contains(input))
                {
                    return double.Parse(vet[j, 1]);
                }
            }
            throw new DomainException("A competência digitada está no modelo inválido, favor digitar a competência no modelo \"02/2021\", por exemplo.");
        }
    }
}
