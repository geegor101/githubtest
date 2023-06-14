using System;
using System.Collections.Generic;
using UnityEngine;
using voxel;

namespace managers
{
    public class LevelManager
    {
        
        //Use a dict for the main level as the initial block in the save
        //Subsequent blocks will be phys objs of a single chunk each
        // -> when these init add them to the phys manager

        
        public Dictionary<Vector3, VoxelChunk> mainLevel = new Dictionary<Vector3, VoxelChunk>();
        public List<VoxelChunk> floatingChunks = new List<VoxelChunk>();

        public Voxel GetVoxelAtPos(Vector3 location)
        {
            
            GetAdjustedCoordinates(location, out Vector3 chunkCoord, out Vector3 subChunkCoord);
            return mainLevel[chunkCoord]._voxels[subChunkCoord];
        }

        public void GetAdjustedCoordinates(Vector3 input, out Vector3 chunkCoord, out Vector3 subChunkCoord)
        {
            chunkCoord = new Vector3();
            subChunkCoord = new Vector3();
            
            subChunkCoord.x = input.x % VoxelChunk.ChunkSize;
            chunkCoord.x = (input.x - subChunkCoord.x) / VoxelChunk.ChunkSize;

            subChunkCoord.y = input.y % VoxelChunk.ChunkSize;
            chunkCoord.y = (input.y - subChunkCoord.y) / VoxelChunk.ChunkSize;

            subChunkCoord.z = input.z % VoxelChunk.ChunkSize;
            chunkCoord.z = (input.z - subChunkCoord.z) / VoxelChunk.ChunkSize;
        }
    }
}