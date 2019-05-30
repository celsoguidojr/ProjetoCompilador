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

            AnalisadorLexico.AnalisadorLexico analisadorLexico = new AnalisadorLexico.AnalisadorLexico(codigoFonte);

            AnalisadorSintatico.AnalisadorSintatico analisadorSintatico = new AnalisadorSintatico.AnalisadorSintatico(codigoFonte);

            analisadorSintatico.AnaliseSintatica();
        }

        private static void EscreveSimboloNaTela(Simbolo s)
        {
            if (s.Token == "ERRO")
            {
                Console.WriteLine($@"Token: {s.Token} Descrição: {s.DescricaoERRO} Linha: {s.Linha} Coluna: {s.Coluna}");
            }
            else
            {
                Console.WriteLine($@"Lexema: { s.Lexema} | Token: { s.Token} | Tipo : { (s.Tipo == null ? "null" : s.Tipo)} ");
            }

        }
    }
}

