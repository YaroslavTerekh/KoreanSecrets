using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Constants;

public static class ValidationMessages
{
    public const string PhoneNumberRequired = "Необхідно ввести номер телефону";
    public const string PhoneNumberTooLong = "Номер телефону задовгий";
    public const string PhoneNumberTooShort = "Номер телефону закороткий";

    public const string IdRequired = "Необхідно вказати Id";
    public const string IdWrong = "Вказане Id некоректне";

    public const string FirstNameTooShort = "Ім'я занадто коротке";
    public const string FirstNameTooLong = "Ім'я занадто довге";
    public const string FirstNameRequired = "Необхідно ввести ім'я";

    public const string LastNameTooShort = "Прізвище занадто коротке";
    public const string LastNameTooLong = "Прізвище занадто довге";
    public const string LastNameRequired = "Необхідно ввести прізвище";

    public const string WrongEmail = "Введіть коректний email";
    public const string EmailRequired = "Необхідно ввести email";

    public const string TitleRequired = "Необхідно ввести назву";
    public const string TitleTooLong = "Назва занадто довга";
    public const string TitleTooShort = "Назва занадто коротка";

    public const string DescriptionRequired = "Необхідно ввести опис";
    public const string DescriptionTooLong = "Опис занадто довгий";
    public const string DescriptionTooShort = "Опис занадто короткий";

    public const string WrongDateTime = "Вказано некоректну дату";
    public const string WrongPrice = "Вказано некоректну ціну";
}
