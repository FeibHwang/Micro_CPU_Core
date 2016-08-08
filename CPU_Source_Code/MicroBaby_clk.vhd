LIBRARY IEEE;
USE IEEE.std_logic_1164.all;
ENTITY clkdrv IS
  PORT(mclk : IN std_logic;
       clk  : OUT std_logic);
END clkdrv;

ARCHITECTURE one OF clkdrv IS
  SIGNAL iclk,iclk2 : std_logic := '0' ;
BEGIN
  PROCESS(mclk,iclk)
  BEGIN
    IF (mclk='1' and mclk'event) THEN iclk <= NOT iclk; END IF;
    IF (iclk='1' and iclk'event) THEN iclk2 <= NOT iclk2; END IF;
    clk <= iclk2;
  END PROCESS;
END one;

--  clk test bentch--
LIBRARY IEEE;
USE IEEE.std_logic_1164.all;

ENTITY CLK_TST IS
END CLK_TST;

ARCHITECTURE Ckbehave of CLK_TST IS
signal system_clk : std_logic;
signal opt_clk : std_logic;

COMPONENT clkdrv IS
  PORT(mclk : IN std_logic;
       clk  : OUT std_logic);
END COMPONENT;

for T4 : clkdrv use entity work.clkdrv(one);

BEGIN

T4: clkdrv port map (system_clk,opt_clk);

process
begin
system_clk <= '0';
wait for 5 ns;
system_clk <= '1';
wait for 5 ns;
END process;


END ARCHITECTURE;
