using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InstallerGenerationInjectionAttribute : Attribute
{
    public Type BoundInjection { get; }

    public InstallerGenerationInjectionAttribute(Type boundInjection)
    {
        BoundInjection = boundInjection;
    }
}