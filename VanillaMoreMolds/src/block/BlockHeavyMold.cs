namespace VanillaMoreMolds
{
    using System.Collections.Generic;
    using System.Linq;
    using Vintagestory.API.Client;
    using Vintagestory.API.Common;
    using Vintagestory.API.Datastructures;
    using Vintagestory.API.MathTools;
    using Vintagestory.GameContent;

        public class HeavyMoldOutput
    {
        public string OutputKey { get; set; } = "";

        public string ItemMatch { get; set; } = "";

        public string HelpItemMatch { get; set; } = "";

        public string ConfigKey { get; set; } = "";

        public string HelpLangCode=> $"vanillamoremolds:blockhelp-vmmheavymold-fill3-{OutputKey}";
    }

    public class BlockHeavyMold : Block
    {
        private string Stage=> Variant?["stage"] ?? "empty";

        private List<HeavyMoldOutput> outputs = [];

        private readonly Dictionary<string, MultiTextureMeshRef> sandMeshCache = new();

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            if (Attributes?["heavymoldOutputs"].Exists == true)
                outputs = Attributes["heavymoldOutputs"].AsObject<List<HeavyMoldOutput>>() ?? [];
        }

        private string[]? FillStages=>
            Attributes?["fillStages"].Exists == true
                ? Attributes["fillStages"].AsObject<string[]>()
                : ["empty", "fill1", "fill2", "fill3"];

        private string? NextFillStage()
        {
            string[] stages = FillStages ?? ["empty", "fill1", "fill2", "fill3"];
            int idx = System.Array.IndexOf(stages, Stage);
            return (idx >= 0 && idx < stages.Length - 1) ? stages[idx + 1] : null;
        }

        private bool IsLastFillStage()
        {
            string[] stages = FillStages ?? ["empty", "fill1", "fill2", "fill3"];
            return Stage == stages[^1];
        }

        private bool IsOutputEnabled(HeavyMoldOutput output)
        {
            if (string.IsNullOrEmpty(output.ConfigKey)) return true;
            var cfg = VanillaMoreMoldsConfig.Current;
            if (cfg == null) return true;
            return output.ConfigKey switch
            {
                "IsHeavyMoldIngotEnabled" => cfg.IsHeavyMoldIngotEnabled,
                "IsHeavyMoldPlateEnabled" => cfg.IsHeavyMoldPlateEnabled,
                "IsHeavyMoldRodEnabled"   => cfg.IsHeavyMoldRodEnabled,
                _ => true
            };
        }

        private static bool ItemMatchesPattern(ItemStack? stack, string pattern)
        {
            if (stack == null || string.IsNullOrEmpty(pattern)) return false;
            string code = stack.Class == EnumItemClass.Block
                ? stack.Block.Code.ToString()
                : stack.Item?.Code.ToString() ?? "";

            int star = pattern.IndexOf('*');
            if (star < 0)  return code == pattern;
            string before = pattern[..star];
            string after  = pattern[(star + 1)..];
            return code.StartsWith(before) && code.EndsWith(after)
                   && code.Length >= before.Length + after.Length;
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (blockSel?.Position == null) return false;

            ItemStack? held     = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack;
            bool       hasShift = byPlayer?.Entity?.Controls?.ShiftKey == true;
            bool       isSand   = held?.Block?.Code?.Path?.StartsWith("sand-") == true;

            if (!IsLastFillStage() && isSand)
            {
                string sandRock = held!.Block.Code.Path["sand-".Length..];
                AdvanceStage(world, byPlayer, blockSel, NextFillStage(), sandRock, consumeItem: true);
                return true;
            }

            if (IsLastFillStage() && hasShift)
            {
                foreach (HeavyMoldOutput output in outputs)
                {
                    if (!IsOutputEnabled(output)) continue;
                    if (!ItemMatchesPattern(held, output.ItemMatch)) continue;

                    AdvanceStage(world, byPlayer, blockSel, output.OutputKey, sandRock: null, consumeItem: false);
                    return true;
                }
            }

            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BEHeavyMold betm && betm.IsOutputStage)
            {
                if (betm.MetalContent == null && betm.FillLevel == 0)
                {
                    if (!hasShift && held == null && byPlayer != null)
                    {
                        if (world.Side == EnumAppSide.Server)
                        {
                            Block block = world.BlockAccessor.GetBlock(blockSel.Position);
                            ItemStack pickupStack = new ItemStack(block);
                            if (betm.SandType != null)
                                pickupStack.Attributes.SetString("sandType", betm.SandType);
                            if (!byPlayer.InventoryManager.TryGiveItemstack(pickupStack))
                                world.SpawnItemEntity(pickupStack, blockSel.Position.ToVec3d());
                            world.BlockAccessor.SetBlock(0, blockSel.Position);
                        }
                        return true;
                    }
                    return false;
                }

                betm.OnPlayerInteract(byPlayer!, blockSel.Face, blockSel.HitPosition);
                return true;
            }

            if (!hasShift && held == null && byPlayer != null)
            {
                if (world.Side == EnumAppSide.Server)
                {
                    Block block = world.BlockAccessor.GetBlock(blockSel.Position);
                    ItemStack pickupStack = new ItemStack(block);

                    var be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEHeavyMold;
                    if (be?.SandType != null)
                        pickupStack.Attributes.SetString("sandType", be.SandType);

                    if (!byPlayer.InventoryManager.TryGiveItemstack(pickupStack))
                        world.SpawnItemEntity(pickupStack, blockSel.Position.ToVec3d());

                    world.BlockAccessor.SetBlock(0, blockSel.Position);
                }
                return true;
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            if (Stage == "empty")
                return base.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);

            string color = Variant?["color"] ?? "blue";
            Block? emptyBlock = world.GetBlock(new AssetLocation("vanillamoremolds:vmmheavymold-empty-" + color));
            if (emptyBlock == null)
                return base.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);

            return [new ItemStack(emptyBlock)];
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            if (Stage == "empty")
            {
                base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
                return;
            }

            var be = world.BlockAccessor.GetBlockEntity(pos) as BEHeavyMold;
            string sandRock = be?.SandType ?? "andesite";

            base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);

            world.PlaySoundAt(new AssetLocation("vanillamoremolds:sounds/block/heavymold/heavymold-in"),
                pos, 0, null, true, 10f, 1f);
            SpawnFallingSandDustParticles(world, pos, sandRock);
        }

        public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack)
        {
            base.OnBlockPlaced(world, blockPos, byItemStack);

            if (world.BlockAccessor.GetBlockEntity(blockPos) is not BEHeavyMold be) return;

            IPlayer? placer = world.NearestPlayer(blockPos.X, blockPos.Y, blockPos.Z);
            if (placer != null)
                be.MeshAngle = (float)(System.Math.Round(placer.Entity.Pos.Yaw / GameMath.PIHALF) * GameMath.PIHALF);

            string? sandType = byItemStack?.Attributes?.GetString("sandType");
            if (sandType != null)
                be.SandType = sandType;

            if (world.Side == EnumAppSide.Server)
                be.MarkDirty(true);
            else
                world.BlockAccessor.MarkBlockEntityDirty(blockPos);
        }

        public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
        {
            ItemStack stack = base.OnPickBlock(world, pos);
            if (world.BlockAccessor.GetBlockEntity(pos) is BEHeavyMold be && be.SandType != null)
                stack.Attributes.SetString("sandType", be.SandType);
            return stack;
        }

        public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            base.OnBeforeRender(capi, itemstack, target, ref renderinfo);

            string? sandType = itemstack.Attributes?.GetString("sandType");
            if (string.IsNullOrEmpty(sandType) || !Textures.ContainsKey("fill")) return;

            string cacheKey = Code.ToString() + "/" + sandType;

            if (!sandMeshCache.TryGetValue(cacheKey, out MultiTextureMeshRef? meshRef))
            {
                AssetLocation texLoc = new AssetLocation("game", "block/stone/sand/" + sandType);
                capi.BlockTextureAtlas.GetOrInsertTexture(texLoc, out int texSubId, out _);

                CompositeTexture prevTex = Textures["fill"];
                Textures["fill"] = new CompositeTexture(texLoc)
                {
                    Baked = new BakedCompositeTexture { BakedName = texLoc, TextureSubId = texSubId }
                };
                capi.Tesselator.TesselateBlock(this, out MeshData? mesh);
                Textures["fill"] = prevTex;

                if (mesh == null) return;
                meshRef = capi.Render.UploadMultiTextureMesh(mesh);
                sandMeshCache[cacheKey] = meshRef;
            }

            renderinfo.ModelRef = meshRef;
        }

        public override void OnUnloaded(ICoreAPI api)
        {
            base.OnUnloaded(api);
            foreach (var meshRef in sandMeshCache.Values)
                meshRef.Dispose();
            sandMeshCache.Clear();
        }

        private void AdvanceStage(IWorldAccessor world, IPlayer? byPlayer, BlockSelection blockSel,
            string? nextStage, string? sandRock, bool consumeItem)
        {
            if (nextStage == null) return;

            string color          = world.BlockAccessor.GetBlock(blockSel.Position).Variant?["color"] ?? "blue";
            var    currentBe      = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEHeavyMold;
            float  meshAngle      = currentBe?.MeshAngle ?? 0f;
            string? existingSand  = currentBe?.SandType;
            string? sandType      = sandRock ?? existingSand;

            bool isOutputStage = outputs.Any(o => o.OutputKey == nextStage);

            Block? nextBlock = world.GetBlock(new AssetLocation("vanillamoremolds:vmmheavymold-" + nextStage + "-" + color));
            if (nextBlock == null) return;

            if (isOutputStage && !string.IsNullOrEmpty(sandType))
                BEHeavyMold.PendingSandType[blockSel.Position.Copy()] = sandType;

            world.BlockAccessor.SetBlock(nextBlock.BlockId, blockSel.Position);

            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BEHeavyMold newBe)
            {
                newBe.MeshAngle = meshAngle;
                newBe.SandType  = sandType;
                if (world.Side == EnumAppSide.Server)
                    newBe.MarkDirty(true);
            }

            if (world.Side == EnumAppSide.Server)
            {
                if (consumeItem && byPlayer?.WorldData.CurrentGameMode != EnumGameMode.Creative)
                {
                    ItemSlot slot = byPlayer!.InventoryManager.ActiveHotbarSlot;
                    if (slot?.Itemstack != null)
                    {
                        slot.Itemstack.StackSize--;
                        if (slot.Itemstack.StackSize <= 0) slot.Itemstack = null;
                        slot.MarkDirty();
                    }
                }

                if (consumeItem && !string.IsNullOrEmpty(sandRock))
                {
                    world.PlaySoundAt(new AssetLocation("vanillamoremolds:sounds/block/heavymold/heavymold-in"),
                        blockSel.Position, 0, null, true, 10f, 1f);
                    SpawnFallingSandDustParticles(world, blockSel, sandRock);
                }

                if (isOutputStage)
                {
                    world.PlaySoundAt(new AssetLocation("vanillamoremolds:sounds/block/heavymold/heavymold-out"),
                        blockSel.Position, 0, null, true, 10f, 1f);
                }

                if (nextBlock.Sounds?.Place != null)
                    world.PlaySoundAt(nextBlock.Sounds.Place, blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z, 0);
            }
        }

        private static void SpawnFallingSandDustParticles(IWorldAccessor world, BlockPos pos, string sandRock)
        {
            if (world.Side != EnumAppSide.Server) return;
            Block? sandBlock = world.GetBlock(new AssetLocation("game:sand-" + sandRock));
            if (sandBlock == null) return;

            Vec3d basePos = pos.ToVec3d();
            SimpleParticleProperties particles = new SimpleParticleProperties(
                30f, 60f, unchecked((int)0xD8C59AFF),
                basePos.AddCopy(0.25, 0.2, 0.25), basePos.AddCopy(0.75, 0.65, 0.75),
                new Vec3f(0f, -0.01f, 0f), new Vec3f(0f, -0.05f, 0f),
                1.0f, 0.00f, 0.1f, 0.3f, EnumParticleModel.Quad)
            {
                ColorByBlock          = sandBlock,
                WithTerrainCollision  = false,
                WindAffected          = false
            };
            world.SpawnParticles(particles, null);
        }

        private static void SpawnFallingSandDustParticles(IWorldAccessor world, BlockSelection blockSel, string sandRock)
        {
            if (world.Side != EnumAppSide.Server) return;
            Block? sandBlock = world.GetBlock(new AssetLocation("game:sand-" + sandRock));
            if (sandBlock == null) return;

            Vec3d basePos = blockSel.Position.ToVec3d();
            SimpleParticleProperties particles = new SimpleParticleProperties(
                30f, 60f, unchecked((int)0xD8C59AFF),
                basePos.AddCopy(0.25, 0.2, 0.25), basePos.AddCopy(0.75, 0.65, 0.75),
                new Vec3f(0f, -0.01f, 0f), new Vec3f(0f, -0.05f, 0f),
                1.0f, 0.00f, 0.1f, 0.3f, EnumParticleModel.Quad)
            {
                ColorByBlock          = sandBlock,
                WithTerrainCollision  = false,
                WindAffected          = false
            };
            world.SpawnParticles(particles, null);
        }


        public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
        {
            var interactions = new List<WorldInteraction>();

            bool isOutputStage = outputs.Any(o => o.OutputKey == Stage);
            var be = world.BlockAccessor.GetBlockEntity(selection.Position) as BEHeavyMold;

            if (!IsLastFillStage() && !isOutputStage)
            {
                ItemStack[] sandStacks = world.SearchBlocks(new AssetLocation("game:sand-*"))
                    .Where(b => b.Code.Path.IndexOf('-', "sand-".Length) < 0)
                    .Select(b => new ItemStack(b))
                    .ToArray();

                if (sandStacks.Length > 0)
                    interactions.Add(new WorldInteraction
                    {
                        ActionLangCode = "vanillamoremolds:blockhelp-vmmheavymold-sand",
                        MouseButton    = EnumMouseButton.Right,
                        Itemstacks     = sandStacks
                    });
            }
            else if (IsLastFillStage() && !isOutputStage)
            {
                foreach (HeavyMoldOutput output in outputs)
                {
                    if (!IsOutputEnabled(output)) continue;

                    ItemStack[] stacks = world.SearchItems(new AssetLocation(output.HelpItemMatch))
                        .Select(i => new ItemStack(i))
                        .Where(s => s.Item != null)
                        .ToArray();

                    if (stacks.Length == 0)
                        stacks = world.SearchBlocks(new AssetLocation(output.HelpItemMatch))
                            .Select(b => new ItemStack(b))
                            .ToArray();

                    if (stacks.Length > 0)
                        interactions.Add(new WorldInteraction
                        {
                            ActionLangCode = output.HelpLangCode,
                            MouseButton    = EnumMouseButton.Right,
                            HotKeyCode     = "shift",
                            Itemstacks     = stacks
                        });
                }
            }
            else if (isOutputStage)
            {

                if (be?.IsFull != true)
                {
                    ItemStack[] smeltedStacks = world.Blocks
                        .Where(b => b is BlockSmeltedContainer)
                        .Select(b => new ItemStack(b))
                        .ToArray();

                    if (smeltedStacks.Length > 0)
                        interactions.Add(new WorldInteraction
                        {
                            ActionLangCode = "blockhelp-toolmold-pour",
                            MouseButton    = EnumMouseButton.Right,
                            HotKeyCode     = "shift",
                            Itemstacks     = smeltedStacks
                        });
                }

                if (be?.IsFull == true && be.IsHardened)
                    interactions.Add(new WorldInteraction
                    {
                        ActionLangCode = "blockhelp-toolmold-takeworkitem",
                        MouseButton    = EnumMouseButton.Right
                    });
            }

            if (!isOutputStage || (be?.MetalContent == null && be?.FillLevel == 0))
                interactions.Add(new WorldInteraction
                {
                    ActionLangCode  = "blockhelp-behavior-rightclickpickup",
                    MouseButton     = EnumMouseButton.Right,
                    RequireFreeHand = true
                });

            return interactions.ToArray();
        }
    }
}