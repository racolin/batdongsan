using FluentValidation;

namespace Application.Common.Requests
{
    public class SetLockUserRequest
    {
        public int UserId { get; set; }
        public bool IsLock { get; set; }
    }
}