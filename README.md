Below is the step-by-step guide to run the application locally.

**Prerequisites:**
1.	Visual Studio V17.10.0 and .NET 8.0 (for .NET Aspire applications)
2.	Ollama – available for Windows in preview mode

**Step 1:** 

Let’s download Ollama, run any model locally and interact with it using command line:

- Download Ollama from -> https://ollama.com/.
  - Ollama is an open-source tool that allows you to run, create, and share LLMs or SLMs locally. This gives you an ability to run these models with low infrastructure resources and in offline mode without internet.  
- Once the installation is complete, get the model locally which you wish to run using Ollama.
  - Learn more on model storage and change default location on Windows - https://github.com/ollama/ollama/blob/main/docs/faq.md#where-are-models-stored
- To get Microsoft’s model Phi-3, run below command from command prompt:
  - <code>ollama pull phi3:3.8b</code>
  - Check out all available models at https://ollama.com/library 
- Once the model files are downloaded, you can run the model using command:
  - <code>ollama run phi3:3.8b</code>
 
**Step 2:** 

Let’s create a sample .NET Aspire application:

- Open Visual Studio -> Create new project (example name AspireApp) -> Select .NET Aspire Starter Application template -> Select .NET 8.0 as target framework -> Enable – Configure for https
- Hit F5 and see the sample application running with default click counter and weather API modules. 

**Step 3:**

Let’s now use Semantic Kernel and OllamaSharp to call the locally running Phi-3 model:

- In the Web frontend project of the Aspire application - AspireApp.Web, install below NuGet packages:
  - <code>Microsoft.SemanticKernel</code> - .NET library that provides a set of interfaces to interact with language models for text generation or chat completion. 
  - <code>OllamaSharp</code> - .NET binding to interact with Ollama APIs. 
- Create a <code>CustomChatCompletionService</code> that inherits from <code>IChatCompletionService</code> of Semantic Kernel.
- Register the service in Program.cs of Web frontend project of the Aspire application.
- Create a <code>ChatState.cs</code> wrapper that will be invoked from chat UI and will talk to custom chat completion service.
- In <code>ChatState.cs</code>, we are initializing the <code>ChatHistory</code> with system message (initial prompt to the model). This can be customized based on business needs. 
- The chat context is preserved is ChatHistory every time user asks a new questions/sends a new prompt. Hence entire context is provided to the model while processing the latest prompt.
- The chat UI is inspired from the eShop application from Microsoft. You can copy the <code>Chatbot</code> folder from this repo under your Web project of the application - AspireApp.Web, under the Components folder. Add the <code>chat.png</code> image under <code>wwwroot</code> folder of AspireApp.Web. Hook the <code>ShowChatbotButton</code> component in <code>MainLayout.razor</code>. 
- Once everything is wired up, run the .NET Aspire application and test the chat application from the webapp localhost URL. (Ensure the model is running locally using Ollama – Step 1)

**References**:

- https://medium.com/@dpn.majumder/how-to-deploy-and-experiment-with-ollama-models-on-your-local-machine-windows-34c967a7ab0e
- https://techcommunity.microsoft.com/t5/educator-developer-blog/extending-semantic-kernel-using-ollamasharp-for-chat-and-text/ba-p/4104953
- https://www.kallemarjokorpi.fi/blog/rag-application-with-net-aspire-and-semantic-kernel.html
- https://github.com/Azure-Samples/eShop-AI-Lab-Build2024
