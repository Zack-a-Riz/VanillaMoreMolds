using VanillaMoreMolds.src;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            // Enregistrement de la classe de comportement
            api.RegisterBlockClass("SpecialMoldBlock", typeof(SpecialMoldBlock));

            // Message de débogage pour confirmer l'enregistrement
            api.Logger.Notification("VanillaMoreMolds: SpecialMoldBlock class successfully registered!");

            api.Logger.Notification("VanillaMoreMolds : " + api.Side);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Logger.Notification(Lang.Get("vanillamoremolds:start"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Logger.Notification(Lang.Get("vanillamoremolds:start"));
        }
    }
}
