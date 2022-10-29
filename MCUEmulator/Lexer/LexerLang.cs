using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;


namespace MCUEmulator.Lexer
{
    public class LexerLang
    {
        /// <summary>
        /// Создание экземпляра обработчика.
        /// </summary>
        public LexerLang()
        {
            avalibleTerminals = new List<Terminal>(
            new Terminal[]
            {
                new Terminal("COMMAND_MOV", "MOV"),     //Пересылка данных
                new Terminal("COMMAND_ADD", "ADD"),     //Сложение
                new Terminal("COMMAND_SUB", "SUB"),     //Вычитание
                new Terminal("COMMAND_MUL", "MUL"),     //Умножение
                new Terminal("COMMAND_DIV", "DIV"),     //Деление
                new Terminal("COMMAND_INC", "INC"),     //Инкремент на 1
                new Terminal("COMMAND_DEC", "DEC"),     //Декремент на 1
                new Terminal("COMMAND_COMPARE", "CMP"),     //Сравнение двух операндов
                new Terminal("COMMAND_CMPXCHG", "CMPXCHG"), //Сравнение и обмен
                new Terminal("COMMAND_JMP", "JMP"),         //Переход
                new Terminal("COMMAND_JTCMP", "JTCMP"),     //Переход с условием
                new Terminal("COMMAND_BT", "BT"),           //Проверка бита
                new Terminal("COMMAND_BTR", "BTR"),         //Проверка бита и сброс в 0
                new Terminal("COMMAND_BTS", "BTS"),         //Проверка бита и устновка в 1
                new Terminal("REG", @"[\D\S]{2}x\s?"),      //Регистры
                new Terminal("INDEX_SOURCE", "ESI"),        //Регистры
                new Terminal("LABEL", @"\#[\D\S]*[0-9]"),  //Метка?
                
                //терминалы для парсера
                new Terminal("COM", ","),
                new Terminal("COMMA", ";"),
                new Terminal("COLON", ":"),
                new Terminal("L_SB", "\\["),
                new Terminal("R_SB", "\\]"),
                new Terminal("CH_SPACE", " ")
            }
            );
        }

        public void Run(List<string> args)
        {
            line = 0;
            for (int i = 1; i < args.Count; ++i)
            {
                string input = args[i];
                while (input != "")
                {
                    if (input[0] == '\n')
                    {
                        ++line;
                    }

                    Token token = NextToken(input);
                    tokens.Insert(tokens.Count, token);
                    Console.WriteLine(token);
                    input = input.Substring(token.Value.Length, input.Length);
                }
            }

            Terminal term = new Terminal("EOF", "");
            Token tok = new Token(term, new string(""));
            tokens.Insert(tokens.Count, tok);
        }

        public void RunFile()
        {
            StreamReader sr = new StreamReader("LexerTest.txt");
            line = 0;
            string? buffer = new string("");
            List<string?> lines = new List<string?>();
            string? strLine;
            while ((strLine = sr.ReadLine()) != null)
            {
                //buffer = sr.ReadLine();
                lines.Add(strLine);
            }

            for (int i = 0; i < lines.Count; ++i)
            {
                string input = new string(lines[i]);
                while (input != "")
                {
                    while (input[0] == '\n')
                    {
                        ++line;
                    }
                    Token token = NextToken(input);
                    tokens.Add(token);
                    input = input.Substring(token.Value.Length);
                }

                line++;
            }
            Terminal term = new Terminal("EOF", "");
            Token tok = new Token(term, new string(""));
            tokens.Insert(tokens.Count, tok);
            
            Print(tokens);
        }
        
        /// <summary>
        /// Поиск токена
        /// </summary>
        /// <param name="avalibleTerminals">Набор разрешённых терминалов.</param>
        public Token NextToken(string input)
        {
            string buffer = new string("");
            buffer += input[0];
            try
            {
                while (!TerminalMatches(buffer))
                {
                    while (!TerminalMatches(buffer) && buffer.Length != input.Length)
                    {
                        buffer+=input[buffer.Length];
                    }

                    if (buffer.Length != input.Length || !TerminalMatches(buffer))
                        buffer.Remove(buffer.Length-1, 1);
                    
                    if (!TerminalMatches(buffer))
                    {
                        throw new LexerException($"Неизвестный символ в строке {line.ToString()} Последние удачные: {string.Join(", ", tokens)}");
                    }
                    List<Terminal> terminal = SearchInTerminals(buffer);
                    Token tok = new Token(GetTerminal(terminal), buffer);
                    return tok;
                }

                if (TerminalMatches(buffer))
                {
                    List<Terminal> terminal = SearchInTerminals(buffer);
                    Token tok = new Token(GetTerminal(terminal), buffer);
                    return tok;
                }
                else
                {
                    throw new LexerException($"Неизвестный символ в строке {line.ToString()} Последние удачные: {string.Join(", ", tokens)}");
                }
            }
            catch(LexerException e)
            {
                Console.WriteLine(e + "\n" + e.StackTrace);
            }

            return null!;
        }
        /// <summary>
        /// Вывод
        /// </summary>
        /// <param name="tokens"></param>
        void Print(List<Token> tokens)
        {
            Console.WriteLine("Lexer: success\n\nTokens:\n");
            foreach (var VARIABLE in tokens)
            {
                Console.WriteLine(VARIABLE.ToString());
            }
        }
        
        /// <summary>
        /// Терминал
        /// </summary>
        /// <param name="terminals"></param>
        /// <returns>Терминал</returns>
        Terminal GetTerminal(List<Terminal> terminals)
        {
            return terminals[0];
        }
        
        /// <summary>
        /// Поиск Терминала
        /// </summary>
        /// <param name="expression">Выражение терминала</param>
        /// <returns>Найденный терминал</returns>
        public List<Terminal> SearchInTerminals(string expression)
        {
            List<Terminal> matched = new List<Terminal>();
            foreach (Terminal ter in avalibleTerminals)
            {
                if (ter.Matches(expression))
                {
                    matched.Add(ter);
                }
            }

            return matched;
        }
        
        /// <summary>
        /// Терминал найден
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>true - если найден, false - если нет</returns>
        public bool TerminalMatches(string expression)
        {
            return SearchInTerminals(expression).Count != 0;
        }
        
        /// <summary>
        /// Список поддерживаемых терминалов.
        /// </summary>
        private readonly List<Terminal> avalibleTerminals;

        /// <summary>
        /// Номер строки
        /// </summary>
        private int line;

        /// <summary>
        /// Список токенов
        /// </summary>
        public List<Token> tokens = new();
        
    }
}

