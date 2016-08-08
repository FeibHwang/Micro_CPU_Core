using System;

namespace _5465
{
    struct data_t
    {
        //snapshort format
        public int[] memory_ins;      //snapshort for instruction memory
        public int[] memory_data;     //snapshort for data memory
        public string[] data_string;  //snapshort for string type data memory

        public int acc;               //tempory acc value for inner function
        public string acc_str;        //associate string type acc value for simulator
        public int PC;                //program counter
        public int IR;                //instruction register
        public string IR_str;         //associate string type
        public int Dbus;              //data bus value for inner function
        public string Dbus_str;       //associate data bus value for inner function

        public int Z_flag;
        public int N_flag;
        public int C_flag;
    }


    class Micro_babysim
    {
        
        //public string temp;               //output 2scomplement
        public int cnt = 0;                 //step count
        public const int Sim_snapshort = 1000;   //total snap short

        public string[,] dtmem = new string[256, Sim_snapshort];

        public data_t data;        

        public string[] insmem_str = new string[256];

        public string complement(int a)
        /*
        input a int number
        return a string type 2's complement
        */
        {
            string temp = "00000000";
            if (a == 0) { temp = "00000000"; }
            else if (a > 0)
            {
                a = a + 256;
                temp = Convert.ToString(a, 2);
                temp = temp.Substring(1);
            }
            else
            {
                a = -a;
                a = ~a;
                a = a + 1;
                temp = Convert.ToString(a, 2);
                temp = temp.Substring(24);
            }
            return temp;

        }

