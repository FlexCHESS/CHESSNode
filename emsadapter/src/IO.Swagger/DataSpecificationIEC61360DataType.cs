namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataSpecificationIEC61360DataType
    {
        [EnumMember(Value = "DATE"), ]
        DATE,
        [EnumMember(Value = "STRING"), ]
        STRING,
        [EnumMember(Value = "STRING_TRANSLATABLE"), Display(Name = "String translatable")]
        STRING_TRANSLATABLE,
        [EnumMember(Value = "INTEGER_MEASURE"), Display(Name = "Integer Measure")]
        INTEGER_MEASURE,
        [EnumMember(Value = "INTEGER_COUNT"), Display(Name = "Integer Count")]
        INTEGER_COUNT,
        [EnumMember(Value = "INTEGER_CURRENCY"), Display(Name = "Integer Currency")]
        INTEGER_CURRENCY,
        [EnumMember(Value = "REAL_MEASURE"), Display(Name = "Real Measure")]
        REAL_MEASURE,
        [EnumMember(Value = "REAL_COUNT"), Display(Name = "Real Count")]
        REAL_COUNT,
        [EnumMember(Value = "REAL_CURRENCY"), Display(Name = "Real Currency")]
        REAL_CURRENCY,
        [EnumMember(Value = "BOOLEAN"), ]
        BOOLEAN,
        [EnumMember(Value = "IRI"), ]
        IRI,
        [EnumMember(Value = "IRDI"), ]
        IRDI,
        [EnumMember(Value = "RATIONAL"), ]
        RATIONAL,
        [EnumMember(Value = "RATIONAL_MEASURE"), Display(Name = "Rational Measure")]
        RATIONAL_MEASURE,
        [EnumMember(Value = "TIME"), Display(Name = "Time")]
        TIME,
        [EnumMember(Value = "TIMESTAMP"), Display(Name = "Timestamp")]
        TIMESTAMP,
        [EnumMember(Value = "HTML"), ]
        HTML,
        [EnumMember(Value = "BLOB"), ]
        BLOB,
        [EnumMember(Value = "FILE"), ]
        FILE
    }
}