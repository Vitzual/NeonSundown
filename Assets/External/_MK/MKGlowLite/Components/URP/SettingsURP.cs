//////////////////////////////////////////////////////
// MK Glow Settings URP 	    	    	        //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Glow.URP
{
    internal sealed class SettingsURP : MK.Glow.Settings
    {
        public static implicit operator SettingsURP(MK.Glow.URP.MKGlowLite input)
        {
            SettingsURP settings = new SettingsURP();
            
            //Main
            settings.debugView = input.debugView.value;
            settings.workflow = input.workflow.value;
            settings.selectiveRenderLayerMask = input.selectiveRenderLayerMask.value;
            settings.anamorphicRatio = input.anamorphicRatio.value;

            //Bloom
            settings.bloomThreshold = input.bloomThreshold.value;
            settings.bloomScattering = input.bloomScattering.value;
            settings.bloomIntensity = input.bloomIntensity.value;

            //LensSurface
            settings.allowLensSurface = input.allowLensSurface.value;
            settings.lensSurfaceDirtTexture = input.lensSurfaceDirtTexture.value;
            settings.lensSurfaceDirtIntensity = input.lensSurfaceDirtIntensity.value;
            settings.lensSurfaceDiffractionTexture = input.lensSurfaceDiffractionTexture.value;
            settings.lensSurfaceDiffractionIntensity = input.lensSurfaceDiffractionIntensity.value;

            return settings;
        }
    }
}
