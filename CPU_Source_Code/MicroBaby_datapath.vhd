LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY busdr8 IS
  PORT(din : IN std_logic_vector(7 downto 0);
       drive : IN std_logic;
       dout : OUT std_logic_vector(7 downto 0));
END busdr8;

ARCHITECTURE one OF busdr8 IS
BEGIN
  dout <= din WHEN drive='1' ELSE "ZZZZZZZZ";
END one;

--===================================================

LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mux8_2to1 IS
  PORT(minl,minr : IN std_logic_vector(7 downto 0);
       msel      : IN std_logic;
       mout      : OUT std_logic_vector(7 downto 0));
END mux8_2to1;

ARCHITECTURE one OF mux8_2to1 IS
BEGIN
  mout <= minl WHEN msel='1' ELSE minr;
END one;

--===================================================

LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mbaccum IS
  PORT(accin : IN std_logic_vector(7 downto 0);
       ld    : IN std_logic;
       accout : OUT std_logic_vector(7 downto 0));
END mbaccum;

ARCHITECTURE one OF mbaccum IS

BEGIN
  PROCESS
  BEGIN
    WAIT UNTIL ld='1' AND ld'event;
    accout <= accin;
  END PROCESS;
END one;

--==================================================

LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mbalu IS
   PORT(A,B : IN std_logic_vector(7 downto 0);
        res : OUT std_logic_vector(7 downto 0);
        cin : IN std_logic;
        Fun : IN std_logic_vector(3 downto 0);
        Csel : IN std_logic_vector(1 downto 0);
        Arlo : IN std_logic;
        AddSub : IN std_logic;
        Cout,N,Z : OUT std_logic);
END mbalu;

ARCHITECTURE one OF mbalu IS
   SIGNAL Lout,ires : std_logic_vector(7 downto 0);
   SIGNAL Binv,Binp,Sum : std_logic_vector(7 downto 0);
   SIGNAL Cs1,Cc1,Cval,Cinp : std_logic;
   SIGNAL Ctemp : std_logic_vector(8 downto 0);
   SIGNAL iFun0,iFun1,iFun2,iFun3 : std_logic_vector(7 downto 0);
   
BEGIN
-- Set up the 8 bits for the function select'
iFun0 <= Fun(0) & Fun(0) & Fun(0) & Fun(0) & Fun(0) & Fun(0) & Fun(0) & Fun(0);
iFun1 <= Fun(1) & Fun(1) & Fun(1) & Fun(1) & Fun(1) & Fun(1) & Fun(1) & Fun(1);
iFun2 <= Fun(2) & Fun(2) & Fun(2) & Fun(2) & Fun(2) & Fun(2) & Fun(2) & Fun(2);
iFun3 <= Fun(3) & Fun(3) & Fun(3) & Fun(3) & Fun(3) & Fun(3) & Fun(3) & Fun(3);
-- Logic Unit  
Lout <= (NOT A AND NOT B AND iFun0) OR (NOT A AND B AND iFun1) OR (A AND NOT B AND iFun2) OR (A AND B AND iFun3);

-- Prep B input to Add/Sub
Binv <= NOT B;
Binp <= B WHEN AddSub = '1' ELSE Binv;

Cs1 <= Cin WHEN Csel(0)='0' ELSE NOT Cin;
Cc1 <= '0' WHEN Csel(0)='0' ELSE '1';
Cinp <= Cs1 WHEN Csel(1)='0' ElSE Cc1;
Ctemp(0) <= Cinp;

-- add 2 inputs
Ctemp(8 downto 1) <= (A AND Binp) OR (A AND Ctemp(7 downto 0)) OR (B AND Ctemp(7 downto 0));
Sum <= A XOR Binp XOR Ctemp(7 downto 0);

-- generate outputs
Cout <= Ctemp(8);
N <= ires(7);
Z <= NOT(ires(7) OR ires(6) OR ires(5) OR ires(4) OR ires(3) OR ires(2) OR ires(1) OR ires(0));

