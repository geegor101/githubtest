﻿namespace voxel
{
    public struct Voxel
    {
        public WorldMaterial _Material;
        //TODO: Change this to init to Air 
        public Voxel(WorldMaterial material)
        {
            _Material = material;
        }
    }
}