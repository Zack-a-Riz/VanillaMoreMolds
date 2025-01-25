using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Config;

namespace VanillaMoreMolds.src
{
    public class SpecialMoldBlock : Block
    {
        // Surcharge de la méthode OnBlockInteractStart du bloc
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            // Vérifie si le joueur est en sneak
            if (byPlayer?.Entity?.Controls.Sneak == true)
            {
                // Récupère l'ItemStack actuellement tenu en main (slot actif)
                ItemStack heldStack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;

                // Vérifie si on tient bien un bloc, et que ce bloc contient "sand" dans son code
                // (Ça marche pour "game:sand", "game:sand-granite", etc. selon la nomenclature que tu utilises)
                if (heldStack?.Block != null && heldStack.Block.Code.Path.Contains("sand-"))
                {
                    // Envoie le message si on est côté serveur
                    if (world.Side == EnumAppSide.Server && byPlayer is IServerPlayer serverPlayer)
                    {
                        serverPlayer.SendMessage(
                            GlobalConstants.GeneralChatGroup,
                            "Shift-clic droit détecté sur le bloc et tu tiens un bloc de sable en main !",
                            EnumChatType.Notification
                        );
                    }

                    // On renvoie true pour indiquer au jeu que l'interaction est "consommée"
                    // (plus aucun autre BlockBehavior ou code ne sera exécuté pour cette action)
                    return true;
                }
                return false;
                // Si tu souhaites que, même en sneak, le joueur puisse faire autre chose
                // quand il ne tient PAS du sable, tu peux renvoyer false ici
                // pour laisser d'autres interactions se produire, par exemple RightClickPickup.
            }

            // Si pas en sneak ou pas de sable en main, on renvoie false pour que
            // l'interaction continue normalement (RightClickPickup, etc.)
            return false;
        }
    }
}
