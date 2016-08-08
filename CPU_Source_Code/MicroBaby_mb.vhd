LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mb IS
  PORT(clk : IN std_logic;
       reset : IN std_logic);
END mb;

ARCHITECTURE one OF mb IS
  -- declare components that make up MicroBaby
  ------------------------------------------------------
  COMPONENT mbctl IS
    PORT(stld : OUT std_logic;
       ldcmpl : IN std_logic;
       Dbus : IN std_logic_vector(7 downto 0);
       addr : OUT std_logic_vector(7 downto 0);
       reset : IN std_logic);
  END COMPONENT;
  FOR all : mbctl USE ENTITY WORK.mbctl(one);
  ------------------------------------------------------  
  COMPONENT mem264 IS
    PORT (rw,ce : IN std_logic;
          addr  : IN std_logic_vector (7 downto 0);
          data  : INOUT std_logic_vector (7 downto 0));  
  END COMPONENT;
  FOR all : mem264 USE ENTITY WORK.mem264(one);
  ------------------------------------------------------
  SIGNAL rw,dce,ice : std_logic;
  SIGNAL Abus,Dbus : std_logic_vector(7 downto 0);
BEGIN
  -- set up data and instruction memory
  md : mem264 PORT MAP (rw,dce,Abus,Dbus);
  mi : mem264 PORT MAP (rw,ice,Abus,Dbus);
END one;
