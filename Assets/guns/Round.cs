using System;

namespace guns
{
    
    public abstract class Round
    {
        //Simplest option
        //public Dictionary<string, string> _attributes = new Dictionary<string, string>();
        
        //Could store this as a JSON or something, would make storing the data easier as well as getting values

        //Relate to storage
        public abstract float GetWeight();
        public abstract float GetLength();
        
        //These feed Muzzle Velocity and accuracy
        public abstract float GetProjectileMass();
        public abstract float GetMuzzleEnergy();
        public abstract float GetCoefOfForm();
        
        
        public float GetBallisticCoef()
        {
            return (float) (GetProjectileMass() / (Math.Pow(GetDiameter(), 2) * GetCoefOfForm()));
        }

        public abstract float GetDiameter();
        

    }
    
}