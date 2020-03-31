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
            es.totalRecibos();

        }

    }

    public static class TabelaCobranca
    {
        public const double precoHR = 2;
        public const double precoAdicionalHr = 0.5;
        public const double precoPacote24Hr = 10;

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
        public double resultado;
        public double resultadoDiaAtual;
        public double resultadoDiaAnteior;
        public void calculaCusto()
        {

            resultado = 0;
            dynamic[,] carros = new dynamic[4, 4];


            List<ClienteCarro> novoCarro = new List<ClienteCarro>();

            carros[0, 0] = "IQW-0000";
            carros[0, 1] = DateTime.Parse("30/03/2020");
            carros[0, 2] = DateTime.Parse("30/03/2020 08:15:00");
            carros[0, 3] = DateTime.Parse("30/03/2020 14:00:00");

            carros[1, 0] = "ASD-1000";
            carros[1, 1] = DateTime.Parse("30/03/2020");
            carros[1, 2] = DateTime.Parse("30/03/2020 09:00:00");
            carros[1, 3] = DateTime.Parse("30/03/2020 20:00:59");

            carros[2, 0] = "FER-2300";
            carros[2, 1] = DateTime.Parse("29/03/2020");
            carros[2, 2] = DateTime.Parse("29/03/2020 11:00:00");
            carros[2, 3] = DateTime.Parse("29/03/2020 13:00:00");

            carros[3, 0] = "WSA-5555";
            carros[3, 1] = DateTime.Parse("29/03/2020");
            carros[3, 2] = DateTime.Parse("29/03/2020 17:00:00");
            carros[3, 3] = DateTime.Parse("30/03/2020 18:59:00");


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
                                      "Pagando o valor total de: R$ {3}",
                                      item.data.ToShortDateString(),
                                      item.placa,
                                      showHora(item.horaEntrada, item.horaSaida),
                                      custoTotal(item.horaEntrada, item.horaSaida)
                                      );
                    Console.WriteLine("-----------------------------------------------------");
                    resultado += custoTotal(item.horaEntrada, item.horaSaida);

                    int t = 0;
                    if (t <= novoCarro.Count)
                    {
                        if (DateTime.Compare(item.data, carros[t+1, 1]) < 0)
                        {
                            resultadoDiaAnteior += custoTotal(item.horaEntrada, item.horaSaida);
                        }
                        if (DateTime.Compare(item.data, carros[t+1, 1]) == 0)
                        {
                            resultadoDiaAtual += custoTotal(item.horaEntrada, item.horaSaida);
                        }
                    }
                    i++;
                }

            }

        }

        public void totalRecibos()
        {
            Console.WriteLine("Faturamento Total: {0}", resultado);
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Faturamento dia Atual: {0}", resultadoDiaAtual);
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Faturamento dia Anterior: {0}", resultadoDiaAnteior);
            Console.WriteLine("-----------------------------------------------------");
        }

        double custoTotal(DateTime entrada, DateTime saida)
        {
            double resultado;
            resultado = 0;

            //Para não ficar negativo o menor deve ser subtraido do maior
            TimeSpan ts = new TimeSpan(saida.Ticks - entrada.Ticks);
            double limite = ts.TotalMinutes;
            double horas = ts.Hours;

            for (int i = 0; i <= horas; i++)
            {
                if (limite < 181)
                {
                    resultado = TabelaCobranca.precoHR;
                }
                else if (limite >= 181 && limite < 1440)
                {
                    resultado += TabelaCobranca.precoAdicionalHr;
                }
                else
                {
                    resultado = TabelaCobranca.precoPacote24Hr;
                }
            }
            return resultado;
        }

        string showHora(DateTime entrada, DateTime saida)
        {
            string dia;

            TimeSpan ts = new TimeSpan(saida.Ticks - entrada.Ticks);
            dia = ts.ToString(@"dd\.hh\:mm");

            return dia;
        }

    }
}


