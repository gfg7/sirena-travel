using System;
namespace sirena_travel.Services;

public static class EnvironmentUtil {
    internal static string Get(string variableName) {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (value is null){
            throw new KeyNotFoundException($"Environment variable '{variableName}' is not set!");
        }

        return value;
    }
} 