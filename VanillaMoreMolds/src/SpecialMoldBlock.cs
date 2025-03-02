using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common.Entities;

namespace VanillaMoreMolds.src
{
    /// <summary>
    /// Représente un moule spécial qui peut passer
    /// par plusieurs stades (empty → stage1 → stage2 → stage3 → ready).
    /// </summary>
    public class SpecialMoldBlock : Block
    {
        /// <summary>
        /// Convertit "stage" (empty, stage1, stage2, stage3, ready)
        /// en un entier de 0 à 4.
        /// </summary>
        public int StageIndex
        {
            get
            {
                switch (Variant["stage"])
                {
                    case "empty": return 0;
                    case "stage1": return 1;
                    case "stage2": return 2;
                    case "stage3": return 3;
                    case "ready": return 4;
                }
                return 0;
            }
        }

        /// <summary>
        /// Détermine la prochaine "partie de code" (stage).
        /// empty  -> stage1
        /// stage1 -> stage2
        /// stage2 -> stage3
        /// stage3 -> ready
        /// ready  -> ready (bloqué)
        /// </summary>
        public string NextStageCodePart
        {
            get
            {
                switch (Variant["stage"])
                {
                    case "empty": return "stage1";
                    case "stage1": return "stage2";
                    case "stage2": return "stage3";
                    case "stage3": return "ready";
                    case "ready": return "ready";
                }
                return "ready";
            }
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            // Vérifie si une sélection de bloc existe
            if (blockSel == null)
            {
                return false;
            }

            // Vérifie le bloc ciblé
            Block targetedBlock = world.BlockAccessor.GetBlock(blockSel.Position);
            if (!(targetedBlock is SpecialMoldBlock))
            {
                return false;
            }

            // Empêche toujours le placement de blocs si le joueur regarde ce bloc
            ItemStack heldStack = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack;
            if (heldStack?.Block != null)
            {
                // Vérifie si l'objet tenu est une variante de sable
                bool isSand = heldStack.Block.Code.Path.Contains("sand");
                if (isSand && StageIndex < 4)
                {
                    if (world.Side == EnumAppSide.Server)
                    {
                        // Passe au prochain stade
                        string nextStage = NextStageCodePart;
                        string size = Variant["size"];
                        AssetLocation newCode = CodeWithParts(nextStage, size);
                        Block nextBlock = world.GetBlock(newCode);

                        if (nextBlock != null)
                        {
                            world.BlockAccessor.ExchangeBlock(nextBlock.BlockId, blockSel.Position);

                            // Consomme un bloc de sable (sauf mode créatif)
                            if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
                            {
                                byPlayer.InventoryManager.ActiveHotbarSlot.TakeOut(1);
                                byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
                            }

                            // Joue un son si défini
                            if (nextBlock.Sounds?.Place != null)
                            {
                                world.PlaySoundAt(
                                    nextBlock.Sounds.Place,
                                    blockSel.Position.X,
                                    blockSel.Position.Y,
                                    blockSel.Position.Z,
                                    byPlayer
                                );
                            }
                        }
                    }

                    // Empêche toute autre interaction
                    return true;
                }

                return true;
            }

            // Empêche toute autre interaction si aucun objet n'est tenu ou si ce n'est pas du sable
            return true;
        }
    }
}
