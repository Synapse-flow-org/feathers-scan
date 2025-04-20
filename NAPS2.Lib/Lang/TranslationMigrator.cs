using System.Globalization;
using System.Resources;

namespace NAPS2.Lang;

public static class TranslationMigrator
{
    public static string PickTranslated(ResourceManager resourceManager, string originalName, string replacementName)
    {
        string englishOriginal = resourceManager.GetString(originalName, CultureInfo.GetCultureInfo("fr-FR"))!;
        string englishReplacement = resourceManager.GetString(replacementName, CultureInfo.GetCultureInfo("fr-FR"))!;
        string translatedOriginal = resourceManager.GetString(originalName)!;
        string translatedReplacement = resourceManager.GetString(replacementName)!;
        if (translatedReplacement != englishReplacement)
        {
            return translatedReplacement;
        }
        if (translatedOriginal != englishOriginal)
        {
            return translatedOriginal;
        }
        return englishReplacement;
    }
}