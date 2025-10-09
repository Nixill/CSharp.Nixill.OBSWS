using System.Text.Json.Nodes;

namespace Nixill.OBSWS;

public static partial class OBSExtraRequests
{
  public static partial class Filters
  {
    public static class ColorCorrection
    {
      public static OBSVoidRequest SetColorCorrectionParameters(ID sourceID, string filterName,
        double? brightness = null, uint? colorAddABGR = null, uint? colorMultiplyABGR = null, double? contrast = null,
        double? gamma = null, double? hueShift = null, double? opacity = null, double? saturation = null)
        => OBSRequests.Filters.SetSourceFilterSettings(sourceID, filterName, new JsonObject()
          .WithValueIfNotNull("brightness", brightness)
          .WithValueIfNotNull("color_add", colorAddABGR)
          .WithValueIfNotNull("color_multiply", colorMultiplyABGR)
          .WithValueIfNotNull("contrast", contrast)
          .WithValueIfNotNull("gamma", gamma)
          .WithValueIfNotNull("hue_shift", hueShift)
          .WithValueIfNotNull("opacity", opacity)
          .WithValueIfNotNull("saturation", saturation)
        );

      public static OBSVoidRequest SetBrightness(ID sourceID, string filterName, double brightness)
        => SetColorCorrectionParameters(sourceID, filterName, brightness: brightness);

      public static OBSVoidRequest SetAddedColor(ID sourceID, string filterName, uint colorAddABGR)
        => SetColorCorrectionParameters(sourceID, filterName, colorAddABGR: colorAddABGR);

      public static OBSVoidRequest SetMultipliedColor(ID sourceID, string filterName, uint colorMultiplyABGR)
        => SetColorCorrectionParameters(sourceID, filterName, colorMultiplyABGR: colorMultiplyABGR);

      public static OBSVoidRequest SetContrast(ID sourceID, string filterName, double contrast)
        => SetColorCorrectionParameters(sourceID, filterName, contrast: contrast);

      public static OBSVoidRequest SetGamma(ID sourceID, string filterName, double gamma)
        => SetColorCorrectionParameters(sourceID, filterName, gamma: gamma);

      public static OBSVoidRequest SetHueShift(ID sourceID, string filterName, double hueShift)
        => SetColorCorrectionParameters(sourceID, filterName, hueShift: hueShift);

      public static OBSVoidRequest SetOpacity(ID sourceID, string filterName, double opacity)
        => SetColorCorrectionParameters(sourceID, filterName, opacity: opacity);

      public static OBSVoidRequest SetSaturation(ID sourceID, string filterName, double saturation)
        => SetColorCorrectionParameters(sourceID, filterName, saturation: saturation);
    }
  }
}