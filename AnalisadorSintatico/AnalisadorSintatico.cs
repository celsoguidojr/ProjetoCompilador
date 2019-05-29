using AnalisadorLexico;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisadorSintatico
{
    public class AnalisadorSintatico
    {
        string _codigoFonte;
        public AnalisadorSintatico(string codigoFonte)          // Instância o analisador léxico tendo como parametro o código fonte.
        {
            this._codigoFonte = codigoFonte;             // Acessa o codigo fonte 
        }

        public Stack<int> _pilha = new Stack<int>();
        private Dictionary<int, string[]> _producoes = Producoes();
        private Dictionary<string, string[]> _erros = Erros();
        private static DataTable _tabelaShiftReduce = CarregaTabelaShiftReduce();

        public void AnaliseSintatica()
        {
            AnalisadorLexico.AnalisadorLexico analisadorLexico = new AnalisadorLexico.AnalisadorLexico(_codigoFonte);

            _pilha.Push(0);
            int estado = 0;
            bool erro = false;
            Simbolo simbolo = analisadorLexico.RetornaToken();
            int f = 1;
            Stack<Simbolo> pilhaDeSimbolos = new Stack<Simbolo>();
            
            while (true)
            {
                //CASO O PRÓXIMO SÍMBOLO SEJA NULO O ANALISADOR ENTENDE QUE CHEGOU AO FINAL DO ARQUIVO
                if (simbolo == null)
                    simbolo = new Simbolo { Token = "$" };

                string acao = _tabelaShiftReduce.Rows[estado][$"{simbolo.Token}"].ToString();


                if (acao.Contains("S"))
                {
                    estado = Convert.ToInt32(acao.Substring(1));
                    _pilha.Push(estado);

                    if (pilhaDeSimbolos.Count == 0)
                    {
                        simbolo = analisadorLexico.RetornaToken();
                    }
                    else
                    {
                        simbolo = pilhaDeSimbolos.Pop();
                    }
                }
                else
                {
                    if (acao.Contains("R"))
                    {
                        int numProducao = Convert.ToInt32(acao.Substring(1));
                        var producao = new string[2];
                        _producoes.TryGetValue(numProducao, out producao);
                        var Simbolos = producao[1].Split(' ');
                        int numSimbolos = Simbolos.Count();
                        //DESEMPILHA QUANTIDADE DE SIMBOLOS LADO DIREITO
                        for (int i = 0; i < numSimbolos; i++)
                            _pilha.Pop();          

                        //EMPILHAR O VALOR DE [t,A] na pilha
                        estado = Convert.ToInt32(_tabelaShiftReduce.Rows[Convert.ToInt32(_pilha.Peek())][producao[0].ToString()]);
                        _pilha.Push(estado);
                        Console.WriteLine($"{f} - {producao[0]} -> {producao[1]}");
                        f++;
                    }
                    else
                    {
                        if (acao.Contains("A"))
                        {
                            Console.WriteLine("CADEIA ACEITA!");
                            Console.ReadLine();
                            break;
                        }
                        else
                        {
                            Simbolo s  = CopiaSimbolo(simbolo);
                            pilhaDeSimbolos.Push(s);
                            simbolo.Token = Erro(acao)[1].ToString(); //Rotina de erro
                        }
                    }
                }

            }

        }

        private string[] Erro(string acao)
        {
            string[] erro;
            _erros.TryGetValue(acao, out erro);
            Console.WriteLine(erro[0] + "\n");

            return erro;
        }

        private static Dictionary<string, string[]> Erros()
        {
            Dictionary<string, string[]> erros = new Dictionary<string, string[]>();

            erros.Add("E1", new string[] { "ESPERA-SE INSERÇÃO -> 'inicio'", "inicio" });
            erros.Add("E2", new string[] { "ESPERA-SE INSERÇÃO -> 'varinicio'", "varinicio" });
            erros.Add("E3", new string[] { "ESPERA - SE INSERÇÃO-> 'id'", "id" });


            //erros.Add("E17", new string[] { "ESPERA-SE INSERÇÃO -> 'leia' ou 'escreva' ou 'id' ou 'se' ou 'fim'", "leia" });

            // erros.Add("E5", new string[] { "ESPERA-SE INSERÇÃO -> 'literal' ou 'num' ou 'id'", "id" });
            erros.Add("E6", new string[] { "ESPERA-SE INSERÇÃO -> '<-' (atribuição)", "rcb" });
           // erros.Add("E7", "ESPERA-SE INSERÇÃO -> 'leia' ou 'escreva' ou 'id' ou 'se' ou 'fimse'");
            erros.Add("E8", new string[] { "ESPERA-SE INSERÇÃO -> '(' abre parênteses", "AB_P" });
            erros.Add("E9", new string[] { "ESPERA-SE INSERÇÃO -> ';' ponto e virgula", "PT_V" });
           // erros.Add("E10", "ESPERA-SE INSERÇÃO -> 'id' ou 'num'");
            erros.Add("E11", new string[] { "ESPERA-SE INSERÇÃO -> ')' fecha parênteses", "FC_P" });
            erros.Add("E12", new string[] { "ESPERA-SE INSERÇÃO -> 'entao'", "entao" });
            erros.Add("E13", new string[] { "ESPERA-SE INSERÇÃO OPERADORES RELACIONAIS -> '<=' ou '>=' ou '<' ou '>' ou '=' ou '<>'", "opr" });
           // erros.Add("E14", "ESPERA-SE INSERÇÃO -> 'varfim' ou 'id'");
           // erros.Add("E15", "ESPERA-SE INSERÇÃO -> 'int' ou 'real' ou 'lit'");
            erros.Add("E16", new string[] { "ESPERA-SE INSERÇÃO OPERADORES ARITMÉTICOS -> '+' ou '-' ou '*' ou '/'", "opm" });

            return erros;
        }

        private static Dictionary<int, string[]> Producoes()
        {
            Dictionary<int, string[]> producoes = new Dictionary<int, string[]>();
            producoes.Add(1, new string[] { "P'", "P" });
            producoes.Add(2, new string[] { "P", "inicio V A" });
            producoes.Add(3, new string[] { "V", "varinicio LV" });
            producoes.Add(4, new string[] { "LV", "D LV" });
            producoes.Add(5, new string[] { "LV", "varfim ;" });
            producoes.Add(6, new string[] { "D", "id TIPO ;" });
            producoes.Add(7, new string[] { "TIPO", "inteiro" });
            producoes.Add(8, new string[] { "TIPO", "real" });
            producoes.Add(9, new string[] { "TIPO", "lit" });
            producoes.Add(10, new string[] { "A", "ES A" });
            producoes.Add(11, new string[] { "ES", "leia id ;" });
            producoes.Add(12, new string[] { "ES", "escreva ARG ;" });
            producoes.Add(13, new string[] { "ARG", "literal" });
            producoes.Add(14, new string[] { "ARG", "num" });
            producoes.Add(15, new string[] { "ARG", "id" });
            producoes.Add(16, new string[] { "A", "CMD A" });
            producoes.Add(17, new string[] { "CMD", "id rcb LD ;" });
            producoes.Add(18, new string[] { "LD", "OPRD opm OPRD" });
            producoes.Add(19, new string[] { "LD", "OPRD" });
            producoes.Add(20, new string[] { "OPRD", "id" });
            producoes.Add(21, new string[] { "OPRD", "num" });
            producoes.Add(22, new string[] { "A", "COND A" });
            producoes.Add(23, new string[] { "COND", "CABEÇALHO CORPO" });
            producoes.Add(24, new string[] { "CABEÇALHO", "se ( EXP_R ) entao" });
            producoes.Add(25, new string[] { "EXP_R", "OPRD opr OPRD" });
            producoes.Add(26, new string[] { "CORPO", "ES CORPO" });
            producoes.Add(27, new string[] { "CORPO", "CMD CORPO" });
            producoes.Add(28, new string[] { "CORPO", "COND CORPO" });
            producoes.Add(29, new string[] { "CORPO", "fimse" });
            producoes.Add(30, new string[] { "A", "fim" });

            return producoes;
        }

        public static DataTable CarregaTabelaShiftReduce() //método para ler a tabela de transição, arquivo em excel
        {
            string arquivoExcel = @"TabelaShiftReduce.xlsx";

            DataTable Data_Table = new DataTable();

            //Pega o endereço do arquivo excel para abrir
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + arquivoExcel + "; Extended Properties = 'Excel 8.0;HDR=YES'";
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            cmd.Connection = conn;
            conn.Open();

            DataTable dtSchema;
            dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string nomePlanilha = dtSchema.Rows[0]["TABLE_NAME"].ToString();
            conn.Close();
            //le todos os dados da planilha para o Data Table    
            conn.Open();

            cmd.CommandText = "SELECT * FROM [" + nomePlanilha + "]";

            dataAdapter.SelectCommand = cmd;

            dataAdapter.Fill(Data_Table);
            conn.Close();

            return (Data_Table);
        }

        private Simbolo CopiaSimbolo(Simbolo s)
        {
            Simbolo simbolo = new Simbolo
            {
                Token = s.Token,
                ColunaDoERRO = s.ColunaDoERRO,
                DescricaoERRO = s.DescricaoERRO,
                Lexema = s.Lexema,
                LinhaDoERRO = s.LinhaDoERRO,
                Tipo = s.Tipo
            };

            return simbolo;
        }
    }
}
