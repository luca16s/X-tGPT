﻿namespace XetAPI
{
    using Azure;
    using Azure.AI.OpenAI;

    using System.Text;
    using System.Threading.Tasks;
    using XetAPI.Model;

    public class Servico
    {
        private readonly OpenAIClient client;

        public Servico(
            OpenAIClient client
        ) => this.client = client;

        public async Task<string> AskQuestionToChatAsync(
            ConversationModel model,
            bool shouldUseBehavior = true
        )
        {
            var behavior = string.Empty;

            switch (model.Context)
            {
                case EContext.JSON:
                    behavior = string.Format(Constantes.behavior, Constantes.JSON);
                    break;
                case EContext.TEXT:
                    behavior = string.Format(Constantes.behavior, Constantes.TEXT);
                    break;
            }

            var chatBehavior = shouldUseBehavior ?
                new ChatMessage(ChatRole.System, behavior) :
                new ChatMessage(ChatRole.System, string.Empty);

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                    chatBehavior,
                    new ChatMessage(ChatRole.User, model.Question),
                }
            };

            Response<StreamingChatCompletions> response = await client.GetChatCompletionsStreamingAsync(
                deploymentOrModelName: "gpt-3.5-turbo",
                chatCompletionsOptions
            );

            using StreamingChatCompletions streamingChatCompletions = response.Value;

            StringBuilder retorno = new();

            await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
            {
                await foreach (ChatMessage message in choice.GetMessageStreaming())
                {
                    retorno.Append(message.Content);
                }
            }

            return retorno.ToString();
        }

        public async Task<string> AskQuestionToCompletionsAsync(
            ConversationModel model,
            bool shouldUseBehavior = true
        )
        {
            var completionOptions = shouldUseBehavior ?
                new CompletionsOptions { User = $"{Constantes.behavior} - {model.Question}?" } :
                new CompletionsOptions { User = $"{model.Question}?" };

            Response<Completions> completionsResponse = await client.GetCompletionsAsync(
                "text-davinci-003",
                completionOptions
            );

            string completion = completionsResponse.Value.Choices[0].Text;

            return completion;
        }
    }
}