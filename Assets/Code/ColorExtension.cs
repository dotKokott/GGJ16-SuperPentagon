﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ColorExtension {

    public static Vector3 ToHSV( this Color color ) {
        float H = 0, S = 0, V = 0;

        if ( (double)color.b > (double)color.g && (double)color.b > (double)color.r )
            RGBToHSVHelper( 4f, color.b, color.r, color.g, out H, out S, out V );
        else if ( (double)color.g > (double)color.r )
            RGBToHSVHelper( 2f, color.g, color.b, color.r, out H, out S, out V );
        else
            RGBToHSVHelper( 0.0f, color.r, color.g, color.b, out H, out S, out V );

        return new Vector3( H, S, V );
    }

    public static void RGBToHSV( Color rgbColor, out float H, out float S, out float V ) {
        if ( (double)rgbColor.b > (double)rgbColor.g && (double)rgbColor.b > (double)rgbColor.r )
            RGBToHSVHelper( 4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V );
        else if ( (double)rgbColor.g > (double)rgbColor.r )
            RGBToHSVHelper( 2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V );
        else
            RGBToHSVHelper( 0.0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V );
    }

    private static void RGBToHSVHelper( float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V ) {
        V = dominantcolor;
        if ( (double)V != 0.0 ) {
            float num1 = (double)colorone <= (double)colortwo ? colorone : colortwo;
            float num2 = V - num1;
            if ( (double)num2 != 0.0 ) {
                S = num2 / V;
                H = offset + ( colorone - colortwo ) / num2;
            } else {
                S = 0.0f;
                H = offset + ( colorone - colortwo );
            }
            H = H / 6f;
            if ( (double)H >= 0.0 )
                return;
            H = H + 1f;
        } else {
            S = 0.0f;
            H = 0.0f;
        }
    }

    public static Color HSVToRGB( float H, float S, float V ) {
        return HSVToRGB( H, S, V, true );
    }

    public static Color HSVToRGB( float H, float S, float V, bool hdr ) {
        var white = Color.white;
        if ( (double)S == 0.0 ) {
            white.r = V;
            white.g = V;
            white.b = V;
        } else if ( (double)V == 0.0 ) {
            white.r = 0.0f;
            white.g = 0.0f;
            white.b = 0.0f;
        } else {
            white.r = 0.0f;
            white.g = 0.0f;
            white.b = 0.0f;
            float num1 = S;
            float num2 = V;
            float f = H * 6f;
            int num3 = (int)Mathf.Floor( f );
            float num4 = f - (float)num3;
            float num5 = num2 * ( 1f - num1 );
            float num6 = num2 * (float)( 1.0 - (double)num1 * (double)num4 );
            float num7 = num2 * (float)( 1.0 - (double)num1 * ( 1.0 - (double)num4 ) );
            switch ( num3 + 1 ) {
                case 0:
                    white.r = num2;
                    white.g = num5;
                    white.b = num6;
                    break;
                case 1:
                    white.r = num2;
                    white.g = num7;
                    white.b = num5;
                    break;
                case 2:
                    white.r = num6;
                    white.g = num2;
                    white.b = num5;
                    break;
                case 3:
                    white.r = num5;
                    white.g = num2;
                    white.b = num7;
                    break;
                case 4:
                    white.r = num5;
                    white.g = num6;
                    white.b = num2;
                    break;
                case 5:
                    white.r = num7;
                    white.g = num5;
                    white.b = num2;
                    break;
                case 6:
                    white.r = num2;
                    white.g = num5;
                    white.b = num6;
                    break;
                case 7:
                    white.r = num2;
                    white.g = num7;
                    white.b = num5;
                    break;
            }
            if ( !hdr ) {
                white.r = Mathf.Clamp( white.r, 0.0f, 1f );
                white.g = Mathf.Clamp( white.g, 0.0f, 1f );
                white.b = Mathf.Clamp( white.b, 0.0f, 1f );
            }
        }
        return white;
    }
}
