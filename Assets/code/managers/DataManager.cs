using UnityEngine;
using voxel;

namespace managers
{
    public static class DataManager
    {
        public const string modid = "default";
        //load materials before level data
        public static Material _Material = new Material(Shader.Find("Standard"));
        
        public static void init()
        {
            //WorldMaterial.WorldMaterials[new AssetLoc(modid, "test mat")] = new WorldMaterial();
            _Material.color = Color.red;
            new WorldMaterial(new AssetLoc(modid, "test mat"), _Material);
        }
    
    }
}