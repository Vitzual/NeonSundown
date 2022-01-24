//////////////////////////////////////////////////////
// MK Install Wizard Configuration            		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
namespace MK.Glow.Editor.InstallWizard
{
    public enum RenderPipeline
    {
        Built_in_Legacy = 0,
        Built_in_PPSV2 = 1,
        //Lightweight = 2,
        Universal = 3
    }
    
    [System.Serializable]
    public class ExampleContainer
    {
        public string name = "";
        public UnityEngine.Object scene = null;
        public UnityEngine.Texture2D icon = null;

        public void DrawEditorButton()
        {
            if(UnityEngine.GUILayout.Button(icon, UnityEngine.GUILayout.Width(64), UnityEngine.GUILayout.Height(64)))
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(UnityEditor.AssetDatabase.GetAssetOrScenePath(scene));
        }
    }
}
#endif