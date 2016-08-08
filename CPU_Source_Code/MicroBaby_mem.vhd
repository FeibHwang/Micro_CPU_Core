LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
USE WORK.mbspt.all;
ENTITY mem264 IS
  PORT (rw,ce : IN std_logic;
        addr  : IN std_logic_vector (7 downto 0);
                tmp1  : OUT std_logic_vector (7 downto 0);
                tmp2  : OUT std_logic_vector (7 downto 0);
                tmp3  : OUT std_logic_vector (7 downto 0);
                tmp4  : OUT std_logic_vector (7 downto 0);
                tmp5  : OUT std_logic_vector (7 downto 0);
                tmp6  : OUT std_logic_vector (7 downto 0);
        data  : INOUT std_logic_vector (7 downto 0));  

END mem264;

ARCHITECTURE one OF mem264 IS
  
BEGIN

PROCESS (rw,ce,addr,data)
  TYPE memwd IS ARRAY (0 to 255) of std_logic_vector (7 downto 0);
  VARIABLE mem : memwd;
  VARIABLE iaddr : integer;
BEGIN
  IF (ce = '1') THEN
    IF (rw = '1') THEN --rw high so a write
      iaddr := bin8_2_int(addr);
      mem(iaddr) := data; 
    ELSE
      iaddr := bin8_2_int(addr);
      data <= mem(iaddr);
    END IF;
  ELSE
    data <= "ZZZZZZZZ";
  END IF;

  tmp1 <= mem(240);
  tmp2 <= mem(242);
  tmp3 <= mem(245);
  tmp4 <= mem(249);
  tmp5 <= mem(250);
  tmp6 <= mem(251);


END PROCESS;  
  
END one;