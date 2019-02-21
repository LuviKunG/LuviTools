using StringBuilder = System.Text.StringBuilder;

namespace LuviKunG.LocaleCore
{
    public class LocaleDataTH : LocaleData
    {
        public override string Get(string key)
        {
            return Adjust(base.Get(key));
        }

        private static string Adjust(string s)
        {
            /*
            ThaiFontAdjuster
            Unity3D font renderer lacks support for GPOS and GSUB. Because Thai font heavily depends on these features, rendered image looks ugly without them.

            This library gives workaround to render Thai font almost correctly. Following image shows differences between results from original Unity3D and this library. Position of tone mark, upper vowel and lower vowel would be adjusted by surrounding characters.
            https://github.com/SaladLab/Unity3D.ThaiFontAdjuster/blob/master/docs/UnderTheHood.md
            */
            var length = s.Length;
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var c = s[i];
                if (IsTop(c) && i > 0)
                {
                    var b = s[i - 1];
                    if (IsLower(b) && i > 1)
                        b = s[i - 2];
                    if (IsBase(b))
                    {
                        var followingNikhahit = (i < length - 1 && (s[i + 1] == '\x0E33' || s[i + 1] == '\x0E4D'));
                        if (IsBaseAsc(b))
                        {
                            if (followingNikhahit)
                            {
                                c = (char)(c + ('\xF713' - '\x0E48'));
                                sb.Append('\xF711');
                                sb.Append(c);
                                if (s[i + 1] == '\x0E33')
                                    sb.Append('\x0E32');
                                i += 1;
                                continue;
                            }
                            else
                            {
                                c = (char)(c + ('\xF705' - '\x0E48'));
                            }
                        }
                        else
                        {
                            if (followingNikhahit == false)
                                c = (char)(c + ('\xF70A' - '\x0E48'));
                        }
                    }
                    if (i > 1 && IsUpper(s[i - 1]) && IsBaseAsc(s[i - 2]))
                    {
                        c = (char)(c + ('\xF713' - '\x0E48'));
                    }
                }
                else if (IsUpper(c) && i > 0 && IsBaseAsc(s[i - 1]))
                {
                    switch (c)
                    {
                        case '\x0E31': c = '\xF710'; break;
                        case '\x0E34': c = '\xF701'; break;
                        case '\x0E35': c = '\xF702'; break;
                        case '\x0E36': c = '\xF703'; break;
                        case '\x0E37': c = '\xF704'; break;
                        case '\x0E4D': c = '\xF711'; break;
                        case '\x0E47': c = '\xF712'; break;
                    }
                }
                else if (IsLower(c) && i > 0 && IsBaseDesc(s[i - 1]))
                {
                    c = (char)(c + ('\xF718' - '\x0E38'));
                }
                else if (c == '\x0E0D' && i < length - 1 && IsLower(s[i + 1]))
                {
                    c = '\xF70F';
                }
                else if (c == '\x0E10' && i < length - 1 && IsLower(s[i + 1]))
                {
                    c = '\xF700';
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static bool IsBase(char c)
        {
            return (c >= '\x0E01' && c <= '\x0E2F') || c == '\x0E30' || c == '\x0E40' || c == '\x0E41';
        }

        private static bool IsBaseDesc(char c)
        {
            return c == '\x0E0E' || c == '\x0E0F';
        }

        private static bool IsBaseAsc(char c)
        {
            return c == '\x0E1B' || c == '\x0E1D' || c == '\x0E1F' || c == '\x0E2C';
        }

        private static bool IsTop(char c)
        {
            return c >= '\x0E48' && c <= '\x0E4C';
        }

        private static bool IsLower(char c)
        {
            return c >= '\x0E38' && c <= '\x0E3A';
        }

        private static bool IsUpper(char c)
        {
            return c == '\x0E31' || c == '\x0E34' || c == '\x0E35' || c == '\x0E36' ||
                   c == '\x0E37' || c == '\x0E47' || c == '\x0E4D';
        }
    }
}