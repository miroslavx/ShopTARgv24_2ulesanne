using System.Runtime.InteropServices;

namespace ShopTARgv24.Core.Dto
{
    public class AccuCityCodeRootDto
    {
        public string? Version { get; set; }
        public string? Key { get; set; }
        public string? Type { get; set; }
        public string? LocalizedName { get; set; }
        public string? EnglishName { get; set; }
        public string? PrimaryPostalCode { get; set; }

        public RegionDto? Region { get; set; }

        public CountryDto? Country { get; set; }
        public AdministrativeAreaDto? AdministrativeArea { get; set; }
        public TimeZoneDto? TimeZone { get; set; }
        public GeoPositionDto? GeoPosition { get; set; }
        public bool? IsAlias { get; set; }
        public List<SupplementalAdminAreaDto>? SupplementalAdminAreas { get; set; }
        public List<string>? DataSets { get; set; }
         

        public class RegionDto
        {
            public string? ID { get; set; }
            public string? LocalizedName { get; set; }
            public string? EnglishName { get; set; }
        }

        public class CountryDto
        {
            public string? ID { get; set; }
            public string? LocalizedName { get; set; }
            public string? EnglishName { get; set; }
        }

        public class AdministrativeAreaDto
        {
            public string? ID { get; set; }
            public string? LocalizedName { get; set; }
            public string? EnglishName { get; set; }
            public int? Level { get; set; }
            public string? LocalizedType { get; set; }
            public string? EnglishType { get; set; }
            public string? CountryID { get; set; }
        }

        public class TimeZoneDto
        {
            public string? Code { get; set; }
            public string? Name { get; set; }
            public double? GmtOffset { get; set; }
            public bool? IsDaylightSaving { get; set; }
            public string? NextOffsetChange { get; set; }
        }

        public class GeoPositionDto
        {
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public ElevationDto? Elevation { get; set; }
        }

        public class ElevationDto
        {
            public MetricDto? Metric { get; set; }
            public ImperialDto? Imperial { get; set; }
        }

        public class MetricDto
        {
            public double? Value { get; set; }
            public string? Unit { get; set; }
            public int? UnitType { get; set; }
        }

        public class ImperialDto
        {
            public double? Value { get; set; }
            public string? Unit { get; set; }
            public int? UnitType { get; set; }
        }
        public class SupplementalAdminAreaDto
        {
            public int? Level { get; set; }
            public string? LocalizedName { get; set; }
            public string? EnglishName { get; set; }
        }
    }
}
