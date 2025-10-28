using SPTarkov.Server.Core.Models.Spt.Mod;

namespace ServerValueModifier
{
    public record SVM_MD : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "fika.ghostfenixx.svm";
        public override string? Name { get; init; } = "SVM";
        public override string? Author { get; init; } = "GhostFenixx";
        public override List<string>? Contributors { get; init; } = [];
        public override SemanticVersioning.Version Version { get; init; } = new("2.0.1");
        public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.2");
        public override List<string>? Incompatibilities { get; init; } = [];
        public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = [];
        public override string? Url { get; init; } = "https://github.com/GhostFenixx";
        public override bool? IsBundleMod { get; init; } = false;
        public override string? License { get; init; } = "PUSL © 2025, GhostFenixx";

    }
}
