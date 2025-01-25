using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace VanillaMoreMolds.src
{
    public class SpecialMoldBlockBehavior : Block
    {
        public SpecialMoldBlockBehavior() : base()
        {
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            // Vérifie si le joueur maintient la touche Shift
            if (byPlayer?.Entity?.Controls?.Sneak == true)
            {
                // Envoie un message dans le chat du joueur
                if (byPlayer is IServerPlayer serverPlayer)
                {
                    serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup,
                        "Shift-clic droit détecté sur le bloc !", EnumChatType.Notification);
                }
                return true; // Empêche d'autres interactions de se produire
            }

            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }
    }
}
