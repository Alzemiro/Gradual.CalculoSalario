﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gradual.RevendaAcos
{
    public interface ITabelaDesconto<T>
    {
        public const decimal Acima50Kg = 0.1M;
        public const decimal Acima5000Reais = 0.05M;              

        public void AplicaDesconto(List<T> lista);
    }
}
