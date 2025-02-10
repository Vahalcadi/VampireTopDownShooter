using System.Collections;
using UnityEngine;

public class AudioManager : SingletonA<AudioManager>
{

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private UI_VolumeSlider[] volumeSliders;
    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySFX;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        Invoke("AllowSFX", 1f);

        playerTransform = PlayerFollowAssigner.OnAssignPlayerAsFollowTarget?.Invoke();
    }

    private void Start()
    {
        foreach (var slider in volumeSliders)
        {
            slider.LoadSlider(PlayerPrefs.GetFloat(slider.parameter));
        }

        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }
    private void Update()
    {
        
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //    return;

        if (!canPlaySFX)
            return;

        if (_source != null && Vector2.Distance(playerTransform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            //sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }

    }

    /*public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(9, bgm.Length);
        PlayBGM(bgmIndex);
    }*/

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;

            yield return new WaitForSeconds(.25f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX()
    {
        canPlaySFX = true;
    }
}
