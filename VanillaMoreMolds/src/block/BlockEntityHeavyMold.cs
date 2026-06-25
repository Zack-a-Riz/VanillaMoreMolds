using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
#nullable disable

namespace VanillaMoreMolds
{
    public class BlockEntityHeavyMold : BlockEntity
    {
        public float MeshAngle { get; set; } = 0f;

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetFloat("meshAngle", MeshAngle);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor world)
        {
            base.FromTreeAttributes(tree, world);
            MeshAngle = tree.GetFloat("meshAngle", 0f);

            if (world.Side == EnumAppSide.Client)
            {
                Api?.World?.BlockAccessor?.MarkBlockEntityDirty(Pos);
                Api?.World?.BlockAccessor?.MarkBlockDirty(Pos);
            }
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            if (MeshAngle == 0f) return false;

            MeshData baseMesh;
            tessThreadTesselator.TesselateBlock(Block, out baseMesh);
            if (baseMesh == null) return false;

            baseMesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, MeshAngle, 0f);
            mesher.AddMeshData(baseMesh);
            return true;
        }
    }
}
