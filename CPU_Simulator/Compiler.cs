using System;
using System.Text.RegularExpressions;

namespace _5465
{
    /// <summary>
    /// Compiler
    /// </summary>

    class Compiler
    {
        public Compiler(){ }

        public string[] basic =
        {
            "LDA_A $",
            "LDA_I #",
            "ADD_A $",
            "ADD_I #",
            "SUB_A $",
            "SUB_I #",
            "STA_A $",
            "INCNA %",
            "DECNA %",
            "JMPWC %",
            "JMPCE %",
            "JMPCN %",
            "JMPZE %",
            "JMPZN %",
            "JMPNE %",
            "JMPNN %",
        }; //All the basic operation goes to here

        public string[] label_basic = { "LBL1", "LBL2", "LBL3", "LBL4", "LBL5", "LBL6", "LBL7", "LBL8", "LBL9",
                                    "LBM1", "LBM2", "LBM3", "LBM4", "LBM5", "LBM6", "LBM7", "LBM8", "LBM9",
                                        "LBA1", "LBA2", "LBA3", "LBA4", "LBLA5", "LBA6", "LBA7", "LBA8", "LBA9",
                                    "LBB1", "LBB2", "LBB3", "LBB4", "LBB5", "LBB6", "LBB7", "LBB8", "LBB9"};
                                    //labal for basic function
        public string[] label_cmp = { "LBN1", "LBN2", "LBN3", "LBN4", "LBN5", "LBN6", "LBN7", "LBN8", "LBN9",
                                    "LBA1", "LBA2", "LBA3", "LBA4", "LBA5", "LBA6", "LBA7", "LBA8", "LBA9",
                                        "LBE1", "LBE2", "LBE3", "LBE4", "LBE5", "LBE6", "LBE7", "LBE8", "LBE9",
                                    "LBW1", "LBW2", "LBW3", "LBW4", "LBW5", "LBW6", "LBW7", "LBW8", "LBW9"};
                                    //label for tempory function transfer
        public string[] acc = { "FF", "FE", "FD", "FC", "FB", "FA", "F9", "F8", "F7", "F6", "F5", "F4", "F3", "F2", "F1", "F0",
                                "CF", "CE", "CD", "CC", "CB", "CA", "C9", "C8", "C7", "C6", "C5", "C4", "C3", "C2", "C1", "C0",
                                "DF", "DE", "DD", "DC", "DB", "DA", "D9", "D8", "D7", "D6", "D5", "D4", "D3", "D2", "D1", "D0" };   
                                //store temporary value in one basic instruction
        public string[] result = { "EF", "EE", "ED", "EC", "EB", "EA", "E9", "E8", "E7", "E6", "E5", "E4", "E3", "E2", "E1", "E0" 
                                };  //store result of one sub instruction

        public string[] stack = { "BF", "BE", "BD", "BC", "BB", "BA", "B9", "B8", "B7", "B6", "B5", "B4", "B3", "B2", "B1", "B0"};
        //So far no porpuse

        public string hexstr(int tmp)                     
        {
            //transfer the integer number into 2 bit hexdecimal string
            return Convert.ToString(tmp, 16);
        }
        
        public string[] split_operand(string str)
        //decomposing the instruction into operands
        {
            string[] dataary = str.Split(',');
            dataary[0] = dataary[0].Split(' ')[1];
            dataary[dataary.Length - 1] = dataary[dataary.Length - 1].Split(')')[0];
            return dataary;
        }


