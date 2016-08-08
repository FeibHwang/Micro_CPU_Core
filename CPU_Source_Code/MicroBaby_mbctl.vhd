LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY mbctl IS
  PORT(stld : OUT std_logic;
       ldcmpl : IN std_logic;
       Dbus : IN std_logic_vector(7 downto 0);
       addr : OUT std_logic_vector(7 downto 0);
       rst : IN std_logic;
       clk,mclk : IN std_logic;
       rw,ice : OUT std_logic);
END mbctl;

ARCHITECTURE one OF mbctl IS
  TYPE state_type IS (reset,f1,f2,f3,f4,f5,f6,f7,f8);
  SIGNAL state,next_state : state_type;
BEGIN
  --FF process to latch next_state to state or set state when rst
  PROCESS (mclk)
  BEGIN
    IF (rst='0') THEN state <= reset;
    ELSIF (mclk'event) THEN state <= next_state;
    END IF;
  END PROCESS;
  
  PROCESS (state)
    
  BEGIN
    --FF to latch next_state to state or set state when r
    
    CASE state IS
      WHEN reset => addr <= "ZZZZZZZZ";
                    stld <= '1';
                    rw <= 'Z';
                    ice <= 'Z';
                    IF ldcmpl='1' THEN next_state<=f1; END IF;
      WHEN f1 => null;
      WHEN others => null;
    END CASE;
  END PROCESS;
END one;




ENTITY  Controler IS
END Controler;

LIBRARY  IEEE;
USE IEEE.STD_LOGIC_1164.ALL;
USE std.textio.all;
USE WORK.mbspt.all;

ARCHITECTURE test OF Controler IS

SIGNAL        DBUS : std_logic_vector (7 downto 0);
SIGNAL        Fun : std_logic_vector(3 downto 0);
SIGNAL        AddSub : std_logic;
SIGNAL        Cin : std_logic;
SIGNAL        Arlo : std_logic;
SIGNAL        Csel : std_logic_vector(1 downto 0);
SIGNAL        DrAcc : std_logic;
SIGNAL        Aal : std_logic;
SIGNAL        Bbu : std_logic;
SIGNAL        Ldacc : std_logic;
SIGNAL        Cout,N,Z : std_logic;
CONSTANT      HighZ : std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL        temp : std_logic_vector(7 downto 0);
SIGNAL        controls : std_logic_vector (12 downto 0) := "0000000000000";

signal system_clk : std_logic;
signal opt_clk : std_logic;

SIGNAL state : integer := 7;
SIGNAL op_state : integer := 1;
SIGNAL opPC : integer := 0;


SIGNAL rw1 : std_logic;
SIGNAL ce1 : std_logic := '0';
SIGNAL rw2 : std_logic := '0';
SIGNAL ce2 : std_logic := '0';
SIGNAL rw3 : std_logic := '0';
SIGNAL ce3 : std_logic := '0';

SIGNAL cet1 : std_logic := '0';
SIGNAL rwt1 : std_logic := '0';
SIGNAL cet2 : std_logic := '0';
SIGNAL rwt2 : std_logic := '0';

SIGNAL cec1 : std_logic := '0';
SIGNAL rwc1 : std_logic := '0';
SIGNAL cec2 : std_logic := '0';
SIGNAL rwc2 : std_logic := '0';

SIGNAL ldfinish1 : std_logic := '1';
SIGNAL ldfinish2 : std_logic := '1';
SIGNAL addr1: std_logic_vector (7 downto 0) := "00000000";
SIGNAL addr2: std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL addr3: std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL data1: std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL data2: std_logic_vector (7 downto 0) := "ZZZZZZZZ";
SIGNAL data3: std_logic_vector (7 downto 0) := "ZZZZZZZZ";

SIGNAL addrt: std_logic_vector (7 downto 0) := "00000000";
SIGNAL addrc: std_logic_vector (7 downto 0) := "00000000";

SIGNAL tmp1 : std_logic_vector (7 downto 0);
SIGNAL tmp2 : std_logic_vector (7 downto 0);
SIGNAL tmp3 : std_logic_vector (7 downto 0);
SIGNAL tmp4 : std_logic_vector (7 downto 0);
SIGNAL tmp5 : std_logic_vector (7 downto 0);
SIGNAL tmp6 : std_logic_vector (7 downto 0);
SIGNAL tmp7 : std_logic_vector (7 downto 0);
SIGNAL tmp8 : std_logic_vector (7 downto 0);
SIGNAL tmp9 : std_logic_vector (7 downto 0);
SIGNAL tmp10 : std_logic_vector (7 downto 0);
SIGNAL tmp11 : std_logic_vector (7 downto 0);
SIGNAL tmp12 : std_logic_vector (7 downto 0);
SIGNAL tmp13 : std_logic_vector (7 downto 0);
SIGNAL tmp14 : std_logic_vector (7 downto 0);
SIGNAL tmp15 : std_logic_vector (7 downto 0);
SIGNAL tmp16 : std_logic_vector (7 downto 0);
SIGNAL tmp17 : std_logic_vector (7 downto 0);
SIGNAL tmp18 : std_logic_vector (7 downto 0);



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

COMPONENT mem264 IS
  PORT (rw,ce : IN std_logic;
        addr  : IN std_logic_vector (7 downto 0);
                tmp1  : OUT std_logic_vector (7 downto 0);
                tmp2  : OUT std_logic_vector (7 downto 0);
                tmp3  : OUT std_logic_vector (7 downto 0);
                tmp4  : OUT std_logic_vector (7 downto 0);
                tmp5  : OUT std_logic_vector (7 downto 0);
                tmp6  : OUT std_logic_vector (7 downto 0);
        data  : INOUT std_logic_vector (7 downto 0));  
END COMPONENT;

FOR T2 : mem264 USE ENTITY WORK.mem264(one);
FOR T3 : mem264 USE ENTITY WORK.mem264(one); 
FOR T4 : mem264 USE ENTITY WORK.mem264(one); 

-- Enter your name in the (  )
TYPE mname IS (FEIBAI);
SIGNAL nm : mname := mname'VAL(0);

BEGIN -- the architecture
  
VF2 : mbdp port map (DBUS,Fun,AddSub,Cin,Arlo,Csel,DrAcc,Aal,Bbu,Ldacc,temp,Cout,N,Z);
VF3 : clkdrv port map (system_clk, opt_clk);
T2: mem264 PORT MAP (rwc2,cec2,addrc,tmp1,tmp2,tmp3,tmp4,tmp5,tmp6,data1);     -- operation memory
T3: mem264 PORT MAP (rwc1,cec1,addr2,tmp7,tmp8,tmp9,tmp10,tmp11,tmp12,data2);  -- data memory
T4: mem264 PORT MAP (rw3,ce3,addr3,tmp13,tmp14,tmp15,tmp16,tmp17,tmp18,data3);  -- output memory


cec1 <= ce2 or cet1;
rwc1 <= rw2 or rwt1;
cec2 <= ce1 or cet2;
rwc2 <= rw1 or rwt2;
addrc <= addr1 or addrt;

  PROCESS
    FILE mem_prog : TEXT; -- OPEN read_mode IS "progmem";
    VARIABLE cur_line : LINE;
    VARIABLE blkaddr : integer;
    VARIABLE byteval : bit_vector(7 downto 0);
    VARIABLE bytevalsl : std_logic_vector(7 downto 0);
    VARIABLE calcaddr : integer;
    VARIABLE stdlogaddr : std_logic_vector(7 downto 0);
  BEGIN
    file_open(mem_prog,"progmem.txt",read_mode);
    ce1 <= '0'; --no chip enable
    rw1 <= '1'; --chip set up to receive data
    WAIT FOR 100 ns;
    FOR O IN 0 to 15 LOOP
      READLINE(mem_prog,cur_line);
      READ(cur_line,blkaddr);
      READLINE(mem_prog,cur_line);
      FOR f1 IN 0 to 7 LOOP
        READ(cur_line,byteval);
        calcaddr := blkaddr*16 + f1;
        stdlogaddr := int_slog8(calcaddr);
        WAIT FOR 10 ns;
        data1 <= To_StdLogicVector(byteval);
        addr1 <= stdlogaddr;
        WAIT FOR 10 ns;
        ce1 <= '1';
        WAIT FOR 20 ns;
        ce1 <= '0';
        WAIT FOR 10 ns;
      END LOOP;  
      READLINE(mem_prog,cur_line);
      FOR f2 IN 0 to 7 LOOP
        READ(cur_line,byteval);
        calcaddr := blkaddr*16 + 8 + f2;
        stdlogaddr := int_slog8(calcaddr);
        WAIT FOR 10 ns;
        data1 <= To_StdLogicVector(byteval);
        addr1 <= stdlogaddr;
        WAIT FOR 10 ns;
        ce1 <= '1';
        WAIT FOR 20 ns;
        ce1 <= '0';
        WAIT FOR 10 ns;
      END LOOP;
    END LOOP;
    ldfinish1 <= '0';
    addr1 <= "00000000";
    data1 <= "ZZZZZZZZ";
    ce1 <= '0'; rw1 <= '0';
    WAIT FOR 100 ns;
    WAIT;
    file_close(mem_prog);
  END PROCESS;
 

  PROCESS
    FILE mem_prog : TEXT; -- OPEN read_mode IS "progmem";
    VARIABLE cur_line : LINE;
    VARIABLE blkaddr : integer;
    VARIABLE byteval : bit_vector(7 downto 0);
    VARIABLE bytevalsl : std_logic_vector(7 downto 0);
    VARIABLE calcaddr : integer;
    VARIABLE stdlogaddr : std_logic_vector(7 downto 0);
  BEGIN
    file_open(mem_prog,"datamem.txt",read_mode);
    ce2 <= '0'; --no chip enable
    rw2 <= '1'; --chip set up to receive data
    WAIT FOR 100 ns;
    FOR O IN 0 to 15 LOOP
      READLINE(mem_prog,cur_line);
      READ(cur_line,blkaddr);
      READLINE(mem_prog,cur_line);
      FOR f1 IN 0 to 7 LOOP
        READ(cur_line,byteval);
        calcaddr := blkaddr*16 + f1;
        stdlogaddr := int_slog8(calcaddr);
        WAIT FOR 10 ns;
        data2 <= To_StdLogicVector(byteval);
        addr2 <= stdlogaddr;
        WAIT FOR 10 ns;
        ce2 <= '1';
        WAIT FOR 20 ns;
        ce2 <= '0';
        WAIT FOR 10 ns;
      END LOOP;  
      READLINE(mem_prog,cur_line);
      FOR f2 IN 0 to 7 LOOP
        READ(cur_line,byteval);
        calcaddr := blkaddr*16 + 8 + f2;
        stdlogaddr := int_slog8(calcaddr);
        WAIT FOR 10 ns;
        data2 <= To_StdLogicVector(byteval);
        addr2 <= stdlogaddr;
        WAIT FOR 10 ns;
        ce2 <= '1';
        WAIT FOR 20 ns;
        ce2 <= '0';
        WAIT FOR 10 ns;
      END LOOP;
    END LOOP;
    ldfinish2 <= '0';
    addr2 <= "ZZZZZZZZ";
    data2 <= "ZZZZZZZZ";
    ce2 <= '0'; rw2 <= '0';
    WAIT FOR 100 ns;
    WAIT;
    file_close(mem_prog);
  END PROCESS;






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

VARIABLE PC1, Bef,Aft : integer := 0;
VARIABLE OPBUS : std_logic_vector (7 downto 0);

BEGIN
IF (ldfinish1 = '0' and ldfinish2 = '0') THEN -- Execute when load operation is finish

Bef := State;

IF (system_clk'event and system_clk = '1') THEN
  IF (op_state = 1) THEN op_state <= 2; rwt2 <= '0'; END IF;
  IF (op_state = 2) THEN 
     IF (state = 7 AND Bef /= Aft) THEN
       op_state <= 3; cet2 <= '1'; 
       addrt <= int_slog8(opPC); 
     END IF;
     END IF;
  IF (op_state = 3) THEN op_state <= 4; OPBUS := data1; END IF;
  IF (op_state = 4) THEN op_state <= 5;  END IF;
  IF (op_state = 5) THEN op_state <= 1;  END IF;
END IF;

CASE OPBUS IS 
      WHEN "01000010" => controls <= "1000101000000"; --ADD
      WHEN "01000011" => controls <= "1000101000000"; --ADDC
      WHEN "01000110" => controls <= "1000001000000"; --SUB
      WHEN "01000111" => controls <= "1000101000000"; --SUBC
      WHEN "01000000" => controls <= "1000101000000"; --INC
      WHEN "01000001" => controls <= "1000101000000"; --DEC

      WHEN "01010010" => controls <= "1000101000000"; --AND
      WHEN "01010011" => controls <= "1000101000000"; --OR
      WHEN "01010000" => controls <= "1000101000000"; --INV
      WHEN "01010110" => controls <= "1000101000000"; --XOR
      WHEN "01010111" => controls <= "1000101000000"; --CLRA
      WHEN OTHERS => NULL;
END CASE;

IF (state = 7 AND Bef /= Aft) THEN
IF (opPC = 255) THEN opPC <= 0;
ELSE opPC <= opPC + 1;
END IF;
END IF;

IF (state = 7) THEN state <= 1; rwt1 <= '0'; END IF;
-- phase 1: Initiation, choose operator for data bud
IF (system_clk'event and system_clk = '1') THEN
  IF (state = 1 and opt_clk = '0') THEN
    DBUS <= HighZ;
    Fun <= controls(12 downto 9);AddSub <= controls(8);
    Cin <= controls(7);Arlo <= controls(6);
    Csel <= controls(5 downto 4);DrAcc <= controls(3);
    Aal <= controls(2);Bbu <= controls(1);
    Ldacc <= controls(0);
      cet1 <= '1';
      addr2 <= int_slog8(1);
    state <= 2;
  END IF;
END IF;


-- phase 2: loading first data
IF (opt_clk'event) THEN
  IF (opt_clk = '1') THEN
    IF (state = 2) THEN

      DBUS <= data2;
      --DBUS <= "01011010"; 
      Aal <= '0';  
      Bbu <= '1';
      state <= 3;
    END IF;
  ELSIF (opt_clk = '0') THEN
    IF (state = 3) THEN
      DBUS <= "ZZZZZZZZ";
  --    cet1 <= '0';
      state <= 4;
    END IF;
  END IF;
END IF;

-- phase 3: 
IF (system_clk'event) THEN
  IF (state = 3) THEN
    IF (system_clk = '1' and opt_clk = '1' and Ldacc = '0') THEN Ldacc <= '1';
    ELSIF (system_clk = '1'and opt_clk = '1' and Ldacc = '1') THEN Ldacc <= '0'; addr2 <= int_slog8(2);
    END IF;
  END IF;
END IF;

IF (opt_clk'event and opt_clk = '0') THEN
IF (state = 4)THEN NULL; END IF;
ELSIF (opt_clk'event and opt_clk = '1') THEN
IF (state = 4)THEN state <= 5; DBUS <= data2; cet1 <= '1'; 
END IF;
END IF;

IF (opt_clk'event) THEN
  IF (opt_clk = '1') THEN
    IF(state = 4) THEN

      --DBUS <= "01011111";
      Aal <= '1'; 
      Bbu <= '0';
      state <= 5;
    END IF;
  ELSIF (opt_clk = '0') THEN
    IF (state = 5) THEN
      DBUS <= "ZZZZZZZZ";
      cet1 <= '0';
      state <= 6;
    END IF;
  END IF;
END IF;

IF (system_clk'event) THEN
  IF (state = 5 and opt_clk = '1') THEN
    IF (system_clk = '1' and opt_clk = '1' and Ldacc = '0') THEN Ldacc <= '1';
    ELSIF (system_clk = '1' and opt_clk = '1' and Ldacc = '1') THEN Ldacc <= '0'; 
    END IF;
  END IF;
END IF;


IF (system_clk'event) THEN
IF (state = 6) THEN
--?????????????
IF (system_clk = '1' and opt_clk = '0' and DrAcc = '0') THEN DrAcc <= '1';
ELSIF (system_clk = '1' and  opt_clk = '0' and DrAcc = '1') THEN DrAcc <= '0'; state <= 7;
END IF;
END IF;
END IF;

Aft := Bef;

END IF;
END PROCESS;

END test;