using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumetricLightFeature : ScriptableRendererFeature
{
    public Settings settings = new Settings();
    Pass pass;

    [System.Serializable]
    public class Settings
    {
        //future settings
        public Material material;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    /// <summary>
    /// Injects one or multiple <see cref="ScriptableRenderPass"/> in the renderer.
    /// </summary>
    /// <param name="renderer"> Renderer used for adding render passes.</param>
    /// <param name="renderingData"> Rendering state. Use this to setup render passes.</param>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        pass.Setup(cameraColorTargetIdent);
        renderer.EnqueuePass(pass);
    }

    /// <summary> 
    /// Initializes this feature's resources. This is called every time serialization happens.
    /// </summary>
    public override void Create()
    {
        pass = new Pass("Volumetric Light");
        name = "Volumetric Light";
        pass.settings = settings;
        pass.renderPassEvent = settings.renderPassEvent;
    }

    class Pass : ScriptableRenderPass
    {
        public Settings settings;
        private RenderTargetIdentifier source;
        private string profilerTag;

        public Pass(string profilerTag)
        {
            this.profilerTag = profilerTag;
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        /// <summary>
        /// Execute the pass. This is where custom rendering occurs.
        /// </summary>
        /// <param name="context"> Use this render context to issue any draw commands during execution. </param>
        /// <param name="renderingData"> Current rendering state information.</param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            
        }
    }
}
