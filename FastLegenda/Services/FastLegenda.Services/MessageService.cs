using FastLegenda.Services.Interfaces;

namespace FastLegenda.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
