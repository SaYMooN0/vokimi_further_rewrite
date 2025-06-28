﻿namespace SharedKernel.domain.ids;

public class AppUserId(Guid value) : GuidBasedId(value)
{
    public static AppUserId CreateNew() => new(Guid.CreateVersion7());
}