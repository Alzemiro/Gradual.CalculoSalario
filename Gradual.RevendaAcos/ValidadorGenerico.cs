using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Gradual.RevendaAcos
{
    public static class ValidadorGenerico
    {
        public static string AceitaApenasNumeros(string entrada)
        {
            //Verifica se a entrada é composta somente por números
            entrada = Console.ReadLine();
            Regex rgx = new Regex("\\d");

            if (rgx.IsMatch(entrada))
            {
                entrada = entrada.Replace(',', '.');
            }
            else
            {
                Console.Write("Digite uma quantidade válida: ");
                AceitaApenasNumeros(entrada);
            }
            return entrada;
        }
    }
}

