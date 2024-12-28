using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace YazilimAcademy.ABPRaffleApp.Domain.Shared;

public sealed record FullName
{
    // Yeni desen: Unicode harf (\p{L}) + diakritik (\p{M}), 
    // ayrıca isteğe bağlı olarak `'` ve `-` işaretlerini de destekliyoruz.
    // Örneğin "O'Connor", "Ahmet-Emre", vs. kabul etsin diye...
    private const string Pattern = @"^[\p{L}\p{M}'-]+$";
    private const int MinLength = 2;
    private const int MaxLength = 100;

    public string FirstName { get; init; }
    public IReadOnlyList<string> MiddleNames { get; init; } = Array.Empty<string>();
    public string LastName { get; init; }

    private FullName(string firstName, IEnumerable<string> middleNames, string lastName)
    {
        // Her bir kısım (FirstName, MiddleNames, LastName) regex + uzunluk kontrolünden geçer
        if (!IsValid(firstName))
        {
            throw new ArgumentException($"Invalid first name: '{firstName}'.");
        }

        foreach (var middleName in middleNames)
        {
            if (!IsValid(middleName))
            {
                throw new ArgumentException($"Invalid middle name: '{middleName}'.");
            }
        }

        if (!IsValid(lastName))
        {
            throw new ArgumentException($"Invalid last name: '{lastName}'.");
        }

        FirstName = firstName;
        MiddleNames = middleNames.ToArray();
        LastName = lastName;
    }

    public static bool IsValid(string value)
    {
        // Boş veya null ise geçersiz
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Uzunluk kontrolü
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return false;
        }

        // Regex kontrolü (ignore case)
        return Regex.IsMatch(value, Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public static FullName Create(string fullNameString)
    {
        // En az 2 parça: (FirstName) + (LastName).
        // Daha fazlası varsa middle name sayılıyor.
        var parts = fullNameString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            throw new ArgumentException("Invalid full name format. Expected at least 'FirstName LastName'.");
        }

        var firstName = parts[0];
        var lastName = parts[^1];
        var middleNames = parts.Length > 2 ? parts[1..^1] : Array.Empty<string>();

        return new FullName(firstName, middleNames, lastName);
    }

    public static implicit operator string(FullName fullName)
    {
        return fullName.ToString();
    }

    public static implicit operator FullName(string value)
    {
        return Create(value);
    }

    public override string ToString()
    {
        if (MiddleNames.Count == 0)
        {
            return $"{FirstName} {LastName}";
        }
        else
        {
            return $"{FirstName} {string.Join(" ", MiddleNames)} {LastName}";
        }
    }

    public string GetInitials()
    {
        // Örneğin "Ahmet Ayyildiz" => A.A.
        // "Ali Mehmet Yıldız" => A.M.Y.
        var initials = new List<char> { FirstName[0] };

        foreach (var m in MiddleNames)
        {
            initials.Add(m[0]);
        }

        initials.Add(LastName[0]);

        return string.Join('.', initials) + ".";
    }
}
