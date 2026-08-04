using SPTarkov.Server.Core.Models.Spt.Mod;

namespace ServerValueModifier
{
    public record SVM_MD : IModMetadata
    {
        public string ModGuid { get; init; } = "fika.ghostfenixx.svm";
        public string? Name { get; init; } = "SVM";
        public string? Author { get; init; } = "GhostFenixx";
        public List<string>? Contributors { get; init; } = [];
        public SemanticVersioning.Version Version { get; init; } = new("2.2.0");
        public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
        public List<string>? Incompatibilities { get; init; } = [];
        public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = [];
        public string? Url { get; init; } = "https://github.com/GhostFenixx";
        public string? License { get; init; } = "PUSL © 2026, GhostFenixx";
        public bool HasPrepatcher { get; init; } = false;
    }
}
