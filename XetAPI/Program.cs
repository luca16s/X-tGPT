
namespace XetAPI
{
    using Azure.AI.OpenAI;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using XetAPI.Model;
    using XetAPI.Model.Classification;

    public class Program
    {
        public const string CORS = "POLICY";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CORS);
            app.UseAuthorization();
            app.UseHttpsRedirection();

            var apiKey = app.Configuration.GetSection("OpenAPIKey");
            OpenAIClient client = new(apiKey.Value, new OpenAIClientOptions());
            var servico = new Servico(client);

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

            app.MapPost("classify", async ([FromBody] ConversationModel model) =>
            {
                var classification = new ClassificationModel
                {
                    Question = model.Question,
                    Pipeline = "zero-shot-classification",
                    Labels = new string[] { "outros", "petróleo" },
                    Model = "MoritzLaurer/mDeBERTa-v3-base-mnli-xnli",
                };

                var questionClassification = await servico.GetClassification(classification);

                classification.Question = await servico.AskQuestionToChatAsync(model);

                var answerClassification = await servico.GetClassification(classification);

                return string.Empty;
            });

            app.Run();
        }
    }
}