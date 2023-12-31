﻿using KoreanSecrets.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoreanSecrets.Domain.Common.Constants;

public static class ErrorMessages
{
    public const string ProductNotInStock = "Товару немає в наявності";

    public const string AddressInfoNotFound = "Адреси, яка належить до користувача не знайдено";
    public const string BrandNotFound = "Бренд не знайдено";
    public const string BannerNotFound = "Банер не знайдено";
    public const string PromoNotFound = "Промокод не знайдено";
    public const string CountryNotFound = "Країну не знайдено";
    public const string SubCatNotFound = "Підкатегорію не знайдено";
    public const string DemandNotFound = "Необхідність не знайдено";
    public const string CategoryNotFound = "Необхідність не знайдено";
    public const string SomeProductNotFound = "Товару не знайдено";

    public const string WrongPassword = "Пароль неправильний";
    public const string WrongPhoneNumber = "Номер телефону неправильний";
    public const string WrongNewPrice = "Нова ціна вказано неправильно";

    public const string FileNotFound = "Файлу не знайдено";
    public const string UserNotFound = "Користувача не знайдено";
    public static string ProductNotFound(string product) => $"{product} не знайдено";
    public static string DeleteProductsFirst(EntityErrorType type, string title) 
    {
        var message = type switch
        {
            EntityErrorType.Brand => "брендом",
            EntityErrorType.Country => "країною",
            EntityErrorType.SubCategory => "підкатегорією",
            EntityErrorType.Demand => "потребою",
            EntityErrorType.Category => "категорією",
            _ => ""
        };

        return $"Спочатку необхідно видалити всі продукти, які пов'язані з {message} {title}";
    }

    public const string UnknownError = "На сервері сталась невідома помилка, або ви вказали невірні дані, спробуйте ще раз";
    public const string UnknownVideoType = "Невідомий тип даних (дозволені типи: .mp4)";
    public const string UnknownPhotoType = "Невідомий тип даних (дозволені типи: .jpg; .jpeg; .png; ";

    public const string PhoneNumberAlreadyConfirmed = "Ви вже підтвердили свій номер телефону";
    public const string PhoneNumberIsNotConfirmed = "Спочатку підтвердьте свій номер телефону";
    public const string ContentAccessForbidden = "Доступ до контенту заборонено";
    public const string PurchaseProductNotRelatedToUser = "Ваша корзина пуста";
    public const string BucketIsEmpty = "Ваша корзина пуста";
}