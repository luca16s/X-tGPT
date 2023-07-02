
namespace XetAPI
{
    using Azure.AI.OpenAI;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using XetAPI.Model;
    using XetAPI.Model.Classification;
    using XetAPI.Model.Embedding;

    public class Program
    {
        public const string CORS = "POLICY";

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddUserSecrets<Program>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS,
                    policy => {
                        policy
                        .WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CORS);
            app.UseAuthorization();
            app.UseHttpsRedirection();

            IConfigurationSection apiKey = app.Configuration.GetSection("OpenAPIKey");
            OpenAIClient client = new(apiKey.Value, new OpenAIClientOptions());
            Servico servico = new(client);

            app.MapPost("chat-com-contexto", async ([FromBody] ConversationModel model) =>
            {
                model.Answer = await servico.AskQuestionToChatAsync(model);

                return model;
            });

            app.MapPost("completions-com-contexto", async ([FromBody] ConversationModel model) =>
            {
                model.Answer = await servico.AskQuestionToCompletionsAsync(model);

                return model;
            });

            app.MapPost("chat-sem-contexto", async ([FromBody] ConversationModel model) =>
            {
                model.Answer = await servico.AskQuestionToChatAsync(model, false);

                return model;
            });

            app.MapPost("completions-sem-contexto", async ([FromBody] ConversationModel model) =>
            {
                model.Answer = await servico.AskQuestionToCompletionsAsync(model, false);

                return model;
            });

            app.MapPost("emb", async ([FromBody] ConversationModel model) =>
            {
                await servico.GetEmb(model);

                return model;
            });

            app.MapPost("local", async ([FromBody] ConversationModel model) =>
            {
                model.Answer = "4.2 milhões de barris\r\n";

                EmbeddingModel embAnswerModel = new()
                {
                    Model = "sentence-transformers/all-MiniLM-L6-v2",
                    Texts = new List<string>
                    {
                        model.Answer
                    }
                };
                EmbeddingModel embContextModel = new()
                {
                    Model = "sentence-transformers/all-MiniLM-L6-v2",
                    Texts = new List<string>
                    {
                        Constantes.TEXT
                    }
                };
                EmbeddingModel embQuestionModel = new()
                {
                    Model = "sentence-transformers/all-MiniLM-L6-v2",
                    Texts = new List<string>
                    {
                        model.Question
                    }
                };

                EmbeddingResponse? embAnswerResult = await servico.GetEmbeddings(embAnswerModel);
                EmbeddingResponse? embContextResult = await servico.GetEmbeddings(embContextModel);
                EmbeddingResponse? embQuestionResult = await servico.GetEmbeddings(embQuestionModel);

                double cosSimilarityAnswer = servico.GetCosSimilarity(embContextResult, embAnswerResult);
                double cosSimilarityQuestion = servico.GetCosSimilarity(embContextResult, embQuestionResult);

                ClassificationModel classificationModel = new()
                {
                    Question = model.Question,
                    Pipeline = "zero-shot-classification",
                    Labels = new string[] { "outros", "petróleo" },
                    Model = "MoritzLaurer/mDeBERTa-v3-base-mnli-xnli",
                };

                ClassificationResponse? questionClassification = await servico.GetClassification(classificationModel);
                ClassificationResponse? answerClassification = await servico.GetClassification(classificationModel);

                Console.WriteLine("/local");
                Console.WriteLine("---------");
                Console.WriteLine(model.Question);
                Console.WriteLine($"Similarity: {cosSimilarityQuestion} - Classification: {questionClassification?.Labels[0]}");
                Console.WriteLine("---------");
                Console.WriteLine(model.Answer);
                Console.WriteLine($"Similarity: {cosSimilarityAnswer} - Classification: {answerClassification?.Labels[0]}");
                Console.WriteLine("---------");

                return model;
            });

            app.Run();
        }
    }
}