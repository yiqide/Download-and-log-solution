using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace dow
{
    /*
    /// 注意 这个序列化不支持字段，范型，接口
    /// 支持多语言
    /// 不需要使用额外的特性
    ///
    ///序列化行为
    ///默认情况下，所有公共属性都会序列化。 可以指定要忽略的属性。
    ///默认编码器会转义非 ASCII 字符、ASCII 范围内的 HTML 敏感字符以及根据 RFC 8259 JSON 规范必须进行转义的字符。
    ///默认情况下，JSON 会缩小。 可以对 JSON 进行优质打印。
    ///默认情况下，JSON 名称的大小写与 .NET 名称匹配。 可以自定义 JSON 名称大小写。
    ///检测到循环引用并引发异常。
    ///将忽略字段。
    ///支持的类型包括：
    ///映射到 JavaScript 基元的 .NET 基元，如数值类型、字符串和布尔。
    ///用户定义的普通旧 CLR 对象 (POCO)。
    ///一维和交错数组(ArrayName[][])。
    ///Dictionary<string, TValue> 其中 TValue 是 object、JsonElement 或 POCO。
    ///以下命名空间中的集合。
    ///System.Collections
    ///System.Collections.Generic
    ///System.Collections.Immutable
    ///System.Collections.Concurrent
    ///System.Collections.Specialized
    ///System.Collections.ObjectModel
    */
    /// <summary>
    ///默认情况下，属性名称匹配区分大小写。 可以指定不区分大小写
    ///ASP.NET Core 应用默认指定不区分大小写。
    ///如果 JSON 包含只读属性的值，则会忽略该值，并且不引发异常。
    ///无参数构造函数（可以是公共的、内部的或专用的）用于反序列化。
    ///不支持反序列化为不可变对象或只读属性。
    ///默认情况下，支持将枚举作为数字。 可以将枚举名称序列化为字符串。
    ///不支持字段。
    ///默认情况下，JSON 中的注释或尾随逗号会引发异常。 可以允许注释和尾随逗号。
    ///默认最大深度为 64。
    /// </summary>
    public static class Jons
        {

            public static string ToJsonString<T>(T json)
            {
                //设置选项
                var option = new JsonSerializerOptions
                {
                    //设置打印出来的格式
                    WriteIndented = true,
                    //要解析的其他字符
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic),
                    //序列化枚举名称(而不是数值) 区分大小写
                    Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
                };

                byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(json, option);

                return Encoding.UTF8.GetString(jsonUtf8Bytes);
            }
            public static byte[] ToJsonToByte<T>(T json)
            {
                //设置选项
                var option = new JsonSerializerOptions
                {
                    //设置打印出来的格式
                    WriteIndented = true,
                    //要解析的其他字符
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic),
                    //序列化枚举名称(而不是数值) 区分大小写
                    Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
                };
                byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(json, option);
                return jsonUtf8Bytes;
            }
            public static T JsonTo<T>(byte[] data)
            {
                //设置选项
                var option = new JsonSerializerOptions
                {
                    //设置打印出来的格式
                    WriteIndented = true,
                    //要解析的其他字符
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic),
                    //序列化枚举名称(而不是数值) 区分大小写
                    Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
                };
                var utf8Reader = new Utf8JsonReader(data);
                T t = JsonSerializer.Deserialize<T>(ref utf8Reader, option);
                return t;
            }
        } 
}
