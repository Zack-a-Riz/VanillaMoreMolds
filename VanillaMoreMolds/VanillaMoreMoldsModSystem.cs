using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace VanillaMoreMolds
{
    public class VanillaMoreMoldsModSystem : ModSystem
    {

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
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

