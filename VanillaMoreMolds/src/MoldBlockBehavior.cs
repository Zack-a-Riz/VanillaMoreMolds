using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace VanillaMoreMolds
{
    public class MoldBlockBehavior : BlockBehavior
    {
        public MoldBlockBehavior(Block block) : base(block) { }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
        {
            handling = EnumHandling.PreventDefault; // Empêche d'autres actions comme poser le sable

            // Log le bloc sélectionné
            world.Logger.Notification($"[VanillaMoreMolds] Bloc sélectionné : {block.Code.Path}");

            // Vérifie si le joueur maintient Shift
            if (byPlayer.WorldData.EntityControls.Sneak)
            {
                // Vérifie si l'item en main est du sable
                var heldItem = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
                if (heldItem == null)
                {
                    world.Logger.Warning("[VanillaMoreMolds] Aucun item en main.");
                    return false;
                }

                if (!heldItem.Collectible.WildCardMatch(new AssetLocation("game:sand*")))
                {
                    world.Logger.Warning($"[VanillaMoreMolds] Item non valide : {heldItem.Collectible.Code.Path}");
                    return false;
                }

                // Vérifie si le bloc est un moule valide
                string blockPath = block.Code.Path;
                if (!IsValidMold(world, blockPath))
                {
                    world.Logger.Warning($"[VanillaMoreMolds] Bloc non valide pour l'interaction : {blockPath}");
                    return false;
                }

                // Détermine le prochain stage du moule
                string nextStage = GetNextStage(blockPath);
                if (nextStage == null)
                {
                    world.Logger.Warning($"[VanillaMoreMolds] Aucun stage suivant défini pour le bloc : {blockPath}");
                    return false;
                }

                // Charge le bloc du prochain stage
                Block nextBlock = world.GetBlock(new AssetLocation(nextStage));
                if (nextBlock == null)
                {
                    world.Logger.Warning($"[VanillaMoreMolds] Bloc pour l'étape suivante introuvable : {nextStage}");
                    return false;
                }

                // Change le bloc vers le prochain état
                world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

                // Consomme le sable
                heldItem.StackSize--;
                if (heldItem.StackSize <= 0)
                {
                    byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack = null;
                }

                world.Logger.Notification($"[VanillaMoreMolds] Transition réussie vers : {nextStage}");
                return true;
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
        }

        private bool IsValidMold(IWorldAccessor world, string blockPath)
        {
            // Vérifie si le bloc appartient à l'un des types de moules connus
            bool isValid = blockPath.Contains("vanillamoremolds:emptymold-heavyempty") ||
                           blockPath.Contains("vanillamoremolds:emptymold-mediumempty") ||
                           blockPath.Contains("vanillamoremolds:emptymold-liteempty");

            world.Logger.Notification($"[VanillaMoreMolds] Validation du bloc : {blockPath} - Valide : {isValid}");
            return isValid;
        }

        private string GetNextStage(string currentStage)
        {
            // Détermine le prochain stage basé sur le stage actuel
            switch (currentStage)
            {
                case "vanillamoremolds:emptymold-heavyempty-north": return "vanillamoremolds:heavingot_stage2";
                case "vanillamoremolds:heavingot_stage2": return "vanillamoremolds:heavingot_stage3";
                case "vanillamoremolds:heavingot_stage3": return "vanillamoremolds:heavingot";

                case "vanillamoremolds:emptymold-mediumempty-north": return "vanillamoremolds:mediumingot_stage2";
                case "vanillamoremolds:mediumingot_stage2": return "vanillamoremolds:mediumingot_stage3";
                case "vanillamoremolds:mediumingot_stage3": return "vanillamoremolds:mediumingot";

                case "vanillamoremolds:emptymold-liteempty-north": return "vanillamoremolds:liteingot_stage2";
                case "vanillamoremolds:liteingot_stage2": return "vanillamoremolds:liteingot_stage3";
                case "vanillamoremolds:liteingot_stage3": return "vanillamoremolds:liteingot";

                default: return null;
            }
        }
    }
}
