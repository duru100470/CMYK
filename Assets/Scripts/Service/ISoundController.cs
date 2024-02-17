public interface ISoundController
{
    void PlayEffect(SFXType type, float volume, float pitch);
    void PlayBGM(BGMType type);
    void StopBGM();
    void ChangeBGMVolume();
}

public enum SFXType
{
    Test
}

public enum BGMType
{

}