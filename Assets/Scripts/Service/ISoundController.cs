public interface ISoundController
{
    void PlayEffect(SFXType type, float volume, float pitch);
    void PlayBGM(BGMType type);
    void StopBGM();
    void ChangeBGMVolume();
}

public enum SFXType
{
    ObtainEraser,
    ObtainPaint,
    GameClear,
    GameOver,
    Test
}

public enum BGMType
{

}