using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR
{
    class Graf
    {
        public int s;
        public int verif;
        public int pozi;
        public List<string> Lista_noduri = new List<string>();
        public string pleaca;//de unde a provenit
        public int nivel { get; set; }
    }

    class Analyze_Table
    {
        public int linie { get; set; }
        public string coloana { get; set; }
        public string value { get; set; }
    }
    class Program
    {

        public static string first_aux_list { get; set; }
        public static List<string> Grammer { get; set; }
        public static List<string> ContextList { get; set; }
        public static List<string> current_terminals { get; set; }
        public static Dictionary<int, string> Dictionary_Grammer { get; set; }
        public static List<Graf> Graf_List { get; set; }
        public static List<Analyze_Table> AnalyzeList { get; set; }
        public static List<List<String>> MatrixTable { get; set; }
        public static Stack<string> stiva { get; set; }
        public static List<string> lista_intrari { get; set; }
        public static List<string> All_Symbols { get; set; }
        public static List<string> actions { get; set; }
        public static List<bool> parcurs { get; set; }
        public static List<bool> parcurs_first { get; set; }

        public static string first(string item, int key = 0, string result = "", bool p_f = false)
        {
            if (p_f == true)
            {
                for (int i = 0; i < parcurs.Count; i++)
                {
                    parcurs_first[i] = false;
                }
            }

          

            var result2 = result.Replace("&", "").Distinct();
            string result3 = string.Empty;
            foreach (var i in result2)
            {
                result3 += "r" + i.ToString();
            }

            result = result3;
            if (result.Equals(""))
            {
                return result;
            }
            else
            {
                return result.Substring(1);
            }

        }

        public static void Group_G(Graf g)
        {
            var par_list = new List<string>();
            for (int i = 0; i < g.Lista_noduri.Count(); i++)
            {
                int ok = 0;
                var curent = g.Lista_noduri[i].Split(',')[0];
                var curent1 = g.Lista_noduri[i].Split(',')[1];
                for (int j = 0; j < par_list.Count; j++)
                    if (par_list[j].Contains(curent))
                    {
                        par_list[j] += "/" + curent1;
                        ok = 1;
                        break;
                    }

                if (ok == 0) par_list.Add(g.Lista_noduri[i]);

            }
            for (int i = 0; i < par_list.Count; i++)
            {
                var u = par_list[i].Split(',')[1].Replace("/", "").Distinct();
                var k = "";
                foreach (var iter in u)
                {
                    k += "/" + iter;
                }
                par_list[i] = par_list[i].Split(',')[0] + "," + k.Substring(1);
            }
            g.Lista_noduri = par_list;
        }


        public static string Move_Dot(String s, int it)
        {
            var index = s.IndexOf(".");
            string somestring = s;
            char[] ch = somestring.ToCharArray();
            var aux = ch[index];
            ch[index] = ch[index + 1];
            ch[index + 1] = aux;
            string newstring = new string(ch);
            return newstring;

        }

        private static void Create_I0(string s, Graf g, int contor = 0, bool reset_parcurs = false)
        {
            if (reset_parcurs == false)
            {
                for (int i = 0; i < parcurs.Count; i++)
                {
                    parcurs[i] = false;
                }
            }

            if (contor == 0)
            {
                //s='S'>.S,$'
                //List<string> generated = new List<string>();
                List<string> firstAfterDotProductions = new List<string>();
                var firstAfterDot = s.Split('.')[1][0].ToString(); //S
                var afterDot = s.Split('.')[1].ToString();
                var afterComma = s.Split(',')[1].ToString();
                var items = Dictionary_Grammer.Where(elem => elem.Value[0].ToString().Equals(firstAfterDot));
                foreach (var item in items)
                {
                    var left = item.Value[0].ToString();
                    var right = item.Value.Split('>')[1];
                    //generated.Add(left + ">." + right + ",");
                    if (firstAfterDot.Equals(right[0].ToString()))
                    {
                        firstAfterDotProductions.Add(right.Substring(1));
                    }
                }
                //string lookahead = first(afterDot.Substring(1).Replace(",",""));
                string lookahead = String.Empty;
                if (!afterDot[1].ToString().Equals(","))
                {
                    lookahead = first(afterDot.Substring(1).Split(',')[0]);
                    if (lookahead.Equals(""))
                    {
                        lookahead = afterComma;
                    }
                }
                else
                {
                    lookahead = afterComma;
                }
                foreach (var i in firstAfterDotProductions)
                {
                    lookahead += '/' + first(i);
                }
                var result2 = lookahead.Replace("/", "").Distinct();
                string result3 = string.Empty;
                foreach (var i in result2)
                {
                    result3 += "/" + i.ToString();
                }

                lookahead = result3.Substring(1);

                foreach (var item in items)
                {

                    var left = item.Value[0].ToString();
                    var right = item.Value.Split('>')[1];
                    g.Lista_noduri.Add(left + ">." + right + ',' + lookahead);
                    if (left.Equals(right[0].ToString()))
                    {
                        var el = left + ">" + right;
                        var poz = Grammer.IndexOf(Grammer.Where(elem => elem.Equals(el)).FirstOrDefault());
                        if (parcurs[poz] == false)
                        {
                            parcurs[poz] = true;
                            Create_I0(left + ">." + right + ',' + lookahead, g, 1, true);
                        }


                    }
                    else
                    {
                        var el = left + ">" + right;
                        var poz = Grammer.IndexOf(Grammer.Where(elem => elem.Equals(el)).FirstOrDefault());
                        if (parcurs[poz] == false)
                        {
                            parcurs[poz] = true;
                            Create_I0(left + ">." + right + ',' + lookahead, g, 0, true);
                        }
                    }

                }
            }
        }

        public static void Create_Table()
        {
            for (int i = 0; i < Graf_List.Count(); i++)
            {
                if (Graf_List[i].pleaca != null && Graf_List[i].pleaca.All(char.IsLower))
                {
                    Analyze_Table a_n = new Analyze_Table();
                    a_n.linie = Graf_List[i].pozi;
                    a_n.coloana = Graf_List[i].pleaca;
                    a_n.value = "s" + i;
                    AnalyzeList.Add(a_n);
                }
                else if (Graf_List[i].pleaca != null && Graf_List[i].pleaca.All(char.IsUpper))
                {
                    Analyze_Table a_n = new Analyze_Table();
                    a_n.linie = Graf_List[i].pozi;
                    a_n.coloana = Graf_List[i].pleaca;
                    a_n.value = i.ToString();
                    AnalyzeList.Add(a_n);
                }
            }
        }

        public static List<string> Get_Terminals()
        {
            var my_non_terminal_list = new List<string>();
            foreach (var item in Grammer)
            {
                var lower = item.Where(char.IsLetter);
                foreach (var item1 in lower)
                {
                    if (my_non_terminal_list.Contains(item1.ToString()) == false)
                    {
                        my_non_terminal_list.Add(item1.ToString());
                    }
                }

            }
            my_non_terminal_list.Add("$");
            return my_non_terminal_list;
        }

        public static void Display_Table()
        {

            var terminale = Get_Terminals();
            Console.Write("    ");
            for (int i = 0; i < terminale.Count; i++)
            {
                Console.Write("   " + terminale[i] + "   ");
            }
            Console.WriteLine();


            for (int i = 0; i < Graf_List.Count; i++)
            {
                var rand = new List<String>();
                for (int t = 0; t < terminale.Count; t++)
                {
                    rand.Add("*");

                }

                for (int t = 0; t < terminale.Count; t++)
                {
                    var simbol = terminale[t];
                    if (simbol.Equals("$") && Graf_List[i].Lista_noduri[0].Equals("S'>S.,$") == true)
                    {
                        rand[terminale.IndexOf(simbol)] = "acc";
                    }
                    else if (char.IsLower(simbol.ToCharArray()[0]))
                    {
                        var item = Graf_List[i].Lista_noduri.Where(elem => elem.Split('.')[1][0].ToString().Equals(simbol.ToString())).FirstOrDefault();
                        if (item != null)
                        {
                            var new_item = Move_Dot(item, 1);
                            var indice = Graf_List.IndexOf(Graf_List.Where(elem => elem.Lista_noduri[0].Equals(new_item)).FirstOrDefault());
                            rand[terminale.IndexOf(simbol)] = "s" + indice;
                        }

                    }
                    else if (char.IsUpper(simbol.ToCharArray()[0]))
                    {
                        var item = Graf_List[i].Lista_noduri.Where(elem => elem.Split('.')[1][0].ToString().Equals(simbol.ToString())).FirstOrDefault();
                        if (item != null)
                        {
                            var new_item = Move_Dot(item, 1);
                            var indice = Graf_List.IndexOf(Graf_List.Where(elem => elem.Lista_noduri[0].Equals(new_item)).FirstOrDefault());
                            rand[terminale.IndexOf(simbol)] = indice.ToString();
                        }

                    }

                    foreach (var item in Graf_List[i].Lista_noduri)
                    {
                        if (item.Split('.')[1][0].Equals(',') && item.Equals("S'>S.,$") == false)
                        {
                            var after = item.Split(',')[1].Split('/');
                            var before = item.Split('.')[0];
                            var key = Dictionary_Grammer.Where(elem => elem.Value.Equals(before.ToString())).FirstOrDefault().Key;
                            foreach (var aft in after)
                            {
                                if (aft.ToString().Equals(simbol.ToString()))
                                {
                                    if (rand[terminale.IndexOf(simbol)].Equals("*"))
                                    {
                                        rand[terminale.IndexOf(simbol)] = "r" + (key + 1);
                                    }
                                    else
                                    {
                                        rand[terminale.IndexOf(simbol)] += ",r" + (key + 1);
                                    }

                                }

                            }

                        }
                    }




                }
                MatrixTable.Add(rand);

            }
            for (int i = 0; i < MatrixTable.Count; i++)
            {
                Console.Write(i + "   ");
                for (int j = 0; j < MatrixTable[i].Count; j++)
                {
                    Console.Write("   " + MatrixTable[i][j] + "   ");
                }
                Console.WriteLine();
            }
        }
        private static bool IS_Lr()
        {
            foreach (var item in MatrixTable)
            {
                foreach (var item1 in item)
                {
                    if (item1.Split(',').Length > 1) return false;
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            Graf g = new Graf();
            Grammer = new List<string>();
            ContextList = new List<string>();
            current_terminals = new List<string>();
            Graf_List = new List<Graf>();
            Dictionary_Grammer = new Dictionary<int, string>();
            AnalyzeList = new List<Analyze_Table>();
            MatrixTable = new List<List<String>>();
            All_Symbols = new List<string>();
            parcurs = new List<bool>();
            lista_intrari = new List<string>();
            parcurs_first = new List<bool>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Asus\source\repos\LR\LR\input.txt");

            foreach (var item in lines)
            {
                var split1 = item.Split('>');
                var split2 = split1[1].Split('|');
                foreach (var item1 in split2)
                {

                    Grammer.Add(split1[0] + ">" + item1);
                    parcurs.Add(false);
                    parcurs_first.Add(false);

                }
            }
            All_Symbols = Get_Terminals();
            var m = Get_Terminals();

            for (int i = 0; i < Grammer.Count(); i++)
            {
                Dictionary_Grammer.Add(i, Grammer[i]);
            }
            g.Lista_noduri.Add("S'>.S,$");
            g.nivel = 0;
            Create_I0("S'>.S,$", g);
           
            Group_G(g);


       
            Graf_List.Add(g);

            int contor_global = 1;
            var copy = Graf_List;
            var current_count = Graf_List.Count();
            var contor_iterator = 0;
            var last = 0;
            while (true)
            {


                for (int i = last; i < current_count; i++)
                {
                    var w = i;
                    var graf = copy[i];
                    Dictionary<string, List<string>> group_dic = new Dictionary<string, List<string>>();
                    foreach (var item in graf.Lista_noduri)
                    {
                        var split = item.Split('.')[1][0].ToString();
                        if (group_dic.ContainsKey(split))
                        {
                            group_dic[split].Add(item);
                        }
                        else
                        {
                            group_dic.Add(split, new List<string>() { item });
                        }
                    }

                    int ok = 0;
                    foreach (var item in group_dic)
                    {
                        var aux_list = new List<Graf>();
                        bool is_one = true;
                        for (int j = 0; j < group_dic[item.Key].Count; j++)
                        {

                            /* if (ok == 0 && group_dic[item.Key].Count > 0)
                             {
                                 ok = 1;
                                 foreach (var k in group_dic[item.Key])
                                 {
                                     g.Lista_noduri.Add(k);
                                 }
                             }*/



                            g = new Graf();
                            var index = item.Value[j].IndexOf('.');
                            var nextChar = item.Value[j][index + 1];
                            if (nextChar.Equals(',') == false)
                            {
                                g.pleaca = item.Value[j].Split('.')[1][0].ToString();
                                string move = Move_Dot(item.Value[j], contor_global);

                                g.Lista_noduri.Add(move);

                                Create_I0(move, g, 0);
                                g.nivel = contor_global;
                                g.pozi = i;
                                var exist = copy.Any(elem => elem.Lista_noduri.ElementAt(0).Equals(move));
                                if (exist == false && item.Value.Count == 1)
                                {

                                    Group_G(g);

                                    Graf_List.Add(g);
                                }
                                else if (exist == false && item.Value.Count > 1)
                                {
                                    is_one = false;
                                    aux_list.Add(g);
                                }

                            }
                            else
                            {
                                //var exist = copy.Any(elem => elem.Lista_noduri.ElementAt(0).Equals(copy[i].Lista_noduri[j]));
                                //if(exist == false)
                                //{
                                //    g.Lista_noduri.Add(copy[i].Lista_noduri[j]);
                                //    Graf_List.Add(g);
                                //}

                            }

                        }
                        if (is_one == false)
                        {
                            foreach (var ii in aux_list)
                            {
                                List<string> copyIi = new List<string>();
                                copyIi.Add(ii.Lista_noduri[0]);
                                ii.Lista_noduri = copyIi;
                            }

                            var gg = new Graf();
                            gg.nivel = aux_list.ElementAt(0).nivel;
                            gg.pozi = aux_list.ElementAt(0).pozi;
                            gg.pleaca = aux_list.ElementAt(0).pleaca;
                            foreach (var item2 in aux_list)
                            {

                                foreach (var item3 in item2.Lista_noduri)
                                {
                                    //var exist = copy.Any(elem => elem.Lista_noduri.Contains(item3));
                                    //if (exist == false)
                                    //{
                                    gg.Lista_noduri.Add(item3);
                                    //}
                                }
                            }
                            Create_I0(gg.Lista_noduri[0], gg);
                            Group_G(gg);
                            //var group_p = gg.Lista_noduri.GroupBy(elem => elem.Split(',')[0]);
                            Graf_List.Add(gg);
                        }


                    }

                }


                if (Graf_List.Count() == current_count)
                {
                    break;
                }
                else
                {
                    last = current_count;
                    current_count = Graf_List.Count();
                    contor_global++;
                }




            }

            Create_Table();


            Display_Table();


            if (IS_Lr() == true)
            {
                Console.WriteLine("Este gramatica data de LR(1)");
                Console.Write("Numarul de elemente:");
                var lungime = Console.ReadLine();
                for (int i = 0; i < int.Parse(lungime); i++)
                {
                    lista_intrari.Add(Console.ReadLine());
                }

                foreach (var item in lista_intrari)
                {
                    var input = item + "$";
                    stiva = new Stack<string>();
                    actions = new List<string>();
                    stiva.Push("0");

                    while (input != "")
                    {
                        var linie = int.Parse(stiva.Peek());
                        var coloana = All_Symbols.IndexOf(input[0].ToString());
                        var elem = MatrixTable[linie][coloana];
                        actions.Add(elem);
                        if (elem.Equals("*"))
                        {
                            Console.WriteLine("Eroare");
                            break;
                        }
                        else if (elem[0].ToString().Equals("s"))
                        {
                            stiva.Push(All_Symbols[coloana]);
                            stiva.Push(elem.Substring(1));
                            input = input.Substring(1);
                        }
                        else if (elem[0].ToString().Equals("r"))
                        {
                            var before_Add = "";
                            var after_r = int.Parse(elem.Substring(1));
                            var grammer = Dictionary_Grammer[after_r - 1];
                            var left = grammer.Split('>')[0].ToString();
                            var right = grammer.Split('>')[1].ToString();
                            var eliminate_number = right.Length * 2;
                            for (int i = 0; i < eliminate_number; i++)
                            {
                                stiva.Pop();
                            }
                            before_Add = stiva.Peek();
                            stiva.Push(left);
                            var linie_p = int.Parse(before_Add);
                            var coloana_p = All_Symbols.IndexOf(stiva.Peek().ToString());
                            var elem_p = MatrixTable[linie_p][coloana_p];
                            actions.Add(elem_p);
                            stiva.Push(elem_p);
                        }
                        else if (elem.Equals("acc"))
                        {
                            Console.WriteLine("Acceptat");
                            foreach (var item2 in actions)
                            {
                                Console.Write(item2 + " ");
                            }
                            Console.WriteLine();
                            break;
                        }
                    }



                }





            }
            else
            {
                Console.WriteLine("Nu este gramatica data de LR(1)");

            }


            Console.ReadKey();
        }


    }
}
