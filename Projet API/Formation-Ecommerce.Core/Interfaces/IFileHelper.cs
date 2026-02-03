using Microsoft.AspNetCore.Http;

namespace Formation_Ecommerce_11_2025.Core.Interfaces
{
    // Interface pour la gestion des fichiers(upload/suppression d'images)
    public interface IFileHelper
    {
        // Télécharge un fichier vers le serveur et retourne son chemin (ou null si échec)
        public string? UploadFile(IFormFile file, string folder);

        // Supprime un fichier du serveur (retourne true si succès)
        public bool DeleteFile(string Path, string folder);
    }
}
