using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using System;

public static class ImageMerge
{
    public static void saveMergedImages(Texture2D tex, string path) {
        //Sprite itemBGSprite = Resources.Load<Sprite>( "_Defaults/Item Images/_Background" );
        //Texture2D itemBGTex = itemBGSprite.texture;
        byte[] itemBGBytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes( "Assets/Resources/" + path , itemBGBytes );
    }
    public static Texture2D mergeTextures(Texture2D bottom, Texture2D top) {
        return bottom.AlphaBlend(top);
    }
    public static Texture2D AlphaBlend(this Texture2D aBottom, Texture2D aTop)
     {
         if (aBottom.width != aTop.width || aBottom.height != aTop.height)
             throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
         var bData = aBottom.GetPixels();
         var tData = aTop.GetPixels();
         int count = bData.Length;
         var rData = new Color[count];
         for(int i = 0; i < count; i++)
         {
             Color B = bData[i];
             Color T = tData[i];
             float srcF = T.a;
             float destF = 1f - T.a;
             float alpha = srcF + destF * B.a;
             Color R = (T * srcF + B * B.a * destF)/alpha;
             R.a = alpha;
             rData[i] = R;
         }
         var res = new Texture2D(aTop.width, aTop.height);
         res.SetPixels(rData);
         res.Apply();
         return res;
     }
}
