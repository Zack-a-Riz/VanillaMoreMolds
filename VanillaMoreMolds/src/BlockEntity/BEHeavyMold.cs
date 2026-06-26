using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
#nullable disable

namespace VanillaMoreMolds
{
    public class BEHeavyMold : BlockEntity
    {
        private static readonly Vec3f RotationPivot = new Vec3f(0.5f, 0.5f, 0.5f);

        public float MeshAngle { get; set; }
        public string SandType { get; set; }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetFloat("meshAngle", MeshAngle);
            if (SandType != null)
                tree.SetString("sandType", SandType);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor world)
        {
            float prevAngle = MeshAngle;
            string prevSand = SandType;
            base.FromTreeAttributes(tree, world);
            MeshAngle = tree.GetFloat("meshAngle", 0f);
            SandType = tree.GetString("sandType", null);

            if (world.Side == EnumAppSide.Client && Api != null && Pos != null &&
                (MeshAngle != prevAngle || SandType != prevSand))
            {
                Api.World.BlockAccessor.MarkBlockDirty(Pos);
            }
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            CompositeTexture prevSandTex = null;

            if (!string.IsNullOrEmpty(SandType) && Block.Textures.ContainsKey("sand")
                && Api is ICoreClientAPI capi)
            {
                prevSandTex = Block.Textures["sand"];
                AssetLocation texLoc = new AssetLocation("game", "block/stone/sand/" + SandType);

                capi.BlockTextureAtlas.GetOrInsertTexture(texLoc, out int texSubId, out _);

                CompositeTexture newTex = new CompositeTexture(texLoc);
                newTex.Baked = new BakedCompositeTexture { BakedName = texLoc, TextureSubId = texSubId };
                Block.Textures["sand"] = newTex;
            }

            try
            {
                tessThreadTesselator.TesselateBlock(Block, out MeshData baseMesh);

                if (baseMesh == null) return false;

                if (MeshAngle != 0f)
                    baseMesh.Rotate(RotationPivot, 0f, MeshAngle, 0f);

                mesher.AddMeshData(baseMesh);
                return true;
            }
            finally
            {
                if (prevSandTex != null)
                    Block.Textures["sand"] = prevSandTex;
            }
        }
    }
}
