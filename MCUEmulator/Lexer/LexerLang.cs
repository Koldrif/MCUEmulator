using System.Text;
using System.Text.RegularExpressions;

namespace MCUEmulator.Lexer;

public class LexerLang
{
    public LexerLang()
    {
        avalibleTerminals = new List<Terminal>()
        {
            //TODO Постараться избавиться от захардкоженных комманд 
            new Terminal("COMMAND",@"^[\S\D]{3,6}\s+"),
            new Terminal("DIGIT", @"\d+[\s;]"),
            // For hex 0x[\da-fA-F]+ 
            new Terminal("COM", @"\,"),
            new Terminal("COMMA", @"\;"),
            /*new Terminal("L_SB", "^[$"),
            new Terminal("R_SB", "^]$")*/
            new Terminal("REGISTER", @"[\D\S]{1}rx\s?"),
            //пробел для парсера
            new Terminal("CH_SPACE", @"\s")
        };
    }
    /// <summary>
    /// Список поддерживаемых терминалов
    /// </summary>
    public readonly List<Terminal> avalibleTerminals;

    /// <summary>
    /// Создание экземпляра обработчика.
    /// </summary>
    /// <param name="avalibleTerminals">Набор разрешённых терминалов.</param>
    public LexerLang(IEnumerable<Terminal> avalibleTerminals)
    {
        this.avalibleTerminals = new List<Terminal>(avalibleTerminals ?? throw new ArgumentNullException());
    }
    
    public virtual List<Token> SearchTokens(StreamReader input)
    {
        if (input is null)
            throw new ArgumentNullException("BufferedStream input = null");
        List<Token> output = new List<Token>(); // Сюда запишем вывод.
        StringBuilder bufferList = new StringBuilder(); // Строка из файла.
        char[] buffer = new char[1]; // Сюда попадает символ перед тем, как попасть в строку.
        List<Terminal> termsFound; // Сюда помещаются подходящие терминалы к строке bufferList.

        // True, если последняя итерация была с добавлением элемента в output. Иначе - False.
        bool lastAdd = false;
        while (!input.EndOfStream || bufferList.Length != 0)
        {
            if (!lastAdd && !input.EndOfStream)
            {
                input.Read(buffer, 0, 1); // Чтение символа.
                bufferList.Append(buffer[0]); // Запись символа в строку.
            }
            lastAdd = false;
            // Получение списка подходящих терминалов:
            termsFound = SearchInTerminals(bufferList.ToString());

            // Ура, мы что-то, кажется, нашли.
            if (termsFound.Count <= 1 || input.EndOfStream)
            {
                if (termsFound.Count == 1 && !input.EndOfStream)
                    // Это ещё не конец файла и есть 1 прецидент. Ищем дальше.
                    continue;
                int last = char.MaxValue + 1;
                if (termsFound.Count == 0)
                {
                    last = bufferList[bufferList.Length - 1]; // Запоминаем последний символ.
                    bufferList.Length--; // Уменьшаем длинну списка на 1.
                    termsFound = SearchInTerminals(bufferList.ToString()); // Теперь ищем терминалы.
                }
                if (termsFound.Count != 1) // Ой, должен был остаться только один.
                {
                    if (termsFound.Count == 0)
                        throw new LexerException
                        ($"Количество подходящих терменалов не равно 1: {termsFound.Count}. Последние удачные: {string.Join(", ", output)}");
                    Terminal need = termsFound.First();
                    Terminal oldNeed = null;
                    bool unical = true; // True, если необходимый терминал имеет самый высокий приоритет.
                    for (int i = 1; i < termsFound.Count; i++)
                    {
                        if (termsFound[i] > need)
                        {
                            need = termsFound[i];
                            unical = true;
                        }
                        else if (Terminal.PriorityEquals(termsFound[i], need))
                        {
                            oldNeed = termsFound[i];
                            unical = false;
                        }
                    }
                    if (!unical)
                        throw new LexerException
                            ($"Количество подходящих терменалов не равно 1: {termsFound.Count}" +
                            $", возможно был конфликт между: {oldNeed} и {need}");
                    termsFound.Clear();
                    termsFound.Add(need);
                }
                // Всё идёт как надо
                // Добавим в результаты
                output.Add(
                    new Token(
                    termsFound.First(),
                    bufferList.ToString()
                    ));
                bufferList.Clear();
                lastAdd = true;
                if (last != char.MaxValue + 1)
                    bufferList.Append((char)last);
            }
        }
        return output;
    }

    public List<Terminal> SearchInTerminals(string expression)
    {
        List<Terminal> output = new List<Terminal>();
        foreach (Terminal ter in avalibleTerminals)
        {
            Match mat = ter.RegularExpression.Match(expression);
            if (mat.Length > 0 && mat.Value.Equals(expression))
            {
                output.Add(ter);
            }
        }

        return output;
    }
}