using UnityEngine;

namespace Code
{
    public class TestSpringBehavior : MonoBehaviour
    {
        [AutofillBehavior] private Rigidbody _rigidbody;

        private SpringContext _springContext;

        
        [SerializeField] private float SpringConstant;
        [SerializeField] private float Length;
        [SerializeField] private float ViscousDampingCoefficient;
        [SerializeField] private float Scale;
        [SerializeField] private AnimationCurve _curve;

        private void Start()
        {
            this.AutofillAttributes();
        }

        private void FixedUpdate()
        {
            _springContext.SpringConstant = SpringConstant;
            _springContext.ViscousDampingCoefficient = ViscousDampingCoefficient;
            _springContext.Length = Length;
            Debug.DrawLine(_rigidbody.position, new Vector3(_springContext.Length, _rigidbody.position.y, _rigidbody.position.z),
                Color.blue);
            _rigidbody.AddForce( 
                Springs.CalculateDampedSpringForceA(_rigidbody.position.x, _rigidbody.velocity.x, _rigidbody.mass, _springContext),
                0,0);
            
        }
    }
}