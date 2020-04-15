using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


//Fiz a abordagem utilizando Classes e List<T> separadas para ter maior poder de reutilização de código.

namespace Gradual.RevendaAcos
{
    public class Pedido : TabelaDesconto
    {
        public int ProdutoId;
        public decimal Quantidade;


        public Pedido()
        { }

        //Foi necessária a sobrecarga do construtor Pedido, pois na linha 80 não é possivel a conversão de um
        //tipo Pedido para um tipo Produto, sendo necessário utilizar o Pedido.add(novoPedido), 
        //onde o mesmo recebe informações da Classe Produto.
        //Em resumo, um Pedido somente é efetuado caso o Id do produto exista na Classe Produto


        public Pedido(int produtoId, decimal quantidade)
        {
            this.ProdutoId = produtoId;
            this.Quantidade = quantidade;

        }

        private List<Pedido> Pedidos = new List<Pedido>();

        private List<Produto> Produtos = new List<Produto> {
            new Produto { Id = 1, Descricao = "Alumínio", ValorKg = 9.80m },
            new Produto { Id = 2, Descricao = "Carbono",   ValorKg = 12.50m }

         };

        public void MenuRealizarPedido()
        {

            string sair = string.Empty;


            while (sair != "0")
            {
                Console.WriteLine("------------------------------\n" +
                                  "Revenda de Aços Gradual\n" +
                                  "------------------------------\n");
                Console.WriteLine("Qual destes produtos deseja comprar?");
                RealizarPedido();
                Console.Write("Continuar comprando (1 - Sim / 0 - Não) ? ");
                sair = Console.ReadLine();

            }
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

                    Pedido novoPedido = new Pedido(p.Id, decimal.Parse(entradaQuantidade, CultureInfo.InvariantCulture));
                    Pedidos.Add(novoPedido);
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
            decimal TotalValorPedido = 0m;
            decimal TotalKgPedido = 0m;
            decimal TotalValor = 0m;
            decimal TotalKg = 0m;

            //Tenho que percorrer a lista de produtos para fazer uma nova verificação, onde se existir um pedido
            //com determinado produto deve-se seguir os calculos
            foreach (var p in Produtos)
            {
                //Somente exibirá em tela e realizar calculos nos produtos que foram pedidos
                if (Pedidos.Exists(x => x.ProdutoId.Equals(p.Id)))
                {

                    //Soma a quantidade de Kg solicitados pelo cliente, se o Id do produto for igual ao ProdutoId que está no pedido
                    TotalKgPedido = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade);

                    //Multiplica a quantidade de Kg pelo valor, se o Id do produto for igual ao ProdutoId que está no pedido
                    TotalValorPedido = Pedidos.Where(x => x.ProdutoId.Equals(p.Id)).Sum(s => s.Quantidade * p.ValorKg);

                    //Soma KG
                    TotalKg += TotalKgPedido;

                    //Soma Total
                    TotalValor += TotalValorPedido;

                    Console.WriteLine("--------------------------");
                    //Verifica a quantidade de pedidos para cada Item
                    Console.WriteLine("Total de pedidos de {0}: {1}", p.Descricao, Pedidos.Count(r => r.ProdutoId.Equals(p.Id)));
                    //Exibe o valor em R$ do Kg
                    Console.WriteLine("Valor unitário do {0}: {1}", p.Descricao, p.ValorKg.ToString("C"));
                    Console.WriteLine("Quantidade total em Kg solicitada: {0}", TotalKgPedido);
                    Console.WriteLine("Valor total por pedido: {0}", TotalValorPedido.ToString("C"));
                    Console.WriteLine("--------------------------");
                }

            }

            Console.WriteLine("Valor total da Compra: {0}", TotalValor.ToString("C"));
            Console.WriteLine("Total de Kg: {0}", TotalKg.ToString());
            Console.WriteLine("--------------------------");
            Console.WriteLine("Descontos");
            Console.WriteLine("--------------------------");
            AplicaDesconto(TotalValor, TotalKg);


        }

        public override void AplicaDesconto(decimal valor, decimal quantidade)
        {
            //A melhor forma que encontrei para mostrar os descontos foi instanciar 2 StringBuilders
            StringBuilder sbValor = new StringBuilder();
            StringBuilder sbKg = new StringBuilder();
            
            var valorMenor5000 = false;
            decimal novoValor = 0M;
            decimal novoValorTotal = 0M;

            foreach (var produto in Produtos)
            {
                
                if (Pedidos.Exists(p => p.ProdutoId.Equals(produto.Id)))
                {
                    var pedido = Pedidos.Find(x => x.ProdutoId.Equals(produto.Id));
                    //Utilizei a quantidade total para aplicar o desconto de 10% a todos os produtos
                    if (quantidade > 50)
                    {
                        sbKg.AppendFormat("{0}: {1} - {2}", produto.Descricao, produto.ValorKg.ToString("C"), (Acima50Kg).ToString("P"));
                        //Aplicação de 10% em cada produto
                        produto.ValorKg = produto.ValorKg - (produto.ValorKg * Acima50Kg);
                        //Recalculo do Valor total
                        novoValor = Pedidos.Where(x => x.ProdutoId.Equals(produto.Id)).Sum(s => s.Quantidade * produto.ValorKg);
                        novoValorTotal += novoValor;
                        sbKg.AppendFormat(" = {0}, Valor com Desconto: {1} \n", produto.ValorKg.ToString("C"), novoValor.ToString("C"));

                        DescontoAplicado = true;
                    }
                    //Inicio do recalculo do valor total, onde levo em consideração os novos valores e sinalizo para o algoritimo
                    //que o valor total é menos que 5000
                    if (novoValorTotal < 5000 && DescontoAplicado)
                    {                        
                        valorMenor5000 = true;
                    }
                }
            }

            Console.WriteLine(sbKg.ToString());

            //Fim do recalculo
            if (valorMenor5000)
            {               
                sbValor.AppendFormat("Valor total com desconto: {0}  ", novoValorTotal.ToString("C"));
                Console.WriteLine(sbValor.ToString());
            }
            

            //Ultima validação aplicando o desconto de 5% 
            if (novoValorTotal > 5000)
            {
                sbValor.AppendFormat("Valor total: {0} - {1} = ", novoValorTotal.ToString("C"), Acima5000Reais.ToString("P"));                
                novoValorTotal = novoValorTotal - (novoValorTotal * Acima5000Reais);
                sbValor.AppendFormat("{0} \n", novoValorTotal.ToString("C"));
                Console.WriteLine(sbValor.ToString());
            }


        }

    }
}


