using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
#nullable disable

namespace VanillaMoreMolds
{
    public class BlockHeavyMold : Block
    {
        private int StageIndex => (Variant?["stage"] ?? "empty") switch
        {
            "empty" => 0,
            "fill1" => 1,
            "fill2" => 2,
            "fill3" => 3,
            "ingot" => 4,
            _ => 0
        };

        private string NextStageCodePart() => (Variant?["stage"] ?? "empty") switch
        {
            "empty" => "fill1",
            "fill1" => "fill2",
            "fill2" => "fill3",
            "fill3" => "ingot",
            _ => null
        };

        public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
        {
            if (!base.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode)) return false;

            if (byPlayer?.WorldData?.CurrentGameMode == EnumGameMode.Creative || 
                byPlayer?.InventoryManager.ActiveHotbarSlot?.Itemstack?.Block?.Code.Path.Contains("vmmheavymold") == true)
            {
                failureCode = "";
                return true;
            }

            return true;
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel?.Position == null) return false;

            ItemStack held = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack;
            bool hasShift = byPlayer?.Entity?.Controls?.ShiftKey == true;
            bool isSand = held != null && (held.Block?.Code?.Path?.Contains("sand") == true || held.Item?.Code?.Path?.Contains("sand") == true);
            bool isIngot = held?.Item?.Code?.Path?.Contains("ingot") == true;

            if (StageIndex < 3 && isSand && hasShift)
            {
                if (world.Side == EnumAppSide.Server)
                    AdvanceStage(world, byPlayer, blockSel);
                return true;
            }

            if (StageIndex == 3 && isIngot && hasShift)
            {
                if (world.Side == EnumAppSide.Server)
                    AdvanceStage(world, byPlayer, blockSel);
                return true;
            }

            if (!hasShift && held == null)
            {
                if (world.Side == EnumAppSide.Server)
                {
                    Block block = world.BlockAccessor.GetBlock(blockSel.Position);
                    ItemStack pickupStack = new ItemStack(block);
                    ItemSlot activeSlot = byPlayer.InventoryManager.ActiveHotbarSlot;

                    if (activeSlot.Empty)
                    {
                        activeSlot.Itemstack = pickupStack;
                        activeSlot.MarkDirty();
                    }
                    else if (!byPlayer.InventoryManager.TryGiveItemstack(pickupStack))
                    {
                        world.SpawnItemEntity(pickupStack, blockSel.Position.ToVec3d());
                    }

                    world.BlockAccessor.SetBlock(0, blockSel.Position);
                }
                return true;
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack)
        {
            base.OnBlockPlaced(world, blockPos, byItemStack);

            IPlayer placer = world.NearestPlayer(blockPos.X, blockPos.Y, blockPos.Z);
            if (placer == null) return;

            float snapped = (float)(System.Math.Round(placer.Entity.Pos.Yaw / GameMath.PIHALF) * GameMath.PIHALF);
            var be = world.BlockAccessor.GetBlockEntity(blockPos) as BlockEntityHeavyMold;
            if (be == null) return;

            be.MeshAngle = snapped;

            if (world.Side == EnumAppSide.Server)
                be.MarkDirty(true);
            else
                world.BlockAccessor.MarkBlockEntityDirty(blockPos);
        }

        private void AdvanceStage(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (world.Side != EnumAppSide.Server) return;

            string nextStage = NextStageCodePart();
            if (nextStage == null) return;

            Block currentBlock = world.BlockAccessor.GetBlock(blockSel.Position);
            string color = currentBlock.Variant?["color"] ?? "blue";

            float meshAngle = (world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityHeavyMold)?.MeshAngle ?? 0f;

            Block nextBlock;

            if (nextStage == "ingot")
            {
                nextBlock = world.GetBlock(new AssetLocation("vanillamoremolds:vmmheavymold-toolmold-ingot-" + color));
            }
            else
            {
                nextBlock = world.GetBlock(CodeWithParts(nextStage, color));
            }

            if (nextBlock == null) return;

            world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityHeavyMold newBe)
            {
                newBe.MeshAngle = meshAngle;
                newBe.MarkDirty(true);
            }

            if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
            {
                ItemStack heldStack = byPlayer.InventoryManager.ActiveHotbarSlot?.Itemstack;
                if (heldStack != null)
                    heldStack.StackSize--;
            }

            if (nextBlock.Sounds?.Place != null)
                world.PlaySoundAt(nextBlock.Sounds.Place, blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z, 0);
        }
    }
}


