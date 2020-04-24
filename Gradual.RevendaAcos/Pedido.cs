using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;


//Fiz a abordagem utilizando Classes e List<T> separadas para ter maior poder de reutilização de código.

namespace Gradual.RevendaAcos
{
    public class Pedido : Produto, ITabelaDesconto<Pedido>
    {
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotalPedido { get; set; }
        public decimal ValorTotalCompra { get; set; }
        public decimal PedidoKg { get; set; }
        public decimal TotalKG { get; set; }
        public bool DescontoAplicado2 { get; set; }



        public Pedido()
        {
            this.ProdutoId = 0;
            this.Quantidade = 0.0m;
            this.DataPedido = DateTime.UtcNow;
            this.DescontoAplicado2 = false;
        }


        public List<Pedido> Pedidos = new List<Pedido>();
        public List<Pedido> PedidoFinalizado = new List<Pedido>();

        public List<Produto> Produtos = new List<Produto> {
            new Produto { Id = 1, Descricao = "Alumínio", ValorKg = 9.80m },
            new Produto { Id = 2, Descricao = "Carbono",   ValorKg = 12.50m }

         };

        public void MenuRealizarPedido()
        {

            string sair = string.Empty;


            while (sair != "0")
            {
                Console.Clear();
                Console.WriteLine("------------------------------\n" +
                                  "Revenda de Aços Gradual\n" +
                                  "------------------------------\n");
                Console.WriteLine("Qual destes produtos deseja comprar?");
                RealizarPedido();
                Console.Write("Continuar comprando (1 - Sim / 0 - Não) ? ");
                sair = Console.ReadLine();

            }
            Console.Clear();
            CalculoPedido();

        }



