namespace Betinho.Rifa.Entities
{
    public class ActionEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalQuotas { get; set; }
        public decimal QuotaValue { get; set; }
        public double MinimumQuotas { get; set; }
        public int MaxQuotaAllowed { get; set; }
        public List<ItemEntity> Items { get; set; }
        public bool Featured { get; set; }

    }

    public class ItemEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
    }
}