ires <= Sum WHEN Arlo = '1' ELSE Lout;
res <= ires;

  

END one;

--================================================

LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mbdp IS
  PORT (Dbus : INOUT std_logic_vector(7 downto 0);
        Fun : IN std_logic_vector(3 downto 0);
        AddSub : IN std_logic;
        Cin : IN std_logic;
        Arlo : IN std_logic;
        Csel : IN std_logic_vector(1 downto 0);
        DrAcc : IN std_logic;
        Aal : IN std_logic;
        Bbu : IN std_logic;
        Ldacc : IN std_logic;
        --tst
        temp : OUT std_logic_vector(7 downto 0);
        --tst
        Cout,N,Z : OUT std_logic);    
END mbdp;

ARCHITECTURE one OF mbdp IS
  -- declare and configure components
  COMPONENT mbalu IS
   PORT(A,B : IN std_logic_vector(7 downto 0);
        res : OUT std_logic_vector(7 downto 0);
        cin : IN std_logic;
        Fun : IN std_logic_vector(3 downto 0);
        Csel : IN std_logic_vector(1 downto 0);
        Arlo : IN std_logic;
        AddSub : IN std_logic;
        Cout,N,Z : OUT std_logic);
  END COMPONENT;
  FOR all : mbalu USE ENTITY WORK.mbalu(one);
  ------------------------------------------------------
  COMPONENT mbaccum IS
    PORT(accin : IN std_logic_vector(7 downto 0);
         ld    : IN std_logic;
         accout : OUT std_logic_vector(7 downto 0));
  END COMPONENT;
  FOR all : mbaccum USE ENTITY WORK.mbaccum(one);
  ------------------------------------------------------
  COMPONENT mux8_2to1 IS
    PORT(minl,minr : IN std_logic_vector(7 downto 0);
         msel      : IN std_logic;
         mout      : OUT std_logic_vector(7 downto 0));
  END COMPONENT;
  FOR all : mux8_2to1 USE ENTITY WORK.mux8_2to1(one);
  ------------------------------------------------------
  COMPONENT busdr8 IS
    PORT(din : IN std_logic_vector(7 downto 0);
         drive : IN std_logic;
         dout : OUT std_logic_vector(7 downto 0));
  END COMPONENT;
  FOR all : busdr8 USE ENTITY WORK.busdr8(one);
  ------------------------------------------------------  
  SIGNAL amuxtoacc : std_logic_vector(7 downto 0);
  SIGNAL accout,bmuxout,alures : std_logic_vector(7 downto 0);
  SIGNAL zero : std_logic_vector(7 downto 0) := "00000000";
  --================================test signal=================
  signal tmp : std_logic_vector(7 downto 0);
  
BEGIN
  alu : mbalu PORT MAP (accout,bmuxout,alures,Cin,Fun,Csel,Arlo,AddSub,Cout,N,Z);
  acc : mbaccum PORT MAP (amuxtoacc,Ldacc,accout);
  m1  : mux8_2to1 PORT MAP (alures,Dbus,Aal,amuxtoacc);
  m2  : mux8_2to1 PORT MAP (zero,Dbus,Bbu,bmuxout);
  bd1 : busdr8 PORT MAP (accout,DrAcc,Dbus);
  tmp <= accout;
  temp <= tmp;
END one;







--======================Test Bentch================================
ENTITY  testbc IS
END testbc;

LIBRARY  IEEE;
USE IEEE.STD_LOGIC_1164.ALL;
ARCHITECTURE test OF testbc IS

