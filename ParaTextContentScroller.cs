using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000048 RID: 72
public class ParaTextContentScroller : MonoBehaviour
{
	// Token: 0x0600008C RID: 140 RVA: 0x00004F2C File Offset: 0x0000312C
	private void Awake()
	{
		this.uiText = base.GetComponent<Text>();
		this.rectTransform = base.GetComponent<RectTransform>();
		if (this.rectTransform != null)
		{
			this.startPosition = this.rectTransform.localPosition;
			this.maxWidth = this.rectTransform.rect.width;
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00004F89 File Offset: 0x00003189
	private void OnEnable()
	{
		this.moveToLeft = true;
		this.moveToRight = true;
		this.UpdateText();
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004F9F File Offset: 0x0000319F
	private void OnDisable()
	{
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
			this.m_coroutine = null;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00004FBC File Offset: 0x000031BC
	public void UpdateText()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.rectTransform == null || this.uiText == null)
		{
			return;
		}
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
			this.m_coroutine = null;
		}
		this.rectTransform.localPosition = this.startPosition;
		float contentWidth = this.GetContentWidth(this.uiText.text);
		if (contentWidth > this.maxWidth * 0.8f)
		{
			this.rectTransform.sizeDelta = new Vector2(contentWidth, this.rectTransform.sizeDelta.y);
			Vector3 localPosition = this.rectTransform.localPosition;
			this.targetLeft = localPosition - new Vector3(contentWidth * 0.5f + this.maxWidth * 0.5f, 0f, 0f);
			this.targetRight = localPosition + new Vector3(contentWidth * 0.5f + this.maxWidth * 0.5f, 0f, 0f);
			this.m_coroutine = base.StartCoroutine(this.EnumMoveToLeft());
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000050E4 File Offset: 0x000032E4
	private float GetContentWidth(string content)
	{
		this.uiText.font.RequestCharactersInTexture(content, this.uiText.fontSize, this.uiText.fontStyle);
		float num = 0f;
		foreach (char c in content)
		{
			CharacterInfo characterInfo;
			this.uiText.font.GetCharacterInfo(c, ref characterInfo, this.uiText.fontSize, this.uiText.fontStyle);
			num += (float)characterInfo.advance;
		}
		return num;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0000516F File Offset: 0x0000336F
	private IEnumerator EnumMoveToLeft()
	{
		while (this.moveToLeft)
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetLeft, Time.deltaTime * this.scrollSpeed);
			if (Mathf.Abs(this.targetLeft.x - base.transform.localPosition.x) < 0.2f)
			{
				this.moveToLeft = false;
				this.moveToRight = true;
				base.StartCoroutine(this.EnumMoveToRight());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0000517E File Offset: 0x0000337E
	private IEnumerator EnumMoveToRight()
	{
		while (this.moveToRight)
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetRight, Time.deltaTime * this.scrollSpeed);
			if (Mathf.Abs(this.targetRight.x - base.transform.localPosition.x) < 0.2f)
			{
				this.moveToRight = false;
				this.moveToLeft = true;
				base.StartCoroutine(this.EnumMoveToLeft());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private float scrollSpeed = 50f;

	// Token: 0x0400017D RID: 381
	private bool moveToLeft = true;

	// Token: 0x0400017E RID: 382
	private bool moveToRight = true;

	// Token: 0x0400017F RID: 383
	private Vector3 targetLeft;

	// Token: 0x04000180 RID: 384
	private Vector3 targetRight;

	// Token: 0x04000181 RID: 385
	private RectTransform rectTransform;

	// Token: 0x04000182 RID: 386
	private Text uiText;

	// Token: 0x04000183 RID: 387
	private Coroutine m_coroutine;

	// Token: 0x04000184 RID: 388
	private Vector3 startPosition;

	// Token: 0x04000185 RID: 389
	private float maxWidth;
}
