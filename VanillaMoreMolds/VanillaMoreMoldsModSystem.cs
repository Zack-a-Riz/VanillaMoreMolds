using Vintagestory.API.Common;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockHeavyMold", typeof(BlockHeavyMold));
            api.RegisterBlockEntityClass("BlockEntityHeavyMold", typeof(BlockEntityHeavyMold));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorSandTexture", typeof(BEBehaviorSandTexture));
        }
    }
}
