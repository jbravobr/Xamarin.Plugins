using Plugin.MobileFirst.Abstractions;
using System;

namespace Plugin.MobileFirst
{
  /// <summary>
  /// Cross platform MobileFirst implemenations
  /// </summary>
  public class CrossMobileFirst
  {
    static Lazy<IMobileFirst> Implementation = new Lazy<IMobileFirst>(() => CreateMobileFirst(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IMobileFirst Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IMobileFirst CreateMobileFirst()
    {
#if PORTABLE
        return null;
#else
        return new MobileFirstImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
