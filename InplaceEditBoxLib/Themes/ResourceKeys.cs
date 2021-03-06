namespace InplaceEditBoxLib.Themes
{
    using System.Windows;

    /// <summary>
    /// Class member act as a reference for resource dictionary based definitions.
    /// </summary>
    public static class ResourceKeys
    {
        #region Normal Control Foreground and Background Keys
        /// <summary>
        /// Цветовой ключ для нормального переднего плана
        /// </summary>
        public static readonly ComponentResourceKey NormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "NormalForegroundKey");

        /// <summary>
        /// Кисть для нормального переднего плана
        /// </summary>
        public static readonly ComponentResourceKey NormalForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "NormalForegroundBrushKey");
        #endregion Normal Control Foreground and Background Keys
    }
}
