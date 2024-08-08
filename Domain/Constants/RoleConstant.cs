using Domain.Common;

namespace Domain.Constants;
public record RoleConstant : IObtainableProperties
{
    /// <summary>
    /// Admin has all permission, is capable of granting and invoking roles for any users
    /// </summary>
    public const string Admin = "Admin";
    /// <summary>
    /// NewsPoster is capable of creating, updating, hiding news
    /// </summary>
    public const string NewsPoster = "NewsPoster";
    /// <summary>
    /// ProjectPoster is capable of creating, updating, hiding projects
    /// </summary>
    public const string ProjectPoster = "ProjectPoster";
    /// <summary>
    /// Caller is capable of updating contacts
    /// </summary>
    public const string Caller = "Caller";
    /// <summary>
    /// RegisteredEmailChecker is capable of updating, hiding registered email
    /// </summary>
    public const string RegisteredEmailChecker = "RegisteredEmailChecker";
    /// <summary>
    /// ImagePoster is capable of creating, updating images
    /// </summary>
    public const string ImagePoster = "ImagePoster";
    /// <summary>
    /// ContentEditor is capable of updating web content 
    /// </summary>
    public const string ContentEditor = "ContentEditor";
    /// <summary>
    /// NewMember will be granted permission by Admin soon
    /// </summary>
    public const string Newbie = "Newbie";
    public static List<string> GetAllProperties()
    {
        return new List<string> { Admin, NewsPoster, ProjectPoster, Caller, RegisteredEmailChecker, ImagePoster, ContentEditor, Newbie };
    }

    public const int MaxLoginFail = 5;
}