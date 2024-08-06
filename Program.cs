using System;
using System.Collections.Generic;
using RestSharp;
using System.Text.Json;
using Models;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Mascote mascote = null;
        string caminhoArquivo = "mascote.json";

        if (File.Exists(caminhoArquivo))
        {
            mascote = Mascote.Carregar(caminhoArquivo);
            Console.WriteLine($"\nVocê já possui um mascote: {mascote.Name}");
            InteragirComMascote(mascote);
        }
        else
        {
            // Defina a URL base da API com um limite para facilitar a visualização
            string baseUrl = "https://pokeapi.co/api/v2/pokemon?limit=20";

            // Crie um cliente RestSharp com a URL base
            var client = new RestClient(baseUrl);

            // Crie uma requisição do tipo GET
            var request = new RestRequest();

            // Execute a requisição GET
            var response = client.Get(request);

            // Verifique se a resposta foi bem-sucedida
            if (response.IsSuccessful)
            {
                var jsonResponse = JsonDocument.Parse(response.Content);
                var results = jsonResponse.RootElement.GetProperty("results");

                // Imprimir as espécies de Pokémon com números
                Console.WriteLine("Lista de espécies de Pokémon:");
                for (int i = 0; i < results.GetArrayLength(); i++)
                {
                    Console.WriteLine($"{i + 1}. {results[i].GetProperty("name")}");
                }

                // Permitir que o usuário escolha um Pokémon
                Console.WriteLine("\nDigite o número ou nome do Pokémon que você deseja escolher:");
                string input = Console.ReadLine();
                string chosenPokemon = GetPokemonNameByIndex(input, results);

                if (chosenPokemon != null)
                {
                    // Buscar detalhes do Pokémon escolhido
                    mascote = GetPokemonDetails(chosenPokemon);

                    if (mascote != null)
                    {
                        // Salvar o mascote adotado
                        mascote.Salvar(caminhoArquivo);
                        InteragirComMascote(mascote);
                    }
                }
                else
                {
                    Console.WriteLine("Escolha inválida.");
                }
            }
            else
            {
                Console.WriteLine("Erro ao acessar a API do Pokémon.");
            }
        }
    }

    static string GetPokemonNameByIndex(string input, JsonElement results)
    {
        if (int.TryParse(input, out int index))
        {
            index--; // Ajustar para índice baseado em zero
            if (index >= 0 && index < results.GetArrayLength())
            {
                return results[index].GetProperty("name").GetString();
            }
        }
        else
        {
            // Se o input não for um número, assuma que é um nome
            for (int i = 0; i < results.GetArrayLength(); i++)
            {
                if (results[i].GetProperty("name").GetString().Equals(input, StringComparison.OrdinalIgnoreCase))
                {
                    return results[i].GetProperty("name").GetString();
                }
            }
        }
        return null;
    }

    static Mascote GetPokemonDetails(string pokemonName)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName}";

        var client = new RestClient(url);
        var request = new RestRequest();
        var response = client.Get(request);

        if (response.IsSuccessful)
        {
            Mascote mascote = JsonSerializer.Deserialize<Mascote>(response.Content);

            // Exibir detalhes do Pokémon
            Console.WriteLine($"\nDetalhes do Pokémon: {mascote.Name}");
            Console.WriteLine($"Altura: {mascote.Height}");
            Console.WriteLine($"Peso: {mascote.Weight}");
            Console.WriteLine("Habilidades:");
            foreach (var ability in mascote.Abilities)
            {
                Console.WriteLine($"- {ability.AbilityDetail.Name}");
            }

            return mascote;
        }
        else
        {
            Console.WriteLine("Erro ao acessar detalhes do Pokémon.");
            return null;
        }
    }

    static void InteragirComMascote(Mascote mascote)
    {
        string caminhoArquivo = "mascote.json";

        while (true)
        {
            Console.WriteLine("\nEscolha uma interação:");
            Console.WriteLine("1. Alimentar");
            Console.WriteLine("2. Treinar");
            Console.WriteLine("3. Dormir");
            Console.WriteLine("4. Exibir Histórico de Interações");
            Console.WriteLine("5. Sair");
            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    mascote.Alimentar();
                    break;
                case "2":
                    mascote.Treinar();
                    break;
                case "3":
                    mascote.Dormir();
                    break;
                case "4":
                    mascote.ExibirHistorico();
                    break;
                case "5":
                    mascote.Salvar(caminhoArquivo);
                    return;
                default:
                    Console.WriteLine("Escolha inválida.");
                    break;
            }
        }
    }
}
