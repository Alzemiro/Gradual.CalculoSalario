using System;
using System.Collections.Generic;
using System.Text;

namespace EstacionamentoGradual.Model
{
    class Faturamento
    {
        public int clienteCarroId;
        public string dataRegistro;
        public string horaEntrada;
        public string horaSaida;
        public double valorCobrado;

        public Faturamento(int clienteCarroId,
                           string dataRegistro, 
                           string horaEntrada, 
                           string horaSaida, 
                           double valorCobrado)
        {
            this.clienteCarroId = clienteCarroId;
            this.dataRegistro = dataRegistro;
            this.horaEntrada = horaEntrada;
            this.horaSaida = horaSaida;
            this.valorCobrado = valorCobrado;
        }
    }

    
}
