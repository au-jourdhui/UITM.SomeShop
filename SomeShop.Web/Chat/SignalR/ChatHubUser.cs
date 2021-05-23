namespace SomeShop.Web.Chat.SignalR
{
    public class ChatHubUser
    {
        public static ChatHubUser Empty = new();

        private ChatHubUser() { }
            
        public ChatHubUser(IdentifierType identifierType, string identifier, string name)
        {
            IdentifierType = identifierType;
            Identifier = identifier;
            Name = name;
        }
        
        public ChatHubUser(IdentifierType identifierType, string identifier, string name, string connectionId) 
            : this(identifierType, identifier, name)
        {
            ConnectionId = connectionId;
        }

        public IdentifierType IdentifierType { get; } = IdentifierType.Unknown;
        public string Identifier { get; }
        public string ConnectionId { get; }
        public string Name { get; }
    }
}