        public string ADD_P(string str)                
        {
            /*
            compile add operation
            accepted format:
            (+ 1,2)
            (+ 1,2,3,4,5)
            (+ 1,2,$FF)->$DE
            (+ $FF,$FE)->$FD
            */
            string[] adder = split_operand(str);
            string ins;
            if (adder[0].StartsWith("$"))
            { ins = basic[0] + adder[0].Replace("$", "") + "\n"; }
            else
            { ins = basic[1] + hexstr(Convert.ToInt32(adder[0], 10)).PadLeft(2, '0') + "\n"; }
            for (int i = 1; i <= adder.Length - 1; i++)
            {
                if (adder[i].StartsWith("$"))
                { ins += basic[2] + adder[i].Replace("$", "") + "\n"; }
                else
                { ins += basic[3] + hexstr(Convert.ToInt32(adder[i], 10)).PadLeft(2, '0') + "\n"; }
            }
            if (str.EndsWith(")"))
            { ins = ins.Substring(0, ins.Length - 1); }
            else
            { ins = ins + basic[6] + str.Substring(str.Length - 2, 2); }
            return ins;
        }

        public string SUB_P(string str)       
        {
            /*
            compile sub operation
            accepted format:
            (- 1,2)
            (- 1,2,3,4,5)
            (- 1,2,$FF)->$DE
            (- $FF,$FE)->$FD
            */
            string[] suber = split_operand(str);
            string ins;
            if (suber[0].StartsWith("$"))
            { ins = basic[0] + suber[0].Replace("$", "") + "\n"; }
            else
            { ins = basic[1] + hexstr(Convert.ToInt32(suber[0], 10)).PadLeft(2, '0') + "\n"; }
            for (int i = 1; i <= suber.Length - 1; i++)
            {
                if (suber[i].StartsWith("$"))
                { ins += basic[4] + suber[i].Replace("$", "") + "\n"; }
                else
                { ins += basic[5] + hexstr(Convert.ToInt32(suber[i], 10)).PadLeft(2, '0') + "\n"; }
            }
            if (str.EndsWith(")"))
            { ins = ins.Substring(0, ins.Length - 1); }
            else
            { ins += basic[6] + str.Substring(str.Length - 2, 2); }
            return ins;
        }

        public string MULT_P(string str, int k, int t)        
        {
            /*
            compile multiplication operation
            accepted format
            (* 1,2)
            (* 2,$FF)
            (* $DE,2)->$DD
            */
            string[] muler = split_operand(str);
            string mul1, mul2, ins;
            if (muler[0].StartsWith("$"))
            {
                mul1 = muler[0].Substring(muler[0].Length - 2, 2);
                ins = basic[0] + mul1 + "\n";
            }
            else
            {
                mul1 = hexstr(Convert.ToInt32(muler[0], 10)).PadLeft(2, '0');
                ins = basic[1] + mul1 + "\n";
            }

            ins = ins + basic[12] + label_basic[k + 1] + "\n";
            ins = ins + basic[5] + "01\n";
            ins = ins + basic[6] + acc[t] + "\n";

            if (muler[1].StartsWith("$"))
            {
                mul2 = muler[1].Substring(muler[1].Length - 2, 2);
                ins = ins + basic[0] + mul2 + "\n";
            }
            else
            {
                mul2 = hexstr(Convert.ToInt32(muler[1], 10)).PadLeft(2, '0');
                ins = ins + basic[1] + mul2 + "\n";
            }
            ins = ins + basic[6] + acc[t + 1] + "\n";
            ins = ins + basic[6] + acc[t + 2] + "\n";

            ins = ins + basic[0] + acc[t] + "\n";
            ins = ins + basic[12] + label_basic[k + 1] + "\n";
            ins = ins + "%" + label_basic[k] + "\n";

            ins = ins + basic[0] + acc[t + 2] + "\n";
            ins = ins + basic[2] + acc[t + 1] + "\n";
            ins = ins + basic[6] + acc[t + 2] + "\n";

            ins = ins + basic[0] + acc[t] + "\n";
            ins = ins + basic[5] + "01" + "\n";
            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + "%" + label_basic[k + 1] + "\n";
            ins = ins + basic[13] + label_basic[k] + "\n";
            if (str.EndsWith(")"))
            { ins = ins + basic[0] + acc[t + 2]; }
            else
            {
                ins = ins + basic[0] + acc[t + 2] + "\n";
                ins += basic[6] + str.Substring(str.Length - 2, 2);
            }
            return ins;
        }

