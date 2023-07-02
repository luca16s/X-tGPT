using Azure.AI.OpenAI;

using Microsoft.Extensions.Configuration;

IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>();

IConfigurationRoot configurationRoot = builder.Build();

IConfigurationSection apiKey = configurationRoot.GetSection("OpenAPIKey");

OpenAIClient client = new(apiKey.Value, new OpenAIClientOptions());

#region Texto
string context = @"
Nome do poço: Baleia Azul
Data de Início: 01/02/2019
Data de Fim: 15/11/2024
Tamanho do Poço: 2.5 milhões de barris
Localização: Golfo do México

Nome do poço: Tubarão Branco
Data de Início: 10/07/2020
Data de Fim: 30/09/2025
Tamanho do Poço: 3.8 milhões de barris
Localização: Mar do Norte

Nome do poço: Golfinho Saltador
Data de Início: 05/12/2017
Data de Fim: 20/08/2023
Tamanho do Poço: 1.9 milhões de barris
Localização: Costa do Alasca

Nome do poço: Baleia Jubarte
Data de Início: 18/03/2022
Data de Fim: 05/12/2027
Tamanho do Poço: 4.2 milhões de barris
Localização: Mar do Caribe

Nome do poço: Marlin Negro
Data de Início: 08/09/2016
Data de Fim: 25/06/2022
Tamanho do Poço: 2.1 milhões de barris
Localização: Golfo do México

Nome do poço: Tubarão-Martelo
Data de Início: 12/11/2019
Data de Fim: 01/07/2024
Tamanho do Poço: 3.5 milhões de barris
Localização: Mar do Norte

Nome do poço: Arraia Manta
Data de Início: 03/06/2018
Data de Fim: 18/02/2024
Tamanho do Poço: 1.6 milhões de barris
Localização: Costa da Austrália

Nome do poço: Tubarão-Tigre
Data de Início: 22/01/2021
Data de Fim: 10/10/2026
Tamanho do Poço: 4.9 milhões de barris
Localização: Mar do Norte

Nome do poço: Orca Assassina
Data de Início: 14/07/2017
Data de Fim: 29/03/2023
Tamanho do Poço: 2.3 milhões de barris
Localização: Golfo do México

Nome do poço: Golfinho Nariz-de-Garrafa
Data de Início: 29/05/2020
Data de Fim: 12/02/2026
Tamanho do Poço: 3.1 milhões de barris
Localização: Mar do Norte
";
#endregion

#region JSON
string json = @"
[
  {
    ""Nome do poço"": ""Baleia Azul"",
    ""Data de Início"": ""01/02/2019"",
    ""Data de Fim"": ""15/11/2024"",
    ""Tamanho do Poço"": ""2.5 milhões de barris"",
    ""Localização"": ""Golfo do México""
  },
  {
    ""Nome do poço"": ""Tubarão Branco"",
    ""Data de Início"": ""10/07/2020"",
    ""Data de Fim"": ""30/09/2025"",
    ""Tamanho do Poço"": ""3.8 milhões de barris"",
    ""Localização"": ""Mar do Norte""
  },
  {
    ""Nome do poço"": ""Golfinho Saltador"",
    ""Data de Início"": ""05/12/2017"",
    ""Data de Fim"": ""20/08/2023"",
    ""Tamanho do Poço"": ""1.9 milhões de barris"",
    ""Localização"": ""Costa do Alasca""
  },
  {
    ""Nome do poço"": ""Baleia Jubarte"",
    ""Data de Início"": ""18/03/2022"",
    ""Data de Fim"": ""05/12/2027"",
    ""Tamanho do Poço"": ""4.2 milhões de barris"",
    ""Localização"": ""Mar do Caribe""
  },
  {
    ""Nome do poço"": ""Marlin Negro"",
    ""Data de Início"": ""08/09/2016"",
    ""Data de Fim"": ""25/06/2022"",
    ""Tamanho do Poço"": ""2.1 milhões de barris"",
    ""Localização"": ""Golfo do México""
  },
  {
    ""Nome do poço"": ""Tubarão-Martelo"",
    ""Data de Início"": ""12/11/2019"",
    ""Data de Fim"": ""01/07/2024"",
    ""Tamanho do Poço"": ""3.5 milhões de barris"",
    ""Localização"": ""Mar do Norte""
  },
  {
    ""Nome do poço"": ""Arraia Manta"",
    ""Data de Início"": ""03/06/2018"",
    ""Data de Fim"": ""18/02/2024"",
    ""Tamanho do Poço"": ""1.6 milhões de barris"",
    ""Localização"": ""Costa da Austrália""
  },
  {
    ""Nome do poço"": ""Tubarão-Tigre"",
    ""Data de Início"": ""22/01/2021"",
    ""Data de Fim"": ""10/10/2026"",
    ""Tamanho do Poço"": ""4.9 milhões de barris"",
    ""Localização"": ""Mar do Norte""
  },
  {
    ""Nome do poço"": ""Orca Assassina"",
    ""Data de Início"": ""14/07/2017"",
    ""Data de Fim"": ""29/03/2023"",
    ""Tamanho do Poço"": ""2.3 milhões de barris"",
    ""Localização"": ""Golfo do México""
  },
  {
    ""Nome do poço"": ""Golfinho Nariz-de-Garrafa"",
    ""Data de Início"": ""29/05/2020"",
    ""Data de Fim"": ""12/02/2026"",
    ""Tamanho do Poço"": ""3.1 milhões de barris"",
    ""Localização"": ""Mar do Norte""
  }
]
";
#endregion

