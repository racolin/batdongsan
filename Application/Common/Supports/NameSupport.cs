using Domain.Constants;

namespace Application.Common.Supports
{
    public static class NameSupport
    {
        public static string? GetNewsTypeName(string type)
        {
            switch (type) {
                case NewsTypeConstant.Market: return "Tin thị trường";
                case NewsTypeConstant.Project: return "Tin dự án";
                default: return null;
            }
        }
        public static string? GetProjectTypeName(string type)
        {
            switch (type) {
                case ProjectTypeConstant.Apartment: return "Căn hộ";
                case ProjectTypeConstant.Ground: return "Đất nền";
                case ProjectTypeConstant.ResortRealEstate: return "Bất động sản nghỉ dưỡng";
                case ProjectTypeConstant.Villa: return "Biệt thự";
                default: return null;
            }
        }
        public static string? GetProjectStateName(string type)
        {
            switch (type) {
                case ProjectStateConstant.Implementing: return "Dự án đang triển khai";
                case ProjectStateConstant.Implemented: return "Dự án đã triển khai";
                default: return null;
            }
        }
        public static string? GetStatusName(string type)
        {
            switch (type) {
                case StatusConstant.Draft: return "Nháp";
                case StatusConstant.Active: return "Hoạt động";
                case StatusConstant.InActive: return "Ngừng";
                case StatusConstant.Removed: return "Đã xóa";
                default: return null;
            }
        }
        public static string? GetRegisterMailStateName(string type)
        {
            switch (type) {
                case RegisterMailStateConstant.Sent: return "Đợi";
                case RegisterMailStateConstant.Added: return "Đã đăng ký";
                case RegisterMailStateConstant.Expired: return "Hết hạn";
                default: return null;
            }
        }
        public static string? GetContactStateName(string type)
        {
            switch (type) {
                case ContactStateConstant.Sent: return "Đợi";
                case ContactStateConstant.Processed: return "Đã xử lý";
                case ContactStateConstant.Refused: return "Đã từ chối";
                case ContactStateConstant.Removed: return "Đã xóa";
                default: return null;
            }
        }
        public static string? GetRoleName(string type)
        {
            switch (type) {
                case RoleConstant.Admin: return "Quản lý";
                case RoleConstant.NewsPoster: return "Người đăng bài";
                case RoleConstant.ProjectPoster: return "Người đăng dự án";
                case RoleConstant.Caller: return "Liên hệ khách";
                case RoleConstant.RegisteredEmailChecker: return "Kiểm tra mail";
                case RoleConstant.ImagePoster: return "Đăng ảnh";
                case RoleConstant.ContentEditor: return "Chỉnh sửa web";
                case RoleConstant.Newbie: return "Người mới";
                default: return null;
            }
        }
    }
}
