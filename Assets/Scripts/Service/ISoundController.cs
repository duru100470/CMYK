public interface ISoundController
{
    void PlayEffect(SFXType type, float volume, float pitch);
    void PlayBGM(BGMType type);
    void StopBGM();
    void ChangeBGMVolume();
}

public enum SFXType
{
    DestroyItem,
    Paint,
    GameClear,
    GameOver,
    PlayerMove,
    PlayerInteract,
    Test
}

public enum BGMType
{

}