SIGNAL  DBUS : std_logic_vector (7 downto 0);
SIGNAL  Fun : std_logic_vector(3 downto 0);
SIGNAL        AddSub : std_logic;
SIGNAL        Cin : std_logic;
SIGNAL        Arlo : std_logic;
SIGNAL        Csel : std_logic_vector(1 downto 0);
SIGNAL        DrAcc : std_logic;
SIGNAL        Aal : std_logic;
SIGNAL        Bbu : std_logic;
SIGNAL        Ldacc : std_logic;
SIGNAL        Cout,N,Z : std_logic;
CONSTANT  HighZ : std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL        temp : std_logic_vector(7 downto 0);

-- ENTER THE COMPONENT DECLARATION AND CONFIGURATION HERE

component mbdp IS
  PORT (Dbus : INOUT std_logic_vector(7 downto 0);
        Fun : IN std_logic_vector(3 downto 0);
        AddSub : IN std_logic;
        Cin : IN std_logic;
        Arlo : IN std_logic;
        Csel : IN std_logic_vector(1 downto 0);
        DrAcc : IN std_logic;
        Aal : IN std_logic;
        Bbu : IN std_logic;
        Ldacc : IN std_logic;
                temp : OUT std_logic_vector(7 downto 0);
        Cout,N,Z : OUT std_logic);    
END COMPONENT;

FOR VF2 : mbdp USE ENTITY WORK.mbdp(one);


-- Enter your name in the (  )
TYPE mname IS (yourname);
SIGNAL nm : mname := mname'VAL(0);

BEGIN -- the architecture
  
VF2 : mbdp port map (DBUS,Fun,AddSub,Cin,Arlo,Csel,DrAcc,Aal,Bbu,Ldacc,temp,Cout,N,Z);

PROCESS
 
BEGIN
  
--????????
DBUS <= HighZ;
Fun <= "1000";
AddSub <= '0';
Cin <= '0';
Arlo <= '1';
Csel <= "00";
DrAcc <= '0';
Aal <= '0';
Bbu <= '0';
Ldacc <= '0';


--?????????????
WAIT FOR 10 ns;
DBUS <= "01011010" , "ZZZZZZZZ" after 9 ns;
AddSub <= '1';
Aal <= '0';
Bbu <= '1';
--accumalator ??
WAIT FOR 1 ns;
Ldacc <= '1','0' after 1 ns;

--?????????????
WAIT FOR 10 ns;
DBUS <= "01011111" , "ZZZZZZZZ" after 9 ns;
Aal <= '1';
Bbu <= '0';
--accumalator ??
WAIT FOR 2 ns;
Ldacc <= '1','0' after 1 ns;

--?????????????
WAIT FOR 10 ns;
DrAcc <= '1', '0' after 5 ns;


WAIT FOR 20 ns;

END PROCESS;

END test;












ENTITY  test_ctwithclk IS
END test_ctwithclk;

LIBRARY  IEEE;
USE IEEE.STD_LOGIC_1164.ALL;
ARCHITECTURE test OF test_ctwithclk IS

SIGNAL  DBUS : std_logic_vector (7 downto 0);
SIGNAL  Fun : std_logic_vector(3 downto 0);
SIGNAL        AddSub : std_logic;
SIGNAL        Cin : std_logic;
SIGNAL        Arlo : std_logic;
SIGNAL        Csel : std_logic_vector(1 downto 0);
SIGNAL        DrAcc : std_logic;
SIGNAL        Aal : std_logic;
SIGNAL        Bbu : std_logic;
SIGNAL        Ldacc : std_logic;
SIGNAL        Cout,N,Z : std_logic;
CONSTANT  HighZ : std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL        temp : std_logic_vector(7 downto 0);

signal system_clk : std_logic;
signal opt_clk : std_logic;

SIGNAL state : integer := 1;

-- ENTER THE COMPONENT DECLARATION AND CONFIGURATION HERE

component mbdp IS
  PORT (Dbus : INOUT std_logic_vector(7 downto 0);
        Fun : IN std_logic_vector(3 downto 0);
        AddSub : IN std_logic;
        Cin : IN std_logic;
        Arlo : IN std_logic;
        Csel : IN std_logic_vector(1 downto 0);
        DrAcc : IN std_logic;
        Aal : IN std_logic;
        Bbu : IN std_logic;
        Ldacc : IN std_logic;
                temp : OUT std_logic_vector(7 downto 0);
        Cout,N,Z : OUT std_logic);    
