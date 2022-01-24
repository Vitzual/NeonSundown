//////////////////////////////////////////////////////
// MK Glow Editor URP					      		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using MK.Glow.Editor;

namespace MK.Glow.URP.Editor
{
	using Tooltips = MK.Glow.Editor.EditorHelper.EditorUIContent.Tooltips;

	[VolumeComponentEditor(typeof(MK.Glow.URP.MKGlowLite))]
	internal class MKGlowEditor : VolumeComponentEditor
	{
		//Behaviors
		private SerializedDataParameter _showEditorMainBehavior;
		private SerializedDataParameter _showEditorBloomBehavior;
		private SerializedDataParameter _showEditorLensSurfaceBehavior;
		private SerializedDataParameter _isInitialized;

		//Main
		private SerializedDataParameter _debugView;
		private SerializedDataParameter _workflow;
		private SerializedDataParameter _selectiveRenderLayerMask;
		private SerializedDataParameter _anamorphicRatio;

		//Bloom
		private SerializedDataParameter _bloomThreshold;
		private SerializedDataParameter _bloomScattering;
		private SerializedDataParameter _bloomIntensity;

		//Lens Surface
		private SerializedDataParameter _allowLensSurface;
		private SerializedDataParameter _lensSurfaceDirtTexture;
		private SerializedDataParameter _lensSurfaceDirtIntensity;
		private SerializedDataParameter _lensSurfaceDiffractionTexture;
		private SerializedDataParameter _lensSurfaceDiffractionIntensity;

		PropertyFetcher<MK.Glow.URP.MKGlowLite> propertyFetcher;
		
		public override void OnEnable()
		{
			propertyFetcher = new PropertyFetcher<MK.Glow.URP.MKGlowLite>(serializedObject);

			//Editor
			_showEditorBloomBehavior = Unpack(propertyFetcher.Find(x => x.showEditorBloomBehavior));
			_showEditorMainBehavior = Unpack(propertyFetcher.Find(x => x.showEditorMainBehavior));
			_showEditorBloomBehavior = Unpack(propertyFetcher.Find(x => x.showEditorBloomBehavior));
			_showEditorLensSurfaceBehavior = Unpack(propertyFetcher.Find(x => x.showEditorLensSurfaceBehavior));
			_isInitialized = Unpack(propertyFetcher.Find(x => x.isInitialized));

			//Main
			_debugView = Unpack(propertyFetcher.Find(x => x.debugView));
			_workflow = Unpack(propertyFetcher.Find(x => x.workflow));
			_selectiveRenderLayerMask = Unpack(propertyFetcher.Find(x => x.selectiveRenderLayerMask));
			_anamorphicRatio = Unpack(propertyFetcher.Find(x => x.anamorphicRatio));

			//Bloom
			_bloomThreshold = Unpack(propertyFetcher.Find(x => x.bloomThreshold));
			_bloomScattering = Unpack(propertyFetcher.Find(x => x.bloomScattering));
			_bloomIntensity = Unpack(propertyFetcher.Find(x => x.bloomIntensity));

			_allowLensSurface = Unpack(propertyFetcher.Find(x => x.allowLensSurface));
			_lensSurfaceDirtTexture = Unpack(propertyFetcher.Find(x => x.lensSurfaceDirtTexture));
			_lensSurfaceDirtIntensity = Unpack(propertyFetcher.Find(x => x.lensSurfaceDirtIntensity));
			_lensSurfaceDiffractionTexture = Unpack(propertyFetcher.Find(x => x.lensSurfaceDiffractionTexture));
			_lensSurfaceDiffractionIntensity = Unpack(propertyFetcher.Find(x => x.lensSurfaceDiffractionIntensity));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if(_isInitialized.value.boolValue == false)
			{
				_bloomIntensity.value.floatValue = 1f;
				_bloomIntensity.overrideState.boolValue = true;

				_lensSurfaceDirtIntensity.value.floatValue = 2.5f;
				_lensSurfaceDirtIntensity.overrideState.boolValue = true;
				_lensSurfaceDiffractionIntensity.value.floatValue = 2f;
				_lensSurfaceDiffractionIntensity.overrideState.boolValue = true;

				_isInitialized.value.boolValue = true;
			}

			EditorHelper.VerticalSpace();

			EditorHelper.EditorUIContent.IsNotSupportedWarning();
			EditorHelper.EditorUIContent.XRUnityVersionWarning();
			if(_workflow.value.enumValueIndex == 1)
            {
				EditorHelper.EditorUIContent.SelectiveWorkflowDeprecated();
			}
			
			if(EditorHelper.HandleBehavior(_showEditorMainBehavior.value.serializedObject.targetObject, EditorHelper.EditorUIContent.mainTitle, "", _showEditorMainBehavior.value, null))
			{
				PropertyField(_debugView, Tooltips.debugView);
				PropertyField(_workflow, Tooltips.workflow);
				EditorHelper.EditorUIContent.SelectiveWorkflowVRWarning((Workflow)_workflow.value.enumValueIndex);
                if(_workflow.value.enumValueIndex == 1)
                {
                    PropertyField(_selectiveRenderLayerMask, Tooltips.selectiveRenderLayerMask);
                }
				PropertyField(_anamorphicRatio, Tooltips.anamorphicRatio);
				EditorHelper.VerticalSpace();
			}
			
			if(EditorHelper.HandleBehavior(_showEditorBloomBehavior.value.serializedObject.targetObject, EditorHelper.EditorUIContent.bloomTitle, "", _showEditorBloomBehavior.value, null))
			{
				if(_workflow.value.enumValueIndex == 0)
					PropertyField(_bloomThreshold, Tooltips.bloomThreshold);
				PropertyField(_bloomScattering, Tooltips.bloomScattering);
				PropertyField(_bloomIntensity, Tooltips.bloomIntensity);
				_bloomIntensity.value.floatValue = Mathf.Max(0, _bloomIntensity.value.floatValue);

				EditorHelper.VerticalSpace();
			}

			if(EditorHelper.HandleBehavior(_showEditorLensSurfaceBehavior.value.serializedObject.targetObject, EditorHelper.EditorUIContent.lensSurfaceTitle, "", _showEditorLensSurfaceBehavior.value, _allowLensSurface.value))
			{
				using (new EditorGUI.DisabledScope(!_allowLensSurface.value.boolValue))
                {
					EditorHelper.DrawHeader(EditorHelper.EditorUIContent.dirtTitle);
					PropertyField(_lensSurfaceDirtTexture, Tooltips.lensSurfaceDirtTexture);
					PropertyField(_lensSurfaceDirtIntensity, Tooltips.lensSurfaceDirtIntensity);
					_lensSurfaceDirtIntensity.value.floatValue = Mathf.Max(0, _lensSurfaceDirtIntensity.value.floatValue);
					EditorGUILayout.Space();
					EditorHelper.DrawHeader(EditorHelper.EditorUIContent.diffractionTitle);
					PropertyField(_lensSurfaceDiffractionTexture, Tooltips.lensSurfaceDiffractionTexture);
					PropertyField(_lensSurfaceDiffractionIntensity, Tooltips.lensSurfaceDiffractionIntensity);
					_lensSurfaceDiffractionIntensity.value.floatValue = Mathf.Max(0, _lensSurfaceDiffractionIntensity.value.floatValue);
				}
				EditorHelper.VerticalSpace();
			}

			EditorHelper.DrawSplitter();

			serializedObject.ApplyModifiedProperties();
		}
    }
}
#endif