namespace VanillaMoreMolds
{
    using System.Collections.Generic;
    using Vintagestory.API.Client;
    using Vintagestory.API.Common;
    using Vintagestory.API.MathTools;
    using Vintagestory.GameContent;

        public class BlockHeavyToolMold : BlockToolMold
    {

        private readonly Dictionary<string, MultiTextureMeshRef> sandMeshCache = new();

        private static string? GetSandType(IWorldAccessor world, BlockPos pos) =>
            world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorSandTexture>()?.SandType;

        private static string? GetSandType(ItemStack stack) =>
            stack.Attributes?.GetString("sandType");

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (world.Side == EnumAppSide.Server
                && byPlayer.InventoryManager.ActiveHotbarSlot.Empty
                && byPlayer.Entity?.Controls?.ShiftKey != true)
            {
                BlockEntityToolMold? betm = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityToolMold;
                if (betm?.MetalContent == null)
                {
                    string? sandType = GetSandType(world, blockSel.Position);
                    if (!string.IsNullOrEmpty(sandType))
                    {
                        ItemStack stack = new ItemStack(this);
                        stack.Attributes.SetString("sandType", sandType);

                        if (!byPlayer.InventoryManager.TryGiveItemstack(stack, true))
                            world.SpawnItemEntity(stack, blockSel.Position.ToVec3d().AddCopy(0.5, 0.1, 0.5));

                        world.BlockAccessor.SetBlock(0, blockSel.Position);
                        world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);

                        BlockSounds? sounds = GetSounds(world.BlockAccessor, blockSel);
                        if (sounds != null)
                            world.PlaySoundAt(sounds.Place.Location,
                                blockSel.Position.X + 0.5, blockSel.Position.Y + 0.5, blockSel.Position.Z + 0.5,
                                byPlayer);

                        return true;
                    }
                }
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
        {
            ItemStack stack = base.OnPickBlock(world, pos);
            string? sandType = GetSandType(world, pos);
            if (sandType != null)
                stack.Attributes.SetString("sandType", sandType);
            return stack;
        }

        public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
        {
            bool placed = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);

            if (placed)
            {
                string? sandType = GetSandType(byItemStack);
                if (!string.IsNullOrEmpty(sandType))
                    world.BlockAccessor.GetBlockEntity(blockSel.Position)
                        ?.GetBehavior<BEBehaviorSandTexture>()
                        ?.SetSandType(sandType);
            }

            return placed;
        }

        public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            base.OnBeforeRender(capi, itemstack, target, ref renderinfo);

            string? sandType = GetSandType(itemstack);
            if (string.IsNullOrEmpty(sandType) || !Textures.ContainsKey("sand")) return;

            string cacheKey = Code.ToString() + "/" + sandType;

            if (!sandMeshCache.TryGetValue(cacheKey, out MultiTextureMeshRef? meshRef))
            {
                AssetLocation texLoc = new AssetLocation("game", "block/stone/sand/" + sandType);
                capi.BlockTextureAtlas.GetOrInsertTexture(texLoc, out int texSubId, out _);

                CompositeTexture prevTex = Textures["sand"];
                Textures["sand"] = new CompositeTexture(texLoc)
                {
                    Baked = new BakedCompositeTexture { BakedName = texLoc, TextureSubId = texSubId }
                };
                capi.Tesselator.TesselateBlock(this, out MeshData? mesh);
                Textures["sand"] = prevTex;

                if (mesh == null) return;
                meshRef = capi.Render.UploadMultiTextureMesh(mesh);
                sandMeshCache[cacheKey] = meshRef;
            }

            renderinfo.ModelRef = meshRef;
        }

        public override void OnUnloaded(ICoreAPI api)
        {
            base.OnUnloaded(api);
            foreach (MultiTextureMeshRef meshRef in sandMeshCache.Values)
                meshRef.Dispose();
            sandMeshCache.Clear();
        }
    }
}
