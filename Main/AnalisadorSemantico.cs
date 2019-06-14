using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class AnalisadorSemantico
    {

        public void AssociaRegraSemantica(object obj, string[] producao)
        {
            StreamWriter x;

            string CaminhoNome = ".\\obj.txt";

            if (File.Exists(CaminhoNome))
                x = File.AppendText(CaminhoNome);
            else
                x = File.CreateText(CaminhoNome);

            //CASO O TIPO DO OBJETO FOR INTEIRO SIGNIFICA QUE É UMA ESCRITA NO ARQUIVO
            if (obj.GetType() == typeof(int))
            {
                switch (obj)
                {
                    case 5:
                        int i = 0;
                        while (i++ < 3)
                            x.WriteLine("");
                        break;
                    
                }
            }
            switch(producao[0])
            {
                case "TIPO":

                    break;
            }

            x.Close();
        }

        private Simbolo CriaSimboloNaoTerminal(string naoTerminal)
        {
            var s = new Simbolo();
            s.Token = naoTerminal;

            return s;
        }
    }


}
