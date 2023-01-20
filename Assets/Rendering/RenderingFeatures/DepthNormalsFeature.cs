using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#pragma warning disable CS0618

namespace Rendering.RenderingFeatures
{
    public class DepthNormalsFeature : ScriptableRendererFeature
    {
        private Material _depthNormalsMaterial;
        private DepthNormalsPass _depthNormalsPass;
        private RenderTargetHandle _depthNormalsTexture;

        public override void Create()
        {
            _depthNormalsMaterial = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
            _depthNormalsPass = new DepthNormalsPass(RenderQueueRange.opaque, -1, _depthNormalsMaterial)
            {
                renderPassEvent = RenderPassEvent.AfterRenderingPrePasses
            };
            _depthNormalsTexture.Init("_CameraDepthNormalsTexture");
        }

        /*
         Here you can inject one or multiple render passes in the renderer.
         This method is called when setting up the renderer once per-camera.
         */
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            _depthNormalsPass.Setup(renderingData.cameraData.cameraTargetDescriptor, _depthNormalsTexture);
            renderer.EnqueuePass(_depthNormalsPass);
        }

        private class DepthNormalsPass : ScriptableRenderPass
        {
            private const int KDepthBufferBits = 32;
            private const string ProfilerTag = "DepthNormals Prepass";
            
            private readonly Material depthNormalsMaterial;
            private readonly ShaderTagId _shaderTagId = new("DepthOnly");
            
            private RenderTargetHandle DepthAttachmentHandle { get; set; }
            private RenderTextureDescriptor Descriptor { get; set; }

            private FilteringSettings _filteringSettings;

            public DepthNormalsPass(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material)
            {
                _filteringSettings = new FilteringSettings(renderQueueRange, layerMask);
                depthNormalsMaterial = material;
            }

            public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle depthAttachmentHandle)
            {
                DepthAttachmentHandle = depthAttachmentHandle;
                baseDescriptor.colorFormat = RenderTextureFormat.ARGB32;
                baseDescriptor.depthBufferBits = KDepthBufferBits;
                Descriptor = baseDescriptor;
            }

            // This method is called before executing the render pass.
            // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
            // When empty this render pass will render to the active camera render target.
            // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
            // The render pipeline will ensure target setup and clearing happens in an performance manner.
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                cmd.GetTemporaryRT(DepthAttachmentHandle.id, Descriptor, FilterMode.Point);
                ConfigureTarget(DepthAttachmentHandle.Identifier());
                ConfigureClear(ClearFlag.All, Color.black);
            }

            // Here you can implement the rendering logic.
            // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
            // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = CommandBufferPool.Get(ProfilerTag);

                using (new ProfilingSample(cmd, ProfilerTag))
                {
                    context.ExecuteCommandBuffer(cmd);
                    cmd.Clear();

                    var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
                    var drawSettings = CreateDrawingSettings(_shaderTagId, ref renderingData, sortFlags);
                    drawSettings.perObjectData = PerObjectData.None;


                    ref var cameraData = ref renderingData.cameraData;
                    var camera = cameraData.camera;
                    if (cameraData.isStereoEnabled)
                        context.StartMultiEye(camera);
                    
                    drawSettings.overrideMaterial = depthNormalsMaterial;

                    context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref _filteringSettings);
                    cmd.SetGlobalTexture("_CameraDepthNormalsTexture", DepthAttachmentHandle.id);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            /// Cleanup any allocated resources that were created during the execution of this render pass.
            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (DepthAttachmentHandle == RenderTargetHandle.CameraTarget) return;
                
                cmd.ReleaseTemporaryRT(DepthAttachmentHandle.id);
                DepthAttachmentHandle = RenderTargetHandle.CameraTarget;
            }
        }
    }
}


