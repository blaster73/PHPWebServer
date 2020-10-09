using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCreator : MonoBehaviour
{

    public PlayerInfo pI;
    public RawImage qrCode;

    private void OnEnable() {
        GenerateQR();
    }

    [ContextMenu("Generate QR")]
    public void GenerateQR(){

        Texture2D myQR = generateQR(pI.ID.ToString());
        qrCode.texture = myQR;
        qrCode.material.mainTexture = myQR;

    }

    public Texture2D generateQR(string text) {

        var encoded = new Texture2D (256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;

    }

    private static Color32[] Encode(string textForEncoding, int width, int height) {

        var writer = new BarcodeWriter {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions {
                    Height = height,
                    Width = width
                }
        };
        
        return writer.Write(textForEncoding);

    }
}
