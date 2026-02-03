namespace Formation_Ecommerce_Client.Services.Interfaces
{
    /// <summary>
    /// Contrat générique d'un service HTTP côté Client MVC : opérations CRUD exposées par l'API pour une ressource donnée.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans le monolithe, les contrôleurs MVC appellent directement des services Application.
    /// - Dans l'architecture client/serveur, le contrôleur MVC appelle un service HTTP (par ressource) qui encapsule les appels API.
    /// - Le découplage permet de remplacer l'UI (mobile/SPA) sans changer les couches métier côté serveur.
    /// </remarks>
    public interface IApiServiceBase<TDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TCreateDto dto);
        Task UpdateAsync(Guid id, TUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