        public string DIV_P(string str, int k, int t)       
        {
        /*
        compile division operation
        accepted format
        (/ 5,2)
        (/ $DE,2)->DD
        */
            string[] diver = split_operand(str);
            string div1, div2, ins;
            
            if (diver[0].StartsWith("$"))
            {
                div1 = diver[0].Substring(diver[0].Length - 2, 2);
                ins = basic[0] + div1 + "\n";
            }
            else
            {
                div1 = hexstr(Convert.ToInt32(diver[0], 10)).PadLeft(2, '0');
                ins = basic[1] + div1 + "\n";
            }

            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[1] + "00" + "\n";
            ins = ins + basic[6] + acc[t + 1] + "\n";
            if (diver[1].StartsWith("$"))
            {
                
                div2 = diver[1].Substring(diver[1].Length - 2, 2);                
                ins = ins + basic[0] + div2 + "\n";
            }
            else
            {
                div2 = hexstr(Convert.ToInt32(diver[1], 10)).PadLeft(2, '0');
                ins = ins + basic[1] + div2 + "\n";
            }
            if (div2 == "00")
            {
                string tm = "Invalid operand!!!";
                return tm;
            }
            ins = ins + basic[6] + acc[t + 2] + "\n";
            ins = ins + "%" + label_basic[k] + "\n";
            ins = ins + basic[0] + acc[t] + "\n";
            ins = ins + basic[4] + acc[t + 2] + "\n";
            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[15] + label_basic[k + 1] + "\n";
            ins = ins + basic[14] + label_basic[k + 2] + "\n";
            ins = ins + "%" + label_basic[k + 1] + "\n";
            ins = ins + basic[0] + acc[t + 1] + "\n";
            ins = ins + basic[3] + "01\n";
            ins = ins + basic[6] + acc[t + 1] + "\n";
            ins = ins + basic[9] + label_basic[k] + "\n";
            ins = ins + "%" + label_basic[k + 2] + "\n";
            ins = ins + basic[0] + acc[t + 1] + "\n";
            if (str.EndsWith(")"))
            { ins = ins + basic[0] + acc[t + 1]; }
            else
            { ins += basic[6] + str.Substring(str.Length - 2, 2); }
            return ins;
        }

        public string MOD_P(string str, int k, int t)
        {
            string[] diver = split_operand(str);
            string mod1, mod2, ins;
            if (diver[0].StartsWith("$"))
            {
                mod1 = diver[0].Substring(diver[0].Length - 2, 2);
                ins = basic[0] + mod1 + "\n";
            }
            else
            {
                mod1 = hexstr(Convert.ToInt32(diver[0], 10)).PadLeft(2, '0');
                ins = basic[1] + mod1 + "\n";
            }

            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[1] + "00" + "\n";
            ins = ins + basic[6] + acc[t + 1] + "\n";
            if (diver[1].StartsWith("$"))
            {
                mod2 = diver[1].Substring(diver[1].Length - 2, 2);
                ins = ins + basic[0] + mod2 + "\n";
            }
            else
            {
                mod2 = hexstr(Convert.ToInt32(diver[1], 10)).PadLeft(2, '0');
                ins = ins + basic[1] + mod2 + "\n";
            }
            ins = ins + basic[6] + acc[t + 2] + "\n";
            ins = ins + "%" + label_basic[k] + "\n";
            ins = ins + basic[0] + acc[t] + "\n";
            ins = ins + basic[4] + acc[t + 2] + "\n";
            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[15] + label_basic[k + 1] + "\n";
            ins = ins + basic[14] + label_basic[k + 2] + "\n";
            ins = ins + "%" + label_basic[k + 1] + "\n";
            ins = ins + basic[0] + acc[t + 1] + "\n";
            ins = ins + basic[3] + "01\n";
            ins = ins + basic[6] + acc[t + 1] + "\n";
            ins = ins + basic[9] + label_basic[k] + "\n";
            ins = ins + "%" + label_basic[k + 2] + "\n";
            ins = ins + basic[0] + acc[t] + "\n";
            ins = ins + basic[2] + acc[t + 2] + "\n";
            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[0] + acc[t] + "\n";
            if (str.EndsWith(")"))
            { ins = ins + basic[0] + acc[t]; }
            else
            { ins += basic[6] + str.Substring(str.Length - 2, 2); }
            return ins;
        }

