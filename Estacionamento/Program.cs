using System;
using System.Collections.Generic;


namespace EstacionamentoGradual
{
    class Program
    {
        static void Main(string[] args)
        {
            Estacionamento es = new Estacionamento();
            es.calculaCusto();
          
        }
        
     }

    public static class TabelaCobranca
    {
        public const decimal precoHR = 2M;
        public const decimal precoAdicionalHr = 0.5M;
        public const decimal precoPacote24Hr = 10M;

    }

    public class ClienteCarro
    {
        public string placa;
        public DateTime data;
        public DateTime horaEntrada;
        public DateTime horaSaida;

        public ClienteCarro(string placa, DateTime data, DateTime horaEntrada, DateTime horaSaida)
        {
            this.placa = placa;
            this.data = data;
            this.horaEntrada = horaEntrada;
            this.horaSaida = horaSaida;
        }
            
    }

    public class Estacionamento
    {
        public void calculaCusto(){

            double quantidadeHora;
            double custoTotal = 1;

            dynamic[,] carros = new dynamic[4, 4];

            List<ClienteCarro> novoCarro = new List<ClienteCarro>();

            carros[0,0] = "IQW-0000";
            carros[0,1] = DateTime.Parse("30/03/2020");
            carros[0,2] = DateTime.Parse("30/03/2020 08:15:00");
            carros[0,3] = DateTime.Parse("30/03/2020 14:00:00");

            carros[1,0] = "ASD-1000";
            carros[1,1] = DateTime.Parse("30/03/2020");
            carros[1,2] = DateTime.Parse("30/03/2020 09:00:00");
            carros[1,3] = DateTime.Parse("30/03/2020 20:00:59");

            carros[2,0] = "FER-2300";
            carros[2,1] = DateTime.Parse("29/03/2020");
            carros[2,2] = DateTime.Parse("29/03/2020 11:00:00");
            carros[2,3] = DateTime.Parse("29/03/2020 13:00:00");

            carros[3,0] = "WSA-5555";
            carros[3,1] = DateTime.Parse("29/03/2020");
            carros[3,2] = DateTime.Parse("29/03/2020 17:00:00");
            carros[3,3] = DateTime.Parse("29/03/2020 22:00:00");
            

             novoCarro.Add(new ClienteCarro(carros[0, 0], carros[0, 1], carros[0, 2], carros[0, 3]));
             novoCarro.Add(new ClienteCarro(carros[1, 0], carros[1, 1], carros[1, 2], carros[1, 3]));
             novoCarro.Add(new ClienteCarro(carros[2, 0], carros[2, 1], carros[2, 2], carros[2, 3]));
             novoCarro.Add(new ClienteCarro(carros[3, 0], carros[3, 1], carros[3, 2], carros[3, 3]));






            for (var i = 0; i <= novoCarro.Count; i++)
            {
                foreach (var item in novoCarro)
                {
                    Console.WriteLine("No dia {0} \n" +
                                      "O carro de placa: {1} \n" +
                                      "Ficou estacionado por: {2}\n" +
                                      "Pagando o valor total de: {3}", item.data.ToShortDateString(), item.placa,
                                      showHora(carros[i, 2], carros[i, 3]), custoTotal);
                    Console.WriteLine("-----------------------------------------------------");
                    i++;
                }
            }

                


            }

            double calculoHora(DateTime hora1, DateTime hora2)
            {
                double temp;
                double resultado;
                //o calculo tem de ser invertido para não haver numeros negativos
                TimeSpan ts = new TimeSpan(hora2.Ticks - hora1.Ticks);
                temp = ts.TotalMinutes;
                resultado = temp / 60;
                return Math.Round(resultado, 2);
            }
            string showHora(DateTime hora1, DateTime hora2)
            {
                string t;
                TimeSpan ts = new TimeSpan(hora2.Ticks - hora1.Ticks);
                t = ts.ToString(@"hh\:mm");
                return t;
            }



        }      
    
    
    }


