using System;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class AudioDescriptionPreprocessor : AssetPostprocessor
{
	// Token: 0x06000054 RID: 84 RVA: 0x000047FC File Offset: 0x000029FC
	public override uint GetVersion()
	{
		return base.GetVersion();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003394 File Offset: 0x00001594
	public void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] clips)
	{
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004804 File Offset: 0x00002A04
	private void OnPostprocessAudio(AudioClip clip)
	{
		AudioImporter audioImporter = (AudioImporter)base.assetImporter;
		AudioImporterSampleSettings audioImporterSampleSettings = default(AudioImporterSampleSettings);
		audioImporterSampleSettings.loadType = 2;
		audioImporterSampleSettings.compressionFormat = 1;
		audioImporterSampleSettings.quality = 0.5f;
		audioImporter.defaultSampleSettings = audioImporterSampleSettings;
	}
}
