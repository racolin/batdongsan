namespace Domain.Constants;
public record RegexConstant
{
    public const string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";
    public const string Phone = @"^[0-9]{10,11}$";
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public const string Number = @"^\d*$";
}