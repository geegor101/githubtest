using System.Collections.Generic;
using managers;
using UnityEngine;

namespace voxel
{
    public class WorldMaterial : GeegorAsset
    {
        public static Dictionary<AssetLoc, WorldMaterial> WorldMaterials = new Dictionary<AssetLoc,WorldMaterial>();

        public Material material;
    
        //TODO Setup any physical chars here

        public bool Occluder()
        {
            return true;
        }

        public WorldMaterial(AssetLoc loc, Material material) : base(loc)
        {
            this.material = material;
            WorldMaterials[loc] = this;
        }

    }
}