END COMPONENT;

FOR VF2 : mbdp USE ENTITY WORK.mbdp(one);

COMPONENT clkdrv IS
  PORT(mclk : IN std_logic;
       clk  : OUT std_logic);
END COMPONENT;

FOR VF3 : clkdrv USE ENTITY WORK.clkdrv(one);

-- Enter your name in the (  )
TYPE mname IS (yourname);
SIGNAL nm : mname := mname'VAL(0);

BEGIN -- the architecture
  
VF2 : mbdp port map (DBUS,Fun,AddSub,Cin,Arlo,Csel,DrAcc,Aal,Bbu,Ldacc,temp,Cout,N,Z);
VF3 : clkdrv port map (system_clk, opt_clk);

PROCESS
begin
system_clk <= '0';
wait for 5 ns;
system_clk <= '1';
wait for 5 ns;
END process;

PROCESS(system_clk,opt_clk)
 
-- Control Signal Data Bus:
-- Fun & AddSub & Cin & Arlo & Csel & DrAcc & Aal & Bbu & Ldacc

BEGIN
  
IF (system_clk'event and system_clk = '1') THEN
IF (state = 1 and opt_clk = '0') THEN
DBUS <= HighZ;
Fun <= "1000";
AddSub <= '0';
Cin <= '0';
Arlo <= '1';
Csel <= "00";
DrAcc <= '0';
Aal <= '0';
Bbu <= '0';
Ldacc <= '0';
state <= 2;
END IF;
END IF;


IF (opt_clk'event) THEN
--?????????????
    IF (opt_clk = '1') THEN
	IF (state = 2) THEN
      DBUS <= "01011010";
      AddSub <= '1'; 
Aal <= '0';  
Bbu <= '1';
      state <= 3;
        END IF;
    ELSIF (opt_clk = '0') THEN
	IF (state = 3) THEN
      DBUS <= "ZZZZZZZZ";
      state <= 4;
        END IF;
    END IF;
END IF;


IF (system_clk'event) THEN
IF (state = 3) THEN
--accumalator ??
IF (system_clk = '1' and opt_clk = '1') THEN Ldacc <= '1';
ELSIF (system_clk = '1'and Ldacc = '1') THEN Ldacc <= '0'; 
END IF;
END IF;
END IF;

IF (opt_clk'event and opt_clk = '0') THEN
IF (state = 4)THEN NULL; END IF;
ELSIF (opt_clk'event and opt_clk = '1') THEN
IF (state = 4)THEN state <= 5; END IF;
END IF;

IF (opt_clk'event) THEN
--?????????????

IF (opt_clk = '1') THEN
IF(state = 4) THEN
DBUS <= "01011111";
Aal <= '1'; 
Bbu <= '0';
state <= 5;
END IF;
ELSIF (opt_clk = '0') THEN
IF (state = 5) THEN
DBUS <= "ZZZZZZZZ";
state <= 6;
END IF;
END IF;

END IF;

IF (system_clk'event) THEN
IF (state = 5 and opt_clk = '1') THEN
--accumalator ??
IF (system_clk = '1' and opt_clk = '1') THEN Ldacc <= '1';
ELSIF (system_clk = '0' and Ldacc = '1') THEN Ldacc <= '0'; 
END IF;
END IF;
END IF;


IF (system_clk'event) THEN
IF (state = 6) THEN
--?????????????
IF (system_clk = '1' and opt_clk = '1' and DrAcc = '0') THEN DrAcc <= '1';
ELSIF (system_clk = '1' and  opt_clk = '1' and DrAcc = '1') THEN DrAcc <= '0'; state <= 1;
END IF;
END IF;
END IF;

END PROCESS;

END test;