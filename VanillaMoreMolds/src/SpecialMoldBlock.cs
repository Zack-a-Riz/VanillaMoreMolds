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
            // Vérifie si le joueur est en sneak (shift)
            if (byPlayer?.Entity?.Controls.Sneak == true)
            {
                // Récupère l'ItemStack tenu en main
                ItemStack heldStack = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack;
                if (heldStack?.Block != null)
                {
                    // Vérifie s'il s'agit de sable (toutes variantes : sand, sand-granite, etc.)
                    bool isSand = heldStack.Block.Code.Path == "sand"
                               || heldStack.Block.Code.Path.StartsWith("sand-");

                    // Vérifie qu'on n'est pas déjà au dernier stade
                    bool notAtFinalStage = (StageIndex < 4);

                    if (isSand && notAtFinalStage)
                    {
                        // ----
                        // ICI, on renvoie 'true' côté client ET serveur pour empêcher la pose de sable
                        // ----

                        // La logique de changement de stage ne se fait que côté serveur
                        if (world.Side == EnumAppSide.Server)
                        {
                            // Stage suivant
                            string nextStage = NextStageCodePart;
                            // Taille (lite, medium, heavy)
                            string size = Variant["size"];

                            // Construit le nouveau code
                            // => "specialmolds-[nextStage]-[size]" (ex: "specialmolds-stage2-lite")
                            AssetLocation newCode = CodeWithParts(nextStage, size);
                            Block nextBlock = world.GetBlock(newCode);

                            if (nextBlock != null)
                            {
                                // Changer le bloc
                                world.BlockAccessor.ExchangeBlock(nextBlock.BlockId, blockSel.Position);

                                // Consommer 1 sable (sauf créatif)
                                if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
                                {
                                    byPlayer.InventoryManager.ActiveHotbarSlot.TakeOut(1);
                                    byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
                                }

                                // Jouer un son si présent
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

                        // On bloque l'interaction pour empêcher le placement du bloc sable
                        return true;
                    }
                }
            }

            // Sinon, on laisse l’interaction se poursuivre normalement
            return false;
        }
    }
}
