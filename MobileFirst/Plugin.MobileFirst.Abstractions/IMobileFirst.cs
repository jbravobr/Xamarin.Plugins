using System.Threading.Tasks;
using Worklight;

namespace Plugin.MobileFirst.Abstractions
{
  /// <summary>
  /// Interface for MobileFirst
  /// </summary>
  public interface IMobileFirst
  {
        void Init(IWorklightClient wlc);
        Task<WorklightResult> ConnectAsync();
        Task<WorklightResult> RestInvokeAsync(string adapterName, string adapterProcedureName, string methodType);
        Task<WorklightResult> InvokeAsync(string adapterName, string adapterProcedureName);
        Task<WorklightResult> SendActivityAsync(string data);
        Task<WorklightResult> SubscribeAsync();
        Task<WorklightResult> UnSubscribeAsync();
  }
}
