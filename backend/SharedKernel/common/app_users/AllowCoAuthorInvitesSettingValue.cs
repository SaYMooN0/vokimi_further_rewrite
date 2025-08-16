﻿namespace SharedKernel.common.app_users;

public enum AllowCoAuthorInvitesSettingValue
{
    Everyone,
    OnlyFriends,
    NoOne
}

public static class AllowCoAuthorInvitesSettingValueExtensions
{
    public static AllowCoAuthorInvitesSettingValue Default =>
        AllowCoAuthorInvitesSettingValue.Everyone;
}