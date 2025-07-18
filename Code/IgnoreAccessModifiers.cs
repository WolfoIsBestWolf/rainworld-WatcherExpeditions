using System;
using System.Security;
using System.Security.Permissions;
using RWCustom;

// SecurityPermision set to minimum and SkipVerification set to true
// for skipping access modifiers check from the mono JIT
// The same attributes are added to the assembly when ticking
// Unsafe Code in the Project settings
// This is done here to allow an explanation of the trick and
// not in an outside source you could potentially miss.

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

public class T
{
    public static string Translate(string s)
    {
        return Custom.rainWorld.inGameTranslator.Translate(s);
    }
    public static string TranslateLineBreak(string s)
    {
        return Custom.rainWorld.inGameTranslator.Translate(s).Replace("<LINE>", Environment.NewLine);
    }
    public static InGameTranslator IGT
    {
        get
        {
            return Custom.rainWorld.inGameTranslator;
        }
    }
}