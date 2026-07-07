using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {
        private const int CurrentConfigVersion = 3; // Note : Update cette version si la configuration change n'oublie pas de mettre à jour la version dans le fichier de configuration aussi.

        private static readonly string DefaultConfig = $$"""
            {
              //████████████████████████████████████████████████████████████████████████████████████████████████████

              //                            === Vanilla More Molds : Configuration File ===

              //████████████████████████████████████████████████████████████████████████████████████████████████████

              //                   Hi, I'm Zack! This is the configuration file for VanillaMoreMolds.
              //  The default settings are designed to keep the game balanced and avoid making blacksmithing obsolete.
              //                      Feel free to change these settings to suit your preferences,
              //      but I recommend keeping the defaults if you want to preserve the intended gameplay balance.
              //            This is a very early version of the configuration file, so it may evolve over time.

              //████████████████████████████████████████████████████████████████████████████████████████████████████

              // Configuration:

              "ConfigVersion": {{CurrentConfigVersion}}, // Version of the configuration file. Do not change this value.

              // === Vanilla More Molds : Additional Vanilla-Style Molds ===

              "EnableVMMToolMolds": true, // Default: true
              // If "EnableVMMToolMolds" is set to false, the individual options below are ignored.

              "EnableArrowheadMold": false, // Default: false
              "EnableHoopMold": true, // Default: true
              "EnableKnifeBladeMold": false, // Default: false
              "EnableNailMold": true, // Default: true
              "EnablePlateMold": true, // Default: true
              "EnableSawBladeMold": false, // Default: false
              "EnableScytheHeadMold": false, // Default: false
              "EnableSpearHeadMold": false, // Default: false

              // === Vanilla More Molds : Heavy Molds ===

              "EnableVMMHeavyMold": true, // Default: true
              // If "EnableVMMHeavyMold" is set to false, the individual options below are ignored.

              "EnableHeavyMoldIngot": true, // Default: true
              "EnableHeavyMoldPlate": true, // Default: true
              "EnableHeavyMoldRod": true  // Default: true

              // WARNING //
              // Don't forget to restart your world after modifying this configuration file for the changes to take effect.
              // WARNING //
            }
            """;

        private static readonly (string Key, string AssetPath)[] ToolMoldAssets =
        {
            ("arrowhead", "vanillamoremolds:recipes/clayforming/rcarrowheadmold.json"),
            ("hoop", "vanillamoremolds:recipes/clayforming/rchoopmold.json"),
            ("knifeblade", "vanillamoremolds:recipes/clayforming/rcknifeblademold.json"),
            ("nail", "vanillamoremolds:recipes/clayforming/rcnailmold.json"),
            ("plate", "vanillamoremolds:recipes/clayforming/rcplatemold.json"),
            ("sawblade", "vanillamoremolds:recipes/clayforming/rcsawblademold.json"),
            ("scythehead", "vanillamoremolds:recipes/clayforming/rcscytheheadmold.json"),
            ("spearhead", "vanillamoremolds:recipes/clayforming/rcspearheadmold.json"),
        };

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockHeavyMold", typeof(BlockHeavyMold));
            api.RegisterBlockEntityClass("BEHeavyMold", typeof(BEHeavyMold));

            string configPath = Path.Combine(api.GetOrCreateDataPath("ModConfig"), "vanillamoremolds.json");

            bool needsRegen = !File.Exists(configPath);
            if (!needsRegen)
            {
                try
                {
                    var existing = api.LoadModConfig<VanillaMoreMoldsConfig>("vanillamoremolds.json");
                    needsRegen = existing == null || existing.ConfigVersion != CurrentConfigVersion;
                }
                catch
                {
                    needsRegen = true;
                    api.Logger.Warning("[VanillaMoreMolds] Configuration file is invalid, resetting to defaults.");
                }
            }

            if (needsRegen)
                File.WriteAllText(configPath, DefaultConfig);

            try
            {
                VanillaMoreMoldsConfig.Current =
                    api.LoadModConfig<VanillaMoreMoldsConfig>("vanillamoremolds.json") ?? new VanillaMoreMoldsConfig();
            }
            catch
            {
                api.Logger.Warning("[VanillaMoreMolds] Failed to load configuration after reset, using defaults.");
                VanillaMoreMoldsConfig.Current = new VanillaMoreMoldsConfig();
            }
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

            if (!cfg.EnableVMMHeavyMold || (!cfg.EnableHeavyMoldIngot && !cfg.EnableHeavyMoldPlate && !cfg.EnableHeavyMoldRod))
            {
                DisableRecipeAsset(api, "vanillamoremolds:recipes/grid/rcheavymold.json");
                DisableRecipeAsset(api, "vanillamoremolds:recipes/clayforming/heavymold/rcheavybasemold.json");
            }
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