        public void LDA_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].memory_data[op];
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }

        }
        public void LDA_I(int ins, int op)
        {
            DATA[cnt].acc = op;
            DATA[cnt].Dbus = op;
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            else
            {
                DATA[cnt].Z_flag = 0;
            }
        }
        public void STA_A(int ins, int op)
        {
            DATA[cnt].memory_data[op] = DATA[cnt].acc;
            DATA[cnt].Dbus = DATA[cnt].acc;
        }
        public void ADD_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc + DATA[cnt].memory_data[op];
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void ADD_I(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc + op;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = op;
        }
        public void ADDCA(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc + DATA[cnt].memory_data[op] + DATA[cnt].C_flag;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }

            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void ADDCI(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc + op + DATA[cnt].C_flag;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = op;
        }
        public void SUB_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - DATA[cnt].memory_data[op];
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void SUB_I(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - op;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = op;
        }
        public void SUBCA(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - DATA[cnt].memory_data[op] - DATA[cnt].C_flag;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void SUBCI(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - op - DATA[cnt].C_flag;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].Dbus = op;
        }
        public void INCNA(int ins, int op)
        {
            DATA[cnt].acc++;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            //Dbus = memory_data[op];
        }
        public void DECNA(int ins, int op)
        {
            DATA[cnt].acc--;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].acc = DATA[cnt].acc - 128;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
                DATA[cnt].acc = DATA[cnt].acc + 128;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            //Dbus = memory_data[op];
        }
        public void AND_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc & DATA[cnt].memory_data[op];
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void AND_I(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc & op;
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = op;
        }
        public void OR_AD(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc | DATA[cnt].memory_data[op];
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void OR_IM(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc | op;
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = op;
        }
        public void INVIN(int ins, int op)
        {
            DATA[cnt].acc = ~DATA[cnt].acc;
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void XOR_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc ^ DATA[cnt].memory_data[op];
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void XOR_I(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc ^ op;
            if (DATA[cnt].acc == 0)
            {
                DATA[cnt].Z_flag = 1;
            }
            DATA[cnt].Dbus = op;
        }
        public void CLRAN(int ins, int op)
        {
            DATA[cnt].acc = 0;
        }
        public void CLRCN(int ins, int op)
        {
            DATA[cnt].C_flag = 0;
        }
        public void CSETN(int ins, int op)
        {
            DATA[cnt].C_flag = 1;
        }
        public void CMP_A(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - DATA[cnt].memory_data[op];
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].acc = DATA[cnt].acc + DATA[cnt].memory_data[op];
            DATA[cnt].Dbus = DATA[cnt].memory_data[op];
        }
        public void CMP_I(int ins, int op)
        {
            DATA[cnt].acc = DATA[cnt].acc - op;
            if (DATA[cnt].acc > 127)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 0;
                DATA[cnt].Z_flag = 0;
            }
            else if (DATA[cnt].acc < -128)
            {
                DATA[cnt].C_flag = 1;
                DATA[cnt].N_flag = 1;
                DATA[cnt].Z_flag = 0;
            }
            else
            {
                DATA[cnt].C_flag = 0;
                if (DATA[cnt].acc == 0)
                {
                    DATA[cnt].Z_flag = 1;
                    DATA[cnt].N_flag = 0;
                }
                else if (DATA[cnt].acc > 0)
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 0;
                }
                else
                {
                    DATA[cnt].Z_flag = 0;
                    DATA[cnt].N_flag = 1;
                }
            }
            DATA[cnt].acc = DATA[cnt].acc + op;
            DATA[cnt].Dbus = op;
        }
        public void JMPWC(int ins, int op)
        {
            DATA[cnt].PC = op - 1;
        }
        public void JMPCE(int ins, int op)
        {
            if (DATA[cnt].C_flag == 1)
            {
                DATA[cnt].PC = op - 1;
            }
        }
        public void JMPCN(int ins, int op)
        {
            if (DATA[cnt].C_flag == 0)
            {
                DATA[cnt].PC = op - 1;
            }
        }
        public void JMPZE(int ins, int op)
        {
            if (DATA[cnt].Z_flag == 1)
            {
                DATA[cnt].PC = op - 1;
            }
        }
        public void JMPZN(int ins, int op)
        {
            if (DATA[cnt].Z_flag == 0)
            {
                DATA[cnt].PC = op - 1;
            }
        }
        public void JMPNE(int ins, int op)
        {
            if (DATA[cnt].N_flag == 1)
            {
                DATA[cnt].PC = op - 1;
            }
        }
        public void JMPNN(int ins, int op)
        {
            if (DATA[cnt].N_flag == 0)
            {
                DATA[cnt].PC = op - 1;
            }
        }

        public void excuteline(int a, int b)
        /*
        input one line of machine code
        the function will call for 31 actrual function to do the execute
        */
        {
            switch (a)
            {
                case 130: LDA_A(a, b); break;
                case 129: LDA_I(a, b); break;
                case 162: STA_A(a, b); break;
                case 66: ADD_A(a, b); break;
                case 65: ADD_I(a, b); break;
                case 74: ADDCA(a, b); break;
                case 73: ADDCI(a, b); break;
                case 82: SUB_A(a, b); break;
                case 81: SUB_I(a, b); break;
                case 114: SUBCA(a, b); break;
                case 113: SUBCI(a, b); break;
                case 76: INCNA(a, b); break;
                case 68: DECNA(a, b); break;
                case 90: AND_A(a, b); break;
                case 89: AND_I(a, b); break;
                case 94: OR_AD(a, b); break;
                case 93: OR_IM(a, b); break;
                case 88: INVIN(a, b); break;
                case 86: XOR_A(a, b); break;
                case 85: XOR_I(a, b); break;
                case 79: CLRAN(a, b); break;
                case 64: CLRCN(a, b); break;
                case 72: CSETN(a, b); break;
                case 126: CMP_A(a, b); break;
                case 125: CMP_I(a, b); break;
                case 192: JMPWC(a, b); break;
                case 228: JMPCE(a, b); break;
                case 224: JMPCN(a, b); break;
                case 201: JMPZE(a, b); break;
                case 200: JMPZN(a, b); break;
                case 210: JMPNE(a, b); break;
                case 208: JMPNN(a, b); break;
                default: Console.WriteLine("Wrong translation"); break;
            }
        }

        public data_t[] DATA = new data_t[Sim_snapshort];//snapshort with max number Sim_snapshort
        public void Execution(string str)
        /*
        Core function of the simulator
        input the machine code from assembler
        excute the code
        everytime when one line of code was excuted
        the function will take a snapshort
        */
        {
            string[] mccodeml = str.Split('\n');
            int memloc = (mccodeml).Length - 1;

            data.memory_ins = new int[256];
            data.memory_data = new int[256];
            for (int i = 0; i < memloc; i++)
            {

                if (mccodeml[i].Length <= 9)
                {
                    data.memory_ins[i] = Convert.ToInt32(mccodeml[i].Substring(0, 8), 2);
                    data.memory_data[i] = 0;
                }
                else if ((mccodeml[i].Length >= 10) & (mccodeml[i].Length <= 17))
                {
                    data.memory_ins[i] = Convert.ToInt32(mccodeml[i].Substring(0, 8), 2);
                    if ((mccodeml[i].Substring(0, 2) != "11") & (mccodeml[i].Substring(6, 2) == "01") & (mccodeml[i].Substring(8, 1) == "0")) { data.memory_data[i] = Convert.ToInt32(mccodeml[i].Substring(9, 7), 2); }
                    else if ((mccodeml[i].Substring(0, 2) != "11") & (mccodeml[i].Substring(6, 2) == "01") & (mccodeml[i].Substring(8, 1) == "1")) { data.memory_data[i] = -1 * (128 - Convert.ToInt32(mccodeml[i].Substring(9, 7), 2)); }
                    else data.memory_data[i] = Convert.ToInt32(mccodeml[i].Substring(8, 8), 2);
                }
            }

            for (int j = 0; j < 256; j++)
            {
                insmem_str[j] = complement(data.memory_ins[j]);
            }
            DATA[0].data_string = new string[256];
            DATA[0].memory_data = data.memory_data;
            DATA[0].acc = 0;
            DATA[0].PC = 0;
            DATA[0].IR = 0;
            DATA[0].Dbus = 0;
            DATA[0].Z_flag = 0;
            DATA[0].N_flag = 0;
            DATA[0].C_flag = 0;

            excuteline(data.memory_ins[DATA[cnt].PC], DATA[cnt].memory_data[DATA[cnt].PC]);  //execute one single line of instruction
            DATA[cnt].IR = data.memory_ins[DATA[cnt].PC];       //store IR
            for (int k = 0; k < 256; k++)
            {
                DATA[0].data_string[k] = complement(DATA[0].memory_data[k]);
                dtmem[k, 0] = complement(DATA[0].memory_data[k]);
            }
            DATA[cnt].acc_str = complement(DATA[cnt].acc);
            DATA[0].IR_str = insmem_str[0];
            DATA[cnt].Dbus_str = complement(DATA[cnt].Dbus);
            DATA[cnt].PC++;
            cnt++;
            DATA[cnt] = DATA[cnt - 1];


            while (DATA[cnt].PC != memloc)
            {
                DATA[cnt].IR = data.memory_ins[DATA[cnt].PC];       //store IR
                excuteline(data.memory_ins[DATA[cnt].PC], DATA[cnt].memory_data[DATA[cnt].PC]);  //execute one single line of instruction
                for (int k = 0; k < 256; k++)
                {
                    DATA[cnt].data_string[k] = complement(DATA[cnt].memory_data[k]);
                    dtmem[k, cnt] = DATA[cnt].data_string[k];
                }
                DATA[cnt].acc_str = complement(DATA[cnt].acc);

                DATA[cnt].IR_str = insmem_str[DATA[cnt].PC];

                DATA[cnt].Dbus_str = complement(DATA[cnt].Dbus);

                DATA[cnt].PC++;

                if (cnt == Sim_snapshort-1)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        DATA[cnt-1].data_string[k] = "11111111";
                        dtmem[k, cnt-1] = DATA[cnt].data_string[k];
                    }
                    break;
                }
                DATA[cnt + 1] = DATA[cnt];
                cnt++;                
            }
        }
    }
}
