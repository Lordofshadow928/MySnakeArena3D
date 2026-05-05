Shader "Toony Colors Pro 2/User/Toony Alpha Clip" {
	Properties {
		[TCP2HeaderHelp(Base)] _BaseColor ("Color", Vector) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Vector) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Vector) = (0.2,0.2,0.2,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		[TCP2Separator] [TCP2Header(Ramp Shading)] _RampThreshold ("Threshold", Range(0.01, 1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001, 1)) = 0.5
		[TCP2Separator] [Enum(ToonyColorsPro.ShaderGenerator.Culling)] _faceCulling ("Face Culling", Float) = 2
		[ToggleOff(_RECEIVE_SHADOWS_OFF)] _ReceiveShadowsOff ("Receive Shadows", Float) = 1
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_MatrixMVP;

			struct Vertex_Stage_Input
			{
				float3 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Hidden/InternalErrorShader"
	//CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}