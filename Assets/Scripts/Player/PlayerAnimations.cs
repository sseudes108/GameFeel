using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController _player;

    //VFX
    [SerializeField] private ParticleSystem _moveParticleVFX;
    [SerializeField] private ParticleSystem _puffDustVFX;

    //Rotation
    [SerializeField] private Transform _cowboyHatSpriteTransform;
    [SerializeField] private Transform _playerSpriteTransform;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _cowboyHatTiltModifier = 2f;

    private void Awake() {
        _player = GetComponent<PlayerController>();
    }

    private void OnEnable() {
        PlayerController.OnJump += PlayPuffDustVFX;
    }
    private void OnDisable() {
        PlayerController.OnJump -= PlayPuffDustVFX;
    }
    
    private void Update() {
        DetectMoveDust();
        ApplyTilt();
    }

    private void PlayPuffDustVFX(){
        _puffDustVFX.Play();
    }
    
    private void DetectMoveDust(){
        if(_player.IsGrounded){
            if(!_moveParticleVFX.isPlaying){
                _moveParticleVFX.Play();
            }
        }else {
            if(_moveParticleVFX.isPlaying){
                _moveParticleVFX.Stop();
            }
        }
    }

    private void ApplyTilt(){
        float targetAngle;

        if(_player.FrameInput.Move.x > 0 ){
            targetAngle = _tiltAngle;
        }else if (_player.FrameInput.Move.x < 0 ){
            targetAngle = -_tiltAngle;
        }else {
            targetAngle = 0;
        }  

        PlayerSpriteTargetRotation(targetAngle);
        CowboyHatSpriteTargetRotation(targetAngle);
    }

    private void PlayerSpriteTargetRotation(float targetAngle){
        Quaternion currentPlayeSpriteRotation = _playerSpriteTransform.rotation;
        Quaternion targetPlayerSpriteRotation = Quaternion.Euler(currentPlayeSpriteRotation.x, currentPlayeSpriteRotation.y, targetAngle);

        _playerSpriteTransform.rotation = Quaternion.Lerp(currentPlayeSpriteRotation, targetPlayerSpriteRotation, _tiltSpeed * Time.deltaTime);
    }

    private void CowboyHatSpriteTargetRotation(float targetAngle){
        Quaternion currentHatSpriteRotation = _cowboyHatSpriteTransform.rotation;
        Quaternion targetHatSpriteRotation = Quaternion.Euler(currentHatSpriteRotation.x, currentHatSpriteRotation.y, -targetAngle / _cowboyHatTiltModifier);

        _cowboyHatSpriteTransform.rotation = Quaternion.Lerp(currentHatSpriteRotation, targetHatSpriteRotation, _tiltSpeed * _cowboyHatTiltModifier * Time.deltaTime);
    }
}
