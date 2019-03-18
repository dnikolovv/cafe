namespace Cafe.Domain
{
    /// <summary>
    /// Used to represent a side effect.
    /// </summary>
    public readonly struct Unit
    {
        public static Unit Value => new Unit();
    }
}
