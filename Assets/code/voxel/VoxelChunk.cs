using System.Collections.Generic;
using UnityEngine;

namespace voxel
{
    public class VoxelChunk
    {

        //private Voxel[,,] _voxels = new Voxel[32,32,32];
        public const int ChunkSize = 32;
        public Dictionary<Vector3, Voxel> _voxels { get; private set; } = new Dictionary<Vector3, Voxel>();

        //Maybe cache mesh stuff here and only update when needed
    
    
    
        public VoxelChunk()
        {
        
        
        }
    
        //Init from data
    
        //init from code

        public WorldMaterial GetMaterialFromVoxel(int x, int y, int z)
        {
        
            return null;
        }
    
        //To store data, use a palette thingy so stuff goes from Mats to integers corresponding with material indecies on the palette
        //Maybe 0 corresponds with air always?
    

    }
}