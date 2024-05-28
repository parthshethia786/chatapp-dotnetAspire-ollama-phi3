using System.Security.Claims;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AspireApp.WebApp.Chatbot;

public class ChatState
{
    private readonly ILogger _logger;
    private readonly Kernel _kernel;

    public ChatState(ClaimsPrincipal user, Kernel kernel, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(typeof(ChatState));

        _kernel = kernel;

        Messages = new ChatHistory("""
            You are an AI customer service agent for the online company Microsoft.
            You NEVER respond about topics other than Microsoft.
            Your job is to answer customer questions about products in the Microsoft catalog.
            Microsoft primarily sells products and services related to AI, security, customer productivity, etc.
            You try to be concise and only provide longer responses if necessary.
            If someone asks a question about anything other than Microsoft, its catalog, or their account,
            you refuse to answer, and you instead ask if there's a topic related to Microsoft you can assist with.
            """);
        Messages.AddAssistantMessage("Hi! I'm SASsy. How can I help?");
    }

    public ChatHistory Messages { get; }

    public async Task AddUserMessageAsync(string userText, Action onMessageAdded)
    {
        Messages.AddUserMessage(userText);
        onMessageAdded();

        try
        {
            var chat = _kernel.GetRequiredService<IChatCompletionService>("CustomChatCompletionService");

            var result = await chat.GetChatMessageContentsAsync(Messages);
        }
        catch (Exception e)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(e, "Error getting chat completions.");
            }
            Messages.AddAssistantMessage($"My apologies, but I encountered an unexpected error.");
        }
        onMessageAdded();
    }
}