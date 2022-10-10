namespace MCUEmulator.Lexer;
/// <summary>
/// Описание поиска терминалов
/// </summary>
public class SearchTerminal
{
    private SearchTerminal() : this(new List<Terminal>()) { }

    private SearchTerminal(IList<Terminal> Terminals, uint Count = 0)
    {
        this.Count = Count;
        this.Terminals = Terminals ?? throw new ArgumentNullException();
    }
    
    /// <summary>
    /// Найденные терминалы
    /// </summary>
    public IList<Terminal> Terminals { get; }
    
    /// <summary>
    /// Количество найденных
    /// </summary>
    public uint Count { private set; get; }
}