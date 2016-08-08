LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
USE std.textio.all;
USE WORK.mbspt.all;
ENTITY load_mem IS
  PORT (rw1,ce1,rw2,ce2,ldfinish1, ldfinish2 : OUT std_logic;
        addr1, addr2 : OUT std_logic_vector (7 downto 0);
        data1, data2: OUT std_logic_vector (7 downto 0));
END load_mem;

ARCHITECTURE one OF load_mem IS

SIGNAL temp1 : std_logic := '1';
SIGNAL temp2 : std_logic := '1';

BEGIN

  ldfinish1 <= temp1;
  ldfinish2 <= temp2;

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
    temp1 <= '0';
    addr1 <= "ZZZZZZZZ";
    data1 <= "ZZZZZZZZ";
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
    temp2 <= '0';
    addr2 <= "ZZZZZZZZ";
    data2 <= "ZZZZZZZZ";
    WAIT FOR 100 ns;
    WAIT;
    file_close(mem_prog);
  END PROCESS;
 
END one;





--===================Loading operation ======================
LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
USE WORK.mbspt.all;

ENTITY load_operation IS
END load_operation;

ARCHITECTURE behave of load_operation IS

SIGNAL rw1, ce1,rw2, ce2, ldfinish1, ldfinish2 : std_logic;
SIGNAL addr1, addr2 : std_logic_vector (7 downto 0);
SIGNAL data1, data2 : std_logic_vector (7 downto 0);
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
SIGNAL tmp11: std_logic_vector (7 downto 0);
SIGNAL tmp12 : std_logic_vector (7 downto 0);

COMPONENT load_mem IS
  PORT (rw1,ce1,rw2,ce2,ldfinish1, ldfinish2 : OUT std_logic;
        addr1, addr2 : OUT std_logic_vector (7 downto 0);
        data1, data2: OUT std_logic_vector (7 downto 0));
END COMPONENT;

FOR T1 : load_mem USE ENTITY WORK.load_mem(one);

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

BEGIN

T1: load_mem PORT MAP (rw1,ce1,rw2,ce2,ldfinish1,ldfinish2,addr1,addr2,data1,data2);
T2: mem264 PORT MAP (rw1,ce1,addr1,tmp1,tmp2,tmp3,tmp4,tmp5,tmp6,data1);     -- operation memory
T3: mem264 PORT MAP (rw2,ce2,addr2,tmp7,tmp8,tmp9,tmp10,tmp11,tmp12,data2);  -- data memory

PROCESS

    VARIABLE calcaddr : integer;
    VARIABLE stdlogaddr : std_logic_vector(7 downto 0);
BEGIN
WAIT UNTIL (ldfinish1 = '0');
  IF (ldfinish1 = '0' and ldfinish2 = '0') THEN

    ce1 <= '0';
    ce2 <= '0';
    rw1 <= '0';
    rw2 <= '0';

    wait for 10 ns;

    for j in 0 to 255 LOOP

      calcaddr := j;
      stdlogaddr := int_slog8(calcaddr);
        WAIT FOR 10 ns;
        addr1 <= stdlogaddr;
        WAIT FOR 10 ns;
        ce1 <= '1';
        WAIT FOR 20 ns;
        ce1 <= '0';
        addr1 <= "ZZZZZZZZ";
        WAIT FOR 10 ns;
    END LOOP;

  END IF;
END PROCESS;
WAIT;
END Behave;


