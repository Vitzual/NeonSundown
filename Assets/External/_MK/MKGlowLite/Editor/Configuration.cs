//////////////////////////////////////////////////////
// MK Install Wizard Configuration            		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
namespace MK.Glow.Editor.InstallWizard
{
    //[CreateAssetMenu(fileName = "Configuration", menuName = "MK/Install Wizard/Create Configuration Asset")]
    public sealed class Configuration : ScriptableObject
    {
        #pragma warning disable CS0414

        internal static bool isReady 
        { 
            get
            { 
                if(_instance == null)
                    TryGetInstance();
                return _instance != null; 
            } 
        }

        [SerializeField]
        private RenderPipeline _renderPipeline = RenderPipeline.Built_in_PPSV2;

        [SerializeField]
        internal bool showInstallerOnReload = true;

        [SerializeField][Space]
        private Texture2D _titleImage = null;

        [SerializeField][Space]
        private Object _readMe = null;

        [SerializeField][Space]
        private Object _basePackageBuiltin = null;
        [SerializeField]
        private Object _basePackageLWRP = null;
        [SerializeField]
        private Object _basePackageURP = null;

        [SerializeField][Space]
        private Object _examplesPackageInc = null;
        [SerializeField]
        private Object _examplesPackageBuiltin = null;
        [SerializeField]
        private Object _examplesPackageLWRP = null;
        [SerializeField]
        private Object _examplesPackageURP = null;

        [SerializeField][Space]
        private ExampleContainer[] _examples = null;

        private static void LogAssetNotFoundError()
        {
            Debug.LogError("Could not find Install Wizard Configuration Asset, please try to import the package again.");
        }

        private static Configuration _instance = null;
        
        internal static Configuration TryGetInstance()
        {
            if(_instance == null)
            {
                string[] _guids = AssetDatabase.FindAssets("t:" + typeof(Configuration).Namespace + ".Configuration", null);
                if(_guids.Length > 0)
                {
                    _instance = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(_guids[0]), typeof(Configuration)) as Configuration;
                    if(_instance != null)
                        return _instance;
                    else
                    {
                        LogAssetNotFoundError();
                        return null;
                    }
                }
                else
                {
                    LogAssetNotFoundError();
                    return null;
                }
            }
            else
                return _instance;
        }

        internal static string TryGetPath()
        {
            if(isReady)
            {
                return AssetDatabase.GetAssetPath(_instance);
            }
            else
            {
                return string.Empty;
            }
        }

        internal static Texture2D TryGetTitleImage()
        {
            if(isReady)
            {
                return _instance._titleImage;
            }
            else
            {
                return null;
            }
        }

        internal static ExampleContainer[] TryGetExamples()
        {
            if(isReady)
            {
                return _instance._examples;
            }
            else
            {
                return null;
            }
        }

        internal static bool TryGetShowInstallerOnReload()
        {
            if(isReady)
            {
                return _instance.showInstallerOnReload;
            }
            else
            {
                return false;
            }
        }
        internal static void TrySetShowInstallerOnReload(bool v)
        {
            if(isReady)
            {
                _instance.showInstallerOnReload = v;
                SaveInstance();
            }
        }
        internal static RenderPipeline TryGetRenderPipeline()
        {
            if(isReady)
            {
                return _instance._renderPipeline;
            }
            else
            {
                return RenderPipeline.Built_in_PPSV2;
            }
        }
        internal static void TrySetRenderPipeline(RenderPipeline v)
        {
            if(isReady)
            {
                _instance._renderPipeline = v;

                SaveInstance();
            }
        }

        internal static void ImportShaders(RenderPipeline renderPipeline)
        {
            if(isReady)
            {
                switch(renderPipeline)
                {
                    case RenderPipeline.Built_in_Legacy:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._basePackageBuiltin), false);
                    break;
                    case RenderPipeline.Built_in_PPSV2:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._basePackageLWRP), false);
                    break;
                    //case RenderPipeline.Lightweight:
                    //    AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._basePackageLWRP), false);
                    //break;
                    case RenderPipeline.Universal:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._basePackageURP), false);
                    break;
                    default:
                    //All cases should be handled
                    break;
                }
                TrySetShowInstallerOnReload(false);
            }
        }

        internal static void SaveInstance()
        {
            if(isReady)
            {
                EditorUtility.SetDirty(_instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        internal static void ImportExamples(RenderPipeline renderPipeline)
        {
            if(isReady)
            {
                AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._examplesPackageInc), false);
                switch(renderPipeline)
                {
                    case RenderPipeline.Built_in_Legacy:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._examplesPackageBuiltin), false);
                    break;
                    case RenderPipeline.Built_in_PPSV2:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._examplesPackageLWRP), false);
                    break;
                    //case RenderPipeline.Lightweight:
                    //    AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._examplesPackageLWRP), false);
                    //break;
                    case RenderPipeline.Universal:
                        AssetDatabase.ImportPackage(AssetDatabase.GetAssetPath(_instance._examplesPackageURP), false);
                    break;
                    default:
                    //All cases should be handled
                    break;
                }
            }
        }

        internal static void OpenReadMe()
        {
            AssetDatabase.OpenAsset(_instance._readMe);
        }

        #pragma warning restore CS0414
    }
}
#endif