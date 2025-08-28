using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public static class ParaAnimatorDefine
{
	// Token: 0x0400001B RID: 27
	public static string BaseLayer = "Base Layer";

	// Token: 0x0400001C RID: 28
	public static string Sitting = "Sitting";

	// Token: 0x0400001D RID: 29
	public static string AdditiveLayer = "Additive Layer";

	// Token: 0x0400001E RID: 30
	public static string AllHandsLayer = "All Hands Layer";

	// Token: 0x0400001F RID: 31
	public static string LeftHandLayer = "Left Hand Layer";

	// Token: 0x04000020 RID: 32
	public static string RightHandLayer = "Right Hand Layer";

	// Token: 0x04000021 RID: 33
	public static string ActionLayer = "Action Layer";

	// Token: 0x04000022 RID: 34
	public static string SystemPostureLayer = "System Posture Layer";

	// Token: 0x04000023 RID: 35
	public static string FXLayer = "FX Layer";

	// Token: 0x04000024 RID: 36
	public static readonly int Forward = Animator.StringToHash("Forward");

	// Token: 0x04000025 RID: 37
	public static readonly int Turn = Animator.StringToHash("Turn");

	// Token: 0x04000026 RID: 38
	public static readonly int GroundId = Animator.StringToHash("OnGround");

	// Token: 0x04000027 RID: 39
	public static readonly int CrouchId = Animator.StringToHash("Crouch");

	// Token: 0x04000028 RID: 40
	public static readonly int JumpId = Animator.StringToHash("Jump");

	// Token: 0x04000029 RID: 41
	public static readonly int JumpLegId = Animator.StringToHash("JumpLeg");

	// Token: 0x0400002A RID: 42
	public static readonly int SwimStateId = Animator.StringToHash("SwimState");

	// Token: 0x0400002B RID: 43
	public static readonly int[] SystemPosture = new int[]
	{
		Animator.StringToHash("Sys_WholeBody"),
		Animator.StringToHash("Sys_UpperBody"),
		Animator.StringToHash("Sys_LowerBody"),
		Animator.StringToHash("Sys_Head"),
		Animator.StringToHash("Sys_Arms"),
		Animator.StringToHash("Sys_LeftArm"),
		Animator.StringToHash("Sys_RightArm"),
		Animator.StringToHash("Sys_LeftHand"),
		Animator.StringToHash("Sys_RightHand")
	};

	// Token: 0x0400002C RID: 44
	public static readonly int Seated = Animator.StringToHash("Seated");

	// Token: 0x0400002D RID: 45
	public static readonly int TPose = Animator.StringToHash("TPose");

	// Token: 0x0400002E RID: 46
	public static readonly int IKPose = Animator.StringToHash("IKPose");

	// Token: 0x0400002F RID: 47
	public static readonly int GestureLeft = Animator.StringToHash("GestureLeft");

	// Token: 0x04000030 RID: 48
	public static readonly int GestureLeftWeight = Animator.StringToHash("GestureLeftWeight");

	// Token: 0x04000031 RID: 49
	public static readonly int GestureRight = Animator.StringToHash("GestureRight");

	// Token: 0x04000032 RID: 50
	public static readonly int GestureRightWeight = Animator.StringToHash("GestureRightWeight");

	// Token: 0x04000033 RID: 51
	public static string[] SystemPostureState = new string[] { "proxy_system", "proxy_system_upperbody", "proxy_system_lowerbody", "proxy_system_head", "proxy_system_arms", "proxy_system_leftarm", "proxy_system_rightarm", "proxy_system_lefthand", "proxy_system_righthand" };
}
