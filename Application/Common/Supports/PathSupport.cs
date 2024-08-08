using Domain.Enums;

namespace Application.Common.Supports
{
    public static class PathSupport
    {
        public static string GetUploadImagePath(string name)
        {
            return GetPath("/upload/images/", name);
        }
        public static string GetUploadThumbDefaultPath(string name, int type)
        {
            if (type == (int)ImageTypeEnum.OnlyFull)
            {
                return GetPath("/upload/images/", name); ;
            }
            return GetPath("/upload/thumbs/", name);
        }
        public static string GetPath(string folder, string name)
        {
            return Path.Combine(folder, name);
        }
    }
}
