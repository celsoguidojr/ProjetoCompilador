﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    public class AnalisadorSemantico
    {
        public Stack<Simbolo> _pilhaSemantica = new Stack<Simbolo>();
        private static string _caminhoNome = ".\\obj.txt";
        int count = 0;

        public AnalisadorSemantico()
        {
            CriaArquivoObjeto();
        }

        public void AssociaRegraSemantica(int numProducao, List<Simbolo> tabelaDeSimbolos)
        {
            Simbolo s = new Simbolo();
            Simbolo simboloNaoterminal = new Simbolo();
            StreamWriter x = File.AppendText(_caminhoNome);
            Simbolo tipo;
            Simbolo arg;
            Simbolo oprd;
            Simbolo ld;
            switch (numProducao)
            {
                case 5:
                    int i = 0;
                    while (i++ < 3)
                        x.WriteLine("");
                    break;
                case 6:
                    _pilhaSemantica.Pop();
                    s.Tipo = _pilhaSemantica.Pop().Tipo;
                    Simbolo simboloTopo = _pilhaSemantica.Peek();
                    simboloTopo.Tipo = s.Tipo;
                    x.WriteLine($"{simboloTopo.Tipo} {simboloTopo.Lexema};");
                    break;
                case 7:
                    s = _pilhaSemantica.Pop();
                    tipo = new Simbolo("TIPO", "TIPO", s.Tipo);
                    _pilhaSemantica.Push(tipo);
                    break;
                case 8:
                    s = _pilhaSemantica.Pop();
                    tipo = new Simbolo("TIPO", "TIPO", s.Tipo);
                    _pilhaSemantica.Push(tipo);
                    break;
                case 9:
                    s = _pilhaSemantica.Pop();
                    tipo = new Simbolo("TIPO", "TIPO", s.Tipo);
                    _pilhaSemantica.Push(tipo);
                    break;
                case 11:
                    _pilhaSemantica.Pop();
                    s = _pilhaSemantica.Peek();
                    if (s.Tipo != null)
                    {
                        switch (s.Tipo)
                        {
                            case "literal":
                                x.WriteLine($"scanf(\"%s\",{s.Lexema});");
                                break;
                            case "int":
                                x.WriteLine($"scanf(\"%d\",&{s.Lexema});");
                                break;
                            case "double":
                                x.WriteLine($"scanf(\"%lf\",&{s.Lexema});");
                                break;
                        }
                    }
                    else
                        Console.WriteLine("ERRO: Variável não declarada.");
                    break;
                case 12:
                    _pilhaSemantica.Pop();
                    arg = _pilhaSemantica.Peek();
                    x.WriteLine($"printf({arg.Lexema});");
                    break;
                case 13:
                    s = _pilhaSemantica.Pop();
                    arg = s.CopiaAtributos();
                    _pilhaSemantica.Push(arg);
                    break;
                case 14:
                    s = _pilhaSemantica.Pop();
                    arg = s.CopiaAtributos();
                    _pilhaSemantica.Push(arg);
                    break;
                case 15:
                    s = _pilhaSemantica.Peek();
                    if (s.Tipo != null)
                    {
                        arg = s.CopiaAtributos();
                        _pilhaSemantica.Push(arg);
                    }
                    else
                        Console.WriteLine("ERRO: Variável não declarada.");
                    break;
                case 17:
                    _pilhaSemantica.Pop();
                    Simbolo num = _pilhaSemantica.Pop();
                    Simbolo rcb = _pilhaSemantica.Pop();
                    Simbolo id = _pilhaSemantica.Pop();
                    if (id.Tipo != null)
                    {
                        if (num.Tipo == id.Tipo)
                            x.WriteLine($"{id.Lexema} {rcb.Tipo} {num.Lexema};");
                        else
                            Console.WriteLine("ERRO: Tipos diferentes para atribuição.");
                    }
                    else
                        Console.WriteLine("ERRO: Variável não declarada.");
                    break;
                case 18:
                    Simbolo oprd1 = _pilhaSemantica.Pop();
                    Simbolo opm = _pilhaSemantica.Pop();
                    Simbolo oprd2 = _pilhaSemantica.Pop();
                    if (oprd1.Tipo == oprd2.Tipo)
                    {
                        Simbolo LD = new Simbolo($"T{count++}", "LD", "LD");
                        x.WriteLine($"{LD.Lexema} = {oprd2.Lexema} {opm.Tipo} {oprd1.Lexema};");
                    }
                    else
                        Console.WriteLine("ERRO: Operandos com tipos incompatíveis.");
                    break;
                case 19:
                    s = _pilhaSemantica.Pop();
                    ld = s.CopiaAtributos();
                    _pilhaSemantica.Push(ld);
                    break;
                case 20:
                    s = _pilhaSemantica.Peek();
                    if (s.Tipo != null)
                    {
                        s = _pilhaSemantica.Pop();
                        oprd = s.CopiaAtributos();
                        _pilhaSemantica.Push(oprd);
                    }
                    else
                        Console.WriteLine("ERRO: Variável não declarada.");
                    break;
                case 21:
                    s = _pilhaSemantica.Pop();
                    oprd = s.CopiaAtributos();
                    _pilhaSemantica.Push(oprd);
                    break;
                case 23:
                    x.WriteLine("}");
                    break;
                case 24:
                    _pilhaSemantica.Pop();
                    _pilhaSemantica.Pop();
                    s = _pilhaSemantica.Pop();
                    x.WriteLine($"if({s.Lexema}){{");
                    break;
                case 25:
                    Simbolo oprnd1 = _pilhaSemantica.Pop();
                    Simbolo opr = _pilhaSemantica.Pop();
                    Simbolo oprnd2 = _pilhaSemantica.Pop();
                    if (oprnd1.Tipo == oprnd2.Tipo)
                    {
                        Simbolo exp_r = new Simbolo($"T{count++}", "tx", "tx");
                        x.WriteLine($"{exp_r.Lexema} = {oprnd2.Lexema} {opr.Tipo} {oprnd1.Lexema};");
                        _pilhaSemantica.Push(exp_r);
                    }
                    else
                        Console.WriteLine("ERRO: Operandos com tipos incompatíveis.");
                    break;
            }

            x.Close();
        }

        private Simbolo CriaSimboloNaoTerminal(string tipo)
        {
            Simbolo simboloNaoterminal = new Simbolo();
            simboloNaoterminal.Tipo = tipo;

            return simboloNaoterminal;
        }

        private static void CriaArquivoObjeto()
        {
            if (File.Exists(_caminhoNome))
            {
                File.Delete(_caminhoNome);
            }
        }
    }


}
