using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(AudioSource))]
public class CharacterSFX : MonoBehaviour
{
    
    private AudioSource _audioSource;
    private CharacterMovement _characterMovement;
    
    private static readonly string FootStepSFXAddress = "Assets/SFX/Character/Footstep_Walk.wav";
    private AudioClip _footStepClip;

    [SerializeField] private float footStepInterval = 0.5f;
    private float _timeSinceLastFootStep = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        _characterMovement = GetComponent<CharacterMovement>();
        
        Addressables.LoadAssetAsync<AudioClip>(FootStepSFXAddress).Completed += handle => { _footStepClip = handle.Result; };
    }

    private void Update()
    {
        PlayFootStep();
        PlayLandingSFX();
    }

    private void PlayFootStep()
    {
        if (_characterMovement.isMoving && _characterMovement.isGround)
        {
            if (_timeSinceLastFootStep > footStepInterval)
            {
                _audioSource.PlayOneShot(_footStepClip);
                _timeSinceLastFootStep = 0;
            }
            else
            {
                _timeSinceLastFootStep += Time.deltaTime;
            }

        }
        else
        {
            _audioSource.Pause();
        }
    }

    private void PlayLandingSFX()
    {
        if (_characterMovement.isLanding)
        {
            _audioSource.PlayOneShot(_footStepClip);
        }
    }
}
