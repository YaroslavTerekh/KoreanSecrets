﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Constants;

public static class ErrorMessages
{
    public const string WrongPassword = "Пароль неправильний";
    public const string WrongPhoneNumber = "Номер телефону неправильний";

    public const string FileNotFound = "Файлу не знайдено";
    public const string UserNotFound = "Користувача не знайдено";
    public static string ProductNotFound(string product) => $"{product} не знайдено";

    public const string UnknownError = "На сервері сталась невідома помилка, або ви вказали невірні дані, спробуйте ще раз";
    public const string UnknownVideoType = "Невідомий тип даних (дозволені типи: .mp4)";
    public const string UnknownPhotoType = "Невідомий тип даних (дозволені типи: .jpg; .jpeg; .png; ";

    public const string PhoneNumberAlreadyConfirmed = "Ви вже підтвердили свій номер телефону";
    public const string PhoneNumberIsNotConfirmed = "Спочатку підтвердьте свій номер телефону";
    public const string ContentAccessForbidden = "Доступ до контенту заборонено";
}