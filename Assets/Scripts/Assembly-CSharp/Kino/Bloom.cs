using UnityEngine;

namespace Kino
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Kino Image Effects/Bloom")]
	public class Bloom : MonoBehaviour
	{
		[SerializeField]
		[Range(0f, 1f)]
		private float _exposure = 0.3f;

		[SerializeField]
		[Range(0f, 5f)]
		private float _radius = 2f;

		[SerializeField]
		[Range(0f, 2f)]
		private float _intensity = 0.5f;

		[SerializeField]
		private bool _antiFlicker;

		[SerializeField]
		[HideInInspector]
		private Shader _shader;

		private Material _material;

		public float exposure
		{
			get
			{
				return _exposure;
			}
			set
			{
				_exposure = value;
			}
		}

		public float radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
			}
		}

		public float intensity
		{
			get
			{
				return _intensity;
			}
			set
			{
				_intensity = value;
			}
		}

		public bool antiFlicker
		{
			get
			{
				return _antiFlicker;
			}
			set
			{
				_antiFlicker = value;
			}
		}

		private RenderTexture GetTempBuffer(int width, int height)
		{
			return RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.DefaultHDR);
		}

		private void ReleaseTempBuffer(RenderTexture rt)
		{
			RenderTexture.ReleaseTemporary(rt);
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (_material == null)
			{
				_material = new Material(_shader);
				_material.hideFlags = HideFlags.DontSave;
			}
			float num = Mathf.Log(source.height, 2f) + _radius - 5f;
			int num2 = (int)num;
			int num3 = Mathf.Max(2, num2);
			_material.SetFloat("_SampleScale", 0.5f + num - (float)num2);
			_material.SetFloat("_Intensity", _intensity);
			float value = 0f - Mathf.Log(_exposure * 0.998f + 0.001f);
			_material.SetFloat("_Prefilter", value);
			if (_antiFlicker)
			{
				_material.EnableKeyword("PREFILTER_MEDIAN");
			}
			else
			{
				_material.DisableKeyword("PREFILTER_MEDIAN");
			}
			RenderTexture[] array = new RenderTexture[num3 + 1];
			RenderTexture[] array2 = new RenderTexture[num3 + 1];
			int num4 = source.width;
			int num5 = source.height;
			for (int i = 0; i < num3 + 1; i++)
			{
				array[i] = GetTempBuffer(num4, num5);
				if (i > 0 && i < num3)
				{
					array2[i] = GetTempBuffer(num4, num5);
				}
				num4 /= 2;
				num5 /= 2;
			}
			Graphics.Blit(source, array[0], _material, 0);
			for (int j = 0; j < num3; j++)
			{
				Graphics.Blit(array[j], array[j + 1], _material, 1);
			}
			_material.SetTexture("_BaseTex", array[num3 - 1]);
			Graphics.Blit(array[num3], array2[num3 - 1], _material, 2);
			for (int num6 = num3 - 1; num6 > 1; num6--)
			{
				_material.SetTexture("_BaseTex", array[num6 - 1]);
				Graphics.Blit(array2[num6], array2[num6 - 1], _material, 2);
			}
			_material.SetTexture("_BaseTex", source);
			Graphics.Blit(array2[1], destination, _material, 3);
			for (int k = 0; k < num3 + 1; k++)
			{
				ReleaseTempBuffer(array[k]);
				ReleaseTempBuffer(array2[k]);
			}
		}
	}
}
