using System;

namespace _5465
{
    class Assembler
    {
        public struct lebel
        /*
        Used in the translation process    
        */
        {
            public string lbname;    //name of the label
            public int loint;        //label location in int
            public string loc;       //label location in string type hexdecimal
        };

        public string Translate_Ins(string str)
        //The function receieve 1 line of assembly instruction
        //Translate and return the machine code of the Instruction
        //Assembler k = new Assembler();
        //k.asscode = "ADD_I #34";
        //Console.WriteLine(k.Translate_Ins(k.asscode));
        {
            string temp;
            switch (str.Substring(0, 5))
            {
                case "LDA_A": temp = "10000010"; break;
                case "LDA_I": temp = "10000001"; break;
                case "STA_A": temp = "10100010"; break;
                case "ADD_A": temp = "01000010"; break;
                case "ADD_I": temp = "01000001"; break;
                case "ADDCA": temp = "01001010"; break;
                case "ADDCI": temp = "01001001"; break;
                case "SUB_A": temp = "01010010"; break;
                case "SUB_I": temp = "01010001"; break;
                case "SUBCA": temp = "01110010"; break;
                case "SUBCI": temp = "01110001"; break;
                case "INCNA": temp = "01001100"; break;
                case "DECNA": temp = "01000100"; break;
                case "AND_A": temp = "01011010"; break;
                case "AND_I": temp = "01011001"; break;
                case "OR_AD": temp = "01011110"; break;
                case "OR_IM": temp = "01011101"; break;
                case "INVIN": temp = "01011000"; break;
                case "XOR_A": temp = "01010110"; break;
                case "XOR_I": temp = "01010101"; break;
                case "CLRAN": temp = "01001111"; break;
                case "CLRCN": temp = "01000000"; break;
                case "CSETN": temp = "01001000"; break;
                case "CMP_A": temp = "01111110"; break;
                case "CMP_I": temp = "01111101"; break;
                case "JMPWC": temp = "11000000"; break;
                case "JMPCE": temp = "11100100"; break;
                case "JMPCN": temp = "11100000"; break;
                case "JMPZE": temp = "11001001"; break;
                case "JMPZN": temp = "11001000"; break;
                case "JMPNE": temp = "11010010"; break;
                case "JMPNN": temp = "11010000"; break;
                default: temp = "00000000"; break;
            }
            return temp;
        }

        public string Translate_Num(string str)
        //The function receieve 1 line of assembly instruction
        //Translate and return the machine code of the Location/ Immediate number
        {
            string hexString = str.Substring(7, 2);
            int num = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
            return Convert.ToString(num, 2).PadLeft(8, '0');
        }

        public string Translate(string b)
        //The translate function is the core of the assembler
        //input: assembler code, ONE string type with multiple line
        //output: machine code 
        {
            b = b.Replace("\r", "");
            string[] codeas = b.Split('\n');
            int lbcount = 0;
            foreach (string i in codeas)
            {
                if (i[0] == '%') { lbcount++; }
            }

            lebel[] aslebel = new lebel[lbcount];
            int tmp1 = 0;
            for (int i = 0; i < codeas.Length; i++)
            {
                if (codeas[i][0] == '%')
                {
                    aslebel[tmp1].lbname = codeas[i].Substring(1, 4);
                    aslebel[tmp1].loint = i - tmp1;
                    aslebel[tmp1].loc = Convert.ToString(i - tmp1, 16).PadLeft(2, '0');
                    tmp1++;
                }
            }

            for (int ii = 0; ii < codeas.Length; ii++)
            {
                if (codeas[ii][0] == 'J')
                {
                    for (int kk = 0; kk < lbcount; kk++)
                    {
                        if (codeas[ii].Substring(7, 4) == aslebel[kk].lbname)
                        {
                            codeas[ii] = codeas[ii].Replace(codeas[ii].Substring(7, 2), aslebel[kk].loc);
                        }
                    }
                }
            }

            string[] mccode = new string[codeas.Length - lbcount];
            int cct = 0;
            int acc = 0;
            while (cct < codeas.Length)
            {
                if (codeas[cct][0] == '%') { cct++; acc++; }
                else if (codeas[cct].Length <= 5)
                {
                    mccode[cct - acc] = Translate_Ins(codeas[cct]);
                    cct++;
                }
                else
                {
                    mccode[cct - acc] = Translate_Ins(codeas[cct]) + Translate_Num(codeas[cct]);
                    cct++;
                }
            }
            string fnmc = string.Empty;
            foreach (string str in mccode)
            {
                fnmc = fnmc + str + "\r\n";
            }
            return fnmc;
        }

        //translation for disp

        public string Translate_fordis(string b)
        //translation for disp, same as Translation, adding extra space for easier observe.
        //The translate function is the core of the assembler
        //input: assembler code, ONE string type with multiple line
        //output: machine code 
        {
            b = b.Replace("\r", "");
            string[] codeas = b.Split('\n');
            
            int lbcount = 0;
            foreach (string i in codeas)
            {
                if (i[0] == '%') { lbcount++; }
            }

            lebel[] aslebel = new lebel[lbcount];
            int tmp1 = 0;
            for (int i = 0; i < codeas.Length; i++)
            {
                if (codeas[i][0] == '%')
                {
                    aslebel[tmp1].lbname = codeas[i].Substring(1, 4);
                    aslebel[tmp1].loint = i - tmp1;
                    aslebel[tmp1].loc = Convert.ToString(i - tmp1, 16).PadLeft(2, '0');
                    tmp1++;
                }
            }

            for (int ii = 0; ii < codeas.Length; ii++)
            {
                if (codeas[ii][0] == 'J')
                {
                    for (int kk = 0; kk < lbcount; kk++)
                    {
                        if (codeas[ii].Substring(7, 4) == aslebel[kk].lbname)
                        {
                            codeas[ii] = codeas[ii].Replace(codeas[ii].Substring(7, 2), aslebel[kk].loc);
                        }
                    }
                }
            }

            string[] mccode = new string[codeas.Length - lbcount];
            int cct = 0;
            int acc = 0;
            while (cct < codeas.Length)
            {
                if (codeas[cct][0] == '%') { cct++; acc++; }
                else if (codeas[cct].Length <= 5)////5->7
                {
                    mccode[cct - acc] = Translate_Ins(codeas[cct]);
                    cct++;
                }
                else
                {
                    mccode[cct - acc] = Translate_Ins(codeas[cct]) + "\t" + Translate_Num(codeas[cct]);
                    cct++;
                }
            }
            string fnmc = string.Empty;
            foreach (string str in mccode)
            {
                fnmc = fnmc + str + "\r\n";
            }
            return fnmc;
        }
    }

}
