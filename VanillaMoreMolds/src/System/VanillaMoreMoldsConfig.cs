
namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsConfig
    {
        public static VanillaMoreMoldsConfig? Current { get; set; }

        public int ConfigVersion { get; set; } = 0;
        public bool EnableVMMToolMolds   { get; set; } = true;
        public bool EnableArrowheadMold  { get; set; } = false;
        public bool EnableHoopMold       { get; set; } = true;
        public bool EnableKnifeBladeMold { get; set; } = false;
        public bool EnableNailMold       { get; set; } = true;
        public bool EnablePlateMold      { get; set; } = true;
        public bool EnableSawBladeMold   { get; set; } = false;
        public bool EnableScytheHeadMold { get; set; } = false;
        public bool EnableSpearHeadMold  { get; set; } = false;
        public bool EnableVMMHeavyMold   { get; set; } = true;
        public bool EnableHeavyMoldIngot { get; set; } = true;
        public bool EnableHeavyMoldPlate { get; set; } = true;

        public bool IsToolMoldEnabled(string key) => EnableVMMToolMolds && key switch
        {
            "arrowhead"  => EnableArrowheadMold,
            "hoop"       => EnableHoopMold,
            "knifeblade" => EnableKnifeBladeMold,
            "nail"       => EnableNailMold,
            "plate"      => EnablePlateMold,
            "sawblade"   => EnableSawBladeMold,
            "scythehead" => EnableScytheHeadMold,
            "spearhead"  => EnableSpearHeadMold,
            _            => true
        };

        public bool IsHeavyMoldIngotEnabled => EnableVMMHeavyMold && EnableHeavyMoldIngot;
        public bool IsHeavyMoldPlateEnabled => EnableVMMHeavyMold && EnableHeavyMoldPlate;
    }
}
