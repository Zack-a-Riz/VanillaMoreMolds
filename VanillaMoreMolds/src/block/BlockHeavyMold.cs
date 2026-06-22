using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

#nullable disable

namespace VanillaMoreMolds
{
    public class BlockHeavyMold : Block
    {
        /// <summary>
        /// Récupère l'indice de stage : 0=empty, 1=fill1, 2=fill2, 3=fill3, 4=fill
        /// </summary>
        public int StageIndex
        {
            get
            {
                string stage = Variant?["stage"] ?? "empty";
                return stage switch
                {
                    "empty" => 0,
                    "fill1" => 1,
                    "fill2" => 2,
                    "fill3" => 3,
                    "fill" => 4,
                    _ => 0
                };
            }
        }

        /// <summary>
        /// Retourne le code du stage suivant
        /// </summary>
        public string NextStageCodePart()
        {
            string stage = Variant?["stage"] ?? "empty";
            return stage switch
            {
                "empty" => "fill1",
                "fill1" => "fill2",
                "fill2" => "fill3",
                "fill3" => "fill",
                _ => null
            };
        }

        /// <summary>
        /// Override CanPlaceBlock pour permettre le placement manuel du mold
        /// </summary>
        public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
        {
            // Permettre le placement en créatif ou si le joueur a un mold en main
            if (byPlayer?.WorldData?.CurrentGameMode == EnumGameMode.Creative)
            {
                failureCode = "";
                return true;
            }

            ItemStack held = byPlayer?.InventoryManager.ActiveHotbarSlot?.Itemstack;
            if (held?.Block?.Code.Path.Contains("vmmheavymold") == true)
            {
                failureCode = "";
                return true;
            }

            failureCode = "notplaceable";
            return false;
        }

        /// <summary>
        /// Retourne les hitbox de collision selon la rotation
        /// </summary>
        public override Cuboidf[] GetCollisionBoxes(IBlockAccessor world, BlockPos pos)
        {
            BlockEntityHeavyMold beem = world.GetBlockEntity(pos) as BlockEntityHeavyMold;
            int rotation = beem?.Rotation ?? 0;

            // Rotation 0 (NS): x=0.0625->0.9375, z=0.1875->0.8125 (original)
            // Rotation 1 (EW): x=0.1875->0.8125, z=0.0625->0.9375 (échangé)
            if (rotation == 1)
            {
                return new Cuboidf[] {
                    new Cuboidf(0.1875f, 0f, 0.0625f, 0.8125f, 0.625f, 0.9375f)
                };
            }

            return new Cuboidf[] {
                new Cuboidf(0.0625f, 0f, 0.1875f, 0.9375f, 0.625f, 0.8125f)
            };
        }

        /// <summary>
        /// Retourne les hitbox de sélection selon la rotation
        /// </summary>
        public override Cuboidf[] GetSelectionBoxes(IBlockAccessor world, BlockPos pos)
        {
            return GetCollisionBoxes(world, pos);
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel?.Position == null) return false;

            ItemStack held = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack;
            bool hasShift = byPlayer.Entity.Controls.ShiftKey;

            bool isSand = held != null && (held.Block?.Code.Path.Contains("sand") == true || held.Item?.Code.Path.Contains("sand") == true);
            bool isIngot = held != null && held.Item?.Code.Path.Contains("ingot") == true;

            System.Console.WriteLine($"[HEAVYMOLD] Stage: {Variant?["stage"]}, StageIndex: {StageIndex}, IsSand: {isSand}, IsIngot: {isIngot}, Shift: {hasShift}, Held: {held}");

            // 1) Progression sable avec SHIFT (empty → fill3)
            if (StageIndex < 3 && isSand && hasShift)
            {
                if (world.Side == EnumAppSide.Server)
                {
                    AdvanceStage(world, byPlayer, blockSel);
                }
                return true;
            }

            // 2) Passage FILL3 → FILL avec lingot ET SHIFT
            if (StageIndex == 3 && isIngot && hasShift)
            {
                if (world.Side == EnumAppSide.Server)
                {
                    AdvanceStage(world, byPlayer, blockSel);
                    held.StackSize--;
                }
                return true;
            }

            // 3) Right-Click normal (sans Shift, rien en main) = pickup
            if (!hasShift && held == null)
            {
                if (world.Side == EnumAppSide.Server)
                {
                    // Récupérer le bloc et le mettre en main du joueur
                    Block block = world.BlockAccessor.GetBlock(blockSel.Position);
                    ItemStack pickupStack = new ItemStack(block);

                    // 1) Essayer de mettre dans la main active (hotbar actif)
                    ItemSlot activeSlot = byPlayer.InventoryManager.ActiveHotbarSlot;

                    if (activeSlot.Empty)
                    {
                        activeSlot.Itemstack = pickupStack;
                        activeSlot.MarkDirty();
                    }
                    else if (!byPlayer.InventoryManager.TryGiveItemstack(pickupStack))
                    {
                        // Si échec, le poser au sol
                        world.SpawnItemEntity(pickupStack, new BlockPos(blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z).ToVec3d());
                    }

                    world.BlockAccessor.SetBlock(0, blockSel.Position);
                }
                return true;
            }

            return false;
        }

        public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack)
        {
            base.OnBlockPlaced(world, blockPos, byItemStack);

            if (world.Side == EnumAppSide.Server)
            {
                BlockEntityHeavyMold beem = world.BlockAccessor.GetBlockEntity(blockPos) as BlockEntityHeavyMold;
                if (beem != null)
                {
                    // Défaut: NS (rotation = 0)
                    beem.Rotation = 0;
                    beem.MarkDirty();
                }
            }
        }

        private void AdvanceStage(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (world.Side != EnumAppSide.Server) return;

            string nextStage = NextStageCodePart();
            if (nextStage == null) return;

            // Récupérer le bloc courant et extraire la couleur
            Block currentBlock = world.BlockAccessor.GetBlock(blockSel.Position);
            string color = currentBlock.Variant?["color"] ?? "blue";

            System.Console.WriteLine($"[HEAVYMOLD] Advancing from {Variant?["stage"]} to {nextStage}");
            System.Console.WriteLine($"[HEAVYMOLD] Current color: {color}");

            // Construire le nouveau code avec stage ET color
            AssetLocation newCode = CodeWithParts(nextStage, color);
            Block nextBlock = world.GetBlock(newCode);

            System.Console.WriteLine($"[HEAVYMOLD] New code: {newCode}, Block found: {nextBlock != null}");

            if (nextBlock == null) return;

            // Remplacer le bloc
            world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

            // Consommer le sable sauf en créatif
            if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
            {
                ItemStack heldStack = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack;
                if (heldStack != null)
                {
                    heldStack.StackSize--;
                }
            }

            // Son
            if (nextBlock.Sounds?.Place != null)
            {
                world.PlaySoundAt(nextBlock.Sounds.Place, blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z, 0);
            }
        }
    }
}


