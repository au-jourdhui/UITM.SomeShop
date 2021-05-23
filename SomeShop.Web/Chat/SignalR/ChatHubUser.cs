namespace SomeShop.Web.Chat.SignalR
{
    public class ChatHubUser
    {
        public ChatHubUser(IdentifierType identifierType, string identifier, string connectionId)
        {
            IdentifierType = identifierType;
            Identifier = identifier;
            ConnectionId = connectionId;
        }
        
        public IdentifierType IdentifierType { get; }
        public string Identifier { get; }
        public string ConnectionId { get; }
    }
}