using Formation_Ecommerce_11_2025.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Formation_Ecommerce_11_2025.Test.Fakes;

/// <summary>
/// Fake de <see cref="IFileHelper"/> pour tester sans écriture disque.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Éviter les opérations de fichiers réelles pendant les tests.
/// - Tracker les appels pour vérifier le comportement du service.
/// </remarks>
public class FakeFileHelper : IFileHelper
{
    /// <summary>
    /// Valeur à retourner lors de l'upload (simule le nom du fichier uploadé).
    /// </summary>
    public string? UploadReturnValue { get; set; }

    /// <summary>
    /// Nombre d'appels à la méthode UploadFile.
    /// </summary>
    public int UploadCallCount { get; private set; }

    /// <summary>
    /// Nombre d'appels à la méthode DeleteFile.
    /// </summary>
    public int DeleteCallCount { get; private set; }

    /// <summary>
    /// Dernier fichier uploadé.
    /// </summary>
    public IFormFile? LastUploadedFile { get; private set; }

    /// <summary>
    /// Dernier chemin de fichier supprimé.
    /// </summary>
    public string? LastDeletedPath { get; private set; }

    public string? UploadFile(IFormFile file, string folder)
    {
        UploadCallCount++;
        LastUploadedFile = file;
        return UploadReturnValue ?? $"{folder}/{file.FileName}";
    }

    public bool DeleteFile(string path, string folder)
    {
        DeleteCallCount++;
        LastDeletedPath = path;
        return true;
    }

    /// <summary>
    /// Réinitialise les compteurs pour les tests.
    /// </summary>
    public void Reset()
    {
        UploadCallCount = 0;
        DeleteCallCount = 0;
        LastUploadedFile = null;
        LastDeletedPath = null;
        UploadReturnValue = null;
    }
}
