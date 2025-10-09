namespace Nixill.OBSWS.Utils;

public static class ColorConversions
{
  public static uint FromColor(System.Drawing.Color color) => ((uint)color.R) << 0 | ((uint)color.G) << 8 | ((uint)color.B) << 16 | ((uint)color.A) << 24;

  static uint HexToUint(string hex, int expectedBytes)
  {
    if (hex.StartsWith('#')) hex = hex[1..];
    if (hex.Length != expectedBytes * 2) throw new FormatException($"The string '{hex}' was not the expected length of {expectedBytes} bytes.");
    return Convert.ToUInt32(hex.StartsWith('#') ? hex[1..] : hex, 16);
  }

  static string UintToHex(uint argb, int bytes)
  => argb.ToString("x8")[^(bytes * 2)..];

  // This returns the input, but could be handy for ensuring disambiguation.
  public static uint FromABGR(uint abgr) => abgr;
  public static uint FromABGR(string abgr) => HexToUint(abgr, 4);

  public static uint FromARGB(uint argb) => (argb & 0xFF00FF00) | ((argb & 0x00FF0000) >> 16) | ((argb & 0x000000FF) << 16);
  public static uint FromARGB(string argb) => FromARGB(HexToUint(argb, 4));

  public static uint FromRGBA(uint rgba) => ((rgba & 0xFF000000) >> 24) | ((rgba & 0x00FF0000) >> 8) | ((rgba & 0x0000FF00) << 8) | ((rgba & 0x000000FF) << 24);
  public static uint FromRGBA(string rgba) => FromRGBA(HexToUint(rgba, 4));

  public static uint FromRGB(uint rgb) => 0xFF000000 | ((rgb & 0xFF0000) >> 16) | (rgb & 0x00FF00) | ((rgb & 0x0000FF) << 16);
  public static uint FromRGB(string rgb) => FromRGB(HexToUint(rgb, 3));

  public static uint FromBGR(uint bgr) => 0xFF000000 | (bgr & 0xFFFFFF);
  public static uint FromBGR(string bgr) => FromBGR(HexToUint(bgr, 3));

  public static uint ToABGR(uint abgr) => abgr;
  public static string ToABGRHex(uint abgr) => UintToHex(abgr, 4);

  public static uint ToARGB(uint abgr) => (abgr & 0xFF00FF00) | ((abgr & 0x00FF0000) >> 16) | ((abgr & 0x000000FF) << 16);
  public static string ToARGBHex(uint abgr) => UintToHex(ToARGB(abgr), 4);

  public static uint ToRGBA(uint abgr) => ((abgr & 0xFF000000) >> 24) | ((abgr & 0x00FF0000) >> 8) | ((abgr & 0x0000FF00) << 8) | ((abgr & 0x000000FF) << 24);
  public static string ToRGBAHex(uint abgr) => UintToHex(ToRGBA(abgr), 4);

  public static uint ToRGB(uint abgr) => ((abgr & 0xFF0000) >> 16) | (abgr & 0x00FF00) | ((abgr & 0x0000FF) << 16);
  public static string ToRGBHex(uint abgr) => UintToHex(ToRGB(abgr), 3);

  public static uint ToBGR(uint abgr) => abgr & 0xFFFFFF;
  public static string ToBGRHex(uint abgr) => UintToHex(ToBGR(abgr), 3);
}