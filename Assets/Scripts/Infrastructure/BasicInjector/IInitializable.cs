namespace BasicInjector
{
    public interface IInitializable
    {
        /// <summary>
        /// Injection 끝난 직후 호출됨
        /// </summary>
        void Initialize();
    }
}