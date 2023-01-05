using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rendering.RenderingFeatures
{
    public class OutlineFeature : ScriptableRendererFeature
    {
        public OutlineSettings settings = new OutlineSettings();
        
        private RenderTargetHandle _outlineTexture;
        private OutlinePass _outlinePass;

        public override void Create()
        {
            _outlinePass = new OutlinePass(settings.outlineMaterial)
            {
                renderPassEvent = RenderPassEvent.AfterRenderingTransparents
            };
            _outlineTexture.Init("_OutlineTexture");
        }
        
        /*
         Here you can inject one or multiple render passes in the renderer.
         This method is called when setting up the renderer once per-camera.
         */
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (settings.outlineMaterial == null)
            {
                Debug.LogWarningFormat("Missing Outline Material");
                return;
            }
            _outlinePass.Setup(renderer.cameraColorTarget, RenderTargetHandle.CameraTarget);
            renderer.EnqueuePass(_outlinePass);
        }


        private class OutlinePass : ScriptableRenderPass
        {
            private readonly Material _outlineMaterial;
            private RenderTargetHandle _temporaryColorTexture;
            
            private RenderTargetIdentifier Source { get; set; }
            private RenderTargetHandle Destination { get; set; }

            public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination) => 
                (Source, Destination) = (source, destination);

            public OutlinePass(Material outlineMaterial) => 
                _outlineMaterial = outlineMaterial;


            // This method is called before executing the render pass.
            // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
            // When empty this render pass will render to the active camera render target.
            // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
            // The render pipeline will ensure target setup and clearing happens in an performance manner.
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {

            }

            // Here you can implement the rendering logic.
            // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
            // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = CommandBufferPool.Get("_OutlinePass");
                var opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDescriptor.depthBufferBits = 0;

                if (Destination == RenderTargetHandle.CameraTarget)
                {
                    cmd.GetTemporaryRT(_temporaryColorTexture.id, opaqueDescriptor, FilterMode.Point);
                    Blit(cmd, Source, _temporaryColorTexture.Identifier(), _outlineMaterial);
                    Blit(cmd, _temporaryColorTexture.Identifier(), Source);

                }
                else Blit(cmd, Source, Destination.Identifier(), _outlineMaterial);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            /// Cleanup any allocated resources that were created during the execution of this render pass.
            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (Destination == RenderTargetHandle.CameraTarget)
                    cmd.ReleaseTemporaryRT(_temporaryColorTexture.id);
            }
        }

        [System.Serializable]
        public class OutlineSettings
        {
            public Material outlineMaterial;
        }
    }
}


