using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace _5465
{
    
    public partial class Form1 : Form
    {
        Assembler test = new Assembler();
        Micro_babysim test2 = new Micro_babysim();
        Compiler comp = new Compiler();
        public int xx;
      
        public int ins_len;
        public int[] gaoliang_pc;
        public bool runflag;
        public string insmemo;
        public string datamemo;
        public string memory_header = "\tColumn0\tColumn1\tColumn2\tColumn3\tColumn4\tColumn5\tColumn6\tColumn7\r\n";
        public string[] addre = { "##00", "##08","##10", "##18", "##20", "##28", "##30", "##38"
                                ,"##40","##48","##50","##58","##60","##68","##70","##78"
                                ,"##80","##88","##90","##98","##A0","##A8","##B0","##B8"
                                ,"##C0","##C8","##D0","##D8","##E0","##E8","##F0","##F8"    };

        //for tab 2
        public int xx2;
        public bool show_flag;
        public int ad1, ad2, st1, st2;

        public string filepath;//used to save code file

        public Form1()
        {
            InitializeComponent();

            DataMemory.Text = "USER INSTRUCTION:\r\n" + "1.Input codes in the CODE INPUT box\r\n" + "2.Click translate to get machine codes\r\n" + "3.Click run to execute all codes\r\n"
                + "4. Click step to show step details. ";

            InstructionMemory.Text = "COMPILER INSTRUCTION:\r\n"+ "Basic function and data is in a parenthesis:(...)\r\n"+ "There is no differences between function and data, every function must have a return value, that is to say:"
                + "function could be nested:(...(..(...).))\r\n"+ "\r\nStandard function：\r\n including basic function/function of functions,the output is AC by default.\r\n"+ "basic function：\r\nIncluding'+'    '-'    '*'    '/' \r\n"
                + "\r\nFormat：\r\n(sign  data)，there must be a space between sign and data; a sequence of data is seprated by comma; \r\n"+ "more than two operands could be operated by'+' and '-';'*' and '/' must have less than 2 operands.\r\n"
                + "(+ 1,2)         //1+2，         \r\n"+ "(- 23,2,1,3,1)      //23-2-1-3-1	\r\n"+ "(* 3,4)           // 3*4\r\n"+ "(/ 5,2)         //\r\n"
                + "\r\nFunction of functions：\r\nThe operand of basic function could be other basic functions/function of functions.\r\n"+ "The number of nested time should be finite, in case of overflow.\r\n"+ "(+ 1,(* 2,4),5)       //1+(2*4)+5\r\n"
                + "(/ (* 2,3),(+ 1,1))      //(2*3)/(1+1)\r\n"+ "\r\nTemporary function：\r\nCompiler(Translate function)could recognize different temporary functions，these functions are created \r\n"+ "While compiler operating the function of functions, they can be the input of the compiler.\r\n"
                + "The format of temporary function is the same as that of basic functions/function of functions;\r\n"+ "It can also read the address."+ "But it's better not to use it alone since"+ "the index may exceed the scope and causs overlap on the existing data.\r\n"
                + "(/ 2,$FF)        //2/a,a is in the memory address FF\r\n"+ "(+ 3,$FE,12,$FD)    //2+a+12+b  a is the the memory address FE, b is in FD.\r\n"+ "(* 3,3)->$AD    //3*3,the result is saved into address AD\r\n"
                + "(/ (* $A3,$DF),(- 12,$DE))->FF     //legal format \r\n";

            //double ch = 0.33;
            //double cw = 0.23;
            //codein.Height =Convert.ToInt32(codein.Height *ch);
            //codein.Width = Convert.ToInt32(codein.Height * cw);
            //mem_occu.Value = 90;
            //data_see.Text = "lskjdflkasjf";
            //int kkk = Convert.ToInt32(adnum1.Text);
            //tryuse.Text = Convert.ToString(kkk);



            //help 页内容！！！！！！！/////////////////////////////////////////////////////////////////////////////////////////
            hlepinfo.Text = DataMemory.Text + "\r\n\r\n\r\n\r\n\r\n\r\n\r\n" + InstructionMemory.Text;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            

            runflag = false;
            show_flag = false;


            PC.Text = "0";
            IR.Text = "0";
            DataBus.Text = "0";
            Accumulator.Text = "0";
            FlagC.Text = "0";
            FlagN.Text = "0";
            FlagZ.Text = "0";
        }






        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void dispmemo(int ww)
        {
            int i, j;
            insmemo = "";
            datamemo = "";
            string temp = "";
            for (i = 0; i < 32; i++)
            {
                temp = temp + addre[i] + "\t";
                for (j = 0; j < 7; j++)
                {
                    temp = temp + test2.insmem_str[8*i + j] + "\t";
                }
                if(i!=31)
                {
                    temp = temp + test2.insmem_str[8 * i + 7] + "\r\n";
                }
                else
                {
                    temp = temp + test2.insmem_str[8 * i + 7];
                }
                //insmemo[i] = temp;
            }
            insmemo = temp;
            double ocins=0.0;
            int ocins_dis=0;
            for (int ii = 0; ii < 256; ii++)
            {
                if (test2.insmem_str[ii] != "00000000")
                {
                    ocins = ocins + 1;
                }
            }
            ocins_dis = Convert.ToInt32(ocins / 256 * 100);
            if (ww == test2.cnt)
            {
                ocins_dis = 0;
            }
            ins_occu.Value = ocins_dis;
            label14.Text = Convert.ToString(ocins_dis) + "%";
            temp = "";
            
            for (i = 0; i < 32; i++)
            {
                temp = temp + addre[i] + "\t";
                for (j = 0; j < 7; j++)
                {
                    temp = temp + test2.dtmem[8 * i + j,ww] + "\t";
                }
                if (i != 31)
                {
                    temp = temp + test2.dtmem[8 * i + 7,ww] + "\r\n";
                }
                else
                {
                    temp = temp + test2.dtmem[8 * i + 7,ww];
                }
            }
            datamemo = temp;
            double oc = 0;
            int oc_dis;
            for (int ii=0; ii<256; ii++)
            {
                if (test2.dtmem[ii, xx] != "00000000")
                {
                    oc = oc + 1;
                }
            }
            oc_dis = Convert.ToInt32(oc / 256 * 100);
            if(ww==test2.cnt)
            {
                oc_dis = 0;
            }
            mem_occu.Value = oc_dis;
            label13.Text = Convert.ToString(oc_dis)+"%";
        }

        public void dispmemo_run()
        {
            int i, j;
            insmemo = "";
            datamemo = "";
            string temp = "";
            for (i = 0; i < 32; i++)
            {
                temp = temp + addre[i] + "\t";
                for (j = 0; j < 7; j++)
                {
                    temp = temp + test2.insmem_str[8 * i + j] + "\t";
                }
                if (i != 31)
                {
                    temp = temp + test2.insmem_str[8 * i + 7] + "\r\n";
                }
                else
                {
                    temp = temp + test2.insmem_str[8 * i + 7];
                }
            }
            insmemo = temp;
            double ocins = 0.0;
            int ocins_dis = 0;
            for (int ii = 0; ii < 256; ii++)
            {
                if (test2.insmem_str[ii] != "00000000")
                {
                    ocins = ocins + 1;
                }
            }
            ocins_dis = Convert.ToInt32(ocins / 256 * 100);
            if (xx == test2.cnt)
            {
                ocins_dis = 0;
            }
            ins_occu.Value = ocins_dis;
            label14.Text = Convert.ToString(ocins_dis) + "%";


            temp = "";
            for (i = 0; i < 32; i++)
            {
                temp = temp + addre[i] + "\t";
                for (j = 0; j < 7; j++)
                {
                    temp = temp + test2.dtmem[8 * i + j, test2.cnt-1] + "\t";
                }
                if (i != 31)
                {
                    temp = temp + test2.dtmem[8 * i + 7, test2.cnt-1] + "\r\n";
                }
                else
                {
                    temp = temp + test2.dtmem[8 * i + 7, test2.cnt - 1];
                }
                //temp = temp + test2.dtmem[8 * i + 7, test2.cnt-1] + "\r\n";
                //insmemo[i] = temp;
            }
            datamemo = temp;
            //data memory occupation rate update
            double oc = 0.0;
            int oc_dis;
            for (int ii = 0; ii < 256; ii++)
            {
                if (test2.dtmem[ii, test2.cnt-1] != "00000000" )
                {
                    oc = oc + 1;
                }
            }
            oc_dis = Convert.ToInt32((oc / 256) * 100);
            mem_occu.Value = oc_dis;
            label13.Text = Convert.ToString(oc_dis)+"%";
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
       

       

        private void Translate_Click(object sender, EventArgs e)
        {
            if (codein.Text != "")
            {
                PC.Text = "0";
                IR.Text = "0";
                DataBus.Text = "0";
                Accumulator.Text = "0";
                FlagC.Text = "0";
                FlagN.Text = "0";
                FlagZ.Text = "0";


                string Debug = codein.Text;
                translation.Text = test.Translate_fordis(Debug);


                tab2codein.Text = codein.Text;
                show_flag = false;

            }
            else
            {
                MessageBox.Show("Please input code first and translate again !!!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //button run
            if (codein.Text != "")
            {
                runflag = true;
                xx = 0;
                //string []temp;
                string mccode = test.Translate(codein.Text);
                ////////////////////////
                string[] codesplit;
                int jj = 0;


                mccode = mccode.Replace("\r", "");
                //test3.Execution(mccode);
                test2.cnt = 0;
                test2.Execution(mccode);

                ////////////////////////////////////////////////////////////////////////////////////////////
                ins_len = mccode.Split('\n').Length;

                codesplit = codein.Text.Split('\n');
                gaoliang_pc = new int[ins_len + 1];  //adding one more to avoid overlap
                for (int i = 0; i < codesplit.Length; i++)
                {
                    if (codesplit[i][0] == '%')
                    {
                        continue;
                    }
                    gaoliang_pc[jj] = i;
                    jj++;
                    if (jj == ins_len)
                    { break; }
                }

                //disp/////////////////
                dispmemo_run();
                InstructionMemory.Text = memory_header + insmemo;
                DataMemory.Text = memory_header + datamemo;//////////////////////////////////

                Accumulator.Text = test2.DATA[test2.cnt - 1].acc_str;
                PC.Text = Convert.ToString(0); //添加了1
                IR.Text = test2.DATA[test2.cnt - 1].IR_str;
                DataBus.Text = test2.DATA[test2.cnt - 1].Dbus_str;
                FlagZ.Text = Convert.ToString(test2.DATA[test2.cnt - 1].Z_flag);
                FlagN.Text = Convert.ToString(test2.DATA[test2.cnt - 1].N_flag);
                FlagC.Text = Convert.ToString(test2.DATA[test2.cnt - 1].C_flag);

                cnt_dis.Text = Convert.ToString(test2.cnt);
                tab2cnt_dis.Text = cnt_dis.Text;

            }
            else
            {
                MessageBox.Show("Please input code first and translate !!!");
            }
        }

        private void Step_Click_1(object sender, EventArgs e)
        {
            if (runflag == true)
            {
                dispmemo(xx);
                InstructionMemory.Text = memory_header + insmemo;
                DataMemory.Text = memory_header + datamemo;


                Accumulator.Text = "";
                PC.Text = "";
                IR.Text = "";
                DataBus.Text = "";
                FlagC.Text = "";
                FlagN.Text = "";
                FlagZ.Text = "";
                ///////////////////////////////////////////////////////////////////////////////
                //gao liang
                if (test2.DATA[xx].PC != 0)
                {
                    //codein.SelectionColor = Color.Black;
                    codein.SelectionBackColor = Color.White;
                }
                if (xx == 0)
                {
                    int st = codein.GetFirstCharIndexFromLine(0);
                    int en = codein.GetFirstCharIndexFromLine(1);
                    if (en < st)
                    { en = codein.Text.Length; }
                    codein.Select(st, en - st);
                    //codein.SelectionColor = Color.Blue;
                    codein.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    int start = codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC]);
                    int end = codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC] + 1);
                    if (end < start)
                    { end = codein.Text.Length; }
                    codein.Select(start, end - start);
                    codein.SelectionBackColor = Color.Yellow;
                }
                
                ///////////////////////////////////////////////////////////////////////////////////


                //data updation//////////////////////////////////////////////////////////////////
                Accumulator.Text = test2.DATA[xx].acc_str;
                if (xx == test2.cnt - 1)
                {
                    PC.Text = Convert.ToString(0);
                }
                else
                {
                    PC.Text = Convert.ToString(test2.DATA[xx].PC);
                }
                IR.Text = test2.DATA[xx].IR_str;
                DataBus.Text = test2.DATA[xx].Dbus_str;
                FlagZ.Text = Convert.ToString(test2.DATA[xx].Z_flag);
                FlagN.Text = Convert.ToString(test2.DATA[xx].N_flag);
                FlagC.Text = Convert.ToString(test2.DATA[xx].C_flag);
                xx++;


                xx_dis.Text = Convert.ToString(xx);


                //message box//////////////////////////////////////////////////////////////////////////////
                if (xx >= test2.cnt + 1)
                {
                    //codein.SelectionColor = Color.Black;  //gaoliang 后续处理
                    codein.SelectionBackColor = Color.White;

                    MessageBox.Show("EXECUTION COMPLETE !", "Message");//弹出对话框
                    xx = 0;  //step循环变量置零

                    
                }
               

            }
            else
            {
                MessageBox.Show("Please run first !!!");
            }
        }


        private void back_Click(object sender, EventArgs e)
        {
            if(xx>1)
            {
                xx = xx - 2;

                dispmemo(xx);
                InstructionMemory.Text = memory_header + insmemo;
                DataMemory.Text = memory_header + datamemo;


                Accumulator.Text = "";
                PC.Text = "";
                IR.Text = "";
                DataBus.Text = "";
                FlagC.Text = "";
                FlagN.Text = "";
                FlagZ.Text = "";
                ///////////////////////////////////////////////////////////////////////////////
                //gao liang
                if (test2.DATA[xx].PC != 0)
                {
                    //codein.SelectionColor = Color.Black;
                    codein.SelectionBackColor = Color.White;
                }
                if (xx == 0)
                {
                    int st = codein.GetFirstCharIndexFromLine(0);
                    int en = codein.GetFirstCharIndexFromLine(1);
                    if (en < st)
                    { en = codein.Text.Length; }
                    codein.Select(st, en - st);
                    //codein.SelectionColor = Color.Blue;
                    codein.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    int start = codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC]);
                    int end = codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC] + 1);
                    if (end < start)
                    { end = codein.Text.Length; }
                    codein.Select(start, end - start);
                    codein.SelectionBackColor = Color.Yellow;
                }

                ///////////////////////////////////////////////////////////////////////////////////


                //data updation//////////////////////////////////////////////////////////////////
                Accumulator.Text = test2.DATA[xx].acc_str;
                if (xx == test2.cnt - 1)
                {
                    PC.Text = Convert.ToString(0);
                }
                else
                {
                    PC.Text = Convert.ToString(test2.DATA[xx].PC);
                }
                IR.Text = test2.DATA[xx].IR_str;
                DataBus.Text = test2.DATA[xx].Dbus_str;
                FlagZ.Text = Convert.ToString(test2.DATA[xx].Z_flag);
                FlagN.Text = Convert.ToString(test2.DATA[xx].N_flag);
                FlagC.Text = Convert.ToString(test2.DATA[xx].C_flag);
                xx++;


                xx_dis.Text = Convert.ToString(xx);
            }
            else if(xx==1)
            {
                MessageBox.Show("Reached the first step.");
            }
            else
            {
                MessageBox.Show("Stepback not available now.");
            }
        }


        private void compiler_click_Click_1(object sender, EventArgs e)
        {
            if (compiler_input.Text != "")
            {
                string temp = comp.translation(compiler_input.Text);
                codein.Text = temp;
            }
            else
            {
                MessageBox.Show("Please input code first and compile again !!!");
            }
        }



        /// <summary>
        /// functions for tab2 below////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clc_Click(object sender, EventArgs e)
        {
            data_see.Text = "";
            show_flag = false;
        }

        private void tab2_datashow_Click(object sender, EventArgs e)
        {
            if(adnum1.Text == "" || adnum2.Text == "" || stnum1.Text == "" || stnum2.Text == "")
            {
                MessageBox.Show("Please input 4 range numbers first!" );
            }
            else if(Convert.ToInt32(stnum2.Text)>test2.cnt)
            {
                MessageBox.Show("Step number overflow !!!");
            }
            else if (Convert.ToInt32(stnum1.Text)<=0)
            {
                MessageBox.Show("Step number must be positive integer!!!");
            }
            else if(Convert.ToInt32(adnum1.Text,16) < 0 || Convert.ToInt32(adnum2.Text,16) > 255)
            {
                MessageBox.Show("Address number wrong !!!");
            }
            else
            {
                
                ad1 = Convert.ToInt32(adnum1.Text,16);
                ad2 = Convert.ToInt32(adnum2.Text,16);
                st1 = Convert.ToInt32(stnum1.Text);
                st2 = Convert.ToInt32(stnum2.Text);


                


                for (int i = st1-1; i<=st2-1;  i++)   //i indicate the data location in the data array, that's why we have '-1'
                {
                    //Accumulator.Text = test2.DATA[xx].acc_str;
                    //if (xx == test2.cnt - 1)
                    //{
                    //    PC.Text = Convert.ToString(0);
                    //}
                    //else
                    //{
                    //    PC.Text = Convert.ToString(test2.DATA[xx].PC);
                    //}
                    //IR.Text = test2.DATA[xx].IR_str;
                    //DataBus.Text = test2.DATA[xx].Dbus_str;
                    //FlagZ.Text = Convert.ToString(test2.DATA[xx].Z_flag);
                    //FlagN.Text = Convert.ToString(test2.DATA[xx].N_flag);
                    //FlagC.Text = Convert.ToString(test2.DATA[xx].C_flag);


                    show_flag = true; 

                    string temp = "";
                    temp = temp + "\r\n\r\n\r\n" + "DATA Memory of step =\t" + Convert.ToString(i+1) + "\r\n";
                    temp = temp + "Accumulator:  "+test2.DATA[i].acc_str+"\t";
                    temp = temp + "DataBus:  " + test2.DATA[i].Dbus_str +"\t";
                    if(i==test2.cnt-1)
                    {
                        temp = temp + "PC:  " + Convert.ToString(0) + "\t";
                    }
                    else
                    {
                        temp = temp + "PC:  " + Convert.ToString(test2.DATA[i].PC) + "\t";
                    }
                    temp = temp + "IR:"+ test2.DATA[i].IR_str + "\r\n";

                    temp = temp + "Flag C:  "+ Convert.ToString(test2.DATA[i].C_flag)+"\t";
                    temp = temp + "Flag N:  " + Convert.ToString(test2.DATA[i].N_flag) + "\t";
                    temp = temp + "Flag Z:  " + Convert.ToString(test2.DATA[i].C_flag) +"\r\n\r\n";

                    int cir8 = 0;
                    int head = ad1;
                    for(int j=ad1; j<= ad2; j++)
                    {   
                        if(cir8 < 8 && cir8 > 0)
                        {
                            temp = temp + "\t";
                        }
                        else if(cir8==8)
                        {
                            cir8 = 0;
                            temp = temp + "\r\n";
                        }
                        if(cir8==0)
                        {
                            
                            temp = temp + "##"+Convert.ToString(head,16).PadLeft(2,'0')+"  to  "+"##"+ Convert.ToString(head+7, 16).PadLeft(2, '0') + "\t";
                            head = head + 8;
                        }
                        temp = temp + test2.dtmem[j, i];
                        cir8 = cir8 + 1;
                    }
                    data_see.Text = data_see.Text + temp;
                }

                //high light

                xx2 = st1 - 1;  // xx2 belong to range st1-1 to st2-1

                tab2cnt.Text = Convert.ToString(xx2+1).PadLeft(3,'0');
                if (test2.DATA[xx2].PC != 0)
                {
                    //codein.SelectionColor = Color.Black;
                    tab2codein.SelectionBackColor = Color.White;
                }
                if (xx2 == 0)
                {
                    int st = tab2codein.GetFirstCharIndexFromLine(0);
                    int en = tab2codein.GetFirstCharIndexFromLine(1);
                    if (en < st)
                    { en = tab2codein.Text.Length; }
                    tab2codein.Select(st, en - st);
                    //codein.SelectionColor = Color.Blue;
                    tab2codein.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    int start = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC]);
                    int end = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx - 1].PC] + 1);
                    if (end < start)
                    { end = tab2codein.Text.Length; }
                    tab2codein.Select(start, end - start);
                    tab2codein.SelectionBackColor = Color.Yellow;
                }

            }
        }

        private void tab2step_Click(object sender, EventArgs e)
        {
            if(show_flag == true)
            {
                //high light

                // xx2 belong to range st1-1 to st2-1, it is equal func as xx
                xx2 = xx2 + 1; // when click show data button, the first line has been highlitened
                if (xx2 >= st2)
                {
                    xx2 = st1 - 1;
                }
                tab2cnt.Text = Convert.ToString(xx2 + 1).PadLeft(3, '0');

                if (test2.DATA[xx2].PC != 0)
                {
                    //codein.SelectionColor = Color.Black;
                    tab2codein.SelectionBackColor = Color.White;
                }
                if (xx2 == 0)
                {
                    int st = tab2codein.GetFirstCharIndexFromLine(0);
                    int en = tab2codein.GetFirstCharIndexFromLine(1);
                    if (en < st)
                    { en = tab2codein.Text.Length; }
                    tab2codein.Select(st, en - st);
                    //codein.SelectionColor = Color.Blue;
                    tab2codein.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    int start = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx2 - 1].PC]);
                    int end = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx2 - 1].PC] + 1);
                    if (end < start)
                    { end = tab2codein.Text.Length; }
                    tab2codein.Select(start, end - start);
                    tab2codein.SelectionBackColor = Color.Yellow;
                }
            }
            else
            {
                MessageBox.Show("Button not available now, please click show data first");
            }
        }

        private void tab2back_Click(object sender, EventArgs e)
        {
            if(show_flag == true)
            {
                //high light

                // xx2 belong to range st1-1 to st2-1, it is equal func as xx
                xx2 = xx2 - 1 ; // when click show data button, the first line has been highlitened
                if (xx2 < 0)
                {
                    xx2 = st2 - 1;
                }
                tab2cnt.Text = Convert.ToString(xx2 + 1).PadLeft(3, '0');

                if (test2.DATA[xx2].PC != 0)
                {
                    //codein.SelectionColor = Color.Black;
                    tab2codein.SelectionBackColor = Color.White;
                }
                if (xx2 == 0)
                {
                    int st = tab2codein.GetFirstCharIndexFromLine(0);
                    int en = tab2codein.GetFirstCharIndexFromLine(1);
                    if (en < st)
                    { en = tab2codein.Text.Length; }
                    tab2codein.Select(st, en - st);
                    //codein.SelectionColor = Color.Blue;
                    tab2codein.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    int start = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx2 - 1].PC]);
                    int end = tab2codein.GetFirstCharIndexFromLine(gaoliang_pc[test2.DATA[xx2 - 1].PC] + 1);
                    if (end < start)
                    { end = tab2codein.Text.Length; }
                    tab2codein.Select(start, end - start);
                    tab2codein.SelectionBackColor = Color.Yellow;
                }
            }


            else
            {
                MessageBox.Show("Button not available now, please click show data first");
            }


        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void Accumulator_TextChanged(object sender, EventArgs e)
        {

        }

        private void Step_Click(object sender, EventArgs e)
        {

        }

        private void PC_TextChanged(object sender, EventArgs e)
        {

        }

        private void FlagZ_TextChanged(object sender, EventArgs e)
        {

        }

        private void codein_TextChanged(object sender, EventArgs e)
        {

        }
        private void DataMemory_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void translation_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void FlagN_TextChanged(object sender, EventArgs e)
        {

        }

        private void debug_TextChanged(object sender, EventArgs e)
        {

        }

        private void compiler_click_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void cnt_dis_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void adnum1_TextChanged(object sender, EventArgs e)
        {

        }

        private void mem_occu_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void readfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\Desktop";
            //if click OK 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    byte[] byteData = new byte[1024 * 1024 * 4];
                    //return length 
                    int length = fs.Read(byteData, 0, byteData.Length);
                    if (length > 0)
                    {
                        string strData = Encoding.UTF8.GetString(byteData);
                        codein.Text = strData;
                        //codein.Text = filepath;
                        MessageBox.Show("Open!");
                    }
                }
            }
            filepath = ofd.FileName;
        }

        private void clearscreen_Click(object sender, EventArgs e)
        {
            codein.Text = "";
        }

       

        //private void save_Click(object sender, EventArgs e)
        //{
        //    string strContent = codein.Text.Trim();
        //    //creat stream
        //    using (FileStream fs = new FileStream(filepath, FileMode.Create))
        //    {
        //        //trandfer string
        //        byte[] byteFile = Encoding.UTF8.GetBytes(strContent);
        //        //
        //        fs.Write(byteFile, 0, byteFile.Length);
        //        MessageBox.Show("Save！");
        //    }  
        //}

        private void help_Click(object sender, EventArgs e)
        {
            string fullfilepath = @"C:\workspace\C# project\GUI_Final\5465Finalreport2.0.pdf";
            System.Diagnostics.Process.Start(fullfilepath); 
        }
        

       
    }
}
