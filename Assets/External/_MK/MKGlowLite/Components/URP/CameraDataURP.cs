//////////////////////////////////////////////////////
// MK Glow Camera Data URP  	    	    	    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2021 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow.URP
{
    internal class CameraDataURP : CameraData
    {
        public static implicit operator CameraDataURP(UnityEngine.Rendering.Universal.CameraData input)
        {
            CameraDataURP data = new CameraDataURP();

            data.width = input.cameraTargetDescriptor.width;
            data.height = input.cameraTargetDescriptor.height;
            #if UNITY_2020_2_OR_NEWER
                data.stereoEnabled = input.xrRendering;
            #else
                data.stereoEnabled = input.isStereoEnabled;
            #endif
            data.aspect = input.camera.aspect;
            data.worldToCameraMatrix = input.camera.worldToCameraMatrix;
            
            return data;
        }
    }
}
