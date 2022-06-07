using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum DownSample { off = 1, half = 2, third = 3, quarter = 4 };

public class VolumetricLightFeature : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        public DownSample downsampling = DownSample.off;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public float intensity = 1;
        public float scattering = -0.4f;
        public float steps = 25;
        public float maxDistance = 75;
        public float jitter;
        public GaussBlur gaussBlur;
    }

    [System.Serializable]
    public class GaussBlur
    {
        public float amount;
        public float samples;
    }

    public Settings settings = new Settings();
    public Shader shader;

    private VolumetricLightPass _pass;
    private Material _material;

    public override void Create()
    {
        if (shader != null)
            _material = new Material(shader);

        _pass = new VolumetricLightPass(_material);
        _pass.settings = settings;
        _pass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _pass.ConfigureInput(ScriptableRenderPassInput.Color);
        RenderTargetIdentifier cameraColorTargetIdent = renderer.cameraColorTarget;
        _pass.Setup(cameraColorTargetIdent);
        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_material);
    }
}
