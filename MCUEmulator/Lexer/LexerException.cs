using System.Runtime.Serialization;

namespace MCUEmulator.Lexer;

public class LexerException : Exception
{
    public LexerException() : base() {}
    
    public LexerException(string message) : base(message) {}

    public LexerException(string message, Exception innerException) : base(message, innerException) {}
    
    public LexerException(SerializationInfo info, StreamingContext context) : base(info,context){}
}