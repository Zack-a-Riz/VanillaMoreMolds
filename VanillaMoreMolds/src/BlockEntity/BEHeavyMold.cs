using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VanillaMoreMolds
{
    public class BEHeavyMold : BlockEntity, ILiquidMetalSink, ITemperatureSensitive, IRotatable
    {
        private static readonly Vec3f RotationPivot = new Vec3f(0.5f, 0.5f, 0.5f);

        public float MeshAngle  { get; set; }
        public string? SandType { get; set; }

        public ItemStack? MetalContent { get; set; }
        public int  FillLevel { get; set; }
        public bool Shattered { get; set; }

        private int      requiredUnits   = 100;
        private float    fillHeight      = 1f;
        private Cuboidf[] fillQuadsByLevel = [];

        public float Temperature => MetalContent?.Collectible.GetTemperature(Api.World, MetalContent) ?? 0f;
        public bool  IsHardened  => Temperature < 0.3f * MetalContent?.Collectible.GetMeltingPoint(Api.World, null, new DummySlot(MetalContent));
        public bool  IsLiquid    => Temperature > 0.8f * MetalContent?.Collectible.GetMeltingPoint(Api.World, null, new DummySlot(MetalContent));
        public bool  IsFull      => FillLevel >= requiredUnits;
        public bool  IsHot       => Temperature >= 200;
        public bool  CanReceiveAny => !Shattered && IsOutputStage;
        public bool  IsOutputStage => Block?.Variant?["stage"] is "ingot" or "plate";

        private ICoreClientAPI? capi;
        private HeavyMoldRenderer? renderer;

        public static readonly Dictionary<BlockPos, string> PendingSandType = new();

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            if (Block?.Attributes != null)
            {
                fillHeight    = Block.Attributes["fillHeight"].AsFloat(1f);
                requiredUnits = Block.Attributes["requiredUnits"].AsInt(100);

                if (Block.Attributes["fillQuadsByLevel"].Exists)
                    fillQuadsByLevel = Block.Attributes["fillQuadsByLevel"].AsObject<Cuboidf[]>() ?? [];
            }

            if (fillQuadsByLevel.Length == 0)
                fillQuadsByLevel = [new Cuboidf(2, 0, 2, 14, 0, 14)];

            capi = api as ICoreClientAPI;
            if (capi != null && IsOutputStage && !Shattered)
            {
                capi.Event.RegisterRenderer(
                    renderer = new HeavyMoldRenderer(Pos!, capi, fillQuadsByLevel),
                    EnumRenderStage.Opaque, "heavymoldrenderer");
                UpdateRenderer();
            }

            if (IsOutputStage && !Shattered)
                RegisterGameTickListener(OnGameTick, 50);

            if (PendingSandType.TryGetValue(Pos!, out string? pending))
            {
                SandType = pending;
                PendingSandType.Remove(Pos!);
            }
        }

        private void OnGameTick(float dt)
        {
            if (renderer == null || MetalContent == null) return;
            renderer.Level       = (float)FillLevel * fillHeight / requiredUnits;
            renderer.stack       = MetalContent;
            renderer.Temperature = Math.Min(1300, MetalContent.Collectible.GetTemperature(Api.World, MetalContent));
        }

        public bool CanReceive(ItemStack metal)
        {
            return !Shattered
                && IsOutputStage
                && (MetalContent == null || (MetalContent.Collectible.Equals(MetalContent, metal, GlobalConstants.IgnoredStackAttributes) && !IsFull))
                && GetMoldedStacks(metal)?.Length > 0;
        }

        public void BeginFill(Vec3d hitPosition) { }

        public void ReceiveLiquidMetal(ItemStack metal, ref int amount, float temperature)
        {
            if (IsFull || (!MetalContent?.Collectible.Equals(MetalContent, metal, GlobalConstants.IgnoredStackAttributes) ?? false)) return;
            if (MetalContent == null)
            {
                MetalContent = metal.Clone();
                MetalContent.ResolveBlockOrItem(Api.World);
                MetalContent.Collectible.SetTemperature(Api.World, MetalContent, temperature, false);
                MetalContent.StackSize = 1;
                (MetalContent.Attributes["temperature"] as ITreeAttribute)?.SetFloat("cooldownSpeed", 300);
            }
            else
            {
                MetalContent.Collectible.SetTemperature(Api.World, MetalContent, temperature, false);
            }

            int toFill = Math.Min(amount, requiredUnits - FillLevel);
            FillLevel += toFill;
            amount    -= toFill;
            UpdateRenderer();
        }

        public void OnPourOver() => MarkDirty(true);

        public bool OnPlayerInteract(IPlayer byPlayer, BlockFacing onFace, Vec3d hitPosition)
        {
            if (Shattered || !IsOutputStage) return false;
            if (byPlayer.Entity.Controls.ShiftKey) return false;
            if (byPlayer.Entity.Controls.HandUse != EnumHandInteract.None) return false;

            return TryTakeContents(byPlayer);
        }

        private bool TryTakeContents(IPlayer byPlayer)
        {
            if (MetalContent == null || FillLevel == 0) return false;

            if (Api is Vintagestory.API.Server.ICoreServerAPI) MarkDirty();

            if (!IsFull || !IsHardened) return false;

            Api.World.PlaySoundAt(new AssetLocation("sounds/block/ingot"), Pos!, -0.5, byPlayer, false);

            if (Api is Vintagestory.API.Server.ICoreServerAPI)
            {
                ItemStack[]? outstacks = GetStateAwareMoldedStacks();
                if (outstacks != null)
                {
                    foreach (ItemStack stack in outstacks)
                    {
                        if (!byPlayer.InventoryManager.TryGiveItemstack(stack))
                            Api.World.SpawnItemEntity(stack, Pos!.ToVec3d().Add(0.5, 0.2, 0.5));
                    }
                    MetalContent = null;
                    FillLevel    = 0;
                    MarkDirty(true);
                }
            }

            UpdateRenderer();
            return true;
        }

        public ItemStack[]? GetStateAwareMoldedStacks()
        {
            if (MetalContent?.Collectible != null && IsHardened && IsFull)
                return GetMoldedStacks(MetalContent);
            return null;
        }

        public ItemStack[]? GetMoldedStacks(ItemStack fromMetal)
        {
            if (Block?.Attributes?["drop"].Exists != true) return null;
            try
            {
                var jstack = Block.Attributes["drop"].AsObject<JsonItemStack>(null, Block.Code.Domain);
                if (jstack == null) return null;

                string metaltype = fromMetal.Collectible.LastCodePart();
                jstack.Code.Path = jstack.Code.Path.Replace("{metal}", metaltype);
                jstack.Resolve(Api.World, "heavy mold drop for " + Block.Code);
                if (jstack.ResolvedItemstack is not ItemStack stack) return Array.Empty<ItemStack>();

                stack.Collectible.SetTemperature(Api.World, stack,
                    MetalContent?.Collectible.GetTemperature(Api.World, MetalContent) ?? 20);
                return [stack];
            }
            catch (JsonReaderException)
            {
                Api.World.Logger.Error("BEHeavyMold: failed to parse drop for {0}", Block?.Code);
                throw;
            }
        }

        private void UpdateRenderer()
        {
            if (renderer == null) return;
            renderer.Level = (float)FillLevel * fillHeight / requiredUnits;
            renderer.TextureName = MetalContent?.Collectible != null
                ? new AssetLocation("block/metal/ingot/" + MetalContent.Collectible.LastCodePart() + ".png")
                : null;
        }

        public override void OnBlockRemoved()
        {
            base.OnBlockRemoved();
            renderer?.Dispose();
            renderer = null;
        }

        public override void OnBlockUnloaded()
        {
            base.OnBlockUnloaded();
            renderer?.Dispose();
            renderer = null;
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            CompositeTexture? prevFillTex = null;

            if (!string.IsNullOrEmpty(SandType) && Block.Textures.ContainsKey("fill") && Api is ICoreClientAPI capi2)
            {
                prevFillTex = Block.Textures["fill"];
                AssetLocation texLoc = new AssetLocation("game", "block/stone/sand/" + SandType);
                capi2.BlockTextureAtlas.GetOrInsertTexture(texLoc, out int texSubId, out _);
                Block.Textures["fill"] = new CompositeTexture(texLoc)
                {
                    Baked = new BakedCompositeTexture { BakedName = texLoc, TextureSubId = texSubId }
                };
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
                if (prevFillTex != null)
                    Block.Textures["fill"] = prevFillTex;
            }
        }

        public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation,
            Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
        {
            MeshAngle  = tree.GetFloat("meshAngle");
            MeshAngle -= degreeRotation * GameMath.DEG2RAD;
            tree.SetFloat("meshAngle", MeshAngle);
        }

        public void CoolNow(float amountRel, OnStackToCool onStackToCoolCallback)
        {
            if (MetalContent == null) return;
            float breakchance = Math.Max(0, amountRel - 0.6f) * Math.Max(Temperature - 250f, 0) / 5000f;
            if (Api.World.Rand.NextDouble() < breakchance)
            {
                Shattered = true;
                Api.World.PlaySoundAt(new AssetLocation("sounds/block/ceramicbreak"), Pos!, -0.4);
                Block.SpawnBlockBrokenParticles(Pos!);
                MetalContent.Collectible.SetTemperature(Api.World, MetalContent, 20, false);
                FillLevel = (int)(FillLevel * (0.7f + Api.World.Rand.NextDouble() * 0.1f));
                MarkDirty(true);
            }
            else
            {
                float temp = Temperature;
                if (temp > 120)
                    Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos!, -0.5, null, false, 16);
                MetalContent.Collectible.SetTemperature(Api.World, MetalContent, Math.Max(20, temp - amountRel * 20), false);
                MarkDirty(true);
            }
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetFloat("meshAngle", MeshAngle);
            if (SandType != null)
                tree.SetString("sandType", SandType);
            tree.SetItemstack("contents", MetalContent);
            tree.SetInt("fillLevel", FillLevel);
            tree.SetBool("shattered", Shattered);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor world)
        {
            float  prevAngle = MeshAngle;
            string? prevSand = SandType;

            base.FromTreeAttributes(tree, world);
            MeshAngle    = tree.GetFloat("meshAngle", 0f);
            SandType     = tree.GetString("sandType", null);
            MetalContent = tree.GetItemstack("contents");
            FillLevel    = tree.GetInt("fillLevel");
            Shattered    = tree.GetBool("shattered");

            if (world != null && MetalContent != null)
                MetalContent.ResolveBlockOrItem(world);

            UpdateRenderer();

            if (world.Side == EnumAppSide.Client && Api != null && Pos != null
                && (MeshAngle != prevAngle || SandType != prevSand))
            {
                Api.World.BlockAccessor.MarkBlockDirty(Pos);
            }
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            if (!IsOutputStage || Shattered) return;

            string state    = IsLiquid ? Lang.Get("liquid") : (IsHardened ? Lang.Get("hardened") : Lang.Get("soft"));
            string matkey   = "material-" + MetalContent?.Collectible.Variant?["metal"];
            string mat      = Lang.HasTranslation(matkey) ? Lang.Get(matkey) : MetalContent?.GetName() ?? "";
            string temp     = Temperature < 21 ? Lang.Get("Cold") : Lang.Get("{0}°C", (int)Temperature);
            string contents = Lang.GetWithFallback("metalmold-blockinfo-unitsofmetal",
                "{0}/{4} units of {1} {2} ({3})", FillLevel, state, mat, temp, requiredUnits);
            dsc.AppendLine((MetalContent != null ? contents
                : Lang.GetWithFallback("metalmold-blockinfo-emptymold", "0/{0} units of metal", requiredUnits)) + "\n");
        }
    }
}
