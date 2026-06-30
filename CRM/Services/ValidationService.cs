using System.Text.RegularExpressions;
using System.Windows;

namespace CRM;

public static class ValidationService
{
    public static bool IsEnglish(string text)
    {
        return Regex.IsMatch(text, @"^[A-Za-z0-9]+$");
    }

    public static bool IsEnglishText(string text)
    {
        return Regex.IsMatch(text, @"^[\x20-\x7E]+$");
    }

    public static bool ValidateEnglishText(string text, string fieldName, bool allowEmpty = false)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            if (allowEmpty) return true;
            MessageBox.Show($"{fieldName} cannot be empty", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!IsEnglishText(text))
        {
            MessageBox.Show($"{fieldName} must contain only English characters", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    public static bool ValidateLoginPassword(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(
                "The password or login is empty",
                "Empty",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return false;
        }

        if (login.Length <= 3 || password.Length <= 3)
        {
            MessageBox.Show(
                "The password or login is too short",
                "Short",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return false;
        }

        if (!IsEnglish(login) || !IsEnglish(password))
        {
            MessageBox.Show(
                "Login and password must contain only English letters and numbers",
                "Invalid characters",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return false;
        }

        return true;
    }

    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return true;

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            MessageBox.Show("Invalid email format", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    public static bool ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return true;

        string digitsOnly = Regex.Replace(phone, @"[\s\-\+\(\)]", "");

        if (!Regex.IsMatch(digitsOnly, @"^\d{7,15}$"))
        {
            MessageBox.Show("Phone must contain 7-15 digits", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }
}