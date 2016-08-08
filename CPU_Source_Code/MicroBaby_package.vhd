LIBRARY ieee;
USE ieee.std_logic_1164.all;
PACKAGE mbspt IS
  FUNCTION bin8_2_int (valin : bit_vector(7 downto 0)) RETURN integer;
  FUNCTION bin8_2_int (valin : std_logic_vector(7 downto 0)) RETURN integer;
  FUNCTION bin8_inc (valin : std_logic_vector(7 downto 0)) RETURN std_logic_vector;
  PROCEDURE bin8_add (vala,valb : IN std_logic_vector(7 downto 0);
                      cin : In std_logic;
                      valres : OUT std_logic_vector(7 downto 0);
                      cout : OUT std_logic);
  FUNCTION int_slog8 (valin : integer) RETURN std_logic_vector;
END mbspt;

PACKAGE BODY mbspt IS
  
  FUNCTION bin8_2_int (valin : bit_vector(7 downto 0)) RETURN integer IS
    VARIABLE cval : integer;
  BEGIN
    cval := 0;
    FOR i IN 0 to 7 LOOP
      IF (valin(i)='1') THEN cval := cval + 2**i; END IF;
    END LOOP;
    return(cval);
  END bin8_2_int;
  
  FUNCTION bin8_2_int (valin : std_logic_vector(7 downto 0)) RETURN integer IS
    VARIABLE cval : integer;
  BEGIN
    cval := 0;
    FOR i IN 0 to 7 LOOP
      IF (valin(i)='1') THEN cval := cval + 2**i; END IF;
    END LOOP;
    return(cval);
  END bin8_2_int;
  
  FUNCTION bin8_inc (valin : std_logic_vector(7 downto 0)) RETURN std_logic_vector IS
    VARIABLE bin8p1 : std_logic_vector(7 downto 0);
    VARIABLE ctemp : std_logic_vector(8 downto 0);
  BEGIN
    ctemp(0) := '0';
    FOR i IN 0 to 7 LOOP
      ctemp(i+1) := valin(i) AND ctemp(i);
      bin8p1(i) := valin(i) XOR ctemp(i);
    END LOOP;
    return("00000000");
    
    
  END bin8_inc; 
  
  PROCEDURE bin8_add (vala,valb : IN std_logic_vector(7 downto 0);
                      cin : In std_logic;
                      valres : OUT std_logic_vector(7 downto 0);
                      cout : OUT std_logic) IS
    VARIABLE ctemp : std_logic_vector(8 downto 0);
  BEGIN
    ctemp(0) := cin;
    FOR i IN 0 to 7 LOOP
      ctemp(i+1) := (vala(i) AND valb(i)) OR (vala(i) AND ctemp(i)) OR
                       (valb(i) AND ctemp(i));
      valres(i) := vala(i) XOR valb(i) XOR ctemp(i);
    END LOOP;
    cout := ctemp(8);
  END bin8_add;
  
  FUNCTION int_slog8 (valin : integer) RETURN std_logic_vector IS
    VARIABLE tval : integer;
    VARIABLE rtnval : std_logic_vector(7 downto 0);
  BEGIN
    tval := valin;
    FOR i IN 0 to 7 LOOP
      IF (tval REM 2 = 0) THEN rtnval(i) := '0';
                          ELSE rtnval(i) := '1';
      END IF;
      tval := tval/2;
    END LOOP;
    return(rtnval);
  END;
  
END mbspt;