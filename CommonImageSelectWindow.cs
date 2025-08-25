using System;
using UnityEditor;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class CommonImageSelectWindow : EditorWindow
{
	// Token: 0x0600033C RID: 828 RVA: 0x00015000 File Offset: 0x00013200
	private void OnEnable()
	{
		base.minSize = new Vector2(550f, 450f);
		base.maxSize = new Vector2(550f, 450f);
		this._cutRect = new Rect(0f, 0f, 200f, 200f);
		this._displayRect = new Rect(0f, 0f, 400f, 300f);
		this.PickWindows();
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001507B File Offset: 0x0001327B
	private void OnDisable()
	{
		if (this._callback != null)
		{
			this._callback(null);
			this._callback = null;
		}
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00015098 File Offset: 0x00013298
	private void OnGUI()
	{
		if (this._originalTexture == null)
		{
			base.Close();
			GUIUtility.ExitGUI();
			return;
		}
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_commonImage_text1"), EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Height(this._kTopTitleLen - 2f) });
		GUILayout.EndHorizontal();
		GUILayout.Space(-2f);
		this.DrawTextureArea();
		GUILayout.Space(this._displayRect.height);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this._description, new GUILayoutOption[]
		{
			GUILayout.Width(270f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(50f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_commonImage_btn0"), new GUILayoutOption[] { GUILayout.Width(130f) }))
		{
			this.OpenPickWindows();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_commonImage_text3"), new GUILayoutOption[]
		{
			GUILayout.Width(350f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_commonImage_text4"), new GUILayoutOption[]
		{
			GUILayout.Width(400f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Box("", new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(true),
			GUILayout.Height(1f)
		});
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(100f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_cancel"), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(30f)
		}))
		{
			base.Close();
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(50f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_ok"), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(30f)
		}))
		{
			this.CutTexture();
			if (this._callback != null)
			{
				this._callback(this._subjectTexture);
				this._callback = null;
			}
			base.Close();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001530E File Offset: 0x0001350E
	private void DrawTextureArea()
	{
		EditorGUI.DrawPreviewTexture(this._textureRect, this._originalTexture);
		this.UpdateTouch();
		this.DrawDarkAreaExceptCut();
		this.DrawCutFrame();
		base.Repaint();
	}

	// Token: 0x06000340 RID: 832 RVA: 0x0001533C File Offset: 0x0001353C
	private void DrawDarkAreaExceptCut()
	{
		float num = (base.minSize.x - this._displayRect.width) / 2f;
		Color color;
		color..ctor(0f, 0f, 0f, 0.618f);
		float num2 = 0.1f;
		EditorGUI.DrawRect(new Rect(num, this._cutRect.y, this._cutRect.x - num + num2, this._cutRect.height + num2), color);
		EditorGUI.DrawRect(new Rect(this._cutRect.x + this._cutRect.width, this._cutRect.y, this._displayRect.width - (this._cutRect.x - num) - this._cutRect.width + num2, this._cutRect.height + num2), color);
		EditorGUI.DrawRect(new Rect(num, this._kTopTitleLen, this._displayRect.width, this._cutRect.y - this._kTopTitleLen), color);
		EditorGUI.DrawRect(new Rect(num, this._cutRect.y + this._cutRect.height, this._displayRect.width, this._displayRect.height - this._cutRect.y - this._cutRect.height + this._kTopTitleLen), color);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x000154A3 File Offset: 0x000136A3
	private void DrawCutFrame()
	{
		EditorUtil.DrawFrame(this._cutRect, Color.white, 2f);
		this.DrawScaleRects();
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000154C0 File Offset: 0x000136C0
	private void DrawScaleRects()
	{
		if (this._scaleRectsDisplay == null)
		{
			return;
		}
		for (int i = 0; i < this._scaleRectsDisplay.Length; i++)
		{
			EditorUtil.DrawFrame(this._scaleRectsDisplay[i], Color.white, 2f);
		}
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00015504 File Offset: 0x00013704
	private bool IsScaleRectsContainsPoint(Vector2 point, out CommonImageSelectWindow.ScaleRectIndex index)
	{
		for (int i = 0; i < this._scaleRects.Length; i++)
		{
			if (this._scaleRects[i].Contains(point))
			{
				index = (CommonImageSelectWindow.ScaleRectIndex)i;
				return true;
			}
		}
		index = CommonImageSelectWindow.ScaleRectIndex.None;
		return false;
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00015544 File Offset: 0x00013744
	private void UpdateTouch()
	{
		switch (Event.current.type)
		{
		case 0:
			if (this.IsScaleRectsContainsPoint(Event.current.mousePosition, out this._touchScaleRectIndex))
			{
				this.SaveCutRect();
				this._isTouchScale = true;
				return;
			}
			if (this._cutRect.Contains(Event.current.mousePosition))
			{
				this.SaveTouchOffset();
				this._isTouchMove = true;
				return;
			}
			break;
		case 1:
			this._isTouchMove = false;
			this._isTouchScale = false;
			return;
		case 2:
			break;
		case 3:
			if (this._isTouchMove)
			{
				this.UpdateCutRect();
				return;
			}
			if (this._isTouchScale && !this._touchedScaleRect.Contains(Event.current.mousePosition))
			{
				this.UpdateCutRect();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00015604 File Offset: 0x00013804
	private void SaveTouchOffset()
	{
		this._touchOffset.x = Event.current.mousePosition.x - this._cutRect.x;
		this._touchOffset.y = Event.current.mousePosition.y - this._cutRect.y;
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0001565D File Offset: 0x0001385D
	private void SaveCutRect()
	{
		this._lastCutRect = this._cutRect;
		this._touchedScaleRect = this._scaleRects[(int)this._touchScaleRectIndex];
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00015684 File Offset: 0x00013884
	private void UpdateCutRect()
	{
		Vector2 mousePosition = Event.current.mousePosition;
		if (this._isTouchMove)
		{
			this.UpdateCutRectTouchMove(mousePosition);
		}
		else if (this._isTouchScale)
		{
			this.LimitCutRectTouchPos(ref mousePosition);
			this.UpdateCutRectTouchScale(mousePosition);
			this.KeepCutRectRatio();
		}
		this.CalculateScaleRects();
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000156D0 File Offset: 0x000138D0
	private void UpdateCutRectTouchMove(Vector2 touchPos)
	{
		this._cutRect.x = Mathf.Clamp(touchPos.x - this._touchOffset.x, this._textureRect.x, this._textureRect.x + this._textureRect.width - this._cutRect.width);
		this._cutRect.y = Mathf.Clamp(touchPos.y - this._touchOffset.y, this._textureRect.y, this._textureRect.y + this._textureRect.height - this._cutRect.height);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00015780 File Offset: 0x00013980
	private void LimitCutRectTouchPos(ref Vector2 touchPos)
	{
		Vector2 vector;
		vector..ctor(this._lastCutRect.x + this._lastCutRect.width, this._lastCutRect.y + this._lastCutRect.height);
		Vector2 vector2;
		vector2..ctor(this._initCutRect.width * 0.2f, this._initCutRect.height * 0.2f);
		Vector2 vector3;
		vector3..ctor(this._textureRect.x + this._textureRect.width, this._textureRect.y + this._textureRect.height);
		switch (this._touchScaleRectIndex)
		{
		case CommonImageSelectWindow.ScaleRectIndex.TopLeft:
			touchPos.x = Mathf.Clamp(touchPos.x, this._textureRect.x, vector.x - vector2.x);
			touchPos.y = Mathf.Clamp(touchPos.y, this._textureRect.y, vector.y - vector2.y);
			return;
		case CommonImageSelectWindow.ScaleRectIndex.TopRight:
			touchPos.x = Mathf.Clamp(touchPos.x, this._lastCutRect.x + vector2.x, vector3.x);
			touchPos.y = Mathf.Clamp(touchPos.y, this._textureRect.y, vector.y - vector2.y);
			return;
		case CommonImageSelectWindow.ScaleRectIndex.BottomLeft:
			touchPos.x = Mathf.Clamp(touchPos.x, this._textureRect.x, vector.x - vector2.x);
			touchPos.y = Mathf.Clamp(touchPos.y, this._lastCutRect.y + vector2.y, vector3.y);
			return;
		case CommonImageSelectWindow.ScaleRectIndex.BottomRight:
			touchPos.x = Mathf.Clamp(touchPos.x, this._lastCutRect.x + vector2.x, vector3.x);
			touchPos.y = Mathf.Clamp(touchPos.y, this._lastCutRect.y + vector2.y, vector3.y);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001598C File Offset: 0x00013B8C
	private void UpdateCutRectTouchScale(Vector2 touchPos)
	{
		Vector2 vector;
		vector..ctor(this._lastCutRect.x + this._lastCutRect.width, this._lastCutRect.y + this._lastCutRect.height);
		switch (this._touchScaleRectIndex)
		{
		case CommonImageSelectWindow.ScaleRectIndex.TopLeft:
			this._cutRect.x = touchPos.x;
			this._cutRect.y = touchPos.y;
			this._cutRect.width = vector.x - this._cutRect.x;
			this._cutRect.height = vector.y - this._cutRect.y;
			return;
		case CommonImageSelectWindow.ScaleRectIndex.TopRight:
			this._cutRect.x = this._lastCutRect.x;
			this._cutRect.y = touchPos.y;
			this._cutRect.width = touchPos.x - this._lastCutRect.x;
			this._cutRect.height = vector.y - this._cutRect.y;
			return;
		case CommonImageSelectWindow.ScaleRectIndex.BottomLeft:
			this._cutRect.x = touchPos.x;
			this._cutRect.y = this._lastCutRect.y;
			this._cutRect.width = vector.x - this._cutRect.x;
			this._cutRect.height = touchPos.y - this._lastCutRect.y;
			return;
		case CommonImageSelectWindow.ScaleRectIndex.BottomRight:
			this._cutRect.x = this._lastCutRect.x;
			this._cutRect.y = this._lastCutRect.y;
			this._cutRect.width = touchPos.x - this._lastCutRect.x;
			this._cutRect.height = touchPos.y - this._lastCutRect.y;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00015B74 File Offset: 0x00013D74
	private void KeepCutRectRatio()
	{
		float num;
		float num2;
		CommonImageSelectWindow.GetKeepRectRationSize(this._cutRect, this._originalCutRectWidthHeightRatio, out num, out num2);
		Vector2 vector;
		vector..ctor(this._cutRect.x + this._cutRect.width, this._cutRect.y + this._cutRect.height);
		switch (this._touchScaleRectIndex)
		{
		case CommonImageSelectWindow.ScaleRectIndex.TopLeft:
			this._cutRect.x = vector.x - num;
			this._cutRect.y = vector.y - num2;
			break;
		case CommonImageSelectWindow.ScaleRectIndex.TopRight:
			this._cutRect.y = vector.y - num2;
			break;
		case CommonImageSelectWindow.ScaleRectIndex.BottomLeft:
			this._cutRect.x = vector.x - num;
			break;
		}
		this._cutRect.width = num;
		this._cutRect.height = num2;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00015C52 File Offset: 0x00013E52
	private static void GetKeepRectRationSize(Rect _rect, float ratioToKeep, out float width, out float height)
	{
		if (_rect.width / _rect.height > ratioToKeep)
		{
			width = _rect.height * ratioToKeep;
			height = _rect.height;
			return;
		}
		width = _rect.width;
		height = _rect.width / ratioToKeep;
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00015C91 File Offset: 0x00013E91
	public void PickWindows()
	{
		this._texturePath = EditorUtility.OpenFilePanel(SdkLangManager.Get("str_sdk_commonImage_btn2"), "", "png,jpg");
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00015CB4 File Offset: 0x00013EB4
	public void OpenPickWindows()
	{
		Texture2D texture2D = EditorUtil.LoadTexture(EditorUtility.OpenFilePanel(SdkLangManager.Get("str_sdk_commonImage_btn2"), "", "png,jpg"));
		if (texture2D != null)
		{
			this._originalTexture = texture2D;
			base.Repaint();
		}
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00015CF8 File Offset: 0x00013EF8
	public void FetchTexture(Rect rect, string title, string des, Action<Texture2D> func)
	{
		base.titleContent.text = title;
		this._originalCutRect = rect;
		this._description = des;
		this._callback = func;
		Texture2D texture2D = EditorUtil.LoadTexture(this._texturePath);
		if (texture2D != null)
		{
			this._originalTexture = texture2D;
			float num = (float)this._originalTexture.height / (float)this._originalTexture.width;
			float num2 = this._displayRect.height / this._displayRect.width;
			this._scaleByHeight = num > num2;
			this.CalculateScaleRate();
			this.CalculateTextureRect();
			this.CalculateCutRect();
			this.SaveInitCutRect();
			this.CalculateScaleRects();
			base.minSize = new Vector2(base.minSize.x, this._displayRect.height + 150f);
			base.maxSize = new Vector2(base.maxSize.x, this._displayRect.height + 150f);
			base.Repaint();
		}
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00015DF4 File Offset: 0x00013FF4
	private void CalculateScaleRate()
	{
		if (this._scaleByHeight)
		{
			this._scaleRate = this._displayRect.height / (float)this._originalTexture.height;
			return;
		}
		this._scaleRate = this._displayRect.width / (float)this._originalTexture.width;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00015E48 File Offset: 0x00014048
	private void CalculateTextureRect()
	{
		this._textureRect.width = (float)this._originalTexture.width * this._scaleRate;
		this._textureRect.height = (float)this._originalTexture.height * this._scaleRate;
		this._textureRect.x = (base.minSize.x - this._textureRect.width) / 2f;
		this._textureRect.y = this._kTopTitleLen + (this._displayRect.height - this._textureRect.height) / 2f;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00015EE8 File Offset: 0x000140E8
	private void CalculateCutRect()
	{
		this._cutRect.x = this._textureRect.x;
		this._cutRect.y = this._textureRect.y;
		float width = this._textureRect.width;
		float height = this._textureRect.height;
		this._originalCutRectWidthHeightRatio = this._originalCutRect.width / this._originalCutRect.height;
		float num = height * this._originalCutRectWidthHeightRatio;
		float num2 = width / this._originalCutRectWidthHeightRatio;
		bool flag;
		if (this._scaleByHeight)
		{
			flag = num2 <= this._displayRect.height;
		}
		else
		{
			flag = num > this._displayRect.width;
		}
		if (flag)
		{
			this._cutRect.width = width;
			this._cutRect.height = num2;
			return;
		}
		this._cutRect.width = num;
		this._cutRect.height = height;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00015FD3 File Offset: 0x000141D3
	private void SaveInitCutRect()
	{
		this._initCutRect = this._cutRect;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015FE1 File Offset: 0x000141E1
	private void CalculateScaleRects()
	{
		this._scaleRects = this.CreateScaleRects(20f);
		this._scaleRectsDisplay = this.CreateScaleRects(4f);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00016008 File Offset: 0x00014208
	private Rect[] CreateScaleRects(float rectLength)
	{
		return new Rect[]
		{
			new Rect(this._cutRect.x - rectLength * 0.5f, this._cutRect.y - rectLength * 0.5f, rectLength, rectLength),
			new Rect(this._cutRect.x - rectLength * 0.5f + this._cutRect.width, this._cutRect.y - rectLength * 0.5f, rectLength, rectLength),
			new Rect(this._cutRect.x - rectLength * 0.5f, this._cutRect.y - rectLength * 0.5f + this._cutRect.height, rectLength, rectLength),
			new Rect(this._cutRect.x - rectLength * 0.5f + this._cutRect.width, this._cutRect.y - rectLength * 0.5f + this._cutRect.height, rectLength, rectLength)
		};
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001611C File Offset: 0x0001431C
	public void CutTexture()
	{
		float num = (this._cutRect.x - this._textureRect.x) / this._scaleRate;
		float num2 = (this._textureRect.y + this._textureRect.height - (this._cutRect.y + this._cutRect.height)) / this._scaleRate;
		float num3 = this._cutRect.width / this._scaleRate;
		float num4 = this._cutRect.height / this._scaleRate;
		this._subjectTexture = new Texture2D((int)num3, (int)num4);
		Color[] pixels = this._originalTexture.GetPixels((int)num, (int)num2, (int)num3, (int)num4);
		this._subjectTexture.SetPixels(0, 0, (int)num3, (int)num4, pixels);
		this._subjectTexture.Apply();
	}

	// Token: 0x040001C4 RID: 452
	private Rect _originalCutRect;

	// Token: 0x040001C5 RID: 453
	private Action<Texture2D> _callback;

	// Token: 0x040001C6 RID: 454
	private Texture2D _originalTexture;

	// Token: 0x040001C7 RID: 455
	private Texture2D _subjectTexture;

	// Token: 0x040001C8 RID: 456
	private Rect _cutRect;

	// Token: 0x040001C9 RID: 457
	private Rect _initCutRect;

	// Token: 0x040001CA RID: 458
	private CommonImageSelectWindow.ScaleRectIndex _touchScaleRectIndex = CommonImageSelectWindow.ScaleRectIndex.None;

	// Token: 0x040001CB RID: 459
	private Rect[] _scaleRects;

	// Token: 0x040001CC RID: 460
	private Rect[] _scaleRectsDisplay;

	// Token: 0x040001CD RID: 461
	private float _scaleRate = 1f;

	// Token: 0x040001CE RID: 462
	private string _description;

	// Token: 0x040001CF RID: 463
	private Rect _displayRect;

	// Token: 0x040001D0 RID: 464
	private bool _isTouchMove;

	// Token: 0x040001D1 RID: 465
	private bool _isTouchScale;

	// Token: 0x040001D2 RID: 466
	private float _kTopTitleLen = 22f;

	// Token: 0x040001D3 RID: 467
	private Rect _textureRect;

	// Token: 0x040001D4 RID: 468
	private string _texturePath;

	// Token: 0x040001D5 RID: 469
	private float _originalCutRectWidthHeightRatio;

	// Token: 0x040001D6 RID: 470
	private const float SCALE_RECT_LENGTH_DISPLAY = 4f;

	// Token: 0x040001D7 RID: 471
	private const float SCALE_RECT_LENGTH = 20f;

	// Token: 0x040001D8 RID: 472
	private const float SCALE_MIN_RATIO = 0.2f;

	// Token: 0x040001D9 RID: 473
	private Vector2 _touchOffset;

	// Token: 0x040001DA RID: 474
	private Rect _lastCutRect;

	// Token: 0x040001DB RID: 475
	private Rect _touchedScaleRect;

	// Token: 0x040001DC RID: 476
	private bool _scaleByHeight;

	// Token: 0x020000B8 RID: 184
	private enum ScaleRectIndex
	{
		// Token: 0x040003B8 RID: 952
		None = -1,
		// Token: 0x040003B9 RID: 953
		TopLeft,
		// Token: 0x040003BA RID: 954
		TopRight,
		// Token: 0x040003BB RID: 955
		BottomLeft,
		// Token: 0x040003BC RID: 956
		BottomRight
	}
}
