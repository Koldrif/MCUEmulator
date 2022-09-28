namespace MCUEmulator.Lexer;

/*
 * Tokens:
 *   Commands:
 *     [COMMOV]
 *     - MOV
 *     [COMSUB}
 *     - SUB
 *     ...
 *   OPERAND:
 *     [OPREGISTER]
 *     - r.. - регистр
 *     [OPMEMORY]
 *     - 0x.. - ячейка памяти
 *     [OPCONSTANT]
 *     - b... \ 
 *     - d... -  константы
 *     - h... /
 *     ...
 *
 *    [NEWLINE]
 *     - \n
 *
 *    [EOF]
 *     - \0
 *
 *    [SEPARATOR]:
 *     - ,
 *
 * TODO
 *  1. Операнды разбить на токены регистры, ячейка памяти, константа
 *  2. Запихнуть мапу лексема - регулярка
 *  
 */
public class Lexer
{
    
}