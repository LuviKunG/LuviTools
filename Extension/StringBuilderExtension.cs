namespace System.Text
{
    public static class StringBuilderExtension
    {
        public static void Clear(this StringBuilder builder)
        {
            builder.Length = 0;
        }
    }
}