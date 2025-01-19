﻿using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;
using NuGet.Packaging;

namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services.Servants
{
    public interface INuspecReaderFactory
    {
        Task<IReadOnlyCollection<NuspecReader>> CreateAllAsync(IReadOnlyCollection<PackageIdentifier> packages);
    }
}