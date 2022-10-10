using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MCUEmulator.Lexer;
/// <summary>
/// Представление теримнала
/// </summary>
public class Terminal
{   
    
    /// <summary>
    /// Создание анонимного терминала.
    /// В отличии от обычного, в нём отсутсвует регулярное выражение,
    /// а приоритет самый низкий (<see cref="uint.MaxValue"/>)
    /// </summary>
    /// <param name="Name">Имя терминала.</param>
    public Terminal(string Name)
        : this(Name, (Regex)null, uint.MaxValue) { }

    /// <summary>
    /// Создание экземпляра терминала.
    /// </summary>
    /// <param name="Name">Имя терминала.</param>
    /// <param name="regularExpression">Регулярное
    /// выражение терминала.</param>
    /// <param name="priority"></param>
    public Terminal(string Name, string regularExpression, uint priority = uint.MaxValue / 2)
        : this(Name, new Regex(regularExpression ?? throw new ArgumentNullException(), RegexOptions.Multiline), priority) { }

    /// <summary>
    /// Создание экземпляра терминала
    /// </summary>
    /// <param name="Name">Имя терминала</param>
    /// <param name="regularExpression">Регулярное выражение терминала</param>
    /// <param name="priority"></param>
    public Terminal(string Name, Regex regularExpression, uint priority = uint.MaxValue / 2)
    {
        this.Name = Name ?? throw new ArgumentNullException();
        this.RegularExpression = regularExpression;
        this.priority = priority;
    }

    /// <summary>
    /// Определяет, является ли текущий объект эквивалетным входному
    /// Сравнение идет только по именам (предполагается, что имена уникальны)
    /// </summary>
    /// <param name="other">Объект, который сравнивается с текущим</param>
    /// <returns><code>true</code>, если объекты эквиваленты
    /// иначе - <code>false</code></returns>
    public override bool Equals(object other)
    {
        if (other == null) return false;
        if (!(other is Terminal)) return false;
        return Name.Equals(((Terminal)other).Name);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<string>.Default.GetHashCode(Name);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(Name);
        if (RegularExpression != null)
        {
            sb.Append(": \"");
            sb.Append(RegularExpression.ToString());
            sb.Append('\"');
        }

        return sb.ToString();
    }
    
    /// <summary>
    /// Сравнение приоритетов левого и правого терминала.
    /// </summary>
    /// <param name="left">Левый терминал.</param>
    /// <param name="right">Правый терминал.</param>
    /// <returns>True, если у левого приоритет такой же, как у right.
    /// Иначе - false.</returns>
    public static bool PriorityEquals(Terminal left, Terminal right)
        => left.priority == right.priority;
    
    /// <summary>
    /// Сравнение приоритетов левого и правого терминала.
    /// </summary>
    /// <param name="left">Левый терминал.</param>
    /// <param name="right">Правый терминал.</param>
    /// <returns>True, если у левого приоритет выше (ближе к 0), чем у right.
    /// Иначе - false.</returns>
    public static bool operator >(Terminal left, Terminal right)
        => left.priority < right.priority;
    
    /// <summary>
    /// Сравнение приоритетов левого и правого терминала.
    /// </summary>
    /// <param name="left">Левый терминал.</param>
    /// <param name="right">Правый терминал.</param>
    /// <returns>True, если у левого приоритет ниже (дальше к 0), чем у right.
    /// Иначе - false.</returns>
    public static bool operator <(Terminal left, Terminal right)
        => left.priority > right.priority;
    
    /// <summary>
    /// Регулярное выражение, которое соответствует данному терминалу
    /// </summary>
    public readonly Regex RegularExpression;
    
    /// <summary>
    /// Имя терминала
    /// </summary>
    public readonly string Name;
    
    /// <summary>
    /// Приоритет терминала
    /// </summary>
    public readonly uint priority;
}