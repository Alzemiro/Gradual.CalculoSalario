﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gradual.RevendaAcos
{
    public abstract class TabelaDesconto
    {
        public const decimal Acima50Kg = 0.1M;
        public const decimal Acima5000Reais = 0.05M;
        public bool DescontoAplicado = false;        

        public abstract void AplicaDesconto(decimal valor, decimal quantidade);
    }
}