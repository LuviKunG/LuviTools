﻿# Add-on: LocaleDataTH for Thai Font Adjuster.

This add-on are apply ThaiFontAdjuster plugins by [SaladLab](https://github.com/SaladLab/Unity3D.ThaiFontAdjuster).
To enable this, put scripting define symbols `LOCALE_DATATH` on your unity project. And done!

# ThaiFontAdjuster
## Philosophy
Unity3D font renderer lacks support for
[GPOS and GSUB](https://www.microsoft.com/typography/otspec/gpos.htm).
Because Thai font heavily depends on these features, rendered image looks ugly without them.

This library gives workaround to render Thai font almost correctly.
Following image shows differences between results from original Unity3D and this library.
Position of tone mark, upper vowel and lower vowel would be adjusted by surrounding characters.

![Features](https://raw.githubusercontent.com/SaladLab/Unity3D.ThaiFontAdjuster/master/docs/Features.png)

## Where can I get it?

Visit [Release](https://github.com/SaladLab/Unity3D.ThaiFontAdjuster/releases)
page to get latest ThaiFontAdjuster unity-package.

## How to use

Just before setting text of UnityEngine.UI.Text, text need to be translated by
`ThaiFontAdjuster.Adjust`. Font of Text should be one of supported Thai fonts.

```csharp
var s = "Thai ก์กิ์ป์ปิ์ฎุญุ";
Label.text = ThaiFontAdjuster.Adjust(s);
```

For other UI library using Unity3D dynamic font (like NGUI), you can do it by the same way.

## Limitation

#### ThaiFontAdjuster only can handle special fonts.
- Font should have extended glyphs from U+F700 to U+F71A providing various position of Thai characters.
- [NECTEC National Fonts](http://www.nectec.or.th/pub/review-software/font/national-fonts.html) (Garuda, Loma, Kinnari, Norasi) already provide it.
- Modified NotoSansThai containing extended glyphs is included in this package.
- If you want to use another font, check it contains extended glyphs, otherwise glyphs have to be added to font by yourself.

#### Adjusting position is not same as true-type rendered one with GPOS and GSUB support.
- This library adjusts position of glyph at best with limited extended set of character.
    But without GPOS, ideal positioning is impossibe.

## Under the hood

If you want to know more detail about this libary, check [UnderTheHood](./docs/UnderTheHood.md) article.
