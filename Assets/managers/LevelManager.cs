using System;
using System.Collections.Generic;
using System.Numerics;
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

        public Voxel getVoxelAtPos(Vector3 location)
        {
            Vector3 chunkCoord;
            Vector3 subChunkCoord;
            getAdjustedCoordinates(location, out chunkCoord, out subChunkCoord);
            return mainLevel[chunkCoord]._voxels[subChunkCoord];
        }

        public void getAdjustedCoordinates(Vector3 input, out Vector3 chunkCoord, out Vector3 subChunkCoord)
        {
            subChunkCoord.X = input.X % VoxelChunk.ChunkSize;
            chunkCoord.X = (input.X - subChunkCoord.X) / VoxelChunk.ChunkSize;
            
            subChunkCoord.Y = input.Y % VoxelChunk.ChunkSize;
            chunkCoord.Y = (input.Y - subChunkCoord.Y) / VoxelChunk.ChunkSize;
            
            subChunkCoord.Z = input.Z % VoxelChunk.ChunkSize;
            chunkCoord.Z = (input.Z - subChunkCoord.Z) / VoxelChunk.ChunkSize;
        }
    }
}