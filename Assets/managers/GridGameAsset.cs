using voxel;

namespace managers
{
    public abstract class GeegorAsset
    {
        public AssetLoc _AssetLoc { get; }

        protected GeegorAsset(AssetLoc loc)
        {
            _AssetLoc = loc;
        }
    }
}