public interface ISoundController
{
    void PlayEffect(SFXType type, float volume, float pitch);
    void PlayBGM(BGMType type);
    void StopBGM();
}

public enum SFXType
{

}

public enum BGMType
{

}