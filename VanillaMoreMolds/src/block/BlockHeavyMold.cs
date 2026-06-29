using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VanillaMoreMolds
{
    public class BlockHeavyMold : Block
    {
        private string Stage => Variant?["stage"] ?? "empty";

        private string? NextStageCodePart() => Stage switch
        {
            "empty" => "fill1",
            "fill1" => "fill2",
            "fill2" => "fill3",
            _ => null
        };

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel?.Position == null) return false;

            ItemStack? held = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack;
            bool hasShift = byPlayer?.Entity?.Controls?.ShiftKey == true;
            bool isSand = held?.Block?.Code?.Path?.StartsWith("sand-") == true;
            bool isIngot = held?.Item?.Code?.Path?.Contains("ingot") == true;
            bool isPlate = held?.Item?.Code?.Path?.StartsWith("metalplate-") == true;

            if (Stage != "fill3" && isSand && hasShift)
            {
                string sandRock = held!.Block.Code.Path.Substring("sand-".Length);
                AdvanceStage(world, byPlayer, blockSel, NextStageCodePart(), sandRock, consumeItem: true);
                return true;
            }

            if (Stage == "fill3" && isIngot && hasShift && (VanillaMoreMoldsConfig.Current?.IsHeavyMoldIngotEnabled ?? true))
            {
                AdvanceStage(world, byPlayer, blockSel, "ingot", sandRock: null, consumeItem: false);
                return true;
            }

            if (Stage == "fill3" && isPlate && hasShift && (VanillaMoreMoldsConfig.Current?.IsHeavyMoldPlateEnabled ?? true))
            {
                AdvanceStage(world, byPlayer, blockSel, "plate", sandRock: null, consumeItem: false);
                return true;
            }

            if (!hasShift && held == null && byPlayer != null)
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
                        world.SpawnItemEntity(pickupStack, blockSel.Position.ToVec3d());

                    world.BlockAccessor.SetBlock(0, blockSel.Position);
                }
                return true;
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack)
        {
            base.OnBlockPlaced(world, blockPos, byItemStack);

            var be = world.BlockAccessor.GetBlockEntity(blockPos) as BEHeavyMold;
            if (be == null) return;

            IPlayer placer = world.NearestPlayer(blockPos.X, blockPos.Y, blockPos.Z);
            if (placer == null) return;

            be.MeshAngle = (float)(System.Math.Round(placer.Entity.Pos.Yaw / GameMath.PIHALF) * GameMath.PIHALF);

            if (world.Side == EnumAppSide.Server)
                be.MarkDirty(true);
            else
                world.BlockAccessor.MarkBlockEntityDirty(blockPos);
        }

        private void AdvanceStage(IWorldAccessor world, IPlayer? byPlayer, BlockSelection blockSel, string? nextStage, string? sandRock, bool consumeItem)
        {
            if (nextStage == null) return;

            string color = world.BlockAccessor.GetBlock(blockSel.Position).Variant?["color"] ?? "blue";
            var currentBe = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEHeavyMold;
            float meshAngle = currentBe?.MeshAngle ?? 0f;
            string? existingSandType = currentBe?.SandType;
            string? sandType = sandRock ?? existingSandType;

            Block? nextBlock = (nextStage == "ingot" || nextStage == "plate")
                ? world.GetBlock(new AssetLocation("vanillamoremolds:vmmheavymold-toolmold-complet-" + color + "-fired-" + nextStage))
                : world.GetBlock(CodeWithParts(nextStage, color));

            if (nextBlock == null) return;

            if ((nextStage == "ingot" || nextStage == "plate") && !string.IsNullOrEmpty(sandType))
                BEBehaviorSandTexture.PendingSandType[blockSel.Position.Copy()] = sandType;

            world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

            var newEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
            if (newEntity is BEHeavyMold newBe)
            {
                newBe.MeshAngle = meshAngle;
                newBe.SandType = sandType;
                if (world.Side == EnumAppSide.Server)
                    newBe.MarkDirty(true);
            }
            else if (newEntity != null)
            {
                newEntity.GetType().GetField("MeshAngle", BindingFlags.Public | BindingFlags.Instance)?.SetValue(newEntity, meshAngle);
                var sandBehavior = newEntity.GetBehavior<BEBehaviorSandTexture>();
                if (sandBehavior != null && !string.IsNullOrEmpty(sandType))
                    sandBehavior.SetSandType(sandType);
                if (world.Side == EnumAppSide.Server)
                    newEntity.MarkDirty(true);
            }

            if (world.Side == EnumAppSide.Server)
            {
                if (consumeItem && byPlayer != null && byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
                {
                    ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
                    if (slot?.Itemstack != null)
                    {
                        slot.Itemstack.StackSize--;
                        if (slot.Itemstack.StackSize <= 0) slot.Itemstack = null;
                        slot.MarkDirty();
                    }
                }

                if (consumeItem && !string.IsNullOrEmpty(sandRock))
                {
                    var soundLocation = new AssetLocation("vanillamoremolds:sounds/block/heavymold/heavymold-in");
                    world.PlaySoundAt(
                        soundLocation,
                        blockSel.Position,
                        0,
                        null,
                        true,
                        10f,
                        1f
                    );
                }

                if (nextStage == "ingot" || nextStage == "plate")
                {
                    var soundLocation = new AssetLocation("vanillamoremolds:sounds/block/heavymold/heavymold-out");

                    world.PlaySoundAt(
                        soundLocation,
                        blockSel.Position,
                        0,
                        null,
                        true,
                        10f,
                        1f
                    );
                }

                if (nextBlock.Sounds?.Place != null)
                    world.PlaySoundAt(nextBlock.Sounds.Place, blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z, 0);
            }
        }
    }
}
