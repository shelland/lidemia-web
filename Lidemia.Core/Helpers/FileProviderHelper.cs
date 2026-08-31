// Created on 25/12/2021 22:51 by shell

using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Lidemia.Core.Helpers;

public class FileProviderHelper
{
    public static string ReadEmbedded(Assembly asm, string fileName)
    {
        var provider = new EmbeddedFileProvider(asm);

        using var stream = provider.GetFileInfo(fileName).CreateReadStream();
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    public static string[] GetEmbeddedFiles(Assembly asm)
    {
        // var files = System.Reflection.Assembly.Load("CoffeePlan.DataAccess").GetManifestResourceNames();
        return asm.GetManifestResourceNames();
    }
}