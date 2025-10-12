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
            string loader = File.ReadAllText(folder + @"\Loader\loader.json");
            JsonNode loadname = JsonNode.Parse(loader);
            string rawJSON = File.ReadAllText(folder + @"\Presets\" + loadname!["CurrentlySelectedPreset"] + ".json");
            cf = JsonSerializer.Deserialize<MainClass.MainConfig>(rawJSON);
            return cf;
        }

        public JsonNode ServerMessage()
        {
            string loader = folder+ @"\Loader\loader.json";
            JsonNode LoadName = JsonNode.Parse(File.ReadAllText(loader));
            return LoadName;
        }
    }
}
