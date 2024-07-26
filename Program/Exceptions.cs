namespace Nixill.OBSWS;

public class RequestFailedException : Exception
{
  public readonly int StatusCode;
  public readonly string Comment;

  public RequestFailedException(int code, string comment) : base($"{code}: {comment}")
  {
    StatusCode = code;
    Comment = comment;
  }
}

public class RequestTimedOutException : Exception
{
  public readonly string Guid;

  public RequestTimedOutException(string guid) : base($"Request {guid} timed out")
  {
    Guid = guid;
  }
}

public class MissingFieldException : Exception
{
  public readonly string Field;

  public MissingFieldException(string field) : base($"Missing required field '{field}'")
  {
    Field = field;
  }
}