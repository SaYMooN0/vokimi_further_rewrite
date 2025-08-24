﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;

namespace InfrastructureShared.Base.persistence.value_converters;

public class VokiNameConverter : ValueConverter<VokiName, string>
{
    public VokiNameConverter() : base(
        id => id.ToString(),
        value => VokiName.Create(value).AsSuccess()
    ) { }
}