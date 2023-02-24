using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;
        private Rigidbody _rb;

        [Space]
        [SerializeField] private float movementSpeed = 5f;
        private Vector3 _inputDir;
        
        private const float RotationSpeed = 1000f;

        private Animator _animator;
        private static readonly int IsWalkAnim = Animator.StringToHash("isWalk");
        
        public bool IsMoving() => joystick.Direction != Vector2.zero;

        public void IncreaseMovementSpeed(float value) => movementSpeed += value;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(IsWalkAnim, joystick.Direction != Vector2.zero);
            if (joystick.Direction == Vector2.zero)
            {
                _inputDir = Vector3.zero;
                return;
            }
            
            var joystickDirection = joystick.Direction.normalized;
            
            _inputDir = new Vector3(joystickDirection.x, 0, joystickDirection.y); 
            var toRotation = Quaternion.LookRotation(_inputDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation, 
                RotationSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _rb.velocity = _inputDir * movementSpeed;
        }
    }
}