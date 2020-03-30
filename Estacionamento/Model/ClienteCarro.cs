using System;
using System.Collections.Generic;
using System.Text;

namespace EstacionamentoGradual.Model
{
    class ClienteCarro 
    {
        public int clienteCarroId;
        public string placa;

        //public static List<Faturamento> Faturamentos;
        public ClienteCarro(int clienteCarroId, string placa)
        {           
            
            this.clienteCarroId = clienteCarroId;
            this.placa = placa;                      
            
        }



        
    }
}
