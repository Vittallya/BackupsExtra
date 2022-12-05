using System.IO;

namespace Backups.FileSystem
{
    public static class VirtualObjectExtensions
    {
        public static string GetAbsoluteSpecialPath(this IVirtualObject obj)
        {
            string path = obj.AbsoluteName;

            if (obj is IVirtualDirectory)
                path += "\\";
            return path;
        }

        public static string GetRelativeSpecialPath(this IVirtualObject obj, IVirtualDirectory? relativeTo = null)
        {
            string path = obj.RelativePath;

            if (obj is IVirtualDirectory)
                path += "\\";

            if (relativeTo != null && !relativeTo.IsRoot)
                path = Path.GetRelativePath(relativeTo.RelativePath, path);

            return path;
        }

        public static string GetNameWithoutExtension(this IVirtualObject obj)
        {
            if (obj is IVirtualFile file)
                return file.NameWithoutExtension;
            return obj.Name;
        }
    }
}
