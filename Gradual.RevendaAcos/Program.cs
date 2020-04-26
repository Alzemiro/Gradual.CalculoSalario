using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gradual.RevendaAcos
{
    class Program
    {
        static void Main(string[] args)
        {
            Pedido pd = new Pedido();
            string escolha = string.Empty;
            

            while (escolha != "0")
            {
                Console.WriteLine("------------------------------\n" +
                                  "Revenda de Aços Gradual\n" +
                                  "------------------------------\n");
                Console.WriteLine("Menu Principal");
                Console.WriteLine("1 - Realizar Pedido");
                Console.WriteLine("2 - Relatorio");
                Console.WriteLine("3 - Sair \n");
                Console.Write("Digite a opção desejada: ");
                escolha = Console.ReadLine();                
                
                switch (Convert.ToInt32(escolha))
                {
                    case 1:
                        Console.Clear();
                        pd.MenuRealizarPedido();                        
                        break;
                    case 2:
                        Console.Clear();
                        pd.Relatorios();                        
                        break;
                    case 3:
                        escolha = "0";
                        break;
                    default:
                        Console.WriteLine("Escolha uma opção valida!");
                        break;
                }
                    


            }

        }
    }   

}
