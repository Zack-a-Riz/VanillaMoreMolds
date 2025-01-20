using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VanillaMoreMolds
{
    public class MoldBlockBehavior : BlockBehavior
    {
        public MoldBlockBehavior(Block block) : base(block) { }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            // Vérifie si le joueur maintient Shift
            if (byPlayer.WorldData.EntityControls.Sneak)
            {
                // Vérifie si l'objet en main est du sable
                var heldItem = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
                if (heldItem != null && heldItem.Collectible.Code.Path.Contains("sand"))
                {
                    // Récupère les variantes du moule (par exemple Stage2, Stage3)
                    string currentStage = block.Code.GetVariant("stage");
                    string nextStage = GetNextStage(currentStage);

                    if (nextStage != null)
                    {
                        // Change le bloc vers le prochain état
                        Block nextBlock = world.GetBlock(block.CodeWithVariant("stage", nextStage));
                        if (nextBlock != null)
                        {
                            world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

                            // Consomme le sable utilisé
                            heldItem.StackSize--;
                            if (heldItem.StackSize <= 0)
                            {
                                byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack = null;
                            }

                            return true;
                        }
                    }
                }
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        private string GetNextStage(string currentStage)
        {
            // Gère les transitions entre les étapes
            switch (currentStage)
            {
                case "Stage1": return "Stage2";
                case "Stage2": return "Stage3";
                case "Stage3": return "Final";
                default: return null;
            }
        }
    }
}
