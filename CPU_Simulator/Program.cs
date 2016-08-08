using System;
using System.Windows.Forms;

namespace _5465
{

    static class Program
    {
        /**/
        /// <summary>
        /// Main entrance
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        
        /*
        //Testing program
        static void Main()
        {
            Compiler comp = new Compiler();
            Assembler assem = new Assembler();
            Micro_babysim sim = new Micro_babysim();
            
            string Lisp_code = "(* (/ 12,3),3)";
            string assembly_code = comp.translation(Lisp_code);
            string machine_code = assem.Translate(assembly_code);
            sim.Execution(machine_code);

            Console.WriteLine(Lisp_code);     //original lisp code
            Console.WriteLine(assembly_code);    //compiled assembly code
            Console.WriteLine(machine_code);     //machine code
            Console.WriteLine(sim.DATA[20].acc_str);   //the acc value after the second instruction 

        } 
        */
    }
}
