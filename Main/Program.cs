using System;
using System.Collections.Generic;
using System.IO; //biblioteca para ler e escrever dados em arquivos.
using AnalisadorLexico;
using AnalisadorSintatico;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var codigoFonte = File.ReadAllText(@"CodigoFonte2.txt");  // Transforma o arquivo todo em string
            codigoFonte = codigoFonte.Replace(' ', '\r');

            AnalisadorSintatico.AnalisadorSintatico analisadorSintatico = new AnalisadorSintatico.AnalisadorSintatico(codigoFonte);

            analisadorSintatico.AnaliseSintatica();
        }
    }
}

