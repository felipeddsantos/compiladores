/*

Compiladores - Compilador
Felipe Daniel Dias dos Santos - 11711ECP004
Graduação em Engenharia de Computação - Faculdade de Engenharia Elétrica - Universidade Federal de Uberlândia

*/

﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Compilador{

    class Program{

        static int count_id = 0, flag_id = 0, estado = 1;
        
        static string buffer = "";
        
        static List<string> tabela_id = new List<string>();
        
        static string[] token = new string[] { "", "" };
        
        static string[] tabela_simbolos = new string[] {"op", "add", "sub", "mul", "div", ";", "dl", "(", ")", "relop",
                                                        "eq", "cmd", "=", "ge", "gt", "lt", "le", "ne", "id", "numero"};

        static int[] tabela_transicao = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                   0, 2, 3, 2, 4, 5, 6, 7, 8, 9, 10, 13, 12, 11, 0, 1,
                                                   0, 2, 2, 2, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
                                                   0, 22, 3, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10,
                                                   0, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 14, 15, 15,
                                                   0, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 16, 17, 17,
                                                   0, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 20, 19, 18, 18,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 17,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21,
                                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22};

        static int[,] tabela_analise_sintatica = new int[8, 15] {{1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                                             { 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                                             { 0, 0, 4, 4, 4, 0, 0, 4, 0, 3, 0, 0, 0, 0, 0},
                                                             { 0, 0, 8, 6, 7, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0},
                                                             { 0, 0, 0, 0, 0, 0, 0, 9, 9, 0, 0, 0, 0, 0, 0},
                                                             { 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0},
                                                             { 0, 0, 0, 0, 0, 12, 11, 0, 0, 0, 12, 0, 12, 0, 0},
                                                             { 0, 0, 0, 0, 0, 0, 0, 13, 14, 0, 0, 0, 0, 0, 0}};

        static string[] terminais = new string[] {"programa", "inicio", "fim", "se", "enquanto", "relop", "op", "id", "numero", "int",
                                                  ";", "(", ")", "$", "=", "VAZIO"};

        static string[] variaveis = new string[] {"principal", "bloco", "declaracao", "comando", "condicao", "expressao", "expressao'",
                                                  "termo"};

        static List<string> simbolos = new List<string>();

        public static int Acao(int v){

            if(v == 1){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("bloco");
                simbolos.Add("programa");

                return 0;
            }

            if(v == 2){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("fim");
                simbolos.Add("comando");
                simbolos.Add("declaracao");
                simbolos.Add("inicio");

                return 0;
            }

            if(v == 3){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("declaracao");
                simbolos.Add(";");
                simbolos.Add("id");
                simbolos.Add("int");

                return 0;
            }

            if(v == 4){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("VAZIO");

                return 0;
            }

            if(v == 5){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("comando");
                simbolos.Add(";");
                simbolos.Add("expressao");
                simbolos.Add("=");
                simbolos.Add("id");

                return 0;
            }

            if(v == 6){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("comando");
                simbolos.Add("bloco");
                simbolos.Add(")");
                simbolos.Add("condicao");
                simbolos.Add("(");
                simbolos.Add("se");

                return 0;
            }

            if(v == 7){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("comando");
                simbolos.Add("bloco");
                simbolos.Add(")");
                simbolos.Add("condicao");
                simbolos.Add("(");
                simbolos.Add("enquanto");

                return 0;
            }

            if(v == 8){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("VAZIO");

                return 0;
            }

            if(v == 9){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("expressao");
                simbolos.Add("relop");
                simbolos.Add("expressao");

                return 0;
            }

            if(v == 10){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("expressao'");
                simbolos.Add("termo");

                return 0;
            }

            if(v == 11){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("expressao");
                simbolos.Add("op");

                return 0;
            }

            if(v == 12){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("VAZIO");

                return 0;
            }

            if(v == 13){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("id");

                return 0;
            }

            if(v == 14){

                simbolos.RemoveAt(simbolos.Count - 1);
                simbolos.Add("numero");

                return 0;
            }

            else

                return -1;
        }

            public static int Transicao(char c){

            if(c == '_')

                return 1;

            else if(char.IsDigit(c))

                return 2;

            else if(char.IsLetter(c))

                return 3;

            else if(c == '+')

                return 4;

            else if(c == '-')

                return 5;

            else if(c == '*')

                return 6;

            else if(c == '/')

                return 7;

            else if(c == ';')

                return 8;

            else if(c == '(')

                return 9;

            else if(c == ')')

                return 10;

            else if(c == '<')

                return 11;

            else if(c == '>')

                return 12;

            else if(c == '=')

                return 13;

            else if(c == ' ' || c == '\r' || c == '\t' || c == '\n')

                return 15;

            else

                return 14;
        }
        
        public static int AnalisadorLexico(char c){

            if(c == '$')

                return 0;

            estado = tabela_transicao[Transicao(c) + 16 * estado];

            if(estado == 0)

                return 1;

            if(tabela_transicao[16 * estado] == 1){

                if(estado == 4){

                    token[0] = tabela_simbolos[0];
                    token[1] = tabela_simbolos[1];
                }

                else if(estado == 5){

                    token[0] = tabela_simbolos[0];
                    token[1] = tabela_simbolos[2];
                }

                else if(estado == 6){

                    token[0] = tabela_simbolos[0];
                    token[1] = tabela_simbolos[3];
                }

                else if(estado == 7){

                    token[0] = tabela_simbolos[0];
                    token[1] = tabela_simbolos[4];
                }

                else if(estado == 8){

                    token[0] = tabela_simbolos[5];
                    token[1] = "";
                }

                else if(estado == 9){

                    token[0] = tabela_simbolos[7];
                    token[1] = tabela_simbolos[6];
                }

                else if(estado == 10){

                    token[0] = tabela_simbolos[8];
                    token[1] = tabela_simbolos[6];
                }

                else if(estado == 14){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[10];
                }

                else if(estado == 15){

                    token[0] = tabela_simbolos[12];
                    token[1] = tabela_simbolos[11];
                }

                else if(estado == 16){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[13];
                }

                else if(estado == 17){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[14];
                }

                else if(estado == 18){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[15];
                }

                else if(estado == 19){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[16];
                }

                else if(estado == 20){

                    token[0] = tabela_simbolos[9];
                    token[1] = tabela_simbolos[17];
                }

                else if(estado == 21){

                    for(int i = 0; i < tabela_id.Count; i++){

                        if(buffer == tabela_id[i]){

                            if(i <= 5){

                                token[0] = tabela_id[i];
                                token[1] = "";
                            }

                            else{

                                token[0] = tabela_simbolos[18];
                                token[1] = (i - 5).ToString();
                            }

                            flag_id = 1;
                        }
                    }

                    if(flag_id == 0){

                        count_id++;

                        token[0] = tabela_simbolos[18];
                        token[1] = count_id.ToString();

                        tabela_id.Add(buffer);
                    }

                    flag_id = 0;
                }

                else if(estado == 22){

                    token[0] = tabela_simbolos[19];
                    token[1] = buffer;
                }

                return 0;
            }

            if(c != ' ' && c != '\r' && c != '\t' && c != '\n')

                buffer = buffer + c;

            return 0;
        }

        static int Main(string[] args){

            int flag_token = 0; 
            int flag_vazio = 0;
            int flag_terminal = 0;

            char c = ' ';

            tabela_id.Add("programa");
            tabela_id.Add("inicio");
            tabela_id.Add("fim");
            tabela_id.Add("int");
            tabela_id.Add("se");
            tabela_id.Add("enquanto");

            simbolos.Add("principal");

            try{

                FileStream codigo_fonte = File.OpenRead(@"codigo_fonte.txt");

                using(codigo_fonte){

                    while((codigo_fonte.CanRead && codigo_fonte.Position < codigo_fonte.Length) || simbolos.Count > 0){

                        flag_terminal = 0;

                        if(flag_vazio == 0){

                            while(tabela_transicao[16 * estado] == 0){
                                
                                c = (char)codigo_fonte.ReadByte();

                                if(flag_token == 1)

                                    c = '$';

                                if(codigo_fonte.Position == codigo_fonte.Length)

                                    flag_token++;

                                flag_vazio = 0;

                                if(AnalisadorLexico(c) == 1){

                                    Console.WriteLine("Cadeia rejeitada");

                                    return 1;
                                }

                                if(flag_token == 2 && estado == 1)

                                    break;
                            }

                            Console.WriteLine("\nToken reconhecido: " + "<" + token[0] + "," + token[1] + ">");
                        }

                        if(simbolos.Count == 0 && c != '$'){

                            Console.WriteLine("Cadeia rejeitada");

                            return 1;
                        }

                       Console.WriteLine("\nPILHA");

                       for(int index = 0; index < simbolos.Count; index++)
                            
                         Console.WriteLine(simbolos[index]);
                           
                       Console.WriteLine("\nSimbolo de entrada: " + token[0]);
                           
                        if((estado == 15 || estado == 17 || estado == 18 || estado == 21 || estado == 22) && codigo_fonte.Position < codigo_fonte.Length)

                            codigo_fonte.Position--;

                        for(int index = 0; index <= 15; index++){

                            if(terminais[index] == simbolos[simbolos.Count - 1]){

                                flag_terminal = 1;

                                flag_vazio = 0;

                                if(simbolos[simbolos.Count - 1] == token[0])

                                    simbolos.RemoveAt(simbolos.Count - 1);

                                else if(simbolos[simbolos.Count - 1] == "VAZIO"){

                                    simbolos.RemoveAt(simbolos.Count - 1);

                                    flag_vazio = 1;
                                }

                                else{

                                    Console.WriteLine("Cadeia Rejeitada");

                                    return 1;
                                }

                                break;
                            }
                        }
                        
                            if(flag_terminal == 0){

                                int i_variavel = 0;
                                int i_terminal = 0;
                                int indice = 0;

                                flag_vazio = 1;
                                flag_terminal = 0;

                                for(i_terminal = 0; i_terminal <= 14; i_terminal++){

                                    if(terminais[i_terminal] == token[0])

                                        break;
                                }

                                for(i_variavel = 0; i_variavel <= 7; i_variavel++){

                                    if(variaveis[i_variavel] == simbolos[simbolos.Count - 1])

                                        break;
                                }

                            indice = tabela_analise_sintatica[i_variavel, i_terminal];

                            if(indice == 0){

                                Console.WriteLine("Cadeia Rejeitada");

                                return 1;
                            }

                            else

                                Acao(indice);
                            }
                            
                       buffer = "";
                       estado = 1;
                    }

                    if(simbolos.Count == 0){

                        Console.WriteLine("Cadeia Aceita");

                        return 0;
                    }

                    else{

                        Console.WriteLine("Cadeia Rejeitada");
                        
                        return 1;
                    }
                }
            }

            catch(Exception){

                Console.WriteLine("Erro ao abrir ou ler o arquivo");

                return -1;
            }
        }
    }
}
