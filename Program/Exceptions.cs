namespace Nixill.OBSWS;

public class RequestFailedException : Exception
{
  public readonly RequestStatus StatusCode;
  public readonly string? Comment;
  public readonly OBSRequest Request;

  public RequestFailedException(OBSRequest request, RequestStatus code) : this(request, code, null) { }

  public RequestFailedException(OBSRequest request, RequestStatus code, string? comment) : base($"{code}: {comment ?? "(No comment)"}")
  {
    Request = request;
    StatusCode = code;
    Comment = comment;
  }
}

public class RequestTimedOutException : Exception
{
  public readonly OBSRequest Request;

  public RequestTimedOutException(OBSRequest request) : base($"Request {request.RequestID} timed out")
  {
    Request = request;
  }
}

public class RequestBatchTimedOutException : Exception
{
  public readonly OBSRequestBatch Requests;

  public RequestBatchTimedOutException(OBSRequestBatch requests) : base($"Request batch {requests.ID} timed out")
  {
    Requests = requests;
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

public class OBSDisconnectedException : Exception { }