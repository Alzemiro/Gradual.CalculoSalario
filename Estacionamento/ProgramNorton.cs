using System;
using System.Collections.Generic;


namespace EstacionamentoGradual
{
    class Program
    {
        static void Main(string[] args)
        {
            Estacionamento es = new Estacionamento();

            //Requsito Ok, mas tem erro de calculo!!!
            es.calculaCusto();


            es.totalRecibos();

        }

    }

    //OK, mas penso que isso ficaria melhor na classe estacionamento, do que em separado
    //Em BD até se justifica ter uma estrura a parte de cobrança, mas para nosso desafio,
    //não era necessário
    public static class TabelaCobranca
    {
        public const double precoHR = 2;
        public const double precoAdicionalHr = 0.5;
        public const double precoPacote24Hr = 10;

    }

    //Não foram solicitadas informações de placa do carro.
    //Poderia estar tudo dentro de estacionamento.
    //Precisamos para o desafio é somente horário de entrada, e horário de saída
    /*
     * 
     *  Essa variável data está sobrando.
     *  Dois objetos DateTime, dtEntrada e dtSaída, seriam suficientes.
     *  Seguindo a tua lógica, temos falta de informação. Não temos a data da saída...
     * 
     */
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

        //ADICIONADO PELO NORTON
        public ClienteCarro () {
            this.placa = "";
            this.data = DateTime.Now;
            this.horaEntrada = DateTime.Now;
            this.horaSaida = new DateTime();
        }

    }

    public class Estacionamento
    {
        public double resultado = 0;
        public double resultadoDiaAtual = 0;
        public double resultadoDiaAnteior = 0;

        public void calculaCusto()
        {
            //Aqui é onde me refiro onde "foi exagerado e duplicidade de código"
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

            //PROBLEMA!! Uma boa prática é permitir construtores vazios nas classes!
            //Tu está obrigando uma instância de objetos já com valores.
            //Por isso, tu está precisando de uma estrutura como a que tu montou.
            //Olha o trabalho que tu está passando por não permitir instanciar o objeto e popular as informaçoes depois...
            //Poderia ter feito dessa forma:
            ClienteCarro oClienteCarro = new ClienteCarro();
            oClienteCarro.placa = "IQW-0000";
            oClienteCarro.horaEntrada = new DateTime(2020, 03, 31, 14, 04, 00);
            oClienteCarro.horaSaida = new DateTime(2020, 03, 31, 18, 38, 00);
            //E agora, vamos adicionar a lista
            novoCarro.Add(oClienteCarro);
            //Viu como fica mais simples?

            novoCarro.Add(new ClienteCarro(carros[0, 0], carros[0, 1], carros[0, 2], carros[0, 3]));
            novoCarro.Add(new ClienteCarro(carros[1, 0], carros[1, 1], carros[1, 2], carros[1, 3]));
            novoCarro.Add(new ClienteCarro(carros[2, 0], carros[2, 1], carros[2, 2], carros[2, 3]));
            novoCarro.Add(new ClienteCarro(carros[3, 0], carros[3, 1], carros[3, 2], carros[3, 3]));

            //Aqui está o maior problema do teu código.
            //Essa tua estrutura tem tudo para dar errado.
            //Tu está usando uma dupla iteração, incrementando 
            //uma variável de loop que já é incrementada pela própria estrutura (uma abordagem muito incorreta!)

            //Minha sugestão (e vou fazer de dois jeitos, para tu ver que a estrutura simples é suficiente
            //Nivel iniciante, pequeno gafanhoto
            for (int j=0; j<novoCarro.Count; j++) {
                string sTexto = "No dia " + novoCarro[j].data.ToShortDateString() + "\n";
                sTexto += "O carro de placa " + novoCarro[j].placa + "\n";
                sTexto += "Ficou estacionado por: " + showHora(novoCarro[j].horaEntrada, novoCarro[j].horaSaida) + "\n";
                sTexto += "Pagando o valor total de: R$ " + custoTotal(novoCarro[j].horaEntrada, novoCarro[j].horaSaida) + "\n";
                Console.WriteLine(sTexto);
                Console.WriteLine("-----------------------------------------------------");
                resultado += custoTotal(novoCarro[j].horaEntrada, novoCarro[j].horaSaida);

                //Agora o elefante na tua sala: o relatório!
                //Ao meu ver, tua abordagem está incorreta. E como está errada, tu teve de fazer adaptações para que ela funcionasse. 
                //Era melhor ter tentado de outra maneira, do que ter insistido no que tu fizeste.
                //Outra coisa: Se eu trocar a data do primeiro carro a estacionar, o que vai acontecer?
                //Exatamente... vai dar erro! Tu confiou tua data atual a primeira entrada da tua lista
                //Esse erro... não da para aceitar, né? 
                //Olha como pode ser feito:

                //Data atual -- Deve ser criada e atribuida fora do loop... Estamos desperdiçando memória aqui
                DateTime dtDiaAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                //Testes
                if (novoCarro[j].data < dtDiaAtual) {
                    resultadoDiaAnteior += custoTotal(novoCarro[j].horaEntrada, novoCarro[j].horaSaida);
                }

                if (novoCarro[j].data >= dtDiaAtual) {
                    resultadoDiaAtual += custoTotal(novoCarro[j].horaEntrada, novoCarro[j].horaSaida);
                }
            }

            //Nivel "avançado"
            foreach (ClienteCarro c in novoCarro) {
                string sTexto = string.Format("No dia {0:dd/MM/yyyy}\n", c.data);
                sTexto = string.Concat(sTexto, string.Format("O carro de placa {0}\n", c.placa));
                sTexto = string.Concat(sTexto, string.Format("Ficou estacionado por: {0}\n", showHora(c.horaEntrada, c.horaSaida)));
                
                double dCustoTotal = custoTotal(c.horaEntrada, c.horaSaida);
                resultado += dCustoTotal;
                sTexto = string.Concat(sTexto, string.Format("Pagando o valor total de R$ {0}\n", dCustoTotal));

                Console.WriteLine(sTexto);
                Console.WriteLine("-----------------------------------------------------");

                //Agora o elefante na tua sala: o relatório!
                //Ao meu ver, tua abordagem está incorreta. E como está errada, tu teve de fazer adaptações para que ela funcionasse. 
                //Era melhor ter tentado de outra maneira, do que ter insistido no que tu fizeste.
                //Outra coisa: Se eu trocar a data do primeiro carro a estacionar, o que vai acontecer?
                //Exatamente... vai dar erro! Tu confiou tua data atual a primeira entrada da tua lista
                //Esse erro... não da para aceitar, né? 
                //Olha como pode ser feito:

                //Data atual -- Deve ser criada e atribuida fora do loop... Estamos desperdiçando memória aqui
                DateTime dtDiaAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                //Testes
                if (DateTime.Compare(c.data, dtDiaAtual) < 0) {
                    resultadoDiaAnteior += dCustoTotal;
                }

                if (DateTime.Compare(c.data, dtDiaAtual) >= 0) {
                    resultadoDiaAtual += dCustoTotal;
                }
            }

            //-------------------------------------------------------------------------------------------------------//

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


        //Calculo aqui está incorreto!
        //Lembra que o enunciado falava em:
        //"Um adicional de $0,50 por hora não necessariamente inteira é cobrado após as três primeiras horas."
        //Então, temos que arredondar o tempo para cima!
        //Ao usar essa abordagem, tu esqueceu de verificar se o carro adentrou uma nova hora, e cobrar ela "cheia".
        //Esta parte aqui precisa ser refeita.
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

            //Como eu faria, dentro do que tu definiu de estrutura de dados
            //Sem o "for" para horas... não é preciso! Tu tem tudo que precisa com os minutos!!!
            if (limite <= 180) {
                resultado = TabelaCobranca.precoHR;
            } else if (limite > 180 && limite < 1440) {
                //Obtem o tempo total, para efetuarmos os calculos
                double tempoTotal = limite;
                //Obtenho o total de horas, em inteiro
                double totalHoras = Math.Truncate(tempoTotal / 60);
                //Resto da divisão vai me dizer se tenho hora não completa, e cobrar por ela também
                double horaExcedente = tempoTotal % 60;

                double valorTotal = TabelaCobranca.precoHR + ((totalHoras - 3) * TabelaCobranca.precoAdicionalHr);
                if (horaExcedente > 0) {
                    valorTotal += TabelaCobranca.precoAdicionalHr;
                }

                resultado = valorTotal;

                //double dRestoHora = limite % 60;
                //resultado += TabelaCobranca.precoAdicionalHr;
            } else {
                resultado = TabelaCobranca.precoPacote24Hr;
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


