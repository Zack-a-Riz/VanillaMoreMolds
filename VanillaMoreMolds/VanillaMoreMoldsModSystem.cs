using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {
        private const string DefaultConfig =
            "{\n" +
            "  \"ConfigVersion\": 1,\n" +
            "\n" +
            "  //\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\n" +
            "\n" +
            "  //                            === Vanilla More Molds : Configuration File ===\n" +
            "  //                                          Config version : 1\n" +
            "\n" +
            "  //\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\n" +
            "\n" +
            "  //                   Hi, I'm Zack! This is the configuration file for VanillaMoreMolds.\n" +
            "  //  The default settings are designed to keep the game balanced and avoid making blacksmithing obsolete.\n" +
            "  //                      Feel free to change these settings to suit your preferences,\n" +
            "  //      but I recommend keeping the defaults if you want to preserve the intended gameplay balance.\n" +
            "  //            This is a very early version of the configuration file, so it may evolve over time.\n" +
            "\n" +
            "  //\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\u2593\n" +
            "\n" +
            "  // Configuration:\n" +
            "\n" +
            "  // === Vanilla More Molds : Additional Vanilla-Style Molds ===\n" +
            "\n" +
            "  \"EnableVMMToolMolds\": true, // Default: true\n" +
            "  // If \"EnableVMMToolMolds\" is set to false, the individual options below are ignored.\n" +
            "\n" +
            "  \"EnableArrowheadMold\": false, // Default: false\n" +
            "  \"EnableHoopMold\": true, // Default: true\n" +
            "  \"EnableKnifeBladeMold\": false, // Default: false\n" +
            "  \"EnableNailMold\": true, // Default: true\n" +
            "  \"EnablePlateMold\": true, // Default: true\n" +
            "  \"EnableSawBladeMold\": false, // Default: false\n" +
            "  \"EnableScytheHeadMold\": false, // Default: false\n" +
            "  \"EnableSpearHeadMold\": false, // Default: false\n" +
            "\n" +
            "  // === Vanilla More Molds : Heavy Molds ===\n" +
            "\n" +
            "  \"EnableVMMHeavyMold\": true, // Default: true\n" +
            "  // If \"EnableVMMHeavyMold\" is set to false, the individual options below are ignored.\n" +
            "\n" +
            "  \"EnableHeavyMoldIngot\": true, // Default: true\n" +
            "  \"EnableHeavyMoldPlate\": true // Default: true\n" +
            "\n" +
            "  // WARNING //\n" +
            "  // Don't forget to restart your world after modifying this configuration file for the changes to take effect.\n" +
            "  // WARNING //\n" +
            "}\n";
        private const int CurrentConfigVersion = 1; // Note : Update cette version si la configuration change n'oublie pas de mettre à jour la version dans le fichier de configuration aussi.

        private static readonly (string Key, string AssetPath)[] ToolMoldAssets =
        {
            ("arrowhead",  "vanillamoremolds:recipes/clayforming/rcarrowheadmold.json"),
            ("hoop",       "vanillamoremolds:recipes/clayforming/rchoopmold.json"),
            ("knifeblade", "vanillamoremolds:recipes/clayforming/rcknifeblademold.json"),
            ("nail",       "vanillamoremolds:recipes/clayforming/rcnailmold.json"),
            ("plate",      "vanillamoremolds:recipes/clayforming/rcplatemold.json"),
            ("sawblade",   "vanillamoremolds:recipes/clayforming/rcsawblademold.json"),
            ("scythehead", "vanillamoremolds:recipes/clayforming/rcscytheheadmold.json"),
            ("spearhead",  "vanillamoremolds:recipes/clayforming/rcspearheadmold.json"),
        };

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockHeavyMold", typeof(BlockHeavyMold));
            api.RegisterBlockEntityClass("BEHeavyMold", typeof(BEHeavyMold));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorSandTexture", typeof(BEBehaviorSandTexture));

            string configPath = Path.Combine(api.GetOrCreateDataPath("ModConfig"), "vanillamoremolds.json");

            bool needsRegen = !File.Exists(configPath);
            if (!needsRegen)
            {
                var existing = api.LoadModConfig<VanillaMoreMoldsConfig>("vanillamoremolds.json");
                needsRegen = existing == null || existing.ConfigVersion != CurrentConfigVersion;
            }

            if (needsRegen)
                File.WriteAllText(configPath, DefaultConfig);

            VanillaMoreMoldsConfig.Current =
                api.LoadModConfig<VanillaMoreMoldsConfig>("vanillamoremolds.json") ?? new VanillaMoreMoldsConfig();
        }

        public override void AssetsLoaded(ICoreAPI api)
        {
            base.AssetsLoaded(api);

            var cfg = VanillaMoreMoldsConfig.Current;
            if (cfg == null) return;

            foreach (var (key, assetPath) in ToolMoldAssets)
            {
                if (!cfg.IsToolMoldEnabled(key))
                    DisableRecipeAsset(api, assetPath);
            }

            if (!cfg.EnableVMMHeavyMold || (!cfg.EnableHeavyMoldIngot && !cfg.EnableHeavyMoldPlate))
                DisableRecipeAsset(api, "vanillamoremolds:recipes/grid/rcheavymold.json");
        }

        private static void DisableRecipeAsset(ICoreAPI api, string assetPath)
        {
            var asset = api.Assets.TryGet(new AssetLocation(assetPath));
            if (asset == null) return;
            var token = JObject.Parse(Encoding.UTF8.GetString(asset.Data));
            token["enabled"] = false;
            asset.Data = Encoding.UTF8.GetBytes(token.ToString(Newtonsoft.Json.Formatting.None));
        }
    }
}
