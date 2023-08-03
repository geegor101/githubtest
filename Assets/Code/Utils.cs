using FishNet;
using UnityEngine;

namespace Code
{
    public static class Utils
    {

        
    }

    public class Springs
    {
        public static float deltaTime { get; private set; }

        public static float CalculateDampedSpringForce(float position, float velocity, SpringContext context)
        {
            if (deltaTime == 0)
                deltaTime = (float) InstanceFinder.TimeManager.TickDelta;
            
            float x = position - context.Length;
            return -1 * deltaTime * (context.SpringConstant * x + context.ViscousDampingCoefficient * velocity);
        }
        
        public static float CalculateDampedSpringForceA(float position, float velocity, float mass, SpringContext context)
        {
            if (deltaTime == 0)
                deltaTime = (float) InstanceFinder.TimeManager.TickDelta;
            
            // mass ( 2 * w_0 * zeta * velocity + w_0 ^ 2 * x) = F
            
            float x = position - context.Length;
            float w_0 = Mathf.Sqrt(context.SpringConstant / mass);
            float zeta = context.ViscousDampingCoefficient / (2 * Mathf.Sqrt(mass * context.SpringConstant));
            return - mass * (2 * zeta * velocity * w_0 + w_0 * w_0 * x);
            
            return -1 * deltaTime * (context.SpringConstant * x + context.ViscousDampingCoefficient * velocity);
        }

        public static float CalculateSpringFromCtx(float pos, float velocity, SpringCtx ctx) =>
            -ctx.Mass * (2 * ctx.DampingRatio * ctx.UndampedAngularFreq * velocity
                         + ctx.UndampedAngularFreq * ctx.UndampedAngularFreq * (pos - ctx.Length));


        public static float CalculateDampedSpringForceTest(float position, float velocity, SpringContext context, 
            AnimationCurve curve, float scale)
        {
            if (deltaTime == 0)
                deltaTime = (float) InstanceFinder.TimeManager.TickDelta;
            float x = position - context.Length;
            float coef = curve.Evaluate( Mathf.Abs(x * scale));
            return -coef * deltaTime * (context.SpringConstant * x + context.ViscousDampingCoefficient * velocity);
        }
    }
    
    public struct SpringContext
    {
        public float SpringConstant; // k
        public float Length;
        public float ViscousDampingCoefficient; //c
        //Displacement:
        // x = position - length

        public float VelocityCoefficient;
        public float PositionCoefficient;

        public SpringContext(float mass, float springConstant, float viscousDampingCoefficient, float length)
        {
            float w_0 = Mathf.Sqrt(springConstant / mass);
            float zeta = viscousDampingCoefficient / (2 * Mathf.Sqrt(mass * springConstant));
            Length = length;
            VelocityCoefficient = -mass * (2 * zeta * w_0);
            PositionCoefficient = -mass * w_0 * w_0;
            
            SpringConstant = springConstant;
            ViscousDampingCoefficient = viscousDampingCoefficient;
        }

    }
    
    public struct SpringCtx
    {
        /// <summary>
        /// ω0 
        /// </summary>
        public float UndampedAngularFreq;
            
        /// <summary>
        /// ζ - Overdamped when > 1, Critical when = 1
        /// </summary>
        public float DampingRatio;

        public float Mass;
        public float Length;
            
        public SpringCtx(float mass, float springConstant, float viscousDampingCoefficient, float length)
        {
            Mass = mass;
            Length = length;
            UndampedAngularFreq = Mathf.Sqrt(springConstant / mass);
            DampingRatio = 0.5f * viscousDampingCoefficient / Mathf.Sqrt(mass * springConstant);
        }
    }
    
    
}