namespace Nixill.OBSWS.Extensions;

public static class CastResult
{
  public static T AsSingle<T>(this OBSRequestResult result) => (T)(OBSSingleValueResult<T>)result;
  public static IEnumerable<T> AsList<T>(this OBSRequestResult result) => (IEnumerable<T>)(OBSListResult<T>)result;
}