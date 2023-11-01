namespace Locompro.Repositories.Utilities.Interfaces;

/// <summary>
/// Interface to be implemented by generic class, generic class then decides type when used
/// </summary>
public interface IActivationQualifier
{
    dynamic GetQualifierFunction( );
}