string behavior = @$"
    Utilize o seguinte contexto para responder a subsequente questão.

    CONTEXTO:
    """"""
    {json}
    """"""

    Somente responda perguntas que estão dentro do escopo do texto fornecido no CONTEXTO.
    No caso da pergunta estar fora do escopo do CONTEXTO fornecido, escreva: ""Não sei meu chapa."".
";

#region Embbeding
EmbeddingsOptions embeddingOptions = new(json);
Azure.Response<Embeddings> embedding = client.GetEmbeddings("text-embedding-ada-002", embeddingOptions);
using (FileStream file = File.Create(@$"C:\Users\lucag\source\repos\luca16s\{Guid.NewGuid()}"))
{
    using (StreamWriter writer = new(file))
    {
        foreach (float value in embedding.Value.Data[0].Embedding)
        {
            writer.Write(value);
        }
    }
}
Console.ReadLine();
#endregion

#region Chat
//var chatCompletionsOptions = new ChatCompletionsOptions()
//{
//    Messages =
//    {
//        new ChatMessage(ChatRole.System, behavior),
//        new ChatMessage(ChatRole.User, "Quem é Michael Jackson?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Who is Michael Jackson?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Is there a well called blue shark?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Is there a well called blue whale?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Existe um poço chamado Baleia Azul?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Existe um poço chamado Tubarão Azul?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Qual o tamanho do poço Baleia Jubarte?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Qual a data de início do poço Baleia Azul?"),// Funciona
//        //new ChatMessage(ChatRole.User, "Converte esses dados em uma tabela?"),// Funciona
//    }
//};

//Response<StreamingChatCompletions> response = await client.GetChatCompletionsStreamingAsync(
//    deploymentOrModelName: "gpt-3.5-turbo",
//    chatCompletionsOptions
//);

//using StreamingChatCompletions streamingChatCompletions = response.Value;

//await foreach (StreamingChatChoice choice in streamingChatCompletions.GetChoicesStreaming())
//{
//    await foreach (ChatMessage message in choice.GetMessageStreaming())
//    {
//        Console.Write(message.Content);
//    }
//    Console.WriteLine();
//}
#endregion

#region Completions
//var completionOptions = new CompletionsOptions
//{

//};

//string pergunta = "";

////pergunta = $"{formatado} - Existe um poço chamado Baleia Azul?";// Funciona
//pergunta = $"{formatado} - Existe um poço chamado Tubarão Azul?";// Mais ou menos
////pergunta = $"{formatado} - Is there a well called blue shark?";// Legal, restringiu só para português
////pergunta = $"{formatado} - Is there a well called blue whale?";// Legal, restringiu só para português
////pergunta = $"{formatado} - Quem é Michael Jackson?";// Funciona, ignora a pergunta
////pergunta = $"{formatado} - Who is Michael Jackson?";// Funciona, ignora a pergunta
////pergunta = $"{formatado} - Qual a data de início do poço Baleia Azul?";// Funciona
////pergunta = $"{formatado} - Qual o tamanho do poço Baleia Jubarte?";// Funciona

//var completionsResponse = client.GetCompletions("text-davinci-003", pergunta);
//string completion = completionsResponse.Value.Choices[0].Text;
//Console.WriteLine($"PRONT: {completion}");
#endregion

// https://platform.openai.com/docs/guides/gpt/completions-api
// https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/openai/Azure.AI.OpenAI