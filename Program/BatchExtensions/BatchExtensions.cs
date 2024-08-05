using System.Collections.Frozen;

namespace Nixill.OBSWS.BatchExtensions;

public static class RequestBatchExtensions
{
  public static async Task<IDictionary<T, V>> SelectOBSResults<T, V>(this IEnumerable<T> inputs,
    OBSClient client, Func<T, OBSRequest> requestMapper, Func<OBSRequestResponse, V> resultMapper,
    int timeoutOffset = 15, bool haltOnFailure = false,
    RequestBatchExecutionType executionType = RequestBatchExecutionType.SerialRealtime,
    Predicate<OBSRequestResponse>? resultCondition = null)
    where T : notnull where V : notnull
  {
    var requests = inputs.Select(i => (Input: i, Request: requestMapper(i)));

    OBSRequestBatch batch = new OBSRequestBatch(requests.Select(i => i.Request),
      haltOnFailure: haltOnFailure, executionType: executionType);
    OBSRequestBatchResult response = await client.SendBatchRequest(batch, timeoutOffset);

    var results = requests.Join(response,
      rq => rq.Request.RequestID,
      rp => rp.OriginalRequest.RequestID,
      (rq, rp) => new KeyValuePair<T, OBSRequestResponse>(rq.Input, rp)
    );

    if (resultCondition != null) results = results.Where(kvp => resultCondition(kvp.Value));

    return results.Select(res => new KeyValuePair<T, V>(res.Key, resultMapper(res.Value))).ToFrozenDictionary();
  }
}