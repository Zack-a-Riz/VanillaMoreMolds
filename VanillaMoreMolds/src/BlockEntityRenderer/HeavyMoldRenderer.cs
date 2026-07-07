using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace VanillaMoreMolds
{
    public class HeavyMoldRenderer : IRenderer
    {
        private readonly BlockPos pos;
        private readonly ICoreClientAPI api;
        private readonly MeshRef[] quadModelRefs;
        private readonly Cuboidf[] fillQuadsByLevel;

        public Matrixf ModelMat = new Matrixf();

        public double RenderOrder => 0.5;
        public int    RenderRange => 24;

        public float Level       = 0f;
        public float Temperature = 0f;
        public AssetLocation? TextureName = null;
        public ItemStack? stack = null;

        private readonly BEHeavyMold entity;

        public HeavyMoldRenderer(BlockPos pos, ICoreClientAPI api, Cuboidf[] fillQuadsByLevel)
        {
            this.pos             = pos;
            this.api             = api;
            this.fillQuadsByLevel = fillQuadsByLevel;

            entity = (api.World.BlockAccessor.GetBlockEntity(pos) as BEHeavyMold)!;

            quadModelRefs = new MeshRef[fillQuadsByLevel.Length];
            MeshData modeldata = QuadMeshUtil.GetQuad();
            modeldata.Rgba  = new byte[4 * 4];
            for (int f = 0; f < modeldata.Rgba.Length; f++) modeldata.Rgba[f] = 255;
            modeldata.Flags = new int[4 * 4];

            for (int i = 0; i < fillQuadsByLevel.Length; i++)
            {
                Cuboidf size = fillQuadsByLevel[i];
                modeldata.Uv = new float[]
                {
                    size.X2 / 16f, size.Z2 / 16f,
                    size.X1 / 16f, size.Z2 / 16f,
                    size.X1 / 16f, size.Z1 / 16f,
                    size.X2 / 16f, size.Z1 / 16f
                };
                quadModelRefs[i] = api.Render.UploadMesh(modeldata);
            }
        }

        public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
        {
            if (Level <= 0 || TextureName == null) return;

            int voxelY = (int)GameMath.Clamp(Level, 0, fillQuadsByLevel.Length - 1);

            IRenderAPI rpi = api.Render;
            Vec3d camPos = api.World.Player.Entity.CameraPos;

            rpi.GlDisableCullFace();
            IStandardShaderProgram prog = rpi.StandardShader;
            prog.Use();
            prog.RgbaAmbientIn  = rpi.AmbientColor;
            prog.RgbaFogIn      = rpi.FogColor;
            prog.FogMinIn       = rpi.FogMin;
            prog.FogDensityIn   = rpi.FogDensity;
            prog.RgbaTint       = ColorUtil.WhiteArgbVec;
            prog.DontWarpVertices = 0;
            prog.AddRenderFlags  = 0;
            prog.ExtraGodray     = 0;
            prog.NormalShaded    = 0;

            if (stack != null)
            {
                prog.AverageColor  = ColorUtil.ToRGBAVec4f(api.BlockTextureAtlas.GetAverageColor(
                    (stack.Item?.FirstTexture ?? stack.Block.FirstTextureInventory).Baked.TextureSubId));
                prog.TempGlowMode  = 1;
            }

            Vec4f lightrgbs  = api.World.BlockAccessor.GetLightRGBs(pos.X, pos.Y, pos.Z);
            float[] glowColor = ColorUtil.GetIncandescenceColorAsColor4f((int)Temperature);
            int extraGlow     = (int)GameMath.Clamp((Temperature - 550) / 2, 0, 255);

            prog.RgbaLightIn = lightrgbs;
            prog.RgbaGlowIn  = new Vec4f(glowColor[0], glowColor[1], glowColor[2], extraGlow / 255f);
            prog.ExtraGlow   = extraGlow;

            int texid = api.Render.GetOrLoadTexture(TextureName);
            Cuboidf rect = fillQuadsByLevel[voxelY];
            float meshAngle = entity?.MeshAngle ?? 0f;

            rpi.BindTexture2d(texid);

            prog.ModelMatrix = ModelMat
                .Identity()
                .Translate(pos.X - camPos.X, pos.Y - camPos.Y, pos.Z - camPos.Z)
                .Translate(0.5f, 0f, 0.5f)
                .RotateY(meshAngle)
                .Translate(-0.5f, 0f, -0.5f)
                .Translate(1 - rect.X1 / 16f, 1.01f / 16f + Math.Max(0, Level / 16f - 0.0625f / 3), 1 - rect.Z1 / 16f)
                .RotateX(90 * GameMath.DEG2RAD)
                .Scale(0.5f * rect.Width / 16f, 0.5f * rect.Length / 16f, 0.5f)
                .Translate(-1, -1, 0)
                .Values;

            prog.ViewMatrix       = rpi.CameraMatrixOriginf;
            prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;

            rpi.RenderMesh(quadModelRefs[voxelY]);
            prog.Stop();
            rpi.GlEnableCullFace();
        }

        public void Dispose()
        {
            api.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);
            for (int i = 0; i < quadModelRefs.Length; i++)
                quadModelRefs[i]?.Dispose();
        }
    }
}
