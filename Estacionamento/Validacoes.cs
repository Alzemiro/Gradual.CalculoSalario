using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EstacionamentoGradual.Model;

namespace EstacionamentoGradual
{
    public static class Validacoes
    {
        public static string ValidaMenu(string entrada)
        {
          string[] valida = new string[4] { "0", "1", "2", "3"};
          
                if (!valida.Contains(entrada))
                {
                    Console.Write("Digite um item válido do menu: ");
                    entrada = Console.ReadLine();
                    return ValidaMenu(entrada);
                }

            return entrada.Trim();
        }


    }
}
