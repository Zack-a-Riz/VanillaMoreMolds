using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

#nullable disable

namespace VanillaMoreMolds
{
    public class BlockEntityHeavyMold : BlockEntity
    {
        /// <summary>
        /// Rotation: 0 = Nord-Sud, 1 = Est-Ouest
        /// </summary>
        public int Rotation { get; set; } = 0;

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessor)
        {
            base.FromTreeAttributes(tree, worldAccessor);
            Rotation = tree.GetInt("rotation", 0);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetInt("rotation", Rotation);
        }
    }
}



