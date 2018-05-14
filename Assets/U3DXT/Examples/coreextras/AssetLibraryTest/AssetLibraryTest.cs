using UnityEngine;
using System.Collections;
using U3DXT.iOS.Native.AssetsLibrary;

public class AssetLibraryTest : MonoBehaviour {

    // Use this for initialization
    ALAssetsLibrary assetsLibrary;


    void Start () {

    }


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, Screen.width-100, Screen.height-100));
        GUILayout.BeginVertical();
        if (GUILayout.Button("Get Asset Store Stuff", GUILayout.ExpandWidth(true), GUILayout.Height(100))) {
            ReadAssetStore();
        }
        GUILayout.EndVertical();    
        GUILayout.EndArea();
    }

    void ReadAssetStore()
    {
        assetsLibrary = new ALAssetsLibrary();
        assetsLibrary.EnumerateGroups( (uint)ALAssetsLibraryTypesofAsset.Album,
            delegate(ALAssetsGroup group, bool stop){
                // enumeration block, when group is null, there's no more
                if ( group == null )
                        return;
				var str = group.Value (ALAssetsGroup.PropertyName);
                Debug.Log ("Album Name: " + str);
            },
            delegate(U3DXT.iOS.Native.Foundation.NSError error){
                // error block
            }
        );
    }
}
