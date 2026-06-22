using Vintagestory.API.Common;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            // Enregistrer les classes de bloc personnalisées
            api.RegisterBlockClass("BlockHeavyMold", typeof(BlockHeavyMold));

            // Enregistrer les BlockEntity
            api.RegisterBlockEntityClass("BlockEntityHeavyMold", typeof(BlockEntityHeavyMold));
        }
    }
}
