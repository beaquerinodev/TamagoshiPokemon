using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;

namespace Models
{
    public class Mascote
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("abilities")]
        public List<Ability> Abilities { get; set; }

        [JsonPropertyName("nivel")]
        public int Nivel { get; set; } = 1;

        [JsonPropertyName("interactionHistory")]
        public List<string> InteractionHistory { get; set; } = new List<string>();

        public void Alimentar()
        {
            Console.WriteLine($"{Name} foi alimentado.");
            InteractionHistory.Add($"{DateTime.Now}: {Name} foi alimentado.");
            IncrementarNivel();
        }

        public void Treinar()
        {
            Console.WriteLine($"{Name} está treinando.");
            InteractionHistory.Add($"{DateTime.Now}: {Name} está treinando.");
            IncrementarNivel();
        }

        public void Dormir()
        {
            Console.WriteLine($"{Name} está dormindo.");
            InteractionHistory.Add($"{DateTime.Now}: {Name} está dormindo.");
            IncrementarNivel();
        }

        private void IncrementarNivel()
        {
            Nivel++;
            Console.WriteLine($"{Name} subiu para o nível {Nivel}!");
        }

        public void Salvar(string caminhoArquivo)
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoArquivo, json);
        }

        public static Mascote Carregar(string caminhoArquivo)
        {
            string json = File.ReadAllText(caminhoArquivo);
            return JsonSerializer.Deserialize<Mascote>(json);
        }

        public void ExibirHistorico()
        {
            Console.WriteLine($"\nHistórico de Interações de {Name}:");
            foreach (var interacao in InteractionHistory)
            {
                Console.WriteLine(interacao);
            }
        }
    }
}
