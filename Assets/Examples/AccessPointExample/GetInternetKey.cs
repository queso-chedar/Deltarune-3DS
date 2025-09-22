using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GetInternetKey : MonoBehaviour
{
#if UNITY_EDITOR && UNITY_N3DS
    private uint Random1(uint seed)
    {
        seed = seed * 1103515245U + 12345U;
        return seed;
    }

    private uint SdbmHash(string str)
    {
        uint num = 0U;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        foreach (byte b in bytes)
        {
            num = (uint)b + (num << 6) + (num << 16) - num;
        }
        return num;
    }

    void Start()
    {
        string str = Application.companyName.Trim();
        string str2 = Application.productName.Trim();
        uint seed = this.SdbmHash(str + "::" + str2);
        string a = "0x" + this.Random1(seed).ToString("X4");
        if (a != PlayerSettings.N3DS.netLibKey)
        {
            Debug.LogError("Network Access Key is invalid. Expected value:");
            Debug.LogError(a);
        }
    }
#endif
}
