using System;
using System.Collections.Generic;
using System.Text;

namespace Gradual.RevendaAcos
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal ValorKg { get; set; }
        public int ProdutoId { get; internal set; }
        public decimal Quantidade { get; internal set; }

        public Produto()
        {
            this.Id = 0;
            this.Descricao = string.Empty;
            this.ValorKg = 0;             
        
        }        

       
    }
}
