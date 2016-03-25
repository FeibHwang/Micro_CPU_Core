# Micro_baby

Micro_baby is a simple CPU architecture designed by Prof.Joanne E. DeGroat from Ohio State University for education porpuse.
http://www2.ece.ohio-state.edu/~degroat/
This project is the design and verification of the Micro_baby architecture based on existing documents.

The project includes two main parts: The architecture and the simulator

The Micro_baby architecture is written in VHDL, it's been tested and verified on Modelsim

The Micro_baby simulator is written with C#, it includes 4 parts:
  1. The Assembler alone with specially designed assembly languade
  2. The Simulation Unit to simulate the Micro_baby processing sequence
  3. The Compiler, to compile a Lisp dialog into the assembly language for Micro_baby
  4. The GUI interface to present the results.
