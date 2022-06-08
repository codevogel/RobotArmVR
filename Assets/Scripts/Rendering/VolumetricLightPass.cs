using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class VolumetricLightPass : ScriptableRenderPass
{
    private enum ShaderPass
    {
        All = -1,
        RayMarching,
        GaussianBlurX,
        GaussianBlurY,
        Compositing,
        SampleDepth
    }

    public VolumetricLightFeature.Settings settings;

    private RenderTargetIdentifier _source;
    private RenderTargetHandle _tempTexture;
    private RenderTargetHandle _tempTexture2;
    private RenderTargetHandle _tempTexture3;

    ProfilingSampler _profilingSample = new ProfilingSampler("VolumetricLight");
    private Material _material;

    public void Setup(RenderTargetIdentifier colorSource)
    {
        _source = colorSource;
    }

    public VolumetricLightPass(Material material)
    {
        _material = material;
    }

    //public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    //{
    //    ConfigureTarget(_source);
    //}

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        RenderTextureDescriptor original = cameraTextureDescriptor;
        int divider = (int)settings.downsampling;

        // Applies down sampling.
        if (Camera.current != null)
        {
            cameraTextureDescriptor.width = (int)Camera.current.pixelRect.width / divider;
            cameraTextureDescriptor.height = (int)Camera.current.pixelRect.height / divider;
        }
        else
        {
            cameraTextureDescriptor.width /= divider;
            cameraTextureDescriptor.height /= divider;
        }

        cameraTextureDescriptor.colorFormat = RenderTextureFormat.R16;
        cameraTextureDescriptor.msaaSamples = 1;

        cmd.GetTemporaryRT(_tempTexture.id, cameraTextureDescriptor);
        ConfigureTarget(_tempTexture.Identifier());

        _tempTexture2.id = 1;
        cmd.GetTemporaryRT(_tempTexture2.id, cameraTextureDescriptor);
        ConfigureTarget(_tempTexture2.Identifier());

        _tempTexture3.id = 2;
        cmd.GetTemporaryRT(_tempTexture3.id, original);
        ConfigureTarget(_tempTexture3.Identifier());

        ConfigureClear(ClearFlag.All, Color.black);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_material == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get();

        using (new ProfilingScope(cmd, _profilingSample))
        {
            try
            {
                // Sets properties.
                _material.SetFloat("_Scattering", settings.scattering);
                _material.SetFloat("_Steps", settings.steps);
                _material.SetFloat("_JitterVolumetric", settings.jitter);
                _material.SetFloat("_MaxDistance", settings.maxDistance);
                _material.SetFloat("_Intensity", settings.intensity);
                _material.SetFloat("_GaussSamples", settings.gaussBlur.samples);
                _material.SetFloat("_GaussAmount", settings.gaussBlur.amount);

                // Raymarch.
                cmd.Blit(_source, _tempTexture.Identifier(), _material, 0);

                ////test copy back
                //cmd.Blit(_tempTexture.Identifier(), _source);

                // Bilateral blur x.
                cmd.Blit(_tempTexture.Identifier(), _tempTexture2.Identifier(), _material, 1);

                // Bilateral blur y.
                cmd.Blit(_tempTexture2.Identifier(), _tempTexture.Identifier(), _material, 2);
                cmd.SetGlobalTexture("_VolumetricTexture", _tempTexture.Identifier());

                // Downsample depth.
                cmd.Blit(_source, _tempTexture2.Identifier(), _material, 4);
                cmd.SetGlobalTexture("_LowResDepth", _tempTexture2.Identifier());

                // Upsample and composite
                cmd.Blit(_source, _tempTexture3.Identifier(), _material, 3);
                cmd.Blit(_tempTexture3.Identifier(), _source);



                //cmd.SetRenderTarget(_tempTexture.Identifier());

                //cmd.DrawProcedural(Matrix4x4.identity, _material, (int)ShaderPass.RayMarching, MeshTopology.Quads, 4, 1, null);

                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.RayMarching);
                //cmd.SetGlobalTexture("_MainTex", _material.mainTexture);



                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.GaussianBlurX);
                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.GaussianBlurY);
                //cmd.SetGlobalTexture("_VolumetricTexture", _tempTexture.Identifier());
                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.SampleDepth);
                //cmd.SetGlobalTexture("_LowResDepth", _tempTexture2.Identifier());
                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.Compositing);



                // raymarch
                //Blit(cmd, _source, _tempTexture.Identifier(), _material, ShaderPass.RayMarching);

                //// gaus blur
                //Blit(cmd, _tempTexture.Identifier(), _tempTexture2.Identifier(), _material, ShaderPass.GaussianBlurX);
                //Blit(cmd, _tempTexture2.Identifier(), _tempTexture.Identifier(), _material, ShaderPass.GaussianBlurY);

                //// prepare volumetric texture
                //cmd.SetGlobalTexture("_VolumetricTexture", _tempTexture.Identifier());

                //// downsample depth
                //Blit(cmd, _source, _tempTexture2.Identifier(), _material, ShaderPass.SampleDepth);

                //// prepare low res depth texture
                //cmd.SetGlobalTexture("_LowResDepth", _tempTexture2.Identifier());

                //// Upsample and composite
                //Blit(cmd, _source, _tempTexture3.Identifier(), _material, ShaderPass.Compositing);

                //cmd.CopyTexture(_tempTexture3.Identifier(), _source);
                //cmd.SetRenderTarget(_source);



                //cmd.SetGlobalVector("_BlitScaleBiasRt", new Vector4(1, 1, 0, 0));
                //cmd.SetGlobalVector("_BlitScaleBias", new Vector4(1, 1, 0, 0));
                //cmd.SetRenderTarget(new RenderTargetIdentifier(_source,0,CubemapFace.Unknown,-1), RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
                ////cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _material, 0, (int)ShaderPass.RayMarching);

                //cmd.DrawProcedural(Matrix4x4.identity, _material, (int)ShaderPass.RayMarching, MeshTopology.Quads, 4, 1, null);

                //cmd.SetRenderTarget(_source);
                //cmd.SetGlobalTexture("_SourceTex", _tempTexture.Identifier());
                //cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, _blitMat);


                //Blit2(cmd, _source, _tempTexture.Identifier(), _material, ShaderPass.RayMarching);

                //Blit2(cmd, _tempTexture.Identifier(), _tempTexture2.Identifier(), _material, ShaderPass.GaussianBlurX);






                context.ExecuteCommandBuffer(cmd);
            }
            catch
            {
                Debug.LogError($"{nameof(VolumetricLightFeature)} Error");
            }
        }



        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    private static void Blit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, ShaderPass pass)
    {
        cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material, 0, (int)pass);
        cmd.CopyTexture(source, destination);
        cmd.SetRenderTarget(destination);
    }

    private static void Blit2(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, ShaderPass pass)
    {
        cmd.SetGlobalTexture("_MainTex", source);
        cmd.SetRenderTarget(destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
        cmd.DrawProcedural(Matrix4x4.identity, material, (int)pass, MeshTopology.Quads, 4, 1);
    }
}
