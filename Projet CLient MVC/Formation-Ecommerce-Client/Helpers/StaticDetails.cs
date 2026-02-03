namespace Formation_Ecommerce_Client.Helpers
{
    /// <summary>
    /// Classe utilitaire côté Client MVC : centralise des constantes (statuts, rôles) utilisées par l'IHM.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans une architecture client/serveur, ces valeurs doivent rester alignées avec le serveur (API) pour filtrer/afficher correctement les états.
    /// - Même si le client peut afficher/filtrer via ces constantes, la source de vérité des règles d'accès reste l'API.
    /// </remarks>
    public static class StaticDetails
    {
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string Status_Approved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Pending = "Pending";
        public const string Status_Completed = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string Status_Cancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
        public const string Status_Refunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";

        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
        public const string Role_Employee = "Employee";
    }
}
