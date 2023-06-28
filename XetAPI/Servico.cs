namespace XetAPI
{
    using Accord.Math.Distances;

    using Azure;
    using Azure.AI.OpenAI;

    using System.Text;
    using System.Threading.Tasks;

    using XetAPI.Model;

    public class Servico
    {
        private readonly OpenAIClient client;
        private static float[]? embeddingContext;

        public Servico(
            OpenAIClient client
        ) => this.client = client;

        public async Task<string> AskQuestionToChatAsync(
            ConversationModel model,
            bool shouldUseBehavior = true
        )
        {
            var behavior = string.Format(Constantes.BEHAVIOR, Constantes.JSON, Constantes.ERROR, Constantes.SAIDA);

            switch (model.Context)
            {
                case EContext.JSON:
                    behavior = string.Format(Constantes.BEHAVIOR, Constantes.JSON, Constantes.ERROR, Constantes.SAIDA);
                    break;
                case EContext.TEXT:
                    behavior = string.Format(Constantes.BEHAVIOR, Constantes.TEXT, Constantes.ERROR, Constantes.SAIDA);
                    break;
            }

            var chatBehavior = shouldUseBehavior ?
                new ChatMessage(ChatRole.System, behavior) :
                new ChatMessage(ChatRole.System, string.Empty);

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Temperature = 0,
                PresencePenalty = 2,
                FrequencyPenalty = 1,
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

            StringBuilder sb = new();

            await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
            {
                await foreach (ChatMessage message in choice.GetMessageStreaming())
                {
                    sb.Append(message.Content);
                }
                sb.AppendLine();
            }

            return sb.ToString();

            // Formatar uma resposta de forma específica
            // campo informando se estava ou não no contexto
        }

        public async Task<string> AskQuestionToCompletionsAsync(
            ConversationModel model,
            bool shouldUseBehavior = true
        )
        {
            var completionOptions = shouldUseBehavior ?
                new CompletionsOptions { User = $"{Constantes.BEHAVIOR} - {model.Question}?" } :
                new CompletionsOptions { User = $"{model.Question}?" };

            Response<Completions> completionsResponse = await client.GetCompletionsAsync(
                "text-davinci-003",
                completionOptions
            );

            string completion = completionsResponse.Value.Choices[0].Text;

            return completion;
        }


        public async Task GetEmb(
            ConversationModel model
        )
        {
            model.Question = $"{model.Question}. Não justifique sua resposta. Somente me dê informações precisas e mencionadas no contexto.";
            EmbeddingsOptions embeddingOptionsQuestion = new(model.Question);
            var embeddingQuestion = client.GetEmbeddings("text-embedding-ada-002", embeddingOptionsQuestion).Value.Data[0].Embedding.ToArray();

            if (embeddingContext is null)
            {
                EmbeddingsOptions embeddingOptionsContext = new(Constantes.TEXT);
                embeddingContext = client.GetEmbeddings("text-embedding-ada-002", embeddingOptionsContext).Value.Data[0].Embedding.ToArray();
            }

            var cos = new Cosine();

            var similarityPerguntaContexto = cos.Similarity(
                embeddingQuestion.Select(x => (double)x).ToArray(),
                embeddingContext.Select(x => (double)x).ToArray()
            );

            var retorno = await AskQuestionToChatAsync(model, true);

            EmbeddingsOptions embeddingOptionsResposta = new(retorno);
            var embeddingResposta = client.GetEmbeddings("text-embedding-ada-002", embeddingOptionsResposta).Value.Data[0].Embedding.ToArray();

            var similarityRespostaContexto = cos.Similarity(
                embeddingResposta.Select(x => (double)x).ToArray(),
                embeddingContext.Select(x => (double)x).ToArray()
            );

            Console.WriteLine("---------");
            Console.WriteLine(model.Question);
            Console.WriteLine(similarityPerguntaContexto);
            Console.WriteLine(retorno);
            Console.WriteLine(similarityRespostaContexto);
            Console.WriteLine("---------");

            return;
        }
    }
}