        private void RealizarPedido()
        {
            string entradaQuantidade = string.Empty;
            string opcao = string.Empty;

            //Mostra a Lista de Produtos
            foreach (var pdt in Produtos)
            {
                Console.WriteLine("{0} - {1}", pdt.Id, pdt.Descricao);
            };

            Console.Write("Digite o número referente ao produto desejado: ");
            opcao = Console.ReadLine();

            //Aqui começa a verificação, onde somente se pode selecionar um produto que exista e 
            //é adicionado um novo Pedido
            try
            {

                if (Produtos.Exists(x => x.Id.ToString() == opcao))
                {
                    var p = Produtos.Find(r => r.Id.ToString() == opcao);

                    Console.Write("Digite a quantidade de {0} desejada: ", p.Descricao);
                    entradaQuantidade = ValidadorGenerico.AceitaApenasNumeros(entradaQuantidade);

                    Pedidos.Add(new Pedido
                    {
                        ProdutoId = p.Id,
                        Quantidade = decimal.Parse(entradaQuantidade, CultureInfo.InvariantCulture),

                    });
                }
                else
                {
                    Console.WriteLine("----------------------\n" +
                                      "Produto não localizado!\n" +
                                      "----------------------\n");
                    RealizarPedido();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Quantidade inválida!");
                RealizarPedido();
            }
        }

        //Aqui foi onde tive mais dificuldades, porém acho que encontrei uma solução interessante
        public void CalculoPedido()
        {
            decimal valorTotalCompra = 0m;
            decimal valorPedido = 0m;
            decimal totalKgPedido = 0m;
            decimal TotalKg = 0m;
            

            Pedido novoPedido = new Pedido();

            //Tenho que percorrer a lista de produtos para fazer uma nova verificação, onde se existir um pedido
            //com determinado produto deve-se seguir os calculos
            foreach (var p in Produtos)
            {
                //Somente exibirá em tela e realizar calculos nos produtos que foram pedidos
                if (Pedidos.Exists(x => x.ProdutoId.Equals(p.Id)))
                {
                    //Soma a quantidade de Kg solicitados pelo cliente, se o Id do produto for igual ao ProdutoId que está no pedido
                    totalKgPedido = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade);

                    //Multiplica a quantidade de Kg pelo valor, se o Id do produto for igual ao ProdutoId que está no pedido
                    valorPedido = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade * p.ValorKg);

                    //Soma KG
                    TotalKg += totalKgPedido;

                    //Soma Total
                    valorTotalCompra += valorPedido;


                    novoPedido = new Pedido
                    {
                        ProdutoId = p.Id,
                        Descricao = p.Descricao,
                        PedidoKg = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade),
                        ValorTotalPedido = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade * p.ValorKg),
                        ValorKg = p.ValorKg,
                        
                        
                    };

                }

            }
            PedidoFinalizado.Add(novoPedido);
            Console.Clear();
            Console.WriteLine("--------------------------\n");
            Console.WriteLine("Valor total da Compra: {0}", valorTotalCompra.ToString("C"));
            Console.WriteLine("Total de Kg: {0}", TotalKg.ToString());
            Console.WriteLine("--------------------------");
            Console.WriteLine("Descontos");
            Console.WriteLine("--------------------------");

            var s = PedidoFinalizado.Select(s => s.DescontoAplicado2).FirstOrDefault();

            if(s == false){
                AplicaDesconto(PedidoFinalizado);
            }

        }

        public void AplicaDesconto(List<Pedido> lista)
        {
            //A melhor forma que encontrei para mostrar os descontos foi instanciar 2 StringBuilders
            StringBuilder sbValor = new StringBuilder();
            StringBuilder sbKg = new StringBuilder();
            bool DescontoAplicado = false;
            bool valorMenor5000 = false;
            bool valorMenor5000QuantidadeMenor300 = false;
            bool valorMaior5000Quantiade300 = false;

            decimal valorTotal10P = 0m;
            decimal valorTotal5P = 0m;



            foreach (var p in PedidoFinalizado)
            {



                    //Utilizei a quantidade total para aplicar o desconto de 10% a todos os produtos
                    p.TotalKG = PedidoFinalizado.Sum(x => x.PedidoKg);

                    if (p.TotalKG > 50)
                    {
                        sbKg.AppendFormat("{0}: {1} - {2}", p.Descricao, p.ValorKg.ToString("C"), (ITabelaDesconto<Pedido>.Acima50Kg).ToString("P"));

                        //Aplicação de 10% em cada produto                    
                        p.ValorKg = p.ValorKg - (p.ValorKg * ITabelaDesconto<Pedido>.Acima50Kg);
                        p.ValorTotalPedido = p.ValorTotalPedido - (p.ValorTotalPedido * 0.1M);
                        sbKg.AppendFormat(" = {0}, Valor com Desconto: {1} \n", p.ValorKg.ToString("C"), p.ValorTotalPedido.ToString("C"));

                        //Recalculo do Valor total
                        p.ValorTotalCompra = PedidoFinalizado.Sum(t => t.ValorTotalPedido);
                        DescontoAplicado = true;
                        
                        

                    }
                    if (p.ValorTotalCompra < 5000 && DescontoAplicado)
                    {
                        valorMenor5000 = true;
                    }
                    if (valorMenor5000 && p.TotalKG < 300)
                    {
                        valorMenor5000QuantidadeMenor300 = true;
                    }
                    if (p.ValorTotalCompra > 5000 || p.TotalKG >= 300)
                    {
                        valorMaior5000Quantiade300 = true;
                        valorTotal10P = p.ValorTotalCompra;
                        p.ValorTotalCompra = p.ValorTotalCompra - (p.ValorTotalCompra * ITabelaDesconto<Pedido>.Acima5000Reais);
                        valorTotal5P = p.ValorTotalCompra;
                    }                    
                
            }

            Console.WriteLine(sbKg.ToString());


            if (valorMenor5000QuantidadeMenor300)
            {
                sbValor.AppendFormat("Valor total com desconto: {0}  ", valorTotal10P.ToString("C"));
                Console.WriteLine(sbValor.ToString());
            }


            //Ultima validação aplicando o desconto de 5% 
            if (valorMaior5000Quantiade300)
            {
                sbValor.AppendFormat("Valor total: {0} - {1} = ", valorTotal10P.ToString("C"), ITabelaDesconto<Pedido>.Acima5000Reais.ToString("P"));
                sbValor.AppendFormat("{0} \n", valorTotal5P.ToString("C"));
                Console.WriteLine(sbValor.ToString());
            }

            

        }

        public void Relatorios()
        {
            string escolhaRelatorio = string.Empty;
            ReportCompras reportCompras = new ReportCompras();

            Console.WriteLine("------------------------------");
            Console.WriteLine("Relatórios");
            Console.WriteLine("------------------------------");

            Console.WriteLine("1 - Histórico de compras ordenado por quantidade, sem valor\n" +
                              "2 - Histórico de compras ordenado por data, com valor\n" +
                              "3 - Valor médio gasto nas compras\n" +
                              "4 - Maior compra\n");

            Console.Write("Digite o número referente ao relatório: ");
            escolhaRelatorio = Console.ReadLine();

            switch (escolhaRelatorio)
            {
                case "1":
                    Console.Clear();
                    reportCompras.HistoricoPedido(Pedidos, Produtos);
                    break;
                case "2":
                    Console.Clear();
                    reportCompras.ComprasData(PedidoFinalizado, Produtos);
                    break;
                case "3":
                    Console.Clear();
                    reportCompras.ValorMedioCompras(PedidoFinalizado, Produtos);
                    break;
                case "4":
                    Console.Clear();
                    reportCompras.MaiorPedido(PedidoFinalizado, Produtos);
                    break;
                default:
                    Console.WriteLine("\nSelecione um relatório válido \n");
                    break;
            }

        }


    }
}


