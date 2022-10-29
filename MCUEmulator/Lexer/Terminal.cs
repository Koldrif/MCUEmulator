using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MCUEmulator.Lexer
{
    /// <summary>
    /// Представление терминала.
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// Конструктор терминала
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="pattern">Регулярка</param>
        public Terminal(string Name, string pattern)
        {
            this.Name = Name;
            this.Pattern = pattern;
        }

        /// <summary>
        /// Имя терминала.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Регулярное выражение, которое соответсвует данному
        /// терминалу.
        /// </summary>
        public readonly string Pattern;

        /// <summary>
        /// Поиск по регулярным выражениям
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Matches(string str)
        {
            return Regex.IsMatch(str, Pattern);
        }

        /// <summary>
        /// Определяет, является ли текущий объект эквивалентным входому.
        /// Стоит отметить, что идёт сравнение только по именам, так как
        /// предполагается, что имена уникальны.
        /// </summary>
        /// <param name="other">Объект, который сравнивается
        /// с текущим. Если отправить null, то функция вернёт
        /// <code>false</code>.</param>
        /// <returns><code>true</code>, если объекты эквивалентны.
        /// Иначе - <code>false</code>.</returns>
        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (!(other is Terminal))
                return false;
            return Name.Equals(((Terminal)other).Name);
        }

        public override int GetHashCode()
        {
            // 539060726 - visual studio сгенерировала.
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name.ToString());
        }
        /// <summary>
        /// Вывод в консоль
        /// </summary>
        /// <returns>Терминал</returns>
        public override string ToString()
        {
            return $"{Name}: \"{Pattern}\"";
        }
    }
}