        public string IF_P(string str, int k, int t)      
        {
        /*
        compile branch operation
        accepted format
        (if 2,3,4)
        (if $DF,$EE,$DD)->$A0
        */
            string[] dt = split_operand(str);
            string judge, op1, op2, ins;
            if (dt[0].StartsWith("$"))
            {
                judge = dt[0].Substring(dt[0].Length - 2, 2);
                ins = basic[0] + judge + "\n";
            }
            else
            {
                judge = hexstr(Convert.ToInt32(dt[0], 10)).PadLeft(2, '0');
                ins = basic[1] + judge + "\n";
            }
            ins = ins + basic[6] + acc[t] + "\n";
            ins = ins + basic[14] + label_cmp[k] + "\n";

            if (dt[1].StartsWith("$"))
            {
                op1 = dt[1].Substring(dt[1].Length - 2, 2);
                ins = ins + basic[0] + op1 + "\n";
            }
            else
            {
                op1 = hexstr(Convert.ToInt32(dt[1], 10)).PadLeft(2, '0');
                ins = ins + basic[1] + op1 + "\n";
            }
            ins = ins + basic[6] + acc[t+1] + "\n";
            ins = ins + basic[9] + label_cmp[k+1] + "\n";
            ins = ins + "%" + label_cmp[k] + "\n";

            if (dt[2].StartsWith("$"))
            {
                op2 = dt[2].Substring(dt[2].Length - 2, 2);
                ins = ins + basic[0] + op2 + "\n";
            }
            else
            {
                op2 = hexstr(Convert.ToInt32(dt[2], 10)).PadLeft(2, '0');
                ins = ins + basic[1] + op2 + "\n";
            }
            ins = ins + basic[6] + acc[t + 1] + "\n";
            ins = ins + "%" + label_cmp[k+1] + "\n";

            if (str.EndsWith(")"))
            { ins = ins + basic[0] + acc[t+1]; }
            else
            {
                ins = ins + basic[0] + acc[t + 1] + "\n";
                ins += basic[6] + str.Substring(str.Length - 2, 2); }
            return ins;
        }        
        

        public string find_subfunction(string str)        
        {
        /*
        The function recieve the Lisp code, find out the deepest level sub_function
        split out the function, replaced by a default symbol "@@@"
        the function can only locate one subfunction at a time

        input         (/ 2,(- 3,2))
        output        (- 3,2)!(/ 2,@@@)
        */
            int subnum = Regex.Matches(str, @"\)").Count;  //count how many sub_function need to produce
            string tmp_instruction = "";
            string fst = "(", nxt = "(";
            int floc = 0, nloc = 0;
            for (int i = 0; i < str.Length; i++)
            {
                string tmp = str.Substring(i, 1);
                if (tmp == "(" || tmp == ")")
                {
                    nxt = tmp; nloc = i;
                }
                if (nxt == fst)
                {
                    fst = nxt;
                    floc = nloc;
                }
                else
                {
                    tmp_instruction = str.Substring(floc, nloc - floc + 1);
                    tmp_instruction += "!" + str.Replace(tmp_instruction, "@@@");
                    break;
                }
            }
            return tmp_instruction;
        }

