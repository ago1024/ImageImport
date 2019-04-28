using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageImport
{
    class IconLoader
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern UInt32 PrivateExtractIcons(String lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, IntPtr[] piconid, UInt32 nIcons, UInt32 flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);


        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryFlags dwFlags);

        private enum LoadLibraryFlags : uint
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }


        public static Icon GetIcon(string strPath, int index, bool bSmall)
        {
            IntPtr instance = LoadLibraryEx(strPath, IntPtr.Zero, LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
            IntPtr handle = LoadIcon(instance, "#701");
            try
            {
                return (Icon) Icon.FromHandle(handle).Clone();

            } finally
            {
                if (handle != IntPtr.Zero)
                    DestroyIcon(handle);
            }
        }



        public static Icon GetIcon2(string strPath, int index, bool bSmall)
        {
            {
                uint readIconCount = 0;
                IntPtr[] hDummy = new IntPtr[1] { IntPtr.Zero };
                IntPtr[] hIconEx = new IntPtr[1] { IntPtr.Zero };

                try
                {
                    if (!bSmall)
                        readIconCount = ExtractIconEx(strPath, 0, hIconEx, hDummy, 1);
                    else
                        readIconCount = ExtractIconEx(strPath, 0, hDummy, hIconEx, 1);

                    if (readIconCount > 0 && hIconEx[0] != IntPtr.Zero)
                    {
                        // GET FIRST EXTRACTED ICON
                        Icon extractedIcon = (Icon)Icon.FromHandle(hIconEx[0]).Clone();

                        return extractedIcon;
                    }
                    else // NO ICONS READ
                        return null;
                }
                catch (Exception ex)
                {
                    /* EXTRACT ICON ERROR */

                    // BUBBLE UP
                    throw new ApplicationException("Could not extract icon", ex);
                }
                finally
                {
                    // RELEASE RESOURCES
                    foreach (IntPtr ptr in hIconEx)
                        if (ptr != IntPtr.Zero)
                            DestroyIcon(ptr);

                    foreach (IntPtr ptr in hDummy)
                        if (ptr != IntPtr.Zero)
                            DestroyIcon(ptr);
                }
            }
        }
    }
}
