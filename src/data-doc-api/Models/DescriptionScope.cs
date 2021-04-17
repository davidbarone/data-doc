/// <summary>
/// The scoping level of the description configuration
/// </summary>
public enum DescriptionScope
{
    /// <summary>
    /// The description configuration is not defined for the attribute
    /// </summary>
    Undefined,
    /// <summary>
    /// The description configuration applies only to the current attribute
    /// </summary>
    Local,
    /// <summary>
    /// The description configuration applies to all attributes sharing the same attribute name within the project
    /// </summary>
    Project,
    /// <summary>
    /// The description configuration applies to all attributes sharing the same attribute name in all projects
    /// </summary>
    Global
}