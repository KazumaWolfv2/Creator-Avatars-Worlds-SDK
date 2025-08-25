using System;
using UnityEditor;
using UnityEngine;

// Token: 0x0200005B RID: 91
[CustomEditor(typeof(AnimatorParamDebugger))]
public class AnimatorParamDebuggerEditor : Editor
{
	// Token: 0x060002AF RID: 687 RVA: 0x00011130 File Offset: 0x0000F330
	private void OnEnable()
	{
		this.debugger = base.target as AnimatorParamDebugger;
		this.animator = this.debugger.GetComponentInChildren<Animator>();
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00011154 File Offset: 0x0000F354
	public override void OnInspectorGUI()
	{
		if (this.animator == null)
		{
			return;
		}
		Rect controlRect = EditorGUILayout.GetControlRect(Array.Empty<GUILayoutOption>());
		controlRect.height = EditorGUIUtility.singleLineHeight;
		float num = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
		{
			switch (animatorControllerParameter.type)
			{
			case 1:
			{
				float @float = this.animator.GetFloat(animatorControllerParameter.name);
				float num2 = EditorGUI.FloatField(controlRect, animatorControllerParameter.name, @float);
				this.animator.SetFloat(animatorControllerParameter.name, num2);
				controlRect.y += num;
				EditorGUILayout.Space(num);
				break;
			}
			case 3:
			{
				int integer = this.animator.GetInteger(animatorControllerParameter.name);
				int num3 = EditorGUI.IntField(controlRect, animatorControllerParameter.name, integer);
				this.animator.SetInteger(animatorControllerParameter.name, num3);
				controlRect.y += num;
				EditorGUILayout.Space(num);
				break;
			}
			case 4:
			{
				bool @bool = this.animator.GetBool(animatorControllerParameter.name);
				bool flag = EditorGUI.Toggle(controlRect, animatorControllerParameter.name, @bool);
				this.animator.SetBool(animatorControllerParameter.name, flag);
				controlRect.y += num;
				EditorGUILayout.Space(num);
				break;
			}
			}
		}
		base.serializedObject.ApplyModifiedProperties();
	}

	// Token: 0x0400017C RID: 380
	private AnimatorParamDebugger debugger;

	// Token: 0x0400017D RID: 381
	private Animator animator;
}
