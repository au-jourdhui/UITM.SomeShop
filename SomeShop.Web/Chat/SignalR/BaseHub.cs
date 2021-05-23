using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SomeShop.Web.Chat.SignalR
{
    public class BaseHub : Hub
    {
        protected BaseHub()
        {
        }

        protected string ConnectionId => this.Context.ConnectionId;
        
        protected Task SendBack(string method, object arg, CancellationToken cancellationToken = default)
        {
            return this.Clients.Client(this.Context.ConnectionId).SendCoreAsync(method, new[] {arg}, cancellationToken);
        }

        protected Task SendBack(string method, CancellationToken cancellationToken = default, params object[] args)
        {
            return this.Clients.Client(this.Context.ConnectionId).SendCoreAsync(method, args, cancellationToken);
        }
    }
}