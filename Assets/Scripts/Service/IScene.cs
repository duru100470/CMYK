public interface IScene
{
    void Load(object param = null);
    void Unload();
    SceneScope SceneScope { get; }
}