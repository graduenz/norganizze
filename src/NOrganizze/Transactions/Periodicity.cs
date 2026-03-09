namespace NOrganizze.Transactions
{
    /// <summary>Constants for recurrence/installment periodicity used by the API. Values match the Organizze application (Anual, Semestral, Trimestral, Bimestral, Mensal, Quinzenal, Semanal, Diário).</summary>
    public static class Periodicity
    {
        /// <summary>Daily (Diário).</summary>
        public const string Daily = "daily";
        /// <summary>Weekly (Semanal).</summary>
        public const string Weekly = "weekly";
        /// <summary>Biweekly / fortnightly (Quinzenal).</summary>
        public const string Biweekly = "biweekly";
        /// <summary>Monthly (Mensal).</summary>
        public const string Monthly = "monthly";
        /// <summary>Bimonthly / every two months (Bimestral).</summary>
        public const string Bimonthly = "bimonthly";
        /// <summary>Trimonthly / quarterly (Trimestral).</summary>
        public const string Trimonthly = "trimonthly";
        /// <summary>Six-monthly / semiannual (Semestral).</summary>
        public const string Sixmonthly = "sixmonthly";
        /// <summary>Yearly (Anual).</summary>
        public const string Yearly = "yearly";
    }
}
