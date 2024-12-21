using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace YazilimAcademy.ABPRaffleApp.Domain.Shared;

public sealed record FullName
{
    private const string Pattern = @"^[a-zA-Z]+$";
    private const int MinLength = 2;
    private const int MaxLength = 100;
    private object value;

    public string FirstName { get; init; }
    public IReadOnlyList<string> MiddleNames { get; init; } = Array.Empty<string>();
    public string LastName { get; init; }

    private FullName(string firstName, IEnumerable<string> middleNames, string lastName)
    {
        // Validate each part
        if (!IsValid(firstName))
            throw new ArgumentException("Invalid first name.");

        if (!IsValid(lastName))
            throw new ArgumentException("Invalid last name.");

        foreach (var middleName in middleNames)
        {
            if (!IsValid(middleName))
                throw new ArgumentException($"Invalid middle name '{middleName}'.");
        }

        FirstName = firstName;
        MiddleNames = middleNames.ToArray();
        LastName = lastName;
    }

    public static bool IsValid(string value)
    {
        return Regex.IsMatch(value, Pattern) && value.Length >= MinLength && value.Length <= MaxLength;
    }

    public static FullName Create(string fullNameString)
    {
        var parts = fullNameString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new ArgumentException("Invalid full name format. Expected at least 'FirstName LastName'.");

        // First name: first part
        var firstName = parts[0];

        // Last name: last part
        var lastName = parts[^1];

        // Middle names: all parts in between (if any)
        var middleNames = parts.Length > 2 ? parts[1..^1] : Array.Empty<string>();

        return new FullName(firstName, middleNames, lastName);
    }

    public static implicit operator string(FullName fullName) => fullName.ToString();
    public static implicit operator FullName(string value) => Create(value);

    public override string ToString() => MiddleNames.Count == 0
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {string.Join(" ", MiddleNames)} {LastName}";

    public string GetInitials()
    {
        // Just use the first letter of first, middle(s), and last
        var initials = new List<char> { FirstName[0] };

        foreach (var m in MiddleNames)
        {
            initials.Add(m[0]);
        }

        initials.Add(LastName[0]);

        return string.Join('.', initials) + ".";
    }
}
