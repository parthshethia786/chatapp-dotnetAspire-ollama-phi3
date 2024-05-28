using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

namespace AspirePhi3App.Web.Components.Chatbot
{
    public class CustomChatCompletionService : IChatCompletionService
    {
        public string ModelUrl { get; set; }
        public string ModelName { get; set; }

        public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

        public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            var ollama = new OllamaApiClient(ModelUrl, ModelName);

            var chat = new Chat(ollama, _ => { });

            foreach (var message in chatHistory)
            {
                if (message.Role == AuthorRole.Assistant)
                {
                    await chat.SendAs(OllamaSharp.Models.Chat.ChatRole.Assistant, message?.Content ?? string.Empty);
                    continue;
                }
            }

            var lastMessage = chatHistory.LastOrDefault();

            var history = (await chat.Send(lastMessage?.Content ?? string.Empty, CancellationToken.None)).ToArray();

            var chatResponse = history.Last().Content;

            chatHistory.AddAssistantMessage(chatResponse);

            return chatHistory;
        }

        public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}