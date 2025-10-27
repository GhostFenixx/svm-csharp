using Greed.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ServerValueModifier
{
    public class SVMConfig(ModHelper modhelper)
    {
       public MainClass.MainConfig cf { get; set; } = new();
       
        //string pathToMod = modhelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        string folder = modhelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        public MainClass.MainConfig CallConfig()
        {
            string loader = File.ReadAllText(Path.Combine(folder, "Loader", "loader.json"));
            JsonNode loadname = JsonNode.Parse(loader);
            if (loadname!["CurrentlySelectedPreset"] != null && 
                !string.Equals(loadname!["CurrentlySelectedPreset"]?.ToString(), "null", StringComparison.OrdinalIgnoreCase))
            {
                string rawJSON = File.ReadAllText(Path.Combine(folder, "Presets", loadname!["CurrentlySelectedPreset"] + ".json"));
                cf = JsonSerializer.Deserialize<MainClass.MainConfig>(rawJSON);
                return cf;
            }
            else 
            {
                throw new FileNotFoundException();
            }
        }

        public JsonNode ServerMessage()
        {
            string loader = Path.Combine(folder, "Loader","loader.json");
            JsonNode LoadName = JsonNode.Parse(File.ReadAllText(loader));
            return LoadName;
        }
    }
}