        public string decomposing_func(string str)       
        {
            /*
            This function recieve the lisp code, and will split the nested function into subfunction,
            The subfunction result store location will be assigned by default according to the sequence of result[]

            input: (+ (/ (+ (* 5,5),2),(+ 2,7)),(* 2,2))
            output: (* 5,5)->$EE
                    (+ $EE,2)->$ED
                    (+ 2,7)->$EC
                    (/ $ED,$EC)->$EB
                    (* 2,2)->$EA
                    (+ $EB,$EA)

            The output is just ONE string, the instructions are connected by "\n"
            */
            string tmp_1 = find_subfunction(str);
            string[] tmp_2 = tmp_1.Split('!');
            string tmp_3 = "";
            int ac = 0;
            while (Regex.Matches(tmp_2[1], @"\)").Count != 0)
            {
                tmp_3 += tmp_2[0] + "->$" + result[++ac] + "\n";
                tmp_2[1] = tmp_2[1].Replace("@@@", "$" + result[ac]);
                tmp_1 = find_subfunction(tmp_2[1]);
                tmp_2 = tmp_1.Split('!');
            }
            tmp_3 += tmp_2[0];
            return tmp_3;
        }       

        public string translation(string str)       
        {
            /*
        The function recieve the lisp code, compile to associate assembly code directally,
        this is the core code for the compilter, no need to calls for other function for compile operation

        input: (* 5,5)
        output: LDA_I #05
                JMPZE %LBL2
                SUB_I #01
                STA_A $FF
                LDA_I #05
                STA_A $FE
                STA_A $FD
                LDA_A $FF
                JMPZE %LBL2
                %LBL1
                LDA_A $FD
                ADD_A $FE
                STA_A $FD
                LDA_A $FF
                SUB_I #01
                STA_A $FF
                %LBL2
                JMPZN %LBL1
                LDA_A $FD
        The assembly code output is still one string, because this is the assembler recieved format 
        */
            string tmp_1 = decomposing_func(str);
            if (Regex.Matches(str, @"\)").Count == 1)            
            {
                tmp_1 = str;
            }
            string[] tmp_2 = tmp_1.Split('\n');
            //split the instruction into array
            string tmp_3 = "";
            int lb = 0;                           //label counter for basic operation
            int t = 0;                            //virtual register assignment counter
            int k = 0;                            //label counter for complicated operation
            for (int i = 0; i < tmp_2.Length; i++)
            {
                if (tmp_2[i].Substring(1, 1) == "*")
                {
                    tmp_3 += MULT_P(tmp_2[i], lb, t) + "\n";
                    lb += 3;
                    t += 3;
                }
                else if (tmp_2[i].Substring(1, 1) == "/")
                {
                    tmp_3 += DIV_P(tmp_2[i], lb, t) + "\n";
                    lb += 3;
                    t += 3;
                }
                else if (tmp_2[i].Substring(1, 1) == "%")
                {
                    tmp_3 += MOD_P(tmp_2[i], lb, t) + "\n";
                    lb += 3;
                    t += 3;
                }
                else if (tmp_2[i].Substring(1, 1) == "+")
                {
                    tmp_3 += ADD_P(tmp_2[i]) + "\n";
                    t += 1;
                }
                else if (tmp_2[i].Substring(1, 1) == "-")
                {
                    tmp_3 += SUB_P(tmp_2[i]) + "\n";
                    t += 1;
                }
                else if (tmp_2[i].Substring(1, 2) == "if")
                {
                    tmp_3 += IF_P(tmp_2[i], lb, k) + "\n";
                    lb += 3;
                    k += 3;

                }
            }
            tmp_3 = tmp_3.Substring(0,tmp_3.Length-1);
            return tmp_3;
        }

        public int ins_count(string str)
        {
        /*
        input: lisp code
        calculate how many assembly instruction will generate  
        */
            string ins = translation(str);
            int count = ins.Replace("\n", "//").Length - ins.Length;
            int lcnt = ins.Length - ins.Replace("\n%", "/").Length ;
            return count - lcnt;
        }

        public bool ins_overlap(string str)
            /*
            input: lisp code
            calcuate to decide wether the instruction will overlap to the virtual register region
            return bool type: true/ fause
            */
        {
            int insnum = ins_count(str);
            if (insnum > 256 - acc.Length - result.Length - stack.Length)
                return false;
            else
                return true;
        }
    }
}