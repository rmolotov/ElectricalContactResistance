//    Copyright (C) 2020 NedMakesGames

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program. If not, see <https://www.gnu.org/licenses/>.

#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out half DistanceAtten, out half ShadowAtten) {
#ifdef SHADERGRAPH_PREVIEW
    Direction = half3(0.5, 0.5, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
#if SHADOWS_SCREEN
    half4 clipPos = TransformWorldToHClip(WorldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}

float GetLightIntensity(float3 color) {
    return max(color.r, max(color.g, color.b));
}

#ifndef SHADERGRAPH_PREVIEW
// This function gets additional light data and calculates realtime shadows
Light GetAdditionalLightForToon(int pixelLightIndex, float3 worldPosition) {
    // Convert the pixel light index to the light data index
    int perObjectLightIndex = GetPerObjectLightIndex(pixelLightIndex);
    // Call the URP additional light algorithm. This will not calculate shadows, since we don't pass a shadow mask value
    Light light = GetAdditionalPerObjectLight(perObjectLightIndex, worldPosition);
    // Manually set the shadow attenuation by calculating realtime shadows
    light.shadowAttenuation = AdditionalLightRealtimeShadow(perObjectLightIndex, worldPosition);
    return light;
}
#endif

void AddAdditionalLights_float(float Smoothness, float3 WorldPosition, float3 WorldNormal, float3 WorldView,
    float MainDiffuse, float MainSpecular, float3 MainColor,
    out float Diffuse, out float Specular, out float3 Color) {

    float mainIntensity = GetLightIntensity(MainColor);
    Diffuse = MainDiffuse * mainIntensity;
    Specular = MainSpecular * mainIntensity;
    Color = MainColor;

#ifndef SHADERGRAPH_PREVIEW
    float highestDiffuse = Diffuse;

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i) {
        // In order to receive shadows in 2020.2, we must do a little extra than before. This function takes care of it
        Light light = GetAdditionalLightForToon(i, WorldPosition);
        half NdotL = saturate(dot(WorldNormal, light.direction));
        half atten = light.distanceAttenuation * light.shadowAttenuation * GetLightIntensity(light.color);
        half thisDiffuse = atten * NdotL;
        half thisSpecular = LightingSpecular(thisDiffuse, light.direction, WorldNormal, WorldView, 1, Smoothness);
        Diffuse += thisDiffuse;
        Specular += thisSpecular;

        if (thisDiffuse > highestDiffuse) {
            highestDiffuse = thisDiffuse;
            Color = light.color;
        }
    }
#endif
}

#endif