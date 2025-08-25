using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Token: 0x0200005C RID: 92
public static class EditorUtil
{
	// Token: 0x060002B2 RID: 690 RVA: 0x000112E4 File Offset: 0x0000F4E4
	public static Texture2D CoverCompressTexture(Texture2D source)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, 7, 2);
		Graphics.Blit(source, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(source.width, source.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00011364 File Offset: 0x0000F564
	public static bool DownLoadTexture(string path, string url, Action<string, bool> action)
	{
		if (File.Exists(path))
		{
			if (action != null)
			{
				action(path, true);
			}
			return true;
		}
		ThreadPool.QueueUserWorkItem(delegate(object pngPath)
		{
			DownLoadProxy.DownloadPng(pngPath as string, path, delegate(int code)
			{
				if (action != null)
				{
					action(path, false);
				}
			});
		}, url);
		return false;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x000113C4 File Offset: 0x0000F5C4
	public static string TransformTimeStamp(string time)
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long num = long.Parse(time + "0000000");
		TimeSpan timeSpan = new TimeSpan(num);
		return dateTime.Add(timeSpan).ToString("d");
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00011418 File Offset: 0x0000F618
	public static string Clamp(string inputString, int maxLength)
	{
		Encoding encoding = new ASCIIEncoding();
		int num = 0;
		byte[] bytes = encoding.GetBytes(inputString);
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] == 63)
			{
				num += 2;
			}
			else
			{
				num++;
			}
			if (num > maxLength)
			{
				return inputString.Substring(0, i);
			}
		}
		return inputString;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00011460 File Offset: 0x0000F660
	public static int GetRealStringCount(string inputString)
	{
		Encoding encoding = new ASCIIEncoding();
		int num = 0;
		byte[] bytes = encoding.GetBytes(inputString);
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] == 63)
			{
				num += 2;
			}
			else
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001149B File Offset: 0x0000F69B
	public static Camera CreateCamera(string cameraName)
	{
		Camera camera = new GameObject(cameraName).AddComponent<Camera>();
		camera.enabled = true;
		camera.gameObject.SetActive(true);
		camera.allowDynamicResolution = true;
		return camera;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000114C4 File Offset: 0x0000F6C4
	public static Texture2D CameraCapture(RenderTexture render, Camera camera, Rect rect)
	{
		render = new RenderTexture((int)rect.width, (int)rect.height, 24);
		camera.targetTexture = render;
		RenderTexture.active = render;
		camera.Render();
		Texture2D texture2D = new Texture2D((int)rect.width, (int)rect.height, 5, false);
		texture2D.ReadPixels(rect, 0, 0);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00011524 File Offset: 0x0000F724
	public static Texture2D LoadTexture(string path)
	{
		Texture2D texture2D = null;
		if (File.Exists(path))
		{
			byte[] array = File.ReadAllBytes(path);
			texture2D = new Texture2D(2, 2);
			if (path.EndsWith(".JPG") || path.EndsWith(".jpg"))
			{
				if (array.Length > 1184366592)
				{
					EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_Texture_TooLarge"), "OK", null);
					return null;
				}
			}
			else if (path.EndsWith(".png") && array.Length > 1579155456)
			{
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_Texture_TooLarge"), "OK", null);
				return null;
			}
			ImageConversion.LoadImage(texture2D, array);
			if (texture2D.imageContentsHash.ToString().Equals("17aa97d92db24d84f27615264d1a9975"))
			{
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_Texture_TooLarge"), "OK", null);
				return null;
			}
		}
		return texture2D;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00011614 File Offset: 0x0000F814
	public static void SaveTextureToLocal(Texture2D texture, string fileName)
	{
		byte[] array = ImageConversion.EncodeToPNG(texture);
		File.WriteAllBytes(fileName, array);
		ParaLog.Log(string.Format("Crop an image: {0}", fileName));
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0001163F File Offset: 0x0000F83F
	public static void DrawTexture(float width, float height, Texture2D texture, string name)
	{
		EditorUtil.DrawTexture((int)width, (int)height, texture, name);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001164C File Offset: 0x0000F84C
	public static void DrawTexture(Rect rect, Texture2D texture, string name)
	{
		EditorUtil.DrawTexture((int)rect.width, (int)rect.height, texture, name);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00011668 File Offset: 0x0000F868
	public static void DrawFrame(Rect rect, Color color, float lineWidth = 2f)
	{
		EditorGUI.DrawRect(new Rect(rect.x, rect.y, lineWidth, rect.height), color);
		EditorGUI.DrawRect(new Rect(rect.x + rect.width - lineWidth, rect.y, lineWidth, rect.height), color);
		EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, lineWidth), color);
		EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - lineWidth, rect.width, lineWidth), color);
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00011710 File Offset: 0x0000F910
	public static void DrawTexture(int width, int height, Texture2D texture, string name)
	{
		if (texture != null)
		{
			GUILayout.Label(texture, new GUILayoutOption[]
			{
				GUILayout.Width((float)width),
				GUILayout.Height((float)height),
				GUILayout.ExpandWidth(false),
				GUILayout.ExpandHeight(false)
			});
			return;
		}
		GUILayout.Box(name, new GUILayoutOption[]
		{
			GUILayout.Width((float)width),
			GUILayout.Height((float)height),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001178B File Offset: 0x0000F98B
	public static void DrawRedStar()
	{
		GUI.color = Color.red;
		GUILayout.Label("*", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUI.color = Color.white;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000117BC File Offset: 0x0000F9BC
	public static string GetPrefabAssetPath(GameObject gameObject)
	{
		if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
		{
			return AssetDatabase.GetAssetPath(gameObject);
		}
		if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
		{
			return AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource<GameObject>(gameObject));
		}
		PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
		if (prefabStage != null)
		{
			return prefabStage.assetPath;
		}
		return null;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00011804 File Offset: 0x0000FA04
	public static void OpenInfo(string title, string message, string btnName = "OK", string btnExtra = null)
	{
		InfoWindow window = EditorWindow.GetWindow<InfoWindow>();
		window.SetInfo(title, message, btnName, btnExtra, null, null, false);
		window.Show();
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00011828 File Offset: 0x0000FA28
	public static void OpenBoxInfo(string title, string message, string btnName = "OK", string btnExtra = null, string titleExtra = null)
	{
		InfoWindow window = EditorWindow.GetWindow<InfoWindow>();
		window.SetInfo(title, message, btnName, btnExtra, titleExtra, null, true);
		window.Show();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00011850 File Offset: 0x0000FA50
	public static void OpenActionInfo(string title, string message, string btnName = "OK", Action funcName = null, string btnExtra = null, Action funcExtra = null, string titleExtra = null, string image = null, bool isBox = false)
	{
		InfoWindow window = EditorWindow.GetWindow<InfoWindow>();
		window.SetInfo(title, message, btnName, btnExtra, titleExtra, null, isBox);
		window.SetCallBack(funcName, funcExtra);
		window.Show();
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00011880 File Offset: 0x0000FA80
	public static List<Material> GetMaterials(GameObject obj, bool includeDisabled)
	{
		List<Material> list = new List<Material>();
		MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>(includeDisabled);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].sharedMaterials.Length; j++)
			{
				if (componentsInChildren[i].sharedMaterials[j] != null)
				{
					list.Add(componentsInChildren[i].sharedMaterials[j]);
				}
			}
			list.AddRange(componentsInChildren[i].sharedMaterials);
		}
		SkinnedMeshRenderer[] componentsInChildren2 = obj.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].sharedMaterials.Length; l++)
			{
				if (componentsInChildren2[k].sharedMaterials[l] != null)
				{
					list.Add(componentsInChildren2[k].sharedMaterials[l]);
				}
			}
		}
		return list;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0001194F File Offset: 0x0000FB4F
	public static string ReplaceSpace(string str)
	{
		return str.Replace(" ", "\u00a0");
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00011964 File Offset: 0x0000FB64
	public static void DeleteDirectory(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		if (path.EndsWith("\\") || path.EndsWith("/"))
		{
			path = path.Remove(path.Length - 1);
		}
		if (Directory.Exists(path))
		{
			Directory.Delete(path, true);
		}
		string text = path + ".meta";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
	}
}
