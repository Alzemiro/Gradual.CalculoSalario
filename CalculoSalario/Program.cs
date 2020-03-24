using System;
using System.Text.RegularExpressions;

namespace CalculoSalario
{
    class Program
    {
        static void Main(string[] args)
        {
            CalculaSalario calcs = new CalculaSalario();

            calcs.calculaSalario();

        }
    }

    class CalculaSalario
    {
        public void calculaSalario()
        {
            decimal valorHora;
            decimal diasTrabalhados;
            decimal horasTrabalhadas = 8M;
            decimal dependentes;
            decimal salarioBruto;

            string entrValorHora;
            string entrDiasTrabalhados;
            string entrDependentes;
            string opcaoSair;

            opcaoSair = "";
            entrValorHora = "";
            entrDiasTrabalhados = "";
            entrDependentes = "";



            while (opcaoSair != "0")
            {
                //Entrada usuario
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Bem vindo a caladora de salário Gradual");
                Console.WriteLine("----------------------------------------");

                Console.WriteLine("Digite o valor da hora trabalhada: ");
                entrValorHora = validaEntrada(entrValorHora);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Digite a quantidade de dias trabalhados: ");
                entrDiasTrabalhados = validaEntrada(entrDiasTrabalhados);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Digite o numero de dependentes: ");
                entrDependentes = validaEntrada(entrDependentes);
                Console.WriteLine("----------------------------------------");

                //conversões

                valorHora = Convert.ToDecimal(entrValorHora);
                dependentes = Convert.ToDecimal(entrDependentes);
                diasTrabalhados = Convert.ToDecimal(entrDiasTrabalhados);                

                //Calculo Salario Bruto
                salarioBruto = valorHora * diasTrabalhados * horasTrabalhadas;

                //Resultado
                Console.WriteLine("Salário Bruto: {0}", salarioBruto);
                Console.WriteLine("Desconto IRPF: {0}", IRPF(salarioBruto, dependentes));
                Console.WriteLine("Desconto INSS: {0}", INSS(salarioBruto));
                Console.WriteLine("Desconto total: {0}", impostoTotal());
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Salario Final: {0}", salarioFinal());
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("digite 1 para continuar ou 0 para sair: ");
                opcaoSair = Console.ReadLine();
            }



            decimal impostoTotal()
            {
                decimal desconto = IRPF(salarioBruto, dependentes) + INSS(salarioBruto);
                return desconto;
            }

            decimal salarioFinal()
            {
                decimal salarioFinal = salarioBruto - impostoTotal();
                return salarioFinal;
            }

        }


        public decimal IRPF(decimal salarioBruto, decimal dependentes)
        {
            decimal[] aliquotas = new decimal[4] { 0.075M, 0.15M, 0.225M, 0.275M };
            decimal[] faixaSalarial = new decimal[4] { 1903.99M, 2826.66M, 3751.06M, 4664.69M };
            decimal[] deducoes = new decimal[4] { 142.80M, 354.80M, 636.13M, 869.36M };
            decimal descontoIRPF = 0M;
            decimal vlrDependente = dependentes * 189.59M;

            //Calculo Imposto de Renda retido na fonte
            decimal SalarioParaRecolhimento;

            SalarioParaRecolhimento = salarioBruto - vlrDependente - INSS(salarioBruto);

            //se o salario for menor 1903.99
            if (SalarioParaRecolhimento < faixaSalarial[0])
            {
                descontoIRPF = SalarioParaRecolhimento * 0M;


            }
            //se o salario for maior ou igual que 1903.99 e menor que 2826.66M aplica 0,75
            else if (SalarioParaRecolhimento >= faixaSalarial[0] && SalarioParaRecolhimento < faixaSalarial[1])
            {
                descontoIRPF = SalarioParaRecolhimento * aliquotas[0] - deducoes[0];


            }
            //se o salario for maior ou igual que 2826.66 e menor que 3751.06 aplica 0.15
            else if (SalarioParaRecolhimento >= faixaSalarial[1] && SalarioParaRecolhimento < faixaSalarial[2])
            {
                descontoIRPF = SalarioParaRecolhimento * aliquotas[1] - deducoes[1];


            }
            //se o salario for maior ou igual que 3751.06 e menor que 4664.69 aplica 0.225
            else if (SalarioParaRecolhimento >= faixaSalarial[2] && SalarioParaRecolhimento < faixaSalarial[3])
            {
                descontoIRPF = SalarioParaRecolhimento * aliquotas[2] - deducoes[2];


            }
            //se o salario for maior que 4664.69 aplica 0.275
            else if (SalarioParaRecolhimento > faixaSalarial[3])
            {
                descontoIRPF = SalarioParaRecolhimento * aliquotas[3] - deducoes[3];

            }



            return Math.Round(descontoIRPF, 2);
        }


        public decimal INSS(decimal salarioBruto)
        {
            decimal[] faixaSalarial = new decimal[3] { 1830.30M, 3050.53M, 6101.07M };
            decimal[] aliquotas = new decimal[3] { 0.08M, 0.09M, 0.11M };

            decimal descontoINSS = 0;


            if (salarioBruto < faixaSalarial[0])
            {
                descontoINSS = salarioBruto * aliquotas[0];

            }
            else if (salarioBruto > faixaSalarial[0] && salarioBruto < faixaSalarial[1])
            {
                descontoINSS = salarioBruto * aliquotas[1];
            }
            else
            {
                descontoINSS = salarioBruto * aliquotas[2];
            }

            return Math.Round(descontoINSS, 2);
        }

        public string validaEntrada(string entrada)        {
            
            
            Regex rgx = new Regex("\\d");
            entrada = Console.ReadLine();

            if (rgx.IsMatch(entrada))
            {
                entrada.Replace('.', ',');                                
            }
            else
            {
                Console.WriteLine("Escreva apenas numeros, digite novamente o valor: ");
                validaEntrada(entrada);
            }
            return entrada;
        }

    }




}


