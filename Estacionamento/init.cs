using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using EstacionamentoGradual.Model;
using System.Linq;

namespace EstacionamentoGradual
{
    public class Init
    {



        //Classe de inicialização
        public void Menu()
        {

            string op;
            int escolha;
            op = "";

            //Menu Principal
            Console.WriteLine("-----------------------------------------\n" +
                              "Bem-vindo ao Estacionamento Gradual \n" +
                              "-------------------------------------------");
            Console.WriteLine("Opções \n" +
                              "1 - Incluir novo veículo\n" +
                              "2 - Consultar Placa\n" +
                              "3 - Relatorios\n" +
                              "0 - Sair \n");
            Console.Write("Insira sua opção: ");

            //Inicialização da Validação
            

            op = Validacoes.ValidaMenu(op);


            switch (op)
            {
                case "1":
                    IncluiVeiculo();
                    break;

            }
        }


        public void IncluiVeiculo()
        {
            string id = "";
            string placaVeiculo = "";
            string escolha = "";
            Console.Write("Digite um código para o Carro (apenas numeros): ");
            id  = Console.ReadLine();

            int idConv = Convert.ToInt32(id);

            Console.WriteLine("Digite a Placa do veiculo: ");
            placaVeiculo = Console.ReadLine();

            ClienteCarro novoCarro = new ClienteCarro(idConv, placaVeiculo);

            Estacionamento.ClienteCarros.Add(novoCarro);

            Console.Write("Deseja incluir novo faturamento para o veiculo {0} S/N? ", id);
            
            if(escolha == "s" || escolha == "S")
            {
                IncluiFaturamento();
            }
            else
            {
              Menu();
            }            
        }


        public void IncluiFaturamento()
        {
            string clienteCarroId;
            int clienteCarroIdConv;
            string dataRegistro;
            DateTime dataReg;
            string horaEntrada;
            string horaSaida;
            double valorCobrado = 25;

            Console.Write("Insira o ID do Veiculo: ");
            clienteCarroId = Console.ReadLine();
            clienteCarroIdConv = Convert.ToInt32(clienteCarroId);
            
            dataReg = DateTime.Now;
            dataRegistro = dataReg.ToString("dd/MM/yyyy");
            Console.Write("Insia a hora de entrada: ");
            horaEntrada = Console.ReadLine();
            Console.WriteLine("Insira a hora de saída: ");
            horaSaida = Console.ReadLine();
            Console.WriteLine("Valor cobrado: ", valorCobrado);

            ClienteCarro.Faturamentos.Add(new Faturamento(clienteCarroIdConv, dataRegistro, horaEntrada, horaSaida, valorCobrado));
            calculaCusto();
        }

        public double calculaCusto()
        {

            var fat = ClienteCarro.Faturamentos;
            var clienteCarros = Estacionamento.ClienteCarros;

            var result = from item in fat
                         join car in clienteCarros
                         on item.clienteCarroId equals car.clienteCarroId
                         select new
                         {
                             car.placa,
                             item.dataRegistro,
                             item.valorCobrado
                         };

          

            foreach (var item in result)
            {
                Console.WriteLine("Placa: " + item.placa + "\n" +
                                  "Data registro: " + item.dataRegistro + "\n" +
                                  "Valor Cobrado: " + item.valorCobrado + "\n");
            }
            


            return 1;
        }

    }
}
