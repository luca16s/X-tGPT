namespace XetGPT
{
    public class Constantes
    {
        public const string TEXT = @"
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

        public const string JSON = @"
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

        public const string BEHAVIOR = @"
            Utilize o seguinte contexto para responder a subsequente questão.

            CONTEXTO:
            """"""
            {0}
            """"""

            Somente responda perguntas que estão dentro do escopo do texto fornecido no CONTEXTO.
            No caso da pergunta estar fora do escopo do contexto fornecido, escreva: ""Não sei meu chapa."".
        ";
    }
}
