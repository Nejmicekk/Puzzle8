using System.Runtime.Serialization;

namespace Puzzle8UI.Logic.Exceptions;

public class MatrixException : Exception
{
    public MatrixException() {}
    protected MatrixException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    public MatrixException(string? message) : base(message) {}
    public MatrixException(string? message, Exception? innerException) : base(message, innerException) {}
}