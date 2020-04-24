using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Gradual.RevendaAcos
{
    public class ReportCompras
    {
        public void HistoricoPedido(List<Pedido> pedidos, List<Produto> produtos)
        {
            if (pedidos.Count >= 1)
            {

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("-------------------------\n" +
                                "Relatório Pedidos  - Maior Quantidade\n" +
                                "-------------------------\n");

                var queryHist = from pedido in pedidos
                                join produto in produtos on pedido.ProdutoId equals produto.Id
                                orderby pedido.Quantidade descending
                                select new
                                {
                                    produtoDescricao = produto.Descricao,
                                    pedidoQuantidade = pedido.Quantidade,

                                };
                foreach (var p in queryHist)
                {
                    sb.AppendFormat("{0}: {1} Kg \n", p.produtoDescricao, p.pedidoQuantidade);
                };

                Console.WriteLine(sb.ToString());
            }
            else
            {

                Console.WriteLine("\nRelatório sem dados\n");
            }
        }


        public void ComprasData(List<Pedido> pedidos, List<Produto> produtos)
        {

            StringBuilder sb = new StringBuilder();
            if (pedidos.Count >= 1)
            {
                sb.AppendFormat("-------------------------\n" +
                            "Relatório Pedidos  - Pedidos por Data\n" +
                            "-------------------------\n");

                var queryHist = from pedido in pedidos
                                join produto in produtos on pedido.ProdutoId equals produto.Id
                                orderby pedido.DataPedido descending
                                select new
                                {
                                    produtoDescricao = produto.Descricao,
                                    pedidoQuantidade = pedido.PedidoKg,
                                    pedidoData = pedido.DataPedido,
                                    valorPedido = pedido.ValorTotalPedido
                                };

                foreach (var p in queryHist)
                {
                    sb.AppendFormat("{0}\n {1}\n {2} Kg\n {3}\n \n", p.pedidoData, p.produtoDescricao, p.pedidoQuantidade, p.valorPedido.ToString("C"));
                }
                Console.WriteLine(sb.ToString());

            }
            else
            {

                Console.WriteLine("\nRelatório sem dados\n");
            }
        }

        public void ValorMedioCompras(List<Pedido> pedidos, List<Produto> produtos)
        {
            
            if (pedidos.Count >= 1)
            {                

                Console.WriteLine("-------------------------\n" +
                                "Relatório Pedidos  - Valor Médio\n" +
                                "-------------------------\n");

                var queryHist = from pedido in pedidos
                                join produto in produtos on pedido.ProdutoId equals produto.Id                                
                                select new
                                {
                                    media = pedido.ValorTotalCompra / pedidos.Count
                                };

                var media = queryHist.Select(s => s.media).FirstOrDefault();       
                

                Console.WriteLine("O valor médio de compras foi: {0}", (media).ToString("C"));
            }
            else
            {
                Console.WriteLine("\nRelatório sem dados\n");
            }

        }

        public void MaiorPedido(List<Pedido> pedidos, List<Produto> produtos) {

            if (pedidos.Count >= 1)
            {
                Console.WriteLine("-------------------------\n" +
                                "Relatório Pedidos  - Valor Médio\n" +
                                "-------------------------\n");

                var queryHist = from pedido in pedidos
                                join produto in produtos on pedido.ProdutoId equals produto.Id
                                select new
                                {
                                    pedido.ValorTotalPedido
                                };
                var maxPedido = queryHist.Select(s => s.ValorTotalPedido).Max();
                Console.WriteLine("O pedidode maior valor: {0}", maxPedido.ToString("C"));
            }
            else
            {
                Console.WriteLine("\nRelatório sem dados\n");
            }
            }
    }
}

