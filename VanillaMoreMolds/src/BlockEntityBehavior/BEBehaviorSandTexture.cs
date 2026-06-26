using System.Collections.Generic;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
#nullable disable

namespace VanillaMoreMolds
{
    public class BEBehaviorSandTexture : BlockEntityBehavior
    {
        internal static readonly Dictionary<BlockPos, string> PendingSandType = new();

        private string sandType;
        private string appliedSandType;
        private readonly MethodInfo genMeshesMethod;

        public BEBehaviorSandTexture(BlockEntity be) : base(be)
        {
            genMeshesMethod = be.GetType()
                .GetMethod("GenMeshes", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void SetSandType(string type) => sandType = type;

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            if (sandType != null) tree.SetString("sandType", sandType);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor world)
        {
            base.FromTreeAttributes(tree, world);
            sandType = tree.GetString("sandType", null);

            if (world.Side == EnumAppSide.Client && Api is ICoreClientAPI capi)
                ApplySandTexture(capi);
        }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            if (string.IsNullOrEmpty(sandType) && Blockentity.Pos != null
                && PendingSandType.TryGetValue(Blockentity.Pos, out string pending))
            {
                sandType = pending;
                PendingSandType.Remove(Blockentity.Pos);
            }

            base.Initialize(api, properties);

            if (api.Side == EnumAppSide.Client && api is ICoreClientAPI capi)
                ApplySandTexture(capi);
        }

        private void ApplySandTexture(ICoreClientAPI capi)
        {
            if (capi == null || string.IsNullOrEmpty(sandType)) return;
            if (!Blockentity.Block.Textures.ContainsKey("sand")) return;
            if (sandType == appliedSandType) return;

            appliedSandType = sandType;

            AssetLocation texLoc = new AssetLocation("game", "block/stone/sand/" + sandType);
            capi.BlockTextureAtlas.GetOrInsertTexture(texLoc, out int texSubId, out _);

            CompositeTexture originalTex = Blockentity.Block.Textures["sand"];
            CompositeTexture newTex = new CompositeTexture(texLoc);
            newTex.Baked = new BakedCompositeTexture { BakedName = texLoc, TextureSubId = texSubId };
            Blockentity.Block.Textures["sand"] = newTex;

            ObjectCacheUtil.Delete(capi, Blockentity.Block.Code.ToString());
            genMeshesMethod?.Invoke(Blockentity, null);

            Blockentity.Block.Textures["sand"] = originalTex;
        }
    }
}
